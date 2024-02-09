using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Domain.Entites.Idetity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Persistence.Services
{
    public class RoleService : IRoleService
    {
        readonly RoleManager<AppRole> _roleManager;

        public RoleService(RoleManager<AppRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<bool> CreateRole(string name)
        {
          IdentityResult result=  await _roleManager.CreateAsync(new() {Id=Guid.NewGuid().ToString(),Name=name});
            return result.Succeeded;
        }

        public async Task<bool> DeleteRole(string id)
        {
            AppRole appRole = await _roleManager.FindByIdAsync(id);
            IdentityResult result=await _roleManager.DeleteAsync(appRole);
            return result.Succeeded;
        }

        public (object, int) GetAllRoles(int page, int size)
        {
            var query = _roleManager.Roles;
            IQueryable<AppRole> QuerybleRole = null;
            if (page != -1 || size != -1)
            {
                QuerybleRole = query.Skip(page * size).Take(size);
            }
            else
            {
                QuerybleRole = query;
            }
            return (QuerybleRole.Select(c => new { c.Id, c.Name }), query.Count());
        }

        public async Task<(string id, string name)> GetRoleById(string id)
        {
           AppRole appRole=await _roleManager.FindByIdAsync(id);
            return (appRole.Id, appRole.Name);
        }

        public async Task<bool> UpdateRole(string id, string name)
        {
            AppRole appRole = await _roleManager.FindByIdAsync(id);
            if(appRole!=null)
            {
                appRole.Name = name;
                IdentityResult result = await _roleManager.UpdateAsync(appRole);
                return result.Succeeded;
            }
            else
                return false;
        }
    }
}
