namespace Kugushev.Scripts.Common.Interfaces
{
    public interface IMultiplierPerk<in T>
    {
        float? GetMultiplier(T criteria);
    }
}