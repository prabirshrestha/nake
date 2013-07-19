using System;
using System.Threading.Tasks;

namespace ScriptCs.Nake
{
    class NakeTask
    {
        public string FilePath { get; set; }

        public int LineNumber { get; set; }

        public NakeTask(string fullName, Action<NakeTaskParameter> callback, string desc)
        {
        }

        public NakeTask(string fullName, Func<NakeTaskParameter, Task> callback, string desc)
        {
        }

        public NakeTask(string fullName, string[] deps, string desc)
        {
        }

        public NakeTask(string fullName, string[] deps, Action<NakeTaskParameter> callback, string desc)
        {
        }

        public NakeTask(string fullName, string callback, string deps)
        {
        }

        public NakeTask(string fullName, string deps, Action<NakeTaskParameter> callback, string desc)
        {
        }

        public NakeTask(string fullName, Func<Task> callback, string desc)
        {
        }

        public NakeTask(string fullName, string[] deps, Func<NakeTaskParameter, Task> callback, string desc)
        {
        }

        public NakeTask(string fullName, string deps, Func<NakeTaskParameter, Task> callback, string desc)
        {
        }
    }
}