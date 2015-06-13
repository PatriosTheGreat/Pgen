using System;
using System.Diagnostics.Contracts;

namespace GenerationCore
{
    public sealed class ServiceInformation
    {
        public ServiceInformation(
            string serviceName, 
            PasswordRestriction restriction)
        {
            Contract.Assert(!string.IsNullOrWhiteSpace(serviceName));
            Contract.Assert(restriction != null);

            Restriction = restriction;
            ServiceName = serviceName;
            UniqueToken = Guid.NewGuid().ToString();
        }

        public string ServiceName { get; private set; }

        public string UniqueToken { get; private set; }

        public PasswordRestriction Restriction { get; private set; }
    }
}
