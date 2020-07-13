namespace VOD.Fixtures.Data
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text;
    using AutoFixture;
    using Mapster;
    using System.Linq.Expressions;
    using VOD.Domain.Entities;
    using VOD.Domain.Responses;
    using VOD.Domain.Requests.Kind;
    using VOD.Domain.Requests.Genre;
    using ExpressionDebugger;
    using VOD.Domain.Requests.Video;

    public static class TestDataFactory
    {
        private static Fixture GenData { get; set; } = new Fixture();

        #region Kind

        public static Kind CreateKind()
        {
            return GenData.Build<Kind>()
                    .Without(t => t.Id)
                    .Without(t => t.Videos)
                    .Create();
        }

        public static Kind CreateKind(IEnumerable<Video> videos)
        {
            Kind kind = CreateKind();
            kind.Videos = videos.ToList();
            return kind;
        }

        #endregion

        #region Genre

        public static Genre CreateGenre()
        {
            return GenData.Build<Genre>()
                   .Without(g => g.Id)
                   .Without(g => g.Videos)
                   .Create();
        }

        public static Genre CreateGenre(IEnumerable<Video> videos)
        {
            Genre genre = CreateGenre();
            genre.Videos = videos.ToList();
            return genre;
        }

        #endregion

        #region Video

        public static Video CreateVideo()
        {
            Video video = GenData.Build<Video>()
                   .Without(v => v.Id)
                   .Without(v => v.KindId)
                   .With(v => v.Kind, CreateKind())
                   .Without(v => v.GenreId)
                   .With(v => v.Genre, CreateGenre())
                   .With(v => v.IsInactive, false)
                   .Create();        

            return video;
        }

        public static Video CreateVideo(Kind kind)
        {
            Video video = CreateVideo();
            video.Kind = kind;

            return video;
        }

        public static Video CreateVideo(Kind kind, Genre genre)
        {
            Video video = CreateVideo();
            video.Kind = kind;
            video.Genre = genre;

            return video;
        }

        public static AddVideoRequest CreateAddVideoRequest()
        {
            AddVideoRequest add = GenData.Create<AddVideoRequest>();
            
            return add;
        }

        public static EditVideoRequest CreateEditVideoRequest()
        {
            EditVideoRequest edit = GenData
                .Build<EditVideoRequest>()
                .With(x => x.IsInactive, false)
                .Create();

            return edit;
        }

        #endregion

        public static TypeAdapterConfig CreateMapper()
        {
            TypeAdapterConfig config = new TypeAdapterConfig()
            {
                Compiler = exp => exp.CompileWithDebugInfo(new ExpressionCompilationOptions { ThrowOnFailedCompilation = true })
            };

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
