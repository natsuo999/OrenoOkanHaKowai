using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ManipulateArrayParts
{
    /// <summary>
    /// ”z—ñ‚Ì’†g‚ğƒ‰ƒ“ƒ_ƒ€‚É•À‚Ñ•Ï‚¦
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    /// <returns></returns>
    public static T[] ShuffleArrayContents<T>(this T[] array)
    {
        var length = array.Length;
        var result = new T[length];

        try
        {
            Array.Copy(array, result, length);
            var random = new System.Random();
            int n = length;
            while (1 < n)
            {
                n--;
                int k = random.Next(n + 1);
                var tmp = result[k];
                result[k] = result[n];
                result[n] = tmp;
            }

        }
        catch(Exception)
        {
            throw;
        }

        return result;
    }
}
