using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

using AcadApp = Autodesk.AutoCAD.ApplicationServices;
using AcadDB = Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using AcadGeo = Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;

using Autodesk.Aec.PropertyData.DatabaseServices;

using Autodesk.Gis.Map.Platform;
using Autodesk.Gis.Map.Platform.Interop;
using Autodesk.Gis.Map.Platform.Internal;
using OSGeo.MapGuide;

using CivilApp = Autodesk.Civil.ApplicationServices;
using CivilDB = Autodesk.Civil.DatabaseServices;

using CivilAPI.Extensions;
using CivilAPI.Forms;
using CivilAPI.Wrappers;
using Autodesk.AutoCAD.LayerManager;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Windows.Forms;
using System.Data;

namespace CivilAPI
{
    public class Example
    {
        [CommandMethod("LayoutFromObject")]
        public void LayoutFromObject()
        {
            AcadApp.Document document = AcadApp.Application.DocumentManager.MdiActiveDocument;
            AcadDB.Database database = document.Database;
            Editor editor = document.Editor;

            AcadDB.Entity entity = document.PickEntity();
            AcadDB.Layout layout = entity.GetLayout();
            editor.WriteMessage(layout.LayoutName);
        }
        [CommandMethod("FlattenPolyline")]
        public void FlattenPolyline()
        {
            AcadApp.Document document = AcadApp.Application.DocumentManager.MdiActiveDocument;
            AcadDB.Database database = document.Database;
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
            AcadApp.Document document = AcadApp.Application.DocumentManager.MdiActiveDocument;
            AcadDB.Database database = document.Database;
            Editor editor = document.Editor;

            PropertySetsForm propertySetsForm = new PropertySetsForm(document, database, editor);
            propertySetsForm.Show();
        }
        [CommandMethod("ExampleProgam")]
        public void ExampleProgam()
        {
            AcadApp.Document document = AcadApp.Application.DocumentManager.MdiActiveDocument;
            AcadDB.Database database = document.Database;
            Editor editor = document.Editor;

            AcadDB.Entity entity = document.PickEntity();
            AcadDB.Layout layout = entity.GetLayout();

            database.Run(tr =>
            {
                AcadDB.BlockTableRecord blockTableRecord = tr.GetObject(layout.BlockTableRecordId, AcadDB.OpenMode.ForRead) as AcadDB.BlockTableRecord;
                blockTableRecord.CreateLine(new AcadGeo.Point3d(0, 0, 0), new AcadGeo.Point3d(10, 10, 0));
            });
        }
        [CommandMethod("SelectOnlyPolys")]
        public void SelectOnlyPolys()
        {
            AcadApp.Document document = AcadApp.Application.DocumentManager.MdiActiveDocument;
            AcadDB.Database database = document.Database;
            Editor editor = document.Editor;

            document.PickEntitiesOfType("CIRCLE");
        }
        [CommandMethod("SelectPipeNetwork")]
        public void SelectPipeNetwork()
        {
            AcadApp.Document document = AcadApp.Application.DocumentManager.MdiActiveDocument;
            AcadDB.Database database = document.Database;
            Editor editor = document.Editor;

            CivilDB.Pipe pipe = document.PickEntityOfType("AECC_PIPE") as CivilDB.Pipe;

            database.Run(tr =>
            {
                CivilDB.Network pipeNetwork = tr.GetObject(pipe.NetworkId, AcadDB.OpenMode.ForRead) as CivilDB.Network;
                     
                var structureIdCollection = pipeNetwork.GetStructureIds();

                foreach (AcadDB.ObjectId structureId in structureIdCollection)
                {
                    CivilDB.Structure structure = tr.GetObject(structureId, AcadDB.OpenMode.ForWrite) as CivilDB.Structure;

                    double pipeLowest = structure.PipeLowestBottomDepth;
                    double rimElevation = structure.RimElevation;
                    double sumpElevation = structure.SumpElevation;

                    if (sumpElevation != rimElevation - pipeLowest)
                    {
                        structure.SumpElevation = rimElevation - pipeLowest;
                        editor.WriteMessage($"{structure.Name} | {sumpElevation.ToString()} => {structure.SumpElevation.ToString()}\n");
                    }
                }
            });
        }
        [CommandMethod("InitMap")]
        public void InitMap()
        {
            AcadApp.Document document = AcadApp.Application.DocumentManager.MdiActiveDocument;
            AcadDB.Database database = document.Database;
            Editor editor = document.Editor;

            AcMapMap currentMap = AcMapMap.GetCurrentMap();
            MgLayerCollection layers = currentMap.GetLayers();
            AcMapLayer layer = layers.First() as AcMapLayer;

            AcadGeo.Point3d point = new AcadGeo.Point3d();
            point = editor.EnterPoint();

            MgGeometryFactory geometryFactory = new MgGeometryFactory();
            MgCoordinate coordinate = geometryFactory.CreateCoordinateXY(point.X, point.Y);
            MgPoint mgPoint = geometryFactory.CreatePoint(coordinate);

            //MgWktReaderWriter writer = new MgWktReaderWriter();
            //string wktPoint = writer.Write(mgPoint);
            //editor.WriteMessage($"{wktPoint}\n");

            MgAgfReaderWriter agfReaderWriter = new MgAgfReaderWriter();
            MgByteReader byteReader = agfReaderWriter.Write(mgPoint);

            MgPropertyCollection propertyCollection = new MgPropertyCollection();
            propertyCollection.Add(new MgGeometryProperty(layer.GetFeatureGeometryName(), byteReader));
            propertyCollection.Add(new MgStringProperty("STNAME", "Autodesk"));

            MgFeatureCommandCollection featureCommandCollection = new MgFeatureCommandCollection();
            featureCommandCollection.Add(new MgInsertFeatures(layer.GetFeatureClassName(), propertyCollection));
            layer.UpdateFeatures(featureCommandCollection);

            layer.SaveFeatureChanges(new MgFeatureQueryOptions());
            layer.ForceRefresh();
        }
    }
}