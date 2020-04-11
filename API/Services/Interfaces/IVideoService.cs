namespace API.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Entities;

    public interface IVideoService : IService<Video>
    {
        Task<IEnumerable<Video>> ReadAllWithTypeAsync(string typeName);      
        
        Task<IEnumerable<Video>> ReadAllWithGenreAsync(string genreName); 

        Task<IEnumerable<Video>> ReadAllContainTitleAsync(string title);

        Task<Video> ReadOneWithChildrenAsync(string altTitle);

        Task<Video> DeleteAsync(string altTitle);
    }
}
