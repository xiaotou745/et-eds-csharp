@echo ���ڰ�װEts.AccountCheck.Service
@Set Path=C:\Windows\Microsoft.NET\Framework\v4.0.30319;
@Set svn_dir=%~dp0
installutil %svn_dir%Ets.AccountCheck.Service.exe

@echo ��������Ets.AccountCheck.Service
C:\WINDOWS\system32\net.exe start "Ets.AccountCheck.Service"

pause
@echo �ɹ���