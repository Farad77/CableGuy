namespace Quantum
{
    public abstract partial class ItemBase
    {
        public string Name;

        public abstract void Use<T>(Frame f, EntityRef entityRef, T item) where T : ItemBase;
        public abstract void OnPickUp<T>(Frame f, EntityRef entityRef, T item) where T : ItemBase;
    }
}