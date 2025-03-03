using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PoemButton : MonoBehaviour
{
    string[] words;
    int currentIndex;
    TextMeshProUGUI textmesh;
    RectTransform rectTransform;
    RectTransform parent;

    private void Awake()
    {
        textmesh = GetComponentInChildren<TextMeshProUGUI>();
        rectTransform = GetComponent<RectTransform>();
        parent = transform.parent.GetComponent<RectTransform>();
        //textmesh.text = words[0];
        //currentIndex = 0;
    }

    public void Initialize(string[] wordList)
    {
        //StartCoroutine(InvokeInitialize(wordList));
        words = wordList;
        textmesh.text = words[0];
        currentIndex = 0;
        StartCoroutine(RebuildLayout());
    }

    public void OnClick_CycleWords()
    {
        GameManager.Instance.OnClickPlay_WordFlip();
        if((currentIndex + 1) == words.Length)
        {
            currentIndex = 0;
            textmesh.text = words[0];
            StartCoroutine(RebuildLayout());
        }
        else 
        {
            currentIndex++;
            textmesh.text = words[currentIndex];
            StartCoroutine(RebuildLayout());
        }
        GetComponentInParent<Poem>().CheckPoem();
    }

    //To rebuild the rectTransform of the button
    IEnumerator RebuildLayout()
    {
        yield return new WaitForSeconds(0.1f);
        //LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
        LayoutRebuilder.ForceRebuildLayoutImmediate(parent);
    }

    /*
    IEnumerator InvokeInitialize(string[] wordList)
    {
        yield return new WaitForSeconds(0.1f);
        Debug.Log(wordList.Length.ToString());
        words = wordList;
        textmesh.text = words[0];
        currentIndex = 0;
        StartCoroutine(RebuildLayout());
    }
    */
}
