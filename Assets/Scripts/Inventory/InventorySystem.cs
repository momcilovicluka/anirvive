using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TextMeshProUGUI = TMPro.TextMeshProUGUI;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem Instance { get; set; }
    public GameObject ItemInfoUI;

    public GameObject inventoryScreenUI;
    public bool isOpen;
    public bool isFull;
    public List<string> itemList = new List<string>();
    public List<GameObject> slotList = new List<GameObject>();
    private GameObject itemToAdd;
    private GameObject equipSlot;

    public GameObject pickupAlert;
    public TextMeshProUGUI pickupName;
    public Image pickupImage;

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

        Cursor.visible = false;
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
            Cursor.visible = true;

            SelectionManager.Instance.DisableSelection();
            SelectionManager.Instance.GetComponent<SelectionManager>().enabled = false;

            isOpen = true;
        }
        else if (Input.GetKeyDown(KeyCode.I) && isOpen)
        {
            inventoryScreenUI.SetActive(false);
            if (!CraftingSystem.Instance.isOpen)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }

            SelectionManager.Instance.EnableSelection();
            SelectionManager.Instance.GetComponent<SelectionManager>().enabled = true;

            isOpen = false;
        }
    }

    public void AddToInventory(string item)
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.pickupItemSound);

        equipSlot = NextEmptySlot();
        itemToAdd = Instantiate(Resources.Load<GameObject>("InventoryItems/" + item), equipSlot.transform.position, equipSlot.transform.rotation);
        itemToAdd.transform.SetParent(equipSlot.transform);

        itemList.Add(item);

        TriggerPickupPopup(item, itemToAdd.GetComponent<Image>().sprite);

        ReCalculateList();
        CraftingSystem.Instance.RefreshNeededItems();
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

        ReCalculateList();
        CraftingSystem.Instance.RefreshNeededItems();
    }

    public void ReCalculateList()
    {
        itemList.Clear();

        foreach (GameObject slot in slotList)
            if (slot.transform.childCount > 0)
                itemList.Add(slot.transform.GetChild(0).name.Replace("(Clone)", ""));
    }

    private void TriggerPickupPopup(string itemName, Sprite itemSprite)
    {
        pickupAlert.SetActive(true);
        pickupName.text = itemName;
        pickupImage.sprite = itemSprite;
        StartCoroutine(waitAndClose());
    }

    public IEnumerator<WaitForSeconds> waitAndClose()
    {
        yield return new WaitForSeconds(3f);
        pickupAlert.SetActive(false);
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