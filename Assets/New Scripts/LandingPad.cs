using UnityEngine;
using UnityEngine.SceneManagement;

public class LandingPad : MonoBehaviour
{
    public float maxLandingVelocity = 5f; // ความเร็วสูงสุดที่ลงจอดได้ปลอดภัย

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // เช็คความเร็วขณะกระทบพื้น 
            if (collision.relativeVelocity.magnitude < maxLandingVelocity)
            {
                Debug.Log("Landing Successful!");
                // เปลี่ยนไปยังซีน Credit หรือด่านถัดไป [cite: 5, 13]
            }
            else
            {
                Debug.Log("Rocket Exploded! Too Fast.");
                // Reset ฉากใหม่
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }
}