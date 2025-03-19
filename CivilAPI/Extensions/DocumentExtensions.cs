using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using CivilAPI.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CivilAPI.Extensions
{
    public static class DocumentExtensions
    {
        public static Entity PickEntity(this Document document, string message = "Select entity: ")
        {
            Entity entity = null;

            PromptSelectionOptions promptSelectionOptions = new PromptSelectionOptions();
            promptSelectionOptions.SingleOnly = true;
            promptSelectionOptions.MessageForAdding = message;

            PromptSelectionResult promptSelectionResult = document.Editor.GetSelection(promptSelectionOptions);

            if (promptSelectionResult.Status == PromptStatus.Error || promptSelectionResult.Value.Count == 0)
            {
                entity = document.PickEntity(message);
            }
            else
            {
                ObjectId objectId = promptSelectionResult.Value[0].ObjectId;                
                document.Database.Run(tr => entity = tr.GetObject(objectId, OpenMode.ForRead) as Entity);
            }
            return entity;
        }
        public static List<Entity> PickEntities(this Document document, string message = "Select entities: ")
        {
            List<Entity> entities = new List<Entity>();

            PromptSelectionOptions promptSelectionOptions = new PromptSelectionOptions();
            promptSelectionOptions.MessageForAdding = message;

            PromptSelectionResult promptSelectionResult = document.Editor.GetSelection(promptSelectionOptions);

            if (promptSelectionResult.Status == PromptStatus.Error || promptSelectionResult.Value.Count == 0)
            {
                entities = document.PickEntities(message);
            }
            else
            {
                foreach (SelectedObject selectedObject in promptSelectionResult.Value)
                {
                    ObjectId objectId = selectedObject.ObjectId;
                    document.Database.Run(tr => entities.Add(tr.GetObject(objectId, OpenMode.ForRead) as Entity));
                }
            }
            return entities;
        }
        public static Entity PickEntityOfType(this Document document, string type, string message = "Select entity: ")
        {
            Entity entity = null;

            TypedValue[] typedValue = new TypedValue[1];
            typedValue.SetValue(new TypedValue((int)DxfCode.Start, type), 0);
            SelectionFilter selectionFilter = new SelectionFilter(typedValue);

            PromptSelectionOptions promptSelectionOptions = new PromptSelectionOptions();
            promptSelectionOptions.SingleOnly = true;
            promptSelectionOptions.MessageForAdding = message;

            PromptSelectionResult promptSelectionResult = document.Editor.GetSelection(promptSelectionOptions, selectionFilter);

            if (promptSelectionResult.Status == PromptStatus.Error || promptSelectionResult.Value.Count == 0)
            {
                entity = document.PickEntityOfType(type, message);
            }
            else
            {
                ObjectId objectId = promptSelectionResult.Value[0].ObjectId;
                document.Database.Run(tr => entity = tr.GetObject(objectId, OpenMode.ForRead) as Entity);
            }
            return entity;
        }
        public static List<Entity> PickEntitiesOfType(this Document document, string type, string message = "Select entity: ")
        {
            List<Entity> entities = new List<Entity>();

            TypedValue[] typedValue = new TypedValue[1];
            typedValue.SetValue(new TypedValue((int)DxfCode.Start, type), 0);
            SelectionFilter selectionFilter = new SelectionFilter(typedValue);

            PromptSelectionOptions promptSelectionOptions = new PromptSelectionOptions();
            promptSelectionOptions.MessageForAdding = message;

            PromptSelectionResult promptSelectionResult = document.Editor.GetSelection(promptSelectionOptions, selectionFilter);

            if (promptSelectionResult.Status == PromptStatus.Error || promptSelectionResult.Value.Count == 0)
            {
                entities = document.PickEntitiesOfType(type, message);
            }
            else
            {
                foreach (SelectedObject selectedObject in promptSelectionResult.Value)
                {
                    ObjectId objectId = selectedObject.ObjectId;
                    document.Database.Run(tr => entities.Add(tr.GetObject(objectId, OpenMode.ForRead) as Entity));
                }
            }
            return entities;
        }
        public static T PickWrapper<T>(this Document document, string type, string message = "Select entity: ") where T : class, IElementWrapper
        {
            Entity entity = document.PickEntityOfType(type, message);
            return Activator.CreateInstance(typeof(T), entity) as T ?? throw new InvalidOperationException();
        }
    }
}
