<?php
/// ПОЛУЧИТЬ ПРЕДУПРЕЖДЕНИЯ
// AVE AVE AVTF
sleep(1);
$host = 'localhost'; // адрес сервера
$database = 'db'; // имя базы данных
$user = 'gast'; // имя пользователя
$password = 'gfgf2567'; // пароль
$mysqli = new mysqli($host, $user, $password, $database);
$query ="SELECT * FROM users where login = \"".$_POST['login']."\" and password = \"".$_POST['password']."\"";
$mysqli->set_charset("utf8");
$result = $mysqli->query($query);
$myArray = array();
if($result->num_rows==0)
{
    echo 0;
}
else{
	echo "[";
	while($row = $result->fetch_array(MYSQLI_ASSOC)) {
            $myArray[] = $row;
			echo "{\"id\":\"".$row['id']."\",\"name\":\"".$row['name']."\",\"age\":\"".$row['age']."\",\"sex\":\"".$row['sex']."\",\"position\":\"".$row['position']."\",\"experience\":\"".$row['experience']."\",\"password\":\"".$row['password']."\",\"login\":\"".$row['login']."\",\"mail\":\"".$row['mail']."\",\"perm_level\":\"".$row['perm_level']."\"}";
    }
    echo "]";
}
?>