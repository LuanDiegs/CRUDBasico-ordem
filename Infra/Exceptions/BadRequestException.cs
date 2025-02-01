using System.Net;

namespace Infra.Exceptions
{
    public class BadRequestException : ExceptionBase
    {
        public readonly IList<string> _errors;

        public BadRequestException(IList<string> messages) : base(string.Empty)
        {
            _errors = messages;
        }

        public override IList<string> GetErrorMessage()
        {
            return _errors;
        }

        public override HttpStatusCode GetStatusCode()
        {
            return HttpStatusCode.BadRequest;
        }
    }
}
