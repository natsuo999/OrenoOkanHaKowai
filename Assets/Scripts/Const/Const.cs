using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Const
{
    /// <summary>
    /// シーン名
    /// </summary>
    public static class SceneName
    {
        /// <summary>タイトル</summary>
        public const string TITLE_SCENE = "TitleScene";

        /// <summary>ゲーム</summary>
        public const string GAME_SCENE = "GameScene";

        /// <summary>ランキング</summary>
        public const string RANKING_SCENE = "RankingScene";
    }

    /// <summary>
    /// マスタ名
    /// </summary>
    public static class TableName
    {
        /// <summary>ステージパラメータテーブル</summary>
        public const string STAGE_PARAM_TABLE = "StageParamTable";

        /// <summary>ステージテーブル</summary>
        public const string STAGE_TABLE = "StageTable";

        /// <summary>エラーメッセージマスタ</summary>
        public const string ERROR_MESSAGE_TABLE = "ErrorMessageTable";

    }

    /// <summary>
    /// サーバ接続処理ID
    /// </summary>
    public static class ServerConnectProcessID
    {
        /// <summary>ランクインチェック処理</summary>
        public const string CHECK_RANK_IN = "001";

        /// <summary>スコアデータ送信処理</summary>
        public const string POST_SCORE_DATA = "002";
    }

    /// <summary>
    /// サーバ接続エラーID
    /// </summary>
    public static class ServerConnectErrorID
    {
        /// <summary>ネットワークエラー</summary>
        public const int ERROR_NETWORK_REGIST_RANK = 1001;

        /// <summary>応答時間オーバー</summary>
        public const int ERROR_RESPONSE_TIME_OVER = 1002;

        /// <summary>パラメータエラーもしくはphp側のエラー</summary>
        public const int ERROR_EXCEPT_RESPONSE_TIME_OVER = 1003;

        /// <summary>ネットワークエラー</summary>
        public const int ERROR_NETWORK_GET_RANKLIST = 1004;

        /// <summary>ハッシュ値エラー</summary>
        public const int ERROR_HASH_GENERATE = 2001;

        /// <summary>JSON形式変換エラー</summary>
        public const int ERROR_JSON_CONVERT = 2002;

    }

    
}

