@echo off

setlocal EnableDelayedExpansion

SET CA_ARCH_CONFIG=Release

SET CA_FOLDER_NAME=MOContab
SET CA_REGKEY_NAME=MOContab
SET CA_APP_EXE_NAME=contab.exe
SET CA_APP_EXE_NAME_NO_EXT=contab
SET CA_APP_SHORTCUT_NAME=Contab.lnk
SET CA_APP_SHOW_NAME=MO Contab
SET CA_APP_MANUFACTURER_NAME=MarcosOrtega

SET CA_BASE_PATH=%~d0%~p0
SET CA_ARCH=

ECHO:
REM ----------
REM - CONTAB -
REM ----------

REM ---------------------------------
REM - Detecting current architecture
REM ---------------------------------

IF DEFINED PROCESSOR_ARCHITEW6432 (
  SET CA_ARCH=%PROCESSOR_ARCHITEW6432%
) ELSE (
  SET CA_ARCH=%PROCESSOR_ARCHITECTURE%
)

REM Posible values: x86, amd64, arm, arm64
if exist "%CA_BASE_PATH%%CA_ARCH_CONFIG%\%CA_ARCH%\" (
  ECHO Arch supported: '%CA_ARCH%'.
) else (
  ECHO Unsupported arch: '%CA_ARCH%'.
  PAUSE
  EXIT /B 1
)

REM ---------------------------------
REM - Detecting start menu folder path
REM ---------------------------------

SET CA_COMMON_PROGRAMS_PATH=
SET CA_COMMON_PROGRAMS_PATH_TOKEN_NUM=1
SET CA_COMMON_PROGRAMS_PATH_TOKEN_FOUND=0
SET CA_COMMON_PROGRAMS_PATH_FOUND=0
:CA_COMMON_PROGRAMS_LOOP
  SET CA_COMMON_PROGRAMS_PATH_TOKEN_FOUND=0
  for /f "tokens=%CA_COMMON_PROGRAMS_PATH_TOKEN_NUM%" %%A in ('reg query "HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Shell Folders" /v "Common Programs"') DO (
    SET CA_TMP_VAL=%%A
    SET "CA_COMMON_PROGRAMS_PATH_TOKEN_FOUND=1"
    SET /A "CA_COMMON_PROGRAMS_PATH_TOKEN_NUM=CA_COMMON_PROGRAMS_PATH_TOKEN_NUM+1"
    IF %CA_COMMON_PROGRAMS_PATH_FOUND% EQU 1 (
      SET "CA_COMMON_PROGRAMS_PATH=%CA_COMMON_PROGRAMS_PATH% %%A"
      REM ECHO Continuing path: %%A
    ) ELSE IF "!CA_TMP_VAL:~1,2!" EQU ":\" (
      SET "CA_COMMON_PROGRAMS_PATH_FOUND=1"
      SET "CA_COMMON_PROGRAMS_PATH=%%A"
      REM ECHO Startup path: %%A
    ) ELSE (
      REM ECHO Ignoring: _%CA_COMMON_PROGRAMS_PATH_TOKEN_NUM%_ %%A
    )
  )
  IF !ERRORLEVEL! NEQ 0 (
    REM reset ERRORLEVEL by using CALL;
    CALL;
  ) ELSE IF !CA_COMMON_PROGRAMS_PATH_TOKEN_FOUND! NEQ 0 (
    GOTO :CA_COMMON_PROGRAMS_LOOP
  )

IF DEFINED CA_COMMON_PROGRAMS_PATH (
   ECHO CommmonPrograms found as '%CA_COMMON_PROGRAMS_PATH%'
) ELSE (
   ECHO CommmonPrograms NOT found.
)

REM ---------------------------------
REM - Detecting desktop folder path
REM ---------------------------------

SET CA_COMMON_DESKTOP_PATH=
SET CA_COMMON_DESKTOP_PATH_TOKEN_NUM=1
SET CA_COMMON_DESKTOP_PATH_TOKEN_FOUND=0
SET CA_COMMON_DESKTOP_PATH_FOUND=0
:CA_COMMON_DESKTOP_LOOP
  SET CA_COMMON_DESKTOP_PATH_TOKEN_FOUND=0
  for /f "tokens=%CA_COMMON_DESKTOP_PATH_TOKEN_NUM%" %%A in ('reg query "HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Shell Folders" /v "Common Desktop"') DO (
    SET CA_TMP_VAL=%%A
    SET "CA_COMMON_DESKTOP_PATH_TOKEN_FOUND=1"
    SET /A "CA_COMMON_DESKTOP_PATH_TOKEN_NUM=CA_COMMON_DESKTOP_PATH_TOKEN_NUM+1"
    IF %CA_COMMON_DESKTOP_PATH_FOUND% EQU 1 (
      SET "CA_COMMON_DESKTOP_PATH=%CA_COMMON_DESKTOP_PATH% %%A"
      REM ECHO Continuing path: %%A
    ) ELSE IF "!CA_TMP_VAL:~1,2!" EQU ":\" (
      SET "CA_COMMON_DESKTOP_PATH_FOUND=1"
      SET "CA_COMMON_DESKTOP_PATH=%%A"
      REM ECHO Desktop path: %%A
    ) ELSE (
      REM ECHO Ignoring: _%CA_COMMON_DESKTOP_PATH_TOKEN_NUM%_ %%A
    )
  )
  IF !ERRORLEVEL! NEQ 0 (
    REM reset ERRORLEVEL by using CALL;
    CALL;
  ) ELSE IF !CA_COMMON_DESKTOP_PATH_TOKEN_FOUND! NEQ 0 (
    GOTO :CA_COMMON_DESKTOP_LOOP
  )

IF DEFINED CA_COMMON_DESKTOP_PATH (
   ECHO CommmonDesktop found as '%CA_COMMON_DESKTOP_PATH%'
) ELSE (
   ECHO CommmonDesktop NOT found.
)

REM ---------------------------------
REM - Detecting package version
REM ---------------------------------
SET CA_PKG_VERSION=0.0.0
IF EXIST "%CA_BASE_PATH%version.txt" (
   set /p CA_PKG_VERSION=<"!CA_BASE_PATH!version.txt"
   set CA_PKG_VERSION=!CA_PKG_VERSION: =!
   ECHO !CA_APP_SHOW_NAME! package version: "!CA_PKG_VERSION!".
) ELSE (
   ECHO version.txt file noy found.
)

:CA_START

ECHO:
ECHO ----------
ECHO - CONTAB -
ECHO ----------

ECHO Detecting current install state ...

REM --------------------------------
REM - Current date/time
REM --------------------------------
SET CA_DATE_SQL=
for /f "skip=1 tokens=1-6 delims= " %%a in ('wmic path Win32_LocalTime Get Day^,Hour^,Minute^,Month^,Second^,Year /Format:table') do (
   IF NOT "%%~f"=="" (
      set /a CA_DATE_SQL=10000 * %%f + 100 * %%d + %%a
   )
)
ECHO Current date: %CA_DATE_SQL%

REM --------------------------------
REM - Installed files search
REM --------------------------------

SET CA_FILES_FOUND=0
IF exist "%ProgramFiles%\%CA_FOLDER_NAME%" (
  ECHO Installed folder found at "%ProgramFiles%\%CA_FOLDER_NAME%".
  SET CA_FILES_FOUND=1
) ELSE (
  reg query "HKEY_LOCAL_MACHINE\Software\Microsoft\Windows\CurrentVersion\uninstall\!CA_REGKEY_NAME!" >NUL 2>&1
  IF !ERRORLEVEL! NEQ 0 (
    REM reset ERRORLEVEL by using CALL;
    CALL;
    ECHO No installed folder/key found.
  ) else (
    ECHO Installed key found at "HKEY_LOCAL_MACHINE\Software\Microsoft\Windows\CurrentVersion\uninstall\!CA_REGKEY_NAME!".
    SET CA_FILES_FOUND=1
  )
)

REM --------------------------------
REM - Actions by input param
REM --------------------------------

if /I "%1"=="-uninstall" (
  ECHO Uninstalling...
  IF %CA_FILES_FOUND% EQU 1 (
    CALL :CA_OPTION_DELETE_FILES 1
  )
  EXIT /B 0
)

if /I "%1"=="-install" (
  ECHO Installing...
  IF %CA_FILES_FOUND% EQU 1 (
    CALL :CA_OPTION_DELETE_FILES 1
  )
  CALL :CA_OPTION_INSTALL_FILES 1
  EXIT /B 0
)

REM --------------------------------
REM - Main menu
REM --------------------------------

:CA_MENU

ECHO:
ECHO Options:
ECHO   0, Exit

SET CA_OPTIONS_MAX_IDX=0

SET CA_OPTION_INSTALL_FILES_IDX=0

IF %CA_FILES_FOUND% EQU 1 (
  (
    SET /a "CA_OPTIONS_MAX_IDX+=1"
    ECHO   !CA_OPTIONS_MAX_IDX!, Update files and prerequisites to this version.
    SET CA_OPTION_INSTALL_FILES_IDX=!CA_OPTIONS_MAX_IDX!
  )
) ELSE (
  SET /a "CA_OPTIONS_MAX_IDX+=1"
  ECHO   !CA_OPTIONS_MAX_IDX!, Install files and prerequisites.
  SET CA_OPTION_INSTALL_FILES_IDX=!CA_OPTIONS_MAX_IDX!
)

SET CA_OPTION_OPEN_APP_IDX=0
SET CA_OPTION_CLOSE_APPS_IDX=0

IF %CA_FILES_FOUND% EQU 0 (
  ECHO    , Open a visual-app instance.
  ECHO    , Close visual-app instances.
) ELSE (
  SET /a "CA_OPTIONS_MAX_IDX+=1"
  ECHO   !CA_OPTIONS_MAX_IDX!, Open a visual-app instance.
  SET CA_OPTION_OPEN_APP_IDX=!CA_OPTIONS_MAX_IDX!
  SET /a "CA_OPTIONS_MAX_IDX+=1"
  ECHO   !CA_OPTIONS_MAX_IDX!, Close visual-app instances.
  SET CA_OPTION_CLOSE_APPS_IDX=!CA_OPTIONS_MAX_IDX!
)

SET CA_OPTION_DELETE_FILES_IDX=0
IF %CA_FILES_FOUND% EQU 0 (
  ECHO    , Uninstall all and cleanup.
) ELSE (
  SET /a "CA_OPTIONS_MAX_IDX+=1"
  ECHO   !CA_OPTIONS_MAX_IDX!, Uninstall all and cleanup.
  SET CA_OPTION_DELETE_FILES_IDX=!CA_OPTIONS_MAX_IDX!
)

ECHO:
ECHO  Note: Options are enabled depending on the installation state.
ECHO  Log-file: C:\contab_manage_log.txt

ECHO:
SET /p CA_OPTION_SEL=Select an option: 

IF %CA_OPTION_SEL% EQU 0 (
  EXIT /B 0

) ELSE IF %CA_OPTION_SEL% EQU %CA_OPTION_INSTALL_FILES_IDX% (
  CALL :CA_OPTION_INSTALL_FILES 0
  GOTO :CA_START

) ELSE IF %CA_OPTION_SEL% EQU %CA_OPTION_OPEN_APP_IDX% (
  CALL :CA_OPTION_OPEN_APP 0
  GOTO :CA_START
) ELSE IF %CA_OPTION_SEL% EQU %CA_OPTION_CLOSE_APPS_IDX% (
  CALL :CA_OPTION_CLOSE_APPS 0
  GOTO :CA_START

) ELSE IF %CA_OPTION_SEL% EQU %CA_OPTION_DELETE_FILES_IDX% (
  CALL :CA_OPTION_DELETE_FILES 0
  GOTO :CA_START
)

GOTO :CA_MENU

REM --------------------------------
REM - ACTION: open app
REM --------------------------------
REM Param-%1: already-acepted(1)

:CA_OPTION_OPEN_APP
;
start /b /D "%ProgramFiles%\%CA_FOLDER_NAME%\%CA_ARCH_CONFIG%\%CA_ARCH%\" %CA_APP_EXE_NAME%
;
EXIT /B 0

REM --------------------------------
REM - ACTION: install files and prerequisites
REM --------------------------------
REM Param-%1: already-acepted(1)

:CA_OPTION_INSTALL_FILES
;
if %1 NEQ 1 (
  ECHO:
  SET /p SelConfirm="Copy files and prerequisites? (y/n): "
  if /I not "!SelConfirm!"=="y" EXIT /B 1
)
;
CALL :CA_OPTION_INSTALL_FILES_SIGNED %1
;
EXIT /B 0

REM --------------------------------
REM - ACTION: install files (signed)
REM --------------------------------
REM Param-%1: already-acepted(1)

:CA_OPTION_INSTALL_FILES_SIGNED
;
ECHO:
CALL :CA_COPY_ALL_FILES 1
IF %ERRORLEVEL% NEQ 0 (
   ECHO ERROR: Files copy failed.
   EXIT /B 1
)
;
ECHO:
ECHO Files copies to local folder.
EXIT /B 0

REM --------------------------
REM CA_COPY_ALL_FILES
REM --------------------------
REM Param-%1: already-acepted(1)

:CA_COPY_ALL_FILES
ECHO Copying files ...
REM ----
REM  folder
REM ----
CALL :CREATE_FOLDER "%ProgramFiles%\%CA_FOLDER_NAME%"
IF %ERRORLEVEL% NEQ 0 (
   ECHO ERROR: Local folder creation failed '%ProgramFiles%\%CA_FOLDER_NAME%'.
   EXIT /B 1
)
copy "%CA_BASE_PATH%manage.bat" "%ProgramFiles%\%CA_FOLDER_NAME%\" >> C:\contab_manage_log.txt
IF %ERRORLEVEL% NEQ 0 (
   ECHO ERROR: File copy failed: "manage.bat".
   EXIT /B 1
)
IF EXIST "%CA_BASE_PATH%source.json" (
   copy "!CA_BASE_PATH!source.json" "!ProgramFiles!\!CA_FOLDER_NAME!\" >> C:\contab_manage_log.txt
   IF !ERRORLEVEL! NEQ 0 (
      ECHO ERROR: File copy failed: "source.json".
      EXIT /B 1
   )
)
IF EXIST "%CA_BASE_PATH%version.txt" (
   copy "!CA_BASE_PATH!version.txt" "!ProgramFiles!\!CA_FOLDER_NAME!\" >> C:\contab_manage_log.txt
   IF !ERRORLEVEL! NEQ 0 (
      ECHO ERROR: File copy failed: "version.txt".
      EXIT /B 1
   )
)
REM ----
REM arch-cfg (release)
REM ----
CALL :CREATE_FOLDER "%ProgramFiles%\%CA_FOLDER_NAME%\%CA_ARCH_CONFIG%"
IF %ERRORLEVEL% NEQ 0 (
   ECHO ERROR: Local folder creation failed '%ProgramFiles%\%CA_FOLDER_NAME%\%CA_ARCH_CONFIG%'
   EXIT /B 1
)
REM ----
REM arch
REM ----
CALL :CREATE_FOLDER "%ProgramFiles%\%CA_FOLDER_NAME%\%CA_ARCH_CONFIG%\%CA_ARCH%"
IF %ERRORLEVEL% NEQ 0 (
   ECHO ERROR: Local folder creation failed '%ProgramFiles%\%CA_FOLDER_NAME%\%CA_ARCH_CONFIG%\%CA_ARCH%'
   EXIT /B 1
)
copy "%CA_BASE_PATH%%CA_ARCH_CONFIG%\%CA_ARCH%\contab.exe" "%ProgramFiles%\%CA_FOLDER_NAME%\%CA_ARCH_CONFIG%\%CA_ARCH%\" >> C:\contab_manage_log.txt
IF %ERRORLEVEL% NEQ 0 (
   ECHO ERROR: File copy failed: "contab.exe", close all running instances and try again.
   EXIT /B 1
)
copy "%CA_BASE_PATH%%CA_ARCH_CONFIG%\%CA_ARCH%\tunnel.dll" "%ProgramFiles%\%CA_FOLDER_NAME%\%CA_ARCH_CONFIG%\%CA_ARCH%\" >> C:\contab_manage_log.txt
IF %ERRORLEVEL% NEQ 0 (
   ECHO ERROR: File copy failed: "tunnel.dll", close all running instances and try again.
   EXIT /B 1
)
IF DEFINED CA_COMMON_PROGRAMS_PATH (
   CALL :CREATE_FOLDER "%CA_COMMON_PROGRAMS_PATH%\%CA_FOLDER_NAME%\"
   IF !ERRORLEVEL! NEQ 0 (
      ECHO ERROR: Local folder creation failed '!CA_COMMON_PROGRAMS_PATH!\!CA_FOLDER_NAME!\'
      EXIT /B 1
   )
   ECHO Creating programs shortcut ...
   ECHO Set oWS = WScript.CreateObject^("WScript.Shell"^) > "%ProgramFiles%\%CA_FOLDER_NAME%\contab-app-shortcut.vbs"
   ECHO sLinkFile = "%CA_COMMON_PROGRAMS_PATH%\%CA_FOLDER_NAME%\%CA_APP_SHORTCUT_NAME%" >> "%ProgramFiles%\%CA_FOLDER_NAME%\contab-app-shortcut.vbs"
   ECHO Set oLink = oWS.CreateShortcut^(sLinkFile^) >> "%ProgramFiles%\%CA_FOLDER_NAME%\contab-app-shortcut.vbs"
   ECHO oLink.TargetPath = "%ProgramFiles%\%CA_FOLDER_NAME%\%CA_ARCH_CONFIG%\%CA_ARCH%\%CA_APP_EXE_NAME%" >> "%ProgramFiles%\%CA_FOLDER_NAME%\contab-app-shortcut.vbs"
   ECHO oLink.Arguments = "" >> "%ProgramFiles%\%CA_FOLDER_NAME%\contab-app-shortcut.vbs"
   ECHO oLink.Save >> "%ProgramFiles%\%CA_FOLDER_NAME%\contab-app-shortcut.vbs"
   cscript "%ProgramFiles%\%CA_FOLDER_NAME%\contab-app-shortcut.vbs" >NUL 2>&1
   IF ERRORLEVEL 1 (
       ECHO ERROR: Programs shortcut creation failed.
       REM reset ERRORLEVEL by using CALL;
       CALL;
   )
   del "%ProgramFiles%\%CA_FOLDER_NAME%\contab-app-shortcut.vbs"
) ELSE (
   ECHO Programs shortcut ignored, CommonPrograms path is missing.
)
;
IF DEFINED CA_COMMON_DESKTOP_PATH (
   ECHO Creating desktop shortcut ...
   ECHO Set oWS = WScript.CreateObject^("WScript.Shell"^) > "%ProgramFiles%\%CA_FOLDER_NAME%\contab-app-shortcut.vbs"
   ECHO sLinkFile = "%CA_COMMON_DESKTOP_PATH%\%CA_APP_SHORTCUT_NAME%" >> "%ProgramFiles%\%CA_FOLDER_NAME%\contab-app-shortcut.vbs"
   ECHO Set oLink = oWS.CreateShortcut^(sLinkFile^) >> "%ProgramFiles%\%CA_FOLDER_NAME%\contab-app-shortcut.vbs"
   ECHO oLink.TargetPath = "%ProgramFiles%\%CA_FOLDER_NAME%\%CA_ARCH_CONFIG%\%CA_ARCH%\%CA_APP_EXE_NAME%" >> "%ProgramFiles%\%CA_FOLDER_NAME%\contab-app-shortcut.vbs"
   ECHO oLink.Arguments = "" >> "%ProgramFiles%\%CA_FOLDER_NAME%\contab-app-shortcut.vbs"
   ECHO oLink.Save >> "%ProgramFiles%\%CA_FOLDER_NAME%\contab-app-shortcut.vbs"
   cscript "%ProgramFiles%\%CA_FOLDER_NAME%\contab-app-shortcut.vbs" >NUL 2>&1
   IF ERRORLEVEL 1 (
       ECHO ERROR: Desktop shortcut creation failed.
       REM reset ERRORLEVEL by using CALL;
       CALL;
   )
   del "%ProgramFiles%\%CA_FOLDER_NAME%\contab-app-shortcut.vbs"
) ELSE (
   ECHO Desktop shortcut ignored, CommonDesktop path is missing.
)
;
REM ----
REM Calculate folders size
REM ----
set CA_INSTALLED_SIZE=0
set CA_INSTALLED_SIZE_KB=0
for /f "tokens=*" %%x in ('dir /s /a /b "%ProgramFiles%\%CA_FOLDER_NAME%\"') do set /a CA_INSTALLED_SIZE+=%%~zx
ECHO %CA_INSTALLED_SIZE% bytes installed.
set /a CA_INSTALLED_SIZE_KB= %CA_INSTALLED_SIZE%/1024
;
REM ----
REM Registry
REM ----
reg query "HKEY_LOCAL_MACHINE\Software\Microsoft\Windows\CurrentVersion\uninstall\%CA_REGKEY_NAME%" >NUL 2>&1
IF %ERRORLEVEL% NEQ 0 (
   REM reset ERRORLEVEL by using CALL;
   CALL;
) else (
   reg delete "HKEY_LOCAL_MACHINE\Software\Microsoft\Windows\CurrentVersion\uninstall\!CA_REGKEY_NAME!" /f >> C:\contab_manage_log.txt
   IF !ERRORLEVEL! NEQ 0 (
      ECHO ERROR: Registry key deletion failed."
      PAUSE
      EXIT /B 1
   ) else (
      ECHO Registry key deleted.
   )
)
reg add "HKEY_LOCAL_MACHINE\Software\Microsoft\Windows\CurrentVersion\uninstall\%CA_REGKEY_NAME%" >> C:\contab_manage_log.txt
IF %ERRORLEVEL% NEQ 0 (
   ECHO ERROR: Registry key creation failed."
   EXIT /B 1
) else (
   ECHO Registry key created
)
reg add "HKEY_LOCAL_MACHINE\Software\Microsoft\Windows\CurrentVersion\uninstall\%CA_REGKEY_NAME%" /v DisplayIcon /t REG_SZ /d "\"%ProgramFiles%\%CA_FOLDER_NAME%\%CA_ARCH_CONFIG%\%CA_ARCH%\%CA_APP_EXE_NAME%\"" >> C:\contab_manage_log.txt
IF %ERRORLEVEL% NEQ 0 (
   ECHO ERROR: Registry 'DisplayIcon' value creation failed."
   EXIT /B 1
)
reg add "HKEY_LOCAL_MACHINE\Software\Microsoft\Windows\CurrentVersion\uninstall\%CA_REGKEY_NAME%" /v DisplayName /t REG_SZ /d "%CA_APP_SHOW_NAME%" >> C:\contab_manage_log.txt
IF %ERRORLEVEL% NEQ 0 (
   ECHO ERROR: Registry 'DisplayName' value creation failed."
   EXIT /B 1
)
IF DEFINED CA_PKG_VERSION (
   reg add "HKEY_LOCAL_MACHINE\Software\Microsoft\Windows\CurrentVersion\uninstall\!CA_REGKEY_NAME!" /v DisplayVersion /t REG_SZ /d !CA_PKG_VERSION! >> C:\contab_manage_log.txt
   IF !ERRORLEVEL! NEQ 0 (
      ECHO ERROR: Registry 'DisplayVersion' value creation failed."
      EXIT /B 1
   )
)
IF %CA_INSTALLED_SIZE_KB% NEQ 0 (
   reg add "HKEY_LOCAL_MACHINE\Software\Microsoft\Windows\CurrentVersion\uninstall\!CA_REGKEY_NAME!" /v EstimatedSize /t REG_DWORD /d !CA_INSTALLED_SIZE_KB! >> C:\contab_manage_log.txt
   IF !ERRORLEVEL! NEQ 0 (
      ECHO ERROR: Registry 'EstimatedSize' value creation failed."
      EXIT /B 1
   )
)
IF DEFINED CA_DATE_SQL (
   reg add "HKEY_LOCAL_MACHINE\Software\Microsoft\Windows\CurrentVersion\uninstall\!CA_REGKEY_NAME!" /v InstallDate /t REG_SZ /d !CA_DATE_SQL! >> C:\contab_manage_log.txt
   IF !ERRORLEVEL! NEQ 0 (
      ECHO ERROR: Registry 'InstallDate' value creation failed."
      EXIT /B 1
   )
)
reg add "HKEY_LOCAL_MACHINE\Software\Microsoft\Windows\CurrentVersion\uninstall\%CA_REGKEY_NAME%" /v InstallLocation /t REG_SZ /d "\"%ProgramFiles%\%CA_FOLDER_NAME%\"" >> C:\contab_manage_log.txt
IF %ERRORLEVEL% NEQ 0 (
   ECHO ERROR: Registry 'InstallLocation' value creation failed."
   EXIT /B 1
)
reg add "HKEY_LOCAL_MACHINE\Software\Microsoft\Windows\CurrentVersion\uninstall\%CA_REGKEY_NAME%" /v ModifyPath /t REG_SZ /d "\"%ProgramFiles%\%CA_FOLDER_NAME%\manage.bat\"" >> C:\contab_manage_log.txt
IF %ERRORLEVEL% NEQ 0 (
   ECHO ERROR: Registry 'ModifyPath' value creation failed."
   EXIT /B 1
)
reg add "HKEY_LOCAL_MACHINE\Software\Microsoft\Windows\CurrentVersion\uninstall\%CA_REGKEY_NAME%" /v NoRepair /t REG_DWORD /d 1 >> C:\contab_manage_log.txt
IF %ERRORLEVEL% NEQ 0 (
   ECHO ERROR: Registry 'NoRepair' value creation failed."
   EXIT /B 1
)
reg add "HKEY_LOCAL_MACHINE\Software\Microsoft\Windows\CurrentVersion\uninstall\%CA_REGKEY_NAME%" /v Publisher /t REG_SZ /d %CA_APP_MANUFACTURER_NAME% >> C:\contab_manage_log.txt
IF %ERRORLEVEL% NEQ 0 (
   ECHO ERROR: Registry 'Publisher' value creation failed."
   EXIT /B 1
)
reg add "HKEY_LOCAL_MACHINE\Software\Microsoft\Windows\CurrentVersion\uninstall\%CA_REGKEY_NAME%" /v UninstallString /t REG_SZ /d "\"%ProgramFiles%\%CA_FOLDER_NAME%\manage.bat\" -uninstall" >> C:\contab_manage_log.txt
IF %ERRORLEVEL% NEQ 0 (
   ECHO ERROR: Registry 'UninstallString' value creation failed."
   EXIT /B 1
)
ECHO End of files copy.
EXIT /B 0

REM --------------------------
REM CREATE_FOLDER
REM --------------------------
; Param-%1: folder-path
:CREATE_FOLDER
if not exist %1\ (
    ECHO Creating folder '%1'...
    md %1 >> C:\contab_manage_log.txt
    EXIT /B !ERRORLEVEL!
)
EXIT /B 0

REM --------------------------------
REM - ACTION: close apps
REM --------------------------------
REM Param-%1: already-acepted(1)

:CA_OPTION_CLOSE_APPS
;
if %1 NEQ 1 (
  ECHO:
  SET /p SelConfirm="Close all instances of the visual-app? (y/n): "
  if /I not "!SelConfirm!"=="y" EXIT /B 1
)
;
ECHO Terminating processes '%CA_APP_EXE_NAME%'...
taskkill /F /IM %CA_APP_EXE_NAME% >NULL 2>&1
;
ECHO Terminating processes '%CA_APP_EXE_NAME_NO_EXT%'...
taskkill /F /IM %CA_APP_EXE_NAME_NO_EXT% >NULL 2>&1
;
ECHO Terminate-commands sent.
;
EXIT /B 0

REM --------------------------------
REM - ACTION: delete files
REM --------------------------------
REM Param-%1: already-acepted(1)

:CA_OPTION_DELETE_FILES
;
if %1 NEQ 1 (
  ECHO:
  SET /p SelConfirm="Uninstall all and clean files? (y/n): "
  if /I not "!SelConfirm!"=="y" EXIT /B 1
)
;
REM Close apps
CALL :CA_OPTION_CLOSE_APPS 1
;
IF DEFINED CA_COMMON_PROGRAMS_PATH (
   IF EXIST "!CA_COMMON_PROGRAMS_PATH!\!CA_FOLDER_NAME!" (
      rmdir "!CA_COMMON_PROGRAMS_PATH!\!CA_FOLDER_NAME!" /q /s
      IF !ERRORLEVEL! NEQ 0 (
         ECHO ERROR: Folder removal failed: "!CA_COMMON_PROGRAMS_PATH!\!CA_FOLDER_NAME!"
         REM reset ERRORLEVEL by using CALL;
         CALL;
      ) else (
         ECHO Folder removed: "!CA_COMMON_PROGRAMS_PATH!\!CA_FOLDER_NAME!"
      )
   )
) ELSE (
   ECHO Programs shotcut ignored, CommonPrograms path is missing.
)
;
IF DEFINED CA_COMMON_DESKTOP_PATH (
   IF EXIST "!CA_COMMON_DESKTOP_PATH!\!CA_APP_SHORTCUT_NAME!" (
      del "!CA_COMMON_DESKTOP_PATH!\!CA_APP_SHORTCUT_NAME!"
      IF !ERRORLEVEL! NEQ 0 (
         ECHO ERROR: Desktop link removal failed: "!CA_COMMON_DESKTOP_PATH!\!CA_APP_SHORTCUT_NAME!"
         REM reset ERRORLEVEL by using CALL;
         CALL;
      ) else (
         ECHO Desktop link removed: "!CA_COMMON_DESKTOP_PATH!\!CA_APP_SHORTCUT_NAME!"
      )
   )
) ELSE (
   ECHO Desktop link ignored, CommonDesktop path is missing.
)
;
reg query "HKEY_LOCAL_MACHINE\Software\Microsoft\Windows\CurrentVersion\uninstall\%CA_REGKEY_NAME%" >NUL 2>&1
IF %ERRORLEVEL% NEQ 0 (
   REM reset ERRORLEVEL by using CALL;
   CALL;
) else (
   reg delete "HKEY_LOCAL_MACHINE\Software\Microsoft\Windows\CurrentVersion\uninstall\!CA_REGKEY_NAME!" /f >> C:\contab_manage_log.txt
   IF !ERRORLEVEL! NEQ 0 (
      ECHO ERROR: Registry key deletion failed."
      PAUSE
      EXIT /B 1
   ) else (
      ECHO Registry key deleted.
   )
)
;
IF EXIST "%ProgramFiles%\%CA_FOLDER_NAME%" (
  rmdir "!ProgramFiles!\!CA_FOLDER_NAME!" /q /s
  IF !ERRORLEVEL! NEQ 0 (
    ECHO ERROR: Folder removal failed: "!ProgramFiles!\!CA_FOLDER_NAME!"
    ECHO Close all running instances and try again.
    PAUSE
    EXIT /B 1
  ) else (
    ECHO Folder removed: "!ProgramFiles!\!CA_FOLDER_NAME!"
  )
)
;
EXIT /B 0

