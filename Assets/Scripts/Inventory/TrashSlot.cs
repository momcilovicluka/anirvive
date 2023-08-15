using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TrashSlot : MonoBehaviour, IDropHandler
{
    public GameObject trashAlertUI;

    private TextMeshProUGUI textToModify;

    private Image imageComponent;

    private Button YesBTN, NoBTN;

    private GameObject draggedItem
    {
        get
        {
            return DragNDrop.itemBeingDragged;
        }
    }

    private GameObject itemToBeDeleted;

    public string itemName
    {
        get
        {
            string name = itemToBeDeleted.name;
            string toRemove = "(Clone)";
            string result = name.Replace(toRemove, "");
            return result;
        }
    }

    private void Start()
    {
        textToModify = trashAlertUI.transform.Find("Text").GetComponent<TextMeshProUGUI>();

        YesBTN = trashAlertUI.transform.Find("Yes").GetComponent<Button>();
        YesBTN.onClick.AddListener(delegate { DeleteItem(); });

        NoBTN = trashAlertUI.transform.Find("No").GetComponent<Button>();
        NoBTN.onClick.AddListener(delegate { CancelDeletion(); });
    }

    public void OnDrop(PointerEventData eventData)
    {
        //itemToBeDeleted = DragDrop.itemBeingDragged.gameObject;
        if (draggedItem.GetComponent<InventoryItem>().isTrashable == true)
        {
            itemToBeDeleted = draggedItem.gameObject;

            StartCoroutine(notifyBeforeDeletion());
        }
    }

    private IEnumerator notifyBeforeDeletion()
    {
        trashAlertUI.SetActive(true);
        textToModify.text = "Discard this " + itemName + "?";
        yield return new WaitForSeconds(1f);
    }

    private void CancelDeletion()
    {
        trashAlertUI.SetActive(false);
    }

    private void DeleteItem()
    {
        DestroyImmediate(itemToBeDeleted.gameObject);
        InventorySystem.Instance.ReCalculateList();
        CraftingSystem.Instance.RefreshNeededItems();
        trashAlertUI.SetActive(false);
    }
}