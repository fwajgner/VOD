namespace VOD.Domain.Services
{
    using Mapster;
    using MapsterMapper;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using VOD.Domain.Entities;
    using VOD.Domain.Repositories;
    using VOD.Domain.Requests.Video;
    using VOD.Domain.Responses;

    public class VideoService : IVideoService
    {
        public VideoService(IVideoRepository videoRepository, IMapper mapper)
        {
            this._videoRepository = videoRepository ?? throw new ArgumentNullException(nameof(videoRepository));
            this._mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        private readonly IVideoRepository _videoRepository;

        private readonly IMapper _mapper;

        public async Task<VideoResponse> AddVideoAsync(AddVideoRequest request)
        {
            Video video = await _mapper.From(request).AdaptToTypeAsync<Video>();

            Video result = _videoRepository.Add(video);

            int affected = await _videoRepository.UnitOfWork.SaveChangesAsync();

            return _mapper.Map<VideoResponse>(result);
        }

        public async Task<VideoResponse> DeleteVideoAsync(DeleteVideoRequest request)
        {
            if (request?.Id == null)
            {
                throw new ArgumentNullException();
            }

            Video result = await _videoRepository.GetAsync(request.Id);
            result.IsInactive = true;

            _videoRepository.Update(result);
            int affected = await _videoRepository.UnitOfWork.SaveChangesAsync();

            return _mapper.Map<VideoResponse>(result);
        }

        public async Task<VideoResponse> EditVideoAsync(EditVideoRequest request)
        {
            Video existingRecord = await _videoRepository.GetAsync(request.Id);

            if (existingRecord == null)
            { 
                throw new ArgumentException($"Video entity with {request.Id} is not present");
            }

            Video entity = _mapper.Map<Video>(request);
            Video result = _videoRepository.Update(entity);

            int affected = await _videoRepository.UnitOfWork.SaveChangesAsync();

            return _mapper.Map<VideoResponse>(result);
        }

        public async Task<VideoResponse> GetVideoAsync(GetVideoRequest request)
        {
            if (request?.Id == null)
            { 
                throw new ArgumentNullException();
            }

            Video entity = await _videoRepository.GetAsync(request.Id);

            return _mapper.Map<VideoResponse>(entity);
        }

        public async Task<IEnumerable<VideoResponse>> GetVideoAsync()
        {
            IEnumerable<Video> result = await _videoRepository.GetAsync();

            return _mapper.Map<IEnumerable<VideoResponse>>(result);
        }
    }
}
