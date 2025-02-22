using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.GraphicsInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CivilAPI.Extensions
{
    public static class Polyline3dExtensions
    {
        public static void FlattenPolyline(this Document doc, ObjectId objectId)
        {
            doc.Run(tr =>
            {
                Polyline3d polyline3d = tr.GetObject(objectId, OpenMode.ForWrite) as Polyline3d;
                ObjectId[] vertexIds = polyline3d.Cast<ObjectId>().ToArray();

                foreach (ObjectId vertexId in vertexIds)
                {
                    PolylineVertex3d vertex = tr.GetObject(vertexId, OpenMode.ForWrite) as PolylineVertex3d;
                    Point3d newLocation = new Point3d(vertex.Position.X, vertex.Position.Y, 0);
                    vertex.Position = newLocation;
                }
            });

            return;
        }
    }
}
