using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PickupManager : MonoBehaviour
{
    public float pickupRadius = 5f; // Radius to detect objects
    public LayerMask pickupLayer; // Layer to specify pickup objects
    public TextMeshProUGUI woodText; // TextMeshPro UI for wood count
    public TextMeshProUGUI tireText; // TextMeshPro UI for tire count

    // Dictionary to store counts for each item type
    private Dictionary<string, int> inventory = new Dictionary<string, int>();

    void Start()
    {
        // Initialize inventory with default values
        inventory["Wood"] = 0;
        inventory["Tire"] = 0;

        UpdateUI();
    }

    void Update()
    {
        // Check for input to pick up objects
        if (Input.GetKeyDown(KeyCode.F))
        {
            PickupObjects();
        }
    }

    void PickupObjects()
    {
        // Find all colliders within the pickup radius on the specified layer
        Collider[] objectsInRange = Physics.OverlapSphere(transform.position, pickupRadius, pickupLayer);

        foreach (Collider obj in objectsInRange)
        {
            string itemType = GetItemType(obj);

            if (!string.IsNullOrEmpty(itemType))
            {
                // Increment the inventory count for the item type
                inventory[itemType]++;
                Debug.Log($"Picked up: {itemType} ({inventory[itemType]} total)");

                // Update the UI
                UpdateUI();

                // Destroy the object after picking it up
                Destroy(obj.gameObject);
            }
        }
    }

    string GetItemType(Collider obj)
    {
        // Determine the item type based on tag or another identifier
        if (obj.CompareTag("Wood"))
        {
            return "Wood";
        }
        else if (obj.CompareTag("Tire"))
        {
            return "Tire";
        }

        return null; // Not a pickable item
    }

    void UpdateUI()
    {
        // Update the TextMeshPro UI with the current inventory counts
        woodText.text = "Wood: " + inventory["Wood"];
        tireText.text = "Tire: " + inventory["Tire"];
    }

    // To visualize the pickup radius in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, pickupRadius);
    }
}
