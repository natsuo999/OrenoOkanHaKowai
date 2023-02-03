using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public static class GenerateHashValue
{
    /// <summary>
    /// �n�b�V���l�̌v�Z(SHA256)
    /// </summary>
    /// <param name="currentTime"></param>
    /// <returns></returns>
    public static string GenerateSHA256HashValue(string inputText)
    {
        try
        {
            byte[] inputByteArray = Encoding.UTF8.GetBytes(inputText);
            SHA256CryptoServiceProvider hm = new SHA256CryptoServiceProvider();
            byte[] hashValue = hm.ComputeHash(inputByteArray);

            // �o�C�g�z���16�i��������ɕϊ�
            StringBuilder sb = new StringBuilder();
            foreach (byte b in hashValue)
            {
                sb.Append(b.ToString("x2"));
            }

            return sb.ToString();

        }
        catch(Exception)
        {
            throw;
        }

    }

}
