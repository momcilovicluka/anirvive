using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSystem : MonoBehaviour
{
    public GameObject craftingScreenUI;

    public GameObject toolScreenUI;

    public List<string> inventoryItemList = new List<string>();

    private Button toolsBTN;
    private Button craftAxeBTN;
    private TextMeshProUGUI AxeReq1, AxeReq2;
    public bool isOpen = false;

    // Blueprints
    public Blueprint AxeBLP = new Blueprint("Axe", 2, "Stone", 3, "Stick", 3);

    public static CraftingSystem Instance { get; set; }

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

    // Start is called before the first frame update
    private void Start()
    {
        StartCoroutine(calculate());

        isOpen = false;
        toolsBTN = craftingScreenUI.transform.Find("ToolsButton").GetComponent<Button>();
        toolsBTN.onClick.AddListener(delegate { OpenToolsCategory(); });

        AxeReq1 = toolScreenUI.transform.Find("Axe").transform.Find("req1").GetComponent<TextMeshProUGUI>();
        AxeReq2 = toolScreenUI.transform.Find("Axe").transform.Find("req2").GetComponent<TextMeshProUGUI>();
        craftAxeBTN = toolScreenUI.transform.Find("Axe").transform.Find("Button").GetComponent<Button>();
        craftAxeBTN.onClick.AddListener(delegate { CraftAnyItem(AxeBLP); });
    }

    private void CraftAnyItem(Blueprint blueprintToCraft)
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.craftingSound);

        StartCoroutine(craftedDelayForSound(blueprintToCraft));

        InventorySystem.Instance.RemoveItem(blueprintToCraft.Req1, blueprintToCraft.Req1Amount);

        if (blueprintToCraft.numOfRequirements == 2)
            InventorySystem.Instance.RemoveItem(blueprintToCraft.Req2, blueprintToCraft.Req2Amount);

        StartCoroutine(calculate());
    }

    private IEnumerator<WaitForSeconds> craftedDelayForSound(Blueprint blueprintToCraft)
    {
        yield return new WaitForSeconds(1f);

        InventorySystem.Instance.AddToInventory(blueprintToCraft.itemName);
    }

    public IEnumerator<WaitForSeconds> calculate()
    {
        yield return new WaitForSeconds(0.1f);

        InventorySystem.Instance.ReCalculateList();

        RefreshNeededItems();
    }

    private void OpenToolsCategory()
    {
        toolScreenUI.SetActive(true);
        craftingScreenUI.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && !isOpen)
        {
            craftingScreenUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            SelectionManager.Instance.DisableSelection();
            SelectionManager.Instance.GetComponent<SelectionManager>().enabled = false;

            isOpen = true;
        }
        else if (Input.GetKeyDown(KeyCode.C) && isOpen)
        {
            craftingScreenUI.SetActive(false);
            toolScreenUI.SetActive(false);

            if (!InventorySystem.Instance.isOpen)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }

            SelectionManager.Instance.EnableSelection();
            SelectionManager.Instance.GetComponent<SelectionManager>().enabled = true;

            isOpen = false;
        }
    }

    public void RefreshNeededItems()
    {
        int stoneCount = 0;
        int stickCount = 0;

        inventoryItemList = InventorySystem.Instance.itemList;

        foreach (string itemName in inventoryItemList)
        {
            switch (itemName)
            {
                case "Stone":
                    stoneCount++;
                    break;

                case "Stick":
                    stickCount++;
                    break;
            }
        }

        // AXE
        AxeReq1.text = "3 Stone [" + stoneCount + "]";
        AxeReq2.text = "3 Stick [" + stickCount + "]";

        if (stoneCount >= 3 && stickCount >= 3)
            craftAxeBTN.interactable = true;
        else
            craftAxeBTN.interactable = false;
    }
}