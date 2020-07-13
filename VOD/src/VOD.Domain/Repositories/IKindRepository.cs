namespace VOD.Domain.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using VOD.Domain.Entities;

    public interface IKindRepository : IRepository
    {
        Task<IEnumerable<Kind>> GetAsync();

        Task<Kind> GetAsync(Guid id);

        Kind Add(Kind kind);

        Kind Update(Kind kind);
    }
}
