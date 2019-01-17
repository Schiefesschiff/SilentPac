using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class IntroManager : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    private void Start()
    {
        //videoPlayer = gameObject.AddComponent<VideoPlayer>();
        videoPlayer.loopPointReached += EndReached; //adds the method EndReached to the event loopPointReached
    }

    private void EndReached(VideoPlayer vp)
    {
        Debug.Log("Loop point reached.");
        //videoPlayer.loopPointReached -= EndReached; //removes the method EndReached from the event loopPointReached to prevent memory leaks
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);        
    }
}
