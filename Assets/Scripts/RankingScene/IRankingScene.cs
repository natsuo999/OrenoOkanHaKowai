using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public interface IRankingScene
{
    void Initialize(WebSererRequestClient db, UserData userData);

    void OnRetryButton();

    delegate void ErrorRankingScenenDelegate(int errorId, string errorDetail);
    ErrorRankingScenenDelegate ErrorActionMethod { get; set; }

    DataTable ErrorMessageData { get; set; }

    string GetErrorMessage(DataTable data, int errorId, string whereColumnName = "ErrorId", string selectColumnName = "ErrorMessage");


}
