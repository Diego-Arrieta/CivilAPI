using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using Autodesk.Aec.PropertyData.DatabaseServices;
using Civil = Autodesk.Civil.DatabaseServices;
using CivilApp = Autodesk.Civil.ApplicationServices;

namespace CivilAPI.Extensions
{
    public static class StructureExtensions
    {
        public static List<ObjectId> getConnectedPipes(this Civil.Structure structure)
        {
            List<ObjectId> pipes = new List<ObjectId>();

            int count = structure.ConnectedPipesCount;

            for (int i = 0; i < count; i++)
            {
                ObjectId pipeId = structure.get_ConnectedPipe(i);
                pipes.Add(pipeId);
            }

            return pipes;
        }
    }
}
