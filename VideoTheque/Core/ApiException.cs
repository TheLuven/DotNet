using System.Net;

namespace VideoTheque.Core
{
    public class ApiException : Exception
    {
        private int _httpStatusCode = (int)HttpStatusCode.InternalServerError;
        public ApiException() { }
        public ApiException(string message) : base(message) { }

        public int httpStatusCode
        {
            get { return this._httpStatusCode; }
        }
    }

    public class NotFoundException : ApiException
    {
        private int _httpStatusCode = (int)HttpStatusCode.NotFound;
        public NotFoundException() { }
        public NotFoundException(string message) : base(message) { }
    }

    public class InternalErrorException : ApiException
    {
        private int _httpStatusCode = (int)HttpStatusCode.InternalServerError;
        public InternalErrorException() { }
        public InternalErrorException(string message) : base(message) { }
    }

    public class BadRequestException : ApiException
    {
        private int _httpStatusCode = (int)HttpStatusCode.BadRequest;
        public BadRequestException() { }
        public BadRequestException(string message) : base(message) { }
    }
}
