using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public string ItemName;
    public bool playerInRange;

    public string GetItemName()
    {
        return ItemName;
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && SelectionManager.Instance.onTarget && SelectionManager.Instance.selectedObject == gameObject &&gameObject.CompareTag("Pickable"))
        {
            if (InventorySystem.Instance.CheckIfFull())
            {
                Debug.Log("Inventory is full");
                return;
            }

            InventorySystem.Instance.AddToInventory(ItemName);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}