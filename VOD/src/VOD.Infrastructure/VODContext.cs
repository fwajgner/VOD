namespace VOD.Infrastructure
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using VOD.Domain.Entities;
    using VOD.Domain.Repositories;
    using VOD.Infrastructure.SchemaDefinitions;

    public class VODContext : IdentityDbContext<User>, IUnitOfWork
    {
        #region Consctructors

        public VODContext(DbContextOptions<VODContext> options)
            : base(options)
        {

        }

        #endregion

        #region Properties

        public const string DEFAULT_SCHEMA = "vod";

        public DbSet<Genre> Genres { get; set; }

        public DbSet<Kind> Kinds { get; set; }

        public DbSet<Video> Videos { get; set; }

        //public DbSet<VideoGenre> VideosGenres { get; set; }

        #endregion

        #region Methods

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserEntitySchemaDefinitions());

            modelBuilder.ApplyConfiguration(new GenreEntitySchemaDefinitions());

            modelBuilder.ApplyConfiguration(new KindEntitySchemaDefinitions());

            modelBuilder.ApplyConfiguration(new VideoEntitySchemaDefinitions());

            #region VideoGenre

            //modelBuilder.Entity<VideoGenre>()
            //    .HasKey(v => new { v.VideoId, v.GenreId });

            //modelBuilder.Entity<VideoGenre>()
            //    .HasOne(vg => vg.Genre)
            //    .WithMany(g => g.VideoLinks)
            //    .HasForeignKey(vg => vg.GenreId)
            //    .OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.Entity<VideoGenre>()
            //    .HasOne(vg => vg.Video)
            //    .WithMany(v => v.GenreLinks)
            //    .HasForeignKey(vg => vg.VideoId)
            //    .OnDelete(DeleteBehavior.Restrict);

            #endregion

            base.OnModelCreating(modelBuilder);
        }

        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
        {
            await SaveChangesAsync(cancellationToken);
            return true;
        }

        #endregion
    }
}
