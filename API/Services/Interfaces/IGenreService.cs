namespace API.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Entities;

    public interface IGenreService : IService<Genre>
    {
        Task<bool> IsGenreExistsAsync(string genreName);

        Task<IEnumerable<Genre>> ReadAllAsync();

        Task<Genre> ReadOneWithChildrenAsync(string genreName);

        Task<Genre> DeleteAsync(string genreName);
    }
}
