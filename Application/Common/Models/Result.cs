namespace Application.Common.Models
{
    public class Result
    {
        public Result(bool succeeded, ICollection<string> errors)
        {
            Succeeded = succeeded;
            Errors = errors;
        }

        public bool Succeeded { get; set; }

        public ICollection<string> Errors { get; set; }

        public static Result Success()
        {
            return new Result(true, new List<string>());
        }

        public static Result Failure(ICollection<string> errors)
        {
            return new Result(false, errors);
        }
    }
}