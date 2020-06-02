[CmdletBinding()]
param (
    [Parameter(Mandatory = $true)]
    [string]
    $server,
    [Parameter(Mandatory = $true)]
    [string]
    $database,
    [Parameter(Mandatory = $true)]
    [string]
    $adminName,
    [Parameter(Mandatory = $true)]
    [securestring]
    $adminPassword,
    [Parameter(Mandatory = $true)]
    [string]
    $NewUserName,
    [Parameter(Mandatory = $true)]
    [string]
    $keyVaultName
)
.'.\Powershell\powershellFunctions.ps1'
$newPassword = Generate-Password
$BSTR = [System.Runtime.InteropServices.Marshal]::SecureStringToBSTR($newPassword)
$ResacSQLServiceAccountPasswordPlain = [System.Runtime.InteropServices.Marshal]::PtrToStringAuto($BSTR)
Write-Host "##vso[task.setvariable variable=ResacSQLServiceAccountPassword;issecret=true;]$ResacSQLServiceAccountPasswordPlain"

$BSTR = [System.Runtime.InteropServices.Marshal]::SecureStringToBSTR($adminPassword)
$sqlServerAdminLoginPasswordPlain = [System.Runtime.InteropServices.Marshal]::PtrToStringAuto($BSTR)
	
$connectionString = "Server=$server;Database=$database;User ID=$adminName;Password=$sqlServerAdminLoginPasswordPlain;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
$connection = New-Object -TypeName System.Data.SqlClient.SqlConnection($connectionString)
$queryPath = ".\Powershell\CreateUser.sql"
$query = [System.IO.File]::ReadAllText($queryPath)
$command = New-Object -TypeName System.Data.SqlClient.SqlCommand($query, $connection)

$username = New-Object -TypeName System.Data.SqlClient.SqlParameter("@Username", $NewUserName)
$password = New-Object -TypeName System.Data.SqlClient.SqlParameter("@Password", $secret.SecretValueText)

$command.Parameters.Add($username)
$command.Parameters.Add($password)

$connection.Open()
$command.ExecuteNonQuery()
$connection.Close()