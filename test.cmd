@REM Uncomment this line to update nuget.exe
@REM Doing so can break SLN build (which uses nuget.exe to
@REM create a nuget package for binskim) so must opt-in
@REM %~dp0.nuget\NuGet.exe update -self

%~dp0.nuget\NuGet.exe restore src\Driver.Utilities.sln 
msbuild /verbosity:minimal /target:rebuild src\Driver.Utilities.sln /p:Configuration=Release

