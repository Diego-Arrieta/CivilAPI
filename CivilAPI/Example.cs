using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using CivilAPI.Extensions;

namespace CivilAPI
{
    public class Example
    {
        [CommandMethod("ExampleTest")]
        public void ExampleTest()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            string value = ed.GetStringData("GAAAA");
            ed.WriteMessage(value);

            // doc.CreateLine(new Point3d(0, 0, 0), new Point3d(10, 10, 0));

            List<ObjectId> objectsIds = doc.PickEntities();
            ed.WriteMessage($"There are {objectsIds.Count.ToString()} entities\n");

            doc.Run(tr =>
            {
                foreach (ObjectId objectsId in objectsIds)
                {
                    Entity entity = tr.GetObject(objectsId, OpenMode.ForRead) as Entity;
                    ed.WriteMessage($"BlockName: {entity.BlockName}\n");
                }
            });
        }

        [CommandMethod("LayoutFromObject")]
        public void LayoutFromObject()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            ObjectId objectId = doc.PickEntity();

            doc.Run(tr =>
            {
                Entity entity = tr.GetObject(objectId, OpenMode.ForRead) as Entity;
                BlockTableRecord btr = tr.GetObject(entity.OwnerId, OpenMode.ForRead) as BlockTableRecord;
                if (btr.IsLayout)
                {
                    Layout lay = tr.GetObject(btr.LayoutId, OpenMode.ForRead) as Layout;
                    ed.WriteMessage($"Layout name: {lay.LayoutName}\n");
                }
            });
        }
        [CommandMethod("FlattenPolyline")]
        public void FlattenPolyline()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            ObjectId objectId = doc.PickEntityOfType("POLYLINE");

            doc.Run(tr =>
            {                
                // BlockTableRecord
                BlockTable bt = tr.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

                // Creating a new polyline
                Polyline polyline = new Polyline();

                // Getting the current polyline3d
                Polyline3d polyline3d = tr.GetObject(objectId, OpenMode.ForRead) as Polyline3d;
                ObjectId[] vertexIds = polyline3d.Cast<ObjectId>().ToArray();

                // Vertex
                for (int i = 0; i < vertexIds.Count(); i++)
                {
                    PolylineVertex3d vertex = tr.GetObject(vertexIds[i], OpenMode.ForRead) as PolylineVertex3d;
                    polyline.AddVertexAt(i, new Point2d(vertex.Position.X, vertex.Position.Y), 0, 0, 0);
                }

                if (polyline3d.Closed) polyline.Closed = true;

                // Adding to the database
                polyline.SetDatabaseDefaults();
                btr.AppendEntity(polyline);
                tr.AddNewlyCreatedDBObject(polyline, true);
                ed.WriteMessage("Done\n");
            });
        }
        [CommandMethod("FlattenPolyline2")]
        public void FlattenPolyline2()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            ObjectId objectId = doc.PickEntityOfType("POLYLINE");

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
        }
    }
}