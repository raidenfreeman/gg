
// This exists, along with IWashable, because some surfaces
// can interact with IPreparable, but not IWashable,
// and vice versa. IInteractible is not enough.
public interface IPreparable : IInteractibleItem
{
    int Prepare(float timeToAdd);
}