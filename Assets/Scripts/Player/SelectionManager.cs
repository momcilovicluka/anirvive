using System.Collections;
using System.Collections.Generic;
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

            NPC npc = selectionTransform.GetComponent<NPC>();

            if (npc && npc.playerInRange)
            {
                interactionInfo.text += "Talk (E)";

                if (Input.GetKeyDown(KeyCode.E) && !npc.isTalkingWithPlayer)
                {
                    npc.StartConversation();
                }

                if (DialogSystem.Instance.dialogUIActive)
                {
                    interactionInfo.text = "";
                    centerDotIcon.gameObject.SetActive(false);
                }
            }

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

            InteractableObject interactableObject = selectionTransform.GetComponent<InteractableObject>();

            if (interactableObject && interactableObject.playerInRange)
            {
                onTarget = true;
                selectedObject = interactableObject.gameObject;
                interactionInfo.text = interactableObject.GetItemName() + " (E)";

                centerDotIcon.gameObject.SetActive(false);
                handIcon.gameObject.SetActive(true);

                handIsVisible = true;
            }

            Animal animal = selectionTransform.GetComponent<Animal>();

            if (animal && animal.playerInRange)
            {
                if (animal.isDead)
                {
                    interactionInfo.text = "Loot (E)";
                    centerDotIcon.gameObject.SetActive(false);
                    handIcon.gameObject.SetActive(true);

                    handIsVisible = true;

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        Lootable lootable = animal.GetComponent<Lootable>();
                        Loot(lootable);
                    }
                }
                else
                {
                    interactionInfo.text = animal.animalName;
                    centerDotIcon.gameObject.SetActive(true);
                    handIcon.gameObject.SetActive(false);
                    handIsVisible = false;

                    if (Input.GetMouseButtonDown(0) && EquipSystem.Instance.IsHoldingWeapon() && !EquipSystem.Instance.IsThereASwingLock())
                        StartCoroutine(DealDamageTo(animal, 0.3f, EquipSystem.Instance.GetWeaponDamage()));
                }
            }

            if (!interactableObject && !animal)
            {
                onTarget = false;
                handIsVisible = false;

                centerDotIcon.gameObject.SetActive(true);
                handIcon.gameObject.SetActive(false);
            }

            if (!npc && !interactableObject && !animal && !choppableTree)
            {
                interactionInfo.text = "";
            }
        }
    }

    private void Loot(Lootable lootable)
    {
        if (!lootable.wasLootCalculated)
        {
            List<LootReceived> receivedLoot = new List<LootReceived>();

            foreach (LootPossibility lootPossibility in lootable.possibleLoot)
            {
                int amount = UnityEngine.Random.Range(lootPossibility.amountMin, lootPossibility.amountMax);
                if (amount > 0)
                {
                    LootReceived lootReceived = new LootReceived();
                    lootReceived.item = lootPossibility.item;
                    lootReceived.amount = amount;
                    receivedLoot.Add(lootReceived);
                }
            }

            lootable.finalLoot = receivedLoot;
            lootable.wasLootCalculated = true;
        }

        Vector3 lootSpawnPosition = lootable.gameObject.transform.position;

        foreach (LootReceived lootReceived in lootable.finalLoot)
            for (int i = 0; i < lootReceived.amount; i++)
            {
                GameObject lootSpawn = Instantiate(Resources.Load<GameObject>(lootReceived.item.name), new Vector3(lootSpawnPosition.x, lootSpawnPosition.y + 0.1f, lootSpawnPosition.z), Quaternion.Euler(0, 0, 0));
            }

        Destroy(lootable.gameObject);
    }

    private IEnumerator DealDamageTo(Animal animal, float delay, int damage)
    {
        yield return new WaitForSeconds(delay);
        animal.TakeDamage(damage);
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