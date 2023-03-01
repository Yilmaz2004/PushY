<?php
	$conn=mysqli_connect('localhost', 'root', '', 'Pushy'); // Check connection
	if (mysqli_connect_errno()){
		echo "Failed to connect to MySQL: " . mysqli_connect_error();
	} else {
		//echo 'from connection Success...,,,' . $conn->host_info . "";
	}
?>
