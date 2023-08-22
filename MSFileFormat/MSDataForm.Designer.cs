namespace MSFileFormat
{
	partial class MSDataForm
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
			this.txtFileName = new System.Windows.Forms.TextBox();
			this.btnRead = new System.Windows.Forms.Button();
			this.txtResult = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// txtFileName
			// 
			this.txtFileName.Location = new System.Drawing.Point(12, 12);
			this.txtFileName.Name = "txtFileName";
			this.txtFileName.Size = new System.Drawing.Size(100, 20);
			this.txtFileName.TabIndex = 0;
			this.txtFileName.Text = "76";
			// 
			// btnRead
			// 
			this.btnRead.Location = new System.Drawing.Point(118, 9);
			this.btnRead.Name = "btnRead";
			this.btnRead.Size = new System.Drawing.Size(75, 23);
			this.btnRead.TabIndex = 1;
			this.btnRead.Text = "Read";
			this.btnRead.UseVisualStyleBackColor = true;
			this.btnRead.Click += new System.EventHandler(this.btnRead_Click);
			// 
			// txtResult
			// 
			this.txtResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtResult.Location = new System.Drawing.Point(12, 38);
			this.txtResult.Multiline = true;
			this.txtResult.Name = "txtResult";
			this.txtResult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtResult.Size = new System.Drawing.Size(612, 392);
			this.txtResult.TabIndex = 0;
			// 
			// MSDataForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(636, 442);
			this.Controls.Add(this.btnRead);
			this.Controls.Add(this.txtResult);
			this.Controls.Add(this.txtFileName);
			this.Name = "MSDataForm";
			this.Text = "MSDataForm";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox txtFileName;
		private System.Windows.Forms.Button btnRead;
		private System.Windows.Forms.TextBox txtResult;
	}
}
