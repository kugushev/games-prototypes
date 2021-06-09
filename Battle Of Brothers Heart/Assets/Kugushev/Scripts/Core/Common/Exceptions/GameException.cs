using System;

namespace Kugushev.Scripts.Core.Common.Exceptions
{
    public class GameException: Exception
    {
        public GameException(string message) : base(message)
        {
        }

        public GameException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}