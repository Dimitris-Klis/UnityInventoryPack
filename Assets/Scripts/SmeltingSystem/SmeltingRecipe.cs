using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Smelting Recipe", menuName = "Jimm's Inventory/Create Smelting Recipe", order = 2)]
public class SmeltingRecipe : ScriptableObject
{
    [Tooltip("The workbench that will show these recipes.")]
    public string RequiredWorkstation;
    public InventorySystem.InventoryItem Ingredient;
    [Space]
    [Tooltip("The time it will take for the item to be smelted.")]
    public float CookTime;
    public Item Result;
    public int ResultAmount;
    
}
