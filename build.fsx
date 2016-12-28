#r "tools/FAKE/tools/FakeLib.dll"
open Fake
open Fake.AssemblyInfoFile

// Properties
let toolsDir = "./tools"
let buildDir = "./build"
let artifactsDir = "./artifacts"
let packagingDir = "./packaging"

let releaseNotesFile = "./ReleaseNotes.md"
let projectName = "Gaucho.Forms"
let solutionAssemblyInfo = "./AssemblyInfo.cs"
let nuspecFile = "./Gaucho.Forms.nuspec"
let releaseNotes =
    ReadFile releaseNotesFile
    |> ReleaseNotesHelper.parseReleaseNotes

// Target
Target "Clean" (fun _ ->
    CleanDir packagingDir
    CleanDir buildDir
)

Target "RestorePackages" (fun _ ->
    RestorePackages()
)

Target "BuildRelease" (fun _ ->
    MSBuildRelease buildDir "Build" !! ("Gaucho.Forms*/*.csproj")
        |> Log "ReleaseBuild:"
)

Target "AssemblyInfo" (fun _ -> 
    CreateCSharpAssemblyInfo solutionAssemblyInfo
            [Attribute.Product projectName
             Attribute.Version releaseNotes.AssemblyVersion
             Attribute.FileVersion releaseNotes.AssemblyVersion]
)

Target "CreateNugetPackages" (fun _ -> 
    CreateDir artifactsDir
    CopyFiles packagingDir !! (buildDir + "/Gaucho.Forms.*.dll")
    NuGet (fun p -> 
        {p with
            Project = projectName
            Version = releaseNotes.NugetVersion
            Publish = false
            OutputPath = artifactsDir
            WorkingDir = packagingDir
            Files= [
                    ("Gaucho.Forms.*", Some "lib\\portable-net45+netcore45+wpa81\\", Some "**\\*.pdb;*iOS*;*Android*" )
                    ("Gaucho.Forms.*", Some "lib\\Xamarin.iOS1.0\\", Some "**\\*.pdb;*Android*" )
                    ("Gaucho.Forms.*", Some "lib\\MonoAndroid1.0\\", Some "**\\*.pdb;*iOS*" )
            ]
        }) nuspecFile
)

// Dependencies
"Clean"
    ==> "RestorePackages"
    ==> "AssemblyInfo"
    ==> "BuildRelease"
    ==> "CreateNugetPackages"
    
RunTargetOrDefault "CreateNugetPackages"