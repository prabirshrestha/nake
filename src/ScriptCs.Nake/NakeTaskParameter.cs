using System;

namespace ScriptCs.Nake
{
    public class NakeTaskParameter
    {
        public DynamicValue this[int number]
        {
            get { throw new NotImplementedException(); }
        }

        public NakeEnvironmentValues /* dynamic when roslyn supports it*/ env
        {
            get { return new NakeEnvironmentValues(); }
        }
    }
}