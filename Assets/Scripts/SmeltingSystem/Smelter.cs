using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smelter : MonoBehaviour
{
    public string WorkstationName;
    public InventorySystem.InventoryItem Input;
    public InventorySystem.InventoryItem Fuel;
    public InventorySystem.InventoryItem Output;

    public float FuelAmount;
    public float ProgressAmount;
    public int QueuedSmelting;
    public SmeltingRecipe currentRecipe;
    /// <summary>
    /// Opens the smelter UI.
    /// </summary>
    public void OpenSmelter()
    {
        InventoryUIHandler.instance.smeltingSystem.OpenSmelter(this);
        InventoryUIHandler.instance.OpenInventoryWithSmelter();
        InventoryUIHandler.instance.smeltingSystem.SetProgress_UI(ProgressAmount);
        InventoryUIHandler.instance.smeltingSystem.SetFuel_UI(FuelAmount);
    }

    /// <summary>
    /// Starts smelting the input if the recipe is valid, there's fuel and an available ouput.
    /// </summary>
    public void Smelt()
    {
        if (QueuedSmelting > 0) return;
        currentRecipe = InventoryUIHandler.instance.smeltingSystem.GetRecipe(Input.item);
        //if there's a recipe with the input's item, fuel and an available output
        if
            (
                currentRecipe != null &&

                QueuedSmelting == 0 &&
                Fuel.amount > 0 &&
                Fuel.item.Fuel &&

                (Input.amount >= currentRecipe.Ingredient.amount && Input.item != null) &&
                Output.amount + currentRecipe.ResultAmount <= currentRecipe.Result.StackSize
            )
        {
            //Remove the required amount from the input
            Input.amount -= currentRecipe.Ingredient.amount;
            //add 1 to QueuedSmelting
            QueuedSmelting++;
            //Update slots and sliders
            InventoryUIHandler.instance.smeltingSystem.UpdateSlotsUI();
            InventoryUIHandler.instance.smeltingSystem.SetMaxProgress_UI(currentRecipe.CookTime);
            nothingthere = false;
        }
        else nothingthere = true;
    }
    bool nothingthere;
    // Update is called once per frame
    void Update()
    {
        if(QueuedSmelting > 0)
        {
            //If fuelAmount runs out and there's still fuel in the fuel slot.
            if(FuelAmount <= 0 && Fuel.amount > 0)
            {
                //Restore the fuelAmount
                FuelAmount = Fuel.item.FuelTime;
                //Remove 1 item from the fuel slot.
                Fuel.amount--;
                //Update slots and sliders.
                InventoryUIHandler.instance.smeltingSystem.SetMaxFuel_UI(Fuel.item.FuelTime);
                InventoryUIHandler.instance.smeltingSystem.UpdateSlotsUI();
            }
            //Otherwise if there's fuel and the output is available
            else if(FuelAmount > 0 && 
                ((Output.item == null || Output.amount == 0) ||
                (Output.item == currentRecipe.Result && Output.amount+currentRecipe.ResultAmount <= currentRecipe.Result.StackSize)))
            {
                //Update sliders
                InventoryUIHandler.instance.smeltingSystem.SetProgress_UI(ProgressAmount);
                InventoryUIHandler.instance.smeltingSystem.SetFuel_UI(FuelAmount);
                //Progress goes up
                ProgressAmount += Time.deltaTime;
                //Fuel goes down
                FuelAmount -= Time.deltaTime;

                //If progress reaches max
                if (ProgressAmount >= currentRecipe.CookTime)
                {
                    //Add item to output
                    Output.item = currentRecipe.Result;
                    Output.amount += currentRecipe.ResultAmount;
                    //Update Slots
                    InventoryUIHandler.instance.smeltingSystem.UpdateSlotsUI();
                    //Reset progress to 0 and Update Progress Slider
                    ProgressAmount = 0;
                    InventoryUIHandler.instance.smeltingSystem.SetProgress_UI(ProgressAmount);
                    //Remove 1 from QueuedSmelting
                    QueuedSmelting--;
                }
            }
        }
        else
        {
            //If QueuedSmelting is 0 and nothingthere is true, try smelting.
            if(!nothingthere)
                Smelt();
        }
    }
}
