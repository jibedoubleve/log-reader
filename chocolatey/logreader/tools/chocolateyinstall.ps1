
$ErrorActionPreference = 'Stop'; 
$toolsDir   = "$(Split-Path -parent $MyInvocation.MyCommand.Definition)"
$url        = 'https://github.com/jibedoubleve/log-reader/releases/download/0.6.0/logreader.0.6.0.setup.exe' 

$packageArgs = @{
  packageName   = $env:ChocolateyPackageName
  unzipLocation = $toolsDir
  fileType      =  'EXE' 
  url           = $url
  url64bit      = $url64

  softwareName  = 'logreader*' 

  checksum      = '46BB8555641E4DA649061D3A87F1AF153DC0BE54E5EC7A08CC3D68AA828FA7F4'
  checksumType  = 'sha256' 

  validExitCodes= @(0, 3010, 1641)
  silentArgs   = '/VERYSILENT /SUPPRESSMSGBOXES /NORESTART /SP-' 
}

Install-ChocolateyPackage @packageArgs 










    








