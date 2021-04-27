using System;

namespace Kugushev.Scripts.Common.Utils
{
    public class SpaceFightException : ApplicationException
    {
        public SpaceFightException(string message) : base(message)
        {
        }
    }
}