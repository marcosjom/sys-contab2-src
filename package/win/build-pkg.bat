@echo off

SET CA_OUTPUT_FOLDER=contab-win

SET CA_SIGNTOOL_PATH=C:\Program Files (x86)\Windows Kits\10\bin\10.0.22000.0\x64\

SET CA_SIGN_NAME=

SET CA_PROJECT_SRC_ROOT_FOLDER=%~d0%~p0..\..\

REM A certificate and private-key issued to CA_SIGN_NAME must be in the current user's local-cerstore.


REM -------------------
REM - Parse input params
REM -------------------
REM -srcProtocol "http://" -srcServer "mortegam.com" -srcPort 80 -srcPath "/sys/contab/" -srcChannel "release" -ver "0.1.0" -prjConfig "Release" -installerDefaultUrl "http://custom-installer-client.com/contab/"

SET CA_INSTALL_DEF_URL=
SET CA_PRJ_CONFIG=Release
SET CA_SRC_PROTOCOL=http://
SET CA_SRC_SERVER=mortegam.com
SET CA_SRC_PORT=80
SET CA_SRC_PATH=/sys/contab/
SET CA_SRC_CHANNEL=release
SET CA_VERSION=

:PARSE_PARAMS
IF "%~1"=="" GOTO :PARSE_PARAMS_END
IF "%~1"=="-installerDefaultUrl" (
    IF "%~2"=="" GOTO :PARSE_PARAMS_END
    SET CA_INSTALL_DEF_URL=%~2
)
IF "%~1"=="-prjConfig" (
    IF "%~2"=="" GOTO :PARSE_PARAMS_END
    SET CA_PRJ_CONFIG=%~2
)
IF "%~1"=="-srcProtocol" (
    IF "%~2"=="" GOTO :PARSE_PARAMS_END
    SET CA_SRC_PROTOCOL=%~2
)
IF "%~1"=="-srcServer" (
    IF "%~2"=="" GOTO :PARSE_PARAMS_END
    SET CA_SRC_SERVER=%~2
)
IF "%~1"=="-srcPort" (
    IF "%~2"=="" GOTO :PARSE_PARAMS_END
    SET CA_SRC_PORT=%~2
)
IF "%~1"=="-srcPath" (
    IF "%~2"=="" GOTO :PARSE_PARAMS_END
    SET CA_SRC_PATH=%~2
)
IF "%~1"=="-srcChannel" (
    IF "%~2"=="" GOTO :PARSE_PARAMS_END
    SET CA_SRC_CHANNEL=%~2
)
IF "%~1"=="-ver" (
    IF "%~2"=="" GOTO :PARSE_PARAMS_END
    SET CA_VERSION=%~2
)
SHIFT
GOTO PARSE_PARAMS
:PARSE_PARAMS_END

REM -------------------
REM - Validate input params
REM -------------------
IF "%CA_PRJ_CONFIG%"=="" (
   CALL :MISING_PARAMS_MSG
   EXIT /B -1
)
IF "%CA_SRC_PROTOCOL%"=="" (
   CALL :MISING_PARAMS_MSG
   EXIT /B -1
)
IF "%CA_SRC_SERVER%"=="" (
   CALL :MISING_PARAMS_MSG
   EXIT /B -1
)
IF "%CA_SRC_PORT%"=="" (
   CALL :MISING_PARAMS_MSG
   EXIT /B -1
)
IF "%CA_SRC_PATH%"=="" (
   CALL :MISING_PARAMS_MSG
   EXIT /B -1
)
IF "%CA_SRC_CHANNEL%"=="" (
   CALL :MISING_PARAMS_MSG
   EXIT /B -1
)
IF "%CA_VERSION%"=="" (
   CALL :MISING_PARAMS_MSG
   EXIT /B -1
)

SET CA_OUTPUT_FOLDER=contab-win-%CA_SRC_CHANNEL%-%CA_VERSION%

REM -------------------
REM - Copy files
REM -------------------
CALL :CREATE_FOLDER %CA_OUTPUT_FOLDER%
IF %ERRORLEVEL% NEQ 0 EXIT /B %ERRORLEVEL%
CALL :CREATE_ARCH amd64 x64 x64 x64
IF %ERRORLEVEL% NEQ 0 EXIT /B %ERRORLEVEL%

ECHO %CA_VERSION% > %CA_OUTPUT_FOLDER%\version.txt
IF %ERRORLEVEL% NEQ 0 (
    ECHO Failed to write "%CA_OUTPUT_FOLDER%\version.txt".
    EXIT /B %ERRORLEVEL%
)

ECHO { "version": "%CA_VERSION%", "channel": "%CA_SRC_CHANNEL%", "protocol": "%CA_SRC_PROTOCOL%", "server": "%CA_SRC_SERVER%", "port": %CA_SRC_PORT%, "path": "%CA_SRC_PATH%" } > %CA_OUTPUT_FOLDER%\source.json
IF %ERRORLEVEL% NEQ 0 (
    ECHO Failed to write "%CA_OUTPUT_FOLDER%\source.json".
    EXIT /B %ERRORLEVEL%
)

powershell Compress-Archive -Force "%CA_OUTPUT_FOLDER%" "%CA_OUTPUT_FOLDER%.zip"
IF %ERRORLEVEL% NEQ 0 EXIT /B %ERRORLEVEL%

copy "%CA_OUTPUT_FOLDER%.zip" "%CA_PROJECT_SRC_ROOT_FOLDER%projects\visual-studio\Contab-installer\Resources\src-pkg.zip" > NUL
IF %ERRORLEVEL% NEQ 0 (
    ECHO Failed to copy "%CA_OUTPUT_FOLDER%.zip" to "%CA_PROJECT_SRC_ROOT_FOLDER%projects\visual-studio\Contab-installer\Resources\src-pkg.zip"
    EXIT /B %ERRORLEVEL%
)

ECHO { "srcProtocol":"%CA_SRC_PROTOCOL%", "srcServer":"%CA_SRC_SERVER%", "srcPort":%CA_SRC_PORT%, "srcPath":"%CA_SRC_PATH%", "srcChannel":"%CA_SRC_CHANNEL%", "ver":"%CA_VERSION%", "defServerUrl":"%CA_INSTALL_DEF_URL%" } > %CA_PROJECT_SRC_ROOT_FOLDER%projects\visual-studio\Contab-installer\Resources\src-head.json
IF %ERRORLEVEL% NEQ 0 (
    ECHO Failed to write "%CA_PROJECT_SRC_ROOT_FOLDER%projects\visual-studio\Contab-installer\Resources\src-head.json".
    EXIT /B %ERRORLEVEL%
)

EXIT /B %ERRORLEVEL%

REM
:MISING_PARAMS_MSG
ECHO Required params are missing.
ECHO CA_PRJ_CONFIG=%CA_PRJ_CONFIG%
ECHO CA_SRC_PROTOCOL=%CA_SRC_PROTOCOL%
ECHO CA_SRC_SERVER=%CA_SRC_SERVER%
ECHO CA_SRC_PORT=%CA_SRC_PORT%
ECHO CA_SRC_PATH=%CA_SRC_PATH%
ECHO CA_SRC_CHANNEL=%CA_SRC_CHANNEL%
ECHO CA_VERSION=%CA_VERSION%
ECHO Expected example: build-pkg.bat -prjConfig "Release" -srcProtocol "http://" -srcServer "mortegam.com" -srcPort 80 -srcPath "/sys/contab/" -srcChannel "release" -ver "0.1.0" -installerDefaultUrl "http://custom-installer-client.com/contab/"
EXIT /B 0

REM Param "%1": folder name.
REM
:CREATE_FOLDER
if not exist "%1\" (
    md "%1" > NUL
    IF ERRORLEVEL 1 (
       ECHO ERROR: creacion de carpeta ha fallado '%1'
       EXIT /B 1
    )
)
EXIT /B 0

REM Param "%1": installer-arch (x86, amd64, arm, arm64, ... as defined by 'PROCESSOR_ARCHITEW6432' and 'PROCESSOR_ARCHITECTURE').
REM Param "%2": vs-compiled-arch (x86, x64, ARM64, ...).
REM Param "%3": vs-dot-net-compiled-arch (x86, x64, AnyCPU, ...).
REM Param "%4": vs-dot-net-dll-arch (x86, x64, AnyCPU, ...).
REM
:CREATE_ARCH
CALL :CREATE_ARCH_CFG %CA_PRJ_CONFIG%, %1 %2 %3 %4
IF %ERRORLEVEL% NEQ 0 EXIT /B %ERRORLEVEL%
EXIT /B 0

REM Param "%1": source path
REM Param "%2": folder path
REM Param "%3": filename
:COPY_FILE
copy %1 %CA_OUTPUT_FOLDER%%2%3 > NUL
IF ERRORLEVEL 1 (
   ECHO ERROR: copia de archivo ha fallado, de '%1' hacia '%CA_OUTPUT_FOLDER%%2%3'
   EXIT /B 1
)
EXIT /B 0

REM Param "%1": source file.
REM Param "%2": folder path
REM Param "%3": filename
:COPY_AND_SIGN_FILE
copy %1 %CA_OUTPUT_FOLDER%%2%3 > NUL
IF ERRORLEVEL 1 (
   ECHO ERROR: copia de archivo ha fallado, de '%1' hacia '%CA_OUTPUT_FOLDER%%2%3'
   EXIT /B 1
)
REM "%CA_SIGNTOOL_PATH%signtool.exe" sign /fd SHA256 /td SHA256 /a /n "%CA_SIGN_NAME%" /tr http://timestamp.digicert.com %CA_OUTPUT_FOLDER%%2%3
REM IF ERRORLEVEL 1 (
REM    ECHO ERROR: firma de archivo copiado ha fallado, de '%1' hacia '%CA_OUTPUT_FOLDER%%2%3'
REM    EXIT /B 1
REM )
EXIT /B 0


REM Param "%1": arch-config (Debug, Release, ...).
REM Param "%2": installer-arch (x86, amd64, arm, arm64, ... as defined by 'PROCESSOR_ARCHITEW6432' and 'PROCESSOR_ARCHITECTURE').
REM Param "%3": vs-compiled-arch (x86, x64, ARM64, ...).
REM Param "%4": vs-dot-net-compiled-arch (x86, x64, AnyCPU, ...).
REM Param "%5": vs-dot-net-dll-arch (x86, x64, AnyCPU, ...).
REM
:CREATE_ARCH_CFG
CALL :CREATE_FOLDER %CA_OUTPUT_FOLDER%\%1
IF %ERRORLEVEL% NEQ 0 EXIT /B %ERRORLEVEL%
CALL :CREATE_FOLDER %CA_OUTPUT_FOLDER%\%1\%2
IF %ERRORLEVEL% NEQ 0 EXIT /B %ERRORLEVEL%
CALL :COPY_FILE %~d0%~p0manage.bat \ manage.bat
IF %ERRORLEVEL% NEQ 0 EXIT /B %ERRORLEVEL%
CALL :COPY_AND_SIGN_FILE %~d0%~p0..\..\projects\visual-studio\Contab\bin\win\%1\%4\contab.exe \%1\%2\ contab.exe
IF %ERRORLEVEL% NEQ 0 EXIT /B %ERRORLEVEL%
CALL :COPY_AND_SIGN_FILE %~d0%~p0..\..\projects\visual-studio\Contab\bin\win\%1\%4\tunnel.dll \%1\%2\ tunnel.dll
IF %ERRORLEVEL% NEQ 0 EXIT /B %ERRORLEVEL%EXIT /B 0
