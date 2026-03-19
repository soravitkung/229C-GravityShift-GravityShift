using UnityEngine;

/// <summary>
/// ติดกับ GameObject ที่เป็น "ลูกบอล"
/// จัดการ:
///   1. ดึงแรงโน้มถ่วงจากดาวทุกดวงใน Scene
///   2. คำนวณแรงต้านอากาศ (Air Resistance)
///   3. ตรวจจับการชนอุกกาบาต (OnCollisionEnter)
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class BallController : MonoBehaviour
{
    [Header("Air Resistance (ทฤษฎีแรงต้านอากาศ)")]
    [Tooltip("ค่าสัมประสิทธิ์แรงต้านอากาศ Cd")]
    public float dragCoefficient = 0.47f;

    [Tooltip("ความหนาแน่นของตัวกลาง (อวกาศ = น้อยมาก, ถ้าต้องการให้รู้สึกได้ให้เพิ่ม)")]
    public float mediumDensity = 0.05f;

    [Tooltip("พื้นที่หน้าตัดของลูกบอล (m^2)")]
    public float crossSectionArea = 0.5f;

    [Header("Health")]
    public int maxHP = 3;
    public int currentHP;

    // Event แจ้ง UI เมื่อ HP เปลี่ยน
    public System.Action<int> OnHPChanged;
    // Event แจ้งเมื่อตาย
    public System.Action OnDead;

    private Rigidbody rb;
    private GravityAttractor[] attractors;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        // ปิด Unity Gravity ปกติ เพราะเราจะคำนวณเอง
        rb.useGravity = false;

        currentHP = maxHP;
    }

    void Start()
    {
        // หาดาวทุกดวงใน Scene
        attractors = FindObjectsByType<GravityAttractor>(FindObjectsSortMode.None);
    }

    void FixedUpdate()
    {
        ApplyPlanetGravity();
        ApplyAirResistance();
    }

    /// <summary>
    /// วนหาดาวทุกดวง แล้วให้แต่ละดวง Attract ลูกบอล
    /// </summary>
    void ApplyPlanetGravity()
    {
        foreach (var attractor in attractors)
        {
            if (attractor != null)
                attractor.Attract(rb);
        }
    }

    /// <summary>
    /// ทฤษฎีแรงต้านอากาศ (Aerodynamic Drag)
    /// F_drag = 0.5 * Cd * rho * A * v^2
    /// ทิศทางตรงข้ามกับการเคลื่อนที่เสมอ
    /// </summary>
    void ApplyAirResistance()
    {
        Vector3 velocity = rb.linearVelocity;
        float speed = velocity.magnitude;

        if (speed < 0.01f) return;

        // ====================================================
        // สูตรแรงต้านอากาศ
        // F_drag = 0.5 * Cd * ρ * A * v²
        // ====================================================
        float dragMagnitude = 0.5f * dragCoefficient * mediumDensity * crossSectionArea * speed * speed;
        Vector3 dragForce = -velocity.normalized * dragMagnitude;

        rb.AddForce(dragForce);
    }

    /// <summary>
    /// Unity Physics 3D: OnCollisionEnter
    /// เมื่อชนอุกกาบาต HP จะลด
    /// </summary>
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            TakeDamage(1);
        }
    }

    void TakeDamage(int amount)
    {
        currentHP -= amount;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
        OnHPChanged?.Invoke(currentHP);

        if (currentHP <= 0)
            OnDead?.Invoke();
    }

    /// <summary>
    /// Reset ลูกบอลกลับสู่สถานะเริ่มต้น
    /// </summary>
    public void ResetBall()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        currentHP = maxHP;
        OnHPChanged?.Invoke(currentHP);
    }
}
