using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly NZWalksDbContext nZWalksDbContext;

        public UserRepository(NZWalksDbContext nZWalksDbContext)
        {
            this.nZWalksDbContext = nZWalksDbContext;
        }


        public async Task<User> AuthenticateAsync(string username, string password)
        {
            var user = nZWalksDbContext.Users.FirstOrDefault(x => x.Username.ToLower() == username.ToLower() && x.Password == password);

            if (user != null)
            {
                var userRoles = await nZWalksDbContext.Users_Roles.Where( x => x.UserId == user.Id).ToListAsync();

                if (userRoles != null)
                {
                    user.Roles = new List<string>();
                    foreach (var ur in userRoles)
                    {
                        var role = await nZWalksDbContext.Roles.FirstOrDefaultAsync(x => x.Id == ur.RoleId);
                        if (role != null)
                        {
                            user.Roles.Add(role.Name);
                        }
                    }
                }
                user.Password = null;
            }

            return user;
        }
    }
}
