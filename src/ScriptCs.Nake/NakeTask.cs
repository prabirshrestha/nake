using System;
using System.Linq;
using System.Threading.Tasks;

namespace ScriptCs.Nake
{
    class NakeTask
    {
        public string FullName { get; private set; }
        public string[] Deps { get; private set; }
        public string Desc { get; set; }

        public string FilePath { get; set; }

        public int LineNumber { get; set; }

        public NakeTask(string fullName, Action<NakeTaskParameter> callback, string desc)
            : this(fullName, "", desc)
        {
        }

        public NakeTask(string fullName, Func<NakeTaskParameter, Task> callback, string desc)
            : this(fullName, "", desc)
        {
        }

        public NakeTask(string fullName, string[] deps, string desc)
        {
            this.FullName = fullName;
            this.Deps = (deps ?? new string[0]).Where(d => !string.IsNullOrWhiteSpace(d)).ToArray();
            this.Desc = desc;
        }

        public NakeTask(string fullName, string[] deps, Action<NakeTaskParameter> callback, string desc)
            : this(fullName, deps, desc)
        {
        }

        public NakeTask(string fullName, string deps, string desc)
        {
            this.FullName = fullName;
            this.Deps = deps.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            this.Desc = desc;
        }

        public NakeTask(string fullName, string deps, Action<NakeTaskParameter> callback, string desc)
            : this(fullName, deps, desc)
        {
        }

        public NakeTask(string fullName, Func<Task> callback, string desc)
            : this(fullName, "", desc)
        {
        }

        public NakeTask(string fullName, string[] deps, Func<NakeTaskParameter, Task> callback, string desc)
            : this(fullName, deps, desc)
        {
        }

        public NakeTask(string fullName, string deps, Func<NakeTaskParameter, Task> callback, string desc)
            : this(fullName, deps, desc)
        {
        }
    }
}