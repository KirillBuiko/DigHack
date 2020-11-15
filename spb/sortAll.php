<?php
/// ПОЛУЧИТЬ НЕВЫПОЛНЕННЫЕ ЖАЛОБЫ ИЗ СОРТИРОВАННЫХ ТАБЛИЦ
$CLUSTER_SIZE = 100;
$config = require_once 'config.php';
$host = $config['host']; // адрес сервера 
$database = $config['db_name']; // имя базы данных
$user = $config['username']; // имя пользователя
$password = $config['password']; // пароль
$mysqli = mysqli_connect("localhost", "gast", "gfgf2567", "db");
$query ="SELECT * FROM `sourcetable` where isRead = 0";
$query1 ="SELECT * FROM `SOURCETABLE` ORDER BY id DESC LIMIT 1 where isRead = 1";
$result = $mysqli->query($query);
$result1 = $mysqli->query($query1);
$last = NULL;
if(false !== $result1)
    $last = (mysqli_fetch_array($result1,MYSQLI_ASSOC));
else $last = NULL;
$lastdat = 0;
if(false !== $result1) if($result1->num_rows==1){
    $lastdat = (mysqli_fetch_array($result1,MYSQLI_ASSOC))['date'];
}
$myArray = array();
if($result->num_rows==0)
{
    echo 0;
}
else{
    echo $result->num_rows;
    while($res=mysqli_fetch_array($result,MYSQLI_ASSOC)){
        if($res["id"]>=100)exit(0);
        if(!(false !== $last)) $last = $res;
        $queryString = "";
        $dat = $res["date"];
        $dateComps=date_parse($dat);
        $day = $dateComps['day'];
        $year = $dateComps['year'];
        $month = $dateComps['month'];
        $hour = $dateComps['hour'];
        $season_map= array(
            '1'=>'winter',
            '2'=>'spring',
            '3'=>'summer',
            '4'=>'autumn'
        );

      
        $typ=$res["type"];

      
        if($month >= 12 || $month <= 2) $season = 1;
        else if($month >=3 && $month <=5) $season = 2;
        else if($month >=6 && $month <=8) $season = 3;
        else if($month >=9 && $month <=11) $season = 4;

        $lastseason = 0;
        $lastdat = $last["date"];
        $lastDayComps = date_parse($lastdat);
        $lastmonth = $lastDayComps['month'];
        $lastday = $lastDayComps['day'];
        $lastyear = $lastDayComps['year'];

        $tempVals = $mysqli->query("SELECT * FROM `temp` WHERE `id` = 1");
        $tempVals = mysqli_fetch_array($tempVals,MYSQLI_ASSOC);
        $nDayNow = $tempVals["dayNow"];
        $nMounthNow = $tempVals["monthNow"];
        $nSeasonNow = $tempVals["seasonNow"];
        $nYearNow = $tempVals["yearNow"];

        if($lastmonth >= 12 || $lastmonth <= 2) $lastseason = 1;
        else if($lastmonth >=3 && $lastmonth <=5) $lastseason = 2;
        else if($lastmonth >=6 && $lastmonth <=8) $lastseason = 3;
        else if($lastmonth >=9 && $lastmonth <=11) $lastseason = 4;

        $table_name = 'n_'.$season_map[$season].'_'.$typ;
        $lastInSeason = mysqli_fetch_array($mysqli->query("SELECT * FROM `$table_name` ORDER BY id DESC LIMIT 1"),MYSQLI_ASSOC);
        $last_table_name = 'n_'.$season_map[$lastseason].'_'.$typ;
        $toselect="SELECT * FROM `$last_table_name` where id = ".$last["id"];
        //exit($toselect);
        $getMiniTable = NULL;
        $getMiniTable = $mysqli->query($toselect);
            $nNormDay = 0;
            $nNormMounth = 0;
            $nNormSeason = 0;
            $nNormYear = 0;
        $ifchanged = array 
        (
             false,
             false,
             false,
             false
        );
        if($lastyear != $year){
            $ifchanged = array 
            (
                 true,
                 true,
                 true,
                 true
            );
        } else if($lastmonth != $month){
            $ifchanged = array 
            (
                 true,
                 true,
                 false,
                 false
            );
        }
            else if($lastday != $day){
               
                $ifchanged = array 
                (
                     true,
                     false,
                     false,
                     false
                );
            }
            if($lastseason != $season){
                $ifchanged[2] = 1;
            }
        
        $clearQuery = "UPDATE `temp` SET ";
        if($ifchanged[0]){
            $nNormDay=($nDayNow+$nNormDay)/2;
            $mysqli->query("UPDATE `temp` SET `dayNow` = 0 WHERE `id` = 1");
            $nMonthNow = 0;

        }
        if($ifchanged[1]){
            $nNormMounth=($nNormMounth+$nMounthNow)/2;
            $mysqli->query("UPDATE `temp` SET `monthNow` = 1");
            $nDayNow = 0;
        }
        if($ifchanged[2]){
            $nNormSeason=($nNormSeason+$nMounthNow)/2;
            $mysqli->query("UPDATE `temp` SET `seasonNow` = 1");
            $nSeasonNow = 0;
        }
        if($ifchanged[3]){
            $nNormYear=($nNormYear+$nYearNow)/2;
            $mysqli->query("UPDATE `temp` SET `yearNow` = 1");
            $nYearNow = 0;
        }
        $mysqli->query("UPDATE `$last_table_name` SET `nDayNorm` = $nNormDay, `nMonhNorm` = $nNormMounth, `nSeasonNorm`=$nNormSeason, `nYearNorm` = $nNormYear, `monthNow` = `monthNow` + 1, `seasonNow` = `seasonNow` + 1, `yearNow` = `yearNow` + 1 WHERE `id` = 1");
        $mysqli->query("UPDATE `temp` SET `dayNow` = `dayNow` + 1, `monthNow` = `monthNow` + 1, `seasonNow` = `seasonNow` + 1, `yearNow` = `yearNow` + 1 WHERE `id` = 1");
        //$table_name = 'n_'.$season_map[$season].'_'.$typ;
        $toInsert = "INSERT INTO `$table_name` (`id`, `nDayNow`, `nMonthNow`, `nSeasonNow`, `nYearNow`, `nDayNorm`, `nMonhNorm`, `nSeasonNorm`, `nYearNorm`, `Month`) VALUES (".$res["id"].",$nDayNow,$nMounthNow,$nSeasonNow,$nYearNow,$nNormDay,$nNormMounth,$nNormSeason,$nNormYear,'$dat')";
        echo "$toInsert <br>";
        //exit($toInsert);
        $mysqli->query($toInsert);
        $last = $res;
    }
}
?>

/*
/// ПОЛУЧИТЬ НЕВЫПОЛНЕННЫЕ ЖАЛОБЫ ИЗ СОРТИРОВАННЫХ ТАБЛИЦ
$CLUSTER_SIZE = 100;
$config = require_once 'config.php';
$host = $config['host']; // адрес сервера 
$database = $config['db_name']; // имя базы данных
$user = $config['username']; // имя пользователя
$password = $config['password']; // пароль
$mysqli = mysqli_connect("localhost", "gast", "gfgf2567", "db");
$query ="SELECT * FROM `sourcetable` where isRead = 0";
$query1 ="SELECT * FROM `SOURCETABLE` ORDER BY id DESC LIMIT 1";
$result = $mysqli->query($query);
$result1 = $mysqli->query($query1);
$last =  (mysqli_fetch_array($result1,MYSQLI_ASSOC));
$lastdat = 0;
if($result1->num_rows==1){
    $lastdat = (mysqli_fetch_array($result1,MYSQLI_ASSOC))['date'];
}
$myArray = array();
if($result->num_rows==0)
{
    echo 0;
}
else{
    while($res=mysqli_fetch_array($result,MYSQLI_ASSOC)){
        if($res["id"]>=100)exit(0);
        if($last == NULL) $last = $res;
        $queryString = "";
        $dat = $res["date"];
        $typ=$res["type"];
        $dateComps=date_parse($dat);
        $day = $dateComps['day'];
        $year = $dateComps['year'];
        $month = $dateComps['month'];
        $hour = $dateComps['hour'];
        
        $season_map= array(
            '1'=>'winter',
            '2'=>'spring',
            '3'=>'summer',
            '4'=>'autumn'
        );

        if($month >= 12 || $month <= 2) $season = 1;
        else if($month >=3 && $month <=5) $season = 2;
        else if($month >=6 && $month <=8) $season = 3;
        else if($month >=9 && $month <=11) $season = 4;

        $lastseason = 0;
        $lastdat = $last["date"];
        $lastDayComps = date_parse($lastdat);
        $lastmonth = $lastDayComps['month'];
        $lastday = $lastDayComps['day'];
        $lastyear = $lastDayComps['year'];
        $lasttype = $last["type"];

        $tempVals = $mysqli->query("SELECT * FROM `temp` WHERE `id` = 1");
        $tempVals = mysqli_fetch_array($tempVals,MYSQLI_ASSOC);
        $nDayNow = $tempVals["dayNow"];
        $nMounthNow = $tempVals["monthNow"];
        $nSeasonNow = $tempVals["seasonNow"];
        $nYearNow = $tempVals["yearNow"];
        
        if($lastmonth >= 12 || $lastmonth <= 2) $lastseason = 1;
        else if($lastmonth >=3 && $lastmonth <=5) $lastseason = 2;
        else if($lastmonth >=6 && $lastmonth <=8) $lastseason = 3;
        else if($lastmonth >=9 && $lastmonth <=11) $lastseason = 4;

        $table_name = 'n_'.$season_map[$season].'_'.$typ;//передаётся в ''
        $lastInSeason = mysqli_fetch_array($mysqli->query("SELECT * FROM `$table_name` ORDER BY id DESC LIMIT 1"),MYSQLI_ASSOC);
        $last_table_name = 'n_'.$season_map[$lastseason].'_'.$lasttype;
        $toselect="SELECT * FROM `$last_table_name` where id = ".$last["id"];
        $getMiniTable = $mysqli->query($toselect);
        if($getMiniTable == NULL){
            $nNormDay = 0;
            $nNormMounth = 0;
            $nNormSeason = 0;
            $nNormYear = 0;
        }
        else{
            $getMiniTable = mysqli_fetch_array($getMiniTable,MYSQLI_ASSOC);
            $nNormDay = $getMiniTable['nDayNow'];
            $nNormMounth = $getMiniTable['nMonthNow'];
            $nNormSeason = $lastInSeason['nSeasonNow'];
            $nNormYear = $getMiniTable['nYearNow'];
        }
        
        $ifchanged = array 
        (
             false,
             false,
             false,
             false
        );
        if($lastyear != $year){
            $ifchanged = array 
            (
                 true,
                 true,
                 true,
                 true
            );
        } else if($lastmonth != $month){
            $ifchanged = array 
            (
                 true,
                 true,
                 false,
                 false
            );
        }
            else if($lastday != $day){
                $ifchanged = array 
                (
                     true,
                     false,
                     false,
                     false
                );
            }
            if($lastseason != $season){
                $ifchanged[2] = 1;
            }
        
        if($ifchanged[0]){
            $nNormMounth=($nNormMounth+$nMounthNow)/2;
            $mysqli->query("UPDATE `temp` SET `dayNow` = 0 WHERE `id` = 1");
            //exit($nNormMounth);
            $nMonthNow = 0;
        }
        if($ifchanged[1]){
            $nNormDay=($nDayNow+$nNormDay)/2;
            $mysqli->query("UPDATE `temp` SET `monthNow` = 1");
            $nDayNow = 0;
        }
        if($ifchanged[2]){
            $nNormSeason=($nNormSeason+$nMounthNow)/2;
            $mysqli->query("UPDATE `temp` SET `seasonNow` = 1");
            $nSeasonNow = 0;
        }
        if($ifchanged[3]){
            $nNormYear=($nNormYear+$nYearNow)/2;
            $mysqli->query("UPDATE `temp` SET `yearNow` = 1");
            $nYearNow = 0;
        }

        //$mysqli->query("UPDATE `$last_table_name` SET `nDayNorm` = $nNormDay, `nMonhNorm` = $nNormMounth, `nSeasonNorm`=$nNormSeason, `nYearNorm` = $nNormYear, `monthNow` = `monthNow` + 1, `seasonNow` = `seasonNow` + 1, `yearNow` = `yearNow` + 1 WHERE `id` = 1");
        $mysqli->query("UPDATE `temp` SET `dayNow` = `dayNow` + 1, `monthNow` = `monthNow` + 1, `seasonNow` = `seasonNow` + 1, `yearNow` = `yearNow` + 1 WHERE `id` = 1");
        //$table_name = 'n_'.$season_map[$season].'_'.$typ;
        $toInsert = "INSERT INTO `$table_name` (`id`, `nDayNow`, `nMonthNow`, `nSeasonNow`, `nYearNow`, `nDayNorm`, `nMonhNorm`, `nSeasonNorm`, `nYearNorm`, `Month`) VALUES (".$res["id"].",$nDayNow,$nMounthNow,$nSeasonNow,$nYearNow,$nNormDay,$nNormMounth,$nNormSeason,$nNormYear,'$dat')";
        exit($toInsert);
        $mysqli->query($toInsert);
        $last = $res;
    }
}
? */