using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingSystem : MonoBehaviour
{
    public List<CraftingSlot> craftingSlots = new();
    public CraftingSlot CraftingSlotPrefab;
    public Transform CraftingSlotsParent;
    public Recipe[] recipes;
    // Start is called before the first frame update
    void Start()
    {
        recipes = Resources.LoadAll<Recipe>("Recipes");
    }

    /// <summary>
    /// Opens the crafting menu, by generating slots with our given workstation's recipes and activating the UI.
    /// </summary>
    public void OpenCrafting(string workstation_name)
    {
        for (int i = 0; i < recipes.Length; i++)
        {
            if (recipes[i].RequiredWorkstation == workstation_name)
            {
                CraftingSlot NewSlot = Instantiate(CraftingSlotPrefab, CraftingSlotsParent);
                NewSlot.recipe = recipes[i];
                NewSlot.UpdateSlot();
                CheckCraftableAmount(NewSlot);
                craftingSlots.Add(NewSlot);
            }
        }
    }/// <summary>
     /// Closes the crafting menu, by destroying all the crafting slots and deactivating the UI.
     /// </summary>
    public void CloseCrafting()
    {
        foreach(CraftingSlot slot in craftingSlots)
        {
            Destroy(slot.gameObject);
        }
        craftingSlots.Clear();
    }
    /// <summary>
    /// Updates which items are craftable.
    /// </summary>
    public void UpdateCrafting()
    {
        if(craftingSlots.Count > 0)
        {
            for (int i = 0; i < craftingSlots.Count; i++)
            {
                CheckCraftableAmount(craftingSlots[i]);
            }
        }
    }
    /// <summary>
    /// Checks how many items are craftable and updates the slot's craftability.
    /// </summary>
    public void CheckCraftableAmount(CraftingSlot slot)
    {
        //Make a list of the most items you can craft with each ingredient.
        List<int> craftableItems = new();
        //Loop through the recipe's ingredients.
        for (int i = 0; i < slot.recipe.Ingredients.Length; i++)
        {
            //Save some time by making a var with a smaller name.
            var ing = slot.recipe.Ingredients[i];
            //The craftableAmount is the amount of the items you have divided by the required ingredient amount.
            float craftableAmount = InventoryUIHandler.instance.inventorySystem.CountItemAmount(ing.item) / ing.amount;
            //If the amount is more or equal to one, we can add the amount to the list as a floored interger.
            if (craftableAmount >= 1)
            {
                craftableItems.Add(Mathf.FloorToInt(craftableAmount));
            }
            //If it's not, that means we can't even craft the item, so there is no reason to continue the function.
            else
            {
                slot.UpdateCraftability(0);
                return;
            }
        }
        //If we get out of the loop normally, the next step can beign:
        //Find the smallest value in the list. That's the amount of items we can craft.

        //Set the smallest value to the first item (or any other item) in the list.
        int smallestvalue = craftableItems[0];
        //Loop through all the items in the list
        for (int i = 0; i < craftableItems.Count; i++)
        {
            //If the item we're looping through is less than our current smallestvalue, assign smallest value with the item.
            if (craftableItems[i] < smallestvalue)
            {
                smallestvalue = craftableItems[i];
            }
        }
        //after the loop we should have the smallest value, so we update the crafting slot.
        slot.UpdateCraftability(smallestvalue);
    }
    //Assuming that the previous checks prevent the player from crafting
    //uncraftable items, this will likely remove only existing items.
    public void CraftItem(CraftingSlot slot)
    {
        int space = InventoryUIHandler.instance.inventorySystem.CheckSpaceForItem(slot.recipe.Result);
        if(space < slot.recipe.ResultAmount)
        {
            Debug.Log($"Jimm's Inventory: There is not enough space to craft {slot.recipe.Result.Name}.");
            return;
        }
        for (int i = 0; i < slot.recipe.Ingredients.Length; i++)
        {
            var ing = slot.recipe.Ingredients[i];
            InventoryUIHandler.instance.inventorySystem.RemoveItem(ing.item, ing.amount);
        }
        InventoryUIHandler.instance.inventorySystem.AddItem(slot.recipe.Result, slot.recipe.ResultAmount);
    }
}