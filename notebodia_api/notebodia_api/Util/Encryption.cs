using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;

namespace notebodia_api.Util
{
    public static class EncryptionUtils
    {
        public static string GenerateSessionToken()
        {
            byte[] bytes = new byte[20];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(bytes);
            }

            // Convert to base32 and remove padding
            string token = Base32.ToBase32String(bytes).Replace("=", "");
            return token.ToLower();
        }
        public static Guid EncodeHexLowerCase(byte[] data)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] hash = sha256.ComputeHash(data);
                // Take only the first 16 bytes since a GUID is 16 bytes long
                byte[] guidBytes = hash.Take(16).ToArray();
                return new Guid(guidBytes);
            }
        }
    }
}