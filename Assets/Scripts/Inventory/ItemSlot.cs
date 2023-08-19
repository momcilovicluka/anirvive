using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    public GameObject Item
    {
        get
        {
            if (transform.childCount > 0)
            {
                return transform.GetChild(0).gameObject;
            }

            return null;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (!Item)
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.dropItemSound);

            DragNDrop.itemBeingDragged.transform.SetParent(transform);
            DragNDrop.itemBeingDragged.transform.localPosition = new Vector2(0, 0);

            DragNDrop.itemBeingDragged.GetComponent<InventoryItem>().isInsideQuickSlot = transform.CompareTag("QuickSlot");
            InventorySystem.Instance.ReCalculateList();
        }
    }
}