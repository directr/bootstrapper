REM Running powershell startup

start /w cmd

if "%EMULATED%"=="true" goto :EXIT


:EXIT
EXIT /B 0