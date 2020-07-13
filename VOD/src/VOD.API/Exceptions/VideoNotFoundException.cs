namespace API.Exceptions
{
    public class VideoNotFoundException : VodApiException
    {
        public VideoNotFoundException(string videoAltTitle) : base($"Video {videoAltTitle} was not found.")
        {

        }

        public VideoNotFoundException(string videoKind, string videoAltTitle) 
            : base($"The video ({videoKind}) with title: {videoAltTitle} was not found.")
        {

        }
    }
}
