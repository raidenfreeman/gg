using UnityEngine;
/// <summary>
/// The data container of every item
/// </summary>
public interface IPickableItem
{
    /// <summary>
    /// Pick up the item from the ground or a surface
    /// </summary>
    void PickUp();

    /// <summary>
    /// Drop the item on the ground
    /// </summary>
    void Drop();

    /// <summary>
    /// Place the item on a surface
    /// </summary>
    /// <param name="surface">The component reference of the target surface</param>
    void Place(PlacementSurface surface);

    Transform LeftHandAnchor { get; }

    Transform RightHandAnchor { get; }

    Transform PlacementAnchor { get; }
}