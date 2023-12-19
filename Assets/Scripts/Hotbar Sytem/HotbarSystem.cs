using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class HotbarSystem : MonoBehaviour
{
    public InventorySlot SlotPrefab;
    public Transform slotsParent;
    public List<InventorySlot> slots = new();
    public int SelectedSlot;
    public UnityEvent DefaultEvent;
    public Image SelectionOutline;
    [System.Serializable]
    public class Action
    {
        public Item item;
        public UnityEvent Event;
    }
    public Action[] actions;
    
    // Start is called before the first frame update
    void Start()
    {
        var hotbarSlotsAmount = InventoryUIHandler.instance.inventorySystem.HotbarSlotsCount;
        for (int i = 0; i < hotbarSlotsAmount; i++)
        {
            InventorySlot newSlot = Instantiate(SlotPrefab, slotsParent);
            //Set the slot's color to the correct color.
            newSlot.slotProperties.BackgroundImage.color = InventoryUIHandler.instance.inventorySystem.slotColors.Hotbar;
            //Set the slot's type to HotbarReceive.
            newSlot.UpdateSlotType(InventorySlot.SlotTypes.HotbarReceive);
            newSlot.UpdateSlot();
            slots.Add(newSlot);
        }
        SelectSlot(0);
        StartCoroutine(DelayedSetSelectionOutline());
    }
    //This had to be done because the 'SelectionOutline' would not go to the right spot otherwise.
    IEnumerator DelayedSetSelectionOutline()
    {
        yield return new WaitForSecondsRealtime(.1f);
        SelectionOutline.transform.position = slots[0].transform.position;
    }
    /// <summary>
    /// Will update the hotbar slot with the inventory's hotbar slot.
    /// </summary>
    public void UpdateHotbarSlot(int slotIndex, InventorySlot Sender)
    {
        slots[slotIndex].UpdateSlot(Sender.Item.item, Sender.Item.amount);
    }
    /// <summary>
    /// Selects a specific slot and activates an event with the slot's item.
    /// </summary>
    public void SelectSlot(int a)
    {
        SelectionOutline.transform.position = slots[a].transform.position;
        if (slots[a].Item.item == null || slots[a].Item.amount == 0) return;
        for (int i = 0; i < actions.Length; i++)
        {
            if (slots[a].Item.item == actions[i].item)
            {
                actions[i].Event.Invoke();
                return;
            }
        }
        DefaultEvent.Invoke();
    }
    // Update is called once per frame
    void Update()
    {
        //if the inventory is open, do not do anything.
        if (InventoryUIHandler.instance.IsOpen()) return;
        //Scroll to select the slots
        if (Input.mouseScrollDelta.y < 0)
        {
            SelectedSlot++;
            if(SelectedSlot == slots.Count)
            {
                SelectedSlot = 0;
            }
            SelectSlot(SelectedSlot);
        }
        else if(Input.mouseScrollDelta.y > 0)
        {
            SelectedSlot--;
            if (SelectedSlot < 0)
            {
                SelectedSlot = slots.Count-1;
            }
            SelectSlot(SelectedSlot);
        }
        string input = Input.inputString;
        int i;
        //Selecting Slots with the numbers on the keyboard.
        switch (input)
        {
            case "1":
                int.TryParse(input, out i);
                SwitchSelectSlot(i);
                break;
            case "2":
                int.TryParse(input, out i);
                SwitchSelectSlot(i);
                break;
            case "3":
                int.TryParse(input, out i);
                SwitchSelectSlot(i);
                break;
            case "4":
                int.TryParse(input, out i);
                SwitchSelectSlot(i);
                break;
            case "5":
                int.TryParse(input, out i);
                SwitchSelectSlot(i);
                break;
            case "6":
                int.TryParse(input, out i);
                SwitchSelectSlot(i);
                break;
            case "7":
                int.TryParse(input, out i);
                SwitchSelectSlot(i);
                break;
            case "8":
                int.TryParse(input, out i);
                SwitchSelectSlot(i);
                break;
            case "9":
                int.TryParse(input, out i);
                SwitchSelectSlot(i);
                break;
            case "0":
                SwitchSelectSlot(10);
                break;
        }
    }
    void SwitchSelectSlot(int slot)
    {
        if (slots.Count >= slot)
        {
            SelectedSlot = slot-1;
            SelectSlot(SelectedSlot);
        }
    }
}