using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem Instance { get; set; }

    public GameObject inventoryScreenUI;
    public bool isOpen;
    public bool isFull;
    public List<string> itemList = new List<string>();
    public List<GameObject> slotList = new List<GameObject>();
    private GameObject itemToAdd;
    private GameObject equipSlot;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        isOpen = false;
        isFull = false;

        PopulateSlotList();
    }

    private void PopulateSlotList()
    {
        foreach (Transform child in inventoryScreenUI.transform)
            if (child.CompareTag("Slot"))
                slotList.Add(child.gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) && !isOpen)
        {
            inventoryScreenUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            isOpen = true;
        }
        else if (Input.GetKeyDown(KeyCode.I) && isOpen)
        {
            inventoryScreenUI.SetActive(false);
            if (!CraftingSystem.Instance.isOpen)
                Cursor.lockState = CursorLockMode.Locked;

            isOpen = false;
        }
    }

    public void AddToInventory(string item)
    {
        equipSlot = NextEmptySlot();
        Debug.Log("Adding " + item + " to inventory");
        itemToAdd = Instantiate(Resources.Load<GameObject>("InventoryItems/" + item), equipSlot.transform.position, equipSlot.transform.rotation);
        itemToAdd.transform.SetParent(equipSlot.transform);
        itemList.Add(item);
    }

    public void RemoveItem(string nameToRemove, int amount)
    {
        int counter = amount;

        for (var i = slotList.Count - 1; i >= 0; i--)
        {
            if (slotList[i].transform.childCount > 0)
            {
                if (slotList[i].transform.GetChild(0).name == nameToRemove + "(Clone)" && counter != 0)
                {
                    Destroy(slotList[i].transform.GetChild(0).gameObject);
                    counter--;
                }
            }
        }
    }

    public void ReCalculateList()
    {
        itemList.Clear();

        foreach (GameObject slot in slotList)
            if (slot.transform.childCount > 0)
                itemList.Add(slot.transform.GetChild(0).name.Replace("(Clone)", ""));
    }

    private GameObject NextEmptySlot()
    {
        foreach (GameObject slot in slotList)
            if (slot.transform.childCount == 0)
                return slot;
        return new GameObject();
    }

    public bool CheckIfFull()
    {
        int count = 0;

        foreach (GameObject slot in slotList)
            if (slot.transform.childCount > 0)
                count++;

        return count == 28;
    }
}