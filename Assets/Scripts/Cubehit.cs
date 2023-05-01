using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cubehit : MonoBehaviour
{
    private BoxCollider painzone;
    void Start()  //finds the damage hitbox (which is slightly larger than the slime)
    {
        painzone = gameObject.GetComponent<BoxCollider>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponentInChildren<Playerfight>().Hurt(1);
            transform.parent.GetComponent<Gelcube>().Land();  //counts as touching the ground, so it bounces on top of players
        }
    }
}
