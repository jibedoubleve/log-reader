param(
    $configuration = "beta"
)
<################################################################################
 # VARIABLES
 ################################################################################>
$ErrorActionPreference = 'Stop'; 

$publishDir = "..\Publish"
$installer = "$publishDir\logreader.*.setup.exe"
$outputDir = "$publishDir\logreader"

$pattern = "logreader\.(\d{1,2}\.\d{1,2}\.\d{1,2}).*((\.|)\d{0,2})\.setup.exe"

<################################################################################
 # FUNCTIONS
 ################################################################################>
function GetSemVersion() {
    

    if ($items = Get-ChildItem $installer) {    
        $items[0].Name -match $pattern > $null
        return "$($Matches[1])$(GetMode).$($Matches[2])"
    }
    else {
        throw "Cannot extract semver: Cannot find file '$installer'. [PWD] $pwd"
    }
}
function GetVersion() {
    if ($items = Get-ChildItem $installer) {
        $items[0].Name -match $pattern > $null
        return "$($Matches[1])$($Matches[2])"
    }
    else {
        throw "Cannot extract version: Cannot find file '$installer'. [PWD] $pwd"
    }
}

function GetMode() {
    if (@("beta", "debug").Contains($configuration.ToLower())) {
        return "-beta"
    }
    else {
        return ""
    }
}

<################################################################################
 # MAIN
 ################################################################################>

 Write-Host "========================================" -ForegroundColor Cyan
 Write-Host "=== BUILDING CHOCOLATEY PACKAGE      ===" -ForegroundColor Cyan
 Write-Host "========================================" -ForegroundColor Cyan
 Write-Host "=== Working dir: $pwd" -ForegroundColor Yellow
 Write-Host "----" -ForegroundColor Yellow

if (Test-Path  $outputDir) {
    Remove-Item -Force -Recurse $outputDir
}

Copy-Item .\logreader $publishDir -Recurse -Force

$nuspec = "$outputDir\logreader.nuspec"
$instScr = "$outputDir\tools\chocolateyinstall.ps1"
$version = GetVersion
$semVersion = GetSemVersion

write-host "Version: $version" -ForegroundColor Cyan

$(Get-Content $nuspec) -replace "<version>.*</version>", "<version>$version</version>" | Set-Content -Path $nuspec
$(Get-Content $instScr) -replace "toolsDir 'logreader.(.*).setup.exe'", "toolsDir 'logreader.$semVersion.setup.exe'" | Set-Content -Path $instScr

Copy-Item $installer "$outputDir\tools"

choco pack $nuspec -out $publishDir

if (Test-Path  $outputDir) {
    Remove-Item -Force -Recurse $outputDir
}