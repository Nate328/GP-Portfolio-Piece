using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cubesight : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            transform.parent.gameObject.GetComponent<Gelcube>().Chase(other.gameObject, true);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            transform.parent.gameObject.GetComponent<Gelcube>().Chase(other.gameObject, false);
        }
    }
}
