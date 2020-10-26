using System.Threading;
using System.Threading.Tasks;
using AdministrationStation.Server.Identity.Data;
using Microsoft.AspNetCore.Identity;

namespace AdministrationStation.Server.Identity
{
    public class AgentManager
    {
        public Task<IdentityResult> CreateAsync(Agent user, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<IdentityResult> DeleteAsync(Agent user, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<Agent> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<Agent> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<string> GetUserIdAsync(Agent user, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<string> GetUserNameAsync(Agent user, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<IdentityResult> UpdateAsync(Agent user, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}