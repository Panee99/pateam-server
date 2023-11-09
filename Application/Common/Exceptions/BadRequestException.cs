using FluentValidation.Results;

namespace Application.Common.Exceptions
{
    public class BadRequestException : Exception
    {
        public IDictionary<string, string[]> Errors { get; } = new Dictionary<string, string[]>();

        public BadRequestException(string message) : base(message)
        {
        }

        public BadRequestException(IEnumerable<ValidationFailure> failures) : base(
            "One or more validation failures have occurred.")
        {
            Errors = failures
                .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
                .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
        }
    }
}