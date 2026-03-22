using UnityEngine;

/// <summary>
/// 2D Side View — ไม่ใช้แล้วสำหรับเกมนี้
/// แต่เก็บไว้ถ้าอยากเพิ่ม Stage ที่มีดาวดูดในอนาคต
/// ตอนนี้ใช้ Unity Gravity ปกติแทน (ตกลงแกน Y)
/// </summary>
public class GravityAttractor : MonoBehaviour
{
    [Header("Newton's Gravitation (optional)")]
    public float G = 100f;
    public float planetMass = 500f;
    public float influenceRadius = 15f;
    public bool showGizmo = true;

    void OnDrawGizmosSelected()
    {
        if (!showGizmo) return;
        Gizmos.color = new Color(1f, 0.8f, 0f, 0.3f);
        Gizmos.DrawSphere(transform.position, influenceRadius);
        Gizmos.color = new Color(1f, 0.8f, 0f, 1f);
        Gizmos.DrawWireSphere(transform.position, influenceRadius);
    }

    public void Attract(Rigidbody ballRb)
    {
        Vector3 direction = transform.position - ballRb.position;
        float distance = direction.magnitude;
        if (distance < 0.1f || (influenceRadius > 0 && distance > influenceRadius)) return;

        float ballMass = ballRb.mass;
        // F = G * m1 * m2 / r^2
        float forceMagnitude = G * planetMass * ballMass / (distance * distance);
        ballRb.AddForce(direction.normalized * forceMagnitude);
    }
}