
$ErrorActionPreference = 'Stop'; 
$toolsDir   = "$(Split-Path -parent $MyInvocation.MyCommand.Definition)"
$url        = 'https://github.com/jibedoubleve/log-reader/releases/download/V0.6.0/logreader.0.6.0.setup.exe' 
$url64      = 'https://github.com/jibedoubleve/log-reader/releases/download/V0.6.0/logreader.0.6.0.setup.exe' 

$packageArgs = @{
  packageName   = $env:ChocolateyPackageName
  unzipLocation = $toolsDir
  fileType      =  'EXE' 
  url           = $url
  url64bit      = $url64

  softwareName  = 'logreader*' 

  checksum      = ''
  checksumType  = 'sha256' 
  checksum64    = ''
  checksumType64= 'sha256' 

  validExitCodes= @(0, 3010, 1641)
  silentArgs   = '/VERYSILENT /SUPPRESSMSGBOXES /NORESTART /SP-' 
}

Install-ChocolateyPackage @packageArgs 










    








