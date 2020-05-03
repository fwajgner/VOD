namespace API.Exceptions
{
    public class KindNotFoundException : VodApiException
    {
        public KindNotFoundException(string kindName) : base($"The video kind {kindName} was not found.")
        {

        }
    }
}
