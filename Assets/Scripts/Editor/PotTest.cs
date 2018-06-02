using UnityEngine;
using NUnit.Framework;

public class PotTest
{
    [Test]
    public void TryAddTomato_NoContents_ReturnTrue()
    {
        var contents = new GameObject().AddComponent<Contents>();
        Assert.AreEqual(contents.ContentsAndAmounts.Count, 0);
        Assert.IsTrue(contents.TryAddIngredient(ContentType.tomato));
        Assert.AreEqual(contents.ContentsAndAmounts.Count, 1);
        Assert.IsTrue(contents.ContentsAndAmounts.ContainsKey(ContentType.tomato));
        var numberOfTomatoesInPot = 0;
        contents.ContentsAndAmounts.TryGetValue(ContentType.tomato, out numberOfTomatoesInPot);
        Assert.AreEqual(numberOfTomatoesInPot, 1);
    }

    [Test]
    public void TryAddTomato_TwoTomatoesContained_ReturnTrue()
    {
        var contents = new GameObject().AddComponent<Contents>();
        contents.TryAddIngredient(ContentType.tomato);
        contents.TryAddIngredient(ContentType.tomato);
        Assert.AreEqual(contents.ContentsAndAmounts.Count, 1); // there should be 1 kind of ingredient
        Assert.IsTrue(contents.TryAddIngredient(ContentType.tomato)); // tomato should be able to be added
        Assert.AreEqual(contents.ContentsAndAmounts.Count, 1); // there should still be be 1 ingredient
        Assert.IsTrue(contents.ContentsAndAmounts.ContainsKey(ContentType.tomato));
        var numberOfTomatoesInPot = 0;
        contents.ContentsAndAmounts.TryGetValue(ContentType.tomato, out numberOfTomatoesInPot);
        Assert.AreEqual(numberOfTomatoesInPot, 3); // there should be 3 tomatoes contained
    }

    [Test]
    public void TryAddOnion_TwoTomatoesContained_ReturnFalseBecauseThereIsNoSuchRecipe()
    {
        var contents = new GameObject().AddComponent<Contents>();
        contents.TryAddIngredient(ContentType.tomato);
        contents.TryAddIngredient(ContentType.tomato);
        Assert.IsFalse(contents.ContentsAndAmounts.ContainsKey(ContentType.onion));
        Assert.AreEqual(contents.ContentsAndAmounts.Count, 1); // there should be 1 kind of ingredient
        Assert.IsFalse(contents.TryAddIngredient(ContentType.onion)); // tomato should be able to be added
        Assert.AreEqual(contents.ContentsAndAmounts.Count, 1); // there should still be be 1 ingredient
        Assert.IsFalse(contents.ContentsAndAmounts.ContainsKey(ContentType.onion));
    }
}
