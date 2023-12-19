using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [Tooltip("The items this chest is storing. For blank slots leave the item amount to 0. To add more slots, just add to the array")]
    public List<InventorySystem.InventoryItem> items;
    public void OpenChest()
    {
        InventoryUIHandler.instance.chestSystem.OpenChest(this);
    }
}
