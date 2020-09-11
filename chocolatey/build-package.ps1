param(
    $configuration = "beta",
    [switch]$publish
)
<################################################################################
 # VARIABLES
 ################################################################################>
$ErrorActionPreference = 'Stop'; 

$publishDir = Resolve-Path "..\Publish"
$installer = "$publishDir\logreader.*.setup.exe"
$outputDir = "$publishDir\logreader"

$pattern = "logreader\.(\d{1,2}\.\d{1,2}\.\d{1,2}).*((\.|)\d{0,2})\.setup.exe"

<################################################################################
 # FUNCTIONS
 ################################################################################>
function Write-Step($message) {
    Write-Host ""                         
    Write-Host "---- $message "  -ForegroundColor Yellow 
    Write-Host "----------------------------------------" -ForegroundColor Yellow
}
function GetSemVersion() {
    

    if ($items = Get-ChildItem $installer) {    
        $items[0].Name -match $pattern > $null
        return "$($Matches[1])$(GetMode)$($Matches[2])"
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
        return "-beta."
    }
    else {
        return ""
    }
}

<################################################################################
 # MAIN
 ################################################################################>
try {
    Write-Host "========================================" -ForegroundColor Cyan
    Write-Host "=== BUILDING CHOCOLATEY PACKAGE      ===" -ForegroundColor Cyan
    Write-Host "========================================" -ForegroundColor Cyan
    Write-Step "Variables"
    Write-Host "Working dir: $pwd" -ForegroundColor DarkYellow

    Write-Step "Copying files"
    Write-Host "Output dir: $outputDir" -ForegroundColor DarkYellow
    if (Test-Path  $outputDir) {
        Remove-Item -Force -Recurse $outputDir
    }

    Copy-Item .\logreader $publishDir -Recurse -Force

    $nuspec = "$outputDir\logreader.nuspec"
    $instScr = "$outputDir\tools\chocolateyinstall.ps1"
    $version = GetVersion
    $semVersion = GetSemVersion

    Write-Host "Version: $version" -ForegroundColor DarkYellow

    $(Get-Content $nuspec) -replace "<version>.*</version>", "<version>$version</version>" | Set-Content -Path $nuspec
    $(Get-Content $instScr) -replace "toolsDir 'logreader.(.*).setup.exe'", "toolsDir 'logreader.$semVersion.setup.exe'" | Set-Content -Path $instScr

    Copy-Item $installer "$outputDir\tools"

    Write-Step "Publishing Chocolatey package."
    choco pack $nuspec -out $publishDir

    if ($publish) {
        $package = Join-Path $publishDir "logreader.$semVersion.nupkg"

        Write-Host "PWD: $pwd" -ForegroundColor DarkYellow 

        Write-Host "Pushing the package $package..." -ForegroundColor DarkYellow
        Write-host "Chocolatey token: $($env:CHOCOLATEY_TOKEN)" -ForegroundColor DarkYellow
        choco push $package --source https://push.chocolatey.org/ -k $env:CHOCOLATEY_TOKEN
    }
    else {
        Write-Host "The package won't be pushed!" -ForegroundColor DarkYellow
    }

    if (Test-Path  $outputDir) {
        Write-Host "Deleting artifacts..." -ForegroundColor DarkYellow
        Remove-Item -Force -Recurse $outputDir
    }
}
finally {
    Write-Host $_ -ForegroundColor Yellow -BackgroundColor DarkRed
    Pop-Location
}