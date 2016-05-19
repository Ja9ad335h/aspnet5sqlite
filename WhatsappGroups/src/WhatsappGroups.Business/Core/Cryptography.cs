using WhatsappGroups.Business.Models;
using WhatsappGroups.Data.Contexts;
using WhatsappGroups.Data.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WhatsappGroups.Business.Core
{
    public class Cryptography
    {
        public enum HashType { MD5, SHA512, SHA256, SHA384, SHA1 }

        public static string ComputeHash(string text, HashType hashType = HashType.SHA512, byte[] saltBytes = null)
        {
            switch (hashType)
            {
                case HashType.MD5:
                    return ComputeHash(text, MD5.Create(), saltBytes);
                case HashType.SHA1:
                    return ComputeHash(text, SHA1.Create(), saltBytes);
                case HashType.SHA256:
                    return ComputeHash(text, SHA256.Create(), saltBytes);
                case HashType.SHA384:
                    return ComputeHash(text, SHA384.Create(), saltBytes);
                case HashType.SHA512:
                    return ComputeHash(text, SHA512.Create(), saltBytes);
                default: return null; // unreachable
            }
        }

        private static string ComputeHash(string text, HashAlgorithm algorithm, byte[] saltBytes = null)
        {
            if (saltBytes == null)
            {
                saltBytes = new byte[new Random().Next(4, 8)];
                RandomNumberGenerator.Create().GetBytes(saltBytes);
            }
            byte[] textBytes = Encoding.UTF8.GetBytes(text);
            byte[] textWithSaltBytes = new byte[textBytes.Length + saltBytes.Length];

            for (int i = 0; i < textBytes.Length; i++)
                textWithSaltBytes[i] = textBytes[i];
            for (int i = 0; i < saltBytes.Length; i++)
                textWithSaltBytes[textBytes.Length + i] = saltBytes[i];

            byte[] hashBytes = algorithm.ComputeHash(textWithSaltBytes);
            byte[] hashWithSaltBytes = new byte[hashBytes.Length + saltBytes.Length];

            for (int i = 0; i < hashBytes.Length; i++)
                hashWithSaltBytes[i] = hashBytes[i];
            for (int i = 0; i < saltBytes.Length; i++)
                hashWithSaltBytes[hashBytes.Length + i] = saltBytes[i];

            return Convert.ToBase64String(hashWithSaltBytes);
        }

        public static bool VerifyHash(string text, string hashText, HashType hashType = HashType.SHA512)
        {
            byte[] hashWithSaltBytes = Convert.FromBase64String(hashText);
            int hashSizeInBytes = 0;
            switch (hashType)
            {
                case HashType.MD5:
                    hashSizeInBytes = 128 / 8;
                    break;
                case HashType.SHA1:
                    hashSizeInBytes = 160 / 8;
                    break;
                case HashType.SHA256:
                    hashSizeInBytes = 256 / 8;
                    break;
                case HashType.SHA384:
                    hashSizeInBytes = 384 / 8;
                    break;
                case HashType.SHA512:
                    hashSizeInBytes = 512 / 8;
                    break;
            }

            if (hashWithSaltBytes.Length < hashSizeInBytes)
                return false;

            byte[] saltBytes = new byte[hashWithSaltBytes.Length - hashSizeInBytes];

            for (int i = 0; i < saltBytes.Length; i++)
                saltBytes[i] = hashWithSaltBytes[hashSizeInBytes + i];

            return hashText == ComputeHash(text, hashType, saltBytes);
        }
        public static RSAParameters GetRSAParameters()
        {
            var dataKey = GetRSAPrivateKey();
            return new RSAParameters()
            {
                D = dataKey.D,
                DP = dataKey.DP,
                DQ = dataKey.DQ,
                Exponent = dataKey.Exponent,
                InverseQ = dataKey.InverseQ,
                Modulus = dataKey.Modulus,
                P = dataKey.P,
                Q = dataKey.Q
            };
        }

        public static RsaSecurityKey GetRSASecurityKey()
        {
            return new RsaSecurityKey(GetRSAParameters());
        }

        public static RSAPrivateKey GetRSAPrivateKey(bool newKey = false)
        {
            using (var db = new WhatsappGroupsAdminContext())
            {
                if (newKey)
                {
                    RSAParameters tempKey = new RSAParameters();
                    using (var rsa = new RSACryptoServiceProvider(2048))
                    {
                        try
                        {
                            tempKey = rsa.ExportParameters(true);
                        }
                        finally
                        {
                            rsa.PersistKeyInCsp = false;
                        }
                    }
                    db.RSAPrivateKeys.Add(new RSAPrivateKey() { D = tempKey.D, DP = tempKey.DP, DQ = tempKey.DQ, Exponent = tempKey.Exponent, InverseQ = tempKey.InverseQ, Modulus = tempKey.Modulus, P = tempKey.P, Q = tempKey.Q, CreateDate = DateTime.Now });
                    db.SaveChanges();
                }
                RSAPrivateKey dataKey = db.RSAPrivateKeys.OrderByDescending(k => k.CreateDate).FirstOrDefault();
                if (dataKey == null) return GetRSAPrivateKey(true);
                else return dataKey;
            }
        }
    }
}
