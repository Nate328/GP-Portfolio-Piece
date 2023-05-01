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
    void Start()  //find sthe platform it is linked to
    {
        ridden = false;
        pa = transform.parent.GetComponent<Movingplat>();
        velo = pa.velocity;
    }

    private void OnTriggerEnter(Collider other)  //triggers when a passenger hops on board
    {
        passenger = other;
        ridden = true;
        velo = pa.velocity / 50;
    }

    private void OnTriggerExit(Collider other)  //triggers when a passenger gets off, stopping the momentum sharing
    {
        ridden = false;
    }

    private void FixedUpdate()  //while a passenger is on the platform, the velocity of the platform is applied to them
    {
        if (ridden)
        {
            velo = pa.velocity / 50;
            passenger.transform.position += velo;
        }
    }
}
