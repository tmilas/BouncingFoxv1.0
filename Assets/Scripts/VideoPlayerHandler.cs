using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoPlayerHandler : MonoBehaviour
{
    VideoPlayer videoPlayer;

    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();

        
        videoPlayer.loopPointReached += EndReached;

        videoPlayer.transform.position.Scale(new Vector3(0.5f, 0.5f, 0.5f));

        videoPlayer.Play();
    }

    void EndReached(UnityEngine.Video.VideoPlayer vp)
    {
        vp.gameObject.SetActive(false);

        FindObjectOfType<OptionsUIHandler>().NextHelpScreen();
    }
}
