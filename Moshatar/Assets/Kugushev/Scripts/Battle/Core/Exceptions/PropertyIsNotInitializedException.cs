using System.Runtime.CompilerServices;

namespace Kugushev.Scripts.Battle.Core.Exceptions
{
    public class PropertyIsNotInitializedException : GameException
    {
        public PropertyIsNotInitializedException([CallerMemberName] string propertyName = "<property-name>")
            : base($"Property {propertyName} is not initialized")
        {
        }
    }
}