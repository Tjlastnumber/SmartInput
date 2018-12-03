@echo off
type %~dp0\Banner.txt
call "%VS150COMNTOOLS%\VsDevCmd.bat"
MSBuild %~dp0\SmartIME.sln /t:Rebuild
echo Success
pause