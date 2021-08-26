namespace Kugushev.Scripts.Presentation.PoC.Fight
{
    public readonly struct Combo
    {
        public int Damage { get; }
        public DamageEffect Effect { get; }

        public Combo(int damage, DamageEffect effect = DamageEffect.None)
        {
            Damage = damage;
            Effect = effect;
        }
    }
}