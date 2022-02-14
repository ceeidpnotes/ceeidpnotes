using System.Runtime.Serialization;

namespace fy21_simplemtapp
{
    [Serializable]
    internal class WebApiMsalUiRequiredException : Exception
    {
        public WebApiMsalUiRequiredException()
        {
        }

        public WebApiMsalUiRequiredException(string? message) : base(message)
        {
        }

        public WebApiMsalUiRequiredException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected WebApiMsalUiRequiredException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}