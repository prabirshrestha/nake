```c#
// this has been heavily influenced by njake https://github.com/prabirshrestha/njake
 
var nake = require<Nake>;
nake.global(); // can we make methods like desc and task global without specifying nake.desc and nake.task?
 
desc("This is the default task");
task("default", _ => {
    Console.WriteLine("This is the default task");
});
 
desc("This task has prerequisites which are defined as array");
task("hasPrereqs", new[] { "foo", "bar", "baz"}, _ => {
    Console.WriteLine("Run some prereqs first");
});
 
desc("This task has prerequisites which are defined as a comma separated string");
task("hasPrereqs", "foo,bar,baz", () => {
    // ....
});
 
desc("This is an asynchronous task");
task("asyncTask", async _ => {
    await someLongRunningTask();
});
 
// File task
desc("This builds a minifed JS file for production");
file("foo-minified.js", "bar,foo-barjs,foo-baz.js", _ => {
    // code to concat and minify goes here
});
 
// Directory task
directory("bar");
 
// namespaces
 
namespaces("foo", () => {
    desc("This is foo:bar task");
    task("bar", () => {
        Console.WriteLine("doing foo:bar task");
    });
});
 
// passing parameters
task("parameters", p => {
    var foo = p[0]; // string by default
    int bar = p[1]; // notice auto conversion to int
 
    var qux = nake.env["qux"]; // string by default
    string qux2 = nake.env.qux; // i love dynamic
    int frang = nake.env["frang"]; // notice auto conversion to int
    int frang2 = nake.env.Frang(); // using extension methods
});
// nake parameters[foo,bar];
// Any parameters passed after the Jake task that contain an equals sign (=) will be added to environment variable
// nake parameters[foo,bar] qux=zoobie frang=1
 
public static class EnvironmentExtensions {
    public static int Frang(this NakeEnvironment env) {
        return env["frang"];
    }
}
 
// running tasks from within other task
desc("Calls the foo:bar task and its prerequeisites.");
task("invokeFooBar", async () => {
    nake.Task["foo:bar"].invoke();
    await nake.Task["foo:bar:asyncTask"].invokeAsync();
    nake.Task["foo:bar"].execute(); // calls foo:bar without prerequisites
});
 
desc("This task fails with an exit-status of 42.");
task("failTask", () => {
    fail("failed", 42);
});
 
// Helper utils
 
// File utils
nake.mkdirP("auto/creates/sub/folder"); // will not throw if directory already exists
nake.cpR("bin/Release/*", "dist/"); // recursive copy
var x = nake.readdirR("src"); // similar to UNIX find command
var files = najake.readdirR("src").Where(x => x.Type == nake.File).ToList(); // get only files
var dirs = nake.readdirR("src").Where(x => x.Type == nake.Dir).ToList(); // get only directories
 
// execute command
desc("Run nuget install");
namespaces("nuget", () ={
    task("install", () => {
        // using c# params[].
        // so can do something like nake.exec("command", "param1", "param2", autoConvertIntParam, 2, 3, 6);
        nake.log.info("installing nuget packages");
        var code = nake.exec("nuget", "install"); // could also support await nake.execAsync
        if(code != 0) {
            nake.log.error("nuget install error log");
            fail("nuget install failed", code);
        }
    });
});
 
// should be smart enough to handle http ftp protocols
// hate using powershell to download as it requires to change privilages which requires admin access
// and every one doesn't have curl
var str = nake.download("https://google.com").asString(); // asBytes, asStream
var file = nake.download("http://google.com/").allowRedirect(true).to("c:\google.html");
var hugeFile = await nake.downloadAsync("http://microsoft.com/dotnet4.msi").withProgressBar(true).to("c:\dotnet4.msi");
 
// All the above code should be part of core nake.
// These are community build packages
// don't make these part of the core.
// incase i dont like the default nake tasks i can import my own
// also this follows unix philosophy
//  * write programs to do one things and to it well
//  * write programs to work together (all these msubild tasks internally uses nake's exec)
 
var msbuild = Require<MSBuild4Nake>();
msbuild.setDefaults({ // use anonymous objects
    properties = new { Configuration = "Release" }
    processor = "x86",
    version = "net4.0"
});
 
// also support dictionary<string,object> use full for conditional compilations
var msbuildDefaults = new Dictionary<string,object>();
if(nake.os.type != nake.windows) {
    msbuildDefaults["exe"] = "/bin/mono"; // normal dictionary
    msbuildDefaults.exe = "/bin/mono"; // dynamic
}
 
msbuild.setDefaults(msbuildDefaults);
 
// and also support fluent api for better intellisense
msbuild.setDefaults()
    .version("net4.0")
    .exe(() => nake.os.type == nake.mac, "/usr/local/bin/xbuild") // homebrew
    .exe(() => nake.os.type == nake.windows, @"c:\program files\mono\xbuid")
    .exe("/bin/xbuild"); // notice no coditions here.
    // all these gets stacked so since we have multiple exe here it will try executing the first one,
    // if condition fails goes to the next till it runs out of the stack.
    // if it does not find in stack uses the one created using setDefaults.
    // if setDefaults is not defined then MSBuild4Nake should have defaults
 
msbuild.setDefaults()
    .useMonoIfAvailable();
 
msbuild.setDefaults()
    .useMono(); // always force mono
 
msbuild.setDefaults()
    .monoPaths(                     // setup mono lookup paths to override the default
        "/usr/local/bin/mono",
        "/bin/mono",
        @"c:\program files\mono\bin\mono" // should be smart enough to understand unix and windows paths
    )
    .useMono();
 
msbuild.setDefaults()
    .useMonoIf(() => nake.env.mono == true); // notice how we are using dynamic here and also how env string is getting converted to bool
                                              // this will allow users to set something like this
                                              // nake build mono=true
                                              // nake build mono=1
 
task("msbuild", () => {
    new msbuild()
        .file("src/sample.sln")
        .targets("Clean", "Build")
        .exec(); // always call exec to actually execute the task
 
    // allow override of complete
    new msbuild().file("src/sample.sln").exec(code => {
        if(code == -1)
            faile("failed", -1);
    }).exec();
});
 
// xunit.net
var xunit = Require<XUnit4Nake>();
xunit.setDefaults().exe("src/packages/xunit/xunit.cosole.clr4.x86.exe");
task("test", "build:all", () => {
    new xunit().assembly("bin/somefile.dll").exec(); // fluent api
    new xunit().set(new { // or anonymous objects, or dictionary
        assembly = "bin/somefile.dll"
    }).exec();
});
 
// assembly info
var assemblyInfo = Require<AssemblyInfo4Nake>();
assemblyInfo
    .file("src/project/Properties/AssemblyInfo.cs")
    .language("c#") // can also include constants like assemblyInfo.langauge(assemblyInfo.langauge.csharp);
    .namespaces("System.Reflection", "System.Runtime.InteropServices")
    .contents(c => {
        c.append(c => c.language  == "vb", """");
        c.append(c => c.langauge == "csharp", "//");
        c.appendLine("do not modify this file manually, use nakefile instead");
        c
            .title("my app")
            .description("my app description")
            .comVisible(false); // understands boolean values
    })
    .exec();
 
// nuget
var nuget = Require<Nuget4Nake>();
nuget.defaults()
    .exe("src/.nuget/NuGet.exe")
    .verbose(true)
    .apiKey(nake.env.nuget_api_key);
 
new nuget.pack()
    .nuspec("src/project.nuspec")
    .version("1.0.0")
    .properties(p => {
        p.owners("Prabir Shrestha", "Some other owner");
    })
    .outputDir("dist/NuGet")
    .exec();
 
new nuget.pack()
    .package("dist/NuGet/project.1.0.0.nupkg")
    .useSymbolSource() // use symbol source
    .source("https://,....") // or specify the direct url
    .exec();
 
// some projects like nuget moves really fast
// so rather then waiting for the Nuget4Nake owner to update it
// we should be able to pass any args. (this should be supported by all and not just Nuget4Nake)
new nuget.pack()
    .package("dist/NuGet/project.1.0.0.nupkg")
    .source("https://....")
    .args("--verbose", "--lint") // all these args gets appended at the end before shelling out
    .exec();
 
// another useful place is where  subcommands get added
new nuget
    .args(
        "pack",
        "dist/NuGet/project.1.0.0.nupkg",
        "--source", "https://nuget.org",
        "--verbose"
    )
    .exec();
```
