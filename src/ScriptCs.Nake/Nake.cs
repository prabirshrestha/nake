using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ScriptCs.Contracts;

namespace ScriptCs.Nake
{
    public class Nake : IScriptPackContext
    {
        private string currentNamespace;
        private string currentDesc;
        private IDictionary<string, NakeTask> tasks;

        public Nake()
        {
            this.currentNamespace = null;
            this.currentDesc = null;
            this.tasks = new Dictionary<string, NakeTask>();
        }

        public void desc(string description)
        {
            this.currentDesc = description;
        }

        public void task(string name, Action<NakeTaskParameter> callback, [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
        {
            var fullName = getFullTaskName(name);
            var task = new NakeTask(fullName, callback, this.currentDesc)
            {
                FilePath = filePath,
                LineNumber = lineNumber
            };
            this.tasks[fullName] = task;
            this.currentDesc = null;
        }

        public void task(string name, Func<NakeTaskParameter, Task> callback, [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
        {
            var fullName = getFullTaskName(name);
            var task = new NakeTask(fullName, callback, this.currentDesc)
            {
                FilePath = filePath,
                LineNumber = lineNumber
            };
            this.tasks[fullName] = task;
            this.currentDesc = null;
        }

        public void task(string name, string[] deps, [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
        {
            var fullName = getFullTaskName(name);
            var task = new NakeTask(fullName, deps, this.currentDesc)
            {
                FilePath = filePath,
                LineNumber = lineNumber
            };
            this.tasks[fullName] = task;
            this.currentDesc = null;
        }

        public void task(string name, string[] deps, Action<NakeTaskParameter> callback, [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
        {
            var fullName = getFullTaskName(name);
            var task = new NakeTask(fullName, deps, callback, this.currentDesc)
            {
                FilePath = filePath,
                LineNumber = lineNumber
            };
            this.tasks[fullName] = task;
            this.currentDesc = null;
        }

        public void task(string name, string deps, [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
        {
            var fullName = getFullTaskName(name);
            var task = new NakeTask(fullName, deps, this.currentDesc)
            {
                FilePath = filePath,
                LineNumber = lineNumber
            };
            this.tasks[fullName] = task;
            this.currentDesc = null;
        }

        public void task(string name, string deps, Action<NakeTaskParameter> callback, [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
        {
            var fullName = getFullTaskName(name);
            var task = new NakeTask(fullName, deps, callback, this.currentDesc)
            {
                FilePath = filePath,
                LineNumber = lineNumber
            };
            this.tasks[fullName] = task;
            this.currentDesc = null;
        }

        public void task(string name, Func<Task> callback, [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
        {
            var fullName = getFullTaskName(name);
            var task = new NakeTask(fullName, callback, this.currentDesc)
            {
                FilePath = filePath,
                LineNumber = lineNumber
            };
            this.tasks[fullName] = task;
            this.currentDesc = null;
        }

        public void task(string name, string[] deps, Func<NakeTaskParameter, Task> callback, [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
        {
            var fullName = getFullTaskName(name);
            var task = new NakeTask(fullName, deps, callback, this.currentDesc)
            {
                FilePath = filePath,
                LineNumber = lineNumber
            };
            this.tasks[fullName] = task;
            this.currentDesc = null;
        }

        public void task(string name, string deps, Func<NakeTaskParameter, Task> callback, [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
        {
            var fullName = getFullTaskName(name);
            var task = new NakeTask(fullName, deps, callback, this.currentDesc)
            {
                FilePath = filePath,
                LineNumber = lineNumber
            };
            this.tasks[fullName] = task;
            this.currentDesc = null;
        }

        public void ns(string ns, Action block)
        {
            var originalNs = this.currentNamespace;

            this.currentNamespace = string.IsNullOrWhiteSpace(this.currentNamespace)
                ? ns
                : string.Format("{0}:{1}", this.currentNamespace, ns);

            this.currentDesc = null;

            block();

            this.currentDesc = null;

            this.currentNamespace = originalNs;
        }

        public int execute(string[] args)
        {
            var node = GenerateTaskNodes();
            try
            {
                var deps = node.ResolveDependencies();
            }
            catch (CircularDependencyException<NakeTask> ex)
            {
                Console.WriteLine("Circular dependency detected between tasks `{0}` and `{1}`", ex.B.Name, ex.A.Name);
                return -1;
            }

            foreach (var task in this.tasks.Where(t => !string.IsNullOrWhiteSpace(t.Value.Desc)))
            {
                Console.WriteLine("{0, -35} # {1}", task.Value.FullName, task.Value.Desc);
            }

            return 0;
        }

        private Node<NakeTask> GenerateTaskNodes()
        {
            var rootNode = new Node<NakeTask>("");
            var nodes = new Dictionary<string, Node<NakeTask>>();
            foreach (var task in tasks)
            {
                var node = new Node<NakeTask>(task.Value.FullName) { Data = task.Value };
                if (!task.Value.FullName.Contains(":"))
                {
                    rootNode.Edges.Add(node);
                }
                nodes.Add(task.Value.FullName, node);
            }

            foreach (var task in tasks)
            {
                foreach (var dep in task.Value.Deps)
                {
                    nodes[task.Value.FullName].DependsOn(nodes[dep]);
                }
            }

            return rootNode;
        }

        private string getFullTaskName(string task)
        {
            return string.IsNullOrWhiteSpace(this.currentNamespace)
                ? task
                : string.Format("{0}:{1}", this.currentNamespace, task);
        }
    }
}
