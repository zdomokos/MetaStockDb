/*
 *  Copyright (C) 2006-2014 Robert Iwancz
 *
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
 *
 */

using System;
using System.Collections;
using System.Windows.Forms;


namespace MSFileFormat
{
    /// <summary>
    /// Summary description for Master.
    /// </summary>
    public class BaseMaster : System.Windows.Forms.Form
    {
        // Implements the manual sorting of items by columns.
        class ListViewItemComparer : IComparer
        {
            private int col;

            public ListViewItemComparer()
            {
                col = 0;
            }

            public ListViewItemComparer(int column)
            {
                col = column;
            }

            public int Compare(object x, object y)
            {
                return String.Compare(((ListViewItem)x).SubItems[col].Text,
                                      ((ListViewItem)y).SubItems[col].Text);
            }
        }

        private void StockView_ColumnClick(object sender,
                                           System.Windows.Forms.ColumnClickEventArgs e)
        {
            // Set the ListViewItemSorter property to a new 
            // ListViewItemComparer object.
            this.StockView.ListViewItemSorter = new ListViewItemComparer(e.Column);
            // Call the sort method to manually sort the column based on the 
            // ListViewItemComparer implementation.
            StockView.Sort();
        }

        private   System.Windows.Forms.GroupBox     HeaderGroupBox;
        private   System.Windows.Forms.Label        label1;
        private   System.Windows.Forms.Label        label2;
        protected System.Windows.Forms.TextBox      RecordsText;
        protected System.Windows.Forms.TextBox      MaxText;
        protected System.Windows.Forms.TextBox      RemainText;
        protected System.Windows.Forms.ColumnHeader RecNumHeader;
        protected System.Windows.Forms.ColumnHeader SymbolHeader;
        protected System.Windows.Forms.ColumnHeader NameHeader;
        protected System.Windows.Forms.ListView     StockView;
        private   System.Windows.Forms.Panel        BottomPanel;
        private   System.Windows.Forms.Panel        Toppanel;

        private System.Windows.Forms.SaveFileDialog saveFileDialog;

        //private System.Windows.Forms.ContextMenuStrip menuContext;
        //private System.Windows.Forms.ToolStripMenuItem miContextExport;
        //private System.Windows.Forms.ToolStripMenuItem miExportExportAll;
        //private System.Windows.Forms.ToolStripMenuItem miExportExport3Sym;
        //private System.Windows.Forms.ToolStripMenuItem miExportExportCustom;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public BaseMaster()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
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
            this.HeaderGroupBox = new System.Windows.Forms.GroupBox();
            this.RemainText     = new System.Windows.Forms.TextBox();
            this.MaxText        = new System.Windows.Forms.TextBox();
            this.RecordsText    = new System.Windows.Forms.TextBox();
            this.label2         = new System.Windows.Forms.Label();
            this.label1         = new System.Windows.Forms.Label();
            this.StockView      = new System.Windows.Forms.ListView();
            this.RecNumHeader   = new System.Windows.Forms.ColumnHeader();
            this.SymbolHeader   = new System.Windows.Forms.ColumnHeader();
            this.NameHeader     = new System.Windows.Forms.ColumnHeader();
            this.BottomPanel    = new System.Windows.Forms.Panel();
            this.Toppanel       = new System.Windows.Forms.Panel();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            //this.menuContext = new System.Windows.Forms.ContextMenuStrip();
            //this.miContextExport = new System.Windows.Forms.ToolStripMenuItem();
            //this.miExportExportAll = new System.Windows.Forms.ToolStripMenuItem();
            //this.miExportExport3Sym = new System.Windows.Forms.ToolStripMenuItem();
            //this.miExportExportCustom = new System.Windows.Forms.ToolStripMenuItem();
            this.HeaderGroupBox.SuspendLayout();
            this.BottomPanel.SuspendLayout();
            this.Toppanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // HeaderGroupBox
            // 
            this.HeaderGroupBox.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top |
                                                       System.Windows.Forms.AnchorStyles.Left)
                                                    | System.Windows.Forms.AnchorStyles.Right)));
            this.HeaderGroupBox.Controls.Add(this.RemainText);
            this.HeaderGroupBox.Controls.Add(this.MaxText);
            this.HeaderGroupBox.Controls.Add(this.RecordsText);
            this.HeaderGroupBox.Controls.Add(this.label2);
            this.HeaderGroupBox.Controls.Add(this.label1);
            this.HeaderGroupBox.Location = new System.Drawing.Point(8, 8);
            this.HeaderGroupBox.Name     = "HeaderGroupBox";
            this.HeaderGroupBox.Size     = new System.Drawing.Size(816, 152);
            this.HeaderGroupBox.TabIndex = 0;
            this.HeaderGroupBox.TabStop  = false;
            this.HeaderGroupBox.Text     = "Header";
            // 
            // RemainText
            // 
            this.RemainText.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top |
                                                       System.Windows.Forms.AnchorStyles.Left)
                                                    | System.Windows.Forms.AnchorStyles.Right)));
            this.RemainText.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular,
                                                           System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.RemainText.Location  = new System.Drawing.Point(184, 16);
            this.RemainText.Multiline = true;
            this.RemainText.Name      = "RemainText";
            this.RemainText.ReadOnly  = true;
            this.RemainText.Size      = new System.Drawing.Size(624, 128);
            this.RemainText.TabIndex  = 5;
            this.RemainText.Text      = "";
            // 
            // MaxText
            // 
            this.MaxText.Location = new System.Drawing.Point(88, 56);
            this.MaxText.Name     = "MaxText";
            this.MaxText.ReadOnly = true;
            this.MaxText.Size     = new System.Drawing.Size(80, 20);
            this.MaxText.TabIndex = 4;
            this.MaxText.Text     = "";
            // 
            // RecordsText
            // 
            this.RecordsText.Location = new System.Drawing.Point(88, 24);
            this.RecordsText.Name     = "RecordsText";
            this.RecordsText.ReadOnly = true;
            this.RecordsText.Size     = new System.Drawing.Size(80, 20);
            this.RecordsText.TabIndex = 3;
            this.RecordsText.Text     = "";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(8, 56);
            this.label2.Name     = "label2";
            this.label2.Size     = new System.Drawing.Size(72, 23);
            this.label2.TabIndex = 1;
            this.label2.Text     = "Max Record:";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(8, 24);
            this.label1.Name     = "label1";
            this.label1.Size     = new System.Drawing.Size(48, 23);
            this.label1.TabIndex = 0;
            this.label1.Text     = "Records:";
            // 
            // StockView
            // 
            this.StockView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[]
                                            {
                                                this.RecNumHeader,
                                                this.SymbolHeader,
                                                this.NameHeader
                                            });
            //this.StockView.ContextMenuStrip = this.menuContext;
            this.StockView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.StockView.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular,
                                                          System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.StockView.Location    =  new System.Drawing.Point(0, 0);
            this.StockView.Name        =  "StockView";
            this.StockView.Size        =  new System.Drawing.Size(840, 533);
            this.StockView.TabIndex    =  1;
            this.StockView.View        =  System.Windows.Forms.View.Details;
            this.StockView.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.StockView_ColumnClick);
            // 
            // RecNumHeader
            // 
            this.RecNumHeader.Text = "Rec";
            // 
            // SymbolHeader
            // 
            this.SymbolHeader.Text = "Symbol";
            // 
            // NameHeader
            // 
            this.NameHeader.Text  = "Name";
            this.NameHeader.Width = 271;
            // 
            // BottomPanel
            // 
            this.BottomPanel.Controls.Add(this.StockView);
            this.BottomPanel.Dock     = System.Windows.Forms.DockStyle.Fill;
            this.BottomPanel.Location = new System.Drawing.Point(0, 168);
            this.BottomPanel.Name     = "BottomPanel";
            this.BottomPanel.Size     = new System.Drawing.Size(840, 533);
            this.BottomPanel.TabIndex = 2;
            // 
            // Toppanel
            // 
            this.Toppanel.Controls.Add(this.HeaderGroupBox);
            this.Toppanel.Dock     = System.Windows.Forms.DockStyle.Top;
            this.Toppanel.Location = new System.Drawing.Point(0, 0);
            this.Toppanel.Name     = "Toppanel";
            this.Toppanel.Size     = new System.Drawing.Size(840, 168);
            this.Toppanel.TabIndex = 3;
            //// 
            //// menuContext
            //// 
            //this.menuContext.MenuItems.AddRange(new System.Windows.Forms.ToolStripMenuItem[] {
            //																			this.miContextExport});
            //// 
            //// miContextExport
            //// 
            //this.miContextExport.Index = 0;
            //this.miContextExport.MenuItems.AddRange(new System.Windows.Forms.ToolStripMenuItem[] {
            //																				this.miExportExportAll,
            //																				this.miExportExport3Sym,
            //																				this.miExportExportCustom});
            //this.miContextExport.Text = "Export";
            //// 
            //// miExportExportAll
            //// 
            //this.miExportExportAll.Index = 0;
            //this.miExportExportAll.Text = "Export All";
            //this.miExportExportAll.Click += new System.EventHandler(this.miExportExportAll_Click);
            //// 
            //// miExportExport3Sym
            //// 
            //this.miExportExport3Sym.Index = 1;
            //this.miExportExport3Sym.Text = "Export 3 char symbols";
            //this.miExportExport3Sym.Click += new System.EventHandler(this.miExportExport3Sym_Click);
            //// 
            //// miExportExportCustom
            //// 
            //this.miExportExportCustom.Index = 2;
            //this.miExportExportCustom.Text = "Custom...";
            //this.miExportExportCustom.Click += new System.EventHandler(this.miExportExportCustom_Click);
            // 
            // BaseMaster
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize        = new System.Drawing.Size(840, 701);
            this.Controls.Add(this.BottomPanel);
            this.Controls.Add(this.Toppanel);
            this.Name = "BaseMaster";
            this.Text = "BaseMaster";
            this.HeaderGroupBox.ResumeLayout(false);
            this.BottomPanel.ResumeLayout(false);
            this.Toppanel.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion


        private void miExportExportAll_Click(object sender, System.EventArgs e)
        {
            saveFileDialog.Filter = "Comma Separated Values (.csv)|*.csv|All Files (*.*)|*.*";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                ExportData(saveFileDialog.FileName, false);
            }
        }

        private void miExportExport3Sym_Click(object sender, System.EventArgs e)
        {
            saveFileDialog.Filter = "Comma Separated Values (.csv)|*.csv|All Files (*.*)|*.*";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                ExportData(saveFileDialog.FileName, true);
            }
        }

        private void miExportExportCustom_Click(object sender, System.EventArgs e)
        {
            Unimplemented();
        }

        protected virtual void ExportData(string filename, bool filter)
        {
            Unimplemented();
        }

        protected static void Unimplemented()
        {
            MessageBox.Show("Not yet implemented", "Unimplemented",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}