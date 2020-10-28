using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AdministrationStation.Server.Data;
using AdministrationStation.Server.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AdministrationStation.Server.Identity
{
    public class UserStore : IUserRoleStore<User>, IUserPasswordStore<User>, IQueryableUserStore<User>
    {
        private ServerContext Context { get; }
        private DbSet<User> Users => Context.Users;
        private DbSet<Role> Roles => Context.Roles;
        private DbSet<UserRole> UserRoles => Context.UserRoles;

        private IdentityErrorDescriber ErrorDescriber { get; } = new IdentityErrorDescriber();

        public UserStore(ServerContext context)
        {
            Context = context;
        }

        private async Task SaveChanges(CancellationToken cancellationToken)
        {
            await Context.SaveChangesAsync(cancellationToken);
        }

        private static int GetIdFromString(string id)
        {
            if (int.TryParse(id, out var intId))
            {
                return intId;
            }

            throw new ArgumentException("User id is not a valid number.");
        }

        public void Dispose()
        {
        }

        public async Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            await Context.AddAsync(user, cancellationToken);
            await Context.SaveChangesAsync(cancellationToken);
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            Context.Remove(user);
            try
            {
                await SaveChanges(cancellationToken);
            }
            catch (DbUpdateConcurrencyException)
            {
                return IdentityResult.Failed(ErrorDescriber.ConcurrencyFailure());
            }

            return IdentityResult.Success;
        }

        public async Task<User> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var id = GetIdFromString(userId);
            return await Users
                .Include(u => u.Agent)
                .Include(u => u.Client)
                .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
        }

        public async Task<User> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var user = await Users
                .Include(u => u.Agent)
                .Include(u => u.Client)
                .FirstOrDefaultAsync(u => u.NormalizedUsername == normalizedUserName,
                    cancellationToken);
            return user;
        }

        public Task<string> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return Task.FromResult(user.NormalizedUsername);
        }

        public Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return Task.FromResult(user.Id.ToString());
        }

        public Task<string> GetUserNameAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return Task.FromResult(user.UserName);
        }

        public Task SetNormalizedUserNameAsync(User user, string normalizedName,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (string.IsNullOrWhiteSpace(normalizedName))
            {
                throw new ArgumentException("Username may not be null or empty.");
            }

            user.NormalizedUsername = normalizedName;
            return Task.CompletedTask;
        }

        public Task SetUserNameAsync(User user, string userName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new ArgumentException("Username may not be null or empty.");
            }

            user.UserName = userName;
            return Task.CompletedTask;
        }

        public async Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            Context.Attach(user);
            user.ConcurrencyStamp = Guid.NewGuid().ToString();
            Context.Update(user);
            try
            {
                await SaveChanges(cancellationToken);
            }
            catch (DbUpdateConcurrencyException)
            {
                return IdentityResult.Failed(ErrorDescriber.ConcurrencyFailure());
            }

            return IdentityResult.Success;
        }

        public async Task AddToRoleAsync(User user, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (string.IsNullOrWhiteSpace(roleName))
            {
                throw new ArgumentException("Role name may not be null or empty.");
            }

            var roleEntity = await FindRoleAsync(roleName, cancellationToken);
            if (roleEntity == null)
            {
                throw new InvalidOperationException($"Role {roleName} does not exist.");
            }

            await UserRoles.AddAsync(new UserRole(user, roleEntity), cancellationToken);
            await SaveChanges(cancellationToken);
        }

        public async Task<IList<string>> GetRolesAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var userId = user.Id;
            var query =
                from userRole in UserRoles
                join role in Roles on userRole.RoleId equals role.Id
                where userRole.UserId.Equals(userId)
                select role.Name;

            return await query.ToListAsync(cancellationToken);
        }

        public async Task<IList<User>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(roleName))
            {
                throw new ArgumentException("Role name may not be null or empty.");
            }

            var role = await FindRoleAsync(roleName, cancellationToken);
            var query =
                from userRole in UserRoles
                join user in Users on userRole.UserId equals user.Id
                where userRole.RoleId.Equals(role.Id)
                select user;
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<bool> IsInRoleAsync(User user, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (string.IsNullOrWhiteSpace(roleName))
            {
                throw new ArgumentException("Role name may not be null or empty.");
            }

            var role = await FindRoleAsync(roleName, cancellationToken);

            if (role == null) return false;

            var userRole = await FindUserRoleAsync(user.Id, role.Id, cancellationToken);

            return userRole != null;
        }

        /// <summary>
        /// Finds and returns the UserRole that corresponds to the give UserId and RoleId, if it exists.
        /// </summary>
        /// <param name="userId">The UserId of the UserRole.</param>
        /// <param name="roleId">The RoleId of the UserRole.</param>
        /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The UserRole corresponding to the UserId and RoleId.</returns>
        private async Task<UserRole> FindUserRoleAsync(int userId, int roleId, CancellationToken cancellationToken)
        {
            return await UserRoles.FirstOrDefaultAsync(
                userRole => userRole.UserId == userId && userRole.RoleId == roleId, cancellationToken);
        }

        /// <summary>
        /// Finds and returns the specified a role, if any, who has the specified name. 
        /// </summary>
        /// <param name="normalizedRoleName">The normalized name of the role.</param>
        /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The specified role or null, if no role by that name exists.</returns>
        /// <exception cref="ArgumentException">Gets thrown if the role name is <c>null</c> or an empty string.</exception>
        private async Task<Role> FindRoleAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (string.IsNullOrWhiteSpace(normalizedRoleName))
            {
                throw new ArgumentException("Role name may not be null or empty.");
            }

            return await Roles.FirstOrDefaultAsync(role => role.NormalizedName == normalizedRoleName,
                cancellationToken);
        }

        /// <summary>
        /// Removes the specified user from the the given named role.
        /// </summary>
        /// <param name="user">The user to add the role to.</param>
        /// <param name="roleName">The role to add to the user.</param>
        /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        public async Task RemoveFromRoleAsync(User user, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var role = await FindRoleAsync(roleName, cancellationToken);
            var userRole = await FindUserRoleAsync(user.Id, role.Id, cancellationToken);
            Context.Remove(userRole);
            await SaveChanges(cancellationToken);
        }

        public Task<string> GetPasswordHashAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.HashedPassword);
        }

        public Task<bool> HasPasswordAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(string.IsNullOrWhiteSpace(user.HashedPassword));
        }

        public Task SetPasswordHashAsync(User user, string passwordHash, CancellationToken cancellationToken)
        {
            user.HashedPassword = passwordHash;
            return Task.CompletedTask;
        }

        IQueryable<User> IQueryableUserStore<User>.Users => Users;
    }
}