using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogPet : MonoBehaviour
{
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void OnMouseEnter()
    {
        animator.SetBool("isPetting", true);
    }
    private void OnMouseExit()
    {
        animator.SetBool("isPetting", false);
    }
}
