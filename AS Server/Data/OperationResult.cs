using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace AdministrationStation.Server.Data
{
    public class OperationResult
    {
        public IEnumerable<OperationError> Errors { get; private set; }
        public bool Succeeded { get; private set; }

        private OperationResult()
        {
        }

        public static OperationResult Success => new OperationResult
        {
            Errors = new OperationError[0],
            Succeeded = true
        };
        
        public static OperationResult Error(OperationError error) => new OperationResult
        {
            Errors = new[] {error},
            Succeeded = false
        };

        public void AddError(OperationError error)
        {
            Succeeded = false;
            Errors = Errors.Append(error);
        }
    }

    public class OperationError
    {
        public static OperationError NoChangeOrOlder =>
            new OperationError(1, "Configuration hasn't changed or is older.");

        public static OperationError AgentNotFound => new OperationError(2, "The agent was not found.");

        private OperationError(int code, string description)
        {
            Code = code;
            Description = description;
        }

        public int Code { get; }
        public string Description { get; }
    }
}