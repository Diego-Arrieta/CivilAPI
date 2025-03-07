using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.DatabaseServices;

namespace CivilAPI.Extensions
{
    public static class EntityExtensions
    {
        public static Layout GetLayout(this Entity entity)
        {
            Database database = entity.Database; 
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
    }
}
