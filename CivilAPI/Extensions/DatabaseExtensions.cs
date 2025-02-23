using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.Aec.PropertyData;
using Autodesk.Aec.PropertyData.DatabaseServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Autodesk.AutoCAD.EditorInput;

namespace CivilAPI.Extensions
{
    public static class DatabaseExtensions
    {
        public static void Run(this Database database, Action<Transaction> action)
        {
            using (Transaction tr = database.TransactionManager.StartTransaction())
            {
                try
                {
                    action.Invoke(tr);
                    tr.Commit();
                }
                catch
                {
                    tr.Abort();
                    throw new NotImplementedException();
                }
            }
        }
        public static Layout GetLayout(this Database database, Entity entity)
        {
            Layout layout = null;

            database.Run(tr =>
            {
                BlockTableRecord blockTableRecord = tr.GetObject(entity.OwnerId, OpenMode.ForRead) as BlockTableRecord;
                if (blockTableRecord.IsLayout)
                {
                    layout = tr.GetObject(blockTableRecord.LayoutId, OpenMode.ForRead) as Layout;
                }
            });

            return layout;
        }
        public static List<PropertySetDefinition> GetPropertySetDefinitions(this Database database)
        {
            List<PropertySetDefinition> propertySetDefinitions = new List<PropertySetDefinition>();
            DictionaryPropertySetDefinitions dictionaryPropertySetDefinitions = new DictionaryPropertySetDefinitions(database);
            ObjectIdCollection propertySetDefinitionsCollection = dictionaryPropertySetDefinitions.Records as ObjectIdCollection;

            database.Run(tr =>
            {                
                foreach (ObjectId objectId in propertySetDefinitionsCollection)
                {
                    PropertySetDefinition propertySetDefinition = tr.GetObject(objectId, OpenMode.ForRead) as PropertySetDefinition;
                    if (propertySetDefinition != null)
                    {
                        propertySetDefinitions.Add(propertySetDefinition);
                    }
                }
            });
                        
            return propertySetDefinitions;
        }
    }
}
