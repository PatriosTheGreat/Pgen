using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;

namespace GenerationCore
{
    public static class ServicePasswordGenerator
    {
        public static string GeneratePassword(
            ServiceInformation service,
            SecureString userPassword)
        {
            Contract.Assert(service != null);
            Contract.Assert(userPassword.Length > 0);

            var passwordBasedNumbers = GetPasswordBasedNumbers(service.UniqueToken, userPassword);

            var passwordLength = 
                CalculatePasswordLength(
                    restriction: service.Restriction, 
                    randomNumber: passwordBasedNumbers.First());

            var randomNumbers = GetRandomNumbersFromPasswordBasedNumbers(
                2 * passwordLength, 
                passwordBasedNumbers.Skip(1).ToArray());

            var mandatoryPartNumbersCount = randomNumbers.Length/2;
            var mandatoryPartNumbers = randomNumbers.Take(mandatoryPartNumbersCount).ToArray();
            
            var mandatoryPart = GenerateMandatoryPart(
                service.Restriction.AcceptedTypes,
                mandatoryPartNumbers);

            var restPartNumbers = randomNumbers.Skip(mandatoryPartNumbersCount).ToArray();
            var restPart = GenerateRestPart(
                service.Restriction.AcceptedTypes,
                restPartNumbers,
                passwordLength - mandatoryPart.Length);

            var resultPassword = mandatoryPart + restPart;

            Contract.Assume(resultPassword.Length > service.Restriction.PasswordMinLength);
            Contract.Assume(resultPassword.Length <= service.Restriction.PasswordMaxLength);

            return resultPassword;
        }
        
        private static string GenerateRestPart(
            SymbolsType acceptedTypes, 
            byte[] restPartNumbers, 
            long length)
        {
            var passwordPartBuilder = new StringBuilder();
            var acceptedSymbols = acceptedTypes.GetSymbols();
            for (var i = 0; i < length; i++)
            {
                var symbolNumber = restPartNumbers[i] % acceptedSymbols.Length;
                passwordPartBuilder.Append(acceptedSymbols[symbolNumber]);
            }

            return passwordPartBuilder.ToString();
        }

        private static string GenerateMandatoryPart(
            SymbolsType acceptedTypes, 
            byte[] passwordSymbolsNumbers)
        {
            var passwordPartBuilder = new StringBuilder();
            var allTypesArray = acceptedTypes.GetFlags().ToArray();
            for (var i = 0; acceptedTypes != 0; i++)
            {
                var randomTypeNumber = passwordSymbolsNumbers[2 * i];
                var randomSymbolNumber = passwordSymbolsNumbers[2 * i + 1];

                var usedType = allTypesArray[randomTypeNumber % allTypesArray.Length];
                var usedSymbols = usedType.GetSymbols();
                passwordPartBuilder.Append(usedSymbols[randomSymbolNumber % usedSymbols.Length]);

                acceptedTypes = TurnOffFlag(acceptedTypes, usedType);
            }

            return passwordPartBuilder.ToString();
        }

        private static SymbolsType TurnOffFlag(SymbolsType flags, SymbolsType flag) => flags & ~flag;

        private static long CalculatePasswordLength(
            PasswordRestriction restriction, 
            int randomNumber)
        {
            var lowBounder = Math.Max(restriction.PasswordMinLength, restriction.AcceptedTypes.Count());
            var upperBounder = restriction.PasswordMaxLength;
            var middleBounder = (upperBounder - lowBounder) / 2;

            return upperBounder - randomNumber % (upperBounder - middleBounder);
        }

        private static byte[] GetRandomNumbersFromPasswordBasedNumbers(
            long necessaryCount,
            byte[] passwordBasedNumbers)
        {
            var byteArrays = new List<byte[]> { passwordBasedNumbers };

            var shaHasher = SHA512.Create();
            while (byteArrays.Sum(array => array.Length) < necessaryCount)
            {
                byteArrays.Add(shaHasher.ComputeHash(byteArrays.Last()));
            }

            return byteArrays.SelectMany(array => array).ToArray();
        }

        private static byte[] GetPasswordBasedNumbers(
            string serviceToken, 
            SecureString userPassword)
        {
            var userPasswordBytes = userPassword.ToByteArray();
            var tokenBytes = Encoding.UTF8.GetBytes(serviceToken);
            
            return SHA512.Create().ComputeHash(userPasswordBytes.Concat(tokenBytes).ToArray());
        }
    }
}
