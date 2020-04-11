namespace Context
{
    using Entities;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class DbSeeder
    {
        #region Public Methods

        public static void Seed(ApplicationDbContext dbContext)
        {
            if(!dbContext.Users.Any())
            {
                CreateUsers(dbContext);
            }

            if (!dbContext.Genres.Any())
            {
                CreateGenres(dbContext);
            }

            if (!dbContext.Types.Any())
            {
                CreateTypes(dbContext);
            }

            if (!dbContext.Videos.Any())
            {
                CreateVideos(dbContext);
            }

            if (!dbContext.VideosGenres.Any())
            {
                CreateVideoGenres(dbContext);
            }
        }

        #endregion

        #region Private Methods
        private static void CreateUsers(ApplicationDbContext dbContext)
        {
            ApplicationUser admin = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "Admin",
                Email = "admin@admin.com"
            };

#if DEBUG
            ApplicationUser user0 = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "User0",
                Email = "user0@test.com",
                SubStartDate = DateTime.Now,
                SubEndDate = DateTime.Now.AddMonths(1)
            };

            ApplicationUser user1 = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "User1",
                Email = "user1@test.com",
            };

            dbContext.AddRange(admin, user0, user1);
#endif

            dbContext.SaveChanges();
        }

        private static void CreateGenres(ApplicationDbContext dbContext)
        {
            Genre gen0 = new Genre()
            {
                Name = "Drama"
            };

            Genre gen1 = new Genre()
            {
                Name = "Comedy"
            };

            Genre gen2 = new Genre()
            {
                Name = "Thriller"
            };

            Genre gen3 = new Genre()
            {
                Name = "Romance"
            };

            Genre gen4 = new Genre()
            {
                Name = "Action"
            };

            Genre gen5 = new Genre()
            {
                Name = "Horror"
            };

            Genre gen6 = new Genre()
            {
                Name = "Crime"
            };

            Genre gen7 = new Genre()
            {
                Name = "Adventure"
            };

            Genre gen8 = new Genre()
            {
                Name = "Mystery"
            };

            Genre gen9 = new Genre()
            {
                Name = "Animation"
            };

            Genre gen10 = new Genre()
            {
                Name = "Sci-Fi"
            };

            dbContext.Genres.AddRange(gen0, gen1, gen2, gen3, gen4, gen5, gen6, gen7,
                gen8, gen9, gen10);

            dbContext.SaveChanges();
        }

        private static void CreateTypes(ApplicationDbContext dbContext)
        {
            Entities.Type type0 = new Entities.Type()
            {
               Name = "Movie"
            };

            Entities.Type type1 = new Entities.Type()
            {
                Name = "Series"
            };

            dbContext.Types.AddRange(type0, type1);
            dbContext.SaveChanges();
        }

        private static void CreateVideos(ApplicationDbContext dbContext)
        {           
            int movieId = dbContext.Types
                .Where(t => t.Name == "Movie")
                .FirstOrDefault()
                .Id;

            int seriesId = dbContext.Types
                .Where(t => t.Name == "Series")
                .FirstOrDefault()
                .Id;
#if DEBUG

            DateTime now = DateTime.Now;
            Random rand = new Random();
            List<Video> videoList = new List<Video>();

            #region Description

            string desc = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, " +
                "sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. " +
                "A diam maecenas sed enim. Sollicitudin ac orci phasellus egestas tellus " +
                "rutrum tellus pellentesque. Pellentesque sit amet porttitor eget dolor " +
                "morbi non arcu. Aliquet nibh praesent tristique magna sit amet purus " +
                "gravida. Aliquam faucibus purus in massa tempor. Scelerisque purus " +
                "semper eget duis at tellus. In iaculis nunc sed augue lacus viverra " +
                "vitae. Nulla at volutpat diam ut venenatis tellus in metus. Elit at " +
                "imperdiet dui accumsan sit amet nulla.";

            #endregion

            #region Movie

            for (int i = 0; i < 90; i++)
            {
                int day = rand.Next(1, 31);
                int month = rand.Next(1, 13);
                int year = rand.Next(1990, 2021);

                Video movie = new Video()
                {
                    Title = $"Title{i}",
                    AltTitle = $"AltTitle{i}",
                    ReleaseYear = new DateTime(year, month, day),
                    Duration = (ushort)rand.Next(70, 200),
                    Description = desc,
                    TypeId = movieId
                };

                videoList.Add(movie);
            }

            for (int i = 0; i < 10; i++)
            {
                int day = rand.Next(1, 31);
                int month = rand.Next(1, 13);
                int year = rand.Next(1990, 2021);

                Video movie = new Video()
                {
                    Title = $"Title40",
                    AltTitle = $"AltTitle40-{i}",
                    ReleaseYear = new DateTime(year, month, day),
                    Duration = (ushort)rand.Next(70, 200),
                    Description = desc,
                    TypeId = movieId
                };

                videoList.Add(movie);
            }

            #endregion

            #region Series

            for (int i = 0; i < 90; i++)
            {
                int day = rand.Next(1, 31);
                int month = rand.Next(1, 13);
                int year = rand.Next(1990, 2021);

                Video series = new Video()
                {
                    Title = $"Title{i}",
                    AltTitle = $"AltTitle{i}",
                    ReleaseYear = new DateTime(year, month, day),
                    Duration = (ushort)rand.Next(30, 61),
                    Description = desc,
                    TypeId = seriesId
                };

                videoList.Add(series);
            }

            for (int i = 0; i < 10; i++)
            {
                int day = rand.Next(1, 31);
                int month = rand.Next(1, 13);
                int year = rand.Next(1990, 2021);
                ushort season = (ushort)rand.Next(1, 10);
                ushort episode = (ushort)rand.Next(1, 20);

                Video movie = new Video()
                {
                    Title = $"Title40",
                    AltTitle = $"AltTitle40s{season}e{episode}",
                    ReleaseYear = new DateTime(year, month, day),
                    Duration = (ushort)rand.Next(70, 200),
                    Description = desc,
                    TypeId = seriesId,
                    Season = season,
                    Episode = episode
                };

                videoList.Add(movie);
            }

            #endregion

            dbContext.Videos.AddRange(videoList);

#endif
            dbContext.SaveChanges();
        }

        private static void CreateVideoGenres(ApplicationDbContext dbContext)
        {
            #region Genres Identifiers          

            int dramaId = dbContext.Genres
                .Where(g => g.Name == "Drama")
                .FirstOrDefault()
                .Id;         

            int comedyId = dbContext.Genres
                .Where(g => g.Name == "Comedy")
                .FirstOrDefault()
                .Id;

            int thrillerId = dbContext.Genres
                .Where(g => g.Name == "Thriller")
                .FirstOrDefault()
                .Id;

            int romanceId = dbContext.Genres
                .Where(g => g.Name == "Romance")
                .FirstOrDefault()
                .Id;

            int actionId = dbContext.Genres
                .Where(g => g.Name == "Action")
                .FirstOrDefault()
                .Id;

            int horrorId = dbContext.Genres
                .Where(g => g.Name == "Horror")
                .FirstOrDefault()
                .Id;

            int crimeId = dbContext.Genres
                .Where(g => g.Name == "Crime")
                .FirstOrDefault()
                .Id;

            int adventureId = dbContext.Genres
                .Where(g => g.Name == "Adventure")
                .FirstOrDefault()
                .Id;

            int mysteryId = dbContext.Genres
                .Where(g => g.Name == "Mystery")
                .FirstOrDefault()
                .Id;

            int animationId = dbContext.Genres
                .Where(g => g.Name == "Animation")
                .FirstOrDefault()
                .Id;

            int sciFiId = dbContext.Genres
                .Where(g => g.Name == "Sci-Fi")
                .FirstOrDefault()
                .Id;

            #endregion

#if DEBUG

            int[] genresId = new int[11];
            genresId[0] = dramaId;
            genresId[1] = comedyId;
            genresId[2] = thrillerId;
            genresId[3] = romanceId;
            genresId[4] = actionId;
            genresId[5] = horrorId;
            genresId[6] = crimeId;
            genresId[7] = adventureId;
            genresId[8] = mysteryId;
            genresId[9] = animationId;
            genresId[10] = sciFiId;

            Random rand = new Random();
            List<VideoGenre> vgList = new List<VideoGenre>();

            foreach (Video video in dbContext.Videos)
            {
                VideoGenre vg = new VideoGenre()
                {
                    VideoId = video.Id,
                    GenreId = genresId[rand.Next(11)]
                };

                vgList.Add(vg);
            }

            dbContext.AddRange(vgList);
#endif

            dbContext.SaveChanges();
        }

        #endregion
    }
}
