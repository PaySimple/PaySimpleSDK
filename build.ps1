gci .\src -Recurse "packages.config" |% {
	"Restoring " + $_.FullName
	.\src\.nuget\nuget.exe i $_.FullName -o .\src\packages
}

Import-Module .\src\packages\psake.4.4.2\tools\psake.psm1
Invoke-Psake .\buildtasks.ps1 -framework "4.6x64" 
Remove-Module psake