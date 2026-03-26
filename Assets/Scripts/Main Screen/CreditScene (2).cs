using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// ติดกับ Canvas ใน Credit Scene
/// ข้อความ Scroll ขึ้นช้าๆ แบบ Sci-fi
/// </summary>
public class CreditScene : MonoBehaviour
{
    [Header("UI References")]
    public RectTransform creditsContainer;  // Panel ที่มีข้อความทั้งหมด
    public TextMeshProUGUI finalScoreText;
    public Button mainMenuButton;

    [Header("Scroll Settings")]
    public float scrollSpeed   = 60f;   // pixel/sec
    public float startDelay    = 1.5f;  // รอก่อน scroll

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

        // ตั้งตำแหน่งเริ่มต้น (อยู่ล่างสุดของจอ)
        if (creditsContainer != null)
        {
            startY = -Screen.height * 0.5f - creditsContainer.rect.height * 0.5f;
            endY   =  Screen.height * 0.5f + creditsContainer.rect.height * 0.5f;
            creditsContainer.anchoredPosition = new Vector2(0, startY);
        }

        StartCoroutine(StartScroll());
    }

    IEnumerator StartScroll()
    {
        yield return new WaitForSeconds(startDelay);
        isScrolling = true;
    }

    void Update()
    {
        if (!isScrolling || creditsContainer == null) return;

        Vector2 pos = creditsContainer.anchoredPosition;
        pos.y += scrollSpeed * Time.deltaTime;
        creditsContainer.anchoredPosition = pos;

        // ถึงท้ายแล้ว หยุด
        if (pos.y >= endY)
        {
            isScrolling = false;
            creditsContainer.anchoredPosition = new Vector2(0, endY);
        }
    }
}
