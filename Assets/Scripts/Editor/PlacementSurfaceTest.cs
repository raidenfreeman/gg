using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Reflection;

public class PlacementSurfaceTest
{
    [Test]
    public void TryPickupItem_NoItemPlaced_ReturnNull()
    {
        var surface = new GameObject().AddComponent<PlacementSurface>();
        var pickedItem = surface.TryPickUpItem();
        Assert.IsNull(pickedItem);
    }

    [Test]
    public void TryPickupItem_ItemPlaced_ReturnPlacedItem()
    {
        var surface = new GameObject().AddComponent<PlacementSurface>();
        var item = new GameObject().AddComponent<FoodIngredient>();
        typeof(PlacementSurface).GetProperty("placedItem", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).SetValue(surface, item);
        var pickedItem = surface.TryPickUpItem();
        Assert.AreSame(item, pickedItem);
    }

    [Test]
    public void TryPlaceItem_ItemPlaced_ReturnFalse()
    {
        var surface = new GameObject().AddComponent<PlacementSurface>();
        var existingItem = new GameObject().AddComponent<FoodIngredient>();
        var newItem = new GameObject().AddComponent<FoodIngredient>();
        typeof(PlacementSurface).GetProperty("placedItem", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).SetValue(surface, existingItem);
        var didPlaceItem = surface.TryPlaceItem(newItem);
        Assert.IsFalse(didPlaceItem);
        Assert.AreNotSame(newItem, surface.placedItem);
    }

    [Test]
    public void TryPlaceItem_NoItemPlaced_ReturnTruePlacedItemHasChanged()
    {
        var surface = new GameObject().AddComponent<PlacementSurface>();
        Assert.IsNull(surface.placedItem);
        var newItem = new GameObject().AddComponent<FoodIngredient>();
        typeof(FoodIngredient)
            .GetField("placementAnchor", BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(newItem, new GameObject().transform);
        var didPlaceItem = surface.TryPlaceItem(newItem);
        Assert.IsTrue(didPlaceItem);
        Assert.AreSame(surface.placedItem, newItem);
    }
}
