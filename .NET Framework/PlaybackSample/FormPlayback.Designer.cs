namespace PlaybackSample
{
    partial class FormPlayback
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPlayback));
            this.treeViewViews = new System.Windows.Forms.TreeView();
            this.imageListIcons = new System.Windows.Forms.ImageList(this.components);
            this.buttonBack = new System.Windows.Forms.Button();
            this.buttonForward = new System.Windows.Forms.Button();
            this.pictureBoxVideo = new System.Windows.Forms.PictureBox();
            this.dateTimePickerGoTo = new System.Windows.Forms.DateTimePicker();
            this.labelCurrentTime = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxVideo)).BeginInit();
            this.SuspendLayout();
            // 
            // treeViewViews
            // 
            this.treeViewViews.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.treeViewViews.ImageIndex = 0;
            this.treeViewViews.ImageList = this.imageListIcons;
            this.treeViewViews.Location = new System.Drawing.Point(12, 12);
            this.treeViewViews.Name = "treeViewViews";
            this.treeViewViews.SelectedImageIndex = 0;
            this.treeViewViews.Size = new System.Drawing.Size(259, 290);
            this.treeViewViews.TabIndex = 0;
            this.treeViewViews.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.TreeViewViewsNodeMouseClick);
            // 
            // imageListIcons
            // 
            this.imageListIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListIcons.ImageStream")));
            this.imageListIcons.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListIcons.Images.SetKeyName(0, "Folder.png");
            this.imageListIcons.Images.SetKeyName(1, "View.png");
            this.imageListIcons.Images.SetKeyName(2, "Camera.png");
            this.imageListIcons.Images.SetKeyName(3, "Carrousel.png");
            this.imageListIcons.Images.SetKeyName(4, "Map.png");
            // 
            // buttonBack
            // 
            this.buttonBack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonBack.Location = new System.Drawing.Point(12, 325);
            this.buttonBack.Name = "buttonBack";
            this.buttonBack.Size = new System.Drawing.Size(75, 23);
            this.buttonBack.TabIndex = 1;
            this.buttonBack.Text = "<";
            this.buttonBack.UseVisualStyleBackColor = true;
            this.buttonBack.Click += new System.EventHandler(this.OnButtonBackClick);
            // 
            // buttonForward
            // 
            this.buttonForward.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonForward.Location = new System.Drawing.Point(196, 325);
            this.buttonForward.Name = "buttonForward";
            this.buttonForward.Size = new System.Drawing.Size(75, 23);
            this.buttonForward.TabIndex = 2;
            this.buttonForward.Text = ">";
            this.buttonForward.UseVisualStyleBackColor = true;
            this.buttonForward.Click += new System.EventHandler(this.OnButtonForwardClick);
            // 
            // pictureBoxVideo
            // 
            this.pictureBoxVideo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxVideo.Location = new System.Drawing.Point(277, 12);
            this.pictureBoxVideo.Name = "pictureBoxVideo";
            this.pictureBoxVideo.Size = new System.Drawing.Size(450, 336);
            this.pictureBoxVideo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBoxVideo.TabIndex = 3;
            this.pictureBoxVideo.TabStop = false;
            // 
            // dateTimePickerGoTo
            // 
            this.dateTimePickerGoTo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.dateTimePickerGoTo.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dateTimePickerGoTo.Location = new System.Drawing.Point(93, 325);
            this.dateTimePickerGoTo.Name = "dateTimePickerGoTo";
            this.dateTimePickerGoTo.Size = new System.Drawing.Size(97, 20);
            this.dateTimePickerGoTo.TabIndex = 4;
            this.dateTimePickerGoTo.ValueChanged += new System.EventHandler(this.GoToTime);
            // 
            // labelCurrentTime
            // 
            this.labelCurrentTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelCurrentTime.AutoSize = true;
            this.labelCurrentTime.Location = new System.Drawing.Point(12, 305);
            this.labelCurrentTime.Name = "labelCurrentTime";
            this.labelCurrentTime.Size = new System.Drawing.Size(89, 13);
            this.labelCurrentTime.TabIndex = 5;
            this.labelCurrentTime.Text = "Current time: N/A";
            // 
            // FormPlayback
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(739, 360);
            this.Controls.Add(this.labelCurrentTime);
            this.Controls.Add(this.dateTimePickerGoTo);
            this.Controls.Add(this.pictureBoxVideo);
            this.Controls.Add(this.buttonForward);
            this.Controls.Add(this.buttonBack);
            this.Controls.Add(this.treeViewViews);
            this.Name = "FormPlayback";
            this.Text = "Playback";
            this.Resize += new System.EventHandler(this.FormPlaybackResize);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxVideo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView treeViewViews;
        private System.Windows.Forms.Button buttonBack;
        private System.Windows.Forms.Button buttonForward;
        private System.Windows.Forms.PictureBox pictureBoxVideo;
        private System.Windows.Forms.ImageList imageListIcons;
        private System.Windows.Forms.DateTimePicker dateTimePickerGoTo;
        private System.Windows.Forms.Label labelCurrentTime;
    }
}

