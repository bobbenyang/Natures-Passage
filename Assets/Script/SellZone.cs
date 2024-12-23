using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellZone : MonoBehaviour
{
    public PickupManager pickupManager;  // Drag and drop the PickupManager in the Inspector
    public int metalSellPrice = 10;
    private bool isPlayerInZone = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = true;
            Debug.Log("Press 'F' to sell metal.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = false;
        }
    }

    void Update()
    {
        if (isPlayerInZone && Input.GetKeyDown(KeyCode.F))
        {
            SellItems();
        }
    }

    void SellItems()
    {
        int metalCount = pickupManager.GetInventoryCount("Metal");
        if (metalCount > 0)
        {
            int totalEarnings = metalCount * metalSellPrice;

            // Add earnings to the player's currency
            pickupManager.AddCurrency(totalEarnings);

            // Clear the metal from inventory after selling
            pickupManager.ClearItem("Metal");

            Debug.Log($"Sold {metalCount} metal for {totalEarnings} coins!");
        }
        else
        {
            Debug.Log("No metal to sell.");
        }
    }
}



