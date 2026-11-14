namespace Centuriin.CardGame.Core.Engine.ECS.Components;

public interface IComponentsCollection : IReadOnlyComponentsCollection
{
    /// <summary>
    /// Adds component.
    /// </summary>
    /// <param name="components">
    /// Component.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// When <paramref name="components"/> is <see langword="null"/>.
    /// </exception>
    public void Add(params IEnumerable<ComponentBase> components);

    /// <summary>
    /// Replaces component.
    /// </summary>
    /// <typeparam name="T">
    /// Component type.
    /// </typeparam>
    /// <param name="replacement">
    /// Replaceable component.
    /// </param>
    public void Replace<T>(T replacement) where T : ComponentBase;

    /// <summary>
    /// Removes component by type.
    /// </summary>
    /// <typeparam name="T">
    /// Concrete type.
    /// </typeparam>
    public void Remove<T>() where T : ComponentBase;
}
