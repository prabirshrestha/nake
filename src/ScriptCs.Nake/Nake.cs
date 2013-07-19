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
            this.currentDesc = null;
        }

        public int execute(string[] args)
        {
            return 0;
        }

    }
}
