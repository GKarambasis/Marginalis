using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class AdventurerController : MonoBehaviour
{
    public CharacterInfo characterInfo;
    [Tooltip("For NPCs that have their dedicated page.")]
    public bool hasNotebookPage = true;

    //Components
    Animator myAnimator;
    TextMeshProUGUI dialogueText;
    GameObject canvas;
    AudioSource audioSource;
    [SerializeField] Image textPanel;
    [SerializeField] Image emotionPanel;

    //Private Variables
    NotebookPage myNotebookPage;
    bool mouseHovering;
    bool isTalking = false;
    Coroutine currentCoroutine;
    string lastDialogueLine;

    //Dialogue
    List<string> adventurerDialogueLines = new List<string>();


    private void InitializeVariables()
    {
        myAnimator = GetComponent<Animator>();
        canvas = GetComponentInChildren<Canvas>(true).gameObject;
        dialogueText = GetComponentInChildren<TextMeshProUGUI>(true);
        audioSource = GetComponent<AudioSource>();
        if(characterInfo.voiceSFX != null) { audioSource.clip = characterInfo.voiceSFX; }

        foreach(string line in characterInfo.dialogueLines)
        {
            adventurerDialogueLines.Add(line);
        }


    }

    private void Start()
    {
        InitializeVariables();
        if (hasNotebookPage){ FindNotebookPage(); }
    }


    private void FindNotebookPage()
    {
        foreach (NotebookPage notebook in FindObjectsOfType<NotebookPage>(true))
        {
            if (notebook.characterInfo == characterInfo)
            {
                myNotebookPage = notebook;
                Debug.Log("Successfully found notebook page for character info: " + characterInfo.characterName);
                return;
            }
        }
        Debug.LogError("Could not find any notebook pages with the character info: " + characterInfo.characterName);
    }

    private void Update()
    {
        OnClick();

        if (myNotebookPage == null)
        {
            FindNotebookPage();
        }
    }

    private void OnClick()
    {
        if (mouseHovering && Input.GetMouseButtonDown(0) && emotionPanel.gameObject.activeInHierarchy && !isTalking)
        {
            ToggleConversation(true);
            PlayDialogue();
        }
        else if (mouseHovering && Input.GetMouseButtonDown(0) && !emotionPanel.gameObject.activeInHierarchy && !isTalking)
        {
            ToggleConversation(false);
        }
        else if (mouseHovering && Input.GetMouseButtonDown(0) && isTalking)
        {
            isTalking = false;
            StopCoroutine(currentCoroutine);
            LayoutRebuilder.ForceRebuildLayoutImmediate(canvas.GetComponent<RectTransform>());

            dialogueText.text = lastDialogueLine;
            StartCoroutine(RebuildLayout(canvas.GetComponent<RectTransform>()));
        }
    }
    IEnumerator RebuildLayout(RectTransform transform)
    {
        yield return new WaitForSeconds(0.1f);
        LayoutRebuilder.ForceRebuildLayoutImmediate(transform);
    }

    private void OnMouseEnter()
    {
        mouseHovering = true;
    }
    private void OnMouseExit()
    {
        mouseHovering = false;
    }

    private void PlayDialogue()
    {
        if (adventurerDialogueLines.Count < 1) 
        { 
            Debug.Log("Out of Lines");
            if (characterInfo.EmotionSprite != null) { emotionPanel.sprite = characterInfo.EmotionSprite; }
            ToggleConversation(false);
            return; 
        }

        isTalking = true;

        dialogueText.text = "";

        StartCoroutine(PlayDialogueCo());
    }


    IEnumerator PlayDialogueCo()
    {
        //Reset Cavas Size
        textPanel.color = Color.clear;
        LayoutRebuilder.ForceRebuildLayoutImmediate(canvas.GetComponent<RectTransform>());
        yield return new WaitForSeconds(0.1f);
        
        textPanel.color = Color.white;
        //Process the text
        string dialogueLine = ProcessText(adventurerDialogueLines[0]);
        lastDialogueLine = dialogueLine;
        currentCoroutine = StartCoroutine(TypeWriterEffect(dialogueText, dialogueLine, 0.05f, canvas.GetComponent<RectTransform>()));
        
        if (adventurerDialogueLines.Count != 0) { adventurerDialogueLines.RemoveAt(0); }
    }


    /// <summary>
    /// Prints out the text with a typewritter effect
    /// </summary>
    /// <returns></returns>
    private IEnumerator TypeWriterEffect(TextMeshProUGUI responseText, string fullText, float delay, RectTransform transformUpdater)
    {
        responseText.text = "";

        string[] textWords = fullText.Split(' ');
        foreach (string word in textWords) 
        {
            if (PowerWordDictionary.Instance.WordExists(word))
            {
                string[] splitTexts = fullText.Split(word);
                foreach (char c in splitTexts[0])
                {
                    responseText.text += c;
                    if (audioSource.clip) { audioSource.Play(); }
                    LayoutRebuilder.ForceRebuildLayoutImmediate(transformUpdater);
                    yield return new WaitForSeconds(delay);
                }

                switch (PowerWordDictionary.Instance.GetWordKey(word).ToLower())
                {
                    case "health":
                        foreach (char c in word)
                        {
                            responseText.text += "<color=green>" + c + "</color>";
                            if (audioSource.clip) { audioSource.Play(); }
                            LayoutRebuilder.ForceRebuildLayoutImmediate(transformUpdater);
                            yield return new WaitForSeconds(delay);
                        }
                        break;

                    case "strength":
                        foreach (char c in word)
                        {
                            responseText.text += "<color=red>" + c + "</color>";
                            if (audioSource.clip) { audioSource.Play(); }
                            LayoutRebuilder.ForceRebuildLayoutImmediate(transformUpdater);
                            yield return new WaitForSeconds(delay);
                        }
                        break;

                    case "dexterity":
                        foreach (char c in word)
                        {
                            responseText.text += "<color=blue>" + c + "</color>";
                            if (audioSource.clip) { audioSource.Play(); }
                            LayoutRebuilder.ForceRebuildLayoutImmediate(transformUpdater);
                            yield return new WaitForSeconds(delay);
                        }
                        break;

                    case "aether":
                        foreach (char c in word)
                        {
                            responseText.text += "<color=purple>" + c + "</color>";
                            if (audioSource.clip) { audioSource.Play(); }
                            LayoutRebuilder.ForceRebuildLayoutImmediate(transformUpdater);
                            yield return new WaitForSeconds(delay);
                        }
                        break;

                    case "will":
                        foreach (char c in word)
                        {
                            responseText.text += "<color=yellow>" + c + "</color>";
                            if (audioSource.clip) { audioSource.Play(); }
                            LayoutRebuilder.ForceRebuildLayoutImmediate(transformUpdater);
                            yield return new WaitForSeconds(delay);
                        }
                        break;

                    default:
                        foreach (char c in word)
                        {
                            responseText.text += c;
                            if (audioSource.clip) { audioSource.Play(); }
                            LayoutRebuilder.ForceRebuildLayoutImmediate(transformUpdater);
                            yield return new WaitForSeconds(delay);
                        }
                        break;

                }

                foreach (char c in splitTexts[1])
                {
                    responseText.text += c;
                    if (audioSource.clip) { audioSource.Play(); }
                    LayoutRebuilder.ForceRebuildLayoutImmediate(transformUpdater);
                    yield return new WaitForSeconds(delay);
                }
                isTalking = false;
                yield break;
            }
        }
         
        foreach (char c in fullText)
        {
            responseText.text += c;
            if (audioSource.clip) { audioSource.Play(); }
            LayoutRebuilder.ForceRebuildLayoutImmediate(transformUpdater);
            yield return new WaitForSeconds(delay);
        }

        isTalking = false;
    }

    /// <summary>
    /// Toggles the EmotionPanel and the Conversation Panel
    /// </summary>
    /// <param name="state"></param>
    void ToggleConversation(bool state)
    {
        emotionPanel.gameObject.SetActive(!state);
        textPanel.gameObject.SetActive(state);   
    }


    string ProcessText(string text)
    {
        bool notificationTriggered = false;
        string updatedText = text;
        
        if (updatedText.Contains(characterInfo.characterName))
        {
            Debug.Log("ProcessText: Detected Adventurer Name");
            myNotebookPage.InitializeName();
            notificationTriggered = true;
        }

        // Process (n:<id>) for Notes
        MatchCollection noteMatches = Regex.Matches(text, @"\(n:(\d+)\)");
        foreach (Match match in noteMatches)
        {
            if (int.TryParse(match.Groups[1].Value, out int noteID))
            {
                myNotebookPage.EnableNote(noteID);
                notificationTriggered = true;
            }
            updatedText = updatedText.Replace(match.Value, "").Trim();
        }

        // Process (s:) for Stats
        MatchCollection statMatches = Regex.Matches(text, @"\(s:\)");
        if (statMatches.Count > 0)
        {
            myNotebookPage.InitializeStats();
            notificationTriggered = true;
            updatedText = updatedText.Replace("(s:)", "").Trim();
        }

        if (notificationTriggered)
            GameManager.Instance.EnableNotification();

        return updatedText;
    }
}
