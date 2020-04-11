namespace Tests
{
    using Entities;
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text;
    using AutoFixture;

    public static class TestDataFactory
    {
        private static Fixture GenData { get; set; } = new Fixture();

        public static Entities.Type CreateType()
        {
            return GenData.Build<Entities.Type>()
                    .Without(t => t.Id)
                    .Without(t => t.Videos)
                    .Without(t => t.CreationDate)
                    .Create();
        }

        public static Entities.Type CreateType(IEnumerable<Video> videos)
        {
            return GenData.Build<Entities.Type>()
                    .Without(t => t.Id)
                    .With(t => t.Videos, videos)
                    .Without(t => t.CreationDate)
                    .Create();
        }

        public static Genre CreateGenre()
        {
            return GenData.Build<Genre>()
                   .Without(g => g.Id)
                   .Without(g => g.VideoLinks)
                   .Without(g => g.CreationDate)
                   .Create();
        }

        public static Genre CreateGenre(IEnumerable<VideoGenre> videosGenres)
        {
            return GenData.Build<Genre>()
                   .Without(g => g.Id)
                   .Without(g => g.CreationDate)
                   .With(g => g.VideoLinks, videosGenres)
                   .Create();
        }

        public static Video CreateVideo(bool withGenreLinks = true)
        {
            Video video = GenData.Build<Video>()
                   .Without(v => v.Id)
                   .Without(v => v.TypeId)
                   .Without(v => v.CreationDate)
                   .With(v => v.Type, CreateType())
                   .Without(v => v.GenreLinks)
                   .Create();

            if (withGenreLinks)
            {
                List<VideoGenre> videosGenres = new List<VideoGenre>()
                {
                    CreateVideoGenre(video)
                };
                video.GenreLinks = videosGenres;
            }          

            return video;
        }

        public static Video CreateVideo(Entities.Type type)
        {
            Video video = GenData.Build<Video>()
                   .Without(v => v.Id)
                   .Without(v => v.TypeId)
                   .Without(v => v.CreationDate)
                   .With(v => v.Type, type)
                   .Without(v => v.GenreLinks)
                   .Create();
            List<VideoGenre> videosGenres = new List<VideoGenre>()
            {
                CreateVideoGenre(video)
            };
            video.GenreLinks = videosGenres;

            return video;
        }

        public static Video CreateVideo(Entities.Type type, IEnumerable<VideoGenre> videosGenres)
        {
            Video video = GenData.Build<Video>()
                   .Without(v => v.Id)
                   .Without(v => v.TypeId)
                   .Without(v => v.CreationDate)
                   .With(v => v.Type, type)
                   .Without(v => v.GenreLinks)
                   .Create();
            VideoGenre[] links = videosGenres.ToArray();
            for (int i = 0; i < links.Length; i++)
            {
                links[i].Video = video;
            }
            video.GenreLinks = links;

            return video;
        }

        public static VideoGenre CreateVideoGenre(Video video)
        {
            return GenData.Build<VideoGenre>()
                    .Without(vg => vg.GenreId)
                    .Without(vg => vg.VideoId)
                    .With(vg => vg.Video, video)
                    .With(vg => vg.Genre, CreateGenre())
                    .Create();
        }

        public static VideoGenre CreateVideoGenre(Video video, Genre genre)
        {
            return GenData.Build<VideoGenre>()
                    .Without(vg => vg.GenreId)
                    .Without(vg => vg.VideoId)
                    .With(vg => vg.Video, video)
                    .With(vg => vg.Genre, genre)
                    .Create();
        }

        public static VideoGenre CreateVideoGenreWithoutVideo(Genre genre)
        {
            return GenData.Build<VideoGenre>()
                    .Without(vg => vg.GenreId)
                    .Without(vg => vg.VideoId)
                    .With(vg => vg.Video, default(Video))
                    .With(vg => vg.Genre, genre)
                    .Create();
        }
    }
}
