using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    //A class that holds a specified item and its amount.
    [System.Serializable]
    public class InventoryItem
    {
        public Item item;
        public int amount;
    }
    [Tooltip("How many slots should be generated")]
    public int SlotsCount = 32;
    [Tooltip("How many of the slots generated will be Hotbar Slots?")]
    public int HotbarSlotsCount = 8;

    [Tooltip("Where slots will be generated. Grid Layout Group & Scrollbar are recommended.")]
    public Transform SlotsParent;
    [Tooltip("Where hotbar related slots will be generated. Grid Layout Group is recommended.")]
    public Transform HotbarSlotsParent;
    [Tooltip("Where the mouse slot will be generated.")]
    public Transform MouseSlotParent;
    [Tooltip("The slot that will be generated.")]
    public InventorySlot slotPrefab;
    [System.Serializable]
    public class SlotColors
    {
        [Tooltip("The color of a slot that isn't part of the hotbar.")]
        public Color Default = Color.white;
        [Tooltip("The color of a slot that is part of the hotbar.")]
        public Color Hotbar = Color.white;
    }
    [Tooltip("This will overwrite the original colors of slots that also affect the hotbar.")]
    public SlotColors slotColors;
    List<InventorySlot> slots = new();
    InventorySlot MouseSlot;


    //Movable Items.+
    //Stack Sizes.+
    //Chests.+
    //Dont add new stuff to inventory if not enough space.+
    //Dont allow crafting if not enough space.+
    //Hotbar +

    // Start is called before the first frame update
    void Awake()
    {
        SetupSlots();
    }
    private void Update()
    {
        MouseSlot.transform.position = Input.mousePosition;
        //If the mouse slot is not empty, display it.
        MouseSlot.SlotGroup.alpha = (MouseSlot.Item.item != null && MouseSlot.Item.amount > 0) ? 1 : 0;
    }
    /// <summary>
     /// Sets up all slots, including mouse and hotbar slots. This is in a function to prevent confusion and spaghettified code.
     /// </summary>
    void SetupSlots()
    {
        int hotbarindex = 0;
        for (int i = 0; i < SlotsCount; i++)
        {
            //Check which slots will be hotbar slots. This is set by 'HotbarSlotsCount'.
            bool IsHotbarSlot = i >= SlotsCount - HotbarSlotsCount;

            //Spawn slots either in 'slots parent' or the 'hotbar slots parent'.
            InventorySlot newSlot = Instantiate(slotPrefab, !IsHotbarSlot ? SlotsParent : HotbarSlotsParent);


            //Set the slot's color to the correct color.
            newSlot.slotProperties.BackgroundImage.color = !IsHotbarSlot ? slotColors.Default : slotColors.Hotbar;
            //If it's a hotbar slot, set its type to HotbarSend.
            if (IsHotbarSlot)
            {
                newSlot.UpdateSlotType(InventorySlot.SlotTypes.HotbarSend);
                newSlot.hotbarIndex = hotbarindex;
                hotbarindex++;
            }

            newSlot.UpdateSlot(null, 0);
            slots.Add(newSlot);
        }
        //Spawn Mouse Slot.
        MouseSlot = Instantiate(slotPrefab, MouseSlotParent);
        MouseSlot.UpdateSlotType(InventorySlot.SlotTypes.Mouse);
    }
    /// <summary>
    /// Checks how much space is available for a specified item.
    /// </summary>
    /// <returns>Available space for item (int)</returns>
    public int CheckSpaceForItem(Item item)
    {
        int spaceForItem = 0;
        for (int i = 0; i < slots.Count; i++)
        {
            //If a slot with a matching item exists, count the amount of items you can add.
            if (slots[i].Item.item == item)
            {
                spaceForItem += (item.StackSize - slots[i].Item.amount);
            }
            //If a clear slot is detected, add the stacksize to the total.
            else if (SlotIsEmpty(slots[i]))
            {
                spaceForItem += item.StackSize;
            }
        }
        return spaceForItem;
    }
    /// <summary>
    /// Counts the amount of a specified item.
    /// </summary>
    /// <returns>Number of items (int)</returns>
    public int CountItemAmount(Item item)
    {
        int Amount = 0;
        for (int i = 0; i < slots.Count; i++)
        {
            //If a slot with a matching item exists, add the item amount to the total.
            if (slots[i].Item.item == item)
            {
                Amount += slots[i].Item.amount;
            }
        }
        return Amount;
    }
    /// <summary>
    /// Adds a specified item with a specified amount to the inventory, if there's enough space.
    /// </summary>
    public void AddItem(Item item, int amount)
    {
        int remaining = amount;
        //First, we iterate through all slots with the same item.
        for (int i = 0; i < slots.Count; i++)
        {
            //If we found a slot matching our item and it hasn't reached the stacksize
            if (slots[i].Item.item == item && slots[i].Item.amount < item.StackSize)
            {
                //How much we can add to the slot = StackSize - how many items are already in the slot.
                int AvailableAmount = item.StackSize - slots[i].Item.amount;
                //'Amount to add' is either equal to the 'available amount' (if 'remaining' exceeds it), or the opposite (if 'available amount' is more than the 'remaing').
                int amountToAdd = Mathf.Min(remaining, AvailableAmount);
                //Add the amount we should be adding.
                slots[i].Item.amount += amountToAdd;
                //Remove from the remaining amount, the amount we added.
                remaining -= amountToAdd;
                slots[i].UpdateSlot();
                if (remaining == 0) return;
            }
        }
        //Then, if we still have a remainder, iterate through all the clear slots.
        for (int i = 0; i < slots.Count; i++)
        {
            if (SlotIsEmpty(slots[i]))
            {
                slots[i].Item.item = item;
                int AvailableAmount = item.StackSize /*- slots[i].Item.amount --> Not needed since we know the slot is clear*/;
                int amountToAdd = Mathf.Min(remaining, AvailableAmount);
                slots[i].Item.amount += amountToAdd;
                remaining -= amountToAdd;
                slots[i].UpdateSlot();
                if (remaining == 0) return;
            }
        }
        //If in the end AmountToAdd is more than 0: Either implement a function that drops excess items, or prevent this scenario altogether.
        if (remaining > 0) Debug.Log("Jimm's Inventory: Your Inventory Is Full! Add a function of your liking to deal with this problem.");
    }
    /// <summary>
    /// Removes a specified amount of a specified item, from the inventory.
    /// </summary>
    public void RemoveItem(Item item, int amount)
    {
        int amountToRemove = amount;
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].Item.item == item)
            {
                int tempAmountToRemove = Mathf.Min(slots[i].Item.amount, amountToRemove);
                slots[i].Item.amount -= tempAmountToRemove;
                amountToRemove -= tempAmountToRemove;
                slots[i].UpdateSlot();
                if (amountToRemove == 0) return;
            }
        }
        //If in the end AmountToRemove is more than 0, you've tried to remove more items than you have.
        if (amountToRemove > 0) Debug.Log("You tried to remove more items than you have.");
    }

    /// <summary>
    /// Swaps the mouseSlot's contents with a specified slot's contents.
    /// Adds items from mouseSlot to slot if they contain the same item.
    /// </summary>
    public void SwapWithMouse(InventorySlot slot)
    {
        //If both the mouse slot and the inventorySlot have no items, there's no reason to continue this function.
        if (SlotIsEmpty(slot) && SlotIsEmpty(MouseSlot)) return;

        //If MouseSlot and slot contain the same item
        if(MouseSlot.Item.item == slot.Item.item)
        {
            //How much we can add to the slot = StackSize - how many items are already in the slot.
            int AvailableAmount = slot.Item.item.StackSize - slot.Item.amount;
            //'Amount to add' is either equal to the 'available amount' (if 'remaining' exceeds it), or the opposite (if 'available amount' is more than the 'remaing').
            int amountToAdd = Mathf.Min(MouseSlot.Item.amount, AvailableAmount);
            if(amountToAdd > 0)
            {
                MouseSlot.Item.amount -= amountToAdd;
                slot.Item.amount += amountToAdd;
                MouseSlot.UpdateSlot();
                slot.UpdateSlot();
                return;
            }
        }
        //Temporary storing the mouseSlot's contents.
        InventoryItem MouseItem = new() { item = MouseSlot.Item.item, amount=MouseSlot.Item.amount };
        //Setting MouseSlot to the InventorySlot.
        MouseSlot.UpdateSlot(slot.Item.item, slot.Item.amount);
        //Setting InventorySlot to the MouseSlot.
        slot.UpdateSlot(MouseItem.item, MouseItem.amount);
        if (slot.slotType == InventorySlot.SlotTypes.HotbarSend) InventoryUIHandler.instance.hotbarSystem.UpdateHotbarSlot(slot.hotbarIndex, slot);
        InventoryUIHandler.instance.craftingSystem.UpdateCrafting();
    }
    /// <summary>
    /// Moves a specified amount of a specified Slot's item to the MouseSlot or the opposite.
    /// If the MouseSlot is empty, half of the amount will go from slot to mouse.
    /// 
    /// </summary>
    public void MouseSlotExchange(InventorySlot slot)
    {
        bool emptySlot = SlotIsEmpty(slot);
        bool emptyMouse = SlotIsEmpty(MouseSlot);
        //If both the mouse slot and the inventorySlot have no items, there's no reason to continue this function.
        if (emptySlot && emptyMouse) return;
        //If MouseSlot and NormalSlot items don't match and the Normal slot contains an item, we can't do do anything.
        if (!emptyMouse && !emptySlot && MouseSlot.Item.item != slot.Item.item ) return;

        if (emptyMouse)
        {
            MouseSlot.UpdateSlot(slot.Item.item, slot.Item.amount/2 + slot.Item.amount%2);
            slot.Item.amount /= 2;
            slot.UpdateSlot();
        }
        else
        {
            MouseSlot.Item.amount--;
            slot.Item.amount++;
            slot.Item.item = MouseSlot.Item.item;
            MouseSlot.UpdateSlot();
            slot.UpdateSlot();
        }
        if (slot.slotType == InventorySlot.SlotTypes.HotbarSend) InventoryUIHandler.instance.hotbarSystem.UpdateHotbarSlot(slot.hotbarIndex, slot);
        InventoryUIHandler.instance.craftingSystem.UpdateCrafting();
    }

    //Check if slot is empty.
    public bool SlotIsEmpty(InventorySlot slot)
    {
        if (slot.Item == null || slot.Item.amount == 0) return true;
        else return false;
    }
    public InventorySlot GetSlot(int i)
    {
        if(i>=0 && i< slots.Count)
        {
            return slots[i];
        }
        return null;
    }
}