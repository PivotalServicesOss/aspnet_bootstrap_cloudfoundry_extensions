$script:project_config = "Release"
properties {
  Framework '4.7.1'
  $base_dir = resolve-path .
  $build_dir = ".\build-artifacts"
  $publish_dir = ".\publish-artifacts"
  $solution_file = "$base_dir\$solution_name.sln"
  $test_dir = "$base_dir\test"
  $nuget_exe = "nuget.exe"
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
task ci -depends CiBuild
task pb -depends PublishBase
task ? -depends help

task emitProperties {
  Write-Host "solution_name=$solution_name"
  Write-Host "base_dir=$base_dir"
  Write-Host "build_dir=$build_dir"
  Write-Host "solution_file=$solution_file"
  Write-Host "test_dir=$test_dir"
  Write-Host "publish_dir=$publish_dir"
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
task DevBuild -depends emitProperties, UpdateVersionProps, SetDebugBuild, Clean, Restore, Compile, UnitTest
task CiBuild -depends emitProperties, UpdateVersionProps, SetReleaseBuild, Clean, Restore, Compile, UnitTest
task PublishBase -depends CiBuild, PublishBasePackage

task SetDebugBuild {
    $script:project_config = "Debug"
}

task SetReleaseBuild {
    $script:project_config = "Release"
}

task SetVersion {
    set-content $base_dir\CommonAssemblyInfo.cs "// Generated file - do not modify",
            "using System.Reflection;",
            "[assembly: AssemblyVersion(`"$version`")]",
            "[assembly: AssemblyFileVersion(`"$version`")]",
            "[assembly: AssemblyInformationalVersion(`"$version`")]"
    Write-Host "Using version#: $version"
}

task UpdateVersionProps {
	Write-Host "******************* Updating versions.props file with Base package version as $version *********************"
    $verPropsPath = "$base_dir\versions.props"
    $verProps = [xml](Get-content $verPropsPath)
    $verProps.Project.PropertyGroup.PivotalServicesBootstrapBaseVersion = "$version"
    $verProps.Save($verPropsPath)
}

task UnitTest {
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

task Compile -depends Clean {
    exec { msbuild.exe /t:build /v:q /p:Configuration=$project_config /p:Platform="Any CPU" /nologo $solution_file }
}

task PublishBasePackage {
    Write-Host "Publishing to $publish_dir *****"
    if (!(Test-Path $publish_dir)) {
        New-Item -ItemType Directory -Force -Path $publish_dir
    }
    Copy-Item -Path $published_website\* -Destination $publish_dir -recurse -Force
}

task Push {
    Push-Location $publish_dir
    exec { & "cf" push -d $domain}
    Pop-Location
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
    Write-Host "******************* Getting the Version Number ********************"
    $version = get-content "$base_Dir\package.version" -ErrorAction SilentlyContinue
    if ($version -eq $null) {
        Write-Host "--------- No version found defaulting to 1.0.0 --------------------" -foregroundcolor Red
        $version = '1.0.0'
    }
    return $version
}

