dotnet add tests/CustomContractResolvers.Tests package coverlet.msbuild
dotnet tool install coveralls.net --version 1.0.0 --tool-path ./tools/coveralls.net
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover

./tools/coveralls.net/csmacnz.Coveralls --opencover -i tests/CustomContractResolvers.Tests/coverage.opencover.xml