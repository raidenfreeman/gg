using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Reflection;

public class DispenserTest
{
    [Test]
    public void TryPickUpItem_NoItemsPlaced_SuccessfulyReturnDispensedItemAndPlacedItemEmpty()
    {
        var dispenser = new GameObject().AddComponent<Dispenser>();
        Assert.IsNull(dispenser.placedItem); // No items are placed
        var dispensibleItem = new GameObject().AddComponent<FoodIngredient>();
        typeof(Dispenser)
            .GetField("itemToDispense", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(dispenser, dispensibleItem);
        var dispensedItem = dispenser.TryPickUpItem();
        Assert.IsNotNull(dispensedItem); // It should have returned an item
        Assert.IsNull(dispenser.placedItem); // Placed item should be empty
    }

    [Test]
    public void TryPickupItem_ItemPlaced_ReturnPlacedItem()
    {
        var dispenser = new GameObject().AddComponent<Dispenser>();
        var item = new GameObject().AddComponent<FoodIngredient>();
        typeof(Dispenser)
            .GetProperty("placedItem", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(dispenser, item);
        var pickedItem = dispenser.TryPickUpItem();
        Assert.AreSame(item, pickedItem);
    }

    [Test]
    public void TryPlaceItem_ItemPlaced_ReturnFalse()
    {
        var dispenser = new GameObject().AddComponent<Dispenser>();
        var existingItem = new GameObject().AddComponent<FoodIngredient>();
        var newItem = new GameObject().AddComponent<FoodIngredient>();
        typeof(Dispenser)
            .GetProperty("placedItem", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(dispenser, existingItem);
        var didPlaceItem = dispenser.TryPlaceItem(newItem);
        Assert.IsFalse(didPlaceItem);
        Assert.AreNotSame(newItem, dispenser.placedItem);
    }

    [Test]
    public void TryPlaceItem_NoItemPlaced_ReturnTruePlacedItemHasChanged()
    {
        var dispenser = new GameObject().AddComponent<Dispenser>();
        Assert.IsNull(dispenser.placedItem);
        var newItem = new GameObject().AddComponent<FoodIngredient>();
        typeof(FoodIngredient)
            .GetField("placementAnchor", BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(newItem, new GameObject().transform);
        var didPlaceItem = dispenser.TryPlaceItem(newItem);
        Assert.IsTrue(didPlaceItem);
        Assert.AreSame(dispenser.placedItem, newItem);
    }




    // =============================================================

    /*
[Test]
public void TryPickUpItem_NoItemsPlaced_SuccessfulyReturnDispensedItem()
{
    var dispenser = new GameObject().AddComponent<Dispenser>();
    Assert.IsNull(dispenser.placedItem); // No items are placed
    var dispensibleItem = new GameObject().AddComponent<FoodIngredient>().gameObject;
    typeof(Dispenser).GetField("itemToDispense", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(dispenser, dispensibleItem);
    var dispensedItem = dispenser.TryPickUpItem();
    Assert.IsNotNull(dispensedItem);
    Assert.IsNull(dispenser.placedItem);
}

[Test]
public void DispenseItem_NoItemsPlaced_SuccessfulyReturnDispensedItem()
{
    var dispenser = new GameObject().AddComponent<Dispenser>();
    Assert.IsNull(dispenser.placedItem); // No items are placed
    var dispensibleItem = new GameObject().AddComponent<FoodIngredient>().gameObject;
    typeof(Dispenser).GetField("itemToDispense", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(dispenser, dispensibleItem);
    var dispensedItem = dispenser.DispenseItem();
    Assert.IsNotNull(dispensedItem);
    Assert.IsInstanceOf(typeof(FoodIngredient), dispensedItem.GetComponent<FoodIngredient>());
}

[Test]
public void DispenseItem_NoItemsAtAnchor_SuccessfulyPlaceAtAnchor()
{
    var dispenser = new GameObject().AddComponent<Dispenser>();
    var dispensibleItem = new GameObject().AddComponent<FoodIngredient>().gameObject;
    var anchorTransform = new GameObject().transform;
    // set anchor
    typeof(Dispenser).GetField("itemPlacementAnchor", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(dispenser, anchorTransform);
    // Set dispended item
    typeof(Dispenser).GetField("itemToDispense", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(dispenser, dispensibleItem);
    var dispensedItem = dispenser.DispenseItem();
    Assert.AreSame(dispensedItem.transform.parent, anchorTransform);
}

[Test]
public void DispenseItem_WithItemAtAnchor_SuccessfulyReturnItemInAnchor()
{
    var dispenser = new GameObject().AddComponent<Dispenser>();
    var dispensibleItemInAnchor = new GameObject().AddComponent<FoodIngredient>().gameObject;
    var anchorTransform = new GameObject().transform;
    // set anchor
    typeof(Dispenser).GetField("itemPlacementAnchor", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(dispenser, anchorTransform);
    // set the item under the anchor's transform
    dispensibleItemInAnchor.transform.parent = (Transform)typeof(Dispenser).GetField("itemPlacementAnchor", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(dispenser);
    var dispensedItem = dispenser.DispenseItem();
    Assert.IsNotNull(dispensedItem);
    Assert.AreSame(dispensedItem, dispensibleItemInAnchor); // Make sure it's the same item in the anchor
}*/
}
