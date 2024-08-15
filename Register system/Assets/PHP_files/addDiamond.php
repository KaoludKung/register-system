<?php
    require("dbconnect.php");
    
    function showRespond($status, $message, $data = null) {
        $response = array("status" => $status, "message" => $message);
        if ($data !== null) {
            $response['data'] = $data;
        }
        echo json_encode($response);
        exit();
    }

    if($_SERVER['REQUEST_METHOD'] == 'POST' && isset($_POST['user_id'], $_POST['diamonds'])){
        $userId = intval($_POST['user_id']);
        $diamonds = intval($_POST['diamonds']);

        $query = "UPDATE user_data SET diamonds = diamonds + $diamonds WHERE user_id = $userId";

        if(mysqli_query($connect, $query)) {
            $query = "SELECT diamonds, hearts FROM user_data WHERE user_id = $userId";
            $result = mysqli_query($connect, $query);
        
            if (mysqli_num_rows($result) > 0) {
                $data = mysqli_fetch_assoc($result);
                $data['diamonds'] = ($data['diamonds'] > 9999999) ? 9999999 : $data['diamonds'];
                showRespond("success", "Diamonds added successfully", $data);
            } else {
                showRespond("error", "User data not found");
            }

        } else {
            showRespond("error", "Error updating diamonds: " . mysqli_error($connect));
        }
    }else{
        showRespond("error", "Invalid request or missing fields");
    }

    mysqli_close($connect);

?>