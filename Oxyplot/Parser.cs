using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System;
using System.Threading.Tasks;

namespace OxyPlotProgram
{
    public class Parser
    {
        public string input { get; set; }
        public Func<double, double> output;
        public int value { get; set; }

        public Parser(string inp = "")
        {
            input = inp;
        }


        public async Task<Func<double, double>> Execute(string code)
        {
            Task<object> task = CSharpScript.EvaluateAsync(code, ScriptOptions.Default.WithImports(new string[] {"System", "System.Math"}));
            return (await task) as Func<double, double>;
        }

        public async void RunCode() //First string is cast, value is the number given the amount of functions in the graph
        {
            string code = "Func<double, double> func69 = (x => " + input + ");" + " return func69;";

            output = await Execute(code);
        }
    }
}