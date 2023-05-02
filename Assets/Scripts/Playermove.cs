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
    
    void Start() //enables the character controls
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

    void FixedUpdate() //checks player movement
    {
        var movement = move.ReadValue<Vector2>();  //gets joystick input
        var boost = speed;  
        if (sprint.ReadValue<float>() > 0) //checks if the player is holding the sprint key
        {
            running = true;
            boost = boost * 2;  //doubles speed bonus
        }
        else
        {
            running = false;
            
        }
        var dropspeed = 0;
        if (falling)  //if mid-air, the player has less control over their motion
        {
            movement = movement / 3;
            dropspeed = -drop;
        }
        var motion = new Vector3(movement.x*boost, dropspeed, movement.y*boost);  //applies boost to horizontal movement
        
        if (Math.Abs(movement.x) > 0.05 || Math.Abs(movement.y) > 0.05)  //if the player is moving:
        {
            skid = 2;
            anim.SetBool("Moving", true);
            if (running)
            { anim.SetFloat("Velocity", 1); }  //plays the sprinting animation
            else
            //plays the walking animation- clamping it to stop it playing the running animation
            { anim.SetFloat("Velocity", Mathf.Clamp((Math.Abs(movement.x) + Math.Abs(movement.y)), 0f, 0.8f)); }
        }
        else
        {
            motion = new Vector3(0, 0, 0);  //helps the player come to a halt when they stop moving
            skid = 20;
            anim.SetBool("Moving", false);
            anim.SetFloat("Velocity", 0f);
        }

        direction = GameObject.Find("Cameraprop").GetComponent<Thecamera>().direction;  //gets the direction the camera is facing, so the player moves relative to it
        
        rb.velocity += direction * (motion / skid);   //applies the calculated motion to the player's body
        var angle = 0f;
        if (Math.Abs(movement.x) > 0.05 || Math.Abs(movement.y) > 0.05)  //if the player is moving:
        {
            var facing = rb.velocity.normalized;  //find out the direction the player is moving in
            angle = (Mathf.Atan2(facing.x, facing.z)) * Mathf.Rad2Deg;  //and turn that into a heading
        }
        else
        {
            angle = direction.eulerAngles.y;  //otherwise, leave that angle as is.
        }
        
        Quaternion rotation = Quaternion.Euler(0, angle, 0);  
        transform.rotation = rotation;  //rotate the player, so they face the direction they're moving in
    }
    
    
    void Jumping(InputAction.CallbackContext context)  //lets the player jump
    {
        if (!falling)  //for when the player jumps off the ground
        {
            rb.velocity += new Vector3(0, jstrength, 0);
            falling = true;
        }
        else if (sparejump > 0)  //for the second mid-air jump
        {
            rb.velocity = new Vector3(rb.velocity.x, jstrength/2, rb.velocity.z);  //adds to the player's horizontal momentum, but adds less of a vertical boost than the jump off the ground.
            sparejump -= 1;
        }
    }

    private void OnTriggerEnter(Collider other)  //collider for feet to tell when the player lands
    {
        falling = false;
        sparejump = 1;
    }

    void Dashing(InputAction.CallbackContext context)  //resets momentum when the player starts sprinting
    {
        rb.velocity = Vector3.zero;
    }

    void Targeting(InputAction.CallbackContext context)  //lets the player lock on to a target
    {
        float reach = 10000;
        Collider closest = null;
        List<Collider> targets = GameObject.Find("Targeter").GetComponent<Targeting>().GetColliders();  //gets the list of objects in the targeter area from the targeter
        foreach (Collider t in targets)  //cycles through the objects
        {
            float distance = (t.transform.position- transform.position).sqrMagnitude;
            if (distance < reach)
            {
                closest = t;  //sets new target object
            }
        }
        if (closest != null)  //if there is a target in the area, target it with the camera
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
