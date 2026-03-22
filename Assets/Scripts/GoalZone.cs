using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ติดกับ Goal GameObject
/// - Collider ต้องเป็น Is Trigger = true
/// - Ball ต้องมี Tag = "Ball"
/// </summary>
public class GoalZone : MonoBehaviour
{
    [Header("Score")]
    public int scoreReward = 100;

    [Header("Next Scene")]
    [Tooltip("ชื่อ Scene ถัดไป ถ้าปล่อยว่างจะ Load Scene ถัดไปใน Build Settings")]
    public string nextSceneName = "";

    [Header("Visual Feedback")]
    public ParticleSystem celebrationParticles;

    void OnTriggerEnter(Collider other)
    {
        // Debug ช่วย log ว่ามีอะไรเข้า Trigger
        Debug.Log($"GoalZone: {other.gameObject.name} tag={other.tag} เข้า Trigger");

        if (!other.CompareTag("Ball")) return;

        Debug.Log("Goal!");

        if (celebrationParticles != null)
            celebrationParticles.Play();

        GameManager.Instance?.AddScore(scoreReward);
        GameManager.Instance?.TriggerWin();

        // ถ้าไม่มี GameManager ให้ Load Scene ตรงๆ เลย
        if (GameManager.Instance == null)
        {
            Invoke(nameof(LoadNext), 1.5f);
        }
    }

    void LoadNext()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
            SceneManager.LoadScene(nextSceneName);
        else
        {
            int next = SceneManager.GetActiveScene().buildIndex + 1;
            SceneManager.LoadScene(next);
        }
    }
}