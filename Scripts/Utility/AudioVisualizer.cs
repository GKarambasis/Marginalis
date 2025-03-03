using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AudioVisualizer : MonoBehaviour
{
    public AudioSource audioSource;  // Assign the audio source in the Inspector
    public TMP_Text textMesh;        // Assign the TextMeshPro component
    public string[] words = { "Cunt " + "Penis " + "Ass " };
    public int frequencyBand = 2;    // Choose a high-frequency band (0-63)
    public float threshold = 1f;   // Adjust sensitivity for word changes
    public float cooldownTime = 0.05f; // Time between word changes (prevents rapid flickering)

    private float[] spectrumData = new float[64]; // Stores audio spectrum analysis
    private int wordIndex = 0;       // Current word index
    private int characterIndex = 0;
    private float lastChangeTime = 0; // Timestamp of last word change

    public bool activate = false;

    void Start()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        /*if (audioSource != null)
            audioSource.Play();*/
    }

    void Update()
    {
        if (activate)
        {
            RhythmicTyping();
        }
    }

    public void RhythmicTyping()
    {
        if (GameManager.Instance.composedPoemLines.Length == 0) return;
        words = GameManager.Instance.composedPoemLines;

        if (!audioSource.isPlaying) return;
        
        // Get real-time spectrum data
        AudioListener.GetSpectrumData(spectrumData, 0, FFTWindow.BlackmanHarris);

        // Get intensity of the chosen frequency band
        float intensity = spectrumData[frequencyBand] * 100;


        // If intensity crosses the threshold and cooldown has passed, change word
        if (intensity > threshold && Time.time - lastChangeTime > cooldownTime)
        {
            //Debug.LogWarning("Intensity crossed the threshold");
            lastChangeTime = Time.time; // Update last change timestamp
            
            // Cycle through words
            if (words[wordIndex].Length < (characterIndex + 1)) 
            {
                textMesh.text += "<br>";
                wordIndex = (wordIndex + 1); 
                characterIndex = 0;     
            }
            //Finish when cycled through all the words
            if (words.Length < (wordIndex + 1)) 
            { 
                activate = false;
                wordIndex = 0;
                return; 
            }

            textMesh.text += words[wordIndex][characterIndex]; // Update text
            characterIndex = (characterIndex + 1);
            
        }

    }
}
