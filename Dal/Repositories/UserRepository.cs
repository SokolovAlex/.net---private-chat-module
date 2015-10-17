using AutoMapper;
using Core.Enums;
using Core.Models;
using Core.Models.User;
using Dal.DbEntities;
using Dal.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal.Repositories.Repositories
{
    public partial class UserRepository: BaseRepository<User>, IUserRepository
    {
        public UserRepository() {
            Mapper.CreateMap<User, UserModel>().
                ForMember(dest => dest.Password,
               opts => opts.MapFrom(src => src.PasswordHash));
            Mapper.CreateMap<UserModel, User>()
                .ForMember(dest => dest.PasswordHash,
               opts => opts.MapFrom(src => src.Password));
        }

        protected override DbSet<User> GetTable() {
            return context.Users;
        }

        public UserModel GetByHash(Guid hashId)
        {
            return Mapper.Map<UserModel>(
                GetTable().FirstOrDefault(x=>x.HashId == hashId));
        }

        public UserModel GetByEmail(string mail)
        {
            return Mapper.Map<UserModel>(
                GetTable().FirstOrDefault(x => Equals(x.Email, mail)));
        }

        public void Save(UserModel model)
        {
            model.RoleId = model.RoleId != 0 ? model.RoleId : (int)UserRole.Client;

            if (model.IsNew()) {
                model.HashId = Guid.NewGuid();
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
