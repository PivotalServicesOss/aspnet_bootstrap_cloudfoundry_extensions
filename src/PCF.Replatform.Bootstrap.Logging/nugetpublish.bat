IF "%1"=="" @GOTO NO_VER_ARG_PROVIDED

%INFO% Argument is the version of the publising package

nuget pack PCF.Replatform.Bootstrap.Logging.csproj -Version %1 -OutputDirectory ..\_artifacts

IF %ERRORLEVEL% NEQ 0 GOTO ERROR

IF "%2"=="" @GOTO NO_SRC_ARG_PROVIDED
nuget push ..\_artifacts/PivotalServices.CloudFoundry.Replatform.Bootstrap.Logging.%1.nupkg -Source %2

IF %ERRORLEVEL% NEQ 0 GOTO ERROR
IF %ERRORLEVEL% EQU 0 GOTO SUCCESS

:NO_SRC_ARG_PROVIDED
nuget add ..\_artifacts/PivotalServices.CloudFoundry.Replatform.Bootstrap.Logging.%1.nupkg -Source c:\MyLocalNugetRepo

IF %ERRORLEVEL% NEQ 0 GOTO ERROR
IF %ERRORLEVEL% EQU 0 GOTO SUCCESS

:NO_VER_ARG_PROVIDED
@echo "*** No Argument(version) provided ***"
exit /B 9

:ERROR
@echo "*** Error occurred wile running the command n %computername% ***"
exit /B 9

:SUCCESS
cd %~dp0
@echo "**************************************************************
@echo "************** Process Completed Successfully ****************
@echo "**************************************************************
exit /B 9