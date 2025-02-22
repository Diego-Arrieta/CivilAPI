using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CivilAPI.Extensions
{
    public static class EditorExtensions
    {
        public static string GetStringData(this Editor ed, string message = "Enter text: ")
        {
            // define variable
            string result = "";

            // Prompt the user using PromptStringOptions
            PromptStringOptions pso = new PromptStringOptions(message);
            pso.AllowSpaces = true;
            
            // Get the results of the user Input
            PromptResult res = ed.GetString(pso);

            if (res.Status == PromptStatus.OK)
            {
                result = res.StringResult;
                ed.WriteMessage($"The text is {result}.\n");
            }
            else
            {
                ed.WriteMessage($"No text entered.\n");
            }
            return result;

        }
    }
}
