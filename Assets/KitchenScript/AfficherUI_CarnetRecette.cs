using UnityEngine;
using UnityEngine.UI;

public class AfficherUI_CarnetRecette : MonoBehaviour
{
    public GameObject uiPanel; // panel des recettes à activer quand le joueur est proche
    public KitchenBehavior kitchen; // référence au script KitchenBehavior
    public Text feedbackText; // optionnel : texte de confirmation
    public Button[] recipeButtons; // assigner les boutons UI (un par recette)
    public string[] recipeNames; // mêmes longueurs que recipeButtons
    public float[] recipeTimes; // en secondes, mêmes longueurs que recipeButtons

    // Nouveau : bouton pour afficher la file et zone texte pour afficher les détails
    public Button showQueueButton;
    public Text queueDetailsText; // affichera la liste d'attente quand on appuie sur showQueueButton

    // Flag pour savoir si le joueur est dans la trigger box
    private bool playerInside = false;

    private void Start()
    {
        // vider feedback au démarrage
        if (feedbackText != null) feedbackText.text = "";

        if (uiPanel != null) uiPanel.SetActive(false);

        if (recipeButtons != null && kitchen != null)
        {
            int count = Mathf.Min(recipeButtons.Length, Mathf.Min(recipeNames.Length, recipeTimes.Length));
            for (int i = 0; i < count; i++)
            {
                int idx = i; // capture safe
                if (recipeButtons[idx] != null)
                {
                    recipeButtons[idx].onClick.RemoveAllListeners();
                    recipeButtons[idx].onClick.AddListener(() =>
                    {
                        kitchen.AjouterRecetteQueue(recipeNames[idx], recipeTimes[idx]);
                        if (feedbackText != null)
                            feedbackText.text = $"Commande envoyée : {recipeNames[idx]}";
                    });
                }
            }
        }

        if (showQueueButton != null)
        {
            showQueueButton.onClick.RemoveAllListeners();
            showQueueButton.onClick.AddListener(() =>
            {
                ShowQueue();
            });
        }
    }

    // Méthode publique pour afficher la file (peut être liée depuis l'Inspector)
    public void ShowQueue()
    {
        if (kitchen == null)
        {
            if (queueDetailsText != null) queueDetailsText.text = "Cuisine non assignée.";
            else if (feedbackText != null) feedbackText.text = "Cuisine non assignée.";
            return;
        }

        string status = kitchen.GetQueueStatusString();

        if (queueDetailsText != null)
        {
            queueDetailsText.gameObject.SetActive(true);
            queueDetailsText.transform.SetAsLastSibling();
            var col = queueDetailsText.color;
            col.a = 1f;
            queueDetailsText.color = col;

            queueDetailsText.text = status;
        }
        else if (feedbackText != null)
        {
            feedbackText.text = status;
        }
    }

    private void Update()
    {
        if (!playerInside) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (uiPanel != null)
            {
                uiPanel.SetActive(!uiPanel.activeSelf);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        playerInside = true;
        if (uiPanel != null) uiPanel.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        playerInside = false;
        if (uiPanel != null) uiPanel.SetActive(false);
        if (feedbackText != null) feedbackText.text = "";
        if (queueDetailsText != null) queueDetailsText.text = "";
    }
}

