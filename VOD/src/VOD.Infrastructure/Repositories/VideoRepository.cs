namespace VOD.Infrastructure.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;
    using System.Threading.Tasks;
    using VOD.Domain.Entities;
    using VOD.Domain.Repositories;

    public class VideoRepository : IVideoRepository
    {
        public VideoRepository(VODContext context)
        {
            this._context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IUnitOfWork UnitOfWork => _context;

        private readonly VODContext _context;

        public Video Add(Video video)
        {
            video.Id = Guid.NewGuid();
            video.CreationDate = DateTimeOffset.Now;

            return _context.Videos.Add(video).Entity;
        }

        public async Task<IEnumerable<Video>> GetAsync()
        {
            return await _context.Videos
                .Where(x => !x.IsInactive)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Video> GetAsync(Guid id)
        {
            Video video = await _context.Videos
                .AsNoTracking()
                .Where(x => x.Id == id && !x.IsInactive)
                .Include(x => x.Genre)
                .Include(x => x.Kind).FirstOrDefaultAsync();

            if (video == null) 
            { 
                return null;
            };

            _context.Entry(video).State = EntityState.Detached;

            return video;
        }

        public async Task<IEnumerable<Video>> GetVideosByGenreIdAsync(Guid id)
        {
            IEnumerable<Video> videos = await _context.Videos
                .Where(x => !x.IsInactive)
                .Where(video => video.GenreId == id)
                .Include(x => x.Genre)
                .Include(x => x.Kind)
                .ToListAsync();

            //Genre genres = await _context.Genres
            //    //.Where(x => !x.IsInactive)
            //    .AsNoTracking()
            //    .Where(genre => genre.Id == id)
            //    .Include(x => x.Videos)
            //        .ThenInclude(x => x.Kind)
            //    .FirstOrDefaultAsync();

            //return genres.Videos;
            return videos;
        }

        public async Task<IEnumerable<Video>> GetVideosByKindIdAsync(Guid id)
        {
            IEnumerable<Video> videos = await _context.Videos
                .Where(x => !x.IsInactive)
                .Where(video => video.KindId == id)
                .Include(x => x.Genre)
                .Include(x => x.Kind)
                .ToListAsync();

            //Kind kind = await _context.Kinds
            //    //.Where(x => !x.IsInactive)
            //    .AsNoTracking()
            //    .Where(kind => kind.Id == id)
            //    .Include(x => x.Videos)
            //        .ThenInclude(x => x.Genre)
            //    .FirstOrDefaultAsync();

            //return kind.Videos;
            return videos;
        }

        public Video Update(Video video)
        {
            video.ModificationDate = DateTimeOffset.Now;

            _context.Entry(video).State = EntityState.Modified;

            return video;
        }

        public async Task<IEnumerable<Video>> GetVideosContainTitleAsync(string titleToSearch)
        {
            string lowerTitleToSearch = titleToSearch.ToLower();
            IEnumerable<Video> videos = await _context.Videos
                .AsNoTracking()
                .Where(video => video.Title.ToLower().Contains(lowerTitleToSearch))
                .Include(x => x.Genre)
                .Include(x => x.Kind)
                .ToListAsync();

            return videos;
        }
    }
}
