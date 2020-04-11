namespace API.Exceptions
{
    public class TypeNotFoundException : VodApiException
    {
        public TypeNotFoundException(string typeName) : base($"Video type {typeName} was not found.")
        {

        }
    }
}
