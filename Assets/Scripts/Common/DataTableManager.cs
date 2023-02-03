using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;

/// <summary>
/// データテーブルを扱うクラス
/// </summary>
public static class DataTableManager
{

    public static DataTable LoadTextToDataTable(TextAsset inputText, string tableName)
    {
        // テキストデータを改行ごとに区切って配列に格納　空白を除去
        string[] lines = inputText.text.Split(new char[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);
        int columns = 0;
        int rows = lines.Length;
        DataTable outputTable = new DataTable(tableName);

        for (int y=0; y< rows; y++)
        {
            // カンマごとに区切って格納
            string[] values = lines[y].Split(new[] { ',' });
            DataRow dr = outputTable.NewRow();

            // 1行目はヘッダー(カラム数)
            if (y == 0)
            {
                columns = values.Length;
                for (int x = 0; x < columns; x++)
                {
                    outputTable.Columns.Add(values[x]);
                }

                continue;
            }
          
            // 2行目以降、データ行を追加
            for (int x=0; x< columns; x++)
            {
                dr[outputTable.Columns[x]] = values[x];
               
            }

            outputTable.Rows.Add(dr);

        }

        return outputTable;
    }

    /// <summary>
    /// データテーブルから単一行の値を取得する
    /// </summary>
    /// <param name="table">データテーブル</param>
    /// <param name="whereClumnsName">WHERE句に指定するカラム名</param>
    /// <param name="value">WHERE句に指定する値</param>
    /// <param name="selectColumnsNamae">SELECT句に指定するカラム名</param>
    /// <returns>取得結果の値(文字列)</returns>
    public static string SelectStrDataFromDatatable(DataTable table, string whereClumnsName, int value, string selectColumnsNamae)
    {
        // LINQを使ってデータを抽出
        var str = table.AsEnumerable()
                          .Where (row => row.Field<string>(whereClumnsName) == value.ToString())
                          .Select(row => row.Field<string>(selectColumnsNamae))
                          .FirstOrDefault().ToString();

        

        return str;
    }
}
