using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
using TMPro;
using UnityEngine;
using System.Text.RegularExpressions;
using UnityEngine.Windows;
using System.Linq;

public class Poem : MonoBehaviour
{
    //Generic Class that every poemLines inherits from
    [Tooltip("Decides whether the poemLines is expected to rhyme or not")]
    public bool isRhyme;
    [Tooltip("Decides whether the poemLines should be structured")]
    public bool isStructured;

    protected NotebookPage notebookPage;
    

    public string[] poemLineContent;
    char[] punctuation = { '.', ',', '!', '?', ';', ':', '"', '\'' };

    /* Rules to Include
     * Check for rhyming - Done
     * Check number of Syllables
     * Check if line x is the same as line y
     * Check if the line starts with letter X
     */
    

    /// <summary>
    /// Updates the full string lines of the poem
    /// </summary>
    public void UpdatePoemLines()
    {
        if(notebookPage == null) notebookPage = GetComponent<NotebookPage>();

        poemLineContent = new string[notebookPage.poemLines.Length];
        
        for (int i = 0; i < notebookPage.poemLines.Length; i++)
        {
            GameObject[] poemElements = new GameObject[notebookPage.poemLines[i].transform.childCount];
            for(int y = 0; y < poemElements.Length; y++)
            {
                poemElements[y] = notebookPage.poemLines[i].transform.GetChild(y).gameObject;

                TextMeshProUGUI textMeshComponent = poemElements[y].GetComponent<TextMeshProUGUI>();
                if(textMeshComponent == null)
                {
                    textMeshComponent = poemElements[y].GetComponentInChildren<TextMeshProUGUI>(true);
                }

                poemLineContent[i] += textMeshComponent.text;
            }
            poemLineContent[i] = Regex.Replace(poemLineContent[i], "0>.*?</", "><");
            poemLineContent[i] = Regex.Replace(poemLineContent[i], "<.*?>", "");
        }
    }

    /// <summary>
    /// Gets the last word in a long string
    /// </summary>
    protected string GetLastWordInLine(string line)
    {
        string word = line.Split(' ').LastOrDefault();

        word = word.Trim(punctuation);

        return word;
    }

    /// <summary>
    /// Gets all words in a line and trims the punctuation
    /// </summary>
    protected string[] GetAllWordsInLine(string line)
    {
        string[] words = line.Split(" ", System.StringSplitOptions.RemoveEmptyEntries);

        for (int i = 0; i < words.Length; i++)
        {
            words[i] = words[i].Trim(punctuation);
        }

        return words;
    }

    /// <summary>
    /// Checks if the poem meets the structural demands of the poem form
    /// </summary>
    public virtual bool CheckPoem()
    {
        UpdatePoemLines();

        return true;
    }


    protected virtual bool CheckRhyme()
    {
        return true;
    }

    /// <summary>
    /// Checks the syllable length of the poem. 0 wiggleRoom indicates exact number of syllables between lines.
    /// </summary>
    protected virtual bool CheckLength(int wiggleRoom)
    {
        return true;
    }


    /// <summary>
    /// Checks if the provided lines are the same. Can take 2 strings or a string array.
    /// </summary>
    protected virtual bool CheckLines(string line1, string line2)
    {   
        return line1 == line2;
    }
    protected virtual bool CheckLines(string[] lines)
    {
        foreach (string line in lines)
        {
            if(line != lines[0]) {  return false; }
        }
        
        return false;
    }
}
