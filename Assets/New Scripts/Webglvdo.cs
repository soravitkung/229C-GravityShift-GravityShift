using UnityEngine;
using UnityEngine.Video;

public class PlayWebGLVideo : MonoBehaviour
{
    void Start()
    {
        VideoPlayer vp = GetComponent<VideoPlayer>();
        string videoPath = System.IO.Path.Combine(Application.streamingAssetsPath, "credit.mp4");

        // บังคับเปลี่ยน \ เป็น / เพื่อให้บราวเซอร์ค้นหา URL เจอ
        vp.url = videoPath.Replace("\\", "/");

        vp.Play();
    }
}