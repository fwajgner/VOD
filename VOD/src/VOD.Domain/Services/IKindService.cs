namespace VOD.Domain.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using VOD.Domain.Responses;
    using VOD.Domain.Requests.Kind;

    public interface IKindService
    {
        Task<IEnumerable<KindResponse>> GetKindAsync();

        Task<KindResponse> GetKindAsync(GetKindRequest request);

        Task<IEnumerable<VideoResponse>> GetVideosByKindIdAsync(GetKindRequest request);

        Task<KindResponse> AddKindAsync(AddKindRequest request);
    }
}
