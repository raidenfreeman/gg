using System.Linq;
using UnityEngine;

public class PlacementSurface : MonoBehaviour
{
    /// <summary>
    /// This item will be placed on the surface OnAwake(). MUST HAVE IPickableItem component.
    /// </summary>
    /// <remarks>This exists, because you can't set through the inspector,
    /// fields/properties of interface types (like placedItem).</remarks>
    public GameObject pickableItemPlacedOnAwake;

    /// <summary>
    /// The transform point to put the placed item
    /// </summary>
    [SerializeField]
    protected Transform placementAnchor;

    /// <summary>
    /// The item placed on the surface
    /// </summary>
    [SerializeField]
    public IPickableItem placedItem { get; protected set; }

    /// <summary>
    /// Try to place an item on the surface
    /// </summary>
    /// <param name="item">The MonoBehaviour of the item to place. Must have Rigidbody attached.</param>
    /// <returns>True if successful, false otherwise</returns>
    public virtual bool TryPlaceItem(IPickableItem item)
    {

        if (placedItem == null)
        {
            var itemTransform = (item as MonoBehaviour).transform;
            PlaceItemOnSurface(item, itemTransform);
            return true;
        }
        else
        {
            var didCombineItems = TryCombineItems(item, placedItem);
            if (didCombineItems)
            {
                var itemMonoBehaviour = item as MonoBehaviour;
                var itemTransform = itemMonoBehaviour.transform;
                // TODO: This is bad. This shouldn't be responsible for deactivating the ingredients
                if ((placedItem as MonoBehaviour)?.GetComponent<ICombinator>() != null)
                {
                    //if the placed item was the combinator, just deactivate the held item
                    itemMonoBehaviour?.GetComponent<FoodIngredient>()?.Deactivate();
                }
                else
                {
                    //if the held item was the combinator, place it on the surface, and deactivate the placed item
                    if (itemMonoBehaviour?.GetComponent<ICombinator>() != null)
                    {
                        (placedItem as MonoBehaviour)?.GetComponent<FoodIngredient>()?.Deactivate();
                        PlaceItemOnSurface(item, itemTransform);
                    }
                }
            }
            return didCombineItems;
        }

    }

    private bool TryCombineItems(IPickableItem heldItem, IPickableItem placedItem)
    {
        var item1MonoBehaviour = (heldItem as MonoBehaviour);
        if (item1MonoBehaviour == null)
        {
            return false;
        }
        var item2MonoBehaviour = (placedItem as MonoBehaviour);
        if (item2MonoBehaviour == null)
        {
            return false;
        }
        // Set the held item as the base
        var combinator = item1MonoBehaviour.GetComponent<ICombinator>();
        var itemToAdd = item2MonoBehaviour.GetComponent<FoodIngredient>();
        if (combinator == null || itemToAdd == null)
        {
            // Set the placed item as the base
            combinator = item2MonoBehaviour.GetComponent<ICombinator>();
            itemToAdd = item1MonoBehaviour.GetComponent<FoodIngredient>();
            if (combinator == null || itemToAdd == null)
            {
                //if no item is a combo base, they can't be combined
                return false;
            }
        }
        if (itemToAdd.isPrepared == false)
        {
            // Only combine with prepared ingredients
            return false;
        }
        return combinator.TryAddIngredient(itemToAdd.ingredientType);
    }

    /// <summary>
    /// Try to get the placed item
    /// </summary>
    /// <returns>The item if sucessful, null otherwise</returns>
    public virtual IPickableItem TryPickUpItem()
    {

        if (placedItem == null)
        {
            return null;
        }
        else
        {
            var itemToReturn = placedItem;
            placedItem = null;
            return itemToReturn;
        }
    }

    private void Awake()
    {
        if (pickableItemPlacedOnAwake != null)
        {
            var item = pickableItemPlacedOnAwake.GetComponent<IPickableItem>();
            if (item == null)
            {
                Debug.LogError(nameof(pickableItemPlacedOnAwake) + " does not contain a component that implements " + nameof(IPickableItem));
                return;
            }
            else
            {
                PlaceItemOnSurface(item, pickableItemPlacedOnAwake.transform);
            }
        }
    }

    private void PlaceItemOnSurface(IPickableItem item, Transform itemTransform)
    {
        itemTransform.parent = placementAnchor;
        itemTransform.localPosition = Vector3.zero;
        itemTransform.localPosition = -item.PlacementAnchor.localPosition;
        itemTransform.localRotation = Quaternion.identity;
        var collider = itemTransform.GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = false;
        }
        var itemRigidbody = itemTransform.GetComponent<Rigidbody>();
        if (itemRigidbody != null)
        {
            itemRigidbody.constraints = RigidbodyConstraints.FreezeAll;
            itemTransform.GetComponent<Collider>().enabled = false;
        }
        else
        {
            // In case that a test fails, and leads you here:
            // TODO: Implement empty object pattern for default placed item, instead of relying on null
            Debug.LogWarning("No rigidbody attached to IPickableItem: " + placedItem.ToString());
        }
        placedItem = item;
    }
}