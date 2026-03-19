using UnityEngine;

/// <summary>
/// ติดกับ GameObject ที่เป็นจุดยิง (Launcher)
/// - Click drag เพื่อเล็งและกำหนดแรง
/// - แสดง Trajectory dot preview ก่อนยิง
/// - ยิงโดยใช้กฎนิวตันข้อ 2: F = m * a
/// </summary>
public class BallLauncher : MonoBehaviour
{
    [Header("Ball Reference")]
    public BallController ballPrefab;
    public Transform launchPoint;

    [Header("Launch Settings")]
    [Tooltip("แรงสูงสุดที่ยิงได้")]
    public float maxLaunchForce = 500f;

    [Tooltip("ระยะ drag สูงสุดที่คิดเป็น maxForce")]
    public float maxDragDistance = 5f;

    [Header("Trajectory Preview")]
    public LineRenderer trajectoryLine;
    public int trajectorySteps = 30;
    public float trajectoryTimeStep = 0.1f;

    private BallController currentBall;
    private Vector3 dragStartPos;
    private bool isDragging = false;
    private Camera mainCam;

    void Start()
    {
        mainCam = Camera.main;
        SpawnBall();
    }

    void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
        if (currentBall == null) return;

        if (Input.GetMouseButtonDown(0))
        {
            dragStartPos = GetMouseWorldPos();
            isDragging = true;
        }

        if (Input.GetMouseButton(0) && isDragging)
        {
            Vector3 currentPos = GetMouseWorldPos();
            Vector3 dragVector = dragStartPos - currentPos;
            DrawTrajectory(launchPoint.position, CalculateLaunchVelocity(dragVector));
        }

        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            isDragging = false;
            trajectoryLine.positionCount = 0;

            Vector3 currentPos = GetMouseWorldPos();
            Vector3 dragVector = dragStartPos - currentPos;
            LaunchBall(dragVector);
        }
    }

    Vector3 GetMouseWorldPos()
    {
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        // ยิง Ray ไปที่ระนาบ Z=0
        Plane plane = new Plane(Vector3.forward, launchPoint.position);
        if (plane.Raycast(ray, out float distance))
            return ray.GetPoint(distance);
        return Vector3.zero;
    }

    Vector3 CalculateLaunchVelocity(Vector3 dragVector)
    {
        float dragRatio = Mathf.Clamp01(dragVector.magnitude / maxDragDistance);

        // ====================================================
        // กฎนิวตันข้อ 2: F = m * a  →  a = F/m
        // เราคำนวณ Force ก่อน แล้วหา initial velocity จาก impulse
        // ====================================================
        float mass = currentBall.GetComponent<Rigidbody>().mass;
        float forceMagnitude = dragRatio * maxLaunchForce;

        // v = F/m * dt (Impulse approximation, dt=1 frame)
        float speed = forceMagnitude / mass;
        return dragVector.normalized * speed;
    }

    void LaunchBall(Vector3 dragVector)
    {
        if (dragVector.magnitude < 0.1f) return;

        Vector3 launchVelocity = CalculateLaunchVelocity(dragVector);
        Rigidbody rb = currentBall.GetComponent<Rigidbody>();
        rb.linearVelocity = launchVelocity;

        // ตัด reference เพื่อไม่ให้ยิงซ้ำ
        currentBall = null;
    }

    /// <summary>
    /// วาด Trajectory โดยจำลองฟิสิกส์ล่วงหน้า (ไม่รวม gravity จากดาว เพราะซับซ้อน)
    /// </summary>
    void DrawTrajectory(Vector3 startPos, Vector3 startVel)
    {
        trajectoryLine.positionCount = trajectorySteps;
        Vector3 pos = startPos;
        Vector3 vel = startVel;

        for (int i = 0; i < trajectorySteps; i++)
        {
            trajectoryLine.SetPosition(i, pos);
            pos += vel * trajectoryTimeStep;
        }
    }

    void SpawnBall()
    {
        if (ballPrefab == null) return;
        currentBall = Instantiate(ballPrefab, launchPoint.position, Quaternion.identity);
        currentBall.OnDead += OnBallDead;
    }

    void OnBallDead()
    {
        // รอแล้ว Respawn
        Invoke(nameof(RespawnBall), 1.5f);
    }

    void RespawnBall()
    {
        if (currentBall != null)
            Destroy(currentBall.gameObject);
        SpawnBall();
    }
}
