using DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace Repository;

public class BaseRepository<T, K> : IBaseRepository<T, K>
        where T : class
        where K : IEquatable<K>
    {
        protected readonly DbSet<T> _dbSet;
        protected readonly AppDbContext _dbContext;

        public BaseRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _dbSet = _dbContext.Set<T>();
        }

        public virtual async Task<T> Create(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            try
            {
                var createdEntity = await _dbSet.AddAsync(entity);
                await _dbContext.SaveChangesAsync();
                return createdEntity.Entity;
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Error creating entity in database", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected error creating entity", ex);
            }
        }

        public virtual async Task<T?> GetById(K id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            try
            {
                return await _dbSet.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving entity", ex);
            }
        }

        public virtual async Task<IEnumerable<T>> GetAll()
        {
            try
            {
                return await _dbSet.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving entities", ex);
            }
        }

        public virtual async Task<T> Update(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            try
            {
                _dbSet.Update(entity);
                await _dbContext.SaveChangesAsync();
                return entity;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new InvalidOperationException("Entity was modified by another process", ex);
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Error updating entity in database", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected error updating entity", ex);
            }
        }

        public virtual async Task<T> Delete(K id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            try
            {
                var entity = await _dbSet.FindAsync(id);
                
                if (entity == null)
                    throw new KeyNotFoundException($"Entity with id {id} not found");

                _dbSet.Remove(entity);
                await _dbContext.SaveChangesAsync();
                return entity;
            }
            catch (KeyNotFoundException)
            {
                throw; // Re-throw KeyNotFoundException as-is
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Error deleting entity from database", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected error deleting entity", ex);
            }
        }

        public virtual async Task<bool> Exists(K id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            try
            {
                var entity = await _dbSet.FindAsync(id);
                return entity != null;
            }
            catch (Exception ex)
            {
                throw new Exception("Error checking entity existence", ex);
            }
        }
    }