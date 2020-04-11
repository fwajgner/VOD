namespace API.Exceptions
{
    using Entities;

    public class VideoNotFoundException : VodApiException
    {
        public VideoNotFoundException(string videoAltTitle) : base($"Video {videoAltTitle} was not found.")
        {

        }

        public VideoNotFoundException(string videoType, string videoAltTitle) 
            : base($"Video type: {videoType} with title: {videoAltTitle} was not found.")
        {

        }
    }
}
