REM C:\Windows\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe HRM.WinService.exe


@ECHO OFF

REM The following directory is for .NET 4.0
set DOTNETFX2=%SystemRoot%\Microsoft.NET\Framework\v4.0.30319
set PATH=%PATH%;%DOTNETFX2%
echo Setting Extension

echo Installing Attendance Win Service...
echo ---------------------------------------------------
C:\Windows\Microsoft.NET\Framework\v4.0.30319\InstallUtil "%~dp0HRM.WinService.exe"
echo ---------------------------------------------------
pause
echo Done.