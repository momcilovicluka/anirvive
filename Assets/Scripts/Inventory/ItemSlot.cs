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
        Debug.Log("OnDrop");

        //if there is not item already then set our item.
        if (!Item)
        {
            DragNDrop.itemBeingDragged.transform.SetParent(transform);
            DragNDrop.itemBeingDragged.transform.localPosition = new Vector2(0, 0);
        }
    }
}