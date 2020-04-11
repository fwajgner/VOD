namespace API.Services
{
    using API.Exceptions;
    using API.Services.Interfaces;
    using Context;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;

    public class TypeService : ITypeService
    {
        public TypeService(ApplicationDbContext context)
        {
            this.Db = context ?? throw new ArgumentNullException(nameof(context));
        }

        private ApplicationDbContext Db { get; set; }

        public async Task<Entities.Type> CreateAsync(Entities.Type type)
        {
            type.CreationDate = DateTime.Now;
            await Db.Types.AddAsync(type);
            int affected = await Db.SaveChangesAsync();

            if (affected == 1)
            {
                return type;
            }
            else
            {
                return null;
            }
        }

        /// <exception cref="TypeNotFoundException"></exception>
        public async Task<Entities.Type> DeleteAsync(string typeName)
        {
            Entities.Type type;
            try
            {
                type = await this.ReadOneAsync(typeName);
            }
            catch (TypeNotFoundException)
            {
                throw;
            }

            return await Task.Run(() =>
            {
                Db.Types.Remove(type);

                int affected = Db.SaveChanges();

                if (affected == 1)
                {
                    return type;
                }
                else
                {
                    return null;
                }
            });
        }

        public async Task<bool> IsTypeExistsAsync(string typeName)
        {
            try
            {
                Entities.Type type = await this.EnforceTypeExistenceAsync(typeName);
                return type != null;
            }
            catch (TypeNotFoundException)
            {
                return false;
            }
        }

        public async Task<IEnumerable<Entities.Type>> ReadAllAsync()
        {
            return await Db.Types.ToListAsync();
        }

        /// <exception cref="TypeNotFoundException"></exception>
        public Task<Entities.Type> ReadOneAsync(string typeName)
        {
            try
            {
                return this.EnforceTypeExistenceAsync(typeName);
            }
            catch (TypeNotFoundException)
            {
                throw;
            }
        }

        /// <exception cref="TypeNotFoundException"></exception>
        public async Task<Entities.Type> ReadOneWithChildrenAsync(string typeName)
        {
            Entities.Type type = await Db.Types
                .Include(t => t.Videos)
                .FirstOrDefaultAsync(g => g.Name.ToLower() == typeName.ToLower());

            if (type == null)
            {
                throw new TypeNotFoundException(typeName);
            }

            return type;
        }

        /// <exception cref="TypeNotFoundException"></exception>
        public async Task<Entities.Type> UpdateAsync(string typeName, Entities.Type newObj)
        {
            Entities.Type type;
            try
            {
                type = await this.ReadOneAsync(typeName);
            }
            catch (TypeNotFoundException)
            {
                throw;
            }

            return await Task.Run(() =>
            {
                type.Name = newObj.Name;
                type.ModificationDate = DateTime.Now;
                Db.Types.Update(type);

                int affected = Db.SaveChanges();

                if (affected == 1)
                {
                    return type;
                }
                else
                {
                    return null;
                }
            });
        }

        /// <exception cref="TypeNotFoundException"></exception>
        private async Task<Entities.Type> EnforceTypeExistenceAsync(string typeName)
        {
            Entities.Type type = await Db.Types.FirstOrDefaultAsync(g => g.Name.ToLower() == typeName.ToLower());

            if (type == null)
            {
                throw new TypeNotFoundException(typeName);
            }

            return type;
        }
    }
}
