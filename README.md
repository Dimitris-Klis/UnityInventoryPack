<h1>My Inventory Pack</h1>
<b>Heads-up: All scripts are written in C#</b>
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
  script with <em>InventoryUIHandler.instance.inventorySystem</em> and call <em>"AddItem(item (Item)"</em>.<br>
  <br><br>
  If you instead want to check how much space there is for an item, call <em>"CheckSpaceForItem(item (Item))"</em> instead.
  <br><br>
  Use <em>"RemoveItem(item (Item), amount (int))"</em> to remove an item.
</p>
<h3>Checking for items and for item space</h3>
<p>
  If you want to check how many items are in your inventory, you can simply find the <u>Inventory System</u>
  script as <em>InventoryUIHandler.instance.inventorySystem</em> and calling <em>"CountItemAmount(item (Item))"</em> , amount (int))"</em>. <b>Make sure the item exists!</b>
</p>
Let's see how a pickup script would look like:
<hr>
https://github.com/Dimitris-Klis/UnityInventoryPack/blob/ac063f332c91e0bfebbc775ff47778bd75920d0e/Assets/Scenes/Example/Example%20Assets/Pickup.cs#L1-L29
