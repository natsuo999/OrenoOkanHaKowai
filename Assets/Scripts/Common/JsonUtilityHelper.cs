using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Json形式データ変換クラス
/// </summary>
public static class JsonUtilityHelper
{
    [Serializable]
    public class DataWrapper<T>
    {
        public T[] array;
        public DataWrapper(T[] collection) => this.array = collection;
    }


    /// <summary>
    /// Json形式のデータをオブジェクト配列に変換する処理
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="responseText">Json形式の読込データ</param>
    /// <returns></returns>
    public static T[] ConvertObjectArrayFromJson<T>(string responseText)
    {
        try
        {
            // JSON 配列からオブジェクト配列への変換
            T[] dataArray = JsonUtility.FromJson<DataWrapper<T>>("{\"array\":" + responseText + "}").array;
            return dataArray;

        }
        catch (Exception)
        {
            throw;
        }


    }

    /// <summary>
    /// Json形式のデータをオブジェクトに変換する処理
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="responseText">Json形式の読込データ</param>
    /// <returns></returns>
    public static T ConvertObjectFromJson<T>(string responseText)
    {
        try
        {
            // JSON 配列からオブジェクトへの変換
            T dataArray = JsonUtility.FromJson<T>(responseText);
            return dataArray;

        }
        catch (Exception)
        {

            throw;
        }


    }
}
