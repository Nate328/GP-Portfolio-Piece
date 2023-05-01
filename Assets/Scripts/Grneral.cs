using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Grneral : MonoBehaviour
{
    void Start()  //Makes the mouse dissapear when you click
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
