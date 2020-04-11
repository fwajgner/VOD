namespace API.Services
{
    using API.Models;
    using API.Services.Interfaces;
    using Context;
    using Entities;
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using API.Exceptions;

    public class GenreService : IGenreService
    {
        public GenreService(ApplicationDbContext context)
        {
            this.Db = context ?? throw new ArgumentNullException(nameof(context));
        }

        private ApplicationDbContext Db { get; set; }

        public async Task<Genre> CreateAsync(Genre genre)
        {
            await Db.Genres.AddAsync(genre);
            int affected = await Db.SaveChangesAsync();

            if (affected >= 1)
            {
                return genre;
            }
            else
            {
                return null;
            }
        }

        /// <exception cref="GenreNotFoundException"></exception>
        public async Task<Genre> DeleteAsync(string genreName)
        {
            Genre genre;
            try
            {
                genre = await this.ReadOneWithChildrenAsync(genreName);
            }
            catch (GenreNotFoundException)
            {
                throw;
            }
            
            return await Task.Run(() => 
            {
                Db.VideosGenres.RemoveRange(genre.VideoLinks);
                Db.Genres.Remove(genre);
                int expectedAffected = genre.VideoLinks.Count() + 1;
                int affected = Db.SaveChanges();

                if (affected == expectedAffected)
                {
                    return genre;
                }
                else
                {
                    return null;
                }
            });          
        }

        public async Task<bool> IsGenreExistsAsync(string genreName)
        {
            try
            {
                Genre genre = await this.EnforceGenreExistenceAsync(genreName);
                return genre != null;
            }
            catch (GenreNotFoundException)
            {
                return false;
            }
        }

        public async Task<IEnumerable<Genre>> ReadAllAsync()
        {
            return await Db.Genres.ToListAsync();
        }

        /// <exception cref="GenreNotFoundException"></exception>
        public Task<Genre> ReadOneAsync(string genreName)
        {
            try
            {
                return this.EnforceGenreExistenceAsync(genreName);
            }
            catch (GenreNotFoundException)
            {
                throw;
            }
        }

        /// <exception cref="GenreNotFoundException"></exception>
        public Task<Genre> ReadOneWithChildrenAsync(string genreName)
        {
            try
            {
                return this.EnforceGenreExistenceAsync(genreName, true);
            }
            catch (GenreNotFoundException)
            {
                throw;
            }
        }

        /// <exception cref="GenreNotFoundException"></exception>
        public async Task<Genre> UpdateAsync(string genreName, Genre newGenre)
        {
            Genre genre;
            try
            {
                genre = await this.ReadOneAsync(genreName);
            }
            catch (GenreNotFoundException)
            {
                throw;
            }

            return await Task.Run(() => 
            { 
                genre.Name = newGenre.Name;
                genre.ModificationDate = DateTime.Now;
                Db.Genres.Update(genre);

                int affected = Db.SaveChanges();

                if (affected >= 1)
                {
                    return genre;
                }
                else
                {
                    return null;
                }
            });
        }

        /// <exception cref="GenreNotFoundException"></exception>
        private async Task<Genre> EnforceGenreExistenceAsync(string genreName, bool loadChildren = false)
        {
            Genre genre;
            if (loadChildren)
            {
                genre = await Db.Genres
                    .Include(g => g.VideoLinks)
                        .ThenInclude(vl => vl.Video)
                    .FirstOrDefaultAsync(g => g.Name.ToLower() == genreName.ToLower());
            }
            else
            {
                genre = await Db.Genres.FirstOrDefaultAsync(g => g.Name.ToLower() == genreName.ToLower());
            }
            
            if (genre == null)
            {
                throw new GenreNotFoundException(genreName);
            }

            return genre;           
        }
    }
}
