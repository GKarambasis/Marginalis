using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

public class NotebookPage : MonoBehaviour
{
    public CharacterInfo characterInfo;
    
    [Space(50)]
    [Header("UI Elements - Do Not Touch")]
    [Header("Left Page")]
    [SerializeField] TextMeshProUGUI characterNameText;
    [SerializeField] Image characterPortrait;

    [SerializeField] GameObject Notes;
    [SerializeField] GameObject NotePrefab;
    private GameObject[] instantiatedNotes;

    [SerializeField] Button poemFormButton;

    [SerializeField] Slider[] statSliders;
    [SerializeField] TextMeshProUGUI[] statTexts;
    
    //Private Variables
    int[] stats = new int[5];
    string poemForm;

    [Header("Right Page")]
    public GameObject[] poemLines;
    [SerializeField] GameObject poemLayoutField;

    [SerializeField] GameObject poemLine, poemButton, poemText, poemInput;

    [SerializeField] Button composeButton;

    [Header("Tutorial Notes")]
    [SerializeField] GameObject tutorialNote;
    [SerializeField] GameObject tutorialNoteTextPrefab;
    private TextMeshProUGUI[] instantiatedTutorialNotes;

    // Start is called before the first frame update
    void Start()
    {
        if (characterInfo != null) 
        { 
            //Initialize Portrait
            characterPortrait.sprite = characterInfo.portrait;

            //Initialize Notes
            InitializeNotes();

            //Initialize Stats
            //InitializeStats();

            //Add Poem Script
            AddPoemClassByPoemType(characterInfo.poemType);

            //Initialize Poem Form
            InitializePoemForm();

            //Initialize Poem
            InitializePoem(characterInfo.poem);

            //Initialize tutorial notes
            InitializeTutorialNotes();

            gameObject.SetActive(false);
            //You should probably move this somewhere else
            poemLayoutField.SetActive(false);
        }

    }
    
    //INITIALIZATION METHODS
    public void InitializeName()
    {
        characterNameText.text = characterInfo.characterName;
    }
    private void InitializeNotes()
    {
        instantiatedNotes = new GameObject[characterInfo.notes.Length];
        for (int i = 0; i < characterInfo.notes.Length; i++)
        {
            GameObject notePrefab = Instantiate(NotePrefab, Notes.transform);
            //notePrefab.GetComponent<TextMeshProUGUI>().text = "•" + characterInfo.notes[i];
            instantiatedNotes[i] = notePrefab;
        }
    }
    public void InitializeStats()
    {
        stats[0] = characterInfo.health;
        stats[1] = characterInfo.stength;
        stats[2] = characterInfo.dexterity;
        stats[3] = characterInfo.aether;
        stats[4] = characterInfo.will;
        for (int i = 0; i < statTexts.Length; i++)
        {
            if (stats[i] < 0)
            {
                statTexts[i].gameObject.SetActive(false);
                continue;
            }

            statTexts[i].text = stats[i].ToString();
            statSliders[i].value = stats[i];
        }
    }
    //search for the appropriate form script and attach it to the gameobject
    private void AddPoemClassByPoemType(CharacterInfo.PoemType poemType)
    {
        string className = poemType.ToString();
        //try to get a poemclass Type from the name
        Type poemClass = Type.GetType(className);

        if (poemClass == null)
        {
            Debug.LogWarning("Could not find the class, searching through all assemblies...");
            // Try finding the type in all assemblies
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                poemClass = assembly.GetType(className);
                if (poemClass != null) break;
            }
        }

        if (poemClass != null && typeof(Component).IsAssignableFrom(poemClass))
        {
            gameObject.AddComponent(poemClass);
            //Debug.Log($"{className} added successfully!");
        }
        else
        {
            Debug.LogError($"Component {className} not found or is not a valid Component.");
        }
    }
    private void InitializePoemForm()
    {
        poemForm = characterInfo.poemType.ToString();
        poemFormButton.GetComponentInChildren<TextMeshProUGUI>().text = poemForm;
        poemFormButton.gameObject.SetActive(false);
    }
    private void InitializePoem(string[] Poem)
    {
        poemLines = new GameObject[Poem.Length];

        for (int i = 0; i < Poem.Length; i++)
        {
            InitializePoemLine(Poem[i], i);
        }                
    }
    private void InitializePoemLine(string lineText, int lineIndex)
    {
        GameObject poemLineObject = Instantiate(poemLine, poemLayoutField.transform);
        poemLines[lineIndex] = poemLineObject;

        string[] lineParts = Regex.Split(lineText, @"(\([^)]*\))");
        
        for (int i = 0; i < lineParts.Length; i++)
        { 
            string trimmedLine;
            //Look for i: and r: keywords 
            if (lineParts[i].StartsWith("(") && lineParts[i].EndsWith(")"))
            {
                if (lineParts[i].StartsWith("(b:"))
                {
                    trimmedLine = lineParts[i].Substring(3);
                    trimmedLine = trimmedLine.TrimEnd(')');

                    GameObject button = Instantiate(poemButton, poemLineObject.transform);
                    button.GetComponent<PoemButton>().Initialize(trimmedLine.Split(","));

                    continue;
                }

                if (lineParts[i].Contains("(i:)"))
                {
                    GameObject input = Instantiate(poemInput, poemLineObject.transform);
                    continue;
                }
            }

            if(lineParts[i].EndsWith(" "))
            {
                trimmedLine = lineParts[i].TrimEnd(' ');
                trimmedLine += "<color=#00000000>i</color>";
                GameObject trimmedTextMesh = Instantiate(poemText, poemLineObject.transform);
                trimmedTextMesh.GetComponent<TextMeshProUGUI>().text = trimmedLine;
                continue;
            }
            
            //Instantiate the text object and assign the proper text
            GameObject textMesh = Instantiate(poemText, poemLineObject.transform);
            textMesh.GetComponent<TextMeshProUGUI>().text = lineParts[i];
        }

    }
    private void InitializeTutorialNotes()
    {
        instantiatedTutorialNotes = new TextMeshProUGUI[characterInfo.tutorialNotes.Length];
        for (int i = 0; i < characterInfo.tutorialNotes.Length; i++)
        {
            GameObject prefab = Instantiate(tutorialNoteTextPrefab, tutorialNote.transform);
            prefab.GetComponent<TextMeshProUGUI>().text = "•" + characterInfo.tutorialNotes[i];
            instantiatedTutorialNotes[i] = prefab.GetComponent<TextMeshProUGUI>();
        }
    }

    /*
    public void UpdateNotes(int notesVisible)
    {
        if(notesVisible > instantiatedNotes.Length)
        {
            Debug.LogError("Can't Update Notes because the param notesVisible is higher than the lenght of instantiatedNotes");
            return;
        }

        for (int i = 0; i < notesVisible; i++) 
        {
            instantiatedNotes[i].SetActive(true);
        }
        poemFormButton.gameObject.SetActive(CheckNoteVisibility());
    }
    */
    public bool CheckNoteVisibility()
    {
        int i = 0;

        foreach(GameObject instantiatedNote in instantiatedNotes)
        {
            if (!instantiatedNote.activeInHierarchy) { i++; };
        }

        return i == 0;
    }

    private void UpdateComposeButton(bool poemState)
    {
        composeButton.interactable = poemState;
    }

    public void OnPoemChecked(bool poemState)
    {
        //UpdateComposeButton(poemState);
    }
    public void OnRhymeChecked(bool rhymeState)
    {
        if (!instantiatedTutorialNotes[0]) { return; }
        UpdateTutorialNoteText(instantiatedTutorialNotes[0], rhymeState);
    }
    
    public void OnLengthChecked(bool lengthState)
    {
        if (!instantiatedTutorialNotes[1]) { return; }
        UpdateTutorialNoteText(instantiatedTutorialNotes[1], lengthState);
    }

    private void UpdateTutorialNoteText(TextMeshProUGUI text, bool state)
    {
        if (state) { text.color = Color.black; }
        else { text.color = Color.red; }
    }

    public void EnableNote(int index)
    {
        instantiatedNotes[index].GetComponent<TextMeshProUGUI>().text = "•" + characterInfo.notes[index];

        bool allActive = instantiatedNotes.All(go => !go.GetComponent<TextMeshProUGUI>().text.Contains("???"));
        poemFormButton.gameObject.SetActive(allActive);
        poemLayoutField.SetActive(allActive);
        composeButton.gameObject.SetActive(allActive);
    }
}
