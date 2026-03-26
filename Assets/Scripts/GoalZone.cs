using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalZone : MonoBehaviour
{
    [Header("Score")]
    public int scoreReward = 100;

    [Header("Goal Settings")]
    public bool requireAllGoals = true;

    [Header("Visual (optional)")]
    public ParticleSystem celebrationParticles;

    private bool isReached = false;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Ball")) return;
        if (isReached) return;

        isReached = true;
        Debug.Log("GoalZone: Ball เข้า Goal!");

        if (celebrationParticles != null)
            celebrationParticles.Play();

        var rend = GetComponent<Renderer>();
        if (rend != null) rend.material.color = Color.gray;

        if (GameManager.Instance != null)
            GameManager.Instance.AddScore(scoreReward);

        if (GoalManager.Instance != null)
        {
            Debug.Log("GoalZone: แจ้ง GoalManager");
            GoalManager.Instance.OnGoalReached(requireAllGoals);
        }
        else
        {
            Debug.LogWarning("GoalZone: ไม่พบ GoalManager! Win ตรงๆ");
            Invoke(nameof(WinDirect), 1.5f);
        }
    }

    void WinDirect()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.TriggerWin();
        else
        {
            int next = SceneManager.GetActiveScene().buildIndex + 1;
            SceneManager.LoadScene(next);
        }
    }
}