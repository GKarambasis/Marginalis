using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TimelineManager : MonoBehaviour
{
    [SerializeField] CutsceneCollection[] cutsceneCollections;
    int level = 0;
    int cutSceneIndex = 0;
    public bool isPlaying;

    public PlayableDirector playableDirector;

    Coroutine currentCoroutine;

    private void Awake()
    {        
        playableDirector = GetComponent<PlayableDirector>();

        level = SceneManager.GetActiveScene().buildIndex;
    }

    private void Start()
    {
        //PlayNextCutscene();
    }

    private void Update()
    {
        SkipCutscene();
    }

    private void UpdatePlayState()
    {
        switch (playableDirector.state)
        {
            case PlayState.Playing:
                isPlaying = true; 
                break;

            case PlayState.Paused:
                isPlaying = false; 
                break;
            
            default:
                isPlaying = false ; 
                break;
        }
        Debug.Log("Updated Director Play State: " + playableDirector.state.ToString());
    }

    public void PlayNextCutscene()
    {
        //Check if the cutscene collection is Out Of Cutscenes
        if (level >= cutsceneCollections.Length) 
        { 
            Debug.Log("Out Of Cutscenes"); 
            return; 
        }
        
        //Play Next Level CutsceneCollection if there is no more cutscenes
        if (cutsceneCollections[level].GetTimeline(cutSceneIndex) == null ) 
        { 
            Debug.Log("No More Cutscenes, Playing next level Cutscenes"); 
            LoadNextLevel(); 
            return; 
        }

        playableDirector.playableAsset = cutsceneCollections[level].GetTimeline(cutSceneIndex);
        Debug.Log("New Playable Asset: " + cutsceneCollections[level].GetTimeline(cutSceneIndex).name);
        cutSceneIndex++;

        currentCoroutine = StartCoroutine(DisableControlsCoroutine(playableDirector.playableAsset.duration));

        playableDirector.Play();

        UpdatePlayState();
    }

    private void LoadNextLevel()
    {
        level++;
        cutSceneIndex = 0;
        if (SceneManager.sceneCountInBuildSettings > (level)) 
        { 
            Debug.Log("Now Playing: Level " + level.ToString());
            SceneManager.LoadScene(level); 
        }
    }

    private IEnumerator DisableControlsCoroutine(double seconds)
    {
        GameManager.Instance.DisableControls();
        yield return new WaitForSeconds((float)seconds);
        GameManager.Instance.EnableControls();
        
        playableDirector.Evaluate();
        UpdatePlayState();
    }

    private void SkipCutscene()
    {
        if (isPlaying && Input.GetKeyDown(KeyCode.Escape))
        {
            playableDirector.time = playableDirector.playableAsset.duration;
            playableDirector.Evaluate();
            UpdatePlayState();

            if (currentCoroutine != null) { StopCoroutine(currentCoroutine); }
            GameManager.Instance.EnableControls();
        }
    }
}
