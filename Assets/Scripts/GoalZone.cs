using UnityEngine;

/// <summary>
/// ติดกับ GameObject เป้าหมาย (Goal)
/// ใช้ Trigger ตรวจจับลูกบอลเข้าถึงเป้า
/// </summary>
public class GoalZone : MonoBehaviour
{
    [Header("Score")]
    public int scoreReward = 100;

    [Header("Visual Feedback")]
    public ParticleSystem celebrationParticles;

    /// <summary>
    /// Unity Physics 3D: OnTriggerEnter
    /// เมื่อลูกบอลเข้า Trigger zone = ชนะ
    /// </summary>
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            if (celebrationParticles != null)
                celebrationParticles.Play();

            GameManager.Instance?.AddScore(scoreReward);
            GameManager.Instance?.TriggerWin();
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(0f, 1f, 0f, 0.3f);
        Gizmos.DrawSphere(transform.position, transform.localScale.x * 0.5f);
    }
}
