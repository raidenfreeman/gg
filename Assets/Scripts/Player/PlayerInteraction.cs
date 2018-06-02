using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Assets.Scripts;
using InControl;

public class PlayerInteraction : MonoBehaviour
{

    //public ReactiveProperty<IPickableItem> heldItem = new ReactiveProperty<IPickableItem>(new NoItem());

    [SerializeField]
    private float reachRadius;

    private Vector3 forwardReachVector
    {
        get
        {
            return transform.forward * reachRadius;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + forwardReachVector);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, reachRadius);
        if (collidersDebug != null)
        {
            Gizmos.color = Color.blue;
            foreach (var col in collidersDebug)
            {
                if (col != selectedColliderDebug)
                {
                    Gizmos.DrawLine(transform.position, col.transform.position);
                }
                else
                {
                    Gizmos.color = Color.cyan;
                    Gizmos.DrawLine(transform.position, col.transform.position);
                    Gizmos.color = Color.blue;
                }
            }
        }
    }

    Collider[] collidersDebug; //for debug
    Collider selectedColliderDebug;
    void Update()
    {
        if (InputManager.ActiveDevice?.Action3?.IsPressed ?? false)
        {
            GetHitCollider();
        }

        if (InputManager.ActiveDevice?.Action1?.IsPressed ?? false)
        {
            GetHitCollider();
        }
    }

    private Collider GetHitCollider()
    {
        var collidersHit = Physics.OverlapSphere(transform.position, reachRadius, 1 << 8);
        collidersDebug = collidersHit;
        var colliderSelected = collidersHit.SelectItemBy(
            (a, b) =>
                Vector3.Angle(this.forwardReachVector, a.transform.position - this.transform.position) <
                Vector3.Angle(this.forwardReachVector, b.transform.position - this.transform.position)
        );
        selectedColliderDebug = colliderSelected;
        return colliderSelected;
    }

}

public static class ExtMethods
{
    public static T SelectItemBy<T>(this IEnumerable<T> enumerable, Func<T, T, bool> predicate)
    {
        var count = enumerable.Count();
        if (count == 1)
        {
            return enumerable.First();
        }
        if (count == 0)
        {
            return default(T);
        }
        return enumerable.Aggregate((max, item) => predicate(item, max) ? item : max);
    }
}
