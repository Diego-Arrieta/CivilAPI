namespace CivilAPI.Forms
{
    partial class PropertySetsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.cmbPSDs = new System.Windows.Forms.ComboBox();
            this.tvwControl = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // cmbPSDs
            // 
            this.cmbPSDs.FormattingEnabled = true;
            this.cmbPSDs.Location = new System.Drawing.Point(12, 12);
            this.cmbPSDs.Name = "cmbPSDs";
            this.cmbPSDs.Size = new System.Drawing.Size(121, 21);
            this.cmbPSDs.TabIndex = 0;
            // 
            // tvwControl
            // 
            this.tvwControl.Location = new System.Drawing.Point(13, 40);
            this.tvwControl.Name = "tvwControl";
            this.tvwControl.Size = new System.Drawing.Size(121, 97);
            this.tvwControl.TabIndex = 1;
            // 
            // PropertySetsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(334, 411);
            this.Controls.Add(this.tvwControl);
            this.Controls.Add(this.cmbPSDs);
            this.Name = "PropertySetsForm";
            this.Text = "PropertySetsForm";
            this.Load += new System.EventHandler(this.PropertySetsForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbPSDs;
        private System.Windows.Forms.TreeView tvwControl;
    }
}