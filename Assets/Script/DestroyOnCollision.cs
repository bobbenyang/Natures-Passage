using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnCollision : MonoBehaviour
{
    // This method is called when another object enters the trigger collider attached to this object
    private void OnTriggerEnter(Collider other)
    {
        // Destroy the object that collided with this object
        Destroy(other.gameObject);
    }
}