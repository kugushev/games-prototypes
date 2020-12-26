using Kugushev.Scripts.Game.ValueObjects;

namespace Kugushev.Scripts.Game.Features
{
    public interface IDamageable
    {
        void Suffer(Damage damage);
    }
}