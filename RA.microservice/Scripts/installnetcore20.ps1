$localPath = 'C:\dotnet-sdk-2.0.0-win-x64.exe'

if(!(Test-Path $localPath))
{
    Invoke-WebRequest -Uri 'https://download.microsoft.com/download/0/F/D/0FD852A4-7EA1-4E2A-983A-0484AC19B92C/dotnet-sdk-2.0.0-win-x64.exe' -OutFile $localPath
    & $localPath /quiet /log c:\InstallNetCore20.log
}