using UnityEngine;
using UnityEngine.UI;

public class Inventories : MonoBehaviour
{
    [Header("HUD Prefabs")]
    public GameObject inventoryUIPrefab; // Parent UI prefab for inventory
    public GameObject slotPrefab;        // Empty prefab to instantiate slot parent
    public GameObject backgroundPrefab;  // Background image prefab
    public GameObject borderPrefab;      // Border image prefab
    public GameObject hudParent;         // Canvas or panel to attach the HUD
    public GameObject hudTextPrefab;     // Prefab containing HudText (TextMeshPro or UI text)
    public Dialogue[] dialogues;

    [Header("Element Sprites")]
    public Sprite fireSprite;
    public Sprite waterSprite;
    public Sprite earthSprite;
    public Sprite airSprite;
    public Sprite emptySprite;

    [Header("Border Colors")]
    public Color highlightColor = Color.yellow;
    public Color generationColor = Color.cyan;
    public Color defaultColor = Color.clear;

    [HideInInspector] public int nextInventoryIndex = -1;
    [HideInInspector] public int nextSlotIndex = -1;

    private IInventory[] inventories;
    public InventoryUI[] inventoryUIs;

    // Predefined positions
    private readonly float[] inventoryXPositions = { 580f, 740f, 900f };
    private readonly float[] slotYPositions = { -450f, -340f, -230f };
    private readonly float textYPosition = -520f;

    private void Start()
    {
        inventories = GameObject.FindGameObjectWithTag("Player").GetComponents<IInventory>();
        inventoryUIs = new InventoryUI[inventories.Length];

        if (hudParent == null)
            hudParent = GameObject.FindGameObjectWithTag("HUD");

        for (int i = 0; i < inventories.Length; i++)
        {
            // Instantiate inventory parent UI
            GameObject invGO = Instantiate(inventoryUIPrefab, hudParent.transform);
            invGO.name = $"Inventory_{i}";
            invGO.transform.localPosition = new Vector3(inventoryXPositions[i], 0f, 0f);

            InventoryUI ui = new InventoryUI();
            ui.slots = new Image[inventories[i].MaxSize];
            ui.borders = new Image[inventories[i].MaxSize];

            // Instantiate each slot
            for (int s = 0; s < inventories[i].MaxSize; s++)
            {
                GameObject slotGO = Instantiate(slotPrefab, invGO.transform);
                slotGO.name = $"Slot_{s}";
                slotGO.transform.localPosition = new Vector3(0f, slotYPositions[s], 0f);

                // Background
                GameObject bg = Instantiate(backgroundPrefab, slotGO.transform);
                bg.name = "Background";
                bg.transform.localPosition = Vector3.zero;

                // Element image
                GameObject elementGO = new GameObject("Element", typeof(RectTransform), typeof(Image));
                elementGO.transform.SetParent(slotGO.transform, false);
                elementGO.transform.localPosition = Vector3.zero;
                Image elementImage = elementGO.GetComponent<Image>();
                elementImage.sprite = emptySprite;
                elementImage.color = Color.clear;

                // Border
                GameObject borderGO = Instantiate(borderPrefab, slotGO.transform);
                borderGO.name = "Border";
                borderGO.transform.localPosition = Vector3.zero;
                Image borderImage = borderGO.GetComponent<Image>();
                borderImage.color = defaultColor;

                ui.slots[s] = elementImage;
                ui.borders[s] = borderImage;
            }

            // Instantiate the HudText for this inventory
            if (hudTextPrefab != null)
            {
                GameObject textGO = Instantiate(hudTextPrefab, invGO.transform);
                textGO.name = $"InventoryText_{i}";
                textGO.transform.localPosition = new Vector3(0f, textYPosition, 0f);
                ui.text = textGO.GetComponent<HudText>();

                switch(inventories[i].Type)
                {
                  case InventoryType.Stack:
                    textGO.GetComponent<HudText>().SetDialogue(dialogues[0]);
                    break;
                  case InventoryType.Queue:
                    textGO.GetComponent<HudText>().SetDialogue(dialogues[1]);
                    break;
                  case InventoryType.LinkedList:
                    textGO.GetComponent<HudText>().SetDialogue(dialogues[2]);
                    break;
                }
            }

            inventoryUIs[i] = ui;
        }
    }

    private void Update()
    {
        RefreshHUD();
    }

    private void RefreshHUD()
    {
        for (int i = 0; i < inventories.Length && i < inventoryUIs.Length; i++)
        {
            var inv = inventories[i];
            var ui = inventoryUIs[i];

            int highlightIndex = -1;
            if (inv is LinkedListInventory linked)
                highlightIndex = linked.selectedIndex;
            else if (inv is StackInventory)
                highlightIndex = inv.Count - 1;
            else if (inv is QueueInventory)
                highlightIndex = 0;

            UpdateInventoryUI(inv.ToArray(), ui.slots, ui.borders, inv.MaxSize, highlightIndex, i);
        }
    }

    private void UpdateInventoryUI(
        Element[] elements,
        Image[] slots,
        Image[] borders,
        int maxSize,
        int highlight,
        int inventoryIndex,
        bool invertVertical = false)
    {
        for (int i = 0; i < maxSize; i++)
        {
            int index = invertVertical ? maxSize - 1 - i : i;

            if (i < elements.Length)
            {
                slots[index].sprite = GetSprite(elements[i]);
                slots[index].color = Color.white;
            }
            else
            {
                slots[index].sprite = emptySprite;
                slots[index].color = Color.clear;
            }

            borders[index].color = defaultColor;
        }

        // highlight
        if (elements.Length > 0 && highlight >= 0 && highlight < maxSize)
        {
            int highlightIndex = invertVertical ? maxSize - 1 - highlight : highlight;
            borders[highlightIndex].color = highlightColor;
        }

        // pointer for next element generation
        if (nextInventoryIndex == inventoryIndex && nextSlotIndex >= 0 && nextSlotIndex < maxSize)
        {
            int pointerIndex = invertVertical ? maxSize - 1 - nextSlotIndex : nextSlotIndex;
            borders[pointerIndex].color = generationColor;
        }
    }

    private Sprite GetSprite(Element element)
    {
        return element switch
        {
            Element.FIRE => fireSprite,
            Element.WATER => waterSprite,
            Element.EARTH => earthSprite,
            Element.AIR => airSprite,
            _ => emptySprite,
        };
    }
}
