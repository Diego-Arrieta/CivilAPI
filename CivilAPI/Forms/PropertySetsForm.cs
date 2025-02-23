using Autodesk.Aec.PropertyData.DatabaseServices;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using CivilAPI.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CivilAPI.Forms
{
    public partial class PropertySetsForm : Form
    {
        Document document;
        Database database;
        Editor editor;

        public PropertySetsForm(Document document, Database database, Editor editor)
        {
            this.document = document;
            this.database = database;
            this.editor = editor;

            InitializeComponent();
        }

        private void PropertySetsForm_Load(object sender, EventArgs e)
        {
            List<string> propertySetDefinitionName = new List<string>();
            List<PropertySetDefinition> propertySetDefinitions = database.GetPropertySetDefinitions();

            foreach (PropertySetDefinition propertySetDefinition in propertySetDefinitions)
            {
                propertySetDefinitionName.Add(propertySetDefinition.Name);
            }
            cmbPSDs.DataSource = propertySetDefinitionName;
        }
    }
}
