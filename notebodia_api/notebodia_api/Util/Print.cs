using System.Runtime.CompilerServices;
using System.Text.Json;

namespace notebodia_api.Util
{
    public static class PrintUtils
    {
        public static void PrettyPrint(object model, [CallerArgumentExpression("model")] string variableName = "")
        {
            Console.WriteLine($"{variableName}: {JsonSerializer.Serialize(model, new JsonSerializerOptions
            {
                WriteIndented = true
            })}");

        }
    }
}