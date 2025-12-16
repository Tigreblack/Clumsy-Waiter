using UnityEngine;

public class AfficherUI_CarnetRecette : MonoBehaviour
{
    public GameObject objectToActivate1;
    public GameObject objectToActivate2;
    // Flag pour savoir si le joueur est dans la trigger box
    private bool playerInside = false;

    private void Start()
    {
        // Désactiver les deux objets au démarrage
        if (objectToActivate1 != null) objectToActivate1.SetActive(false);
        if (objectToActivate2 != null) objectToActivate2.SetActive(false);
    }

    private void Update()
    {
        // Vérifier l'entrée chaque frame seulement si le joueur est dans la zone
        if (!playerInside) return;
        // Quand l'objet 1 est actif et que l'utilisateur appuie sur Space, basculer vers l'objet 2
        if (objectToActivate1 != null && objectToActivate1.activeSelf && Input.GetKeyDown(KeyCode.Space))
        {
            objectToActivate1.SetActive(false);
            if (objectToActivate2 != null) objectToActivate2.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        playerInside = true;
        // Activer l'objet 1 quand le joueur entre
        if (objectToActivate1 != null) objectToActivate1.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        playerInside = false;
        // Optionnel : désactiver les objets quand le joueur sort
        if (objectToActivate1 != null) objectToActivate1.SetActive(false);
        if (objectToActivate2 != null) objectToActivate2.SetActive(false);
    }
}
