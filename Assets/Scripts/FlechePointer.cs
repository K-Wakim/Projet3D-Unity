using UnityEngine;

public class FlechePointer : MonoBehaviour
{

    private GameObject tresor;
    void Start()
    {
        tresor = GameObject.FindWithTag("Tresor");
        if (tresor != null)
        {
            Vector3 positionTresor = tresor.transform.position;
            positionTresor.y = transform.position.y;
            transform.LookAt(positionTresor);
        }
    }

    void Update()
    {
        transform.Rotate(0f, 0f, 180f * Time.deltaTime, Space.Self);
    }
}
