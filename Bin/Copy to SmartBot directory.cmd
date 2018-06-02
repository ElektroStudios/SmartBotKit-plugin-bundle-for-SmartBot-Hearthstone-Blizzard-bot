@Echo OFF

Set "SmartBotDir=G:\Windows\Online\Hearthstone\Cheats\SmartBot"
Set "FolderToCopy=Release"

Echo+
Echo [+] Plugins:
FOR %%# IN ("%CD%\%FolderToCopy%\Plugins\*.dll") DO (
	Echo %%~nx#
	(Copy /Y "%%~f#" "%SmartBotDir%\Plugins\%%~nx#")1>NUL
)

Echo+
Echo [+] Plugin Resources:
FOR %%# IN ("%CD%\%FolderToCopy%\Plugins\*.wav") DO (
	Echo %%~nx#
	(Copy /Y "%%~f#" "%SmartBotDir%\Plugins\%%~nx#")1>NUL
)

Echo+
Echo [+] Libs:
FOR %%# IN ("%CD%\%FolderToCopy%\Plugins\libs\*.dll") DO (
	Echo %%~nx#
	(Copy /Y "%%~f#" "%SmartBotDir%\Plugins\libs\%%~nx#")1>NUL
)

Echo+
Pause
Exit /B 0