<?php
/// УДАЛИТЬ ЗАПИСЬ ПО ID
sleep(1);
$config = require_once 'config.php';
$host = $config['host']; // адрес сервера 
$database = $config['db_name']; // имя базы данных
$user = $config['username']; // имя пользователя
$password = $config['password']; // пароль
$mysqli = mysqli_connect("localhost", "gast", "gfgf2567", "db");
$query ="SELECT * FROM `temp`";
$res=mysqli_query($mysqli,$query);
$getArr = mysqli_fetch_array($res,MYSQLI_ASSOC);
$idNow = $getArr['idNow'];
$updateTemp = "UPDATE `temp` SET idNow=$idNow+1";
$updateQuery = "UPDATE `sourcetable` SET isRead=0,isDone=0 WHERE id=$idNow";
$mysqli->query($updateQuery);
$mysqli->query($updateTemp);
?>