﻿using System.Diagnostics.Contracts;
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
            Contract.Assert(passwordMinLength >= PasswordMinBounder);
            Contract.Assert(passwordMaxLength <= PasswordMaxBounder);
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

        private const int PasswordMinBounder = 3;
        private const int PasswordMaxBounder = 25;
    }
}
