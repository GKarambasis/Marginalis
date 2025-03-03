using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PoemInput : MonoBehaviour
{
    RectTransform rectTransform;
    RectTransform parent;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        parent = transform.parent.GetComponent<RectTransform>();
    }

    public void OnClick_RebuildLayout()
    {
        StartCoroutine(RebuildLayout());
        GetComponentInParent<Poem>().CheckPoem();
    }

    //To rebuild the rectTransform of the button
    IEnumerator RebuildLayout()
    {
        yield return new WaitForSeconds(0.1f);
        //LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
        LayoutRebuilder.ForceRebuildLayoutImmediate(parent);
    }
}
