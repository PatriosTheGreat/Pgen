using System.Diagnostics.Contracts;

namespace GenerationCore
{
    public sealed class PasswordRestriction
    {
        public PasswordRestriction(
           SymbolsType acceptedTypes,
           int passwordMinLength = PasswordMinBounder,
           int passwordMaxLength = PasswordMaxBounder)
        {
            Contract.Assert(passwordMinLength >= PasswordMinBounder);
            Contract.Assert(passwordMaxLength <= PasswordMaxBounder);
            Contract.Assert(passwordMaxLength > passwordMinLength);
            Contract.Assert(acceptedTypes.Count() < passwordMaxLength);

            AcceptedTypes = acceptedTypes;
            PasswordMaxLength = passwordMaxLength;
            PasswordMinLength = passwordMinLength;
        }
        
        public int PasswordMinLength { get; private set; }

        public int PasswordMaxLength { get; private set; }

        public SymbolsType AcceptedTypes { get; private set; }

        private const int PasswordMinBounder = 3;
        private const int PasswordMaxBounder = 25;
    }
}
