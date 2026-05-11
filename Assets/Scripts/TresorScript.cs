using UnityEngine;

public class TresorScript : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) GameManager.NextLevel();
    }
}
