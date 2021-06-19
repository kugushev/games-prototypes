namespace Kugushev.Scripts.Common.Core.Exceptions
{
    public class PropertyIsNotInitializedException : GameException
    {
        public PropertyIsNotInitializedException(string propertyName)
            : base($"Property {propertyName} is not initialized")
        {
        }
    }
}