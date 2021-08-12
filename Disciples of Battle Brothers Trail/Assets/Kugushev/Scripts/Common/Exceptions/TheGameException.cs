using System;

namespace Kugushev.Scripts.Common.Exceptions
{
    public class TheGameException : Exception
    {
        public TheGameException(string message) : base(message)
        {
        }
    }
}