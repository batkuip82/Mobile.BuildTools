$currentDirectory = split-path $MyInvocation.MyCommand.Definition

# See if we have the ClientSecret available
if([string]::IsNullOrEmpty($env:SignClientSecret)){
    Write-Host "Client Secret not found, not signing packages"
    return;
}

dotnet tool install SignClient -g

# Setup Variables we need to pass into the sign client tool

$appSettings = "$currentDirectory\appsettings.json"
$fileList = "$currentDirectory\filelist.txt"

$nupkgs = gci $env:BUILD_ARTIFACTSTAGINGDIRECTORY\*.nupkg -recurse | Select-Object -ExpandProperty FullName

foreach ($nupkg in $nupkgs){
   Write-Host "Submitting $nupkg for signing"

    SignClient 'sign' -c $appSettings -i $nupkg -f $fileList -r $env:SignClientUser -s $env:SignClientSecret -n 'Mobile.BuildTools' -d 'Mobile.BuildTools' -u 'https://github.com/dansiegel/Mobile.BuildTools'

    Write-Host "Finished signing $nupkg"
}

Write-Host "Sign-package complete"