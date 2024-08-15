<?php
    require("dbconnect.php");

    function showRespond($status, $message) {
        echo json_encode(array("status" => $status, "message" => $message));
        exit();
    }

    if($_SERVER['REQUEST_METHOD'] == 'POST' && isset($_POST['username'], $_POST['password'], $_POST['confirm_password'])){
        $username = mysqli_real_escape_string($connect, $_POST['username']);
        $password = mysqli_real_escape_string($connect, $_POST['password']);
        $confirm_password = mysqli_real_escape_string($connect, $_POST['confirm_password']);
        $diamonds = isset($_POST['diamonds']) ? intval($_POST['diamonds']) : 0;
        $hearts = isset($_POST['hearts']) ? intval($_POST['hearts']) : 0;

        if($password !== $confirm_password){
            showRespond("error", "Passwords do not match");
        }

        $query = "SELECT * FROM users WHERE username='$username'";
        $result = mysqli_query($connect, $query);

        if(mysqli_num_rows($result) > 0){
            showRespond("error", "Username already exists");
        }else{
            $hashed_password = password_hash($password, PASSWORD_BCRYPT);
            $query = "INSERT INTO users (username, password) VALUES ('$username', '$hashed_password')";
            $result = mysqli_query($connect, $query);

            if (!$result) {
                showRespond("error", "Error inserting user: " . mysqli_error($connect));
            }

            $user_id = mysqli_insert_id($connect);

            $query = "INSERT INTO user_data (user_id, diamonds, hearts) VALUES ($user_id, $diamonds, $hearts)";
            $result = mysqli_query($connect, $query);

            if(!$result){
                showRespond("error", "Error inserting user data: " . mysqli_error($connect));
            }

            showRespond("success", "User registered successfully");
        }
    }else{
        showRespond("error", "Invalid request or missing fields");
    }

    mysqli_close($connect);
    
?>