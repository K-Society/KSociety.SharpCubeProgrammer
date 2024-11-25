@setlocal
@pushd %~dp0
@set _C=Release
@set _D=x64

@if "%VCToolsVersion%"=="" call :StartDeveloperCommandPrompt || exit /b

msbuild KSociety.SharpCubeProgrammer.sln -t:restore -p:RestorePackagesConfig=true -p:Configuration=%_C% -p:Platform=%_D% || exit /b

msbuild KSociety.SharpCubeProgrammer.sln -t:build -p:Configuration=%_C% -p:Platform=%_D% || exit /b

msbuild KSociety.SharpCubeProgrammer.sln -t:clean || exit /b

@set _D=x86

msbuild KSociety.SharpCubeProgrammer.sln -t:restore -p:RestorePackagesConfig=true -p:Configuration=%_C% -p:Platform=%_D% || exit /b

msbuild KSociety.SharpCubeProgrammer.sln -t:build -p:Configuration=%_C% -p:Platform=%_D% || exit /b

@set _D=AnyCPU

msbuild src\01\KSociety.SharpCubeProgrammer\KSociety.SharpCubeProgrammer.csproj -t:restore -p:Configuration=%_C% -p:Platform=%_D% || exit /b

msbuild src\01\KSociety.SharpCubeProgrammer\KSociety.SharpCubeProgrammer.csproj -t:build -p:Configuration=%_C% -p:Platform=%_D% || exit /b

goto LExit

:StartDeveloperCommandPrompt

echo Initializing developer command prompt

if not exist "%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe" (
  "%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe"
  exit /b 2
)

for /f "usebackq delims=" %%i in (`"%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe" -version [17.0^,18.0^) -property installationPath`) do (
  if exist "%%i\Common7\Tools\vsdevcmd.bat" (
    call "%%i\Common7\Tools\vsdevcmd.bat" -no_logo
    exit /b
  )
  echo developer command prompt not found in %%i
)

echo No versions of developer command prompt found
exit /b 2

:LExit
@popd
@endlocal
