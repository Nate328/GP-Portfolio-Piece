using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Playermove : MonoBehaviour
{
    public InputAction move;
    public InputAction jump;
    public InputAction sprint;

    public InputAction lockon;
    public bool interacting = false;
    
    
    public int speed;
    public int jstrength;
    public int drop;
    public Quaternion direction;
    private int skid;
    private bool falling;
    private bool running;
    private int sparejump;

    private Rigidbody rb;
    private Animator anim;

    public GameObject prefab;
    public Vector3 spawn;
    
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        anim = gameObject.GetComponent<Animator>();
        jump.performed += ctx => { Jumping(ctx); };
        sprint.performed += ctx => { Dashing(ctx); };
        lockon.performed += ctx => { Targeting(ctx); };
        lockon.canceled += ctx => { Detarget(ctx); };
        move.Enable();
        jump.Enable();
        sprint.Enable();
        lockon.Enable();
    }

    void FixedUpdate()
    {
        var movement = move.ReadValue<Vector2>();
        var boost = speed;
        if (sprint.ReadValue<float>() > 0)
        {
            running = true;
            boost = boost * 2;
            var rev = rb.velocity.magnitude;
        }
        else
        {
            running = false;}

        var dropspeed = 0;
        if (falling)
        {
            movement = movement / 3;
            dropspeed = -drop;
        }
        var motion = new Vector3(movement.x*boost, dropspeed, movement.y*boost);
        
        if (Math.Abs(movement.x) > 0.05 || Math.Abs(movement.y) > 0.05)
        {
            skid = 2;
            anim.SetBool("Moving", true);
            if (running)
            { anim.SetFloat("Velocity", 1); }
            else
            { anim.SetFloat("Velocity", Mathf.Clamp((Math.Abs(movement.x) + Math.Abs(movement.y)), 0f, 0.8f)); }
        }
        else
        {
            motion = new Vector3(0, 0, 0);
            skid = 20;
            anim.SetBool("Moving", false);
            anim.SetFloat("Velocity", 0f);
        }

        direction = GameObject.Find("Cameraprop").GetComponent<Thecamera>().direction;
        
        rb.velocity += direction * (motion / skid);
        //if rb.velocity's x or z get too high, reduce them.
        var angle = 0f;
        if (Math.Abs(movement.x) > 0.05 || Math.Abs(movement.y) > 0.05)
        {
            var facing = rb.velocity.normalized;
            angle = (Mathf.Atan2(facing.x, facing.z)) * Mathf.Rad2Deg;
        }
        else
        {
            angle = direction.eulerAngles.y;
        }
        
        Quaternion rotation = Quaternion.Euler(0, angle, 0);
        transform.rotation = rotation;
    }
    
    
    void Jumping(InputAction.CallbackContext context)
    {
        if (!falling)
        {
            rb.velocity += new Vector3(0, jstrength, 0);
            falling = true;
        }
        else if (sparejump > 0)
        {
            rb.velocity = new Vector3(rb.velocity.x, jstrength/2, rb.velocity.z);
            sparejump -= 1;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        falling = false;
        sparejump = 1;
    }

    void Dashing(InputAction.CallbackContext context)
    {
        rb.velocity = Vector3.zero;
    }

    void Targeting(InputAction.CallbackContext context)
    {
        Debug.Log("Target locked");
        float reach = 10000;
        Collider closest = null;
        List<Collider> targets = GameObject.Find("Targeter").GetComponent<Targeting>().GetColliders();
        foreach (Collider t in targets)
        {
            float distance = (t.transform.position- transform.position).sqrMagnitude;
            if (distance < reach)
            {
                closest = t;
            }
        }
        if (closest != null)
        {
            GameObject cam = GameObject.Find("Cameraprop");
            cam.GetComponent<Thecamera>().target = closest;
            cam.GetComponent<Thecamera>().lockon = true;
        }
    }

    void Detarget(InputAction.CallbackContext context)
    {
        GameObject.Find("Cameraprop").GetComponent<Thecamera>().lockon = false;
    }

    public void Die()
    {
        transform.position = spawn;
    }
    
    //fix distance between player and target when moving along "spline" so as to fake it.
    
    
}
