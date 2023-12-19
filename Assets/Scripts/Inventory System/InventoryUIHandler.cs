using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIHandler : MonoBehaviour
{
    public static InventoryUIHandler instance;

    public InventorySystem inventorySystem;
    public CraftingSystem craftingSystem;
    public ChestSystem chestSystem;
    public SmeltingSystem smeltingSystem;
    public HotbarSystem hotbarSystem;
    [SerializeField] Image BackgroundImage;
    private void Awake()
    {
        instance = this;
        if (inventorySystem == null) inventorySystem = FindObjectOfType<InventorySystem>();
        if (craftingSystem == null) craftingSystem = FindObjectOfType<CraftingSystem>();
        if (chestSystem == null) chestSystem = FindObjectOfType<ChestSystem>();
        if (smeltingSystem == null) smeltingSystem = FindObjectOfType<SmeltingSystem>();
        if (hotbarSystem == null) hotbarSystem = FindObjectOfType<HotbarSystem>();
    }
    [System.Serializable]
    public class ConvenientCanvasGroup
    {
        public string Name;
        public CanvasGroup group;
        public void Activate(bool shouldActivate)
        {
            group.interactable = group.blocksRaycasts = shouldActivate;
            group.alpha = shouldActivate?1:0;
        }
    }

    [Space(20)]

    [Tooltip("This uses the old input system, therefore a specified inventory button in player settings is required.")]
    [SerializeField] string InventoryButton;

    [Space]
    
    [SerializeField] ConvenientCanvasGroup[] groups;
    
    [Tooltip("What crafting default station will open when you open your inventory?")]
    [SerializeField] string DefaultCraftingStation = "Crafting";

    public string InventoryUIName = "InventoryUI";
    public string ChestUIName = "ChestUI";
    public string SmeltingUIName = "SmeltingUI";
    public string CraftingUIName = "CraftingUI";
    public string HotbarUIName = "HotbarUI";

    bool open;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < groups.Length; i++)
        {
            groups[i].Activate(false);
        }
        GetGroup(HotbarUIName).Activate(true);
        hotbarSystem.SelectionOutline.enabled = true;
        BackgroundImage.enabled = open;
    }
    public void OpenInventoryWithChest()
    {
        GetGroup(InventoryUIName).Activate(true);
        GetGroup(ChestUIName).Activate(true);
        GetGroup(HotbarUIName).Activate(false);
        hotbarSystem.SelectionOutline.enabled = false;
        open = true;
        BackgroundImage.enabled = open;
    }
    public void OpenInventoryWithSmelter()
    {
        GetGroup(InventoryUIName).Activate(true);
        GetGroup(SmeltingUIName).Activate(true);
        GetGroup(HotbarUIName).Activate(false);
        hotbarSystem.SelectionOutline.enabled = false;
        open = true;
        BackgroundImage.enabled = open;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown(InventoryButton))
        {
            open = !open;
            BackgroundImage.enabled = open;
            if (open)
            {
                GetGroup(InventoryUIName).Activate(true);
                GetGroup(CraftingUIName).Activate(true);
                craftingSystem.OpenCrafting(DefaultCraftingStation);
                GetGroup(HotbarUIName).Activate(false);
                hotbarSystem.SelectionOutline.enabled = false;
            }
            else
            {
                GetGroup(InventoryUIName).Activate(false);
                GetGroup(CraftingUIName).Activate(false);
                chestSystem.CloseChest();
                craftingSystem.CloseCrafting();
                smeltingSystem.CloseSmelter();
                GetGroup(HotbarUIName).Activate(true);
                hotbarSystem.SelectionOutline.enabled = true;
            }
        }
    }
    /// <summary>
    /// This simplifies enabling and disabling Inventory UI.
    /// </summary>
    /// <returns>ConvenientCanvasGroup (A class consisting of a string called 'Name',a CanvasGroup and a function named 'Activate(bool shouldActivate)'.)</returns>
    public ConvenientCanvasGroup GetGroup(string name)
    {
        for (int i = 0; i < groups.Length; i++)
        {
            if (groups[i].Name == name)
            {
                return groups[i];
            }
        }
        Debug.LogWarning($"No Group with the name '{name}' was found.");
        return null;
    }
    public bool IsOpen()
    {
        return open;
    }
}
