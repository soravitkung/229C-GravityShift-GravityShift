using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

/// <summary>
/// ติดกับ Canvas ใน Credit Scene
/// แสดงรายชื่อสมาชิก, Score สุดท้าย และ Logo ITI
/// </summary>
public class CreditScene : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI creditsText;
    public Button mainMenuButton;
    public RectTransform creditsContainer;

    [Header("Scroll Settings")]
    [Tooltip("ความเร็วการ Scroll ของ Credits")]
    public float scrollSpeed = 50f;

    [Header("Scene Settings")]
    public string mainMenuSceneName = "MainMenu";

    void Start()
    {
        // แสดง Score สุดท้าย
        int finalScore = PlayerPrefs.GetInt("FinalScore", 0);
        if (finalScoreText != null)
            finalScoreText.text = $"Final Score: {finalScore}";

        // ตั้งค่า Credits Text
        if (creditsText != null)
            creditsText.text = GetCreditsContent();

        // ปุ่มกลับ Main Menu
        mainMenuButton?.onClick.AddListener(() =>
            SceneManager.LoadScene(mainMenuSceneName));
    }

    void Update()
    {
        // Scroll Credits ขึ้นช้าๆ
        if (creditsContainer != null)
        {
            creditsContainer.anchoredPosition +=
                Vector2.up * scrollSpeed * Time.deltaTime;
        }
    }

    string GetCreditsContent()
    {
        return
            "=== GRAVITY SHIFT ===\n\n" +

            "── DEVELOPMENT TEAM ──\n\n" +

            // >>> แก้ข้อมูลสมาชิกตรงนี้ <<<
            "รหัสนักศึกษา: XXXXXXXXX\n" +
            "ชื่อ-นามสกุล: ชื่อ นามสกุล\n" +
            "Section: XX  เลขที่: XX\n" +
            "หน้าที่: Programming, Physics System\n\n" +

            "── ASSETS & TOOLS ──\n\n" +
            "3D Models: Meshy.ai (AI Generated)\n" +
            "Music: Suno AI (AI Generated)\n" +
            "Unity Version: Unity 6\n\n" +

            "── SPECIAL THANKS ──\n\n" +
            "Free Assets จาก Unity Asset Store\n" +
            "ขอบคุณ Asst.Prof. [ชื่ออาจารย์]\n\n" +

            "── PHYSICS USED ──\n\n" +
            "• Newton's Law of Universal Gravitation\n" +
            "  F = G·m₁·m₂ / r²\n\n" +
            "• Air Resistance Theory\n" +
            "  F_drag = ½·Cd·ρ·A·v²\n\n" +
            "• Unity Physics 3D\n" +
            "  Collision, Trigger Detection\n\n" +

            "────────────────────\n\n" +
            "คณะเทคโนโลยีสารสนเทศและนวัตกรรม\n" +
            "Bangkok University\n\n" +

            "© 2025 Gravity Shift Team\n" +
            "GI204 Game Physics Project";
    }
}
