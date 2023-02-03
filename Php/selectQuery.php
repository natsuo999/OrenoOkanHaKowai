<?php

require_once('phpAdmin_connect.php');
$pdo = connectDB();

//POSTうけとり
$id = $_POST["user_name"]; //要求されてくるid

try {
    //今回ここではSELECT文を送信している。UPDATE、DELETEなどは、また少し記法が異なる。
    $stmt = $pdo->query("SELECT * FROM `user_profile` WHERE `user_name` = '". $id. "'");
    foreach ($stmt as $row) {
    //今回はただカラムを指定し、出力された文字列を結合して出力
        $res = $row['user_id'];
        $res = $res. $row['user_name'];
        $res = $res. $row['score'];
    }

} catch (PDOException $e) {
    var_dump($e->getMessage());
}
$pdo = null;    //DB切断

echo $res;  //unity に結果を返す

?>