using System;
using System.Dynamic;

namespace ScriptCs.Nake
{
    public class NakeEnvironmentValues : DynamicObject
    {
        public DynamicValue this[string number]
        {
            get { throw new NotImplementedException(); }
        }
    }
}