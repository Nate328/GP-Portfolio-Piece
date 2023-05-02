using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camerashift : MonoBehaviour
{

    public char nextcam;
    public Vector3 orient;
    public Vector3 focalpoint;
    public GameObject focalobject;

    void Start()
    {
        if (focalobject != null)  //allows for objects to be loaded as focal points, without causing issues if not used.
        {
            focalpoint = focalobject.transform.position;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")  //activates when the player touches the hitbox
        {
            GameObject.Find("Cameraprop").GetComponent<Thecamera>().Camerachange(nextcam, orient);  //sets the new camera view and angle
            GameObject.Find("Cameraprop").GetComponent<Thecamera>().Cameracurve(focalpoint);  //sets a focal point for the camera
        }
    }
}
