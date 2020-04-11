namespace API.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ITypeService : IService<Entities.Type>
    {
        Task<bool> IsTypeExistsAsync(string typeName);

        Task<IEnumerable<Entities.Type>> ReadAllAsync();

        Task<Entities.Type> ReadOneWithChildrenAsync(string typeName);

        Task<Entities.Type> DeleteAsync(string typeName);
    }
}
