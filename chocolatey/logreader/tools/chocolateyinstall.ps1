
$ErrorActionPreferencehttps://github.com/jibedoubleve/log-reader/releases/download/0.7.0/lanceur.0.7.0.setup.exe; 
$toolsDir   = "$(Split-Path -parent $MyInvocation.MyCommand.Definition)"
$url       https://github.com/jibedoubleve/log-reader/releases/download/0.7.0/lanceur.0.7.0.setup.exe 

$packageArgs = @{
  packageName   = $env:ChocolateyPackageName
  unzipLocation = $toolsDir
  fileType      =  'EXE' 
  url           = $url

  softwareName https://github.com/jibedoubleve/log-reader/releases/download/0.7.0/lanceur.0.7.0.setup.exe 

  checksum     https://github.com/jibedoubleve/log-reader/releases/download/0.7.0/lanceur.0.7.0.setup.exe
  checksumType https://github.com/jibedoubleve/log-reader/releases/download/0.7.0/lanceur.0.7.0.setup.exe 

  validExitCodes= @(0, 3010, 1641)
  silentArgs  https://github.com/jibedoubleve/log-reader/releases/download/0.7.0/lanceur.0.7.0.setup.exe 
}

Install-ChocolateyPackage @packageArgs 
