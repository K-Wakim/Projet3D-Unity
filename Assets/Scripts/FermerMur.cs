using UnityEngine;

public class FermerMur : MonoBehaviour
{
    public GameObject joueur;
    public static bool murFerme = false;

    public AudioClip ouvertureMurAudio;
    private AudioSource audioSource;
    private bool sonJoue = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        Vector3 pos = transform.position;

        if (!murFerme && joueur != null && joueur.transform.position.x >= 3.25f)
        {
            murFerme = true;
        }
        else if (murFerme)
        {
            float nouveauY = Mathf.MoveTowards(pos.y, 0f, 1 * Time.deltaTime);
            transform.position = new Vector3(pos.x, nouveauY, pos.z);

            if (!sonJoue)
            {
                if (ouvertureMurAudio != null && audioSource != null)
                {
                    Invoke(nameof(JouerSonMur), 1.0f);
                }
                else
                {
                    Debug.LogError("Missing AudioSource or OuvertureMurAudio on " + gameObject.name);
                }

                sonJoue = true;
            }
        }
        else if (!murFerme)
        {
            transform.position = new Vector3(pos.x, -3.01f, pos.z);
        }

        if (joueur.transform.position.x >= -1f && joueur.transform.position.x <= 1f &&
            joueur.transform.position.z >= -1f && joueur.transform.position.z <= 1f
        )
        {
            murFerme = false;
        }
    }

    void JouerSonMur()
    {
        audioSource.PlayOneShot(ouvertureMurAudio);
    }
}
