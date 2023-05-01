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
    
    void Start() //finds all of the components and sets the starting values
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

    void FixedUpdate()  //gives the player i-frames and lets their attack linger
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
    
    void Hit(InputAction.CallbackContext context)  //when the player attacks, enables the hitbox, plays an animation and a particle effect
    {
        anim.SetTrigger("Trigger");
        hitbox.enabled = true;
        burst.Play();
    }

    private void OnTriggerEnter(Collider other)  //if the attack hitbox hits an enemy, trigger their pain script
    {
        if (other.tag == "Foe")
        {
            Debug.Log("stab");
            other.GetComponent<Gelcube>().Pain(strength);
        }
    }

    public void Hurt(int damage)  //if the player takes damage, give them i-frames and check if they die
    {
        if (iframes <= 0)
        {
            health -= damage;
            if (health <= 0)  //if the player dies, a new player is made back at spawn
            {
                health = 10;
                transform.parent.GetComponent<Playermove>().Die();
            }

            iframes = 1;
        }
    }
}
