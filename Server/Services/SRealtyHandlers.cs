using Server.Database;
using Server.Entities.SRealtyRealty;

namespace Server.Services;

public sealed class SRealtyHandlers: ICrudService<SRealtyPropertyEntity>
{
    private readonly AppDbContext _context; // Replace with your DbContext name

    public SRealtyHandlers(AppDbContext context)
    {
        _context = context;
    }

    public Task<SRealtyPropertyEntity> CreateAsync(SRealtyPropertyEntity entity, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<SRealtyPropertyEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
    
    public Task<SRealtyPropertyEntity?> GetByIdAsync(string advertRkId, string realtyAgentRkId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<SRealtyPropertyEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<SRealtyPropertyEntity?> UpdateAsync(Guid id, SRealtyPropertyEntity entity, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}