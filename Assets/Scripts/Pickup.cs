using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public bool collected;
    private GameObject burst;
    public enum items
    {
        Speed,
        Jump,
        Damage,
        HealthS,
        HealthL,
        Supersonic,
        NULL,  //Above- pickups with powers, Below- inventory items
        Coin,
        Key,
        Doodad,
    }
    public items thing;
    private int item;

    void Start()  //gets the item assigned to this pickup
    {
        item = (int) thing;
        burst = GameObject.Find("poof");
        burst.SetActive(false);
    }

    void Update()  //destroys the pickup when the particle effect is done
    {
        if (collected && burst.GetComponent<ParticleSystem>().isPlaying == false)
        {Destroy(gameObject);}
    }

    private void OnTriggerEnter(Collider other)  //Lets the player collect the item
    {
        if (other.CompareTag("Player") && !collected)
        {
            burst.SetActive(true);  //plays a pickup particle effect
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            if (item < (int) items.NULL)
            {GameObject.Find("Canvas").GetComponent<Uiscript>().Gainitems(item, true);}  //gives the player the item
            else if (item > (int) items.NULL)
            {GameObject.Find("Canvas").GetComponent<Uiscript>().Gainitems(item, false);}
            collected = true;  //Stops the player from picking it up more than once
        }
    }
}
