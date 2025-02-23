using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using CivilAPI.Extensions;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CivilAPI.Wrappers
{
    public class Polyline3dWrapper : IElementWrapper
    {
        private readonly Polyline3d polyline3d;
        private readonly Database database;

        public Entity GetEntity()
        {
            return polyline3d;
        }
        public Polyline3dWrapper(Polyline3d polyline3d)
        {
            this.polyline3d = polyline3d;
            this.database = polyline3d.Database;
        }
        public int CountVertex()
        {
            ObjectId[] vertexIds = polyline3d.Cast<ObjectId>().ToArray();

            return vertexIds.Count();
        }
        public void FlattenPolyline()
        {
            database.Run(tr =>
            {
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
        public Polyline ToPolyline()
        {
            // Creating a new polyline
            Polyline polyline = new Polyline();

            database.Run(tr =>
            {
                BlockTableRecord blockTableRecord = tr.GetObject(polyline3d.OwnerId, OpenMode.ForWrite) as BlockTableRecord;

                ObjectId[] vertexIds = polyline3d.Cast<ObjectId>().ToArray();
                for (int i = 0; i < vertexIds.Count(); i++)
                {
                    PolylineVertex3d vertex = tr.GetObject(vertexIds[i], OpenMode.ForRead) as PolylineVertex3d;
                    polyline.AddVertexAt(i, new Point2d(vertex.Position.X, vertex.Position.Y), 0, 0, 0);
                }

                if (polyline3d.Closed) polyline.Closed = true;

                // Adding to the database
                polyline.SetDatabaseDefaults();
                blockTableRecord.AppendEntity(polyline);
                tr.AddNewlyCreatedDBObject(polyline, true);

                Entity poly3d = tr.GetObject(polyline3d.ObjectId, OpenMode.ForWrite) as Entity;
                poly3d.Erase(true);
            });

            return polyline;
        }
    }
}
