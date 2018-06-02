using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class FoodIngredient : MonoBehaviour, IPickableItem, IPreparable
{
    /// <summary>
    /// The time needed to complete preparing the item
    /// </summary>
    public readonly float timeToPrepare = 1;

    public bool isPrepared
    {
        get
        {
            return completionPercentage >= 100;
        }
    }

    /// <summary>
    /// How much % is completed
    /// </summary>
    public int completionPercentage
    {
        get
        {
            return (int)(TimeSpentPreparing / timeToPrepare * 100f);
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

    [SerializeField]
    public readonly ContentType ingredientType;

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
    /// Set in editor, the point where to place the left hand
    /// </summary>
    /// <remarks>
    /// Don't modify, should be readonly but stupid editor can't handle it
    /// </remarks>
    [SerializeField]
    private Transform leftHandAnchor;
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

    private float timeSpentPreparing;

    /// <summary>
    /// The amount of time invested in preparing this item
    /// </summary>
    private float TimeSpentPreparing
    {
        get
        {
            return timeSpentPreparing;
        }
        set
        {
            if (value > timeToPrepare)
            {
                value = timeToPrepare;
            }
            timeSpentPreparing = value;
            ProgressBar?.UpdatePercentage(completionPercentage);
            //if (completionPercentage > 0 && completionPercentage < 100)
            //{
            //    ProgressBar?.transform.parent.gameObject.SetActive(true);
            //}
            //else
            //{
            //    ProgressBar?.transform.parent.gameObject.SetActive(false);
            //}
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
        if (surface.GetComponent<SinkSurface>())
        {
            throw new ArgumentException("Can't place food in Sink", nameof(surface));
        }
        throw new NotImplementedException();
    }

    public void Deactivate()
    {
        this.TimeSpentPreparing = 0;
        this.transform.parent = null;
        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// Increments preparation by time
    /// </summary>
    /// <param name="timeToAdd">The time in milliseconds to add</param>
    /// <returns>The percentage of preparation</returns>
    public int Prepare(float timeToAdd)
    {
        TimeSpentPreparing += timeToAdd;
        return completionPercentage;
    }

    [SerializeField]
    ProgressBar ProgressBar;
    //public SimpleHealthBar sb;
}
