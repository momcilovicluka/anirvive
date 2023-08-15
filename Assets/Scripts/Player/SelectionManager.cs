using UnityEngine;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{
    public TMPro.TextMeshProUGUI interactionInfo;
    public bool onTarget;
    public GameObject selectedObject;

    public Image centerDotIcon;
    public Image handIcon;
    public static SelectionManager instance { get; private set; }

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
                onTarget = true;
                selectedObject = interactableObject.gameObject;
                interactionInfo.text = interactableObject.GetItemName() + " (E)";

                if (interactableObject.CompareTag("Pickable"))
                {
                    centerDotIcon.gameObject.SetActive(false);
                    handIcon.gameObject.SetActive(true);
                }
                else
                {
                    centerDotIcon.gameObject.SetActive(true);
                    handIcon.gameObject.SetActive(false);
                }
            }
            else
            {
                interactionInfo.text = "";
                onTarget = false;
                centerDotIcon.gameObject.SetActive(true);
                handIcon.gameObject.SetActive(false);
            }
        }
        else
        {
            interactionInfo.text = "";
            onTarget = false;
            centerDotIcon.gameObject.SetActive(true);
            handIcon.gameObject.SetActive(false);
        }
    }
}