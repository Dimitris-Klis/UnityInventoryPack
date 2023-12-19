using UnityEngine;

public class Pickup : MonoBehaviour
{
    //This is a 2D example. It is not used in the example scene.
    [SerializeField] int AmountToGive;
    [SerializeField] Item itemToGive;
    [Space]
    [SerializeField] LayerMask PlayerMask;
    [SerializeField] float PickupRadius;
    [SerializeField] KeyCode InteractionKeyCode = KeyCode.E;
    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, PickupRadius);
    }
    void Update()
    {
        if (Physics2D.OverlapCircle((Vector2)transform.position, PickupRadius, PlayerMask))
        {
            //If We have enough space for an item and we press 'e' (or any other keyCode we want).
            if (Input.GetKeyDown(InteractionKeyCode) && InventoryUIHandler.instance.inventorySystem.CheckSpaceForItem(itemToGive) >= AmountToGive)
            {
                //Add the item to the inventory.
                InventoryUIHandler.instance.inventorySystem.AddItem(itemToGive, AmountToGive);
                Destroy(this.gameObject);
            }
        }
    }
}