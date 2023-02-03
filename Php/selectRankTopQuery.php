<?php

//ユーザ名やDBアドレスの定義
    $dsn = 'mysql:host=mysql144.phy.lolipop.lan;dbname=LAA1139738-szk100;charset=utf8';
    $username = 'LAA1139738';
    $password = 'Abcd19880922';
    
//PDO MySQL接続
function connectDB(){

        return new PDO($dsn, $username, $password);
        
        $opt = array(
            PDO::ATTR_ERRMODE => PDO::ERRMODE_EXCEPTION,
            PDO::ATTR_DEFAULT_FETCH_MODE => PDO::FETCH_ASSOC
        );
        
        return new PDO($dsn, $username, $password, $opt);

}

//require_once('phpAdmin_connect.php');

$pdo = connectDB();

//POSTうけとり

$sql = "SET @rownum=0; SELECT @rownum:=@rownum+1 as RANK, `user_name`,`score` FROM `user_profile` ORDER BY `score` DESC LIMIT 10";


//今回ここではSELECT文を送信している。UPDATE、DELETEなどは、また少し記法が異なる。
$stmt = $pdo->query($sql);
         
    
    // SQLの実行結果を多次元連想配列で処理する
    $result = $stmt->fetchAll();
    foreach ($result as $key=>$val) {
      $k=0;
      while ($k <= ( $dbcolum - 1)) {
        print $val[$k] ;
        $k=$k+1;
        if ($k < $dbcolum){
            print ",";
        }
      }
      print "\n";
    
    }

?>



