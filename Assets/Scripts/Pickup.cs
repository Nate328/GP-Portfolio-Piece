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
        NULL,
        Coin,
        Key,
        Doodad,
    }
    public items thing;
    private int item;

    void Start()
    {
        item = (int) thing;
        burst = GameObject.Find("poof");
        burst.SetActive(false);
    }

    void Update()
    {
        if (collected && burst.GetComponent<ParticleSystem>().isPlaying == false)
        {Destroy(gameObject);}
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !collected)
        {
            burst.SetActive(true);
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            if (item < (int) items.NULL)
            {GameObject.Find("Canvas").GetComponent<Uiscript>().Gainitems(item, true);}
            else if (item > (int) items.NULL)
            {GameObject.Find("Canvas").GetComponent<Uiscript>().Gainitems(item, false);}
            collected = true;
        }
    }
}
