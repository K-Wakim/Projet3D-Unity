using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public AudioSource audioRecommencerNiveau;



    public static GameManager gameManagerInstance;
    public Scene sceneGameOver;
    public Scene sceneWin;

    public static int niveau = 1;
    public static int score = 300;
    public static float temps = 60;

    public static int nombreOuvreur = 0;
    public static int nombreFleches = 0;
    public static int nombreTeleTransporteur = 0;
    public static int nombreTeleRecepteur = 0;




    public GameObject mursNonOuvrablesParent;
    public GameObject mursOuvrablesParent;

    public GameObject objetParentPlacer;
    public GameObject flecheGameObj;
    public GameObject tresorObj;
    public GameObject teleTransporteurObj;
    public GameObject teleRecepteurObj;

    // Ajouter animations pour les deux (restart et next)
    public static void NextLevel()
    {
        niveau++;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        temps = 60;
        PlayerController.joueurInput = false;

        score += (int)(temps * 10);

        if (niveau > 10)
        {
            SceneManager.LoadScene("WinScene");
        }
    }

    public static void RestartLevel()
    {


        gameManagerInstance.audioRecommencerNiveau.Play();


        GameObject joueur = GameObject.FindGameObjectWithTag("Player");
        CharacterController controller = joueur.GetComponent<CharacterController>();

        score -= 200;
        temps = 60;
        FermerMur.murFerme = false;
        PlayerController.joueurInput = false;

        controller.enabled = false;
        joueur.transform.position = new Vector3(0.5f, 0.5f, 0.5f);
        joueur.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
        controller.enabled = true;

        if (score < 0)
        {
            SceneManager.LoadScene("GameOver");
        }
    }

    private static List<Vector3> listeMursPosition = new List<Vector3>();
    public void Start()
    {
        gameManagerInstance = this;

        nombreOuvreur = 4 - (int)Math.Floor((niveau - 1) * 0.5);
        nombreFleches = 18 - (2 * (niveau - 1));
        nombreTeleTransporteur = (int)Math.Floor(niveau * 0.5);
        nombreTeleRecepteur = niveau - 1;


        List<Vector3> listePlacementPotentielle = new List<Vector3>();

        int grilleXMax = 15;
        int grilleXMin = -15;

        int grilleZMax = 15;
        int grilleZMin = -15;

        listeMursPosition = new List<Vector3>();

        // Enlever les tuiles potentielle dans l'enclos
        enleverTuilePotentielle(2, 0);

        enleverTuilePotentielle(1, 1);
        enleverTuilePotentielle(1, 0);
        enleverTuilePotentielle(1, -1);

        enleverTuilePotentielle(0, 1);
        enleverTuilePotentielle(0, 0);
        enleverTuilePotentielle(0, -1);

        enleverTuilePotentielle(-1, 1);
        enleverTuilePotentielle(-1, 0);
        enleverTuilePotentielle(-1, -1);

        // Enlever la possibilite de mettre des objets sur les murs non ouvrables
        foreach (Transform child in mursNonOuvrablesParent.transform)
        {
            listeMursPosition.Add(child.gameObject.transform.position);
        }

        // Enlever la possibilite de mettre des objets sur les murs ouvrables
        foreach (Transform child in mursOuvrablesParent.transform)
        {
            listeMursPosition.Add(child.gameObject.transform.position);
        }

        for (int x = grilleXMin; x <= grilleXMax; x++)
        {
            for (int z = grilleZMin; z <= grilleZMax; z++)
            {
                bool tuilePotentielle = true;

                foreach (Vector3 position in listeMursPosition)
                {
                    if (position.x == x &&
                        position.z == z)
                    {
                        tuilePotentielle = false;
                    }
                }

                if (tuilePotentielle)
                {
                    // GameObject tuileAjoutDebug = Instantiate(prefabTest, new Vector3(x, 0, z), Quaternion.identity); // DEBUG
                    listePlacementPotentielle.Add(new Vector3(x, 0, z));
                }
            }
        }

        GameObject objetInstantiee;
        // Mettre les tresors
        int indexPositionFleche = UnityEngine.Random.Range(0, listePlacementPotentielle.Count);
        objetInstantiee = Instantiate(tresorObj, listePlacementPotentielle[indexPositionFleche] + new Vector3(0.5f, 0.0f, 0.5f), Quaternion.identity);
        listePlacementPotentielle.RemoveAt(indexPositionFleche);
        objetInstantiee.tag = "Tresor";
        objetInstantiee.transform.parent = objetParentPlacer.transform;

        // Mettre les fleches
        for (int i = 0; i < nombreFleches; i++)
        {
            indexPositionFleche = UnityEngine.Random.Range(0, listePlacementPotentielle.Count);
            objetInstantiee = Instantiate(flecheGameObj, listePlacementPotentielle[indexPositionFleche] + new Vector3(0.5f, 1.5f, 0.5f), Quaternion.identity);
            listePlacementPotentielle.RemoveAt(indexPositionFleche);
            objetInstantiee.transform.parent = objetParentPlacer.transform;
        }

        for (int i = 0; i < nombreTeleTransporteur; i++)
        {
            indexPositionFleche = UnityEngine.Random.Range(0, listePlacementPotentielle.Count);
            Instantiate(teleTransporteurObj, listePlacementPotentielle[indexPositionFleche] + new Vector3(0.5f, 0.0f, 0.5f), Quaternion.identity);
            listePlacementPotentielle.RemoveAt(indexPositionFleche);
            objetInstantiee.transform.parent = objetParentPlacer.transform;
        }

        for (int i = 0; i < nombreTeleRecepteur; i++)
        {
            indexPositionFleche = UnityEngine.Random.Range(0, listePlacementPotentielle.Count);
            Instantiate(teleRecepteurObj, listePlacementPotentielle[indexPositionFleche] + new Vector3(0.5f, 0.0f, 0.5f), Quaternion.identity);
            listePlacementPotentielle.RemoveAt(indexPositionFleche);
            objetInstantiee.transform.parent = objetParentPlacer.transform;
        }
    }


    public static void enleverTuilePotentielle(float x, float z)
    {
        listeMursPosition.Add(new Vector3(x, 0, z));
    }

    void Update()
    {
        if (PlayerController.joueurInput) temps -= Time.deltaTime;
        if (temps <= 0) RestartLevel();
    }

}
