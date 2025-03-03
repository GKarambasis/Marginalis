using System.Collections;
using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacterInfo", menuName = "ScriptableObjects/Character")]
public class CharacterInfo : ScriptableObject
{
    [Header("Character Details")]
    public string characterName;

    public Sprite portrait;

    [Header("Starting Stats (-1 for disabled stat)")]
    [Range(-1, 10)]
    public int health;
    [Range(-1, 10)]
    public int stength, dexterity, aether, will;
  
    [Header("Conversation")]
    [Tooltip("The default emotion text bubble hovering over the adventurer")]
    public Sprite EmotionSprite;
    [Space(10)]
    [TextArea(5, 5)]
    public string[] dialogueLines;

    [Header("Notebook notes based on dialogue")]
    [TextArea(5, 5)]
    public string[] notes;

    public enum PoemType 
    { 
        Couplet,
        Haiku,
        Acrostic,
        Clerihew,
        Triolet,
        PatternPoem,
        Pantoum,
        Ghazal,
        Etheree,
        EnglishSonnet,
        FreeVerse
    }

    [Header("Poem Settings")]
    public PoemType poemType;
    [Space(10)]
    [Tooltip("Button: (b:text1,text2,text3)\nInputField: (i:100)")]
    [TextArea(1, 5)]
    public string[] poem;
    [Space(10)]
    [Tooltip("The tips in the tutorial note that will teach the poem form structure. (e.g. two rhyming lines)")]
    [TextArea(1, 5)]
    public string[] tutorialNotes;





    [Header("Audio")]
    public AudioClip voiceSFX;
}
