# pull dependencies

$has_nuget = Get-PackageProvider -ListAvailable | Out-String | Select-String -Pattern "NuGet" -Quiet
if(-Not($has_nuget)) {
    #install Nuget Package Provider
    [System.Net.WebRequest]::DefaultWebProxy.Credentials = [System.Net.CredentialCache]::DefaultCredentials
    Write-Host "No Nuget Package Provider Found: Installing now" -ForegroundColor Red
    Install-PackageProvider -Name NuGet -MinimumVersion 5.3.1.6268 -Force
}

$has_vsSetup = Get-Module -ListAvailable | Select-String -Pattern "VSSetup" -Quiet
if(-Not($has_vsSetup)) {
    #install VSSetup
    Write-Host "No VSSetup Module Found: Installing now" -ForegroundColor Red
    Set-PSRepository -Name PSGallery -InstallationPolicy Trusted
    Install-Module VSSetup -Scope CurrentUser
}

$has_psake = Get-Module -ListAvailable | Select-String -Pattern "Psake" -Quiet
if(-Not($has_psake)) {
    #install psake
    Write-Host "No Psake Module Found: Installing now" -ForegroundColor Red
    Set-PSRepository -Name PSGallery -InstallationPolicy Trusted
    Install-Module Psake -Scope CurrentUser
}