using BLL.DAL;
using BLL.Models;
using BLL.Services.Bases;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace BLL.Services
{

    public interface IUserService
    {
        public IQueryable<UserModel> Query();
        public ServiceBase Create(User record);
        public ServiceBase Update(User record);
        public ServiceBase Delete(int id);


    }
    public class UserService : ServiceBase, IUserService
    {
        public UserService(Db db) : base(db)
        {
        }

        public IQueryable<UserModel> Query()
        {
            return _db.Users.Include(u => u.Role).OrderByDescending(u => u.IsActive).ThenBy(u => u.UserName).Select(u => new UserModel() { Record = u });
        }

        public ServiceBase Create(User user)
        {
            if (_db.Users.Any(u => u.UserName.ToUpper() == user.UserName.ToUpper().Trim()))
                return Error("User already exists");
            user.UserName = user.UserName.Trim();
            user.Password = user.Password.Trim();
            user.IsActive = user.IsActive;
            user.RoleId = user.RoleId;
            _db.Users.Add(user);
            _db.SaveChanges();
            return Success("User created successfully.");
        }

        public ServiceBase Update(User user)
        {
            if (_db.Users.Any(u => u.Id != user.Id && u.UserName.ToUpper() == user.UserName.ToUpper().Trim()))
                return Error("User already exists");

            var entity = _db.Users.SingleOrDefault(u => u.Id == user.Id);
            if (entity is null)
                return Error("User not found");

            entity.UserName = user.UserName.Trim();
            entity.Password = user.Password.Trim();
            entity.IsActive = user.IsActive;
            entity.RoleId = user.RoleId;
            _db.Users.Update(entity);
            _db.SaveChanges();
            return Success("User updated successfully.");
        }

        public ServiceBase Delete(int id)
        {
            var entity = _db.Users.SingleOrDefault(u => u.Id == id);
            _db.Users.Remove(entity);
            _db.SaveChanges();
            return Success($"User with \"{entity.UserName}\" deleted successfully.");
        }
    }
}