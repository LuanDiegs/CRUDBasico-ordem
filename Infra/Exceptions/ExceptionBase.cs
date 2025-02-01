using System.Net;

namespace Infra.Exceptions
{
    public abstract class ExceptionBase : SystemException
    {
        public ExceptionBase(string message) : base(message)
        {
        }

        public abstract HttpStatusCode GetStatusCode();

        public abstract IList<string> GetErrorMessage();
    }
}
