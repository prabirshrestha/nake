using System;

namespace ScriptCs.Nake
{
    public class DynamicValue
    {
        public DynamicValue(object value)
        {
        }

        public static implicit operator string(DynamicValue value)
        {
            throw new NotImplementedException();
        }

        public static implicit operator int(DynamicValue value)
        {
            throw new NotImplementedException();
        }

        public static implicit operator long(DynamicValue value)
        {
            throw new NotImplementedException();
        }

        public static implicit operator double(DynamicValue value)
        {
            throw new NotImplementedException();
        }

        public static implicit operator decimal(DynamicValue value)
        {
            throw new NotImplementedException();
        }
    }
}