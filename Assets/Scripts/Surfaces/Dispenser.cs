using System.Collections.Generic;
using System.Linq;
using UnityEngine;


/// <summary>
/// A pool-stack wrapper
/// </summary>
public class Dispenser : PlacementSurface
{
    /// <summary>
    /// This item will be set as the item to dispense OnAwake(). MUST HAVE IPickableItem component.
    /// </summary>
    /// <remarks>This exists, because you can't set through the inspector,
    /// fields/properties of interface types (like itemToDispense).</remarks>
    public GameObject pickableItemToDispenseAddedOnAwake;

    [SerializeField]
    private IPickableItem itemToDispense;

    [SerializeField]
    private int initialPoolCount = 15;

    private Stack<IPickableItem> itemPool = new Stack<IPickableItem>();

    private IEnumerable<IPickableItem> inactiveGameObjectPool
    {
        get
        {
            return itemPool.Where(x => (x as MonoBehaviour).gameObject.activeInHierarchy == false).AsEnumerable();
        }
    }

    public override IPickableItem TryPickUpItem()
    {
        if (base.placedItem == null)
        {
            return DispenseItem();
        }
        else
        {
            return base.TryPickUpItem();
        }
    }

    /// <summary>
    /// Get an item from the pool, and place it at anchor
    /// </summary>
    /// <returns>The reference to the placed object</returns>
    private IPickableItem DispenseItem()
    {
        // if the pool is empty
        if (inactiveGameObjectPool.Count() <= 0)
        {
            // fill it
            PopulatePool();
        }
        // Get the first deactivated item
        var item = inactiveGameObjectPool.First();
        (item as MonoBehaviour).gameObject.SetActive(true);
        return item;
    }

    private void Awake()
    {
        if (pickableItemToDispenseAddedOnAwake != null)
        {
            var item = pickableItemToDispenseAddedOnAwake.GetComponent<IPickableItem>();
            if (item == null)
            {
                Debug.LogError(nameof(pickableItemPlacedOnAwake) + " does not contain a component that implements " + nameof(IPickableItem));
                return;
            }
            else
            {
                itemToDispense = item;
            }
            PopulatePool();
        }
        else
        {
            Debug.LogError(nameof(pickableItemPlacedOnAwake) + " has no value in " + this.gameObject.ToString() + " in component " + nameof(Dispenser));
        }
    }

    private void PopulatePool()
    {
        var gameObject = (itemToDispense as MonoBehaviour).gameObject;
        for (int i = 0; i < initialPoolCount; i++)
        {
            // Create a new item, as a child of the anchor
            var item = Instantiate(gameObject);
            item.SetActive(false);
            itemPool.Push(item.GetComponent<IPickableItem>());
        }
    }
}
