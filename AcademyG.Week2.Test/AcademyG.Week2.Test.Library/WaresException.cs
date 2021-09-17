using System;
using System.Runtime.Serialization;

namespace AcademyG.Week2.Test.Library
{
    public class WaresException : Exception
    {

        public Good Item;

        public WaresException() { }

        public WaresException(Good item) : this()
        {
            Item = item;
        }

        public WaresException(string message) : base(message) { }

        public WaresException(string message, Good item) : this(message)
        {
            Item = item;
        }

        public WaresException(string message, Exception innerException) : base(message, innerException) { }
    }
}