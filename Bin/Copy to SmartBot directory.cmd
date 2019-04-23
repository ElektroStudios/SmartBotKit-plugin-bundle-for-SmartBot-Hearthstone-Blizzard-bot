@Echo OFF

Set "SmartBotDir=C:\Games\Hearthstone\Cheats\SmartBot"
Set "FolderToCopy=Release"

Echo+
Echo [+] Plugins:
FOR %%# IN ("%CD%\%FolderToCopy%\Plugins\*.dll") DO (
	Echo %%~nx#
	(Copy /Y "%%~f#" "%SmartBotDir%\Plugins\%%~nx#")1>NUL
)

Echo+
Echo [+] Plugin Audio Resources:
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
Echo [+] Power-Shell Scripts:
FOR %%# IN ("%CD%\%FolderToCopy%\Plugins\libs\*.ps1") DO (
	Echo %%~nx#
	(Copy /Y "%%~f#" "%SmartBotDir%\Plugins\libs\%%~nx#")1>NUL
)

Echo+
Echo [+] Executables:
FOR %%# IN ("%CD%\%FolderToCopy%\Plugins\libs\*.exe") DO (
	Echo %%~nx#
	(Copy /Y "%%~f#" "%SmartBotDir%\Plugins\libs\%%~nx#")1>NUL
)

Echo+
TimeOut /T 2
Exit /B 0