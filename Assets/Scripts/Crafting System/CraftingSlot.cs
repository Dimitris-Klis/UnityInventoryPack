using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class CraftingSlot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Tooltip("The Recipe of the crafting slot.")]
    public Recipe recipe;
    public InventorySlot.SlotProperties slotProperties;
    [Space(20)]
    public Image UncraftableImage;
    // Start is called before the first frame update
    void Start()
    {
        //These are all nullchecks, to ensure that no true errors will appear.
        if (slotProperties.ItemAmountText != null)
        {
            if (slotProperties.ItemAmountText.raycastTarget != false)
                slotProperties.ItemAmountText.raycastTarget = false;
        }
        else
        {
            Debug.LogWarning($"Jimm's Inventory: Error! {nameof(slotProperties.ItemAmountText)} is null!");
        }
        if (slotProperties.IconImage == null)
        {
            Debug.LogWarning($"Jimm's Inventory: Error! {nameof(slotProperties.IconImage)} is null!");
        }
        if (slotProperties.BackgroundImage == null)
        {
            Debug.LogWarning($"Jimm's Inventory: Error! {nameof(slotProperties.BackgroundImage)} is null!");
        }
        //if (SlotGroup == null)
        //{
        //    Debug.LogWarning($"Jimm's Inventory: Error! {nameof(SlotGroup)} is null!");
        //}
        if (slotProperties.Overlay == null)
        {
            Debug.LogWarning($"Jimm's Inventory: Error! {nameof(slotProperties.Overlay)} in {nameof(slotProperties)} is null!");
        }
    }
    public void UpdateSlot()
    {
        //Update the icon to the recipe's result icon.
        slotProperties.IconImage.sprite = recipe.Result.Icon;
    }
    public void UpdateCraftability(int CraftableItems)
    {
        //If we can craft at least one of this item
        if (CraftableItems != 0)
        {
            //set the text to the amount we can craft.
            slotProperties.ItemAmountText.text = CraftableItems.ToString();
            //Disable the uncraftable overlay.
            UncraftableImage.enabled = false;
        }
        else //Otherwise
        {
            //Set the text to nothing.
            slotProperties.ItemAmountText.text = "";
            //Enable the uncraftable overlay.
            UncraftableImage.enabled = true;
        }
         if(mouseOver)
            OpenTooltip();
    }
    bool mouseOver;
    void OpenTooltip()
    {
        string message = $"{recipe.Result.Name} (x{recipe.ResultAmount})\n<size=80%>{recipe.Result.Description}</size>";
        for (int i = 0; i < recipe.Ingredients.Length; i++)
        {
            int currentAmount = InventoryUIHandler.instance.inventorySystem.CountItemAmount(recipe.Ingredients[i].item);
            string ValidAmountColor = "<color=#00ff00>";
            string InvalidAmountColor = "<color=#ff0000>";
            message += $"\n{recipe.Ingredients[i].item.Name}:{(currentAmount >= recipe.Ingredients[i].amount ? ValidAmountColor : InvalidAmountColor)}{currentAmount}/{recipe.Ingredients[i].amount}</color>";
        }
        Tooltip.instance.OpenTooltip(message);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseOver = true;
        if (slotProperties.Overlay != null)
            slotProperties.Overlay.color = slotProperties.Hover;
        OpenTooltip();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseOver = false;
        if (slotProperties.Overlay != null)
            slotProperties.Overlay.color = slotProperties.Default;
        Tooltip.instance.CloseTooltip();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (slotProperties.Overlay != null)
            slotProperties.Overlay.color = slotProperties.Click;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (slotProperties.Overlay != null)
            slotProperties.Overlay.color = slotProperties.Hover;
    }
    //This is where the button behavior happens
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!UncraftableImage.enabled)
        {
            InventoryUIHandler.instance.craftingSystem.CraftItem(this);
        }
    }
}
