﻿#nullable enable
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace FugleNET.Keyrings
{
    public class CryptFileKeyring : FileKeyring
    {
        protected override string Encrypt(string password, byte[]? assoc = null)
        {
            var salt = CreateSalt();
            var cipher = new ArgonAESEncryption().CreateCipher(KeyringKey, salt);
            var passwordBytes = Encoding.UTF8.GetBytes(password);
            var mac = new byte[AesGcm.TagByteSizes.MaxSize];
            var nonce = new byte[AesGcm.NonceByteSizes.MaxSize];

            var cipherText = new byte[passwordBytes.Length];
            cipher.Encrypt(nonce, passwordBytes, cipherText, mac, assoc);

            var data = new Dictionary<string, string>()
            {
                {"Salt", Convert.ToBase64String(salt)},
                {"Data", Convert.ToBase64String(cipherText)},
                {"Mac", Convert.ToBase64String(mac)},
                {"Nonce", Convert.ToBase64String(nonce)}
            }.ToJson();

            return Convert.ToBase64String(Encoding.UTF8.GetBytes(data));
        }

        protected override string Decrypt(string passwordEncrypted, byte[]? assoc = null)
        {
            var data = Encoding.UTF8.GetString(Convert.FromBase64String(passwordEncrypted)).FromJson<Dictionary<string, object>>();
            foreach (var (key, value) in data!)
            {
                data[key] = Convert.FromBase64String(value.ToString()!);
            }

            var salt = (byte[])data["Salt"];
            var nonce = (byte[])data["Nonce"];
            var mac = (byte[])data["Mac"];
            var cipherText = (byte[])data["Data"];
            var cipher = new ArgonAESEncryption().CreateCipher(KeyringKey, salt);
            var plainText = new byte[cipherText.Length];

            cipher.Decrypt(nonce, cipherText, mac, plainText, assoc);
            return Encoding.UTF8.GetString(plainText);
        }

        private byte[] CreateSalt()
        {
            var bytes = new byte[16];
            RandomNumberGenerator.Fill(bytes);
            return bytes;
        }
    }
}
