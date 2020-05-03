namespace VOD.Infrastructure.Repositories
{
    using System;
    using System.Collections.Generic;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using System.Threading.Tasks;
    using VOD.Domain.Entities;
    using VOD.Domain.Repositories;

    public class KindRepository : IKindRepository
    {
        public KindRepository(VODContext context)
        {
            this._context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IUnitOfWork UnitOfWork => _context;

        private readonly VODContext _context;

        public Kind Add(Kind kind)
        {
            kind.Id = new Guid();
            kind.CreationDate = DateTimeOffset.Now;

            return _context.Kinds
                .Add(kind).Entity;
        }

        public async Task<IEnumerable<Kind>> GetAsync()
        {
            return await _context
                .Kinds
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Kind> GetAsync(Guid id)
        {
            Kind kind = await _context.Kinds
                .FindAsync(id);

            if (kind == null)
            {
                return null;
            }

            _context.Entry(kind).State = EntityState.Detached;

            return kind;
        }

        public Kind Update(Kind kind)
        {
            kind.ModificationDate = DateTimeOffset.Now;

            _context.Entry(kind).State = EntityState.Modified;

            return kind;
        }
    }
}
