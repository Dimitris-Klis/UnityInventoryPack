using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [System.Serializable]
    public class SlotProperties
    {
        [Tooltip("Overlay image is used to change how the slot looks. Probably best to make said image not block raycasts.")]
        public Image Overlay;
        [Space(20)]

        [Tooltip("The Image that will display what item is inside.")]
        public Image IconImage;

        [Tooltip("The Image that will be displaying the slot's shape.")]
        public Image BackgroundImage;

        [Tooltip("The text that will show how many items are in the slot")]
        public TMP_Text ItemAmountText;
        [Space(20)]
        [Tooltip("The color of the overlay when the mouse is not over the slot.")]
        public Color Default = Color.clear;

        [Tooltip("The color of the overlay when the mouse is hovering over the slot.")]
        public Color Hover = new(0, 0, 0, .5f);

        [Tooltip("The color of the overlay when the mouse is clicking the slot.")]
        public Color Click = new(0, 0, 0, .8f);
    }
    public void SetDefaultColors(Color _default, Color _hover, Color _click)
    {
        slotProperties.Default = _default;
        slotProperties.Hover = _hover;
        slotProperties.Click = _click;
        slotProperties.Overlay.color = slotProperties.Default;
    }
    public SlotProperties slotProperties;
    
    [Tooltip("mainly used if the Slot Type is 'Mouse'")]
    public CanvasGroup SlotGroup;
    public enum SlotTypes { Inventory, HotbarSend,HotbarReceive, Smelt, Mouse };
    [Space]
    [Tooltip("Inventory is the default slot type.\nIf it's a hotbar slot, it will affect what the hotbar is displaying accordingly.\nIf it's a mouse slot, it will be used to transfer items between slots.")]
    public SlotTypes slotType = SlotTypes.Inventory;
    [Tooltip("Specifies Which hotbar slot will be affected if SlotType is 'Hotbar'")]
    public int hotbarIndex;
    [Space]
    [Space(30)]
    [Tooltip("The Item that the slot is holding.")]
    public InventorySystem.InventoryItem Item;
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (slotType == SlotTypes.HotbarReceive) return;
        if (slotProperties.Overlay != null)
            slotProperties.Overlay.color = slotProperties.Hover;
        if(Item.item != null && Item.amount > 0)
        {
            string message = $"{Item.item.Name}\n<size=80%>{Item.item.Description}</size>";
            Tooltip.instance.OpenTooltip(message);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (slotType == SlotTypes.HotbarReceive) return;
        if (slotProperties.Overlay != null)
            slotProperties.Overlay.color = slotProperties.Default;
        Tooltip.instance.CloseTooltip();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (slotType == SlotTypes.HotbarReceive) return;
        if (slotProperties.Overlay != null)
            slotProperties.Overlay.color = slotProperties.Click;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (slotType == SlotTypes.HotbarReceive) return;
        if (slotProperties.Overlay != null)
            slotProperties.Overlay.color = slotProperties.Hover;
    }
    //This is where the button behavior happens
    public void OnPointerClick(PointerEventData eventData)
    {
        if (slotType == SlotTypes.HotbarReceive) return;
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            InventoryUIHandler.instance.inventorySystem.SwapWithMouse(this);
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            InventoryUIHandler.instance.inventorySystem.MouseSlotExchange(this);
        }
        if(slotType == SlotTypes.Smelt)
        {
            InventoryUIHandler.instance.smeltingSystem.AttemptSmelt();
        }
    }
    /// <summary>
    /// Updates the slot's item and amount.
    /// </summary>
    public void UpdateSlot(Item item, int amount)
    {
        Item.item = item;
        Item.amount = amount;

        if (item == null || amount == 0)
        {
            if(slotProperties.IconImage != null)
            {
                slotProperties.IconImage.enabled = false;
            }
            if (slotProperties.ItemAmountText != null)
            {
                slotProperties.ItemAmountText.text = "";
            }
        }
        else
        {
            if (slotProperties.IconImage != null)
            {
                slotProperties.IconImage.enabled = true;
                slotProperties.IconImage.sprite = item.Icon;
            }
            if (slotProperties.ItemAmountText != null)
            {
                slotProperties.ItemAmountText.text = amount.ToString();
            }
        }
        //if(slotType == SlotTypes.Hotbar)
        //{
        //    //Set HotbarIndex;
        //}
    }
    /// <summary>
    /// Updates the slot's display. (Icon and text)
    /// </summary>
    public void UpdateSlot()
    {
        if (Item.item == null || Item.amount == 0)
        {
            if (slotProperties.IconImage != null)
            {
                slotProperties.IconImage.enabled = false;
            }
            if (slotProperties.ItemAmountText != null)
            {
                slotProperties.ItemAmountText.text = "";
            }
        }
        else
        {
            if (slotProperties.IconImage != null)
            {
                slotProperties.IconImage.enabled = true;
                slotProperties.IconImage.sprite = Item.item.Icon;
            }
            if (slotProperties.ItemAmountText != null)
            {
                slotProperties.ItemAmountText.text = Item.amount.ToString();
            }
        }
        if(slotType != SlotTypes.Smelt &&InventoryUIHandler.instance.craftingSystem != null)
        {
            InventoryUIHandler.instance.craftingSystem.UpdateCrafting();
        }
        //if(slotType == SlotTypes.Hotbar)
        //{
        //    //Set HotbarIndex;
        //}
    }
    private void Start()
    {
        //These are all nullchecks, to ensure that no true errors will appear.
        if(slotProperties.ItemAmountText != null)
        {
            if(slotProperties.ItemAmountText.raycastTarget != false)
                slotProperties.ItemAmountText.raycastTarget = false;
        }
        else
        {
            Debug.LogWarning($"Jimm's Inventory: Error! {nameof(slotProperties.ItemAmountText)} is null!");
        }
        if(slotProperties.IconImage == null)
        {
            Debug.LogWarning($"Jimm's Inventory: Error! {nameof(slotProperties.IconImage)} is null!");
        }
        if (slotProperties.BackgroundImage == null)
        {
            Debug.LogWarning($"Jimm's Inventory: Error! {nameof(slotProperties.BackgroundImage)} is null!");
        }
        if (SlotGroup == null)
        {
            Debug.LogWarning($"Jimm's Inventory: Error! {nameof(SlotGroup)} is null!");
        }
        if (slotProperties.Overlay == null)
        {
            Debug.LogWarning($"Jimm's Inventory: Error! {nameof(slotProperties.Overlay)} in {nameof(slotProperties)} is null!");
        }
    }
    /// <summary>
    /// There are 4 types of Inventory slots:
    /// <list type="number">
    ///     <item><u>Inventory:</u> The default type</item>
    ///     <item><u>HotbarSend:</u> Signals a UI update to the hotbar system, upon slot update. Functions like an inventory slot.</item>
    ///     <item><u>HotbarReceive:</u> A hotbar slot that belongs in the hotbar system. Cannot be modified directly</item>
    ///     <item><u>Smelt:</u> Update the smelting system when an interaction happens</item>
    ///     <item><u>Mouse:</u> Without it, your inventory is unsortable.</item>
    /// </list>
    /// </summary>
    public void UpdateSlotType(SlotTypes type)
    {
        slotType = type;
        if (slotType == SlotTypes.Mouse)
        {
            if(SlotGroup != null)
            {
                SlotGroup.blocksRaycasts = false;
                SlotGroup.interactable = false;
            }
            if(slotProperties.BackgroundImage != null)
            {
                slotProperties.BackgroundImage.enabled = false;
            }
        }
        else
        {
            if (SlotGroup != null)
            {
                SlotGroup.blocksRaycasts = true;
                SlotGroup.interactable = true;
            }
            if (slotProperties.BackgroundImage != null)
            {
                slotProperties.BackgroundImage.enabled = true;
            }
        }
    }
}