using System.Linq;
using UnityEngine;

public class Teleportation : MonoBehaviour
{

    public AudioSource audioTeleporter;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        audioTeleporter.Play();

        GameObject joueur = GameObject.FindGameObjectWithTag("Player");
        CharacterController controller = joueur.GetComponent<CharacterController>();
        GameObject[] listeTelerecepteurs = GameObject.FindGameObjectsWithTag("Telerecepteur");

        GameObject telerecepteurHasard = listeTelerecepteurs[Random.Range(0, listeTelerecepteurs.Count())];
        controller.enabled = false;
        joueur.transform.position = new Vector3(telerecepteurHasard.transform.position.x,0.5f,telerecepteurHasard.transform.position.z);
        controller.enabled = true;
    }
}
