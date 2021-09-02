$manifests = @((Get-ChildItem -Recurse -Filter "*manifest.yml").FullName) -join ' '
foreach($manifest in $manifests.Split(" "))
{
    Write-Host "Executing cf push for: $manifest"
    cf push -f $manifest
    if($LASTEXITCODE -ne 0) {exit $LASTEXITCODE}
}