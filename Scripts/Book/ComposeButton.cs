using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ComposeButton : MonoBehaviour
{
    Button button;
    Poem myPoem;

    private void FixedUpdate()
    {
        FindParentPoem();
    }

    private void FindParentPoem()
    {
        if (myPoem == null)
        {
            myPoem = GetComponentInParent<Poem>();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() => GameManager.Instance.PlayPoem(myPoem.poemLineContent));
        button.onClick.AddListener(() => button.interactable = false);
    }
}
