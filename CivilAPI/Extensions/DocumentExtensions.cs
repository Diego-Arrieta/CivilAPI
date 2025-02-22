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
    public static class DocumentExtensions
    {
        public static void CreateLine(this Document doc, Point3d pnt1, Point3d pnt2)
        {
            using (Transaction tr = doc.Database.TransactionManager.StartTransaction())
            {
                try
                {
                    BlockTable bt = tr.GetObject(doc.Database.BlockTableId, OpenMode.ForRead) as BlockTable;
                    BlockTableRecord btr = tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

                    // Send a message to the user
                    doc.Editor.WriteMessage("Drawing a Line Object:\n");
                    Line ln = new Line(pnt1, pnt2);
                    btr.AppendEntity(ln);
                    tr.AddNewlyCreatedDBObject(ln, true);
                    tr.Commit();
                }
                catch (System.Exception ex)
                {
                    doc.Editor.WriteMessage($"Error encountered: {ex.Message}");
                    tr.Abort();
                }
            }
            return;
        }
        public static List<ObjectId> PickEntities(this Document doc, string message = "Select entities: ")
        {
            List<ObjectId> results = new List<ObjectId>();

            // Selected Options
            PromptSelectionResult psr = doc.Editor.GetSelection();

            if (psr.Status == PromptStatus.OK && psr.Value.Count > 0)
            {
                foreach (SelectedObject sObj in psr.Value)
                {
                    results.Add(sObj.ObjectId);                    
                }
            }            
            return results;
        }
        public static ObjectId PickEntity(this Document doc, string message = "Select entity: ")
        {
            ObjectId result = ObjectId.Null;

            // Prompt Selection Options
            PromptSelectionOptions pso = new PromptSelectionOptions();
            pso.SingleOnly = true;
            pso.MessageForAdding = message;
            PromptSelectionResult psr = doc.Editor.GetSelection(pso);

            if (psr.Status == PromptStatus.OK && psr.Value.Count > 0)
            {
                result = psr.Value[0].ObjectId;
                return result;
            }
            return result;
        }
        public static ObjectId PickEntityOfType(this Document doc, string type, string message = "Select entity: ")
        {
            ObjectId result = ObjectId.Null;

            // Selection Filter
            TypedValue[] tv = new TypedValue[1];
            tv.SetValue(new TypedValue((int)DxfCode.Start, type), 0);
            SelectionFilter filter = new SelectionFilter(tv);

            // Prompt Selection Options
            PromptSelectionOptions pso = new PromptSelectionOptions();
            pso.SingleOnly = true;
            pso.MessageForAdding = message;
            PromptSelectionResult psr = doc.Editor.GetSelection(pso, filter);

            // Validation
            if (psr.Status != PromptStatus.OK) throw new NotImplementedException();
            if (psr.Value.Count > 0)
            {
                result = psr.Value[0].ObjectId;
            }            

            return result;
        }
        public static void Run(this Document doc, Action<Transaction> action)
        {          
            using (Transaction tr = doc.Database.TransactionManager.StartTransaction())
            {
                try
                {
                    action.Invoke(tr);
                    tr.Commit();
                }
                catch (System.Exception ex)
                {
                    tr.Abort();
                    doc.Editor.WriteMessage($"Error encountered: {ex.ToString()}\n");
                }
            }                  
        }
    }
}
