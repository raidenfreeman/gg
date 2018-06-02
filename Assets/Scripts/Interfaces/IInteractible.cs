public interface IInteractibleSurface
{
    /// <summary>
    /// Interact with the item
    /// </summary>
    /// <remarks>For example cut it/prepare it.</remarks>
    /// <returns>True if you can interact with this item.</returns>
    bool TryInteract();
}