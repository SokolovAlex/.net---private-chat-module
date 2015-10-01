using AutoMapper;
using Core.Models;
using Core.Models.User;
using Dal.DbEntities;
using Ssibir.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal.Repositories.IRepositories
{
    public class UserRepository: BaseRepository<User>, IUserRepository
    {
        public UserRepository() {
            Mapper.CreateMap<User, UserModel>();
        }

        protected override DbSet<User> GetTable() {
            return context.Users;
        }

        public UserModel GetByHash(string hashId)
        {
            return Mapper.Map<UserModel>(
                GetTable().FirstOrDefault(x=>x.HashId == hashId));
        }

        public void Save(UserModel model)
        {
            if (model.IsNew()) {
                context.Users.Add(Mapper.Map<User>(model));
            } else {
                var dbModel = GetById(model.Id);
                dbModel.Name = model.Name;
                dbModel.PasswordHash = model.Salt;
                dbModel.RoleId = model.RoleId;
                dbModel.Salt = model.Salt;
            }
        }
    }
}
