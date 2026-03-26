using UnityEngine;
using UnityEngine.Video;

public class PlayVideo : MonoBehaviour
{
    public VideoPlayer video;

    public void PlayVideoFromButton()
    {
        video.url = "https://html-classic.itch.zone/html/16939308/New%20folder/StreamingAssets/credit.mp4";

        video.Prepare();
        video.prepareCompleted += (vp) =>
        {
            Debug.Log("VIDEO READY");
            vp.Play();
        };

        video.errorReceived += (vp, msg) =>
        {
            Debug.LogError("VIDEO ERROR: " + msg);
        };
    }
}