using UnityEngine;

/// <summary>
/// ติดกับ GameObject ที่เป็น "ดาว" แต่ละดวง
/// คำนวณแรงโน้มถ่วงตามกฎของนิวตัน: F = G * m1 * m2 / r^2
/// แล้ว AddForce ไปยังลูกบอลที่อยู่ในรัศมี
/// </summary>
public class GravityAttractor : MonoBehaviour
{
    [Header("Newton's Gravitation Settings")]
    [Tooltip("ค่าคงที่แรงโน้มถ่วง (ปรับได้เพื่อ Gameplay)")]
    public float G = 667.4f;

    [Tooltip("มวลของดาว (kg)")]
    public float planetMass = 1000f;

    [Tooltip("รัศมีที่ดาวจะดึงดูดลูกบอล (0 = ไม่จำกัด)")]
    public float influenceRadius = 20f;

    [Tooltip("แสดง Gizmo รัศมีใน Scene View")]
    public bool showGizmo = true;

    void OnDrawGizmosSelected()
    {
        if (!showGizmo) return;
        Gizmos.color = new Color(1f, 0.8f, 0f, 0.3f);
        Gizmos.DrawSphere(transform.position, influenceRadius);
        Gizmos.color = new Color(1f, 0.8f, 0f, 1f);
        Gizmos.DrawWireSphere(transform.position, influenceRadius);
    }

    /// <summary>
    /// เรียกจาก BallController ใน FixedUpdate
    /// คำนวณและ Apply แรงโน้มถ่วงไปที่ Rigidbody ของลูกบอล
    /// </summary>
    public void Attract(Rigidbody ballRb)
    {
        Vector3 direction = transform.position - ballRb.position;
        float distance = direction.magnitude;

        // ป้องกัน division by zero
        if (distance < 0.1f) return;

        // เช็ครัศมีอิทธิพล
        if (influenceRadius > 0 && distance > influenceRadius) return;

        float ballMass = ballRb.mass;

        // ====================================================
        // กฎความโน้มถ่วงสากลของนิวตัน
        // F = G * m1 * m2 / r^2
        // ====================================================
        float forceMagnitude = G * planetMass * ballMass / (distance * distance);
        Vector3 force = direction.normalized * forceMagnitude;

        ballRb.AddForce(force);
    }
}
