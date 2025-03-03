using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

[RequireComponent(typeof(AudioSource))]
public class OutroScript : MonoBehaviour
{
    public VideoPlayer videoPlayer; // Assign in Inspector
    AudioSource audioSource;
    bool videoFinished = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (videoPlayer == null)
        {
            videoPlayer = GetComponent<VideoPlayer>();
        }
        //Subscribe to event
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += OnVideoFinished;
        }
        else
        {
            Debug.LogError("No VideoPlayer component found!");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && videoFinished)
        {
            Application.Quit();
        }
        if (Input.GetKeyDown(KeyCode.R) && videoFinished)
        {
            SceneManager.LoadScene(0);
        }
    }
    void OnVideoFinished(VideoPlayer vp)
    {
        StartCoroutine(FadeOut(audioSource, 2f));
        videoFinished = true;
    }

    private IEnumerator FadeOut(AudioSource audioSource, float duration)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / duration;
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume; // Reset volume for future use
    }
}
