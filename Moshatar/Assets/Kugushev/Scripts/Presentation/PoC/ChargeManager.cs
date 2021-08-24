namespace Kugushev.Scripts.Presentation.PoC
{
    public class ChargeManager
    {
        private ZombieView _activeTarget;

        public ZombieView ActiveTarget
        {
            get => _activeTarget;
            set
            {
                if (_activeTarget != value && !(_activeTarget is null)) 
                    _activeTarget.DeselectForCharge();
                _activeTarget = value;
            }
        }
    }
}