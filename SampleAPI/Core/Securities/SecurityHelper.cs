﻿using System.Security.Cryptography;
using System.Text;

namespace Core.Securities;

public static class SecurityHelper
{
    public static string GetSha256Hash(string input)
    {
        //using (var sha256 = new SHA256CryptoServiceProvider())
        var byteValue = Encoding.UTF8.GetBytes(input);
        var byteHash = SHA256.HashData(byteValue);
        return Convert.ToBase64String(byteHash);
        //return BitConverter.ToString(byteHash).Replace("-", "").ToLower();
    }

    public static string ToSha256Hash(this string input)
    {
        //using (var sha256 = new SHA256CryptoServiceProvider())
        var byteValue = Encoding.UTF8.GetBytes(input);
        var byteHash = SHA256.HashData(byteValue);
        return Convert.ToBase64String(byteHash);
        //return BitConverter.ToString(byteHash).Replace("-", "").ToLower();
    }
}