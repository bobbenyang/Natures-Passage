using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportManager : MonoBehaviour
{
    public PickupManager pickupManager; // Reference to the PickupManager
    public string targetSceneName; // Scene to teleport to
    public int requiredWood = 5; // Required amount of wood
    public int requiredTires = 3; // Required amount of tires
    public int requiredMetal = 2; // Required amount of metal
    public int requiredCurrency = 100; // Required amount of currency

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            TryTeleport();
        }
    }

    void TryTeleport()
    {
        // Check if player meets the item and currency requirements
        if (pickupManager.GetInventoryCount("Wood") >= requiredWood &&
            pickupManager.GetInventoryCount("Tire") >= requiredTires &&
            pickupManager.GetInventoryCount("Metal") >= requiredMetal &&
            pickupManager.GetCurrency() >= requiredCurrency)
        {
            Debug.Log("Teleporting to " + targetSceneName);

            // Teleport to the target scene
            SceneManager.LoadScene(targetSceneName);
        }
        else
        {
            Debug.Log("Not enough items or currency to teleport!");
        }
    }
}

