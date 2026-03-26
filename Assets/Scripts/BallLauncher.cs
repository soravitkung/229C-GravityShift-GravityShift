using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 2D Side View BallLauncher
/// - Orthographic Camera
/// - Mouse World Position ตรงตัวเลย ไม่ต้อง Raycast Plane
/// - ลาก Slingshot style แล้วปล่อยยิง
/// </summary>
public class BallLauncher : MonoBehaviour
{
    [Header("Ball Reference")]
    public BallController ballPrefab;
    public Transform launchPoint;

    [Header("Launch Settings")]
    public float maxLaunchForce = 15f;
    public float maxDragDistance = 3f;

    [Header("Trajectory Preview")]
    public LineRenderer trajectoryLine;
    public int trajectorySteps = 40;
    public float trajectoryTimeStep = 0.05f;

    [Header("Ball Timeout")]
    public float ballTimeout = 5f;

    private enum LaunchState { WaitingForInput, ReadyToShoot, Launched, WaitingRespawn }
    private LaunchState state = LaunchState.WaitingForInput;

    private BallController currentBall;
    private Vector3 dragStartWorld;
    private bool isDragging = false;
    private float launchTimer = 0f;
    private Camera mainCam;
    private Mouse mouse;

    void Start()
    {
        mainCam = Camera.main;
        mouse = Mouse.current;
        SpawnBall();
    }

    void Update()
    {
        if (mouse == null) mouse = Mouse.current;

        switch (state)
        {
            case LaunchState.ReadyToShoot:
                HandleShootInput();
                break;
            case LaunchState.Launched:
                launchTimer += Time.deltaTime;
                if (launchTimer >= ballTimeout)
                    StartRespawn();
                break;
        }
    }

    void HandleShootInput()
    {
        if (currentBall == null || mouse == null) return;

        if (mouse.leftButton.wasPressedThisFrame)
        {
            dragStartWorld = GetMouseWorldPos();
            isDragging = true;
        }

        if (mouse.leftButton.isPressed && isDragging)
        {
            Vector3 dragVec = dragStartWorld - GetMouseWorldPos();
            dragVec = ClampDrag(dragVec);
            DrawTrajectory(launchPoint.position, dragVec * maxLaunchForce);

            // แสดงลูกบอลตามนิ้ว (Slingshot feel)
            Vector3 ballPos = launchPoint.position - dragVec;
            ballPos.z = 0;
            currentBall.transform.position = ballPos;
        }

        if (mouse.leftButton.wasReleasedThisFrame && isDragging)
        {
            isDragging = false;
            if (trajectoryLine != null) trajectoryLine.positionCount = 0;

            Vector3 dragVec = dragStartWorld - GetMouseWorldPos();
            dragVec = ClampDrag(dragVec);

            if (dragVec.magnitude < 0.1f)
            {
                // drag น้อยเกินไป reset ลูกบอลกลับ
                currentBall.transform.position = launchPoint.position;
                return;
            }

            LaunchBall(dragVec);
        }
    }

    /// <summary>
    /// Orthographic: ScreenToWorldPoint ตรงตัวเลย ไม่ต้อง Raycast
    /// </summary>
    Vector3 GetMouseWorldPos()
    {
        Vector3 screenPos = mouse.position.ReadValue();
        screenPos.z = Mathf.Abs(mainCam.transform.position.z);
        Vector3 worldPos = mainCam.ScreenToWorldPoint(screenPos);
        worldPos.z = 0f;
        return worldPos;
    }

    Vector3 ClampDrag(Vector3 drag)
    {
        if (drag.magnitude > maxDragDistance)
            drag = drag.normalized * maxDragDistance;
        return drag;
    }

    void LaunchBall(Vector3 dragVec)
    {
        // ====================================================
        // กฎนิวตันข้อ 2: F = m * a
        // คำนวณ Force จาก drag แล้ว AddForce
        // ====================================================
        Rigidbody rb = currentBall.GetComponent<Rigidbody>();
        float mass = rb.mass;
        Vector3 force = dragVec * maxLaunchForce * mass;
        rb.linearVelocity = Vector3.zero;
        rb.AddForce(force, ForceMode.Impulse);

        currentBall.transform.position = launchPoint.position - dragVec;
        currentBall = null;
        launchTimer = 0f;
        state = LaunchState.Launched;
    }

    /// <summary>
    /// วาด Trajectory โดยจำลอง projectile motion
    /// รวม Gravity ด้วย ทำให้เส้นโค้งถูกต้อง
    /// </summary>
    void DrawTrajectory(Vector3 startPos, Vector3 startVel)
    {
        if (trajectoryLine == null) return;
        trajectoryLine.positionCount = trajectorySteps;

        Vector3 pos = startPos;
        Vector3 vel = startVel;
        Vector3 gravity = Physics.gravity;

        for (int i = 0; i < trajectorySteps; i++)
        {
            trajectoryLine.SetPosition(i, pos);
            vel += gravity * trajectoryTimeStep;
            pos += vel * trajectoryTimeStep;
        }
    }

    void SpawnBall()
    {
        if (ballPrefab == null)
        {
            Debug.LogWarning("BallLauncher: ยังไม่ได้ลาก BallPrefab!");
            return;
        }

        // ใช้ rotation ของ Prefab แทน Quaternion.identity
        currentBall = Instantiate(ballPrefab, launchPoint.position, ballPrefab.transform.rotation);
        currentBall.OnDead += OnBallDead;
        isDragging = false;

        state = LaunchState.WaitingForInput;
        Invoke(nameof(SetReadyToShoot), 0.1f);
    }

    void SetReadyToShoot() => state = LaunchState.ReadyToShoot;

    void OnBallDead()
    {
        state = LaunchState.WaitingRespawn;
        Invoke(nameof(StartRespawn), 1.5f);
    }

    void StartRespawn()
    {
        CancelInvoke(nameof(StartRespawn));
        if (currentBall != null) { Destroy(currentBall.gameObject); currentBall = null; }
        GameObject old = GameObject.FindWithTag("Ball");
        if (old != null) Destroy(old);
        launchTimer = 0f;
        Invoke(nameof(SpawnBall), 0.05f);
    }
}