/* 
ServerChecker v4 operates and manages various kinds of software on server systems.
Copyright (C) 2010 Stijn Devriendt

This program is free software; you can redistribute it and/or
modify it under the terms of the GNU General Public License
as published by the Free Software Foundation; either version 2
of the License.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software
Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
*/

namespace SC.DefaultPlugins.GUI
{
    partial class TimerPluginPanel
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lstActivations = new System.Windows.Forms.ListBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tsRepeat = new SC.GUI.Utility.TimeSpanSelector();
            this.lblRepeat = new System.Windows.Forms.Label();
            this.lblDuration = new System.Windows.Forms.Label();
            this.lblDurationType = new System.Windows.Forms.Label();
            this.lblStartTime = new System.Windows.Forms.Label();
            this.tsDuration = new SC.GUI.Utility.TimeSpanSelector();
            this.dtDate = new System.Windows.Forms.DateTimePicker();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.radTrueDuration = new System.Windows.Forms.RadioButton();
            this.radStopEarly = new System.Windows.Forms.RadioButton();
            this.lblDefault = new System.Windows.Forms.Label();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.radEnabled = new System.Windows.Forms.RadioButton();
            this.radDisabled = new System.Windows.Forms.RadioButton();
            this.grpActivation = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnApply = new System.Windows.Forms.Button();
            this.timeSpanSelector1 = new SC.GUI.Utility.TimeSpanSelector();
            this.timeSpanSelector2 = new SC.GUI.Utility.TimeSpanSelector();
            this.tableLayoutPanel2.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.grpActivation.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lstActivations
            // 
            this.lstActivations.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstActivations.FormattingEnabled = true;
            this.lstActivations.Location = new System.Drawing.Point(113, 162);
            this.lstActivations.Name = "lstActivations";
            this.lstActivations.Size = new System.Drawing.Size(366, 95);
            this.lstActivations.TabIndex = 10;
            this.lstActivations.SelectedIndexChanged += new System.EventHandler(this.lstActivations_SelectedIndexChanged);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.tsRepeat, 1, 3);
            this.tableLayoutPanel2.Controls.Add(this.lblRepeat, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.lblDuration, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.lblDurationType, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.lblStartTime, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.tsDuration, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.dtDate, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.flowLayoutPanel1, 1, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 4;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(360, 103);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // tsRepeat
            // 
            this.tsRepeat.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tsRepeat.Location = new System.Drawing.Point(93, 84);
            this.tsRepeat.MinimumSize = new System.Drawing.Size(184, 32);
            this.tsRepeat.Name = "tsRepeat";
            this.tsRepeat.Number = 0;
            this.tsRepeat.Size = new System.Drawing.Size(264, 32);
            this.tsRepeat.TabIndex = 1;
            this.tsRepeat.Unit = SC.Interfaces.DefaultPlugins.TimeUnit.Minute;
            this.tsRepeat.ValueChanged += new System.EventHandler(this.tsRepeat_ValueChanged);
            // 
            // lblRepeat
            // 
            this.lblRepeat.AutoSize = true;
            this.lblRepeat.Location = new System.Drawing.Point(3, 81);
            this.lblRepeat.Name = "lblRepeat";
            this.lblRepeat.Size = new System.Drawing.Size(45, 13);
            this.lblRepeat.TabIndex = 9;
            this.lblRepeat.Text = "Repeat:";
            // 
            // lblDuration
            // 
            this.lblDuration.AutoSize = true;
            this.lblDuration.Location = new System.Drawing.Point(3, 54);
            this.lblDuration.Name = "lblDuration";
            this.lblDuration.Size = new System.Drawing.Size(50, 13);
            this.lblDuration.TabIndex = 8;
            this.lblDuration.Text = "Duration:";
            // 
            // lblDurationType
            // 
            this.lblDurationType.AutoSize = true;
            this.lblDurationType.Location = new System.Drawing.Point(3, 27);
            this.lblDurationType.Name = "lblDurationType";
            this.lblDurationType.Size = new System.Drawing.Size(77, 13);
            this.lblDurationType.TabIndex = 7;
            this.lblDurationType.Text = "Duration Type:";
            // 
            // lblStartTime
            // 
            this.lblStartTime.AutoSize = true;
            this.lblStartTime.Location = new System.Drawing.Point(3, 0);
            this.lblStartTime.Name = "lblStartTime";
            this.lblStartTime.Size = new System.Drawing.Size(58, 13);
            this.lblStartTime.TabIndex = 6;
            this.lblStartTime.Text = "Start Time:";
            // 
            // tsDuration
            // 
            this.tsDuration.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tsDuration.Location = new System.Drawing.Point(93, 57);
            this.tsDuration.MinimumSize = new System.Drawing.Size(184, 32);
            this.tsDuration.Name = "tsDuration";
            this.tsDuration.Number = 0;
            this.tsDuration.Size = new System.Drawing.Size(264, 32);
            this.tsDuration.TabIndex = 0;
            this.tsDuration.Unit = SC.Interfaces.DefaultPlugins.TimeUnit.Minute;
            // 
            // dtDate
            // 
            this.dtDate.CustomFormat = "yyyy/MM/dd HH:mm";
            this.dtDate.Dock = System.Windows.Forms.DockStyle.Left;
            this.dtDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtDate.Location = new System.Drawing.Point(93, 3);
            this.dtDate.Name = "dtDate";
            this.dtDate.Size = new System.Drawing.Size(174, 20);
            this.dtDate.TabIndex = 13;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.radTrueDuration);
            this.flowLayoutPanel1.Controls.Add(this.radStopEarly);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(93, 30);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(264, 21);
            this.flowLayoutPanel1.TabIndex = 5;
            // 
            // radTrueDuration
            // 
            this.radTrueDuration.AutoSize = true;
            this.radTrueDuration.Checked = true;
            this.radTrueDuration.Location = new System.Drawing.Point(3, 3);
            this.radTrueDuration.Name = "radTrueDuration";
            this.radTrueDuration.Size = new System.Drawing.Size(65, 17);
            this.radTrueDuration.TabIndex = 3;
            this.radTrueDuration.TabStop = true;
            this.radTrueDuration.Text = "Duration";
            this.radTrueDuration.UseVisualStyleBackColor = true;
            // 
            // radStopEarly
            // 
            this.radStopEarly.AutoSize = true;
            this.radStopEarly.Location = new System.Drawing.Point(74, 3);
            this.radStopEarly.Name = "radStopEarly";
            this.radStopEarly.Size = new System.Drawing.Size(72, 17);
            this.radStopEarly.TabIndex = 4;
            this.radStopEarly.Text = "Stop early";
            this.radStopEarly.UseVisualStyleBackColor = true;
            // 
            // lblDefault
            // 
            this.lblDefault.AutoSize = true;
            this.lblDefault.Location = new System.Drawing.Point(3, 0);
            this.lblDefault.Name = "lblDefault";
            this.lblDefault.Size = new System.Drawing.Size(74, 13);
            this.lblDefault.TabIndex = 12;
            this.lblDefault.Text = "Default Mode:";
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Controls.Add(this.radEnabled);
            this.flowLayoutPanel2.Controls.Add(this.radDisabled);
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(113, 3);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(366, 25);
            this.flowLayoutPanel2.TabIndex = 11;
            // 
            // radEnabled
            // 
            this.radEnabled.AutoSize = true;
            this.radEnabled.Checked = true;
            this.radEnabled.Location = new System.Drawing.Point(3, 3);
            this.radEnabled.Name = "radEnabled";
            this.radEnabled.Size = new System.Drawing.Size(64, 17);
            this.radEnabled.TabIndex = 0;
            this.radEnabled.TabStop = true;
            this.radEnabled.Text = "Enabled";
            this.radEnabled.UseVisualStyleBackColor = true;
            // 
            // radDisabled
            // 
            this.radDisabled.AutoSize = true;
            this.radDisabled.Location = new System.Drawing.Point(73, 3);
            this.radDisabled.Name = "radDisabled";
            this.radDisabled.Size = new System.Drawing.Size(66, 17);
            this.radDisabled.TabIndex = 1;
            this.radDisabled.Text = "Disabled";
            this.radDisabled.UseVisualStyleBackColor = true;
            // 
            // grpActivation
            // 
            this.grpActivation.Controls.Add(this.tableLayoutPanel2);
            this.grpActivation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpActivation.Location = new System.Drawing.Point(113, 34);
            this.grpActivation.Name = "grpActivation";
            this.grpActivation.Size = new System.Drawing.Size(366, 122);
            this.grpActivation.TabIndex = 1;
            this.grpActivation.TabStop = false;
            this.grpActivation.Text = "Activation Settings:";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 110F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.tableLayoutPanel1.Controls.Add(this.lstActivations, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.grpActivation, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnRemove, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblDefault, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnAdd, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnApply, 2, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 128F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(557, 268);
            this.tableLayoutPanel1.TabIndex = 11;
            // 
            // btnRemove
            // 
            this.btnRemove.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnRemove.Location = new System.Drawing.Point(485, 162);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(69, 25);
            this.btnRemove.TabIndex = 12;
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnAdd.Location = new System.Drawing.Point(485, 34);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(69, 25);
            this.btnAdd.TabIndex = 11;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "Add Activation:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 159);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(99, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Current Activations:";
            // 
            // btnApply
            // 
            this.btnApply.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnApply.Location = new System.Drawing.Point(485, 3);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(69, 25);
            this.btnApply.TabIndex = 15;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // timeSpanSelector1
            // 
            this.timeSpanSelector1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.timeSpanSelector1.Location = new System.Drawing.Point(93, 78);
            this.timeSpanSelector1.MinimumSize = new System.Drawing.Size(184, 32);
            this.timeSpanSelector1.Name = "timeSpanSelector1";
            this.timeSpanSelector1.Number = 0;
            this.timeSpanSelector1.Size = new System.Drawing.Size(247, 32);
            this.timeSpanSelector1.TabIndex = 0;
            this.timeSpanSelector1.Unit = SC.Interfaces.DefaultPlugins.TimeUnit.Minute;
            // 
            // timeSpanSelector2
            // 
            this.timeSpanSelector2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.timeSpanSelector2.Location = new System.Drawing.Point(93, 104);
            this.timeSpanSelector2.MinimumSize = new System.Drawing.Size(184, 32);
            this.timeSpanSelector2.Name = "timeSpanSelector2";
            this.timeSpanSelector2.Number = 0;
            this.timeSpanSelector2.Size = new System.Drawing.Size(247, 32);
            this.timeSpanSelector2.TabIndex = 1;
            this.timeSpanSelector2.Unit = SC.Interfaces.DefaultPlugins.TimeUnit.Minute;
            // 
            // TimerPluginPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.MinimumSize = new System.Drawing.Size(566, 276);
            this.Name = "TimerPluginPanel";
            this.Size = new System.Drawing.Size(566, 276);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            this.grpActivation.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lstActivations;
        private SC.GUI.Utility.TimeSpanSelector timeSpanSelector1;
        private SC.GUI.Utility.TimeSpanSelector timeSpanSelector2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private SC.GUI.Utility.TimeSpanSelector tsRepeat;
        private System.Windows.Forms.Label lblRepeat;
        private System.Windows.Forms.Label lblDuration;
        private System.Windows.Forms.Label lblDurationType;
        private System.Windows.Forms.Label lblStartTime;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.RadioButton radEnabled;
        private System.Windows.Forms.RadioButton radDisabled;
        private SC.GUI.Utility.TimeSpanSelector tsDuration;
        private System.Windows.Forms.DateTimePicker dtDate;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.RadioButton radTrueDuration;
        private System.Windows.Forms.RadioButton radStopEarly;
        private System.Windows.Forms.Label lblDefault;
        private System.Windows.Forms.GroupBox grpActivation;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnApply;
    }
}
