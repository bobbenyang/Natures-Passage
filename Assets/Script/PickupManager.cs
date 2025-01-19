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
    public TextMeshProUGUI metalText; // TextMeshPro UI for metal count
    public TextMeshProUGUI currencyText; // TextMeshPro UI for player currency

    private Dictionary<string, int> inventory = new Dictionary<string, int>();
    private int playerCurrency = 0; // Player's currency
    public Animator animator;  // Animator reference

    public GameObject pickupPopupPrefab; // Assign a prefab for the popup in the inspector
    public Transform popupSpawnPoint;    // Point above the player’s head to spawn the popup

    void ShowPickupPopup(string itemType, int amount, Vector3 position)
    {
        if (pickupPopupPrefab != null)
        {
            // Instantiate the popup at the item's position with an offset
            Vector3 popupPosition = position + new Vector3(0, 2f, 0); // Slightly above the item
            GameObject popup = Instantiate(pickupPopupPrefab, popupPosition, Quaternion.identity);
            
            TextMeshProUGUI popupText = popup.GetComponentInChildren<TextMeshProUGUI>();
            if (popupText != null)
            {
                popupText.text = $"+{amount} {itemType}";
            }

            Destroy(popup, 1.5f); // Destroy the popup after 1.5 seconds
        }
    }


    public int GetCurrency()
    {
        return playerCurrency;
    }


    void Start()
    {
        // Initialize inventory with default values
        inventory["Wood"] = 0;
        inventory["Tire"] = 0;
        inventory["Metal"] = 0;

        UpdateUI();

        // Get the Animator component
        animator = GetComponent<Animator>();
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
        Collider[] objectsInRange = Physics.OverlapSphere(transform.position, pickupRadius, pickupLayer);

        foreach (Collider obj in objectsInRange)
        {
            string itemType = GetItemType(obj);

            if (!string.IsNullOrEmpty(itemType))
            {
                animator.SetTrigger("PickupTrigger");
                inventory[itemType]++;
                
                // Show popup with item name and amount
                ShowPickupPopup(itemType, 1, obj.transform.position);
                
                Debug.Log($"Picked up: {itemType} ({inventory[itemType]} total)");
                UpdateUI();
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
        else if (obj.CompareTag("Metal"))
        {
            return "Metal";
        }

        return null; // Not a pickable item
    }

    void UpdateUI()
    {
        // Update the TextMeshPro UI with the current inventory counts
        woodText.text = "Wood: " + inventory["Wood"];
        tireText.text = "Tire: " + inventory["Tire"];
        metalText.text = "Metal: " + inventory["Metal"];
        currencyText.text = "Coins: " + playerCurrency;
    }

    public int GetInventoryCount(string itemType)
    {
        if (inventory.ContainsKey(itemType))
        {
            return inventory[itemType];
        }
        return 0;
    }

    public void ClearItem(string itemType)
    {
        if (inventory.ContainsKey(itemType))
        {
            inventory[itemType] = 0;
            UpdateUI();
        }
    }

    public void AddCurrency(int amount)
    {
        playerCurrency += amount;
        UpdateUI();
    }

    // To visualize the pickup radius in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, pickupRadius);
    }
}

