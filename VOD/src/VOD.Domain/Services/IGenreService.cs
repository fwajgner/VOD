namespace VOD.Domain.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using VOD.Domain.Responses;
    using VOD.Domain.Requests.Genre;

    public interface IGenreService 
    { 
        Task<IEnumerable<GenreResponse>> GetGenreAsync();

        Task<GenreResponse> GetGenreAsync(GetGenreRequest request);

        Task<IEnumerable<VideoResponse>> GetVideosByGenreIdAsync(GetGenreRequest request);

        Task<GenreResponse> AddGenreAsync(AddGenreRequest request);
    }
}
