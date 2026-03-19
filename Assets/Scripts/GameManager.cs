using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// GameManager — Singleton
/// จัดการ Score, Timer, Win/Lose condition และเปลี่ยน Scene
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game Settings")]
    public float timeLimit = 60f;

    // Events แจ้ง UI
    public System.Action<int> OnScoreChanged;
    public System.Action<float> OnTimeChanged;
    public System.Action OnGameWin;
    public System.Action OnGameOver;

    private int score = 0;
    private float timeRemaining;
    private bool gameActive = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        timeRemaining = timeLimit;
        gameActive = true;
    }

    void Update()
    {
        if (!gameActive) return;

        timeRemaining -= Time.deltaTime;
        OnTimeChanged?.Invoke(timeRemaining);

        if (timeRemaining <= 0)
        {
            timeRemaining = 0;
            TriggerGameOver();
        }
    }

    public void AddScore(int amount)
    {
        score += amount;
        OnScoreChanged?.Invoke(score);
    }

    public int GetScore() => score;

    /// <summary>
    /// เรียกเมื่อลูกบอลถึงเป้าหมาย (Goal)
    /// </summary>
    public void TriggerWin()
    {
        if (!gameActive) return;
        gameActive = false;
        OnGameWin?.Invoke();

        // บันทึก score ข้ามฉาก
        PlayerPrefs.SetInt("FinalScore", score);
        PlayerPrefs.Save();

        Invoke(nameof(LoadNextScene), 2f);
    }

    public void TriggerGameOver()
    {
        if (!gameActive) return;
        gameActive = false;
        OnGameOver?.Invoke();
        Invoke(nameof(ReloadScene), 2f);
    }

    void LoadNextScene()
    {
        int next = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(next);
    }

    void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
