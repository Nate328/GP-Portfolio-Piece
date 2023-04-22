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
        if (focalobject != null)
        {
            focalpoint = focalobject.transform.position;}
        //focalpoint += transform.parent.position;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameObject.Find("Cameraprop").GetComponent<Thecamera>().Camerachange(nextcam, orient);
            GameObject.Find("Cameraprop").GetComponent<Thecamera>().Cameracurve(focalpoint);
        }
    }
}
