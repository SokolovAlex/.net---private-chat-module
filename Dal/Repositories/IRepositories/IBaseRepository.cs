using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using Dal.DbEntities;
using Dal.Models;

namespace Dal.Repositories.IRepositories
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        IQueryable<T> GetAll();
        T GetById(int id);
        IQueryable<T> GetFiltered(IList<Expression<Func<T, bool>>> filters);
        List<T> GetPage(IList<Expression<Func<T, bool>>> filters, PagingParameters<T> pagingParams);
        T Save(T entity);
        void Commit();
        bool Delete(int id);
    }
}
