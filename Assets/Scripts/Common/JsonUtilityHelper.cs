using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Json�`���f�[�^�ϊ��N���X
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
    /// Json�`���̃f�[�^���I�u�W�F�N�g�z��ɕϊ����鏈��
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="responseText">Json�`���̓Ǎ��f�[�^</param>
    /// <returns></returns>
    public static T[] ConvertObjectArrayFromJson<T>(string responseText)
    {
        try
        {
            // JSON �z�񂩂�I�u�W�F�N�g�z��ւ̕ϊ�
            T[] dataArray = JsonUtility.FromJson<DataWrapper<T>>("{\"array\":" + responseText + "}").array;
            return dataArray;

        }
        catch (Exception)
        {
            throw;
        }


    }

    /// <summary>
    /// Json�`���̃f�[�^���I�u�W�F�N�g�ɕϊ����鏈��
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="responseText">Json�`���̓Ǎ��f�[�^</param>
    /// <returns></returns>
    public static T ConvertObjectFromJson<T>(string responseText)
    {
        try
        {
            // JSON �z�񂩂�I�u�W�F�N�g�ւ̕ϊ�
            T dataArray = JsonUtility.FromJson<T>(responseText);
            return dataArray;

        }
        catch (Exception)
        {

            throw;
        }


    }
}
