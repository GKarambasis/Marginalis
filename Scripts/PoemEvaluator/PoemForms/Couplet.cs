using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Couplet : Poem
{
    /*
     * two rhyming lines
     * lines of similar length (i.e. similar number of syllables)
     */

    void Start()
    {
        Invoke("CheckPoem", 0.5f);
    }

    public override bool CheckPoem()
    {
        base.CheckPoem();
        bool correctStructure = CheckRhyme() && CheckLength(0);

        notebookPage.OnPoemChecked(correctStructure);
        return correctStructure;
    }

    //Check if the lines rhyme
    protected override bool CheckRhyme()
    {
        base.CheckRhyme();
        bool isRhyme = CMUDictionary.Instance.DoWordsRhyme(GetLastWordInLine(poemLineContent[0]), GetLastWordInLine(poemLineContent[1]));
        notebookPage.OnRhymeChecked(isRhyme);
        if (!isRhyme) { Debug.LogWarning("Incorrect Poem Structure, " + GetLastWordInLine(poemLineContent[0]) + " does not rhyme with " + GetLastWordInLine(poemLineContent[1])); }
        else { Debug.Log("Correct Poem Structure, " + GetLastWordInLine(poemLineContent[0]) + " rhymes with " + GetLastWordInLine(poemLineContent[1])); }
        return isRhyme;
    }
    //Check if the length is similar 
    protected override bool CheckLength(int wiggleRoom)
    {
        base.CheckLength(wiggleRoom);
        bool isLength = false;
        int[] syllableCounts = new int[poemLineContent.Length];
        
        //Calculate line lenght for both lines
        for (int i = 0; i < poemLineContent.Length; i++)
        {
            string[] words = GetAllWordsInLine(poemLineContent[i]);
            int syllableCount = 0;

            for (int y = 0; y < words.Length; y++)
            {
                if (CMUDictionary.Instance.GetPhonemes(words[y]).Length == 0) { continue; }
                string phonemes = string.Join("", CMUDictionary.Instance.GetPhonemes(words[y]));
                //Counts the stressed syllables for each word in the line
                syllableCount += phonemes.Count(c => char.IsDigit(c));
            }

            syllableCounts[i] = syllableCount;
        }

        //line 1 14
        //line 2 12
        //wiggleroom 3
        //Compare lines
        isLength = (syllableCounts[1] >= (syllableCounts[0] - wiggleRoom) && syllableCounts[1] <= (syllableCounts[0] + wiggleRoom));
        notebookPage.OnLengthChecked(isLength);

        if (isLength) { Debug.Log("Correct Length. Line 1: " + syllableCounts[0] + ", Line 2: " + syllableCounts[1]); }
        else { Debug.LogWarning("Incorrect Length. Line 1: " + syllableCounts[0] + ", Line 2: " + syllableCounts[1]); }

        return isLength;
    }
}
