using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// UIManager — ควบคุม HUD ทั้งหมด
/// แสดง HP, Score, Timer และ Panel Win/GameOver
/// </summary>
public class UIManager : MonoBehaviour
{
    [Header("HUD")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI hpText;

    [Header("HP Icons (optional)")]
    public Image[] hpIcons;

    [Header("Panels")]
    public GameObject winPanel;
    public GameObject gameOverPanel;
    public TextMeshProUGUI finalScoreText;

    [Header("References")]
    public BallController ball;
    public GameManager gameManager;

    void Start()
    {
        // Subscribe events
        if (gameManager != null)
        {
            gameManager.OnScoreChanged += UpdateScore;
            gameManager.OnTimeChanged  += UpdateTimer;
            gameManager.OnGameWin      += ShowWinPanel;
            gameManager.OnGameOver     += ShowGameOverPanel;
        }

        if (ball != null)
            ball.OnHPChanged += UpdateHP;

        // Init UI
        winPanel?.SetActive(false);
        gameOverPanel?.SetActive(false);
        UpdateScore(0);
        UpdateHP(ball != null ? ball.maxHP : 3);
    }

    void UpdateScore(int value)
    {
        if (scoreText != null)
            scoreText.text = $"Score: {value}";
    }

    void UpdateTimer(float value)
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(value / 60);
            int seconds = Mathf.FloorToInt(value % 60);
            timerText.text = $"Time: {minutes:00}:{seconds:00}";

            // เตือนเมื่อเวลาน้อย
            timerText.color = value < 10f ? Color.red : Color.white;
        }
    }

    void UpdateHP(int value)
    {
        if (hpText != null)
            hpText.text = $"HP: {value}";

        // อัพเดท HP icons (ถ้ามี)
        for (int i = 0; i < hpIcons.Length; i++)
        {
            if (hpIcons[i] != null)
                hpIcons[i].enabled = i < value;
        }
    }

    void ShowWinPanel()
    {
        winPanel?.SetActive(true);
        int finalScore = PlayerPrefs.GetInt("FinalScore", 0);
        if (finalScoreText != null)
            finalScoreText.text = $"Score: {finalScore}";
    }

    void ShowGameOverPanel()
    {
        gameOverPanel?.SetActive(true);
    }

    void OnDestroy()
    {
        // Unsubscribe เพื่อป้องกัน memory leak
        if (gameManager != null)
        {
            gameManager.OnScoreChanged -= UpdateScore;
            gameManager.OnTimeChanged  -= UpdateTimer;
            gameManager.OnGameWin      -= ShowWinPanel;
            gameManager.OnGameOver     -= ShowGameOverPanel;
        }
        if (ball != null)
            ball.OnHPChanged -= UpdateHP;
    }
}
