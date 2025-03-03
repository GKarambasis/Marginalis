using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TextMeshProUGUI))]
[RequireComponent (typeof(AudioSource))]
public class TypeWriterEffect : MonoBehaviour
{
    TextMeshProUGUI textMesh;
    AudioSource audioSource;
    string text;
    public float textDelay = 0.05f; 

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        textMesh = GetComponent<TextMeshProUGUI>();
        text = textMesh.text;
        textMesh.text = "";
        
    }

    private void Start()
    {
        StartCoroutine(TypeWritterText(textMesh, text, textDelay));
    }

    private IEnumerator TypeWritterText(TextMeshProUGUI responseText, string fullText, float delay)
    {
        responseText.text = "";

        foreach (char c in fullText)
        {
            responseText.text += c; 
            if(audioSource.clip != null) { audioSource.Play(); }
            yield return new WaitForSeconds(delay); 
        }
    }
}
