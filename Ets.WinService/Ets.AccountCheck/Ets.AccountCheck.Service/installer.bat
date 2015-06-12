@echo 正在安装Ets.AccountCheck.Service
@Set Path=C:\Windows\Microsoft.NET\Framework\v4.0.30319;
@Set svn_dir=%~dp0
installutil %svn_dir%Ets.AccountCheck.Service.exe

@echo 正在启动Ets.AccountCheck.Service
C:\WINDOWS\system32\net.exe start "Ets.AccountCheck.Service"

pause
@echo 成功！