using UnityEngine;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{
    public TMPro.TextMeshProUGUI interactionInfo;
    public bool onTarget;
    public GameObject selectedObject;

    public Image centerDotIcon;
    public Image handIcon;
    public static SelectionManager Instance { get; private set; }

    public bool handIsVisible;

    public GameObject selectedTree;
    public GameObject chopHolder;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else Instance = this;
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

            ChoppableTree choppableTree = selectionTransform.GetComponent<ChoppableTree>();

            if (choppableTree && choppableTree.playerInRange)
            {
                choppableTree.canBeChopped = true;
                selectedTree = choppableTree.gameObject;
                chopHolder.gameObject.SetActive(true);
            }
            else
            {
                if (selectedTree != null)
                {
                    selectedTree.GetComponent<ChoppableTree>().canBeChopped = false;
                    selectedTree = null;
                    chopHolder.gameObject.SetActive(false);
                }
            }

            if (interactableObject && interactableObject.playerInRange)
            {
                onTarget = true;
                selectedObject = interactableObject.gameObject;
                interactionInfo.text = interactableObject.GetItemName() + " (E)";

                if (interactableObject.CompareTag("Pickable"))
                {
                    centerDotIcon.gameObject.SetActive(false);
                    handIcon.gameObject.SetActive(true);

                    handIsVisible = true;
                }
                else
                {
                    centerDotIcon.gameObject.SetActive(true);
                    handIcon.gameObject.SetActive(false);

                    handIsVisible = false;
                }
            }
            else
            {
                interactionInfo.text = "";
                onTarget = false;
                centerDotIcon.gameObject.SetActive(true);
                handIcon.gameObject.SetActive(false);

                handIsVisible = false;
            }
        }
        else
        {
            interactionInfo.text = "";
            onTarget = false;
            centerDotIcon.gameObject.SetActive(true);
            handIcon.gameObject.SetActive(false);

            handIsVisible = false;
        }
    }

    public void DisableSelection()
    {
        handIcon.enabled = false;
        centerDotIcon.enabled = false;
        interactionInfo.text = "";
        selectedObject = null;
    }

    public void EnableSelection()
    {
        centerDotIcon.gameObject.SetActive(true);
        handIcon.gameObject.SetActive(false);
    }
}