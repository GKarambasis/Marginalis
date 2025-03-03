using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField] Cursor defaultCursor;
    [SerializeField] Cursor selectedCursor;
    [SerializeField] Cursor specialCursor;

    private void Start()
    {
        //Cursor.SetCursor(defaultCursor, default, CursorMode.Auto);

    }


}
