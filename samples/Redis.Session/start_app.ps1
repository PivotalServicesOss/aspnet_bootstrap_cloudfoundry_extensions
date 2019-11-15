Set-Item -path env:PORT -value (5001) 

Write-Output "Server Starting at port $env:PORT, http://localhost:$env:PORT"

..\server\hwc.exe -appRootPath ".\WebForms"

