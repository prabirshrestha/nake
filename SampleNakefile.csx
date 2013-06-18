var nake = Require<Nake>();

nake.desc("default task");
nake.task("default", p => {
    Console.WriteLine("default task");
});

nake.desc("task with deps as array");
nake.task("taskWithArrayDeps", new [] { "default" }, p => {
    Console.WriteLine("task with deps as array");
});

nake.task("taskWithArrayDepsWithoutCallback", new [] { "default" });

nake.desc("task with deps as comma seperated strings");
nake.task("taskWithCommaSeperatedDeps", "default,taskWithArrayDeps", p => {
    Console.WriteLine("task with deps as comma seperated strings");
});

nake.task("taskWithCommaSeperatedDepsWithoutCallback", "default,taskWithArrayDeps");

nake.desc("async task");
nake.task("asyncTask", p => {
    return Task.Delay(100); // need to wait for roslyn to support async/await
});

nake.desc("async task with deps as array");
nake.task("asyncTaskWithArrayDeps", new [] { "default" }, p => {
     return Task.Delay(100);
});

nake.desc("async  task with deps as comma seperated strings");
nake.task("asyncTaskWithCommaSeperatedDeps", "default,taskWithArrayDeps", p => {
    return Task.Delay(100);
});

nake.desc("parameters task");
nake.task("parameter", p => {
    var foo = p[0]; // Dynamic value
    string fooString = p[0]; // auto conversion to string
    int bar = p[1]; // auto conversion to int

    // This language feature ('dynamic') is not yet implemented in Roslyn. :(
    var qux = p.env["qux"]; // DynamicValue
    string quxString = p.env["qux"]; // string auto conversion
    // string qux2 = p.env.qux; // i love dynamic
    int frang = p.env["frang"]; // notice auto conversion to int
    // int frang2 = p.env.Frang(); // using extension methods not possible on dynamic
});

nake.ns("namespace", ()=> {

    nake.task("foo", p => {
        Console.WriteLine("namespace:foo");
    });

    nake.ns("secondlevel", ()=> {
        nake.task("bar", p=> {
            Console.WriteLine("namespace:secondlevel:foo");
        });
    });

});

nake.execute(ScriptArgs);
