using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PreparationSurface : PlacementSurface, IInteractibleSurface
{
    public bool TryInteract()
    {
        var preparableItem = placedItem as IPreparable;
        // If the surface has an item, and we can interact with it
        if (preparableItem !=null)
        {
            preparableItem.Prepare(Time.deltaTime);
            return true;
        }
        return false;
    }
}
