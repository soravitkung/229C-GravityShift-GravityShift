using UnityEngine;
using TMPro; // สำหรับใช้ UI TextMeshPro [cite: 13, 81]

public class RocketController : MonoBehaviour
{
    [Header("Physics Settings")]
    public float targetAcceleration = 15f; // ค่าความเร่งที่ต้องการ (a)
    private Rigidbody rb;

    [Header("UI Settings")]
    public TextMeshProUGUI statusText; // ลาก UI Text มาใส่ตรงนี้ 

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // ตั้งค่ามวลของยาน (m) 
        rb.mass = 2.0f;
    }

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
        }

        UpdateUI();
    }

    void ApplyThrust()
    {
        // คำนวณแรง (F) จากกฎข้อที่ 2 ของนิวตัน: F = m * a 
        float mass = rb.mass;
        float forceValue = mass * targetAcceleration;

        // สร้าง Vector แรงในทิศทางด้านบนของยาน
        Vector3 thrustForce = transform.up * forceValue;

        // ใช้แรงที่คำนวณได้จริงในการ AddForce 
        rb.AddForce(thrustForce);
    }

    void UpdateUI()
    {
        if (statusText != null)
        {
            // แสดงผลค่าที่คำนวณได้บน UI 
            statusText.text = $"Mass: {rb.mass} kg\n" +
                              $"Thrust: {rb.mass * targetAcceleration} N\n" +
                              $"Velocity: {rb.linearVelocity.magnitude:F2} m/s";
        }
    }
}