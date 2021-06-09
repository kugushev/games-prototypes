using JetBrains.Annotations;
using Kugushev.Scripts.Core.Battle.ValueObjects;

namespace Kugushev.Scripts.Core.Common.Exceptions
{
    public class PropertyIsNotInitializedException : GameException
    {
        public PropertyIsNotInitializedException(string propertyName)
            : base($"Property {propertyName} is not initialized")
        {
        }
    }
}