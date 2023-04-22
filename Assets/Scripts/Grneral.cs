using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Grneral : MonoBehaviour
{
    public InputAction pause;
    private bool paused;
    
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        pause.performed += ctx => { Paused(ctx);};
        pause.Enable();
    }

    void Paused(InputAction.CallbackContext context )
    {
        if (!paused)
        {
            Time.timeScale = 0f;
            paused = true;
        }

        if (paused)
        {
            Time.timeScale = 0f;
            paused = true;
        }
    }
}
