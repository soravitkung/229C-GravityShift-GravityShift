using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class CreditScene : MonoBehaviour
{
    [Header("UI References")]
    public RectTransform creditsContainer;
    public TextMeshProUGUI finalScoreText;
    public Button mainMenuButton;

    [Header("Scroll Settings")]
    public float scrollSpeed = 60f;   // ความเร็ว
    public float startDelay = 1.5f;  // รอก่อนเริ่มเลื่อน
    public float endOffset = 200f;  // เผื่อระยะตอนจบ (ถ้ายังเลื่อนไม่สุดให้เพิ่มค่านี้)

    [Header("Scene")]
    public string mainMenuScene = "MainMenu";

    private bool isScrolling = false;
    private float startY;
    private float endY;

    void Start()
    {
        // แสดง Score
        int score = PlayerPrefs.GetInt("FinalScore", 0);
        if (finalScoreText != null)
            finalScoreText.text = $"FINAL SCORE: {score:D6}";

        // ปุ่ม Main Menu
        mainMenuButton?.onClick.AddListener(() =>
            SceneManager.LoadScene(mainMenuScene));

        // เริ่ม Coroutine จัดการการเลื่อน
        StartCoroutine(SetupAndScroll());
    }

    IEnumerator SetupAndScroll()
    {
        // 1. รอ 1 เฟรม เพื่อให้ Content Size Fitter ยืดขนาดข้อความเสร็จก่อน
        yield return null;

        if (creditsContainer != null)
        {
            // 2. หาความสูงของหน้าจอ (อิงจากกรอบของ Canvas ไม่ใช่พิกเซลจอ)
            RectTransform parentRect = creditsContainer.parent.GetComponent<RectTransform>();
            float containerHeight = creditsContainer.rect.height;
            float screenHeight = parentRect != null ? parentRect.rect.height : Screen.height;

            // 3. คำนวณจุดเริ่ม (อยู่ขอบล่างพอดี) และจุดจบ (ทะลุขอบบนไปแล้ว)
            startY = -(screenHeight * 0.5f) - (containerHeight * 0.5f);
            endY = (screenHeight * 0.5f) + (containerHeight * 0.5f) + endOffset;

            // เซ็ตตำแหน่งเริ่มต้นรอไว้
            creditsContainer.anchoredPosition = new Vector2(0, startY);
        }

        // 4. รอเวลา Start Delay ตามที่ตั้งไว้ แล้วค่อยสั่งให้ขยับ
        yield return new WaitForSeconds(startDelay);
        isScrolling = true;
    }

    void Update()
    {
        if (!isScrolling || creditsContainer == null) return;

        // เลื่อนขึ้นไปเรื่อยๆ
        Vector2 pos = creditsContainer.anchoredPosition;
        pos.y += scrollSpeed * Time.deltaTime;
        creditsContainer.anchoredPosition = pos;

        // ถ้าเลื่อนถึงจุดหมายแล้ว ให้หยุด
        if (pos.y >= endY)
        {
            isScrolling = false;
            creditsContainer.anchoredPosition = new Vector2(0, endY);
        }
    }
}