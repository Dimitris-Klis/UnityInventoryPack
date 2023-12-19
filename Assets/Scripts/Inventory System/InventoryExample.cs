using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script was made for debugging purposes. You are free to remove this from the project.
public class InventoryExample : MonoBehaviour
{
    [System.Serializable]
    public class ItemToGive
    {
        public KeyCode key = KeyCode.Alpha1;
        public Item item;
        public int amount = 1;
    }
    [SerializeField] ItemToGive[] itemsToGive;
    [Space]
    public Chest chest;
    public Smelter smelter;
    public KeyCode OpenChestKeyCode;
    public KeyCode OpenSmelterKeyCode;

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < itemsToGive.Length; i++)
        {
            if (Input.GetKeyDown(itemsToGive[i].key))
            {
                InventoryUIHandler.instance.inventorySystem.AddItem(itemsToGive[i].item, itemsToGive[i].amount);
            }
        }
        if (Input.GetKeyDown(OpenChestKeyCode))
        {
            chest.OpenChest();
        }
        if (Input.GetKeyDown(OpenSmelterKeyCode))
        {
            smelter.OpenSmelter();
        }
    }
}
