using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestSystem : MonoBehaviour
{
    [Tooltip("Where slots will be generated. Grid Layout Group & Scrollbar are recommended.")]
    public Transform SlotsParent;
    [Tooltip("The slot that will be generated.")]
    public InventorySlot slotPrefab;
    [Tooltip("This will overwrite the original color of chest slots.")]
    List<InventorySlot> slots = new();
    public Color DefaultSlotColor = Color.white;
    public Chest CurrentChest;
    bool openChest;
    /// <summary>
    /// Opens the chest, meaning we instantiate slots and assign the chest's items to them.
    /// </summary>
    public void OpenChest(Chest chest)
    {
        //If the chest is already open, there's no reason to open it again.
        if (openChest) return;
        CurrentChest = chest;
        //loop through the items in our chest and spawn slots with the correct items.
        for (int i = 0; i < CurrentChest.items.Count; i++)
        {
            //Spawn slots either in 'slots parent'
            InventorySlot newSlot = Instantiate(slotPrefab, SlotsParent);
            //Set the slot's color to the correct color;
            newSlot.slotProperties.BackgroundImage.color = DefaultSlotColor;
            //Update the slot's UI.
            newSlot.UpdateSlot(CurrentChest.items[i].item, CurrentChest.items[i].amount);
            slots.Add(newSlot);
        }
        //Show the Inventory UI.
        InventoryUIHandler.instance.OpenInventoryWithChest();
        openChest = true;
    }/// <summary>
     /// Closes the chest by hiding the inventory UI, deleting the slots and setting the items in the chest.
     /// </summary>
    public void CloseChest()
    {
        //If the chest is already closed, there's no reason to close it again.
        if (!openChest) return;
        CurrentChest.items.Clear();
        for (int i = 0; i < slots.Count; i++)
        {
            CurrentChest.items.Add(slots[i].Item);
            Destroy(slots[i].gameObject);
        }
        slots.Clear();
        InventoryUIHandler.instance.GetGroup(InventoryUIHandler.instance.ChestUIName).Activate(false);
        openChest = false;
    }
}
