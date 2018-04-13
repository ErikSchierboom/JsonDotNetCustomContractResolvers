<#
.Synopsis
    Replace project files with dotnet core format files.  Examples are not
    updated in order to demonstrate .net framework compatiblility.
#>

Set-Location $PSScriptRoot

# Remove project files that will be replaced
'.\CustomContractResolvers.sln',
'.\src\CustomContractResolvers\CustomContractResolvers.csproj',
'.\src\CustomContractResolvers\packages.config',
'.\tests\CustomContractResolvers.Tests\CustomContractResolvers.Tests.csproj',
'.\tests\CustomContractResolvers.Tests\packages.config' |
Remove-Item

dotnet new sln --name CustomContractResolvers

dotnet new library -o .\src\CustomContractResolvers
Remove-Item .\src\CustomContractResolvers\Class1.cs
dotnet add .\src\CustomContractResolvers\CustomContractResolvers.csproj package Newtonsoft.Json
dotnet sln .\CustomContractResolvers.sln add .\src\CustomContractResolvers\CustomContractResolvers.csproj

dotnet new xunit -o .\tests\CustomContractResolvers.Tests
Remove-Item .\tests\CustomContractResolvers.Tests\UnitTest1.cs
'Newtonsoft.Json', 'coveralls.net', 'OpenCover' |
ForEach-Object {
    dotnet add .\tests\CustomContractResolvers.Tests\CustomContractResolvers.Tests.csproj package $_
}
dotnet add .\tests\CustomContractResolvers.Tests reference .\src\CustomContractResolvers\CustomContractResolvers.csproj

'.\tests\CustomContractResolvers.Tests\CustomContractResolvers.Tests.csproj',
'.\samples\CustomContractResolvers.Samples.Console\CustomContractResolvers.Samples.Console.csproj',
'.\samples\CustomContractResolvers.Samples.Website\CustomContractResolvers.Samples.Website.csproj' |
ForEach-Object {
    dotnet sln .\CustomContractResolvers.sln add $_
}

git add .
git commit -m 'Run script'
