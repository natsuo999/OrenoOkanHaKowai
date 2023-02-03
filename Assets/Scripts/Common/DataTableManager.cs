using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;

/// <summary>
/// �f�[�^�e�[�u���������N���X
/// </summary>
public static class DataTableManager
{

    public static DataTable LoadTextToDataTable(TextAsset inputText, string tableName)
    {
        // �e�L�X�g�f�[�^�����s���Ƃɋ�؂��Ĕz��Ɋi�[�@�󔒂�����
        string[] lines = inputText.text.Split(new char[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);
        int columns = 0;
        int rows = lines.Length;
        DataTable outputTable = new DataTable(tableName);

        for (int y=0; y< rows; y++)
        {
            // �J���}���Ƃɋ�؂��Ċi�[
            string[] values = lines[y].Split(new[] { ',' });
            DataRow dr = outputTable.NewRow();

            // 1�s�ڂ̓w�b�_�[(�J������)
            if (y == 0)
            {
                columns = values.Length;
                for (int x = 0; x < columns; x++)
                {
                    outputTable.Columns.Add(values[x]);
                }

                continue;
            }
          
            // 2�s�ڈȍ~�A�f�[�^�s��ǉ�
            for (int x=0; x< columns; x++)
            {
                dr[outputTable.Columns[x]] = values[x];
               
            }

            outputTable.Rows.Add(dr);

        }

        return outputTable;
    }

    /// <summary>
    /// �f�[�^�e�[�u������P��s�̒l���擾����
    /// </summary>
    /// <param name="table">�f�[�^�e�[�u��</param>
    /// <param name="whereClumnsName">WHERE��Ɏw�肷��J������</param>
    /// <param name="value">WHERE��Ɏw�肷��l</param>
    /// <param name="selectColumnsNamae">SELECT��Ɏw�肷��J������</param>
    /// <returns>�擾���ʂ̒l(������)</returns>
    public static string SelectStrDataFromDatatable(DataTable table, string whereClumnsName, int value, string selectColumnsNamae)
    {
        // LINQ���g���ăf�[�^�𒊏o
        var str = table.AsEnumerable()
                          .Where (row => row.Field<string>(whereClumnsName) == value.ToString())
                          .Select(row => row.Field<string>(selectColumnsNamae))
                          .FirstOrDefault().ToString();

        

        return str;
    }
}
