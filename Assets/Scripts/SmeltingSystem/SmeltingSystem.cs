using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SmeltingSystem : MonoBehaviour
{
    public InventorySlot InputSlot;
    public InventorySlot FuelSlot;
    public InventorySlot OutputSlot;
    [Space]
    public Slider ProgressSlider;
    public Slider FuelSlider;

    [Space]

    public Smelter selectedSmelter;
    public SmeltingRecipe[] recipes;
    public List<SmeltingRecipe> CurrentWorkbenchRecipes = new();
    bool openSmelter;
    // Start is called before the first frame update
    void Start()
    {
        //Load all smelting recipes.
        recipes = Resources.LoadAll<SmeltingRecipe>("Smelting Recipes");
    }
    /// <summary>
    /// Opens the smelter.
    /// </summary>
    /// <param name="smelter"></param>
    public void OpenSmelter(Smelter smelter)
    {
        //If the smelter is already open there's no reason to open it again.
        if (openSmelter) return;
        selectedSmelter = smelter;
        
        //Set and update the input slot to the smelter's input.
        InputSlot.Item = smelter.Input;
        InputSlot.UpdateSlot();
        
        //Set and update the fuel slot to the smelter's fuel.
        FuelSlot.Item = smelter.Fuel;
        FuelSlot.UpdateSlot();

        //Set and update the output slot to the smelter's output.
        OutputSlot.Item = smelter.Output;
        OutputSlot.UpdateSlot();
        //Loop through all recipes to find all recipes that belong on the current workstation (smelter).
        for (int i = 0; i < recipes.Length; i++)
        {
            if (recipes[i].RequiredWorkstation == selectedSmelter.WorkstationName)
            {
                CurrentWorkbenchRecipes.Add(recipes[i]);
            }
        }
        openSmelter = true;
    }
    //Closes the smelter
    public void CloseSmelter()
    {
        if (!openSmelter) return;
        CurrentWorkbenchRecipes.Clear();
        openSmelter = false;
        InventoryUIHandler.instance.GetGroup(InventoryUIHandler.instance.SmeltingUIName).Activate(false);
    }
    /// <summary>
    /// Initiates the smelting process on the smelter, after ensuring that all slots are ok. 
    /// </summary>
    public void AttemptSmelt()
    {
        selectedSmelter.Input = InputSlot.Item;
        selectedSmelter.Fuel = FuelSlot.Item;
        selectedSmelter.Output = OutputSlot.Item;
        //Loop through all the smelter's recipes to detect the recipe we're trying to make.
        for (int i = 0; i < CurrentWorkbenchRecipes.Count; i++)
        {
            var cwr = CurrentWorkbenchRecipes[i];
            //if:
            //  -The input matches the recipe's ingredient
            //  -The input has the recipe's required amount of items (or above)
            //  -The output slot either is empty, or has the item we're making
            //  -There's enough space on the output
            if
                (
                    cwr.Ingredient.item == InputSlot.Item.item &&
                    cwr.Ingredient.amount <= InputSlot.Item.amount &&
                    (OutputSlot.Item.item == cwr.Result &&
                    OutputSlot.Item.amount + cwr.ResultAmount <= cwr.Result.StackSize) || (OutputSlot.Item.item == null || OutputSlot.Item.amount==0)
                )
            {
                //Initiate the smelting process and return.
                selectedSmelter.Smelt();
                return;
            }
        }
    }
     /// <summary>
     /// Tries to find a recipe with our specified ingredient. 
     /// </summary>
    public SmeltingRecipe GetRecipe(Item ingredient)
    {
        for (int i = 0; i < CurrentWorkbenchRecipes.Count; i++)
        {
            var cwr = CurrentWorkbenchRecipes[i];
            if(ingredient == cwr.Ingredient.item)
            {
                return cwr;
            }
        }
        return null;
    }
    /// <summary>
    /// Sets the Fuel Slider's max value to our specified amount.
    /// </summary>
    public void SetMaxFuel_UI(float fuel)
    {
        FuelSlider.maxValue = fuel;
    }
    /// <summary>
    ///Sets the Progress Slider's max value to our specified amount.
    /// </summary>
    public void SetMaxProgress_UI(float progress)
    {
        ProgressSlider.maxValue = progress;
    }
    /// <summary>
    ///Sets the Fuel Slider's current value to our specified amount.
    /// </summary>
    public void SetFuel_UI(float fuel)
    {
        FuelSlider.value = fuel;
    }
    /// <summary>
    ///Sets the Progress Slider's current value to our specified amount.
    /// </summary>
    public void SetProgress_UI(float progress)
    {
        ProgressSlider.value = progress;
    }
    /// <summary>
    ///Updates the Smelter System's slots.
    /// </summary>
    public void UpdateSlotsUI()
    {

        InputSlot.Item = selectedSmelter.Input;
        InputSlot.UpdateSlot();

        FuelSlot.Item = selectedSmelter.Fuel;
        FuelSlot.UpdateSlot();

        OutputSlot.Item = selectedSmelter.Output;
        OutputSlot.UpdateSlot();
    }
}