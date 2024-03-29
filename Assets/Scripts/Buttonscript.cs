using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Buttonscript : MonoBehaviour
{
    public int state;
    public bool locked;  //Does it have a cost?
    public int costtype;   //0 = money. 1 = key. 2 = doodad.
    public int cost;
    private Uiscript inv;
    
    public Vector3 shift;
    public GameObject doorform;
    private doorshift door;

    public Vector3 lookangle;
    public Vector3 lookoffset;
    private Thecamera camcode;

    public InputActionAsset act;
    private InputAction interact;

    public UnityEngine.Object taskscript;
    private GameObject tholder;

    void Start()   //finds all the UI, input and game objects it needs to function
    {
        inv = GameObject.Find("Canvas").GetComponent<Uiscript>();
        interact = act.FindActionMap("New action map").FindAction("Interact");
        camcode = GameObject.Find("Cameraprop").GetComponent<Thecamera>();
        door = doorform.GetComponent<doorshift>();
        shift = doorform.transform.position + shift;
        act.FindActionMap("New action map").Enable();
    }

    private void OnTriggerStay(Collider other)  //if in the button area, the player can interact
    {
        if (act.FindAction("Interact").ReadValue<float>() > 0)
        {
            if (!locked)  //activates the button's function if never locked
            {
                Corefunction();
            }
            else if (locked && inv.Spend(costtype, cost))  //actiavtes the button's function if they have the key, or key equivalent.
            {
                Corefunction();
            }
        }
    }

    void Corefunction()  //whatever the button does- in this case, move objects & provide a cutscene.
    {
        var script = gameObject.GetComponent(Type.GetType(taskscript.name));   //gets the script it needs to activate
        camcode.CutCamerachange(lookangle, (door.oldspot + lookoffset));
        door.newspot = shift;
        door.act = state;
    }
    
    
}
