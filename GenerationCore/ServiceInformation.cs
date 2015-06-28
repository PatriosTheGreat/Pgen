using System;
using System.Diagnostics.Contracts;
using System.Runtime.Serialization;

namespace GenerationCore
{
    [DataContract, KnownType(typeof(PasswordRestriction))]
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

        [DataMember]
        public string ServiceName { get; private set; }

        [DataMember]
        public string UniqueToken { get; private set; }

        [DataMember]
        public PasswordRestriction Restriction { get; private set; }
    }
}
