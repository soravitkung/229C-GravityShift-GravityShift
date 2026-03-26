using UnityEngine;

/// <summary>
/// ติดกับ Empty GameObject ชื่อ "GoalManager" ใน Scene
/// นับ Goal ที่เข้าแล้ว และตัดสินว่า Win เมื่อไหร่
/// </summary>
public class GoalManager : MonoBehaviour
{
    public static GoalManager Instance { get; private set; }

    private int totalGoals = 0;
    private int goalsReached = 0;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // นับ GoalZone ทั้งหมดใน Scene ตอน Start (หลัง Awake ทุกตัวทำงานแล้ว)
        totalGoals = FindObjectsByType<GoalZone>(FindObjectsSortMode.None).Length;
        goalsReached = 0;
        Debug.Log($"GoalManager: มี Goal ทั้งหมด {totalGoals} อัน");
    }

    public void OnGoalReached(bool requireAll)
    {
        goalsReached++;
        Debug.Log($"GoalManager: เข้า Goal {goalsReached}/{totalGoals}");

        if (!requireAll)
        {
            // เข้าอันไหนก็ Win ทันที
            Win();
        }
        else if (goalsReached >= totalGoals)
        {
            // ครบทุก Goal แล้ว
            Win();
        }
    }

    void Win()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.TriggerWin();
        else
        {
            int next = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1;
            UnityEngine.SceneManagement.SceneManager.LoadScene(next);
        }
    }
}