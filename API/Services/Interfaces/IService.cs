namespace API.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IService<T>
    {
        Task<T> CreateAsync(T obj);

        Task<T> ReadOneAsync(string id);

        Task<T> UpdateAsync(string id, T newObj);
    }
}
