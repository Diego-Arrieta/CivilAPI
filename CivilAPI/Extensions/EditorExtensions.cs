using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CivilAPI.Extensions
{
    public static class EditorExtensions
    {
        public static string EnterString(this Editor editor, string message = "Enter text: ")
        {
            string result = null;

            PromptStringOptions promptStringOptions = new PromptStringOptions(message);
            promptStringOptions.AllowSpaces = true;
            PromptResult promptResult = editor.GetString(promptStringOptions);

            if (promptResult.Status == PromptStatus.OK)
            {
                result = promptResult.StringResult;
            }
            else
            {
                editor.WriteMessage($"No text entered.\n");
            }
            return result;
        }
        public static Point3d EnterPoint(this Editor editor, string message = "Enter point: ")
        {
            Point3d point3d = new Point3d();

            PromptPointOptions promptPointOptions = new PromptPointOptions(message);
            PromptPointResult promptPointResult = editor.GetPoint(promptPointOptions);

            if (promptPointResult.Status == PromptStatus.OK)
            {
                point3d = promptPointResult.Value;
            }
            else
            {
                editor.WriteMessage($"No point entered.\n");
            }
            return point3d;
        }
        public static double EnterDistance(this Editor editor, string message = "Enter distance: ")
        {
            double distance = new double();

            PromptDoubleResult promptDoubleResult = editor.GetDistance(message);

            if (promptDoubleResult.Status == PromptStatus.OK)
            {
                distance = promptDoubleResult.Value;
            }
            else
            {
                editor.WriteMessage($"No distance entered.\n");
            }
            return distance;
        }
    }
}
