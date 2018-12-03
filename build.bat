@echo off
echo ********************************************************************** 
echo *** 发布SmartInpus 
echo ********************************************************************** 
REM 本地2017开发者命令提示符地址
call "%VS150COMNTOOLS%\VsDevCmd.bat"
REM 当前路径下项目文件
MSBuild %~dp0\SmartIME.sln /t:Rebuild
echo Success
pause