using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cubesight : MonoBehaviour
{
    void OnTriggerEnter(Collider other)  //starts chasing the player if they enter its view hitbox
    {
        if (other.tag == "Player")
        {
            transform.parent.gameObject.GetComponent<Gelcube>().Chase(other.gameObject, true);
        }
    }
    void OnTriggerExit(Collider other)  //stops chasing them if they leave.
    {
        if (other.tag == "Player")
        {
            transform.parent.gameObject.GetComponent<Gelcube>().Chase(other.gameObject, false);
        }
    }
}
