namespace VOD.Infrastructure
{
    using Microsoft.AspNetCore.Identity;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using VOD.Domain.Entities;

    public static class DbSeeder
    {
        #region Public Methods

        public static void Seed(VODContext dbContext, RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            if (!dbContext.Users.Any())
            {
                CreateUsersAsync(dbContext, roleManager, userManager).Wait();
            }

            if (!dbContext.Genres.Any())
            {
                CreateGenres(dbContext);
            }

            if (!dbContext.Kinds.Any())
            {
                CreateKinds(dbContext);
            }

            if (!dbContext.Videos.Any())
            {
                CreateVideos(dbContext);
            }

            //if (!dbContext.VideosGenres.Any())
            //{
            //    CreateVideoGenres(dbContext);
            //}
        }

        #endregion

        #region Private Methods
        private static async Task CreateUsersAsync(VODContext dbContext, RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            string roleAdmin = "Administrator";
            string roleRegisteredUser = "RegisteredUser";

            if (!await roleManager.RoleExistsAsync(roleAdmin))
            {
                await roleManager.CreateAsync(new IdentityRole(roleAdmin));
            }
            if (!await roleManager.RoleExistsAsync(roleRegisteredUser))
            {
                await roleManager.CreateAsync(new IdentityRole(roleRegisteredUser));
            }

            User admin = new User()
            {
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = "admin@vod.com",
                Email = "admin@vod.com",
                CreationDate = DateTime.Now
            };

            if (await userManager.FindByEmailAsync(admin.Email) == null)
            {
                await userManager.CreateAsync(admin, "Pa$$w0rd");
                await userManager.AddToRoleAsync(admin, roleAdmin);
                await userManager.AddToRoleAsync(admin, roleRegisteredUser);
                admin.EmailConfirmed = true;
                admin.LockoutEnabled = false;
            }

#if DEBUG
            User user0 = new User()
            {
                SecurityStamp = Guid.NewGuid().ToString(),
                Name = "User0",
                UserName = "user0@vod.com",
                Email = "user0@vod.com",
                CreationDate = DateTime.Now,
                SubStartDate = DateTime.Now,
                SubEndDate = DateTime.Now.AddMonths(1)
            };

            if (await userManager.FindByEmailAsync(user0.Email) == null)
            {
                await userManager.CreateAsync(user0, "Pa$$w0rd");
                await userManager.AddToRoleAsync(user0, roleRegisteredUser);
                user0.EmailConfirmed = true;
            }

            User user1 = new User()
            {
                SecurityStamp = Guid.NewGuid().ToString(),
                Name = "User1",
                UserName = "user1@vod.com",
                Email = "user1@vod.com",
                CreationDate = DateTime.Now
            };

            if (await userManager.FindByEmailAsync(user1.Email) == null)
            {
                await userManager.CreateAsync(user1, "Pa$$w0rd");
                await userManager.AddToRoleAsync(user1, roleRegisteredUser);
                user1.EmailConfirmed = true;
            }
#endif

            await dbContext.SaveChangesAsync();
        }

        private static void CreateGenres(VODContext dbContext)
        {
            DateTime now = DateTime.Now;

            Genre gen0 = new Genre()
            {
                Name = "Drama",
                CreationDate = now
            };

            Genre gen1 = new Genre()
            {
                Name = "Comedy",
                CreationDate = now
            };

            Genre gen2 = new Genre()
            {
                Name = "Thriller",
                CreationDate = now
            };

            Genre gen3 = new Genre()
            {
                Name = "Romance",
                CreationDate = now
            };

            Genre gen4 = new Genre()
            {
                Name = "Action",
                CreationDate = now
            };

            Genre gen5 = new Genre()
            {
                Name = "Horror",
                CreationDate = now
            };

            Genre gen6 = new Genre()
            {
                Name = "Crime",
                CreationDate = now
            };

            Genre gen7 = new Genre()
            {
                Name = "Adventure",
                CreationDate = now
            };

            Genre gen8 = new Genre()
            {
                Name = "Mystery",
                CreationDate = now
            };

            Genre gen9 = new Genre()
            {
                Name = "Animation",
                CreationDate = now
            };

            Genre gen10 = new Genre()
            {
                Name = "Sci-Fi",
                CreationDate = now
            };

            dbContext.Genres.AddRange(gen0, gen1, gen2, gen3, gen4, gen5, gen6, gen7,
                gen8, gen9, gen10);

            dbContext.SaveChanges();
        }

        private static void CreateKinds(VODContext dbContext)
        {
            Kind kind0 = new Kind()
            {
                Name = "Movie",
                CreationDate = DateTime.Now
            };

            Kind kind1 = new Kind()
            {
                Name = "Series",
                CreationDate = DateTime.Now
            };

            dbContext.Kinds.AddRange(kind0, kind1);
            dbContext.SaveChanges();
        }

        private static void CreateVideos(VODContext dbContext)
        {
            Guid movieId = dbContext.Kinds
                .Where(t => t.Name == "Movie")
                .FirstOrDefault()
                .Id;

            Guid seriesId = dbContext.Kinds
                .Where(t => t.Name == "Series")
                .FirstOrDefault()
                .Id;

            #region Genres Identifiers          

            Guid dramaId = dbContext.Genres
                .Where(g => g.Name == "Drama")
                .FirstOrDefault()
                .Id;

            Guid comedyId = dbContext.Genres
                .Where(g => g.Name == "Comedy")
                .FirstOrDefault()
                .Id;

            Guid thrillerId = dbContext.Genres
                .Where(g => g.Name == "Thriller")
                .FirstOrDefault()
                .Id;

            Guid romanceId = dbContext.Genres
                .Where(g => g.Name == "Romance")
                .FirstOrDefault()
                .Id;

            Guid actionId = dbContext.Genres
                .Where(g => g.Name == "Action")
                .FirstOrDefault()
                .Id;

            Guid horrorId = dbContext.Genres
                .Where(g => g.Name == "Horror")
                .FirstOrDefault()
                .Id;

            Guid crimeId = dbContext.Genres
                .Where(g => g.Name == "Crime")
                .FirstOrDefault()
                .Id;

            Guid adventureId = dbContext.Genres
                .Where(g => g.Name == "Adventure")
                .FirstOrDefault()
                .Id;

            Guid mysteryId = dbContext.Genres
                .Where(g => g.Name == "Mystery")
                .FirstOrDefault()
                .Id;

            Guid animationId = dbContext.Genres
                .Where(g => g.Name == "Animation")
                .FirstOrDefault()
                .Id;

            Guid sciFiId = dbContext.Genres
                .Where(g => g.Name == "Sci-Fi")
                .FirstOrDefault()
                .Id;

            Guid[] genresId = new Guid[11];
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

            #endregion
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
                int day = rand.Next(1, 29);
                int month = rand.Next(1, 13);
                int year = rand.Next(1990, 2021);

                Video movie = new Video()
                {
                    Title = $"Title{i}",
                    AltTitle = $"AltTitle{i}",
                    ReleaseYear = new DateTime(year, month, day),
                    Duration = (ushort)rand.Next(70, 200),
                    Description = desc,
                    KindId = movieId,
                    GenreId = genresId[rand.Next(genresId.Length)],
                    CreationDate = now
                };

                videoList.Add(movie);
            }

            for (int i = 0; i < 10; i++)
            {
                int day = rand.Next(1, 29);
                int month = rand.Next(1, 13);
                int year = rand.Next(1990, 2021);

                Video movie = new Video()
                {
                    Title = $"Title40",
                    AltTitle = $"AltTitle40-{i}",
                    ReleaseYear = new DateTime(year, month, day),
                    Duration = (ushort)rand.Next(70, 200),
                    Description = desc,
                    KindId = movieId,
                    GenreId = genresId[rand.Next(genresId.Length)],
                    CreationDate = now
                };

                videoList.Add(movie);
            }

            #endregion

            #region Series

            for (int i = 0; i < 90; i++)
            {
                int day = rand.Next(1, 29);
                int month = rand.Next(1, 13);
                int year = rand.Next(1990, 2021);

                Video series = new Video()
                {
                    Title = $"Title{i}S",
                    AltTitle = $"AltTitle{i}s{month}e{day}",
                    ReleaseYear = new DateTime(year, month, day),
                    Duration = (ushort)rand.Next(30, 61),
                    Description = desc,
                    KindId = seriesId,
                    GenreId = genresId[rand.Next(genresId.Length)],
                    CreationDate = now,
                    Season = (ushort)month,
                    Episode = (ushort)day
                };

                videoList.Add(series);
            }

            for (int i = 0; i < 10; i++)
            {
                int day = rand.Next(1, 29);
                int month = rand.Next(1, 13);
                int year = rand.Next(1990, 2021);
                ushort season = (ushort)(i + 1);
                ushort episode = (ushort)(day + 28);

                Video series = new Video()
                {
                    Title = $"Title40S",
                    AltTitle = $"AltTitle40s{season}e{episode}",
                    ReleaseYear = new DateTime(year, month, day),
                    Duration = (ushort)rand.Next(70, 200),
                    Description = desc,
                    KindId = seriesId,
                    GenreId = genresId[rand.Next(genresId.Length)],
                    CreationDate = now,
                    Season = season,
                    Episode = episode
                };

                videoList.Add(series);
            }

            #endregion

            dbContext.Videos.AddRange(videoList);

#endif
            dbContext.SaveChanges();
        }

        /*private static void CreateVideoGenres(VODContext dbContext)
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
        }*/

        #endregion
    }
}