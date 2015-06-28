using System.Security;

namespace PgenWindowsClient
{
    public interface IPasswordProvider
    {
        SecureString Password { get; }

        void Clear();
    }
}
