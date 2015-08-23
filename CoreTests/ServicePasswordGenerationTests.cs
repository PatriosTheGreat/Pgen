using System.Linq;
using System.Security;
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
            _userPassword = new SecureString();
            InitializeSecurityString("qwerty");
        }

        [TestMethod]
        public void GeneratedPasswordShouldNotBeEmpty()
        {
            var serviceInfo = new ServiceInformation(
                "testService",
                new PasswordRestriction(SymbolsType.LowcaseLatin));

            var password =
                ServicePasswordGenerator.GeneratePassword(serviceInfo, _userPassword);

            Assert.IsTrue(password.Length > 0);
        }

        [TestMethod]
        public void SecondaryGeneratedPasswordShouldBeSame()
        {
            var serviceInfo = new ServiceInformation(
                "testService",
                new PasswordRestriction(SymbolsType.LowcaseLatin));

            var password =
                ServicePasswordGenerator.GeneratePassword(serviceInfo, _userPassword);
            var secondaryGenerated =
                ServicePasswordGenerator.GeneratePassword(serviceInfo, _userPassword);

            Assert.AreEqual(password, secondaryGenerated);
        }

        [TestMethod]
        public void ResultPasswordWithOtherUserPasswordShouldNotBeSame()
        {
            var serviceInfo = new ServiceInformation(
                "testService",
                new PasswordRestriction(SymbolsType.LowcaseLatin));

            var password = 
                ServicePasswordGenerator.GeneratePassword(serviceInfo, _userPassword);

            InitializeSecurityString("otherPassword");
            var otherPassword = 
                ServicePasswordGenerator.GeneratePassword(serviceInfo, _userPassword);

            Assert.AreNotEqual(password, otherPassword);
        }

        [TestMethod]
        public void PasswordForOtherServiceShouldNotBeSame()
        {
            var firstService = new ServiceInformation(
                "testService",
                new PasswordRestriction(SymbolsType.LowcaseLatin));

            var password = ServicePasswordGenerator.GeneratePassword(
                firstService,
                _userPassword);

            var secondService = new ServiceInformation(
                "testService",
                new PasswordRestriction(SymbolsType.LowcaseLatin));

            var otherPassword = ServicePasswordGenerator.GeneratePassword(
                secondService,
                _userPassword);
            
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

            var password = ServicePasswordGenerator.GeneratePassword(
                serviceWithLotSymbolTypes,
                _userPassword);

            Assert.IsTrue(
                SymbolsType.LowcaseLatin.GetSymbols().Any(symbol => password.Contains(symbol)));
            Assert.IsTrue(
                SymbolsType.UpcaseLatin.GetSymbols().Any(symbol => password.Contains(symbol)));
            Assert.IsTrue(
                SymbolsType.Digital.GetSymbols().Any(symbol => password.Contains(symbol)));
        }

        [TestMethod]
        public void LongPasswordShouldBeSuccessfulGenerated()
        {

            var serviceWithLotSymbolTypes = new ServiceInformation(
                "testService",
                new PasswordRestriction(
                    SymbolsType.LowcaseLatin |
                    SymbolsType.UpcaseLatin |
                    SymbolsType.Digital,
                    passwordMinLength: 2000,
                    passwordMaxLength: 3000));

            var password = ServicePasswordGenerator.GeneratePassword(
                serviceWithLotSymbolTypes,
                _userPassword);
            
            Assert.IsTrue(password.Length > 2000);
            Assert.IsTrue(password.Length <= 3000);
        }

        private static void InitializeSecurityString(string password)
        {
            foreach (var symbol in password)
            {
                _userPassword.AppendChar(symbol);
            }
        }

        private static SecureString _userPassword;
    }
}
