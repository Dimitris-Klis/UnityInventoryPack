using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName ="Jimm's Inventory/Create Item", order = 0)]
public class Item : ScriptableObject
{
    public string Name;
    [TextArea(4,8)]
    public string Description;
    [Space]
    public Sprite Icon;
    [Space]
    public int StackSize = 100;
    [Space]
    [Tooltip("Whether it can be used as fuel.")]
    public bool Fuel;
    [Tooltip("How long it will last as fuel (seconds).")]
    public float FuelTime;

    [Tooltip("Whether it can be placed in the world.")]
    public bool Building;
    [Space(20)]
    [Tooltip("What prefab will be spawned to place in the world.")]
    public GameObject BuildingPrefab;
}
