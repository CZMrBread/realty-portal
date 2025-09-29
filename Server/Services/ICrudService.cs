namespace Server.Services;

public interface ICrudService<TDto,TEntity> where TEntity : class
{
    public Task<TDto> CreateAsync(TEntity entity, CancellationToken cancellationToken = default);
    public Task<TDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    public Task<IEnumerable<TDto>> GetAllAsync(CancellationToken cancellationToken = default);
    public Task<TDto?> UpdateAsync(Guid id, TEntity entity, CancellationToken cancellationToken = default);
    public Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}