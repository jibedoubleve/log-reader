
/* ----------------------------------------------------------------------------
 * Build script for Log-Reader
 * ----------------------------------------------------------------------------
 * This script uses environment variables. To run correctly, this script needs
 * these variables to be set: 
 *  - CAKE_GITHUB_TOKEN    - Github token of the LogReader repository
 *  - CAKE_GITHUB_USERNAME - Github username with R/W for LogReader repository
 */
///////////////////////////////////////////////////////////////////////////////
// TOOLS / ADDINS
///////////////////////////////////////////////////////////////////////////////
#tool vswhere
#tool GitVersion.CommandLine
#tool xunit.runner.console
#tool gitreleasemanager

#addin Cake.Figlet
#addin Cake.Powershell

///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////
var repoName = "LogReader";

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var verbosity = Argument("verbosity", Verbosity.Minimal);

/* This list contains the path of the assets to release.
 * It is cleared and filled into task "Zip" and used into
 * the task "Release-GitHub".
 */
var assets = new List<string>(); 


///////////////////////////////////////////////////////////////////////////////
// PREPARATION
///////////////////////////////////////////////////////////////////////////////

//Files & directories
var solution   = "./src/Probel.LogReader.sln";
var publishDir = "./Publish/";
var inno_setup = "./setup.iss";
var binDirectory = $"./src/Probel.LogReader/bin/{configuration}/";
var binCsvDir = $"./src/plugins/Probel.LogReader.Plugins.Csv/bin/{configuration}/";

GitVersion gitVersion = GitVersion(new GitVersionSettings 
{ 
    OutputType = GitVersionOutput.Json,
    UpdateAssemblyInfo  = true,
    UpdateAssemblyInfoFilePath  = "./src/Version.cs",
});
var branchName = gitVersion.BranchName;

///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////
Setup(ctx =>
{
    if(!IsRunningOnWindows())
    {
        throw new NotImplementedException($"{repoName} should only run on Windows");
    }
    
    Information(Figlet($"Probel   {repoName}"));

    Information("Configuration             : {0}", configuration);
    Information("Branch                    : {0}", branchName);
    Information("Informational      Version: {0}", gitVersion.InformationalVersion);
    Information("SemVer             Version: {0}", gitVersion.SemVer);
    Information("AssemblySemVer     Version: {0}", gitVersion.AssemblySemVer);
    Information("AssemblySemFileVer Version: {0}", gitVersion.AssemblySemFileVer);
    Information("MajorMinorPatch    Version: {0}", gitVersion.MajorMinorPatch);
    Information("NuGet              Version: {0}", gitVersion.NuGetVersion);  
});

///////////////////////////////////////////////////////////////////////////////
// TASKS
///////////////////////////////////////////////////////////////////////////////
Task("Clean")
    .Does(()=> {
        var dirToDelete = GetDirectories("./**/obj")
                            .Concat(GetDirectories("./**/bin"))
                            .Concat(GetDirectories("./**/Publish"));
        DeleteDirectories(dirToDelete, new DeleteDirectorySettings{ Recursive = true, Force = true});
});


Task("Restore")
    .Does(() => {
        NuGetRestore(solution);
});

Task("Build")
    .Does(() => {    
        var msBuildSettings = new MSBuildSettings {
            Verbosity = verbosity,
            Configuration = configuration
        };

        MSBuild(solution, msBuildSettings
            .WithProperty("Description", "A simple launcher.")
        );
        
});

Task("Unit-Test")      
    .Does(() => {
        var testPrj = "./src/tests/Probel.LogReader.Tests/Probel.LogReader.Tests.csproj";
        DotNetCoreTest(testPrj);
});

Task("Zip")
    .Does(()=> {
        var zipName = publishDir + "/logreader." + gitVersion.SemVer + ".bin.zip";
        Information("Zip           : {0}: ", zipName);
        Information("Bin dir       : {0}", binDirectory);

        EnsureDirectoryExists(Directory(publishDir));
        Zip(binDirectory, zipName);

        var dir = new DirectoryInfo(binDirectory + @"/../../../plugins/");        
        foreach(var d in dir.GetDirectories())
        {
            if(d.Name.ToLower().Contains("csv")==false)
            { 
                var pluginBin = d.FullName + @"\bin\Release\";
                var dest = publishDir + "/" + d.Name.Replace("Probel.LogReader.Plugins.","Plugin-") + "-" + gitVersion.SemVer + ".bin.zip";
                assets.Add(dest);

                Information("Zipping plugin:  {0}", dest);
                Information("  pluginBin : {0}", pluginBin);
                Information("  dest      : {0}", dest);

                Zip(pluginBin, dest);
            }
        }
});  

Task("Inno-Setup")
    .Does(() => {
        var path = binDirectory.Replace("/", "\\").TrimStart('.').TrimStart('\\');
        var pluginDir = binCsvDir.Replace("/", "\\").TrimStart('.').TrimStart('\\');

        Information("Bin path   : {0}: ", path);
        Information("Plugin path: {0}: ", pluginDir);

        InnoSetup(inno_setup, new InnoSetupSettings { 
            OutputDirectory = publishDir,
            Defines = new Dictionary<string, string> {
                 { "MyAppVersion", gitVersion.SemVer },
                 { "BinDirectory", path },
                 { "BinPluginDir", pluginDir }
            }
        });
});

Task("Release-GitHub")
    .Does(()=>{
        //https://stackoverflow.com/questions/42761777/hide-services-passwords-in-cake-build
        var token = EnvironmentVariable("CAKE_GITHUB_TOKEN");
        var owner = EnvironmentVariable("CAKE_GITHUB_USERNAME");

        var stg = new GitReleaseManagerCreateSettings 
        {
            Milestone         = "V" + gitVersion.MajorMinorPatch,            
            Name              = gitVersion.SemVer,
            Prerelease        = gitVersion.SemVer.Contains("alpha"),
            Assets            = publishDir + "/lanceur." + gitVersion.SemVer + ".bin.zip," + publishDir + "/lanceur." + gitVersion.SemVer + ".setup.exe",
        };

        GitReleaseManagerCreate(token, owner, "log-reader", stg);  
    });

Task("Default")
    .IsDependentOn("Clean")
    .IsDependentOn("Restore")
    .IsDependentOn("Build")
    .IsDependentOn("Unit-Test")
    .IsDependentOn("Zip")
    .IsDependentOn("Inno-Setup");

Task("Github")    
    .IsDependentOn("Default")
    .IsDependentOn("Release-GitHub");

RunTarget(target);