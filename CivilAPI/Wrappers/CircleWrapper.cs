using Autodesk.AutoCAD.DatabaseServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CivilAPI.Wrappers
{
    public class CircleWrapper : IElementWrapper
    {
        private readonly Circle circle;
        private readonly Database database;
        public Entity GetEntity()
        {
            return circle;
        }
        public CircleWrapper(Circle circle)
        {
            this.circle = circle;
            this.database = circle.Database;
        }
    }
}
