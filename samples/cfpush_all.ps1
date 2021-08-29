$manifests = @((Get-ChildItem -Recurse -Filter "*manifest.yml").FullName) -join ' '
foreach($manifest in $manifests.Split(" "))
{
    Write-Host "Executing cf push for: $manifest"
    exec { 
        & cf push -f $manifest
    }
}