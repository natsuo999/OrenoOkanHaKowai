<?php
require_once('DBconnect.php');
$pdo = connectDB();

//$sql = 'SELECT Name, Score FROM score ORDER BY score DESC, date, id LIMIT 50';
// ƒ†[ƒU’è‹`•Ï”‚ªŽæ“¾‚Å‚«‚È‚¢
//$sql = 'SET @rownum=0; SELECT @rownum:=@rownum+1 as rank, `user_name`,`score` FROM `user_profile` ORDER BY `score` DESC LIMIT 10';
$sql = 'SELECT `user_name`,`score` FROM `user_profile` ORDER BY `score` DESC,`created_at` ASC LIMIT 10';

$rows = $pdo->query($sql)->fetchAll(PDO::FETCH_ASSOC);

echo json_encode($rows);

?>