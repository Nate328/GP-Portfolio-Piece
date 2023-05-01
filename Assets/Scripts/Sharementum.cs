using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sharementum : MonoBehaviour
{
    private Movingplat pa;
    private bool ridden;
    private Vector3 velo;
    private Collider passenger;
    void Start()  //
    {
        ridden = false;
        pa = transform.parent.GetComponent<Movingplat>();
        velo = pa.velocity;
    }

    private void OnTriggerEnter(Collider other)
    {
        passenger = other;
        ridden = true;
        velo = pa.velocity / 50;
        //speed = pa.speed;
        /*if (axis == 'X')
        { Debug.Log("Defining speed"); movement = new Vector3(speed, 0, 0); }
        if (axis == 'Y')
        { movement = new Vector3(0, speed, 0); }
        if (axis == 'Z')
        { movement = new Vector3(0, 0, speed); }*/

    }

    private void OnTriggerExit(Collider other)
    {
        ridden = false;
    }

    private void FixedUpdate()
    {
        if (ridden)
        {
            velo = pa.velocity / 50;
            Debug.Log(velo);
            passenger.transform.position += velo;
        }
    }
}
