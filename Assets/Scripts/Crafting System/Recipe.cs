using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Recipe", menuName = "Jimm's Inventory/Create Recipe", order = 1)]
public class Recipe : ScriptableObject
{
    [Tooltip("The workbench that will show these recipes.")]
    public string RequiredWorkstation;
    public InventorySystem.InventoryItem[] Ingredients;

    public Item Result;
    public int ResultAmount;
}