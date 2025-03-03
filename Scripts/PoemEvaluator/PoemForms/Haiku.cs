using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Haiku : Poem
{
    /* 
     * three unrhyming lines
     * line 1: 5 syllables
     * line 2: 7 syllables
     * line 3: 5 syllables
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

    protected override bool CheckRhyme()
    {
        //Make sure that the poem does not rhyme
        base.CheckRhyme();
        bool isRhyme1 = CMUDictionary.Instance.DoWordsRhyme(GetLastWordInLine(poemLineContent[0]), GetLastWordInLine(poemLineContent[1]));
        bool isRhyme2 = CMUDictionary.Instance.DoWordsRhyme(GetLastWordInLine(poemLineContent[0]), GetLastWordInLine(poemLineContent[2]));
        bool isRhyme3 = CMUDictionary.Instance.DoWordsRhyme(GetLastWordInLine(poemLineContent[1]), GetLastWordInLine(poemLineContent[2]));
        bool isRhyme = isRhyme1 || isRhyme2 || isRhyme3;

        notebookPage.OnRhymeChecked(!isRhyme);

        if (isRhyme) { Debug.LogWarning("Incorrect Poem Structure, Haikus do not Rhyme: lines 1+2 - " + isRhyme1 + ", lines 1+3 - " + isRhyme2 + ", lines 2+3 - " + isRhyme3); }
        else { Debug.Log("Correct Poem Structure, Haikus do not Rhyme"); }
        return !isRhyme;
    }

    protected override bool CheckLength(int wiggleRoom)
    {
        base.CheckLength(wiggleRoom);
        bool isLength = false;
        int[] syllableCounts = new int[poemLineContent.Length];

        //Calculate line lenght for three lines
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

        //Compare lines
        isLength = (syllableCounts[0] == 5 && syllableCounts[1] == 7 && syllableCounts[2] == 5);

        notebookPage.OnLengthChecked(isLength);

        if (isLength) { Debug.Log("Correct Length. Line 1: " + syllableCounts[0] + ", Line 2: " + syllableCounts[1] + ", Line 3: " + syllableCounts[2]); }
        else { Debug.LogWarning("Incorrect Length. Line 1: " + syllableCounts[0] + ", Line 2: " + syllableCounts[1] + ", Line 3: " + syllableCounts[2]); }

        return isLength;
    }

}
