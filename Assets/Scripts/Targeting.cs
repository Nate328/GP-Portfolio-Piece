using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeting : MonoBehaviour
{
    private List<Collider> colliders = new List<Collider>();  //List of objects in the targeting area
    public List<Collider> GetColliders () { return colliders; }  //returns the colliders
 
    private void OnTriggerEnter (Collider other) {     //adds colliders that move into the trigger area
        if (!colliders.Contains(other)) { colliders.Add(other); }
    }
 
    private void OnTriggerExit (Collider other) {   //removes colliders that move out of the trigger area
        colliders.Remove(other);
    }
}
