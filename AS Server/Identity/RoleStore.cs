using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AdministrationStation.Server.Data;
using AdministrationStation.Server.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AdministrationStation.Server.Identity
{
    //Todo: Implement mechanism to define client or agent only roles.
    public class RoleStore : IRoleStore<Role>, IQueryableRoleStore<Role>
    {
        public RoleStore(ServerContext context)
        {
            Context = context;
        }

        private ServerContext Context { get; }
        private DbSet<Role> Roles => Context.Roles;

        private IdentityErrorDescriber ErrorDescriber { get; } = new IdentityErrorDescriber();
        private const string RoleNameMayNotBeNullOrEmpty = "Role name may not be null or empty.";

        public void Dispose()
        {
        }

        private async Task SaveChanges(CancellationToken cancellationToken)
        {
            await Context.SaveChangesAsync(cancellationToken);
        }

        public async Task<IdentityResult> CreateAsync(Role role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            await Context.AddAsync(role, cancellationToken);
            await SaveChanges(cancellationToken);
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(Role role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            Context.Remove(role);
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

        public async Task<Role> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (int.TryParse(roleId, out var id))
            {
                throw new ArgumentException(RoleNameMayNotBeNullOrEmpty);
            }

            var role = await Roles.FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
            return role;
        }

        public async Task<Role> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (string.IsNullOrWhiteSpace(normalizedRoleName))
            {
                throw new ArgumentException(RoleNameMayNotBeNullOrEmpty);
            }

            var role = await Roles.FirstOrDefaultAsync(r => r.NormalizedName == normalizedRoleName, cancellationToken);
            return role;
        }

        public Task<string> GetNormalizedRoleNameAsync(Role role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            return Task.FromResult(role.NormalizedName);
        }

        public Task<string> GetRoleIdAsync(Role role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            return Task.FromResult(role.Id.ToString());
        }

        public Task<string> GetRoleNameAsync(Role role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            return Task.FromResult(role.Name);
        }

        public async Task SetNormalizedRoleNameAsync(Role role, string normalizedName,
            CancellationToken cancellationToken)
        {
            role.NormalizedName = normalizedName;
            await SaveChanges(cancellationToken);
        }

        public async Task SetRoleNameAsync(Role role, string roleName, CancellationToken cancellationToken)
        {
            role.Name = roleName;
            await SaveChanges(cancellationToken);
        }

        public async Task<IdentityResult> UpdateAsync(Role role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            Context.Update(role);

            await SaveChanges(cancellationToken);
            return IdentityResult.Success;
        }

        IQueryable<Role> IQueryableRoleStore<Role>.Roles => Roles;
    }
}