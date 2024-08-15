<?php
    require("dbconnect.php");

    function showRespond($status, $message, $id =null, $data = null) {
        $response = array("status" => $status, "message" => $message);
        if ($id !== null && $data !== null) {
            $response['id'] = $id;
            $response['data'] = $data;
        }
        echo json_encode($response);
        exit();
    }

    if($_SERVER['REQUEST_METHOD'] == 'POST' && isset($_POST['username'], $_POST['password'])){
        $username = mysqli_real_escape_string($connect, $_POST['username']);
        $password = mysqli_real_escape_string($connect, $_POST['password']);

        $query = "SELECT id, password FROM users WHERE username='$username'";
        $result = mysqli_query($connect, $query);

        if(mysqli_num_rows($result) > 0){
            $user = mysqli_fetch_assoc($result);
            $userId = $user['id'];
            
            if(password_verify($password, $user['password'])){
                $query = "INSERT INTO login_history (user_id, login_time) VALUES ($userId, NOW())";
                mysqli_query($connect, $query);

                $query = "SELECT diamonds, hearts FROM user_data WHERE user_id=$userId";
                $dataResult = mysqli_query($connect, $query);

                if(mysqli_num_rows($dataResult) > 0){
                    $data = mysqli_fetch_assoc($dataResult);
                    $data['hearts'] = ($data['hearts'] > 10) ? 10 : $data['hearts'];
                    $data['diamonds'] = ($data['diamonds'] > 9999999) ? 9999999 : $data['diamonds'];
                    showRespond("success", "Login successful", $userId ,$data);
                }else{
                    showRespond("error", "User data not found");
                }

            }else{
                showRespond("error", "Incorrect password ");
            }

        }else{
            showRespond("error", "Username does not exist");
        }
        
    }else{
        showRespond("error", "Invalid request or missing fields");
    }
    mysqli_close($connect);

?>