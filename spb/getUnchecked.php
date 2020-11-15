<?php
/// ПОЛУЧИТЬ НЕВЫПОЛНЕННЫЕ ЖАЛОБЫ ИЗ СОРТИРОВАННЫХ ТАБЛИЦ
$config = require_once 'config.php';
$host = $config['host']; // адрес сервера 
$database = $config['db_name']; // имя базы данных
$user = $config['username']; // имя пользователя
$password = $config['password']; // пароль
$mysqli = mysqli_connect("localhost", "gast", "gfgf2567", "db");
$query ="SELECT `id`, `type`, `latitude`, `longitude` FROM `sourcetable` where isDone = 0";
$result = $mysqli->query($query);
if($result->num_rows==0)
{
    echo 0;
}
else{
    while($res=mysqli_fetch_array($result,MYSQLI_ASSOC)){
		$data[] = $res;
	};
	echo json_encode($data);
}
?>