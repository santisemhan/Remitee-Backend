namespace Remitee_Backend.Middleware.GlobalException
{
    public sealed class Envelope : Envelope<string>
    {
        private Envelope(string errorMessage)
            : base(null, errorMessage)
        {
        }

        public static Envelope Error(string errorMessage)
        {
            return new Envelope(errorMessage);
        }
    }

    public class Envelope<T>
    {
        protected internal Envelope(T? result, string errorMessage)
        {
            Result = result;
            ErrorMessage = errorMessage;
            TimeGenerated = DateTime.UtcNow;
        }

        public T? Result { get; }
        public string ErrorMessage { get; }
        public DateTime TimeGenerated { get; }
    }
}
