using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

public static class StringExtensions {
    // -------------------------------------------------------------------------------------
    // Constants
    // -------------------------------------------------------------------------------------
    private static readonly Random RANDOM = new Random((int)DateTime.UtcNow.Ticks);
    private static readonly byte[] IV = new byte[] { 6, 69, 167, 70, 92, 82, 31, 127 };
    private static readonly char[] CHARS = new[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' };

    // -------------------------------------------------------------------------------------
    // Functions
    // -------------------------------------------------------------------------------------
    public static string Decrypt(this string input, string key) {
        byte[] toDecryptArray = Convert.FromBase64String(input);
        byte[] keyArray = UTF8Encoding.UTF8.GetBytes(key);

        using (var crypto = new TripleDESCryptoServiceProvider {
            IV = IV,
            Key = keyArray,
            Mode = CipherMode.ECB,
            Padding = PaddingMode.PKCS7,
        }) {
            ICryptoTransform cTransform = crypto.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toDecryptArray, 0, toDecryptArray.Length);

            return UTF8Encoding.UTF8.GetString(resultArray);
        }
    }
    public static string Encrypt(this string input, out string key) {
        byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(input);

        key = GenerateKey();
        byte[] keyArray = UTF8Encoding.UTF8.GetBytes(key);

        using (var crypto = new TripleDESCryptoServiceProvider {
            IV = IV,
            Key = keyArray,
            Mode = CipherMode.ECB,
            Padding = PaddingMode.PKCS7,
        }) {
            ICryptoTransform cTransform = crypto.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
    }
    public static string FormatWith(this string input, params object[] args) {
        return string.Format(input, args);
    }
    public static string GetHashed(this string input) {
        UnicodeEncoding UE = new UnicodeEncoding();
        byte[] hashValue, message = UE.GetBytes(input);

        MD5 hashString = new MD5CryptoServiceProvider();
        string hex = "";

        hashValue = hashString.ComputeHash(message);
        foreach (byte x in hashValue) {
            hex += String.Format("{0:x2}", x);
        }

        return hex;
    }
    public static T ToDeserialized<T>(this string input) {
        return JsonSerializer.DeserializeFromString<T>(input);
    }
    public static T ToEnum<T>(this string input, T defaultValue) where T : struct {
        if (typeof(T).IsEnum) {
            T result;
            if (Enum.TryParse<T>(input, out result)) {
                return result;
            }

            return defaultValue;
        }

        throw new NotSupportedException("T must be an Enum");
    }
    public static string ToShortened(this string input, int length) {
        if (string.IsNullOrWhiteSpace(input)) {
            return input;
        }

        var clean = input.Trim();

        if (clean.Length <= length) {
            return clean;
        }

        return clean.Substring(0, length - 3) + "...";
    }

    // -------------------------------------------------------------------------------------
    // Private Helpers
    // -------------------------------------------------------------------------------------
    private static string GenerateKey() {
        string result = "";

        for (int i = 0; i < 24; i++) {
            result += CHARS[RandomNumber(0, 51)];
        }

        return result;
    }
    private static int RandomNumber(int minValue = 0, int maxValue = int.MaxValue) {
        return RANDOM.Next(minValue, maxValue);
    }

}
