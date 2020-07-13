namespace VOD.Domain.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using VOD.Domain.Entities;

    public interface IVideoRepository : IRepository
    {
        Task<IEnumerable<Video>> GetAsync();

        Task<Video> GetAsync(Guid id);

        Task<IEnumerable<Video>> GetVideosByGenreIdAsync(Guid id);

        Task<IEnumerable<Video>> GetVideosByKindIdAsync(Guid id);

        Task<IEnumerable<Video>> GetVideosContainTitleAsync(string titleToSearch);

        Video Add(Video video);

        Video Update(Video video);
    }
}
