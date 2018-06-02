using System;
using UnityEngine;


public interface ICombinator
{
    bool TryAddIngredient(ContentType ingredient);
}

public class Pot : MonoBehaviour, IPickableItem, ICombinator, ICookable
{
    [SerializeField]
    private Contents contents;

    /// <summary>
    /// Set in editor, the point where to place the left hand
    /// </summary>
    /// <remarks>
    /// Don't modify, should be readonly but stupid editor can't handle it
    /// </remarks>
    [SerializeField]
    private Transform leftHandAnchor;

    /// <summary>
    /// Set in editor, the point where to place the left hand
    /// </summary>
    /// <remarks>
    /// Don't modify, should be readonly but stupid editor can't handle it
    /// </remarks>
    public Transform LeftHandAnchor
    {
        get
        {
            return leftHandAnchor;
        }
    }

    /// <summary>
    /// Set in editor, the point where to place the right hand
    /// </summary>
    /// <remarks>
    /// Don't modify, should be readonly but stupid editor can't handle it
    /// </remarks>
    [SerializeField]
    private Transform rightHandAnchor;
    /// <summary>
    /// Set in editor, the point where to place the right hand
    /// </summary>
    /// <remarks>
    /// Don't modify, should be readonly but stupid editor can't handle it
    /// </remarks>
    public Transform RightHandAnchor
    {
        get
        {
            return rightHandAnchor;
        }
    }
    /// <summary>
    /// Set in editor, used to position on surfaces
    /// </summary>
    /// <remarks>
    /// Don't modify, should be readonly but stupid editor can't handle it
    /// </remarks>
    [SerializeField]
    private Transform placementAnchor;
    /// <summary>
    /// Set in editor, used to position on surfaces
    /// </summary>
    /// <remarks>
    /// Don't modify, should be readonly but stupid editor can't handle it
    /// </remarks>
    public Transform PlacementAnchor
    {
        get
        {
            return placementAnchor;
        }
    }

    public void Drop()
    {
        throw new NotImplementedException();
    }

    public void PickUp()
    {
        throw new NotImplementedException();
    }

    public void Place(PlacementSurface surface)
    {
        throw new NotImplementedException();
    }


    public bool TryAddIngredient(ContentType ingredient)
    {
        return contents.TryAddIngredient(ingredient);
    }

    public int Cook(float timeToAdd)
    {
        return contents.Cook(timeToAdd);
    }
}