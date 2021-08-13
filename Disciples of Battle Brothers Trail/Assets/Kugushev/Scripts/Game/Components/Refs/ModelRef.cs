namespace Kugushev.Scripts.Game.Components.Refs
{
    public readonly struct ModelRef<T> where T : class
    {
        public ModelRef(T model)
        {
            Model = model;
        }

        public T Model { get; }
    }
}