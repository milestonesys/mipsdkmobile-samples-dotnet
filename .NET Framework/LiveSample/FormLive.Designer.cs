namespace LiveSample
{
    partial class FormLive
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
            this.components = new System.ComponentModel.Container();
            this.pictureBoxVideo = new System.Windows.Forms.PictureBox();
            this.dataGridViewCameras = new System.Windows.Forms.DataGridView();
            this.itemNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bindingSourceViewGroupTree = new System.Windows.Forms.BindingSource(this.components);
            this.labelMotion = new System.Windows.Forms.Label();
            this.labelRecording = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxVideo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCameras)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceViewGroupTree)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBoxVideo
            // 
            this.pictureBoxVideo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxVideo.Location = new System.Drawing.Point(268, 12);
            this.pictureBoxVideo.Name = "pictureBoxVideo";
            this.pictureBoxVideo.Size = new System.Drawing.Size(602, 459);
            this.pictureBoxVideo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBoxVideo.TabIndex = 1;
            this.pictureBoxVideo.TabStop = false;
            // 
            // dataGridViewCameras
            // 
            this.dataGridViewCameras.AllowUserToAddRows = false;
            this.dataGridViewCameras.AllowUserToDeleteRows = false;
            this.dataGridViewCameras.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dataGridViewCameras.AutoGenerateColumns = false;
            this.dataGridViewCameras.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewCameras.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.itemNameDataGridViewTextBoxColumn});
            this.dataGridViewCameras.DataSource = this.bindingSourceViewGroupTree;
            this.dataGridViewCameras.Location = new System.Drawing.Point(12, 12);
            this.dataGridViewCameras.Name = "dataGridViewCameras";
            this.dataGridViewCameras.ReadOnly = true;
            this.dataGridViewCameras.RowHeadersWidth = 4;
            this.dataGridViewCameras.Size = new System.Drawing.Size(250, 457);
            this.dataGridViewCameras.TabIndex = 2;
            this.dataGridViewCameras.SelectionChanged += new System.EventHandler(this.OnDataGridViewCamerasSelectionChanged);
            // 
            // itemNameDataGridViewTextBoxColumn
            // 
            this.itemNameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.itemNameDataGridViewTextBoxColumn.DataPropertyName = "ItemName";
            this.itemNameDataGridViewTextBoxColumn.HeaderText = "Camera";
            this.itemNameDataGridViewTextBoxColumn.Name = "itemNameDataGridViewTextBoxColumn";
            this.itemNameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // bindingSourceViewGroupTree
            // 
            this.bindingSourceViewGroupTree.DataSource = typeof(VideoOS.Mobile.Portable.ViewGroupItem.ViewGroupTree);
            // 
            // labelMotion
            // 
            this.labelMotion.AutoSize = true;
            this.labelMotion.BackColor = System.Drawing.Color.GreenYellow;
            this.labelMotion.Location = new System.Drawing.Point(270, 14);
            this.labelMotion.Name = "labelMotion";
            this.labelMotion.Size = new System.Drawing.Size(39, 13);
            this.labelMotion.TabIndex = 3;
            this.labelMotion.Text = "Motion";
            // 
            // labelRecording
            // 
            this.labelRecording.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelRecording.AutoSize = true;
            this.labelRecording.BackColor = System.Drawing.Color.Red;
            this.labelRecording.Location = new System.Drawing.Point(811, 14);
            this.labelRecording.Name = "labelRecording";
            this.labelRecording.Size = new System.Drawing.Size(56, 13);
            this.labelRecording.TabIndex = 4;
            this.labelRecording.Text = "Recording";
            // 
            // FormLive
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(882, 481);
            this.Controls.Add(this.labelRecording);
            this.Controls.Add(this.labelMotion);
            this.Controls.Add(this.dataGridViewCameras);
            this.Controls.Add(this.pictureBoxVideo);
            this.Name = "FormLive";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Live";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormLiveFormClosing);
            this.Resize += new System.EventHandler(this.FormLiveResize);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxVideo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCameras)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceViewGroupTree)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox pictureBoxVideo;
        private System.Windows.Forms.DataGridView dataGridViewCameras;
        private System.Windows.Forms.BindingSource bindingSourceViewGroupTree;
        private System.Windows.Forms.DataGridViewTextBoxColumn itemNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.Label labelMotion;
        private System.Windows.Forms.Label labelRecording;
    }
}

