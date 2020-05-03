namespace VOD.Domain.Mappers
{
    using System;
    using System.Linq.Expressions;
    using System.Linq;
    using Mapster;
    using VOD.Domain.Entities;
    using VOD.Domain.Responses;
    using VOD.Domain.Requests.Kind;
    using VOD.Domain.Requests.Genre;
    using VOD.Domain.Requests.Video;

    public static class MapperConfig
    {
        public static TypeAdapterConfig CreateMap()
        {
            TypeAdapterConfig config = new TypeAdapterConfig();

            config.NewConfig<Kind, KindResponse>()
                  .MaxDepth(1)
                  .Map(dest => dest.KindId, src => src.Id)
                  .TwoWays();

            config.NewConfig<AddKindRequest, Kind>()
                .MaxDepth(1)
                .Map(dest => dest.Name, src => src.KindName)
                .TwoWays();

            //config.NewConfig<Genre, GetGenreRequest>();

            config.NewConfig<Genre, GenreResponse>()
                .MaxDepth(1)
                .Map(dest => dest.GenreId, src => src.Id)
                .TwoWays();

            config.NewConfig<AddGenreRequest, Genre>()
                .MaxDepth(1)
                .Map(dest => dest.Name, src => src.GenreName)
                .TwoWays();

            //config.NewConfig<AddVideoRequest, Video>();

            //config.NewConfig<EditVideoRequest, Video>();

            config.NewConfig<Video, VideoResponse>()
                .MaxDepth(2)
                .TwoWays();

            config.Compile();

            return config;
        }
    }
}
