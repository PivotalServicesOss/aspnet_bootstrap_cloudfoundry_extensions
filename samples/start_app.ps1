Set-Item -path env:PORT -value (5001) 

$path=$args[0]

Write-Output "Server Starting at port $env:PORT, http://localhost:$env:PORT"

.\server\hwc.exe -appRootPath $path

