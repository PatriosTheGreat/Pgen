using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace GenerationCore
{
    public static class SymbolsTypeExtention
    {
        public static string GetSymbols(this SymbolsType type)
        {
            var resultSymbols = string.Empty;

            foreach (var typeFlag in type.GetFlags())
            {
                switch (typeFlag)
                {
                    case SymbolsType.LowcaseLatin:
                        resultSymbols += LowcaseLatinSymbols;
                        break;
                    case SymbolsType.UpcaseLatin:
                        resultSymbols += UpcaseLatinSymbols;
                        break;
                    case SymbolsType.Digital:
                        resultSymbols += NumberSymbols;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(type), type, null);
                }
            }

            Contract.Assume(resultSymbols != null);
            return resultSymbols;
        }

        public static long Count(this SymbolsType type)
        {
            return Enumerable.Range(0, 64).Sum(number => ((long)type >> number) & 1);
        }

        public static IEnumerable<SymbolsType> GetFlags(this SymbolsType input)
        {
            return AllTypes().Where(type => input.HasFlag(type));
        }

        private static SymbolsType[] AllTypes()
        {
            return Enum.GetValues(typeof (SymbolsType)).Cast<SymbolsType>().ToArray();
        }

        private const string LowcaseLatinSymbols = "abcdefghijklmnopqrstuvwxyz";
        private const string UpcaseLatinSymbols = "ABCDEFGHIJKLMNOPQRSTUUWXYZ";
        private const string NumberSymbols = "1234567890";
    }
}
