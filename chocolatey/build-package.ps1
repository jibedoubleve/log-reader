param(
    $configuration = "beta",
    $version
)
<################################################################################
 # VARIABLES
 ################################################################################>
$ErrorActionPreference = 'Stop'; 

$publishDir = "..\Publish"
$outputDir = "$publishDir\logreader"
$nuspec = "$outputDir\logreader.nuspec"
$installScript    = "$pwd\logreader\tools\chocolateyinstall.ps1"

<################################################################################
 # FUNCTIONS
 ################################################################################>
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
 Write-Host "=== Working dir  : $pwd"           -ForegroundColor Yellow
 Write-Host "=== version      : $version"       -ForegroundColor Yellow
 Write-Host "=== outputDir    : $outputDir"     -ForegroundColor Yellow
 Write-Host "=== nuspec       : $nuspec"        -ForegroundColor Yellow
 Write-Host "=== publishDir   : $publishDir"    -ForegroundColor Yellow
 Write-Host "=== installScript: $installScript" -ForegroundColor Yellow
 Write-Host "----" -ForegroundColor Yellow

if (Test-Path  $outputDir) {
    Remove-Item -Force -Recurse $outputDir
}

Copy-Item .\logreader $publishDir -Recurse -Force

$(Get-Content $nuspec) -replace "<version>.*</version>", "<version>$version</version>" | Set-Content -Path $nuspec

$(Get-Content $installScript) -replace "https://github.com/jibedoubleve/log-reader/releases/download/\d\.\d\.\d/logreader.\d{1,3}\.\d{1,3}\.\d{1,3}\.setup\.exe", "https://github.com/jibedoubleve/log-reader/releases/download/V$version/logreader.$version.setup.exe" | Set-Content -Path $installScript

choco pack $nuspec -out $publishDir

if (Test-Path  $outputDir) {
    Remove-Item -Force -Recurse $outputDir
}