using System.Linq;
using GenerationCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoreTests
{
    [TestClass]
    public class ServicePasswordGenerationTests
    {
        [TestInitialize]
        public void InitializeTest()
        {
            _passwordGenerator.UserPassword = "qwerty";
        }

        [TestMethod]
        public void GeneratedPasswordShouldNotBeEmpty()
        {
            var serviceInfo = new ServiceInformation(
                "testService",
                new PasswordRestriction(SymbolsType.LowcaseLatin));

            var password = _passwordGenerator.GeneratePassword(serviceInfo);

            Assert.IsTrue(password.Length > 0);
        }

        [TestMethod]
        public void SecondaryGeneratedPasswordShouldBeSame()
        {
            var serviceInfo = new ServiceInformation(
                "testService",
                new PasswordRestriction(SymbolsType.LowcaseLatin));

            var password = _passwordGenerator.GeneratePassword(serviceInfo);
            var secondaryGenerated = _passwordGenerator.GeneratePassword(serviceInfo);

            Assert.AreEqual(password, secondaryGenerated);
        }

        [TestMethod]
        public void ResultPasswordWithOtherUserPasswordShouldNotBeSame()
        {
            var serviceInfo = new ServiceInformation(
                "testService",
                new PasswordRestriction(SymbolsType.LowcaseLatin));

            var password = _passwordGenerator.GeneratePassword(serviceInfo);

            _passwordGenerator.UserPassword = "otherPassword";
            var otherPassword = _passwordGenerator.GeneratePassword(serviceInfo);

            Assert.AreNotEqual(password, otherPassword);
        }

        [TestMethod]
        public void PasswordForOtherServiceShouldNotBeSame()
        {
            var firstService = new ServiceInformation(
                "testService",
                new PasswordRestriction(SymbolsType.LowcaseLatin));

            var password = _passwordGenerator.GeneratePassword(firstService);

            var secondService = new ServiceInformation(
                "testService",
                new PasswordRestriction(SymbolsType.LowcaseLatin));

            var otherPassword = _passwordGenerator.GeneratePassword(secondService);

            Assert.AreNotEqual(password, otherPassword);
        }

        [TestMethod]
        public void PasswordShouldContainsDifferentSymbols()
        {
            var serviceWithLotSymbolTypes = new ServiceInformation(
                "testService",
                new PasswordRestriction(
                    SymbolsType.LowcaseLatin |
                    SymbolsType.UpcaseLatin |
                    SymbolsType.Digital));

            var password = _passwordGenerator.GeneratePassword(serviceWithLotSymbolTypes);

            Assert.IsTrue(
                SymbolsType.LowcaseLatin.GetSymbols().Any(symbol => password.Contains(symbol)));
            Assert.IsTrue(
                SymbolsType.UpcaseLatin.GetSymbols().Any(symbol => password.Contains(symbol)));
            Assert.IsTrue(
                SymbolsType.Digital.GetSymbols().Any(symbol => password.Contains(symbol)));
        }

        private readonly ServicePasswordGenerator _passwordGenerator = new ServicePasswordGenerator();
    }
}
