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

    void Awake()
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
        if (iframes > 0)
        {
            iframes -= 0.1f;
            if (iframes <= 0)
            {
                gameObject.GetComponent<MeshRenderer>().material = basemat;
            }
        }

        if (!aggro)
        {
            Patrol();
            //Hop();
        }
        else
        {
            transform.LookAt(target.transform.position);
            transform.eulerAngles = new Vector3(0, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            direction = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
            Hop();
        }
    }

    void Hop()
    {
        //move the slime in bounding leaps
        if (falling)
        {
            speed = new Vector3(speed.x, -2, speed.z);
        }
        else
        {
            speed = new Vector3(speed.x, jump, speed.z);
            falling = true;
            attack.enabled = true;
        }

        rb.velocity += direction * speed;
    }

    void Patrol()
    {
        agent.destination = destination;
    }

    public void Chase(GameObject victim, bool hunt)
    {
        if (hunt)
        {
            target = victim;
            aggro = true;
            agent.enabled = false;
            falling = false;
        }
        else
        {
            aggro = false;
            agent.enabled = true;
            falling = false;
        }
    }

    public void Pain()
    {
        gameObject.GetComponent<MeshRenderer>().material = painmat;
        if (iframes <= 0)
        {
            health -= 1;
            if (health <= 0)
            {
                Die();
            }

            iframes = 1;
        }
    }

    void Die()
    {
        if (scale != 'S')
        {
            var sizedown = 'S';
            if (scale == 'L')
            {
                if (cargo != null)
                { cargo.transform.parent = null;}
                sizedown = 'M';
            }

            GameObject blob = Instantiate(prefab, transform.position, transform.rotation);
            blob = Instantiate(prefab, transform.position, transform.rotation);
        }

        Debug.Log("deaded.");
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ground")
        {
            Land();
        }
    }

    public void Land()
    {
        falling = false;
        attack.enabled = false;
    }
}