<?php

//PDO MySQL接続
function connectDB(){

//ユーザ名やDBアドレスの定義
    $dsn = 'mysql:host=mysql144.phy.lolipop.lan;dbname=LAA1139738-szk100;charset=utf8';
    $username = 'LAA1139738';
    $password = 'Abcd19880922';

    try {
        $pdo = new PDO($dsn, $username, $password);
    } catch (PDOException $e) {
        exit('' . $e->getMessage());
    }

    return $pdo;
}

?>