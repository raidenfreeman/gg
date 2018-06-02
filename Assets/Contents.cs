using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Recipe = System.Collections.Generic.Dictionary<ContentType, int>;

public class Contents : MonoBehaviour
{
    /// <summary>
    /// The time needed to complete preparing the item
    /// </summary>
    public float timeToCook
    {
        get
        {
            return timeToCookPerIngredient * TotalIngredients;
        }
    }

    public int TotalIngredients
    {
        get
        {
            return ContentsAndAmounts.Aggregate(0, (sum, ingredient) => sum += ingredient.Value);
        }
    }

    public float timeToCookPerIngredient = 2;

    public float timeToBurn = 3; // TODO: make this readonly, it's not cause of the inspector atm

    [SerializeField]
    ProgressBar ProgressBar;

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
            return (int)(TimeCooked / timeToCook * 100f);
        }
    }

    private float timeCooked;

    private float TimeCooked
    {
        get
        {
            return timeCooked;
        }
        set
        {
            timeCooked = value;
            if (timeCooked > timeToCook)
            {
                Overcook();
            }
            ProgressBar?.UpdatePercentage(completionPercentage <= 100 ? completionPercentage : 100);
        }
    }

    private void Overcook()
    {
        if (isBurning)
        {
            Debug.Log("Burning!");
            return;
        }
        Debug.Log("Overcooking!");
    }

    public int Cook(float timeToAdd)
    {
        TimeCooked += timeToAdd;

        return completionPercentage;
    }

    public bool TryAddIngredient(ContentType ingredient)
    {
        bool canAddIngredient = recipes
            .Where(x => x.ContainsKey(ingredient))// filter the recipes that contain the ingredient
            .Where(x => x.Keys.Intersect(contents.Keys).Count() == contents.Count) // 
            .Where(x => contents.All(y => x[y.Key] >= y.Value))
            .Any(x =>
            {
                if (contents.ContainsKey(ingredient))
                {
                    return x[ingredient] > contents[ingredient];
                }
                else
                {
                    return true;
                }
            });// is there any recipe, that has this ingredient more times than it already exists in contents
        if (canAddIngredient)
        {
            if (contents.ContainsKey(ingredient))
            {
                contents[ingredient] += 1;
            }
            else
            {
                contents[ingredient] = 1;
            }
            UpdateContentsDisplay();
        }
        foreach (var item in contents)
        {
            Debug.Log($"{item.Value.ToString()} x {item.Key.ToString()}");
        }
        Debug.Log("================");
        return canAddIngredient;
    }

    Dictionary<ContentType, int> contents = new Dictionary<ContentType, int>();

    public Dictionary<ContentType, int> ContentsAndAmounts
    {
        get
        {
            return contents;
        }

        private set
        {
            contents = value;
            UpdateContentsDisplay();
        }
    }

    List<Recipe> recipes = new List<Recipe>
    {
        new Recipe
        {
            { ContentType.tomato, 3 }
        },
        new Recipe
        {
            { ContentType.onion, 3 }
        }
    };

    [SerializeField]
    GameObject Canvas;
    [SerializeField]
    Transform ContentDisplayPanel;
    void UpdateContentsDisplay()
    {
        if (ContentDisplayPanel != null)
        {
            foreach (Transform child in ContentDisplayPanel)
            {
                Destroy(child.gameObject);
            }
            foreach (var item in ContentsAndAmounts)
            {
                var image = IngredientToImage(item.Key);
                for (int i = 0; i < item.Value; i++)
                {
                    Instantiate(image, ContentDisplayPanel);
                }
            }
            Canvas?.SetActive(ContentsAndAmounts.Count > 0);
        }
    }

    [SerializeField]
    GameObject tomatoImage;

    private bool isDone
    {
        get
        {
            return timeCooked >= timeToCook;
        }
    }

    private bool isBurning
    {
        get
        {
            return timeCooked >= timeToCook + timeToBurn;
        }
    }

    GameObject IngredientToImage(ContentType ingredient)
    {
        return tomatoImage;
    }
}
