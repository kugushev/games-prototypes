using Kugushev.Scripts.Common.ValueObjects;

namespace Kugushev.Scripts.Common.Interfaces
{

    public interface IPercentPerk<in T>
    {
        Percentage GetPercentage(T criteria);
    }
}