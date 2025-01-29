using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagCollisionHandler : MonoBehaviour
{
    public int trashPoints = 1;
    public int recyclePoints = -1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Trash"))
        {
            GameManager.Instance.AddScore(trashPoints); // Add points to the global score
        }
        else if (other.CompareTag("Recycle"))
        {
            GameManager.Instance.AddScore(recyclePoints); // Add points to the global score
        }

        Destroy(other.gameObject); // Destroy the collided object
    }
}
