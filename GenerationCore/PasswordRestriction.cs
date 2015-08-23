using System.Diagnostics.Contracts;
using System.Runtime.Serialization;

namespace GenerationCore
{
    [DataContract]
    public sealed class PasswordRestriction
    {
        public PasswordRestriction(
           SymbolsType acceptedTypes,
           int passwordMinLength = PasswordMinBounder,
           int passwordMaxLength = PasswordMaxBounder)
        {
            Contract.Assert(passwordMinLength > 0);
            Contract.Assert(passwordMaxLength > passwordMinLength);
            Contract.Assert(acceptedTypes.Count() < passwordMaxLength);

            AcceptedTypes = acceptedTypes;
            PasswordMaxLength = passwordMaxLength;
            PasswordMinLength = passwordMinLength;
        }
        
        [DataMember]
        public int PasswordMinLength { get; private set; }

        [DataMember]
        public int PasswordMaxLength { get; private set; }

        [DataMember]
        public SymbolsType AcceptedTypes { get; private set; }

        public const int PasswordMinBounder = 3;
        public const int PasswordMaxBounder = 25;
    }
}
