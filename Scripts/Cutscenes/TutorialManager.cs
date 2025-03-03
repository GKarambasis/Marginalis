using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class TutorialManager : MonoBehaviour
{
    public GameObject startPanel;
    public PlayableDirector playableDirector;

    public VideoPlayer videoPlayer; // Assign in Inspector

    TimelineManager timelineManager;

    bool videoFinished = false;

    private void Awake()
    {
        timelineManager = FindObjectOfType<TimelineManager>();
        if (PlayerPrefs.HasKey("Level"))
        {
            SceneManager.LoadScene(PlayerPrefs.GetInt("Level"));
        }

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
        if (Input.anyKeyDown && videoFinished)
        {
            startPanel.SetActive(false);

            timelineManager.PlayNextCutscene();
            gameObject.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Escape) && !videoFinished)
        {
            videoPlayer.time = videoPlayer.length - 1f;
        }

        if(startPanel == null) { startPanel = GameObject.Find("StartPanel"); }
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        videoFinished = true;
        GetComponent<RawImage>().enabled = false;
        StartCoroutine(FadeOut(GetComponent<AudioSource>(), 2f));
        playableDirector.Play();
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
