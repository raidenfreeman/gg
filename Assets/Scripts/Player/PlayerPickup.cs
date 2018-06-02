using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Assets.Scripts;
using InControl;
using UnityEditor;



// Responsible for handling picking up and droping pickable items.
// Should not hold any reference to the item (you can get it from the anchor if needed)
public class PlayerPickup : MonoBehaviour
{

    //public ReactiveProperty<IPickableItem> heldItem = new ReactiveProperty<IPickableItem>(new NoItem());

    [SerializeField]
    private float reachRadius;

    internal float GetReachRadius { get { return reachRadius; } }

    [SerializeField]
    internal float reachAngle = 120f;

    [SerializeField]
    private Transform holdingAnchor;

    [SerializeField]
    private Transform leftHand;

    [SerializeField]
    private Transform rightHand;

    // TODO: Replace with animation on the final model
    private readonly Vector3 leftHandOriginalPosition = new Vector3(-0.6f, 0, 0.3f);
    private readonly Vector3 rightHandOriginalPosition = new Vector3(0.6f, 0, 0.3f);

    private bool isHoldingItem { get { return holdingAnchor.childCount > 0; } }

    private Transform heldItem { get { return holdingAnchor.GetChild(0); } }

    private Vector3 forwardReach
    {
        get
        {
            return transform.forward * reachRadius;
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, forwardReach + transform.position);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, reachRadius);
        if (collidersDebug != null)
        {
            Gizmos.color = Color.blue;
            foreach (var col in collidersDebug)
            {
                Gizmos.DrawLine(transform.position, col.transform.position);
            }
        }
    }
#endif

    IEnumerable<Collider> collidersDebug; //for debug

    public float angleStep = 5;

    void Update()
    {
        if ((InputManager.ActiveDevice?.Action3?.IsPressed ?? false) && !isHoldingItem)
        {
            //Try interacting with every in range surface in order, stopping at the first
            GetSurfacesInRangeGroupedByAngle().Any(x => x.Any(y => y.GetComponent<IInteractibleSurface>()?.TryInteract() ?? false));
        }
        if (InputManager.ActiveDevice?.Action1?.WasPressed ?? false)
        {
            if (isHoldingItem)
            {
                PlaceItem();
            }
            else
            {
                PickUpItem();
            }
        }
    }

    private void PlaceItem()
    {
        var heldItem = this.heldItem?.GetComponent<IPickableItem>();
        if (heldItem == null)
        {
            return;
        }
        var surfaceGroups = GetSurfacesInRangeGroupedByAngle();
        if (!surfaceGroups.Any(x =>
        {
            return x.Any(y => y.TryPlaceItem(heldItem));
        }))
        {
            DropItemOnGround();
        }
        ReturnHandsToOriginalPosition();
    }

    private void DropItemOnGround()
    {
        var itemRB = heldItem.GetComponent<Rigidbody>();
        itemRB.constraints = RigidbodyConstraints.None;
        heldItem.GetComponent<Collider>().enabled = true;
        itemRB.isKinematic = false;
        heldItem.parent = null;
    }


    private void PickUpItem()
    {
        IPickableItem item = null;
        // Pickup an item from the surfaces
        GetPickableCollidersInRange().Any(group => group.Any(collider =>
        {
            var surface = collider.GetComponent<PlacementSurface>();
            if (surface != null)
            {
                item = surface.TryPickUpItem();
                return item != null;
            }
            else
            {
                item = collider.GetComponent<IPickableItem>();
                // We assume, that we cannot hit a collider on top of a surface,
                // because IPickableItem colliders on placement surfaces are disabled
                return item != null;
            }
        }));
        if (item != null)
        {
            var itemTransform = (item as MonoBehaviour).transform;
            itemTransform.parent = holdingAnchor;
            itemTransform.GetComponent<Rigidbody>().isKinematic = true;
            itemTransform.GetComponent<Collider>().enabled = false;
            itemTransform.localPosition = Vector3.zero;
            itemTransform.localRotation = Quaternion.identity;

            leftHand.position = item.LeftHandAnchor.position;
            rightHand.position = item.RightHandAnchor.position;
        }
    }

    private void ReturnHandsToOriginalPosition()
    {
        leftHand.localPosition = leftHandOriginalPosition;
        rightHand.localPosition = rightHandOriginalPosition;
    }

    /// <summary>
    /// Get the surfaces hit, grouped by angle, ordered, with the one closes to the front first.
    /// </summary>
    /// <returns>An array of PlacementSurface</returns>
    private IEnumerable<IOrderedEnumerable<PlacementSurface>> GetSurfacesInRangeGroupedByAngle()
    {
        var _transform = this.transform;
        //filter hit colliders, so that those on the back (from 210deg to 330deg)
        var collidersHit = Physics.OverlapSphere(transform.position, reachRadius, 1 << 8)
            .Where(x =>
            {
                var angle = Vector3.Angle(this.forwardReach, Vector3.ProjectOnPlane(x.transform.position - _transform.position, _transform.up));
                return angle < reachAngle;
            });

#if UNITY_EDITOR

        collidersDebug = collidersHit;

#endif
        return collidersHit
            // group them relative to the angle from the forward vector, in steps of +/-5deg
            .GroupBy(x =>
                (int)(Vector3.Angle(this.forwardReach, Vector3.ProjectOnPlane(x.transform.position - _transform.position, _transform.up)) / angleStep))
            //Order the groups based on how far they are from 0 deg
            .OrderBy(x => x.Key)
            // filter out those who are not placement surfaces
            .Select(x => x.Select(y => y.GetComponent<PlacementSurface>()).Where(y => y != null))
            // inside the groups, order them by proximity to the player
            .Select(x => x.OrderBy(y => (_transform.position - y.transform.position).sqrMagnitude));
    }

    private IEnumerable<IOrderedEnumerable<Collider>> GetPickableCollidersInRange()
    {
        // IMPORTANT: WE ASSUME THAT OBJECTS PLACED ON SURFACES, HAVE THEIR COLLIDERS DISABLED
        var _transform = this.transform;
        //filter hit colliders, so that those on the back (from 210deg to 330deg)
        var collidersHit = Physics.OverlapSphere(transform.position, reachRadius, 1 << 8 | 1 << 9)
            .Where(x =>
            {
                var angle = Vector3.Angle(this.forwardReach, x.transform.position - _transform.position);
                return angle < reachAngle;
            });

#if UNITY_EDITOR

        collidersDebug = collidersHit;

#endif
        return collidersHit
            // group them relative to the angle from the forward vector, in steps of +/-5deg
            .GroupBy(x =>
                (int)(Vector3.Angle(this.forwardReach, x.transform.position - _transform.position) / angleStep))
            //Order the groups based on how far they are from 0 deg
            .OrderBy(x => x.Key)
            // inside the groups, order them by proximity to the player
            .Select(x => x.OrderBy(y => (_transform.position - y.transform.position).sqrMagnitude));
    }
}

// Create a 180 degrees wire arc with a ScaleValueHandle attached to the disc
// that lets you modify the "shieldArea" var in the WireArcExample.js
[CustomEditor(typeof(PlayerPickup))]
public class DrawSolidArc : Editor
{
    void OnSceneGUI()
    {
        Handles.color = new Color(1, 1, 1, 0.3f);
        PlayerPickup player = (PlayerPickup)target;
        Handles.DrawSolidArc(player.transform.position + Vector3.up * 0.3f, player.transform.up, player.transform.forward, player.reachAngle, player.GetReachRadius);
        Handles.DrawSolidArc(player.transform.position + Vector3.up * 0.3f, player.transform.up, player.transform.forward, -player.reachAngle, player.GetReachRadius);

        Handles.color = new Color(250, 0, 0, 0.5f);
        for (float i = player.angleStep; i < player.reachAngle; i += player.angleStep)
        {
            Handles.DrawSolidArc(player.transform.position + Vector3.up * 0.3f, player.transform.up, player.transform.forward, i, player.GetReachRadius);
            Handles.DrawSolidArc(player.transform.position + Vector3.up * 0.3f, player.transform.up, player.transform.forward, -i, player.GetReachRadius);
            Handles.color = new Color(0, 0, 250, 0.1f);
        }
    }
}