<?php

$dbName = "2624038_dimitar";
$dbHost = "pdb23.awardspace.net";
$dbPass = "Xavimaestro66*";
$db = "2624038_dimitar";

$secretKey = "123456789";

function dbConnect(){

    global $dbName;
    global $secretKey;

    $link = new mysqli("pdb23.awardspace.net", "2624038_dimitar", "Xavimaestro66*", "2624038_dimitar")
     or die("Connection failed. %s\n" . $link -> error);

     return $link; 
}

function safe($var){
    $var = addslashes(trim($var));

    return $var;
}

function fail($errMsg){
    print $errMsg;

    exit;
}

function CloseConnection($link){
    $link -> close();
}

?>