@echo off

SET CA_OUTPUT_FOLDER=.

SET CA_SIGNTOOL_PATH=C:\Program Files (x86)\Windows Kits\10\bin\10.0.22000.0\x64\

SET CA_SIGN_NAME=

REM A certificate and private-key issued to CA_SIGN_NAME must be in the current user's local-cerstore.

REM -------------------
REM - Parse input params
REM -------------------

REM -prjConfig "Release" -srcChannel "release" -ver "1.8.9"
SET CA_PRJ_CONFIG=Release
SET CA_SRC_CHANNEL=release
SET CA_VERSION=

:PARSE_PARAMS
IF "%~1"=="" GOTO :PARSE_PARAMS_END
IF "%~1"=="-prjConfig" (
    IF "%~2"=="" GOTO :PARSE_PARAMS_END
    SET CA_PRJ_CONFIG=%~2
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
IF "%CA_SRC_CHANNEL%"=="" (
   CALL :MISING_PARAMS_MSG
   EXIT /B -1
)
IF "%CA_VERSION%"=="" (
   CALL :MISING_PARAMS_MSG
   EXIT /B -1
)

REM -------------------
REM - Copy files
REM -------------------
CALL :CREATE_ARCH amd64 x64 x64 x64 %CA_SRC_CHANNEL% %CA_VERSION%
IF %ERRORLEVEL% NEQ 0 EXIT /B %ERRORLEVEL%
EXIT /B %ERRORLEVEL%

REM
:MISING_PARAMS_MSG
ECHO Required params are missing.
ECHO CA_PRJ_CONFIG=%CA_PRJ_CONFIG%
ECHO CA_SRC_CHANNEL=%CA_SRC_CHANNEL%
ECHO CA_VERSION=%CA_VERSION%
ECHO Expected example: build-installer.bat -prjConfig "Release" -srcChannel "release" -ver "1.8.9"
EXIT /B 0

REM Param "%1": installer-arch (x86, amd64, arm, arm64, ... as defined by 'PROCESSOR_ARCHITEW6432' and 'PROCESSOR_ARCHITECTURE').
REM Param "%2": vs-compiled-arch (x86, x64, ARM64, ...).
REM Param "%3": vs-dot-net-compiled-arch (x86, x64, AnyCPU, ...).
REM Param "%4": vs-dot-net-dll-arch (x86, x64, AnyCPU, ...).
REM Param "%5": src-channel ("release", "debug" or any other configurated on the download server)
REM Param "%6": src-version ("x.x.x" according to configurated on the download server)
REM
:CREATE_ARCH
CALL :CREATE_ARCH_CFG %CA_PRJ_CONFIG% %1 %2 %3 %4 %5 %6
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
REM Param "%6": src-channel ("release", "debug" or any other configurated on the download server)
REM Param "%7": src-version ("x.x.x" according to configurated on the download server)
REM
:CREATE_ARCH_CFG
CALL :COPY_AND_SIGN_FILE %~d0%~p0..\..\projects\visual-studio\Contab-installer\bin\win\%1\%4\contab-installer.exe \ contab-installer-%6-%7-%2.exe
IF %ERRORLEVEL% NEQ 0 EXIT /B %ERRORLEVEL%
EXIT /B 0
