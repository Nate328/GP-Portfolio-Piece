using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Object = UnityEngine.Object;

public class Gelcube : MonoBehaviour
{
    public GameObject prefab;
    public char scale;
    public int health;
    private float iframes;

    public Material basemat;
    public Material painmat;

    public Vector3 destination; 

    private Quaternion direction;
    private Vector3 speed = new Vector3(0f, 0f, 0.5f);
    public float jump;
    private bool falling = false;

    private GameObject target;
    private bool aggro;
    private BoxCollider attack;

    private Rigidbody rb;
    private NavMeshAgent agent;

    public GameObject cargo;

    void Awake()  //gets child components & navmesh
    {
        agent = GetComponent<NavMeshAgent>();
        attack = transform.GetChild(1).GetComponent<BoxCollider>();
        direction = new Quaternion(0, 0, 0, 0);
        iframes = 1;
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (iframes > 0)  //checks if it is in invincibility frames
        {
            iframes -= 0.1f;
            if (iframes <= 0)
            {
                gameObject.GetComponent<MeshRenderer>().material = basemat;  //changes colour back to normal when vulnerable again
            }
        }

        if (!aggro)  //moves around without a target
        {
            Patrol();
        }
        else  //moves towards the player, hopping
        {
            transform.LookAt(target.transform.position);
            transform.eulerAngles = new Vector3(0, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            direction = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
            Hop();
        }
    }

    void Hop() //move the slime in bounding leaps
    {
        if (falling)
        {
            speed = new Vector3(speed.x, -2, speed.z);
        }
        else  //if not falling, bounces up again
        {
            speed = new Vector3(speed.x, jump, speed.z);
            falling = true;
            attack.enabled = true;
        }
        rb.velocity += direction * speed;
    }

    void Patrol()  //wanders towards a preset destination
    {
        agent.destination = destination;
    }

    public void Chase(GameObject victim, bool hunt)   //Target, or disengage, the player
    {
        if (hunt)
        {
            target = victim;
            aggro = true;
            agent.enabled = false;
            Hop();
            falling = false;
        }
        else
        {
            aggro = false;
            agent.enabled = true;
            falling = false;
        }
    }

    public void Pain(int dmg)  //deal damage to the slime, and begin its i-frames
    {
        gameObject.GetComponent<MeshRenderer>().material = painmat;
        if (iframes <= 0)
        {
            health -= dmg;
            if (health <= 0)
            {
                Die();
            }

            iframes = 1;
        }
    }

    void Die()  //kill the slime, and if it is large enough, it splits
    {
        if (scale != 'S')  //small slimes don't split
        {
            var sizedown = 'S';
            if (scale == 'L')
            {
                if (cargo != null) //if the slime is carrying something, it drops it
                { cargo.GetComponent<Pickup>().Collected();}
                sizedown = 'M';
            }

            GameObject blob = Instantiate(prefab, transform.position, transform.rotation);  //creates a pair of new, smaller slimes
            blob = Instantiate(prefab, transform.position, transform.rotation);
        }
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other) //tells when it touches the ground
    {
        if (other.tag == "Ground")
        {
            Land();
        }
    }

    public void Land()   //lets the slime jump again
    {
        falling = false;
        attack.enabled = false;
    }
}