using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AfficherUI_CarnetRecette : MonoBehaviour
{
    public GameObject uiPanel;
    public KitchenBehavior kitchen;
    public Text feedbackText;
    public Button[] recipeButtons;
    public string[] recipeNames;
    public float[] recipeTimes;
    public Button showQueueButton;
    public Text queueDetailsText;
    public TextMeshProUGUI warningText;
    public float warningDisplayDuration = 3f;

    private bool playerInside = false;
    private float warningTimer = 0f;

    void Start()
    {
        if (feedbackText != null) feedbackText.text = "";
        if (warningText != null) warningText.text = "";
        if (uiPanel != null) uiPanel.SetActive(false);

        if (recipeButtons != null && kitchen != null)
        {
            int count = Mathf.Min(recipeButtons.Length, Mathf.Min(recipeNames.Length, recipeTimes.Length));
            for (int i = 0; i < count; i++)
            {
                int idx = i;
                if (recipeButtons[idx] != null)
                {
                    recipeButtons[idx].onClick.RemoveAllListeners();
                    recipeButtons[idx].onClick.AddListener(() =>
                    {
                        bool success = kitchen.AjouterRecetteQueue(recipeNames[idx], recipeTimes[idx]);
                        if (success)
                        {
                            if (feedbackText != null)
                                feedbackText.text = "Commande envoyée : " + recipeNames[idx];
                        }
                        else
                        {
                            ShowWarning("Vous ne pouvez plus cuisiner actuellement");
                        }
                    });
                }
            }
        }

        if (showQueueButton != null)
        {
            showQueueButton.onClick.RemoveAllListeners();
            showQueueButton.onClick.AddListener(() => ShowQueue());
        }
    }

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

    void ShowWarning(string message)
    {
        if (warningText != null)
        {
            warningText.text = message;
            warningTimer = warningDisplayDuration;
        }
    }

    void Update()
    {
        if (!playerInside) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (uiPanel != null)
            {
                uiPanel.SetActive(!uiPanel.activeSelf);
            }
        }

        if (warningTimer > 0f)
        {
            warningTimer -= Time.deltaTime;
            if (warningTimer <= 0f && warningText != null)
            {
                warningText.text = "";
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        playerInside = true;
        if (uiPanel != null) uiPanel.SetActive(true);
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        playerInside = false;
        if (uiPanel != null) uiPanel.SetActive(false);
        if (feedbackText != null) feedbackText.text = "";
        if (queueDetailsText != null) queueDetailsText.text = "";
        if (warningText != null) warningText.text = "";
        warningTimer = 0f;
    }
}

