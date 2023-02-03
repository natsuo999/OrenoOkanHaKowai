<?php
require_once('DBconnect.php');
$pdo = connectDB();

//POSTうけとり
$my_score = $_POST["score"];

if(isset($my_score)){

    try {
             
             // 自分のスコア値以上のデータ行を取得
             $stmt = $pdo->prepare("SELECT COUNT(*) AS RANK FROM `user_profile` WHERE score >= :Score");
             $stmt->bindValue(':Score',$my_score, PDO::PARAM_INT);
             $stmt->execute();
             $row = $stmt->fetch();
            //  foreach ($rows as $row)
            //  {
            //     $cnt = $row['RANK'];
            //  }
            $cnt = $row['RANK'];
             // 10件以上取得した場合はランク外                             
             if($cnt >= 10)
             {
                  // ランク外の場合は-1を返す
                  // ランキング最下位(50位)の点数と自分のスコアが同値の場合は登録順で51位になるのでランク外
                  $res = -1;  
             }
             else
             {
                  // 50件未満の場合はランクイン
                  // 取得したデータ行+1が自分のランク ※同値の場合でも登録順とする
                  $res = $cnt + 1;
             }
             
             
    } catch (PDOException $e) {
        var_dump($e->getMessage());
    }
    
    //DB切断
    $pdo = null;

    // 取得したランキングを返す
    echo $res;
    exit;
}
else
{
    // パラメータが受け取れなかった場合
    $res = 0;
    echo $res;
    exit;
}


?>