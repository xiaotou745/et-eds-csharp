@echo ����ֹͣServiceWindowService
C:\WINDOWS\system32\net.exe stop "Ets.AccountCheck.Service"

@echo ����ж��WindowService

@Set Path=C:\WINDOWS\Microsoft.NET\Framework\v4.0.30319;
@Set svn_dir=%~dp0
installutil %svn_dir%Ets.AccountCheck.Service.exe /u

pause
@echo �ɹ���