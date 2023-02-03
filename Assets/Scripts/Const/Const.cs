using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Const
{
    /// <summary>
    /// �V�[����
    /// </summary>
    public static class SceneName
    {
        /// <summary>�^�C�g��</summary>
        public const string TITLE_SCENE = "TitleScene";

        /// <summary>�Q�[��</summary>
        public const string GAME_SCENE = "GameScene";

        /// <summary>�����L���O</summary>
        public const string RANKING_SCENE = "RankingScene";
    }

    /// <summary>
    /// �}�X�^��
    /// </summary>
    public static class TableName
    {
        /// <summary>�X�e�[�W�p�����[�^�e�[�u��</summary>
        public const string STAGE_PARAM_TABLE = "StageParamTable";

        /// <summary>�X�e�[�W�e�[�u��</summary>
        public const string STAGE_TABLE = "StageTable";

        /// <summary>�G���[���b�Z�[�W�}�X�^</summary>
        public const string ERROR_MESSAGE_TABLE = "ErrorMessageTable";

    }

    /// <summary>
    /// �T�[�o�ڑ�����ID
    /// </summary>
    public static class ServerConnectProcessID
    {
        /// <summary>�����N�C���`�F�b�N����</summary>
        public const string CHECK_RANK_IN = "001";

        /// <summary>�X�R�A�f�[�^���M����</summary>
        public const string POST_SCORE_DATA = "002";
    }

    /// <summary>
    /// �T�[�o�ڑ��G���[ID
    /// </summary>
    public static class ServerConnectErrorID
    {
        /// <summary>�l�b�g���[�N�G���[</summary>
        public const int ERROR_NETWORK_REGIST_RANK = 1001;

        /// <summary>�������ԃI�[�o�[</summary>
        public const int ERROR_RESPONSE_TIME_OVER = 1002;

        /// <summary>�p�����[�^�G���[��������php���̃G���[</summary>
        public const int ERROR_EXCEPT_RESPONSE_TIME_OVER = 1003;

        /// <summary>�l�b�g���[�N�G���[</summary>
        public const int ERROR_NETWORK_GET_RANKLIST = 1004;

        /// <summary>�n�b�V���l�G���[</summary>
        public const int ERROR_HASH_GENERATE = 2001;

        /// <summary>JSON�`���ϊ��G���[</summary>
        public const int ERROR_JSON_CONVERT = 2002;

    }

    
}

