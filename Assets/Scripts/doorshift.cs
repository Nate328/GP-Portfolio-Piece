using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorshift : MonoBehaviour
{
    public char method;
    public Vector3 oldspot;
    public Vector3 newspot;
    public int  act = 0;
    public float increment;

    private Thecamera camcode;
    
    void Start()  
    {
        camcode = GameObject.Find("Cameraprop").GetComponent<Thecamera>();
        increment = 0.0f;
        oldspot = transform.position;
    }

    void Update() //moves the door, if it needs to
    {
        if (act > 0)
        {
            transform.position = Vector3.Lerp(oldspot, newspot, increment);
            Progress(newspot);
        }
        else if (act < 0)
        {
            transform.position = Vector3.Lerp(transform.position, oldspot, increment);
            Progress(oldspot);
        }
    }

    void Progress(Vector3 target) //increments the door, and stops everything once it's done.
    {
        increment += 0.001f;
        if (transform.position == target)
        {
            act = 0;
            camcode.CutCameraback();
        }
    }
}
