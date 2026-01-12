using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }
    
    public TextMeshProUGUI scoreText;
    
    private int score = 0;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        UpdateUI();
    }
    
    public void AddScore(int points)
    {
        score += points;
        if (score < 0) score = 0;
        UpdateUI();
    }
    
    public void RemoveScore(int points)
    {
        score -= points;
        if (score < 0) score = 0;
        UpdateUI();
    }
    
    void UpdateUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }
    
    public int GetCurrentScore()
    {
        return score;
    }
    
    public void ResetScore()
    {
        score = 0;
        UpdateUI();
    }
}
