# This is a convenience script for setting up docker for SQL Server and the app in local dev.
$dockerDesktopProcess = Get-Process 'Docker Desktop' -ErrorAction SilentlyContinue
if($dockerDesktopProcess -eq $null) 
{
	Write-Error "Docker Desktop must be installed and running before continuing."
	Exit
}

if ($IsLinux) {
    Write-Warning "Script is running on Linux.  Not fully tested."
}
elseif ($IsWindows) {
    Write-Host "Script is running on Windows"
}


$envFile = ".\dev.env"
if((Test-Path -Path $envFile -PathType leaf) -eq $False){
	Write-Error "dev.env file does not exist. Cannot continue."
	Exit
}

Write-Output "Process environment variables"
$server = hostname
$envFileContent = Get-Content -Path $envFile
$originalEnvFileContent = Get-Content -Path $envFile
$envFileContent -replace '{SERVER}', $server | Set-Content $envFile
$envFileContent = Get-Content -Path $envFile
		
$devVariables = @{}
$envFileContent | foreach {
	$name, $value = $_.split('=')
	if ([string]::IsNullOrWhiteSpace($name) -or $name.Contains('#')) {
	}
	else {
		$devVariables[$name] = $value
		$envFileContent = $envFileContent -replace "{$name}", $value
	}
}

Set-Content -Path $envFile -Value $envFileContent
	
$dockerSqlSaPassword = $devVariables['DOCKER_SQL_SA_PASSWORD']
$dockerSqlPort = $devVariables['DOCKER_SQL_PORT']
$devCertPassword = $devVariables['ASPNETCORE_Kestrel__Certificates__Default__Password']
		
Write-Output $dockerSqlConnectionString
	
# Run Sql Server container
# Note: array count = 2 because first item contains headers
$isSqlDockerRunning = @(docker ps -f name=sql).Count -eq 2  
if($isSqlDockerRunning) {
	Write-Output "Sql server container already running"
}
else {
	Write-Output "Pull sql server image"
	docker pull mcr.microsoft.com/mssql/server

	Write-Output "Run sql server image"
    Start-Process powershell -ArgumentList "-noexit -command docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=$dockerSqlSaPassword' -p $dockerSqlPort`:1433 --name sql-server-docker -d mcr.microsoft.com/mssql/server"
	Start-Sleep -Seconds 10
}
	
Write-Output "Build spare parts image"
docker build -t spare-parts-image -f SpareParts.API\Dockerfile .
		
# TODO this won't work on Linux. Need to implement solution for it.
Write-Output "Setup local dev certs"
dotnet dev-certs https -ep $env:USERPROFILE\.aspnet\https\aspnetapp.pfx -p $devCertPassword
dotnet dev-certs https --trust
	
Write-Output "Run spare parts image"
Start-Process powershell -ArgumentList "-noexit -command docker run --name spare-parts --rm -it -p 8000:80 -p 8001:443 --env-file $envFile -v $env:USERPROFILE\.aspnet\https:/https/ spare-parts-image"

#Ping app to see if it is up yet
$siteUri = "https://localhost:8001"
$isSiteOK = $False
$retryAttempts = 10
while($isSiteOK -eq $False -and $retryAttempts -gt 0) {

    Write-Output "Checking App is available to start."
    try {
        $req = Invoke-WebRequest -UseBasicParsing -uri $siteUri
        if($req.StatusCode -eq 200) {
            Write-Output "App is Ready."
            $isSiteOK = $True
        }
    }
    catch {
        Write-Output "App Not Ready. Attempts remaining: $retryAttempts " 
        $retryAttempts -= 1
        Start-Sleep -Seconds 5
    }    
}

Write-Output "Launch app in browser"
Start-Process $siteUri

Set-Content -Path $envFile -Value $originalEnvFileContent
