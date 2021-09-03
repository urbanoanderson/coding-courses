using Contracts;
using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Repository
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected RepositoryContext RepositoryContext { get; set; }

        public RepositoryBase(RepositoryContext repositoryContext)
        {
            this.RepositoryContext = repositoryContext;
        }

        public IQueryable<T> FindAll(bool trackChanges)
        {
            return !trackChanges ? this.RepositoryContext.Set<T>().AsNoTracking() : this.RepositoryContext.Set<T>();
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges)
        {
            return !trackChanges ? this.RepositoryContext.Set<T>().Where(expression).AsNoTracking()
                : this.RepositoryContext.Set<T>().Where(expression);
        }

        public void Create(T entity) => this.RepositoryContext.Set<T>().Add(entity);

        public void Update(T entity) => this.RepositoryContext.Set<T>().Update(entity);

        public void Delete(T entity) => this.RepositoryContext.Set<T>().Remove(entity);
    }
}
