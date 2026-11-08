using System;

namespace Core.Infrastructures.AssetManagement
{
    public sealed class ContentLoadException : Exception
    {
        public ContentLoadException(string message, Exception innerException) :
            base(message, innerException) { }
    }
}
