using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public static bool joueurInput = false;

    public float tempsAerienne = 0;

    public float vitesseJoueur = 1f;
    public float vitesseCamera = 2.5f;
    private float xInput;
    private float zInput;

    private CharacterController characterController;

    private bool murOuvert = false;
    private static bool vueAerien = false;
    private static bool objetsVisble = false;

    public GameObject objetsPlacer;

    public Camera cameraJoueur;
    public Camera cameraAerien;

    public AudioClip ouvertureMurAudio;
    private AudioSource audioSource;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        transform.Rotate(0, 90, 0);

        cameraJoueur.enabled = true;
        cameraAerien.enabled = false;

        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        bool ctrlShiftSpace =
            (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) &&
            (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) &&
            Input.GetKeyDown(KeyCode.Space);

        if (ctrlShiftSpace)
        {
            vueAerien = !vueAerien;

            cameraJoueur.enabled = !vueAerien;
            cameraAerien.enabled = vueAerien;

            objetsVisble = vueAerien;
        }

        if (vueAerien)
        {
            if (GameManager.score < 10)
            {
                cameraJoueur.enabled = true;
                cameraAerien.enabled = false;
                vueAerien = false;
                objetsVisble = true;
            }

            tempsAerienne += Time.deltaTime;
            if (tempsAerienne >= 1)
            {
                tempsAerienne = 0;
                GameManager.score -= 10;
            }
        }

        if (Input.anyKey && !Input.GetKey(KeyCode.Alpha2)) joueurInput = true;

        xInput = Input.GetAxis("Horizontal");
        zInput = Input.GetAxis("Vertical");

        if (!vueAerien)
        {
            transform.Rotate(new Vector3(0, 1, 0) * xInput * vitesseCamera * Time.deltaTime);
            characterController.Move(transform.forward * zInput * vitesseJoueur * Time.deltaTime);
        }

        // DEBUG
        if (Input.GetKeyDown(KeyCode.Alpha1) == true) GameManager.NextLevel();
        if (Input.GetKeyDown(KeyCode.Alpha2) == true) GameManager.RestartLevel();
        // DEBUG

        if (Input.GetKeyDown(KeyCode.Space) == true && !ctrlShiftSpace && !murOuvert && GameManager.nombreOuvreur > 0 && GameManager.score >= 50)
        {
            murOuvert = true;
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1f))
            {
                GameObject murOuvrable = hit.collider.gameObject.transform.parent.gameObject;
                if (murOuvrable.tag == "Ouvrable")
                {
                    murOuvrable.tag = "Ouvrant";
                    GameManager.nombreOuvreur--;
                    GameManager.score -= 50;
                    JouerSonMur();
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.Space) == true) murOuvert = false;

        if (Input.GetKeyDown(KeyCode.PageUp) && !vueAerien)
        {
            cameraJoueur.enabled = false;
            cameraAerien.enabled = true;
            vueAerien = true;
            objetsVisble = false;
        }
        else if (Input.GetKeyDown(KeyCode.PageDown) && vueAerien)
        {
            cameraJoueur.enabled = true;
            cameraAerien.enabled = false;
            vueAerien = false;
            objetsVisble = true;
        }

        foreach (Transform gameObjectVisibleTransform in objetsPlacer.transform)
        {
            gameObjectVisibleTransform.gameObject.SetActive(objetsVisble);
        }

        GameObject[] murOuvrants = GameObject.FindGameObjectsWithTag("Ouvrant");
        foreach (GameObject mur in murOuvrants)
        {
            Vector3 pos = mur.transform.position;
            float nouveauY = Mathf.MoveTowards(pos.y, -3.1f, 1 * Time.deltaTime);
            mur.transform.position = new Vector3(pos.x, nouveauY, pos.z);

            if (nouveauY == -3.1f)
            {
                mur.tag = "Untagged";
            }
        }
    }

    void JouerSonMur()
    {
        audioSource.PlayOneShot(ouvertureMurAudio);
    }
}