using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Notebook Information")]
    [SerializeField] GameObject Notebook;
    [SerializeField] GameObject NotebookButton;
    [SerializeField] GameObject buttonNotification;
    [SerializeField] GameObject recitingPanel;
    

    [Header("Audio Components")]
    [SerializeField] AudioClipManager audioClipManager;
    [SerializeField] AudioVisualizer visualizer;
    public AudioSource bgmAudioSource;
    public AudioSource sfxAudioSource;

    TimelineManager timelineManager;

    [Header("Debug")]
    public bool controlsDisabled = false;

    public string[] composedPoemLines;

    public enum BGMType
    {
        Default,
        Reciting,
        Night,
        None
    }

    public enum SFXType
    {
        OpenBook,
        CloseBook,
        TurnPage,
        WordFlip,
        ComposeButton
    }

    private BGMType musicType;
    private SFXType sfxType;

    //Singleton
    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        timelineManager = FindObjectOfType<TimelineManager>();
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keeps the object alive between scenes
        }
        else
        {
            Destroy(gameObject); // Ensures only one instance exists
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !controlsDisabled)
        {
            ToggleNoteBook();
        }
    }

    public void ToggleNoteBook()
    {
        DisableNotification();
        Notebook.SetActive(Notebook.activeSelf ? false : true);

        //SFX
        if(audioClipManager.closeBook == null || audioClipManager.openBook == null) { return; }
        sfxAudioSource.clip = Notebook.activeSelf ? audioClipManager.closeBook : audioClipManager.openBook;
        sfxAudioSource.Play();
    }

    public void PlayPoem(string[] poem)
    {
        StartCoroutine(PlayPoemCo(poem));
    }
    public IEnumerator PlayPoemCo(string[] poem)
    {
        composedPoemLines = poem;
        yield return new WaitForSeconds(0.5f);
 
        //Start Playing Music
        ToggleBGM(BGMType.Reciting);
        recitingPanel.SetActive(true);
        visualizer.activate = true;

        timelineManager.PlayNextCutscene();
        

        Debug.LogWarning("Testing");

        yield return new WaitForSeconds((float)timelineManager.playableDirector.playableAsset.duration);
        Debug.LogWarning("Testing 2");
        ToggleBGM(BGMType.None);
        recitingPanel.SetActive(false);

        yield return new WaitForSeconds(1f);
        visualizer.textMesh.text = "";
        timelineManager.PlayNextCutscene();

    }

    public void EnableControls()
    {
        controlsDisabled = false;
        NotebookButton.SetActive(true);
    }
    public void DisableControls()
    {
        controlsDisabled = true;
        Notebook.SetActive(false);
        NotebookButton.SetActive(false);
    }

    public void EnableNotification()
    {
        buttonNotification.SetActive(true);
    }
    public void DisableNotification()
    {
        buttonNotification.SetActive(false);
    }

    //AUDIO SECTION
    public void ToggleBGM(BGMType type)
    {
        switch (type)
        {
            case BGMType.Default:
                if (audioClipManager.defaultBGM == null) { return; }
                bgmAudioSource.clip = audioClipManager.defaultBGM;
                bgmAudioSource.Play();
                break;

            case BGMType.Reciting:
                if (audioClipManager.recitingBGM == null) { return; }
                bgmAudioSource.clip = audioClipManager.recitingBGM[UnityEngine.Random.Range(0, audioClipManager.recitingBGM.Length)];
                bgmAudioSource.Play();
                break;
            
            case BGMType.Night:
                if (audioClipManager.nightBGM == null) { return; }
                bgmAudioSource.clip = audioClipManager.nightBGM;
                bgmAudioSource.Play();
                break;

            case BGMType.None:
                StartCoroutine(FadeOut(bgmAudioSource, 2f));
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// Fade Out an Audio Source over x amount of time.
    /// </summary>
    /// <param name="audioSource"></param>
    /// <param name="duration"></param>
    /// <returns></returns>
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

    private void OnClick_PlaySFX(SFXType type)
    {
        switch (type)
        {
            case SFXType.OpenBook:
                if (audioClipManager.openBook == null) { return; }
                sfxAudioSource.clip = audioClipManager.openBook;
                sfxAudioSource.Play();
                break;

            case SFXType.CloseBook:
                if (audioClipManager.closeBook == null) { return; }
                sfxAudioSource.clip = audioClipManager.closeBook;
                sfxAudioSource.Play();
                break;

            case SFXType.TurnPage:
                if (audioClipManager.turnPage == null) { return; }
                sfxAudioSource.clip = audioClipManager.turnPage;
                sfxAudioSource.Play();
                break;

            case SFXType.WordFlip:
                if (audioClipManager.wordFlip == null) { return; }
                sfxAudioSource.clip = audioClipManager.wordFlip[UnityEngine.Random.Range(0, audioClipManager.wordFlip.Length)];
                sfxAudioSource.Play();
                break;

            case SFXType.ComposeButton:
                if (audioClipManager.composeButton == null) { return; }
                sfxAudioSource.clip = audioClipManager.composeButton;
                sfxAudioSource.Play();
                break;

            default:
                sfxAudioSource.Stop();
                break;
        }
    }

    public void OnClickPlay_TurnPage()
    {
        OnClick_PlaySFX(SFXType.TurnPage);
    }
    public void OnClickPlay_OpenBook()
    {
        OnClick_PlaySFX(SFXType.OpenBook);
    }
    public void OnClickPlay_CloseBook()
    {
        OnClick_PlaySFX(SFXType.CloseBook);
    }
    public void OnClickPlay_WordFlip()
    {
        OnClick_PlaySFX(SFXType.WordFlip);
    }
    public void OnClickPlay_ComposeButton()
    {
        OnClick_PlaySFX(SFXType.ComposeButton);
    }


    //On Scene Load
    private void OnLevelWasLoaded(int level)
    {
        timelineManager = FindObjectOfType<TimelineManager>();
    }

}
