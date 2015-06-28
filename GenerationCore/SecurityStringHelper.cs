using System.Runtime.InteropServices;
using System.Security;

namespace GenerationCore
{
    public static class SecurityStringHelper
    {
        public static byte[] ToByteArray(this SecureString str)
        {
            var unmanagedBytes = Marshal.SecureStringToGlobalAllocUnicode(str);
            byte[] result;
            try
            {
                unsafe
                {
                    var byteArray = (byte*)unmanagedBytes.ToPointer();

                    // Find the end of the string
                    var pEnd = byteArray;
                    char symbol;
                    do
                    {
                        var firstByte = *pEnd++;
                        var secondByte = *pEnd++;
                        symbol = (char)(firstByte << 8);
                        symbol += (char)secondByte;
                    } while (symbol != '\0');

                    // Length is effectively the difference here (note we're 2 past end) 
                    var length = (int)((pEnd - byteArray) - 2);
                    result = new byte[length];
                    for (var i = 0; i < length; ++i)
                    {
                        // Work with data in byte array as necessary, via pointers, here
                        result[i] = *(byteArray + i);
                    }
                }
            }
            finally
            {
                // This will completely remove the data from memory
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedBytes);
            }

            return result;
        }
    }
}
