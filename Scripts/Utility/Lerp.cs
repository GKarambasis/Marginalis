using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lerp : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("The point where the animation will start from")]
    public Transform startingPoint;
    [Tooltip("The point where the animation will end")]
    public Transform endPoint;
    [Tooltip("How fast the gameobject will mome during the animation")]
    public float speed = 1.0f;
    
    private float t = 0.0f;

    [Header("Debug")]
    public bool moveToEnd = false;
    public bool moveToStart = false;

    void Update()
    {
        //Move to A
        if (t < 1.0f && moveToEnd)
        {
            t += Time.deltaTime * speed; // Increase t over time
            transform.position = Vector3.Lerp(startingPoint.position, endPoint.position, t);
        }
        else if (t >= 1.0f && moveToEnd)
        {
            t = 1.0f;
            moveToEnd = false;
        }

        //Move to B
        if (t > 0.0f && moveToStart)
        {
            t -= Time.deltaTime * speed; // Increase t over time
            transform.position = Vector3.Lerp(startingPoint.position, endPoint.position, t);
        }
        else if(t <= 0.0f && moveToStart)
        {
            t = 0.0f;
            moveToStart = false;
        }

    }

    void MoveToEnd()
    {
        moveToEnd = true;
    }

    void MoveToStart()
    {
        moveToStart = true;
    }

    void ReverseMove()
    {
        if (moveToStart)
        {
            moveToStart = false;
            moveToEnd = true;
        }
        else if (moveToEnd)
        {
            moveToEnd = false;
            moveToStart = true;
        }
    }

    public void OnClick_MoveBackAndForth()
    {
        if(!moveToEnd && !moveToStart && t==0f)
        {
            MoveToEnd();
        }
        else if (!moveToEnd && !moveToStart && t == 1f)
        {
            MoveToStart();
        }
        else if (moveToEnd || moveToStart)
        {
            ReverseMove();
        }
    }

}
