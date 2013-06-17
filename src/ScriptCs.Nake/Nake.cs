using System;
using System.Threading.Tasks;
using ScriptCs.Contracts;

namespace ScriptCs.Nake
{
    public class Nake : IScriptPackContext
    {
        public void desc(string description)
        {
        }

        public void task(string name, Action<NakeTaskParameter> callback)
        {
        }

        public void task(string name, Func<NakeTaskParameter, Task> callback)
        {
        }

        public void task(string name, string[] deps)
        {
            task(name, p => { });
        }

        public void task(string name, string[] deps, Action<NakeTaskParameter> callback)
        {
        }

        public void task(string name, string deps)
        {
            task(name, deps, p => { });
        }

        public void task(string name, string deps, Action<NakeTaskParameter> callback)
        {
            task(name, deps.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries), callback);
        }

        public void task(string name, Func<Task> callback)
        {
        }

        public void task(string name, string[] deps, Func<NakeTaskParameter, Task> callback)
        {
        }

        public void task(string name, string deps, Func<NakeTaskParameter, Task> callback)
        {
            task(name, deps.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries), callback);
        }

        public void ns(string ns, Action block)
        {
        }

    }
}
