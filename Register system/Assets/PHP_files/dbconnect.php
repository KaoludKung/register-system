<?php
   $hostname = "localhost";
   $username = "root";
   $password = "";
   $database = "user_database";

   $connect =  mysqli_connect($hostname,$username,$password,$database);
   
    if (!$connect) {
        die("Database connection failed: " . mysqli_connect_error());
    }

?>