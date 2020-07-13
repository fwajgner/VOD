namespace VOD.Domain.Services
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using VOD.Domain.Responses;
    using VOD.Domain.Requests.Genre;
    using VOD.Domain.Repositories;
    using MapsterMapper;
    using VOD.Domain.Entities;
    using Mapster;

    public class GenreService : IGenreService
    {
        public GenreService(IGenreRepository genreRepository, IVideoRepository videoRepository, IMapper mapper)
        {
            this._genreRepository = genreRepository ?? throw new ArgumentNullException(nameof(genreRepository));
            this._videoRepository = videoRepository ?? throw new ArgumentNullException(nameof(videoRepository));
            this._mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        private readonly IGenreRepository _genreRepository;

        private readonly IVideoRepository _videoRepository;

        private readonly IMapper _mapper;

        public async Task<GenreResponse> AddGenreAsync(AddGenreRequest request)
        {
            Genre genre = await _mapper.From(request).AdaptToTypeAsync<Genre>();

            Genre result = _genreRepository.Add(genre);

            int affected = await _genreRepository.UnitOfWork.SaveChangesAsync();

            return _mapper.Map<GenreResponse>(result);
        }

        public async Task<GenreResponse> GetGenreAsync(GetGenreRequest request)
        {
            if (request?.Id == null)
            {
                throw new ArgumentNullException();
            }

            Genre result = await _genreRepository.GetAsync(request.Id);

            return result == null ? null : _mapper.Map<GenreResponse>(result);
        }

        public async Task<IEnumerable<GenreResponse>> GetGenreAsync()
        {

            IEnumerable<Genre> result = await _genreRepository.GetAsync();

            return _mapper.Map<IEnumerable<GenreResponse>>(result);
        }

        public async Task<IEnumerable<VideoResponse>> GetVideosByGenreIdAsync(GetGenreRequest request)
        {
            if (request?.Id == null)
            {
                throw new ArgumentNullException();
            }

            IEnumerable<Video> result = await _videoRepository.GetVideosByGenreIdAsync(request.Id);

            return _mapper.Map<IEnumerable<VideoResponse>>(result);
        }
    }
}
