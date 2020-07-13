namespace VOD.Fixtures.Extensions
{
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using System.IO;

    public static class ModelBuilderExtensions
    {
        public static ModelBuilder Seed<T>(this ModelBuilder modelBuilder, string file) where T : class 
        {
            using (StreamReader reader = new StreamReader(file))
            {
                string json = reader.ReadToEnd();
                T[] data = JsonConvert.DeserializeObject<T[]>(json);
                modelBuilder.Entity<T>().HasData(data);
            }

            return modelBuilder;
        }
    }
}
