namespace Repository;

public interface IBaseRepository<T, K> where T : class where K : IEquatable<K>
{
    public Task<T> Create(T entity);
    public Task<T> GetById(K Id);
    public Task<T> Update(T entity);
    public Task<T> Delete(K entity);
}