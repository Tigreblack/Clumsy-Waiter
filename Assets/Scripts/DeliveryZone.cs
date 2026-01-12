using UnityEngine;
using TMPro;
using System;

public class DeliveryZone : MonoBehaviour
{
    public string requestedFood;
    public int questPoints = 3;
    
    public TextMeshProUGUI requestText;
    public TextMeshProUGUI timerText;
    public Canvas worldCanvas;
    
    public Color correctColor = Color.green;
    public Color wrongColor = Color.red;
    public Color defaultColor = Color.white;
    public Color warningColor = new Color(1f, 0.5f, 0f);
    public Color criticalColor = Color.red;
    
    public float questDuration = 180f;
    public float warningTime = 60f;
    public float criticalTime = 30f;
    public float feedbackDisplayTime = 2f;
    
    bool completed = false;
    bool failed = false;
    float timeLeft;
    
    public event Action OnDeliveryComplete;
    public event Action OnQuestFailed;
    
    void Start()
    {
        timeLeft = questDuration;
        UpdateRequestUI();
        UpdateTimerUI();
    }
    
    void Update()
    {
        if (!completed && !failed)
        {
            timeLeft -= Time.deltaTime;
            UpdateTimerUI();
            
            if (timeLeft <= 0)
            {
                QuestFailed();
            }
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (completed) return;
        
        if (other.CompareTag("Plate"))
        {
            PlateSlotManager plateManager = other.GetComponent<PlateSlotManager>();
            if (plateManager != null)
            {
                var items = plateManager.GetItems();
                if (items.Count > 0)
                {
                    PlateItem item = items[0];
                    string deliveredFood = GetFoodName(item.gameObject.name);
                    CheckDelivery(deliveredFood, other.gameObject);
                }
            }
        }
        else if (other.CompareTag("Food"))
        {
            string deliveredFood = GetFoodName(other.gameObject.name);
            CheckDelivery(deliveredFood, other.gameObject);
        }
    }
    
    void CheckDelivery(string deliveredFood, GameObject obj = null)
    {
        bool correct = deliveredFood.Equals(requestedFood, StringComparison.OrdinalIgnoreCase);
        
        if (correct)
        {
            completed = true;
            if (requestText != null) requestText.color = correctColor;
            if (timerText != null) timerText.color = correctColor;
            
            if (ScoreManager.Instance != null)
                ScoreManager.Instance.AddScore(questPoints);
            
            if (obj != null)
                Destroy(obj, feedbackDisplayTime);
            
            Invoke(nameof(TriggerComplete), feedbackDisplayTime);
        }
    }
    
    void TriggerComplete()
    {
        OnDeliveryComplete?.Invoke();
    }
    
    void QuestFailed()
    {
        if (failed) return;
        
        failed = true;
        
        if (ScoreManager.Instance != null)
            ScoreManager.Instance.RemoveScore(questPoints);
        
        if (requestText != null) requestText.color = wrongColor;
        if (timerText != null)
        {
            timerText.text = "FAILED!";
            timerText.color = wrongColor;
        }
        
        Invoke(nameof(TriggerFailed), feedbackDisplayTime);
    }
    
    void TriggerFailed()
    {
        OnQuestFailed?.Invoke();
    }
    
    void UpdateRequestUI()
    {
        if (requestText != null)
        {
            string vip = questPoints > 5 ? " [VIP]" : "";
            requestText.text = "Order: " + requestedFood + vip + " (" + questPoints + "pts)";
            requestText.color = defaultColor;
        }
    }
    
    void UpdateTimerUI()
    {
        if (timerText != null)
        {
            int mins = Mathf.FloorToInt(timeLeft / 60f);
            int secs = Mathf.FloorToInt(timeLeft % 60f);
            timerText.text = string.Format("{0:00}:{1:00}", mins, secs);
            
            if (timeLeft <= criticalTime)
                timerText.color = criticalColor;
            else if (timeLeft <= warningTime)
                timerText.color = warningColor;
            else
                timerText.color = defaultColor;
        }
    }
    
    string GetFoodName(string name)
    {
        name = name.Replace("(Clone)", "").Trim();
        
        if (name.Contains("Pizza")) return "Pizza";
        if (name.Contains("Hotdog")) return "Hotdog";
        if (name.Contains("Curry")) return "Curry";
        if (name.Contains("Udon")) return "Udon";
        if (name.Contains("Maki") || name.Contains("Sushi")) return "Sushi";
        if (name.Contains("Doughnut")) return "Doughnut";
        
        return name;
    }
    
    public void SetRequestedFood(string food, int points)
    {
        requestedFood = food;
        questPoints = points;
        completed = false;
        failed = false;
        timeLeft = questDuration;
        UpdateRequestUI();
        UpdateTimerUI();
    }
}
