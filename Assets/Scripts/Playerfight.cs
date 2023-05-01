using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Playerfight : MonoBehaviour
{
    private BoxCollider hitbox;
    private ParticleSystem burst;
    private Animator anim;
    public InputAction attack;

    public int health;
    public int healthmax = 10;
    private float iframes;

    public int strength;
    
    void Start() //
    {
        strength = 1;
        health = healthmax;
        attack.performed += ctx => { Hit(ctx); };
        attack.Enable();
        anim = transform.parent.gameObject.GetComponent<Animator>();
        hitbox = gameObject.GetComponent<BoxCollider>();
        hitbox.enabled = false;
        burst = gameObject.GetComponent<ParticleSystem>();
    }

    void FixedUpdate()
    {
        if (iframes > 0)
        {
            iframes -= 0.1f;
        }
        if (burst.isPlaying == false)
        {
            hitbox.enabled = false;
        }
    }
    
    void Hit(InputAction.CallbackContext context)
    {
        anim.SetTrigger("Trigger");
        hitbox.enabled = true;
        burst.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Foe")
        {
            Debug.Log("stab");
            other.GetComponent<Gelcube>().Pain(strength);
        }
    }

    public void Hurt(int damage)
    {
        if (iframes <= 0)
        {
            health -= damage;
            if (health <= 0)
            {
                health = 10;
                transform.parent.GetComponent<Playermove>().Die();
            }

            iframes = 1;
        }
    }
}
