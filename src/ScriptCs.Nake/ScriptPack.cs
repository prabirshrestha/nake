using ScriptCs.Contracts;

namespace ScriptCs.Nake
{
    public class ScriptPack : IScriptPack
    {
        public void Initialize(IScriptPackSession session)
        {
        }

        public IScriptPackContext GetContext()
        {
            return new Nake();
        }

        public void Terminate()
        {
        }
    }
}