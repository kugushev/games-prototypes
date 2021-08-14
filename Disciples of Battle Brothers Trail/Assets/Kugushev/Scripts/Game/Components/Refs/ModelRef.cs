namespace Kugushev.Scripts.Game.Components.Refs
{
    public readonly struct ModelRef<T> where T : class
    {
        public ModelRef(T @ref)
        {
            Ref = @ref;
        }

        public T Ref { get; }
    }
}