using System;
using System.Threading.Tasks;
using ScriptCs.Contracts;

namespace ScriptCs.Nake
{
    public class Nake : IScriptPackContext
    {
        private string currentNamespace;
        private string currentDesc;
        public Nake()
        {
            this.currentNamespace = null;
            this.currentDesc = null;
        }

        public void desc(string description)
        {
            this.currentDesc = description;
        }

        public void task(string name, Action<NakeTaskParameter> callback)
        {
            this.currentDesc = null;
        }

        public void task(string name, Func<NakeTaskParameter, Task> callback)
        {
            this.currentDesc = null;
        }

        public void task(string name, string[] deps)
        {
            task(name, p => { });
            this.currentDesc = null;
        }

        public void task(string name, string[] deps, Action<NakeTaskParameter> callback)
        {
            this.currentDesc = null;
        }

        public void task(string name, string deps)
        {
            task(name, deps, p => { });
            this.currentDesc = null;
        }

        public void task(string name, string deps, Action<NakeTaskParameter> callback)
        {
            task(name, deps.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries), callback);
            this.currentDesc = null;
        }

        public void task(string name, Func<Task> callback)
        {
            this.currentDesc = null;
        }

        public void task(string name, string[] deps, Func<NakeTaskParameter, Task> callback)
        {
            this.currentDesc = null;
        }

        public void task(string name, string deps, Func<NakeTaskParameter, Task> callback)
        {
            task(name, deps.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries), callback);
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
            return 0;
        }

        private string getFullTaskName(string task)
        {
            return string.IsNullOrWhiteSpace(this.currentNamespace)
                ? task
                : string.Format("{0}:{1}", this.currentNamespace, task);
        }
    }
}
