$script:project_config = "Debug"
properties {
  Framework '4.7'
  $base_dir = resolve-path .
  $build_dir = ".\build-artifacts"
  $publish_dir = ".\publish-artifacts"
  $solution_file = "$base_dir\$solution_name.sln"
  $test_dir = "$base_dir\test"
  $nuget_exe = "nuget.exe"
  $local_nuget_repo = "c:\MyLocalNugetRepo"
  $remote_nuget_repo = "https://api.nuget.org/v3/index.json"
  $remote_myget_repo = "https://www.myget.org/F/ajaganathan/api/v3/index.json"
  $repo_api_key = $api_key
  $version = get_version
  $date = Get-Date 
  $ReleaseNumber = $version
  
  Write-Host "**********************************************************************"
  Write-Host "Release Number: $ReleaseNumber"
  Write-Host "**********************************************************************"
  
  $packageId = if ($env:package_id) { $env:package_id } else { "$solution_name" }
}

#These are aliases for other build tasks. They typically are named after the camelcase letters (rd = Rebuild Databases)
task default -depends DevBuild
task cib -depends CiBuild
task cip -depends CiPublish
task ? -depends help
task rl -depends ReleaseLocal
task rn -depends ReleaseNuget
task rm -depends ReleaseMyget


task emitProperties {
  Write-Host "solution_name=$solution_name"
  Write-Host "base_dir=$base_dir"
  Write-Host "build_dir=$build_dir"
  Write-Host "solution_file=$solution_file"
  Write-Host "test_dir=$test_dir"
  Write-Host "publish_dir=$publish_dir"
  Write-Host "project_config=$project_config"
  Write-Host "version=$version"
  Write-Host "ReleaseNumber=$ReleaseNumber"
}

task help {
   Write-Help-Header
   Write-Help-Section-Header "Comprehensive Building"
   Write-Help-For-Alias "(default)" "Intended for first build or when you want a fresh, clean local copy"
   Write-Help-For-Alias "ci" "Continuous Integration build (long and thorough) with packaging"
   Write-Help-Footer
   exit 0
}

#These are the actual build tasks. They should be Pascal case by convention
task DevBuild -depends SetDebugBuild, emitProperties, Clean, Restore, Compile, UnitTest
task CiBuild -depends SetReleaseBuild, emitProperties, Clean, Restore, Compile, UnitTest
task CiPublish -depends CiBuild, NugetPack
task ReleaseLocal -depends DevBuild, NugetPushLocal
task ReleaseNuget -depends CiPublish, NugetPack, NugetPush
task ReleaseMyget -depends CiPublish, NugetPack, MygetPush

task SetDebugBuild {
    $script:project_config = "Debug"
}

task SetReleaseBuild {
    $script:project_config = "Release"
}

task NugetPushLocal -depends NugetPack {
	Push-Location $base_dir
	$packages = @(Get-ChildItem -Recurse -Filter "*.nupkg" | Where-Object {$_.Directory -like "*publish-artifacts*"}).FullName
	
	foreach ($package in $packages) {
		Write-Host "Executing nuget add for the package: $package"
		exec { & $nuget_exe add $package -Source $local_nuget_repo }
	}

	Pop-Location
}

task NugetPush {
	Push-Location $base_dir
	$packages = @(Get-ChildItem -Recurse -Filter "*.nupkg" | Where-Object {$_.Directory -like "*publish-artifacts*"}).FullName
	
	foreach ($package in $packages) {
		Write-Host "Executing nuget add for the package: $package"
		exec { & $nuget_exe push $package -Source $remote_nuget_repo -ApiKey $repo_api_key}
	}

	Pop-Location
}

task MygetPush {
	Push-Location $base_dir
	$packages = @(Get-ChildItem -Recurse -Filter "*.nupkg" | Where-Object {$_.Directory -like "*publish-artifacts*"}).FullName
	
	foreach ($package in $packages) {
		Write-Host "Executing nuget add for the package: $package"
		exec { & $nuget_exe push $package -Source $remote_myget_repo -ApiKey $repo_api_key}
	}

	Pop-Location
}

task NugetPack -depends UnitTest{
	Push-Location $base_dir
	$projects = @(Get-ChildItem -Recurse -Filter "*.csproj" | Where-Object {$_.Directory -like '*src*'}).FullName	

	foreach ($project in $projects) {
		Write-Host "Executing nuget pack on the project: $project"
		exec { & $nuget_exe pack $project -Version $version -OutputDirectory $publish_dir -Properties Configuration=$project_config }
	}

	Pop-Location
}

task UnitTest -depends Compile{
   Write-Host "******************* Now running Unit Tests *********************"
   $vstest_exe = get_vstest_executable
   Push-Location $base_dir
   $test_assemblies = @((Get-ChildItem -Recurse -Filter "*Tests.dll" | Where-Object {$_.Directory -like '*test*'}).FullName) -join ' '
   Write-Host "Executing tests on the following assemblies: $test_assemblies"
   Start-Process -FilePath $vstest_exe -ArgumentList $test_assemblies ,"/Parallel" -NoNewWindow -Wait
   Pop-Location
}

task Clean {
    Get-ChildItem -inc build-artifacts -rec | Remove-Item -rec -Force
    if (Test-Path $publish_dir) {
        delete_directory $publish_dir
    }
    Write-Host "******************* Now Cleaning the Solution *********************"
    exec { msbuild /t:clean /v:q /p:Configuration=$project_config /p:Platform="Any CPU" $solution_file }
}

task Restore -depends Clean{
    exec { & $nuget_exe restore $solution_file  }
}

task Compile -depends Restore {
    exec { msbuild.exe /t:build /v:q /p:Configuration=$project_config /p:Platform="Any CPU" /nologo $solution_file }
}


# -------------------------------------------------------------------------------------------------------------
# generalized functions for Help Section
# --------------------------------------------------------------------------------------------------------------

function Write-Help-Header($description) {
   Write-Host ""
   Write-Host "********************************" -foregroundcolor DarkGreen -nonewline;
   Write-Host " HELP " -foregroundcolor Green  -nonewline;
   Write-Host "********************************"  -foregroundcolor DarkGreen
   Write-Host ""
   Write-Host "This build script has the following common build " -nonewline;
   Write-Host "task " -foregroundcolor Green -nonewline;
   Write-Host "aliases set up:"
}

function Write-Help-Footer($description) {
   Write-Host ""
   Write-Host " For a complete list of build tasks, view default.ps1."
   Write-Host ""
   Write-Host "**********************************************************************" -foregroundcolor DarkGreen
}

function Write-Help-Section-Header($description) {
   Write-Host ""
   Write-Host " $description" -foregroundcolor DarkGreen
}

function Write-Help-For-Alias($alias,$description) {
   Write-Host "  > " -nonewline;
   Write-Host "$alias" -foregroundcolor Green -nonewline;
   Write-Host " = " -nonewline;
   Write-Host "$description"
}

# -------------------------------------------------------------------------------------------------------------
# generalized functions
# --------------------------------------------------------------------------------------------------------------

function global:delete_file($file) {
    if($file) { remove-item $file -force -ErrorAction SilentlyContinue | out-null }
}

function global:delete_directory($directory_name)
{
  rd $directory_name -recurse -force  -ErrorAction SilentlyContinue | out-null
}

function global:delete_files($directory_name) {
    Get-ChildItem -Path $directory_name -Include * -File -Recurse | foreach { $_.Delete()}
}

function global:get_vstest_executable() {
    $vstest_exe = exec { & "c:\\Program Files (x86)\\Microsoft Visual Studio\\Installer\\vswhere.exe"  -latest -products * -requires Microsoft.VisualStudio.PackageGroup.TestTools.Core -property installationPath}
    $vstest_exe = join-path $vstest_exe 'Common7\IDE\CommonExtensions\Microsoft\TestWindow\vstest.console.exe'
    return $vstest_exe
}

function global:get_version(){
	$verPropsPath = "$base_dir\versions.props"
    $verProps = [xml](Get-content $verPropsPath)
    $versionNumber = $verProps.Project.PropertyGroup[0].PivotalServicesBootstrapVersion
    $versionSuffix = $verProps.Project.PropertyGroup[0].PivotalServicesBootstrapVersionSuffix

    return  $versionNumber+$versionSuffix
}

