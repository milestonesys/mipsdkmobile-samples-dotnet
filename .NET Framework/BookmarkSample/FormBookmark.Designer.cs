namespace BookmarkSample
{
    partial class FormBookmark
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
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageCreate = new System.Windows.Forms.TabPage();
            this.comboBoxCamera = new System.Windows.Forms.ComboBox();
            this.labelTime = new System.Windows.Forms.Label();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.dateTimePickerTime = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.labelCamera = new System.Windows.Forms.Label();
            this.buttonCreate = new System.Windows.Forms.Button();
            this.tabPageUpdate = new System.Windows.Forms.TabPage();
            this.label2 = new System.Windows.Forms.Label();
            this.dateTimePickerNewTime = new System.Windows.Forms.DateTimePicker();
            this.textBoxNewName = new System.Windows.Forms.TextBox();
            this.labelName = new System.Windows.Forms.Label();
            this.buttonUpdate = new System.Windows.Forms.Button();
            this.labelUpdateId = new System.Windows.Forms.Label();
            this.textBoxUpdateId = new System.Windows.Forms.TextBox();
            this.tabPageDelete = new System.Windows.Forms.TabPage();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.labelDeleteId = new System.Windows.Forms.Label();
            this.textBoxDeleteId = new System.Windows.Forms.TextBox();
            this.tabPageGet = new System.Windows.Forms.TabPage();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.idDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cameraNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descriptionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.referenceDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.timeBeginDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.timeEndDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.timeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.userDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bookmarkViewEntryBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.tabControl.SuspendLayout();
            this.tabPageCreate.SuspendLayout();
            this.tabPageUpdate.SuspendLayout();
            this.tabPageDelete.SuspendLayout();
            this.tabPageGet.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bookmarkViewEntryBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPageCreate);
            this.tabControl.Controls.Add(this.tabPageUpdate);
            this.tabControl.Controls.Add(this.tabPageDelete);
            this.tabControl.Controls.Add(this.tabPageGet);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(752, 291);
            this.tabControl.TabIndex = 25;
            this.tabControl.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabControl_Selected);
            // 
            // tabPageCreate
            // 
            this.tabPageCreate.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageCreate.Controls.Add(this.comboBoxCamera);
            this.tabPageCreate.Controls.Add(this.labelTime);
            this.tabPageCreate.Controls.Add(this.textBoxName);
            this.tabPageCreate.Controls.Add(this.dateTimePickerTime);
            this.tabPageCreate.Controls.Add(this.label1);
            this.tabPageCreate.Controls.Add(this.labelCamera);
            this.tabPageCreate.Controls.Add(this.buttonCreate);
            this.tabPageCreate.Location = new System.Drawing.Point(4, 22);
            this.tabPageCreate.Name = "tabPageCreate";
            this.tabPageCreate.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageCreate.Size = new System.Drawing.Size(744, 265);
            this.tabPageCreate.TabIndex = 0;
            this.tabPageCreate.Text = "Create";
            // 
            // comboBoxCamera
            // 
            this.comboBoxCamera.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCamera.FormattingEnabled = true;
            this.comboBoxCamera.Location = new System.Drawing.Point(84, 6);
            this.comboBoxCamera.Name = "comboBoxCamera";
            this.comboBoxCamera.Size = new System.Drawing.Size(280, 21);
            this.comboBoxCamera.TabIndex = 33;
            // 
            // labelTime
            // 
            this.labelTime.AutoSize = true;
            this.labelTime.Location = new System.Drawing.Point(40, 35);
            this.labelTime.Name = "labelTime";
            this.labelTime.Size = new System.Drawing.Size(33, 13);
            this.labelTime.TabIndex = 36;
            this.labelTime.Text = "Time:";
            // 
            // textBoxName
            // 
            this.textBoxName.Location = new System.Drawing.Point(84, 61);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(280, 20);
            this.textBoxName.TabIndex = 31;
            this.textBoxName.Text = "Default Bookmark Name";
            // 
            // dateTimePickerTime
            // 
            this.dateTimePickerTime.CustomFormat = "HH:mm:ss  (dd-MM-yyyy)";
            this.dateTimePickerTime.DropDownAlign = System.Windows.Forms.LeftRightAlignment.Right;
            this.dateTimePickerTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerTime.Location = new System.Drawing.Point(84, 35);
            this.dateTimePickerTime.Name = "dateTimePickerTime";
            this.dateTimePickerTime.Size = new System.Drawing.Size(280, 20);
            this.dateTimePickerTime.TabIndex = 35;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(35, 65);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 32;
            this.label1.Text = "Name:";
            // 
            // labelCamera
            // 
            this.labelCamera.AutoSize = true;
            this.labelCamera.Location = new System.Drawing.Point(31, 9);
            this.labelCamera.Name = "labelCamera";
            this.labelCamera.Size = new System.Drawing.Size(46, 13);
            this.labelCamera.TabIndex = 34;
            this.labelCamera.Text = "Camera:";
            // 
            // buttonCreate
            // 
            this.buttonCreate.Location = new System.Drawing.Point(417, 7);
            this.buttonCreate.Name = "buttonCreate";
            this.buttonCreate.Size = new System.Drawing.Size(225, 28);
            this.buttonCreate.TabIndex = 25;
            this.buttonCreate.Text = "Create";
            this.buttonCreate.UseVisualStyleBackColor = true;
            this.buttonCreate.Click += new System.EventHandler(this.buttonCreate_Click);
            // 
            // tabPageUpdate
            // 
            this.tabPageUpdate.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageUpdate.Controls.Add(this.label2);
            this.tabPageUpdate.Controls.Add(this.dateTimePickerNewTime);
            this.tabPageUpdate.Controls.Add(this.textBoxNewName);
            this.tabPageUpdate.Controls.Add(this.labelName);
            this.tabPageUpdate.Controls.Add(this.buttonUpdate);
            this.tabPageUpdate.Controls.Add(this.labelUpdateId);
            this.tabPageUpdate.Controls.Add(this.textBoxUpdateId);
            this.tabPageUpdate.Location = new System.Drawing.Point(4, 22);
            this.tabPageUpdate.Name = "tabPageUpdate";
            this.tabPageUpdate.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageUpdate.Size = new System.Drawing.Size(744, 265);
            this.tabPageUpdate.TabIndex = 1;
            this.tabPageUpdate.Text = "Update";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(44, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 13);
            this.label2.TabIndex = 38;
            this.label2.Text = "Time:";
            // 
            // dateTimePickerNewTime
            // 
            this.dateTimePickerNewTime.CustomFormat = "HH:mm:ss  (dd-MM-yyyy)";
            this.dateTimePickerNewTime.DropDownAlign = System.Windows.Forms.LeftRightAlignment.Right;
            this.dateTimePickerNewTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerNewTime.Location = new System.Drawing.Point(85, 35);
            this.dateTimePickerNewTime.Name = "dateTimePickerNewTime";
            this.dateTimePickerNewTime.Size = new System.Drawing.Size(280, 20);
            this.dateTimePickerNewTime.TabIndex = 37;
            // 
            // textBoxNewName
            // 
            this.textBoxNewName.Location = new System.Drawing.Point(85, 63);
            this.textBoxNewName.Name = "textBoxNewName";
            this.textBoxNewName.Size = new System.Drawing.Size(280, 20);
            this.textBoxNewName.TabIndex = 33;
            this.textBoxNewName.Text = "New Bookmark Name";
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(17, 66);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(61, 13);
            this.labelName.TabIndex = 34;
            this.labelName.Text = "New name:";
            // 
            // buttonUpdate
            // 
            this.buttonUpdate.Location = new System.Drawing.Point(417, 7);
            this.buttonUpdate.Name = "buttonUpdate";
            this.buttonUpdate.Size = new System.Drawing.Size(225, 28);
            this.buttonUpdate.TabIndex = 32;
            this.buttonUpdate.Text = "Update";
            this.buttonUpdate.UseVisualStyleBackColor = true;
            this.buttonUpdate.Click += new System.EventHandler(this.buttonUpdate_Click);
            // 
            // labelUpdateId
            // 
            this.labelUpdateId.AutoSize = true;
            this.labelUpdateId.Location = new System.Drawing.Point(8, 7);
            this.labelUpdateId.Name = "labelUpdateId";
            this.labelUpdateId.Size = new System.Drawing.Size(72, 13);
            this.labelUpdateId.TabIndex = 31;
            this.labelUpdateId.Text = "Bookmark ID:";
            // 
            // textBoxUpdateId
            // 
            this.textBoxUpdateId.Location = new System.Drawing.Point(85, 7);
            this.textBoxUpdateId.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxUpdateId.Name = "textBoxUpdateId";
            this.textBoxUpdateId.Size = new System.Drawing.Size(280, 20);
            this.textBoxUpdateId.TabIndex = 28;
            // 
            // tabPageDelete
            // 
            this.tabPageDelete.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageDelete.Controls.Add(this.buttonDelete);
            this.tabPageDelete.Controls.Add(this.labelDeleteId);
            this.tabPageDelete.Controls.Add(this.textBoxDeleteId);
            this.tabPageDelete.Location = new System.Drawing.Point(4, 22);
            this.tabPageDelete.Name = "tabPageDelete";
            this.tabPageDelete.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDelete.Size = new System.Drawing.Size(744, 265);
            this.tabPageDelete.TabIndex = 2;
            this.tabPageDelete.Text = "Delete";
            // 
            // buttonDelete
            // 
            this.buttonDelete.Location = new System.Drawing.Point(418, 7);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(225, 28);
            this.buttonDelete.TabIndex = 35;
            this.buttonDelete.Text = "Delete";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // labelDeleteId
            // 
            this.labelDeleteId.AutoSize = true;
            this.labelDeleteId.Location = new System.Drawing.Point(8, 7);
            this.labelDeleteId.Name = "labelDeleteId";
            this.labelDeleteId.Size = new System.Drawing.Size(72, 13);
            this.labelDeleteId.TabIndex = 34;
            this.labelDeleteId.Text = "Bookmark ID:";
            // 
            // textBoxDeleteId
            // 
            this.textBoxDeleteId.Location = new System.Drawing.Point(86, 7);
            this.textBoxDeleteId.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxDeleteId.Name = "textBoxDeleteId";
            this.textBoxDeleteId.Size = new System.Drawing.Size(280, 20);
            this.textBoxDeleteId.TabIndex = 33;
            // 
            // tabPageGet
            // 
            this.tabPageGet.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageGet.Controls.Add(this.dataGridView);
            this.tabPageGet.Location = new System.Drawing.Point(4, 22);
            this.tabPageGet.Margin = new System.Windows.Forms.Padding(2);
            this.tabPageGet.Name = "tabPageGet";
            this.tabPageGet.Padding = new System.Windows.Forms.Padding(2);
            this.tabPageGet.Size = new System.Drawing.Size(744, 265);
            this.tabPageGet.TabIndex = 3;
            this.tabPageGet.Text = "Get";
            // 
            // dataGridView
            // 
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.idDataGridViewTextBoxColumn,
            this.cameraNameDataGridViewTextBoxColumn,
            this.descriptionDataGridViewTextBoxColumn,
            this.nameDataGridViewTextBoxColumn,
            this.referenceDataGridViewTextBoxColumn,
            this.timeBeginDataGridViewTextBoxColumn,
            this.timeEndDataGridViewTextBoxColumn,
            this.timeDataGridViewTextBoxColumn,
            this.userDataGridViewTextBoxColumn});
            this.dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView.Location = new System.Drawing.Point(2, 2);
            this.dataGridView.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.ReadOnly = true;
            this.dataGridView.RowHeadersWidth = 51;
            this.dataGridView.RowTemplate.Height = 24;
            this.dataGridView.Size = new System.Drawing.Size(740, 261);
            this.dataGridView.TabIndex = 0;
            // 
            // idDataGridViewTextBoxColumn
            // 
            this.idDataGridViewTextBoxColumn.DataPropertyName = "Id";
            this.idDataGridViewTextBoxColumn.HeaderText = "Id";
            this.idDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.idDataGridViewTextBoxColumn.Name = "idDataGridViewTextBoxColumn";
            this.idDataGridViewTextBoxColumn.ReadOnly = true;
            this.idDataGridViewTextBoxColumn.Width = 125;
            // 
            // cameraNameDataGridViewTextBoxColumn
            // 
            this.cameraNameDataGridViewTextBoxColumn.DataPropertyName = "CameraName";
            this.cameraNameDataGridViewTextBoxColumn.HeaderText = "CameraName";
            this.cameraNameDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.cameraNameDataGridViewTextBoxColumn.Name = "cameraNameDataGridViewTextBoxColumn";
            this.cameraNameDataGridViewTextBoxColumn.ReadOnly = true;
            this.cameraNameDataGridViewTextBoxColumn.Width = 125;
            // 
            // descriptionDataGridViewTextBoxColumn
            // 
            this.descriptionDataGridViewTextBoxColumn.DataPropertyName = "Description";
            this.descriptionDataGridViewTextBoxColumn.HeaderText = "Description";
            this.descriptionDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.descriptionDataGridViewTextBoxColumn.Name = "descriptionDataGridViewTextBoxColumn";
            this.descriptionDataGridViewTextBoxColumn.ReadOnly = true;
            this.descriptionDataGridViewTextBoxColumn.Width = 125;
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn.HeaderText = "Name";
            this.nameDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            this.nameDataGridViewTextBoxColumn.ReadOnly = true;
            this.nameDataGridViewTextBoxColumn.Width = 125;
            // 
            // referenceDataGridViewTextBoxColumn
            // 
            this.referenceDataGridViewTextBoxColumn.DataPropertyName = "Reference";
            this.referenceDataGridViewTextBoxColumn.HeaderText = "Reference";
            this.referenceDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.referenceDataGridViewTextBoxColumn.Name = "referenceDataGridViewTextBoxColumn";
            this.referenceDataGridViewTextBoxColumn.ReadOnly = true;
            this.referenceDataGridViewTextBoxColumn.Width = 125;
            // 
            // timeBeginDataGridViewTextBoxColumn
            // 
            this.timeBeginDataGridViewTextBoxColumn.DataPropertyName = "TimeBegin";
            this.timeBeginDataGridViewTextBoxColumn.HeaderText = "TimeBegin";
            this.timeBeginDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.timeBeginDataGridViewTextBoxColumn.Name = "timeBeginDataGridViewTextBoxColumn";
            this.timeBeginDataGridViewTextBoxColumn.ReadOnly = true;
            this.timeBeginDataGridViewTextBoxColumn.Width = 125;
            // 
            // timeEndDataGridViewTextBoxColumn
            // 
            this.timeEndDataGridViewTextBoxColumn.DataPropertyName = "TimeEnd";
            this.timeEndDataGridViewTextBoxColumn.HeaderText = "TimeEnd";
            this.timeEndDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.timeEndDataGridViewTextBoxColumn.Name = "timeEndDataGridViewTextBoxColumn";
            this.timeEndDataGridViewTextBoxColumn.ReadOnly = true;
            this.timeEndDataGridViewTextBoxColumn.Width = 125;
            // 
            // timeDataGridViewTextBoxColumn
            // 
            this.timeDataGridViewTextBoxColumn.DataPropertyName = "Time";
            this.timeDataGridViewTextBoxColumn.HeaderText = "Time";
            this.timeDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.timeDataGridViewTextBoxColumn.Name = "timeDataGridViewTextBoxColumn";
            this.timeDataGridViewTextBoxColumn.ReadOnly = true;
            this.timeDataGridViewTextBoxColumn.Width = 125;
            // 
            // userDataGridViewTextBoxColumn
            // 
            this.userDataGridViewTextBoxColumn.DataPropertyName = "User";
            this.userDataGridViewTextBoxColumn.HeaderText = "User";
            this.userDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.userDataGridViewTextBoxColumn.Name = "userDataGridViewTextBoxColumn";
            this.userDataGridViewTextBoxColumn.ReadOnly = true;
            this.userDataGridViewTextBoxColumn.Width = 125;
            // 
            // bookmarkViewEntryBindingSource
            // 
            this.bookmarkViewEntryBindingSource.DataSource = typeof(BookmarkSample.BookmarkViewEntry);
            // 
            // FormBookmark
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(752, 291);
            this.Controls.Add(this.tabControl);
            this.Name = "FormBookmark";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormBookmark_FormClosing);
            this.tabControl.ResumeLayout(false);
            this.tabPageCreate.ResumeLayout(false);
            this.tabPageCreate.PerformLayout();
            this.tabPageUpdate.ResumeLayout(false);
            this.tabPageUpdate.PerformLayout();
            this.tabPageDelete.ResumeLayout(false);
            this.tabPageDelete.PerformLayout();
            this.tabPageGet.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bookmarkViewEntryBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPageCreate;
        private System.Windows.Forms.TabPage tabPageUpdate;
        private System.Windows.Forms.TabPage tabPageDelete;
        private System.Windows.Forms.Button buttonCreate;
        private System.Windows.Forms.TextBox textBoxUpdateId;
        private System.Windows.Forms.Button buttonUpdate;
        private System.Windows.Forms.Label labelUpdateId;
        private System.Windows.Forms.TextBox textBoxNewName;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.ComboBox comboBoxCamera;
        private System.Windows.Forms.Label labelTime;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.DateTimePicker dateTimePickerTime;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelCamera;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dateTimePickerNewTime;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.Label labelDeleteId;
        private System.Windows.Forms.TextBox textBoxDeleteId;
        private System.Windows.Forms.TabPage tabPageGet;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn idDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cameraNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn descriptionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn referenceDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn timeBeginDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn timeEndDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn timeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn userDataGridViewTextBoxColumn;
        private System.Windows.Forms.BindingSource bookmarkViewEntryBindingSource;
    }
}

