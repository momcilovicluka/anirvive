using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager instance { get; private set; }

    public TMPro.TextMeshProUGUI interactionInfo;
    public bool onTarget;
    public GameObject selectedObject;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else instance = this;
    }

    private void Start()
    {
        interactionInfo.text = "";
        onTarget = false;
    }

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        Debug.DrawRay(ray.origin, ray.direction * 100, Color.yellow);

        if (Physics.Raycast(ray, out hit))
        {
            var selectionTransform = hit.transform;
            InteractableObject interactableObject = selectionTransform.GetComponent<InteractableObject>();

            if (interactableObject && interactableObject.playerInRange)
            {
                interactionInfo.text = interactableObject.GetItemName() + " (E)";
                onTarget = true;
                selectedObject = interactableObject.gameObject;
            }
            else
            {
                interactionInfo.text = "";
                onTarget = false;
            }
        }
        else
        {
            interactionInfo.text = "";
            onTarget = false;
        }
    }
}