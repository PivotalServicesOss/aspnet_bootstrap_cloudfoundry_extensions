@echo Executing build with default.ps1 configuration
@echo off
powershell.exe -NoProfile -ExecutionPolicy bypass -Command "& {.\configure-build.ps1 }"
powershell.exe -NoProfile -ExecutionPolicy bypass -Command "& {invoke-psake .\default.ps1 %1 -parameters @{"solution_name"="'Bootstrap.Cf.Extensions'";"api_key='%2'"}; exit !($psake.build_success) }"