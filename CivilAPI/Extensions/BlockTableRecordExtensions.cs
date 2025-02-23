using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CivilAPI.Extensions
{
    public static class BlockTableRecordExtensions
    {
        public static void CreateLine(this BlockTableRecord blockTableRecord, Point3d pointStart, Point3d pointEnd)
        {
            blockTableRecord.Database.Run(tr =>
            {
                BlockTableRecord btr = tr.GetObject(blockTableRecord.ObjectId, OpenMode.ForWrite) as BlockTableRecord;

                Line line = new Line(pointStart, pointEnd);
                btr.AppendEntity(line);
                tr.AddNewlyCreatedDBObject(line, true);
            });

            return;
        }
    }
}
