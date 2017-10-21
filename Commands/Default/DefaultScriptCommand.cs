using Jint;
using Microsoft.Owin;
using System.Collections;
using System.Threading.Tasks;

namespace apistation.owin.Commands.Default
{
    [CommandOptions("get", "/script/*")]
    public class DefaultScriptCommand : IScriptCommand
    {
        private Engine _engine;
        private readonly Jint.Parser.Ast.Program _program;

        public DefaultScriptCommand(Jint.Parser.Ast.Program program)
        {
            _program = program;
        }

        public void Dispose()
        {
        }

        public Task<Hashtable> Invoke(IOwinContext context)
        {
            // apply the owin context in the jint engine
            _engine = new Engine().SetValue("context", context);
            var scriptResult = _engine.Execute(_program).GetCompletionValue().AsObject();
            var result = new Hashtable();
            result.Add("result", scriptResult);
            return Task.FromResult(result);
        }
    }
}