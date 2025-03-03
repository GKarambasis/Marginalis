using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPoemFormInfo", menuName = "ScriptableObjects/PoemForms")]
public class PoemInfo : ScriptableObject
{
    [Header("Poem Info")]
    public CharacterInfo.PoemType poemType;

    [Space(10)]
    [Tooltip("The text that is going to show up when the player clicks on the poem form (e.g. 2 rhyming lines).")]
    [TextArea(1,5)]
    public string[] stickyNoteRules;
}
