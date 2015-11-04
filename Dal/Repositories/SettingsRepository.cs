using Dal.DbEntities;
using Dal.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dal.Models;
using System.Linq.Expressions;
using System.Data.Entity;

namespace Dal.Repositories
{
    public class SettingsRepository : BaseRepository<Settings>, ISettingsRepository
    {
        protected override DbSet<Settings> GetTable()
        {
            return context.Settings;
        }

        public IQueryable<BaseEntity> GetFiltered(IList<Expression<Func<BaseEntity, bool>>> filters)
        {
            throw new NotImplementedException();
        }

        public List<BaseEntity> GetPage(IList<Expression<Func<BaseEntity, bool>>> filters, PagingParameters<BaseEntity> pagingParams)
        {
            throw new NotImplementedException();
        }

        public BaseEntity Save(BaseEntity entity)
        {
            throw new NotImplementedException();
        }

        IQueryable<BaseEntity> IBaseRepository<BaseEntity>.GetAll()
        {
            throw new NotImplementedException();
        }

        BaseEntity IBaseRepository<BaseEntity>.GetById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
