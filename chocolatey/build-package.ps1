param(
    $configuration = "beta",
    $version
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

write-host "Version: $version" -ForegroundColor Cyan

$(Get-Content $nuspec) -replace "<version>.*</version>", "<version>$version</version>" | Set-Content -Path $nuspec

Copy-Item $installer "$outputDir\tools"

choco pack $nuspec -out $publishDir

if (Test-Path  $outputDir) {
    Remove-Item -Force -Recurse $outputDir
}