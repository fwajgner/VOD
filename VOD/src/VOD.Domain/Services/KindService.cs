namespace VOD.Domain.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Linq;
    using VOD.Domain.Responses;
    using VOD.Domain.Requests.Kind;
    using MapsterMapper;
    using VOD.Domain.Repositories;
    using VOD.Domain.Entities;
    using Mapster;

    public class KindService : IKindService
    {
        public KindService(IKindRepository kindRepository, IVideoRepository videoRepository, IMapper mapper)
        {
            this._kindRepository = kindRepository ?? throw new ArgumentNullException(nameof(kindRepository));
            this._videoRepository = videoRepository ?? throw new ArgumentNullException(nameof(videoRepository));
            this._mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        private readonly IKindRepository _kindRepository;

        private readonly IVideoRepository _videoRepository;

        private readonly IMapper _mapper;

        public async Task<KindResponse> AddKindAsync(AddKindRequest request)
        {
            Kind kind = await _mapper.From(request).AdaptToTypeAsync<Kind>();

            Kind result = _kindRepository.Add(kind);

            int affected = await _kindRepository.UnitOfWork.SaveChangesAsync();

            return _mapper.Map<KindResponse>(result);
        }

        public async Task<KindResponse> GetKindAsync(GetKindRequest request)
        {
            if (request?.Id == null) 
            {
                throw new ArgumentNullException();
            };

            Kind result = await _kindRepository.GetAsync(request.Id);

            return result == null ? null : _mapper.Map<KindResponse>(result);
        }

        public async Task<IEnumerable<KindResponse>> GetKindAsync()
        {
            IEnumerable<Kind> result = await _kindRepository.GetAsync();

            return _mapper.Map<IEnumerable<KindResponse>>(result);
        }

        public async Task<IEnumerable<VideoResponse>> GetVideosByKindIdAsync(GetKindRequest request)
        {
            if (request?.Id == null)
            {
                throw new ArgumentNullException();
            };

            IEnumerable<Video> result = await _videoRepository.GetVideosByKindIdAsync(request.Id);

            return _mapper.Map<IEnumerable<VideoResponse>>(result);
        }
    }
}
