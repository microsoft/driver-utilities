SETLOCAL
@REM Uncomment this line to update nuget.exe
@REM Doing so can break SLN build (which uses nuget.exe to
@REM create a nuget package for binskim) so must opt-in
@REM %~dp0.nuget\NuGet.exe update -self

set MAJOR=1
set MINOR=0
set PATCH=3
set PRERELEASE=-beta

set VERSION_CONSTANTS=src\Driver.Utilities\VersionConstants.cs

rd /s /q bld

@REM Rewrite VersionConstants.cs
echo // Copyright (c) Microsoft. All rights reserved. Licensed under the MIT         > %VERSION_CONSTANTS%
echo // license. See LICENSE file in the project root for full license information. >> %VERSION_CONSTANTS%
echo namespace Microsoft.CodeAnalysis.Sql                                           >> %VERSION_CONSTANTS%
echo {                                                                              >> %VERSION_CONSTANTS%
echo     public static class VersionConstants                                       >> %VERSION_CONSTANTS%
echo     {                                                                          >> %VERSION_CONSTANTS%
echo         public const string Prerelease = "%PRERELEASE%";                       >> %VERSION_CONSTANTS%
echo         public const string AssemblyVersion = "%MAJOR%.%MINOR%.%PATCH%";       >> %VERSION_CONSTANTS%
echo         public const string FileVersion = AssemblyVersion + ".0";              >> %VERSION_CONSTANTS%
echo         public const string Version = AssemblyVersion + Prerelease;            >> %VERSION_CONSTANTS%
echo     }                                                                          >> %VERSION_CONSTANTS%
echo  }                                                                             >> %VERSION_CONSTANTS%

%~dp0.nuget\NuGet.exe restore src\Driver.Utilities.sln 
msbuild /verbosity:minimal /target:rebuild src\Driver.Utilities.sln /p:Configuration=Release

md bld\bin\nuget

.nuget\NuGet.exe pack .\src\Nuget\Driver.Utilities.nuspec -Symbols -Properties id=Driver.Utilities;major=%MAJOR%;minor=%MINOR%;patch=%PATCH%;prerelease=%PRERELEASE% -Verbosity Quiet -BasePath .\bld\bin\Driver.Utilities -OutputDirectory .\bld\bin\Nuget



