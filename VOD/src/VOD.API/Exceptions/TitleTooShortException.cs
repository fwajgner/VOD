namespace API.Exceptions
{
    public class TitleTooShortException : VodApiException
    {
        public TitleTooShortException(int length) : base($"The title must be at least {length} characters.")
        {

        }
    }
}
