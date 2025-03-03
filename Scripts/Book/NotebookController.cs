using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotebookController : MonoBehaviour
{
    [Header("Debug")]
    public bool deactivateOnStart;
    
    [Header("Gameobject Assignment - Do Not Touch")] 
    [SerializeField] GameObject rightbuttonParent;
    [SerializeField] GameObject leftbuttonParent; 
    
    [SerializeField] GameObject pageParent;
    [SerializeField] GameObject defaultPage;

    GameObject currentPage;
    

    private Button[] buttons;
    private GameObject[] pages;

    private void Start()
    {
        //Construct buttons Array
        buttons = new Button[rightbuttonParent.transform.childCount];
        for (int i = 0; i < rightbuttonParent.transform.childCount; i++)
        {
            buttons[i] = rightbuttonParent.transform.GetChild(i).GetComponent<Button>();
        }

        //Construct pages Array
        pages = new GameObject[pageParent.transform.childCount];
        for (int i = 0; i < pageParent.transform.childCount; i++)
        {
            pages[i] = pageParent.transform.GetChild(i).gameObject;
        }

        //Debug
        if(pages.Length != buttons.Length) { Debug.LogError("The amount of buttons[] do not equal to the amount of pages[], this might create errors."); }

        currentPage = defaultPage;

        if (deactivateOnStart)
        {
            Invoke("Deactivate", 1f);
        }
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }

    public void OnClick_GoToPage(int pageIndex)
    {
        //Disable automatic arrangement of buttons
        if (rightbuttonParent.GetComponent<VerticalLayoutGroup>().enabled) { rightbuttonParent.GetComponent<VerticalLayoutGroup>().enabled = false; }
        if (leftbuttonParent.GetComponent<VerticalLayoutGroup>().enabled) { leftbuttonParent.GetComponent<VerticalLayoutGroup>().enabled = false; }
        
        //Buttons moving left and right
        for (int i = 0; i < buttons.Length; i++) 
        {
            //Move to Left
            if (i < pageIndex)
            {
                buttons[i].transform.SetParent(leftbuttonParent.transform, false);
                buttons[i].transform.SetSiblingIndex(i);
            }
            //Move to Right
            else if (i > pageIndex)
            {
                buttons[i].transform.SetParent(rightbuttonParent.transform, false);
                buttons[i].transform.SetSiblingIndex(i);
            }

            //Code for the clicked Button
            else if (i == pageIndex)
            {
                //if the button clicked is in the left parent
                if (buttons[i].transform.parent.gameObject == leftbuttonParent) 
                {
                    buttons[i].transform.SetParent(rightbuttonParent.transform, false);
                    buttons[i].transform.SetSiblingIndex(i);

                    //if the book needs to be closed
                    if ((pageIndex - 1) < 0)
                    {
                        FlipPage(defaultPage);
                    }
                    //if the book remains open
                    else
                    {
                        FlipPage(pageIndex - 1);
                    }
                }
                //if the button clicked is in the right parent
                else if (buttons[i].transform.parent.gameObject == rightbuttonParent) 
                { 
                    buttons[i].transform.SetParent(leftbuttonParent.transform, false);
                    buttons[i].transform.SetSiblingIndex(i);
                    
                    FlipPage(pageIndex);
                }
            }
        }
    }

    void FlipPage(int pageIndex)
    {
        //Disable Last Page
        currentPage.SetActive(false);

        pages[pageIndex].SetActive(true);

        
        //Save the current active page
        currentPage = pages[pageIndex];
    }
    void FlipPage(GameObject page)
    {
        currentPage.SetActive(false);

        page.SetActive(true);

        //Save the active page
        currentPage = page;
    }

    public void ToggleButtons(bool state)
    {
        for (int i = 0; i < pages.Length; i++)
        {
            buttons[i].interactable = state;
        }
    }
    public void ToggleButtons(GameObject page, bool state)
    {
        for (int i = 0; i < pages.Length; i++)
        {
            if (pages[i].gameObject == page) { buttons[i].interactable = state; };
        }
    }

}
