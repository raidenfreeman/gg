using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Dish : MonoBehaviour, IPickableItem, IWashable
{
    /// <summary>
    /// Occurs when the food is prepared
    /// </summary>
    public event EventHandler WashedDishEvent;

    /// <summary>
    /// The time needed to complete washing the item
    /// </summary>
    public readonly float timeToWash;

    /// <summary>
    /// How much % is completed
    /// </summary>
    public int completionPercentage
    {
        get
        {
            return (int)(timeSpentWashing / timeToWash * 100f);
        }
    }



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

    /// <summary>
    /// The amount of time invested in washing this item
    /// </summary>
    private float timeSpentWashing = 0;

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


    /// <summary>
    /// Called when the item is washed
    /// </summary>
    void WashingComplete()
    {
        WashedDishEvent?.Invoke(this, new EventArgs());
    }

    /// <summary>
    /// Increments washing by time
    /// </summary>
    /// <param name="timeToAdd">The time in milliseconds to add</param>
    /// <returns>The percentage of preparation</returns>
    public int Wash(float timeToAdd)
    {
        timeSpentWashing += timeToAdd;
        if (timeSpentWashing >= timeToWash)
        {
            timeSpentWashing = timeToWash;
            WashingComplete();
        }
        return completionPercentage;
    }
}
