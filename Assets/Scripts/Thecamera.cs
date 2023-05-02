using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Thecamera : MonoBehaviour
{
    public GameObject player;
    public InputAction pan;
    public Collider target;
    public bool lockon;
    public bool curving;
    public Vector3 focal;
    public InputAction camreset;
    public int camspeed;

    public Quaternion direction;
    private Vector3 perspective;

    private Quaternion lastperspec;
    private Quaternion nextperspec;
    private float nearing;
    private Vector3 afterSpot;
    private Quaternion afterRot;
    
    private Vector3 ocamspot;
    private Quaternion ocamrot;

    private int curvepoints;
    
    private GameObject camerabox;

    private Vector3 FCamSpot;
    private Quaternion FCamRot;
    private Vector3 SCamSpot;
    private Quaternion SCamRot;
    private Vector3 TCamSpot;
    private Quaternion TCamRot;
    private Vector3 CCamSpot;
    private Quaternion CCamRot;
    
    public char camerastate;
    public bool transition;
    private char oldstate;
    
    void Start()  //find the camera and define the perspective points
    {
        camerabox = GameObject.Find("Camerabox");
        FCamSpot = new Vector3(-0.008f,2.77f,-4.87f);  //Follow Camera
        FCamRot = new Quaternion(0.19f,0,0,0.98f);
        SCamSpot = new Vector3(-0.008f,0.29f,-6.87f);   //Side-on Camera
        SCamRot = new Quaternion(0,0,0,1);
        TCamSpot = new Vector3(0,7.5f,0);    //Top-down Camera
        TCamRot = new Quaternion(0.7f,0,0,0.7f);
        CCamSpot = new Vector3(-0.44f, 3.59f, -12.47f);   //Cutscene Camera
        CCamRot = new Quaternion(0.14f,0,0,0.99f);
        lockon = false;
        camerastate = 'F';
        transition = false;
        pan.Enable();
    }


    void FixedUpdate() //perform orientation specific updates
    {
        if (camerastate != 'C')  //If not in a cutscene, the camera's focus returns to the player
        {transform.position = player.transform.position;}
        if (curving)
        {
            Curvealong();   //If curving, focus on that central point and adjust momentum accordingly.
            transform.LookAt(focal);
        }
        else if (lockon)  //If Locked on, the camera focuses on that.
        {transform.LookAt(target.transform);}
        if (camerastate == 'F')    //If in third-person, keep the camera close and give the player control
        {Followcamera();}
        if ((camerastate == 'S' || camerastate == 'T') && !curving)  //If either side-on or top-down, Maintain a certain perspective.
        {direction = Quaternion.Euler(perspective); }
        if (transition)  //If moving between states, continue.
        { Camswing(); }
    }

    public void Camerachange(char nextcam, Vector3 orient) //switches camera state to a different state
    {
        if (transition == false && camerastate != nextcam)  //If the camera is changing state, or is already in the requested state, nothing happens.
        {
            camerastate = nextcam;   //switches the camera state to the new one, and begins the transition- giving the camera the new position and rotation values it needs
            if (camerastate == 'F')  
            {
                afterSpot = FCamSpot;
                afterRot = FCamRot;
            }
            if (camerastate == 'S')
            {
                afterSpot = SCamSpot;
                afterRot = SCamRot;
            }
            if (camerastate == 'T')
            {
                afterSpot = TCamSpot;
                afterRot = TCamRot;
            }
            Startswing(orient);
        }
    }

    public void CutCamerachange(Vector3 rotate, Vector3 focus)  //switches camera to a cutscene camera
    {
        if (camerastate != 'C') //stops it from locking the player in cutscene mode forever
        {
            oldstate = camerastate;
            afterSpot = CCamSpot;
            afterRot = CCamRot;
            Startswing(rotate);
        }
        camerastate = 'C';  //sets the state as 'cutscene'
        transform.position = focus;   //changes the camera position to the one desired.
        
    }

    public void CutCameraback()  //returns from a transition camera
    {
        Camerachange(oldstate, perspective);
    }

    void Followcamera()  //3rd person 3D camera control
    {
        var rotate = pan.ReadValue<Vector2>().normalized;
        var panning = new Vector3(transform.eulerAngles.x + -(rotate.y*(camspeed/2)), transform.eulerAngles.y + (rotate.x*camspeed), 0);
        transform.eulerAngles = panning;
        direction = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
    }

    void Curvealong()  //lets the camera move around a given point
    {
        /*float x = focal.x;
        float z = focal.z;
        float y = focal.y;
        if (focal.x == 0)   //set any of the three coordinates to zero to allow the camera to follow the player around that axis.
        { x = player.transform.position.x; }
        if (focal.x == 0)
        { z = player.transform.position.z; }
        if (focal.y == 0)
        { y = player.transform.position.z; }
        focal.y = player.transform.position.z;
        focal = new Vector3(x, y, z);  
        
        direction = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);*/
        float x = focal.x;
        float z = focal.z;
        if (focal.x == 0)
        { x = player.transform.position.x; }

        if (focal.y == 0)
        { z = player.transform.position.z; }
        focal = new Vector3(x, player.transform.position.y, z);
        
        direction = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
    }

    void Startswing(Vector3 orient)  //Prepares to swing the camera
    {
        transition = true;
        perspective = orient;
        nextperspec = Quaternion.Euler(orient);  //stores the old & new position and rotation values to lerp between
        lastperspec = transform.rotation;
        ocamspot = camerabox.transform.localPosition;
        ocamrot = camerabox.transform.rotation;
        nearing = 0;
    }

    void Camswing()  //moves the camera from one view to the next
    {
        transform.rotation = Quaternion.Lerp(lastperspec, nextperspec, nearing);
        //adds a lerp to shift the rotation & position of the transition camera to match the parent.
        camerabox.transform.localPosition = Vector3.Lerp(ocamspot, afterSpot, nearing);
        camerabox.transform.localRotation = Quaternion.Lerp(ocamrot, afterRot, nearing);
        nearing += 0.05f;
        if (nearing >= 1)  //once it has reached its destination, it stops.
        {
            transition = false;
            if (camerastate == 'S' || camerastate == 'T')
            {
                transform.rotation = nextperspec;
                camerabox.transform.localPosition = afterSpot;
                camerabox.transform.localRotation = afterRot;
            }
        }
    }

    public void Cameracurve(Vector3 focalpoint)
    {
        if (focalpoint != Vector3.zero)
        {
            curving = true;
            focal = focalpoint;
        }
        else
        { curving = false; }
    }

}