<?php
	header("Access-Control-Allow-Origin: *");
	header('Access-Control-Allow-Methods: POST GET');
	require ("./dbconnect.php");
	date_default_timezone_set('Europe/Amsterdam');


	if (isset($_POST['Register']))
	{
		$json = $_POST['Register'];
		$json = json_decode($json, true);

		$name = $json['Name'];
		$pwd = $json['Password'];
		$nickname = $json['NickName'];
		$token = $json['Token'];

		$sql = "SELECT nickname FROM users WHERE nickname='$nickname'";
	 	$result = $conn->query($sql);


		if ($result !== false && $result -> num_rows > 0)
		{
				echo "NOK";
		}
		else
		{
			try {
				$sql = "insert INTO users (`name`, `password`, `nickname`, `archive`, token) VALUES ('$name', '$pwd', '$nickname', '0', '$token')";
				$result = $conn->query($sql);

				$user_id = $conn->insert_id;
				echo $user_id;
			} catch (\Exception $e) {
				echo $e;
			}
		}
	}

	if (isset($_POST['Login']))
	{
		$json = $_POST['Login'];
		$json = json_decode($json, true);

		$nickname = $json['NickName'];
		$pwd = $json['Password'];

		$sql = "SELECT id FROM users WHERE archive=0 AND nickname='$nickname' AND password='$pwd'";
		$result = $conn->query($sql);

		if ($result !== false && $result -> num_rows > 0)
		{
			if ($row = $result->fetch_assoc())
			{
				echo $row['id'];
			}
		}
		else
		{
			echo "NOK";
		}
	}

	if (isset($_POST['AllUsers']))
	{
		$id = $_POST['AllUsers'];

		$sql = "SELECT id, nickname FROM users WHERE archive=0 and id <> '$id'";
		$result = $conn->query($sql);

		if ($result !== false && $result -> num_rows > 0)
		{
			$arr=[];
			$inc=0;

			while ($row = $result->fetch_assoc())
			{
				$jsonArrayObject = (array('Id' => $row['id'],
                      						'NickName' => $row['nickname']
			 										 ));
				 $arr[$inc] = $jsonArrayObject;
				 $inc++;
			}
		 $json_array = json_encode($arr);
		 echo $json_array;
		}
	}


	if (isset($_POST['UserChat']))
	{
		$json = $_POST['UserChat'];
		$json = json_decode($json, true);

		$from_id = $json['From_Id'];
		$to_id = $json['To_Id'];

		$sql = "SELECT message, from_id, to_id FROM messages WHERE (from_id='$from_id' AND to_id='$to_id') OR (from_id='$to_id' AND to_id='$from_id')";
		$result = $conn->query($sql);

		if ($result !== false && $result -> num_rows > 0)
		{
			$arr=[];
			$inc=0;

			while ($row = $result->fetch_assoc())
			{
				$jsonArrayObject = (array('From_Id' => $row['from_id'],
																	'To_Id' => $row['to_id'],
																	'Message' => $row['message']
													 ));
				 $arr[$inc] = $jsonArrayObject;
				 $inc++;
			}
		 $json_array = json_encode($arr);
		 echo $json_array;
		}
		elseif ($result -> num_rows == 0)
		{
			echo "NMSG";
		}
		else {
			echo "NOK";
		}
	}


	if (isset($_POST['SendMessage']))
	{
		$json = $_POST['SendMessage'];
		$json = json_decode($json, true);

		$from_id = $json['From_Id'];
		$message = $json['Message'];
		$to_id = $json['To_Id'];

		$sql = "insert into messages (`from_id`, `message`, `to_id`) values ('$from_id', '$message', '$to_id')";
		$result = $conn->query($sql);

		if ($result)
		{
			PushAll($conn, $from_id, $message, $to_id);
		}
		else
		{
			echo "NOK";
		}
	}

	function PushAll($conn, $from_id, $message, $to_id)
	{
			$sql = "select token from users where id <> '$to_id'";
			$server_key = 'AAAA-02vzsY:APA91bEsku0tr40oqsvQsIjvHVBmJflD9hh5_yh5H1LM8TfWRvamiTXGzg2bPV6CECmHe-kOz9qEoWVVDwGjRpUa3MNcJJy8nUMmYjMt2Epso_2t_5RjPFr-2jSg9BVnzZ60NZ4we3GG';

			$result = $conn->query($sql);
			if ($result !== false && $result -> num_rows > 0)
			{
				$arr = [];
				$inc = 0;
				while($row = $result->fetch_assoc())
				{
					$arr[$inc] = $row['token'];
					$inc++;
				}
			}

			$url = 'https://fcm.googleapis.com/fcm/send';
//
			//$payload = array('type'=> $kind);
			$data = array(
					'title'=>'Message',
					'sound' => "default",
					'msg'=> $message,
					'body'=> $message,
					'color' => "#79bc64"
			);

			$fields = array();
			$fields['registration_ids'] = $arr;
			$fields['notification'] = $data;
			$fields['priority'] = 'high';
			$fields['data'] = $payload;
			//header with content_type api key
			$headers = array(
				'Content-Type:application/json',
				'Authorization:key='.$server_key
			);

			$ch = curl_init();
			curl_setopt($ch, CURLOPT_URL, $url);
			curl_setopt($ch, CURLOPT_POST, true);
			curl_setopt($ch, CURLOPT_HTTPHEADER, $headers);
			curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
			curl_setopt($ch, CURLOPT_SSL_VERIFYHOST, 0);
			curl_setopt($ch, CURLOPT_SSL_VERIFYPEER, false);
			curl_setopt($ch, CURLOPT_POSTFIELDS, json_encode($fields));
			$result = curl_exec($ch);
			if ($result === FALSE) {
				die('FCM Send Error: ' . curl_error($ch));
			}
			curl_close($ch);
			echo $result;
		}




?>
