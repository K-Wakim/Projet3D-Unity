using UnityEngine;
using UnityEngine.UIElements;

public class HUDScript : MonoBehaviour
{

    Label niveau;
    Label ouvreur;
    Label score;
    Label temps;

    void Start()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        niveau = root.Q<Label>("NombreNiveau");
        ouvreur = root.Q<Label>("NombreOuvreur");
        score = root.Q<Label>("Score");
        temps = root.Q<Label>("Temps");

        niveau.text = GameManager.niveau.ToString();

    }

    void Update()
    {
        score.text = GameManager.score.ToString();
        ouvreur.text = GameManager.nombreOuvreur.ToString();

        int minutes = (int)Mathf.Floor(GameManager.temps / 60f);
        int secondes = (int)Mathf.Floor(GameManager.temps % 60f);

        temps.text = string.Format("{0:00}:{1:00}", minutes, secondes);
    }
}
