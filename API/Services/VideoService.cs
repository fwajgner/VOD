namespace API.Services
{
    using API.Services.Interfaces;
    using Context;
    using Entities;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using API.Exceptions;

    public class VideoService : IVideoService
    {
        public VideoService(ApplicationDbContext context, ITypeService typeService, IGenreService genreService)
        {
            this.Db = context ?? throw new ArgumentNullException(nameof(context));
            this.TypeService = typeService;
            this.GenreService = genreService;
        }

        private ApplicationDbContext Db { get; set; }

        private ITypeService TypeService { get; set; }

        private IGenreService GenreService { get; set; }

        public async Task<Video> CreateAsync(Video video)
        {
            await Db.AddAsync(video);

            int affected = await Db.SaveChangesAsync();
            if (affected >= 1)
            {
                return video;
            }
            else
            {
                return null;
            }
        }

        /// <exception cref="VideoNotFoundException"></exception>
        public async Task<Video> DeleteAsync(string altTitle)
        {
            Video video;
            try
            {
                video = await this.ReadOneWithChildrenAsync(altTitle);
            }
            catch (VideoNotFoundException)
            {
                throw;
            }
            
            return await Task.Run(() => 
            { 
                Db.VideosGenres.RemoveRange(video.GenreLinks);
                Db.Videos.Remove(video);
                int expectedAffected = video.GenreLinks.Count() + 1;
                int affected = Db.SaveChanges();

                if (affected == expectedAffected)
                {
                    return video;
                }
                else
                {
                    return null;
                }
            });
        }

        /// <exception cref="TitleTooShortException"></exception>
        public async Task<IEnumerable<Video>> ReadAllContainTitleAsync(string title)
        {
            int length = 3;
            if (title == null || title.Length < length)
            {
                throw new TitleTooShortException(length);
            }

            IEnumerable<Video> videos = await Db.Videos
                .Where(v => v.Title.ToLower().Contains(title.ToLower()))
                .Include(v => v.Type)
                .Include(v => v.GenreLinks)
                    .ThenInclude(vg => vg.Genre)
                .ToListAsync();

            if (videos.Count() == 0)
            {
                return null;
            }
            else
            {
                return videos;
            }
        }

        /// <exception cref="GenreNotFoundException"></exception>
        public async Task<IEnumerable<Video>> ReadAllWithGenreAsync(string genreName)
        {
            Genre genre;
            try
            {
                genre = await GenreService.ReadOneWithChildrenAsync(genreName);
            }
            catch (GenreNotFoundException)
            {
                throw;
            }

            if (genre.VideoLinks.Count() == 0)
            {
                return null;
            }
            else
            {
                return genre.VideoLinks.Select(g => g.Video).AsEnumerable();
            }
        }

        public async Task<IEnumerable<Video>> ReadAllWithTypeAsync(string typeName)
        {
            Entities.Type type;
            try
            {
                type = await TypeService.ReadOneWithChildrenAsync(typeName);
            }
            catch (TypeNotFoundException)
            {
                throw;
            }

            if (type.Videos.Count() == 0)
            {
                return null;
            }
            else
            {
                return type.Videos.AsEnumerable();
            }            
        }

        /// <exception cref="VideoNotFoundException"></exception>
        public async Task<Video> ReadOneAsync(string altTitle)
        {
            Video video = await Db.Videos.FirstOrDefaultAsync(v => v.AltTitle.ToLower() == altTitle.ToLower());

            if (video == null)
            {
                throw new VideoNotFoundException(altTitle);
            }

            return video;
        }

        /// <exception cref="VideoNotFoundException"></exception>
        public async Task<Video> ReadOneWithChildrenAsync(string altTitle)
        {
            Video video = await Db.Videos
                .Include(v => v.Type)
                .Include(v => v.GenreLinks)
                    .ThenInclude(vg => vg.Genre)
                .FirstOrDefaultAsync(v => v.AltTitle.ToLower() == altTitle.ToLower());

            if (video == null)
            {
                throw new VideoNotFoundException(altTitle);
            }

            return video;
        }

        /// <exception cref="VideoNotFoundException"></exception>
        public async Task<Video> UpdateAsync(string altTitle, Video newVideo)
        {
            Video video;

            try
            {
                video = await this.ReadOneWithChildrenAsync(altTitle);
            }
            catch (VideoNotFoundException)
            {
                throw;
            }

            return await Task.Run(() =>
             {
                 video.AltTitle = newVideo.AltTitle;
                 video.Description = newVideo.Description;
                 video.Duration = newVideo.Duration;
                 video.Episode = newVideo.Episode;
                 video.ReleaseYear = newVideo.ReleaseYear;
                 video.Season = newVideo.Season;
                 video.Title = newVideo.Title;
                 video.Type = newVideo.Type;
                 video.GenreLinks = newVideo.GenreLinks;
                 video.ModificationDate = DateTime.Now;

                 Db.Update(video);
                 int affected = Db.SaveChanges();
                 if (affected >= 1)
                 {
                     return video;
                 }
                 else
                 {
                     return null;
                 }
             });
        }
    }
}
