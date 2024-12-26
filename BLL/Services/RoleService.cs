using BLL.DAL;
using BLL.Models;
using BLL.Services.Bases;
using Microsoft.EntityFrameworkCore;


namespace BLL.Services
{
    public interface IRoleService
    {
        public IQueryable<RoleModel> Query();
        public ServiceBase Create(Role record);
        public ServiceBase Update(Role record);
        public ServiceBase Delete(int id);


    }
    public class RoleService : ServiceBase , IRoleService
    {
        public RoleService(Db db) : base(db)
        {
        }

        public IQueryable<RoleModel> Query()
        {
            return _db.Roles.Select(r => new RoleModel() { Record = r });
        }

        public ServiceBase Create(Role role)
        {
            if (_db.Roles.Any(r => r.Name.ToLower() == role.Name.ToUpper().Trim()))
                return Error("Role with the same name exists!");
            role.Name = role.Name.Trim();
            _db.Roles.Add(role);
            _db.SaveChanges();
            return Success("Role created successfully.");
        }

        public ServiceBase Update(Role role)
        {
            if (_db.Roles.Any(r => r.Id != role.Id && r.Name.ToUpper() == role.Name.ToUpper().Trim()))
                return Error("Role with the same name exists!");
            var entity = _db.Roles.SingleOrDefault(r => r.Id == role.Id);
            entity.Name = role.Name.Trim();
            _db.Roles.Update(entity);
            _db.SaveChanges();
            return Success("Role updated successfully.");
        }

        public ServiceBase Delete(int id)
        {
            var entity = _db.Roles.Include(r => r.Users).SingleOrDefault(r => r.Id == id);
            if (entity.Users.Any())
                return Error("Role has relationship with users!");
            _db.Roles.Remove(entity);
            _db.SaveChanges();
            return Success("Role deleted successfully.");
        }
    }
}
