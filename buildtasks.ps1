properties {
	$base_directory = Resolve-Path .
	$src_directory = "$base_directory\src"
	$output_directory = "$base_directory\build"
	$dist_directory = "$base_directory\distribution"
	$sln_file = "$src_directory\PaySimpleSdk.sln"
	$target_config = "Release"
	$framework_version = "v4.5"
	$xunit_path = "$src_directory\packages\xunit.runner.console.2.1.0\tools\xunit.console.exe"	
	$nuget_path = "$src_directory\.nuget\nuget.exe"

	$version = "1.4.3"
	$preRelease = ""
}

task default -depends Clean, RunTests, CreateNuGetPackage

task Compile {
	exec { msbuild /nologo /verbosity:quiet $sln_file /p:Configuration=$target_config /p:TargetFrameworkVersion=v4.5 }
	mkdir $output_directory -ea SilentlyContinue > $null
	copy-item $src_directory\PaySimpleSdk\bin\Release\PaySimpleSdk.dll $output_directory	
}

task Clean {
	rmdir $output_directory -ea SilentlyContinue -recurse > $null
	rmdir $dist_directory -ea SilentlyContinue -recurse > $null
	exec { msbuild /nologo /verbosity:quiet $sln_file /p:Configuration=$target_config /t:Clean }
}

task RunTests -depends Compile {
	$project = "PaySimpleSdkTests"
	mkdir $output_directory\xunit\$project -ea SilentlyContinue > $null
	.$xunit_path "$src_directory\PaySimpleSdkTests\bin\Release\$project.dll"
}

task CreateNuGetPackage -depends Compile {	
	$vSplit = $version.Split('.')
		
	if($vSplit.Length -ne 3) {
		throw "Version number is invalid. Must be in the form of 0.0.0"
	}	

	$major = $vSplit[0]
	$minor = $vSplit[1]
	$patch = $vSplit[2]
	$packageVersion =  "$major.$minor.$patch"	

	if($preRelease) {
		$packageVersion = $packageVersion + "-" + $preRelease	
	}
			
	mkdir $dist_directory -ea SilentlyContinue > $null
	mkdir $dist_directory\lib\net45 -ea SilentlyContinue > $null
	copy-item $src_directory\PaySimpleSdk.nuspec $dist_directory -ea SilentlyContinue > $null
	copy-item $output_directory\PaySimpleSdk.dll $dist_directory\lib\net45\ -ea SilentlyContinue > $null
	exec { . $nuget_path pack $dist_directory\PaySimpleSdk.nuspec -BasePath $dist_directory -o $dist_directory -version $packageVersion }	
}
