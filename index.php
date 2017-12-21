<?php
$servername = "127.0.0.1";
$username = "root";
$password = "";
$dbname = "pb_TunnelVisualizarDatabase";
$iddata = $_GET['iddata'];
$data_waterFlow = $_GET['data_waterFlow'];
$data_waterlavel = $_GET['data_waterlavel'];
if (!$iddata) { 
  $iddata = -1;
}
if (!$data_waterFlow) { 
  $data_waterFlow = 0;
}
if (!$data_waterlavel) {
  $data_waterlavel = 0;
}
echo("iddata = ".$iddata."\t"."data_waterFlow = ".$data_waterFlow."\t"."data_waterlavel = ".$data_waterlavel."<BR>\n");
$conn = mysqli_connect($servername,$username,$password,$dbname);
$sql = "INSERT INTO `pb_TunnelVisualizarDatabase`.`data_waterFlow` (`data`, `sensor_iddata`) VALUES ('$data_waterFlow', '$iddata')";
$result = mysqli_query($conn,$sql);
$sql = "INSERT INTO `pb_TunnelVisualizarDatabase`.`data_waterLavel` (`data`, `sensor_iddata`) VALUES ('$data_waterlavel', '$iddata');";
$result = mysqli_query($conn,$sql);
echo "<h2>The Field and Value data have been sent</h2>";
mysqli_close($conn);
?>