<h1>My Inventory Pack</h1>
<b>Heads-up: All scripts were written C#</b>
<h2>Contents</h2>
<h3>This Inventory Pack Includes:</h3>
<ol>
  <li>An Inventory System</li>
  <li>A Crafting System</li>
  <li>A Chest System</li>
  <li>A Hotbar System</li>
  <li>A tooltip</li>
</ol>
<h2>Documentation:</h2>
<h3>Adding & Removing Items</h3>
<p>
  If you want to add or remove items from the inventory (in code), find the <u>Inventory System</u>
  script with <em>InventoryUIHandler.instance.inventorySystem</em> and call <em>"CountItemAmount(item (Item))"</em>.<br>
  If you instead want to check how much space there is for an item, call <em>"CheckSpaceForItem(item (Item))"</em> instead.
</p>
<h3>Checking for items and for item space</h3>
<p>
  If you want to check how many items are in your inventory, you can simply find the <u>Inventory System</u>
  script as <em>InventoryUIHandler.instance.inventorySystem</em> and calling <em>"AddItem(item (Item), amount (int))"</em>
  or <em>"RemoveItem(item (Item), amount (int))"</em>. <b>Make sure the item exists!</b>
</p>
You're going to have to make your own pickup script (2D or 3D) which could look a bit like this:
<hr>
using UnityEngine;
public class Pickup: Monobehavior
{
  //This is a 2D example
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
    if(Physics2D.OverlapCircle((Vector2)transform.position, PickupRadius, PlayerMask))
    {
      //If We have enough space for an item and we press 'e' (or any other keyCode we want).
      if(Input.GetKeyDown(InteractionKeyCode) && CheckSpaceForItem(itemToGive) >= AmountToGive)
      {
        //Add the item to the inventory.
        InventoryUIHandler.instance.inventorySystem.Add(itemToGive, AmountToGive);
        Destroy(this.gameObject);
      }
    }
  }
}
