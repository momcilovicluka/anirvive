using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{
    public TMPro.TextMeshProUGUI interactionInfo;

    private void Start()
    {
        interactionInfo.text = "";
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        Debug.DrawRay(ray.origin, ray.direction * 100, Color.yellow);


        if (Physics.Raycast(ray, out hit))
        {
            var selectionTransform = hit.transform;

            if (selectionTransform.GetComponent<InteractableObject>())
            {
                interactionInfo.text = selectionTransform.GetComponent<InteractableObject>().GetItemName();
            }
            else
            {
                interactionInfo.text = "";
            }

        }
    }
}
