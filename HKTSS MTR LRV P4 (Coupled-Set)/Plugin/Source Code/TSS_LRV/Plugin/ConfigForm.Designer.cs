
namespace Plugin {
    partial class ConfigForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
			this.Line1 = new System.Windows.Forms.Label();
			this.TrainCarNumberLabel = new System.Windows.Forms.Label();
			this.FirstCarLabel = new System.Windows.Forms.Label();
			this.CarNum1Box = new System.Windows.Forms.NumericUpDown();
			this.SecondCarLabel = new System.Windows.Forms.Label();
			this.CarNum2Box = new System.Windows.Forms.NumericUpDown();
			this.ApplyChanges = new System.Windows.Forms.Button();
			this.Line2 = new System.Windows.Forms.Label();
			this.SafetySystemLabel = new System.Windows.Forms.Label();
			this.DoorLockCheckBox = new System.Windows.Forms.CheckBox();
			this.ApplyBrakeCheckBox = new System.Windows.Forms.CheckBox();
			this.iSPSDoorLockEnabled = new System.Windows.Forms.CheckBox();
			this.Line3 = new System.Windows.Forms.Label();
			this.MiscLabel = new System.Windows.Forms.Label();
			this.CrashCheckBox = new System.Windows.Forms.CheckBox();
			this.MTRBeeping = new System.Windows.Forms.CheckBox();
			this.RevAtStation = new System.Windows.Forms.CheckBox();
			this.TrainStatusLabel = new System.Windows.Forms.Label();
			this.TrainStatusBox = new System.Windows.Forms.ComboBox();
			((System.ComponentModel.ISupportInitialize)(this.CarNum1Box)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.CarNum2Box)).BeginInit();
			this.SuspendLayout();
			// 
			// Line1
			// 
			this.Line1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.Line1.Location = new System.Drawing.Point(-8, 18);
			this.Line1.MaximumSize = new System.Drawing.Size(10000, 2);
			this.Line1.MinimumSize = new System.Drawing.Size(0, 2);
			this.Line1.Name = "Line1";
			this.Line1.Size = new System.Drawing.Size(376, 2);
			this.Line1.TabIndex = 0;
			// 
			// TrainCarNumberLabel
			// 
			this.TrainCarNumberLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.TrainCarNumberLabel.Font = new System.Drawing.Font("Microsoft Tai Le", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.TrainCarNumberLabel.Location = new System.Drawing.Point(78, 9);
			this.TrainCarNumberLabel.Name = "TrainCarNumberLabel";
			this.TrainCarNumberLabel.Size = new System.Drawing.Size(82, 21);
			this.TrainCarNumberLabel.TabIndex = 1;
			this.TrainCarNumberLabel.Text = "Car Number";
			this.TrainCarNumberLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// FirstCarLabel
			// 
			this.FirstCarLabel.AutoSize = true;
			this.FirstCarLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
			this.FirstCarLabel.Location = new System.Drawing.Point(17, 42);
			this.FirstCarLabel.Name = "FirstCarLabel";
			this.FirstCarLabel.Size = new System.Drawing.Size(70, 20);
			this.FirstCarLabel.TabIndex = 2;
			this.FirstCarLabel.Text = "First car:";
			// 
			// CarNum1Box
			// 
			this.CarNum1Box.Location = new System.Drawing.Point(110, 44);
			this.CarNum1Box.Maximum = new decimal(new int[] {
            1220,
            0,
            0,
            0});
			this.CarNum1Box.Minimum = new decimal(new int[] {
            1001,
            0,
            0,
            0});
			this.CarNum1Box.Name = "CarNum1Box";
			this.CarNum1Box.Size = new System.Drawing.Size(82, 20);
			this.CarNum1Box.TabIndex = 4;
			this.CarNum1Box.Value = new decimal(new int[] {
            1001,
            0,
            0,
            0});
			this.CarNum1Box.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.CarNumFilter);
			// 
			// SecondCarLabel
			// 
			this.SecondCarLabel.AutoSize = true;
			this.SecondCarLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
			this.SecondCarLabel.Location = new System.Drawing.Point(16, 73);
			this.SecondCarLabel.Name = "SecondCarLabel";
			this.SecondCarLabel.Size = new System.Drawing.Size(94, 20);
			this.SecondCarLabel.TabIndex = 5;
			this.SecondCarLabel.Text = "Second car:";
			// 
			// CarNum2Box
			// 
			this.CarNum2Box.Location = new System.Drawing.Point(110, 74);
			this.CarNum2Box.Maximum = new decimal(new int[] {
            1220,
            0,
            0,
            0});
			this.CarNum2Box.Minimum = new decimal(new int[] {
            1001,
            0,
            0,
            0});
			this.CarNum2Box.Name = "CarNum2Box";
			this.CarNum2Box.Size = new System.Drawing.Size(82, 20);
			this.CarNum2Box.TabIndex = 6;
			this.CarNum2Box.Value = new decimal(new int[] {
            1001,
            0,
            0,
            0});
			this.CarNum2Box.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.CarNumFilter);
			// 
			// ApplyChanges
			// 
			this.ApplyChanges.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.ApplyChanges.Location = new System.Drawing.Point(156, 372);
			this.ApplyChanges.Name = "ApplyChanges";
			this.ApplyChanges.Size = new System.Drawing.Size(75, 23);
			this.ApplyChanges.TabIndex = 7;
			this.ApplyChanges.Text = "OK";
			this.ApplyChanges.UseVisualStyleBackColor = true;
			this.ApplyChanges.Click += new System.EventHandler(this.ApplyChanges_Click);
			// 
			// Line2
			// 
			this.Line2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.Line2.Location = new System.Drawing.Point(-74, 215);
			this.Line2.MaximumSize = new System.Drawing.Size(10000, 2);
			this.Line2.MinimumSize = new System.Drawing.Size(0, 2);
			this.Line2.Name = "Line2";
			this.Line2.Size = new System.Drawing.Size(376, 2);
			this.Line2.TabIndex = 8;
			// 
			// SafetySystemLabel
			// 
			this.SafetySystemLabel.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.SafetySystemLabel.Font = new System.Drawing.Font("Microsoft Tai Le", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.SafetySystemLabel.Location = new System.Drawing.Point(77, 208);
			this.SafetySystemLabel.Name = "SafetySystemLabel";
			this.SafetySystemLabel.Size = new System.Drawing.Size(87, 19);
			this.SafetySystemLabel.TabIndex = 9;
			this.SafetySystemLabel.Text = "Safety System";
			this.SafetySystemLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// DoorLockCheckBox
			// 
			this.DoorLockCheckBox.Location = new System.Drawing.Point(10, 230);
			this.DoorLockCheckBox.Name = "DoorLockCheckBox";
			this.DoorLockCheckBox.Size = new System.Drawing.Size(203, 26);
			this.DoorLockCheckBox.TabIndex = 10;
			this.DoorLockCheckBox.Text = "Lock all doors when the train departs";
			this.DoorLockCheckBox.UseVisualStyleBackColor = true;
			// 
			// ApplyBrakeCheckBox
			// 
			this.ApplyBrakeCheckBox.Location = new System.Drawing.Point(10, 262);
			this.ApplyBrakeCheckBox.Name = "ApplyBrakeCheckBox";
			this.ApplyBrakeCheckBox.Size = new System.Drawing.Size(208, 26);
			this.ApplyBrakeCheckBox.TabIndex = 11;
			this.ApplyBrakeCheckBox.Text = "Apply brake when the door is opened";
			this.ApplyBrakeCheckBox.UseVisualStyleBackColor = true;
			// 
			// iSPSDoorLockEnabled
			// 
			this.iSPSDoorLockEnabled.Location = new System.Drawing.Point(10, 293);
			this.iSPSDoorLockEnabled.Name = "iSPSDoorLockEnabled";
			this.iSPSDoorLockEnabled.Size = new System.Drawing.Size(208, 31);
			this.iSPSDoorLockEnabled.TabIndex = 12;
			this.iSPSDoorLockEnabled.Text = "Apply brake until driver opens the door when approaching a station";
			this.iSPSDoorLockEnabled.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.iSPSDoorLockEnabled.UseVisualStyleBackColor = true;
			// 
			// Line3
			// 
			this.Line3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.Line3.Location = new System.Drawing.Point(-74, 116);
			this.Line3.MaximumSize = new System.Drawing.Size(10000, 2);
			this.Line3.MinimumSize = new System.Drawing.Size(0, 2);
			this.Line3.Name = "Line3";
			this.Line3.Size = new System.Drawing.Size(376, 2);
			this.Line3.TabIndex = 13;
			// 
			// MiscLabel
			// 
			this.MiscLabel.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.MiscLabel.AutoSize = true;
			this.MiscLabel.Font = new System.Drawing.Font("Microsoft Tai Le", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.MiscLabel.Location = new System.Drawing.Point(82, 109);
			this.MiscLabel.Name = "MiscLabel";
			this.MiscLabel.Size = new System.Drawing.Size(80, 16);
			this.MiscLabel.TabIndex = 14;
			this.MiscLabel.Text = "Train Settings";
			this.MiscLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// CrashCheckBox
			// 
			this.CrashCheckBox.AutoSize = true;
			this.CrashCheckBox.Location = new System.Drawing.Point(10, 131);
			this.CrashCheckBox.Name = "CrashCheckBox";
			this.CrashCheckBox.Size = new System.Drawing.Size(221, 17);
			this.CrashCheckBox.TabIndex = 15;
			this.CrashCheckBox.Text = "Crash effect when hitting the trains infront";
			this.CrashCheckBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.CrashCheckBox.UseVisualStyleBackColor = true;
			// 
			// MTRBeeping
			// 
			this.MTRBeeping.AutoSize = true;
			this.MTRBeeping.Location = new System.Drawing.Point(10, 157);
			this.MTRBeeping.Name = "MTRBeeping";
			this.MTRBeeping.Size = new System.Drawing.Size(169, 17);
			this.MTRBeeping.TabIndex = 16;
			this.MTRBeeping.Text = "Use MTR Door Close Beeping";
			this.MTRBeeping.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.MTRBeeping.UseVisualStyleBackColor = true;
			// 
			// RevAtStation
			// 
			this.RevAtStation.AutoSize = true;
			this.RevAtStation.Location = new System.Drawing.Point(10, 333);
			this.RevAtStation.Name = "RevAtStation";
			this.RevAtStation.Size = new System.Drawing.Size(226, 17);
			this.RevAtStation.TabIndex = 17;
			this.RevAtStation.Text = "Allow reversing after approaching a station";
			this.RevAtStation.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.RevAtStation.UseVisualStyleBackColor = true;
			// 
			// TrainStatusLabel
			// 
			this.TrainStatusLabel.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.TrainStatusLabel.AutoSize = true;
			this.TrainStatusLabel.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.TrainStatusLabel.Location = new System.Drawing.Point(6, 182);
			this.TrainStatusLabel.Name = "TrainStatusLabel";
			this.TrainStatusLabel.Size = new System.Drawing.Size(86, 16);
			this.TrainStatusLabel.TabIndex = 18;
			this.TrainStatusLabel.Text = "Train Status: ";
			this.TrainStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// TrainStatusBox
			// 
			this.TrainStatusBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.TrainStatusBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.TrainStatusBox.DropDownWidth = 144;
			this.TrainStatusBox.FormattingEnabled = true;
			this.TrainStatusBox.Items.AddRange(new object[] {
            "None",
            "NOT TO GO (KCR)",
            "NOT TO GO (MTR)",
            "SCOTCH BLOCK (MTR)"});
			this.TrainStatusBox.Location = new System.Drawing.Point(100, 181);
			this.TrainStatusBox.Name = "TrainStatusBox";
			this.TrainStatusBox.Size = new System.Drawing.Size(136, 21);
			this.TrainStatusBox.TabIndex = 19;
			// 
			// ConfigForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(243, 407);
			this.Controls.Add(this.TrainStatusBox);
			this.Controls.Add(this.TrainStatusLabel);
			this.Controls.Add(this.RevAtStation);
			this.Controls.Add(this.MTRBeeping);
			this.Controls.Add(this.CarNum2Box);
			this.Controls.Add(this.CarNum1Box);
			this.Controls.Add(this.CrashCheckBox);
			this.Controls.Add(this.MiscLabel);
			this.Controls.Add(this.Line3);
			this.Controls.Add(this.iSPSDoorLockEnabled);
			this.Controls.Add(this.ApplyBrakeCheckBox);
			this.Controls.Add(this.DoorLockCheckBox);
			this.Controls.Add(this.SafetySystemLabel);
			this.Controls.Add(this.Line2);
			this.Controls.Add(this.ApplyChanges);
			this.Controls.Add(this.SecondCarLabel);
			this.Controls.Add(this.FirstCarLabel);
			this.Controls.Add(this.TrainCarNumberLabel);
			this.Controls.Add(this.Line1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ConfigForm";
			this.ShowIcon = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Train Configuration";
			this.Load += new System.EventHandler(this.ConfigForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.CarNum1Box)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.CarNum2Box)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Line1;
        private System.Windows.Forms.Label TrainCarNumberLabel;
        private System.Windows.Forms.Label FirstCarLabel;
        private System.Windows.Forms.NumericUpDown CarNum1Box;
        private System.Windows.Forms.Label SecondCarLabel;
        private System.Windows.Forms.NumericUpDown CarNum2Box;
        private System.Windows.Forms.Button ApplyChanges;
        private System.Windows.Forms.Label Line2;
        private System.Windows.Forms.Label SafetySystemLabel;
        private System.Windows.Forms.CheckBox DoorLockCheckBox;
        private System.Windows.Forms.CheckBox ApplyBrakeCheckBox;
        private System.Windows.Forms.CheckBox iSPSDoorLockEnabled;
        private System.Windows.Forms.Label Line3;
        private System.Windows.Forms.Label MiscLabel;
        private System.Windows.Forms.CheckBox CrashCheckBox;
        private System.Windows.Forms.CheckBox MTRBeeping;
		private System.Windows.Forms.CheckBox RevAtStation;
		private System.Windows.Forms.Label TrainStatusLabel;
		private System.Windows.Forms.ComboBox TrainStatusBox;
	}
}