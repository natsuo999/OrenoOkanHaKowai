<?php
require_once('DBconnect.php');
$pdo = connectDB();

//POSTうけとり
$my_name = $_POST["user_name"];
$my_score = $_POST["score"];
$hash = $_POST["hashValue"];

if(isset($my_name)){

	try 
	{
			// ハッシュ値を算出し、比較する
			$mydigestb = hash('sha256' , $my_name.$my_score."club_y");
	        if($hash <> $mydigestb)
	        {
	             throw new Exception("データ不一致");//$mydigestb.'名前+スコアから計算したハッシュ値が異なります。');
	        }
            //if(file_exists('NameChecker.php'))
			//{
			//	require_once('NameChecker.php');
			//	$my_name = CheckName(ReplaceSpecialchars($my_name));
			//}
			try 
			{
	        	 //$sysday = date("Y-m-d H:i:s");
	        	 //$stmt = $pdo->prepare('INSERT INTO `user_profile` (user_name, score, Date) VALUES(:Name, :Score, :Date)');
                 $stmt = $pdo->prepare('INSERT INTO `user_profile` (user_name, score) VALUES(:Name, :Score)');
                 
         		 // 値をセット(名前とスコアはエスケープ処理後の値を使用する)
            	 $stmt->bindValue(':Name', ReplaceSpecialchars($my_name));
            	 $stmt->bindValue(':Score',ReplaceSpecialchars($my_score));
             	 //$stmt->bindValue(':Date', $sysday);
    
             	// SQL実行
            	 $stmt->execute();
             
			} catch (PDOException $e) {
	
    			var_dump($e->getMessage());
    	
			} finally {
	
			//DB切断
			$pdo = null;  
    
    		}
    	
    } catch (Exception $e) {
    	var_dump($e->getMessage());
    }
    
}
else
{
	// パラメータが受け取れなかった場合
	$res = "パラメータ設定エラー";
    echo $res;
	exit;
}

function ReplaceSpecialchars($inputChar){

	// htmlspecialchars( 変換対象文字, 変換パターン, 文字コード ) 
    return htmlspecialchars($inputChar, ENT_QUOTES, 'UTF-8');
    
}

?>