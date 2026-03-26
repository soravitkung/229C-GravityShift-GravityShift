using UnityEngine;

public class HTMLAutoOpener : MonoBehaviour
{
    void Start()
    {
        // §ÈπÀ“‰ø≈Ï credits.html „π‚ø≈‡¥Õ√Ï StreamingAssets
        string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, "credits.html");

        //  —Ëß‡ª‘¥‰ø≈Ï∑—π∑’∑’Ë Scene ‚À≈¥‡ √Á®·≈– §√‘ªµÏ‡√‘Ë¡∑”ß“π
        Application.OpenURL("file://" + filePath);
    }
}