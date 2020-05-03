namespace VOD.Fixtures
{
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using VOD.Domain.Entities;
    using VOD.Fixtures.Extensions;
    using VOD.Infrastructure;

    public class TestVODContext : VODContext
    {
        public TestVODContext(DbContextOptions<VODContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Seed<Kind>("./Data/kind.json");
            modelBuilder.Seed<Genre>("./Data/genre.json");
            modelBuilder.Seed<Video>("./Data/video.json");
        }
    }
}
