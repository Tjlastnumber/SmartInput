@echo off
echo ********************************************************************** 
echo *** ����SmartInpus 
echo ********************************************************************** 
REM ����2017������������ʾ����ַ
call "%VS150COMNTOOLS%\VsDevCmd.bat"
REM ��ǰ·������Ŀ�ļ�
MSBuild %~dp0\SmartIME.sln /t:Rebuild
echo Success
pause