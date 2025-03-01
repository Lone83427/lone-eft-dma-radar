using System.Reflection;
using System.Security.Cryptography;

namespace eft_dma_shared.Common.Misc
{
    public static class FileCrypt
    {
        /// <summary>
        /// Opens an encrypted file from disk and loads it into memory.
        /// </summary>
        /// <param name="filePath">Path of the file to decrypt/open.</param>
        /// <returns>Readable MemoryStream of the decrypted file contents.</returns>
        public static MemoryStream OpenEncryptedFile(string filePath)
        {
            try
            {
                byte[] salt = new byte[12];
                byte[] nonce = new byte[12];
                byte[] tag = new byte[16];
                byte[] ct, pt;
                using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    ct = new byte[fs.Length - salt.Length - nonce.Length - tag.Length];
                    fs.ReadExactly(salt);
                    fs.ReadExactly(nonce);
                    fs.ReadExactly(tag);
                    fs.ReadExactly(ct);
                }
                pt = new byte[ct.Length];
                byte[] key;
                using (var h = new Rfc2898DeriveBytes(UnpackEncryptionKey(), salt, 600000, HashAlgorithmName.SHA512))
                {
                    key = h.GetBytes(32);
                }
                using (var aes = new AesGcm(key, tag.Length))
                {
                    aes.Decrypt(nonce, ct, tag, pt);
                }
                return new MemoryStream(pt, false);
            }
            catch
            {
                return null;
            }
        }

        private static byte[] UnpackEncryptionKey()
        {
            // Unpack AES Key from Resources
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("eft-dma-shared.FILE_CRYPT_KEY.bin"))
            {
                var key = new byte[stream!.Length];
                stream.ReadExactly(key);
                return key;
            }
        }
    }
}
