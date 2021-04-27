using System;
using System.Runtime.CompilerServices;

namespace Kugushev.Scripts.Common.Utils
{
    public class SpaceFightException : ApplicationException
    {
        public SpaceFightException(string message) : base(message)
        {
        }
    }
}