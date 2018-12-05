@echo off
type %~dp0\Banner.txt
@REM 需要配置本地环境变量 VS150COMNTOOLS 到vs的开发人员命令行目录
call "%VS150COMNTOOLS%\VsDevCmd.bat"
MSBuild %~dp0\SmartIME.sln /t:Rebuild /p:Configuration=Release
echo Success
pause