namespace VOD.Infrastructure.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using VOD.Domain.Entities;
    using VOD.Domain.Repositories;

    public class GenreRepository : IGenreRepository
    {
        public GenreRepository(VODContext context)
        {
            this._context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IUnitOfWork UnitOfWork => _context;

        private readonly VODContext _context;

        public Genre Add(Genre genre)
        {
            genre.Id = new Guid();
            genre.CreationDate = DateTimeOffset.Now;

            return _context.Genres
                .Add(genre).Entity;
        }

        public async Task<IEnumerable<Genre>> GetAsync()
        {
            return await _context
                .Genres
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Genre> GetAsync(Guid id)
        {
            Genre genre = await _context.Genres
                .FindAsync(id);

            if (genre == null)
            {
                return null;
            }

            _context.Entry(genre).State = EntityState.Detached;

            return genre;
        }

        public Genre Update(Genre genre)
        {
            genre.ModificationDate = DateTimeOffset.Now;

            _context.Entry(genre).State = EntityState.Modified;

            return genre;
        }
    }
}
