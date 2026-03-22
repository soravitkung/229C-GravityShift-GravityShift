using UnityEngine;

/// <summary>
/// 2D Side View — กล้องอยู่นิ่ง ไม่ต้องติดตามอะไร
/// แค่ตั้ง Orthographic ใน Inspector ก็พอครับ
/// เก็บ script นี้ไว้ในกรณีอยากเพิ่ม Shake effect
/// </summary>
public class CameraFollow : MonoBehaviour
{
    [Header("Screen Shake (optional)")]
    public float shakeDuration = 0.2f;
    public float shakeMagnitude = 0.1f;

    private Vector3 originalPos;

    void Start() => originalPos = transform.position;

    public void SetTarget(Transform t) { }        // ไม่ทำอะไร กล้องอยู่นิ่ง
    public void ResetToLaunchPoint() { }        // ไม่ทำอะไร

    /// <summary>
    /// เรียกเมื่อลูกบอลชนอะไรแรงๆ เพื่อความรู้สึก
    /// </summary>
    public void TriggerShake()
    {
        StopAllCoroutines();
        StartCoroutine(Shake());
    }

    System.Collections.IEnumerator Shake()
    {
        float elapsed = 0f;
        while (elapsed < shakeDuration)
        {
            transform.position = originalPos + (Vector3)Random.insideUnitCircle * shakeMagnitude;
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = originalPos;
    }
}