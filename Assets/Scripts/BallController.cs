using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BallController : MonoBehaviour
{
    [Header("Air Resistance")]
    public float dragCoefficient = 0.47f;
    public float mediumDensity = 0.1f;
    public float crossSectionArea = 0.1f;

    [Header("Health")]
    public int maxHP = 3;
    public int currentHP;

    public System.Action<int> OnHPChanged;
    public System.Action OnDead;

    private Rigidbody rb;
    private GravityAttractor[] attractors;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.FreezePositionZ
                       | RigidbodyConstraints.FreezeRotationX
                       | RigidbodyConstraints.FreezeRotationY;
        currentHP = maxHP;
    }

    void Start()
    {
        // หา GravityAttractor ทุกดวงใน Scene
        attractors = FindObjectsByType<GravityAttractor>(FindObjectsSortMode.None);
        Debug.Log($"BallController: พบ GravityAttractor {attractors.Length} ดวง");
    }

    void FixedUpdate()
    {
        ApplyPlanetGravity();
        ApplyAirResistance();
    }

    /// <summary>
    /// วนดึงแรงโน้มถ่วงจากดาวทุกดวง
    /// F = G * m1 * m2 / r^2  (คำนวณใน GravityAttractor.Attract)
    /// </summary>
    void ApplyPlanetGravity()
    {
        foreach (var attractor in attractors)
        {
            if (attractor != null && attractor.gameObject.activeInHierarchy)
                attractor.Attract(rb);
        }
    }

    /// <summary>
    /// F_drag = 0.5 * Cd * rho * A * v^2
    /// </summary>
    void ApplyAirResistance()
    {
        Vector3 velocity = rb.linearVelocity;
        float speed = velocity.magnitude;
        if (speed < 0.01f) return;

        float dragMagnitude = 0.5f * dragCoefficient * mediumDensity
                              * crossSectionArea * speed * speed;
        rb.AddForce(-velocity.normalized * dragMagnitude);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
            TakeDamage(1);
    }

    void TakeDamage(int amount)
    {
        currentHP = Mathf.Clamp(currentHP - amount, 0, maxHP);
        OnHPChanged?.Invoke(currentHP);
        if (currentHP <= 0) OnDead?.Invoke();
    }

    public void ResetBall()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        currentHP = maxHP;
        OnHPChanged?.Invoke(currentHP);
    }
}