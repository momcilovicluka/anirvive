using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EquipSystem : MonoBehaviour
{
    public static EquipSystem Instance { get; set; }

    // -- UI -- //
    public GameObject quickSlotsPanel;

    public List<GameObject> quickSlotsList = new List<GameObject>();

    public GameObject numbersHolder;

    public int selectedNumber = -1;
    public GameObject selectedItem;

    public GameObject toolHolder;

    public GameObject selectedItemModel;

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
        PopulateSlotList();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            SelectQuickSlot(1);
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            SelectQuickSlot(2);
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            SelectQuickSlot(3);
        else if (Input.GetKeyDown(KeyCode.Alpha4))
            SelectQuickSlot(4);
        else if (Input.GetKeyDown(KeyCode.Alpha5))
            SelectQuickSlot(5);
        else if (Input.GetKeyDown(KeyCode.Alpha6))
            SelectQuickSlot(6);
        else if (Input.GetKeyDown(KeyCode.Alpha7))
            SelectQuickSlot(7);
    }

    private void SelectQuickSlot(int number)
    {
        if (checkIfSlotIsFull(number))
        {
            if (selectedNumber != number)
            {
                selectedNumber = number;

                if (selectedItem != null)
                    selectedItem.gameObject.GetComponent<InventoryItem>().isSelected = false;

                selectedItem = getSelectedItem(number);
                selectedItem.GetComponent<InventoryItem>().isSelected = true;

                SetEquippedModel(selectedItem);

                foreach (Transform child in numbersHolder.transform)
                    child.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().color = Color.white;

                TextMeshProUGUI text = numbersHolder.transform.Find("number" + number).Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
                text.color = Color.cyan;
            }
            else
            {
                selectedNumber = -1;

                if (selectedItem != null)
                {
                    selectedItem.gameObject.GetComponent<InventoryItem>().isSelected = false;
                    selectedItem = null;
                }

                if (selectedItemModel != null)
                {
                    Destroy(selectedItemModel.gameObject);
                    selectedItemModel = null;
                }

                foreach (Transform child in numbersHolder.transform)
                    child.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().color = Color.white;
            }
        }
    }

    private void SetEquippedModel(GameObject selectedItem)
    {
        if (selectedItemModel != null)
        {
            Destroy(selectedItemModel.gameObject);
            selectedItemModel = null;
        }

        string selectedItemName = selectedItem.name.Replace("(Clone)", "");
        selectedItemModel = Instantiate(Resources.Load<GameObject>(selectedItemName + "_Model"), new Vector3(0.6f, 0, 0.4f), Quaternion.Euler(10f, 90f, 30f));
        selectedItemModel.transform.SetParent(toolHolder.transform, false);
    }

    private GameObject getSelectedItem(int number)
    {
        return quickSlotsList[number - 1].transform.GetChild(0).gameObject;
    }

    private bool checkIfSlotIsFull(int number)
    {
        return quickSlotsList[number - 1].transform.childCount > 0;
    }

    private void PopulateSlotList()
    {
        foreach (Transform child in quickSlotsPanel.transform)
            if (child.CompareTag("QuickSlot"))
                quickSlotsList.Add(child.gameObject);
    }

    public void AddToQuickSlots(GameObject itemToEquip)
    {
        GameObject availableSlot = FindNextEmptySlot();
        itemToEquip.transform.SetParent(availableSlot.transform, false);

        InventorySystem.Instance.ReCalculateList();
    }

    private GameObject FindNextEmptySlot()
    {
        foreach (GameObject slot in quickSlotsList)
        {
            if (slot.transform.childCount == 0)
            {
                return slot;
            }
        }
        return new GameObject();
    }

    public bool CheckIfFull()
    {
        int counter = 0;

        foreach (GameObject slot in quickSlotsList)
        {
            if (slot.transform.childCount > 0)
            {
                counter += 1;
            }
        }

        if (counter == 7)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    internal bool IsHoldingWeapon()
    {
        if(selectedItem != null && selectedItem.GetComponent<Weapon>() != null)
            return true;
        else
            return false;

    }

    internal int GetWeaponDamage()
    {
        if (selectedItem != null)
            return selectedItem.GetComponent<Weapon>().weaponDamage;
        else
            return 0;
    }

    internal bool IsThereASwingLock()
    {
        if(selectedItemModel && selectedItemModel.GetComponent<EquipableItem>())
        {
            return selectedItemModel.GetComponent<EquipableItem>().swingWait;
        }
        else
        {
            return false;
        }
    }
}