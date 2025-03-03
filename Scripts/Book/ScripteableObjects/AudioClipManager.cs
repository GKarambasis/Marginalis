using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAudioClipManager", menuName = "ScriptableObjects/Audio")]
public class AudioClipManager : ScriptableObject
{
    [Header("Music")]
    [Tooltip("Default music that will always play in the background")]
    public AudioClip defaultBGM;
    [Tooltip("Music that will play when the Bard Composes a Song")]
    public AudioClip[] recitingBGM;
    [Tooltip("Music that will play at night, when talking to the Muse")]
    public AudioClip nightBGM;

    [Header("SFX")]
    public AudioClip openBook;
    public AudioClip closeBook;
    public AudioClip turnPage;
    [Space(20)]
    public AudioClip[] wordFlip;
    public AudioClip composeButton;

    [Header("Characters")]
    public AudioClip defaultCharacterChatter;

}
