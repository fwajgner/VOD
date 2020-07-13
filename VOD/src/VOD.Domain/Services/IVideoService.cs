namespace VOD.Domain.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using VOD.Domain.Responses;
    using VOD.Domain.Requests.Video;

    public interface IVideoService
    {
        Task<IEnumerable<VideoResponse>> GetVideoAsync();

        Task<VideoResponse> GetVideoAsync(GetVideoRequest request);

        //Task<IEnumerable<VideoResponse>> GetAllContainTitleAsync(string title);

        Task<VideoResponse> AddVideoAsync(AddVideoRequest request);

        Task<VideoResponse> EditVideoAsync(EditVideoRequest request);

        Task<VideoResponse> DeleteVideoAsync(DeleteVideoRequest request);
    }
}
