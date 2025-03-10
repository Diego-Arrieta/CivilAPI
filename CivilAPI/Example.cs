﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Autodesk.Aec.PropertyData.DatabaseServices;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using CivilAPI.Extensions;
using CivilAPI.Forms;
using CivilAPI.Wrappers;

namespace CivilAPI
{
    public class Example
    {
        [CommandMethod("LayoutFromObject")]
        public void LayoutFromObject()
        {
            Document document = Application.DocumentManager.MdiActiveDocument;
            Database database = document.Database;
            Editor editor = document.Editor;

            Entity entity = document.PickEntity();
            Layout layout = entity.GetLayout();
            editor.WriteMessage(layout.LayoutName);
        }
        [CommandMethod("FlattenPolyline")]
        public void FlattenPolyline()
        {
            Document document = Application.DocumentManager.MdiActiveDocument;
            Database database = document.Database;
            Editor editor = document.Editor;

            //Polyline3d polyline3d = document.PickEntityOfType("POLYLINE") as Polyline3d;
            //Polyline3dWrapper polyline3dWrapper = new Polyline3dWrapper(polyline3d);
            //polyline3dWrapper.FlattenPolyline();

            Polyline3dWrapper polyline3dWrapper = document.PickWrapper<Polyline3dWrapper>("POLYLINE");
            polyline3dWrapper.ToPolyline();
        }
        [CommandMethod("FormExample")]
        public void FormExample()
        {
            Document document = Application.DocumentManager.MdiActiveDocument;
            Database database = document.Database;
            Editor editor = document.Editor;

            PropertySetsForm propertySetsForm = new PropertySetsForm(document, database, editor);
            propertySetsForm.Show();
        }
        [CommandMethod("ExampleProgam")]
        public void ExampleProgam()
        {
            Document document = Application.DocumentManager.MdiActiveDocument;
            Database database = document.Database;
            Editor editor = document.Editor;

            Entity entity = document.PickEntity();
            Layout layout = entity.GetLayout();

            database.Run(tr =>
            {
                BlockTableRecord blockTableRecord = tr.GetObject(layout.BlockTableRecordId, OpenMode.ForRead) as BlockTableRecord;
                blockTableRecord.CreateLine(new Point3d(0, 0, 0), new Point3d(10, 10, 0));
            });
        }
        [CommandMethod("SelectOnlyPolys")]
        public void SelectOnlyPolys()
        {
            Document document = Application.DocumentManager.MdiActiveDocument;
            Database database = document.Database;
            Editor editor = document.Editor;

            document.PickEntitiesOfType("CIRCLE");
        }
    }
}