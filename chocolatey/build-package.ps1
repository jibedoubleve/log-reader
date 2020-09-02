param(
    $release = "beta"
)

$ErrorActionPreference = 'Stop'; 
function GetSemVersion() {
    $items = Get-ChildItem "..\Publish\logreader.*.setup.exe"

    $items[0].Name -match "logreader\.(\d{1,2}\.\d{1,2}\.\d{1,2}).*\.(\d{1,2})\.setup.exe" > $null
    return "$($Matches[1])$(GetMode).$($Matches[2])"
}
function GetVersion() {
    $items = Get-ChildItem "..\Publish\logreader.*.setup.exe"

    $items[0].Name -match "logreader\.(\d{1,2}\.\d{1,2}\.\d{1,2}).*\.(\d{1,2})\.setup.exe" > $null
    return "$($Matches[1]).$($Matches[2])"
}

function GetMode() {
    if (@("beta", "debug").Contains($release.ToLower())) {
        return "-beta"
    }
    else {
        return ""
    }
}


if (Test-Path  "..\Publish\logreader") {
    Remove-Item -Force -Recurse "..\Publish\logreader"
}

Copy-Item .\logreader ..\Publish -Recurse

$nuspec = "..\Publish\logreader\logreader.nuspec"
$instScr = "..\Publish\logreader\tools\chocolateyinstall.ps1"
$version = GetVersion
$semVersion = GetSemVersion

write-host $version -ForegroundColor Cyan

$(Get-Content $nuspec) -replace "<version>.*</version>", "<version>$version</version>" | Set-Content -Path $nuspec
$(Get-Content $instScr) -replace "toolsDir 'logreader.(.*).setup.exe'", "toolsDir 'logreader.$semVersion.setup.exe'" | Set-Content -Path $instScr

Copy-Item "..\Publish\logreader.*.setup.exe" "..\Publish\logreader\tools"

choco pack $nuspec -out "..\Publish"
