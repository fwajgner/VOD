namespace VOD.Domain.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using VOD.Domain.Entities;

    public interface IGenreRepository : IRepository
    {
        Task<IEnumerable<Genre>> GetAsync();

        Task<Genre> GetAsync(Guid id);

        Genre Add(Genre genre);

        Genre Update(Genre genre);
    }
}
