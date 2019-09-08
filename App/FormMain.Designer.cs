namespace App
{
    partial class FormMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.MainMenu = new System.Windows.Forms.MenuStrip();
            this.mIDIToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MainMenu_MIDI_Input = new System.Windows.Forms.ToolStripMenuItem();
            this.MainMenu_MIDI_Output = new System.Windows.Forms.ToolStripMenuItem();
            this.programToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MainMenu_Program_Mode = new System.Windows.Forms.ToolStripMenuItem();
            this.debugToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MainMenu_Debug_SendOutput = new System.Windows.Forms.ToolStripMenuItem();
            this.MainMenu_Debug_Break = new System.Windows.Forms.ToolStripMenuItem();
            this.StatusBar = new System.Windows.Forms.StatusStrip();
            this.StatusBar_Mode = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusBar_Input = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusBar_Output = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusBar_FileName = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusBar_Measure = new System.Windows.Forms.ToolStripStatusLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.TextBox_Measure = new System.Windows.Forms.TextBox();
            this.Button_ChangeMeasure = new System.Windows.Forms.Button();
            this.MainMenu.SuspendLayout();
            this.StatusBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainMenu
            // 
            this.MainMenu.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MainMenu.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mIDIToolStripMenuItem,
            this.programToolStripMenuItem,
            this.debugToolStripMenuItem});
            this.MainMenu.Location = new System.Drawing.Point(0, 0);
            this.MainMenu.Name = "MainMenu";
            this.MainMenu.Padding = new System.Windows.Forms.Padding(10, 3, 0, 3);
            this.MainMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.MainMenu.Size = new System.Drawing.Size(1248, 42);
            this.MainMenu.TabIndex = 0;
            this.MainMenu.Text = "menuStrip1";
            // 
            // mIDIToolStripMenuItem
            // 
            this.mIDIToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MainMenu_MIDI_Input,
            this.MainMenu_MIDI_Output});
            this.mIDIToolStripMenuItem.Name = "mIDIToolStripMenuItem";
            this.mIDIToolStripMenuItem.Size = new System.Drawing.Size(78, 36);
            this.mIDIToolStripMenuItem.Text = "MIDI";
            // 
            // MainMenu_MIDI_Input
            // 
            this.MainMenu_MIDI_Input.Name = "MainMenu_MIDI_Input";
            this.MainMenu_MIDI_Input.Size = new System.Drawing.Size(172, 36);
            this.MainMenu_MIDI_Input.Text = "Input";
            // 
            // MainMenu_MIDI_Output
            // 
            this.MainMenu_MIDI_Output.Name = "MainMenu_MIDI_Output";
            this.MainMenu_MIDI_Output.Size = new System.Drawing.Size(172, 36);
            this.MainMenu_MIDI_Output.Text = "Output";
            // 
            // programToolStripMenuItem
            // 
            this.programToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MainMenu_Program_Mode});
            this.programToolStripMenuItem.Name = "programToolStripMenuItem";
            this.programToolStripMenuItem.Size = new System.Drawing.Size(117, 36);
            this.programToolStripMenuItem.Text = "Program";
            // 
            // MainMenu_Program_Mode
            // 
            this.MainMenu_Program_Mode.Name = "MainMenu_Program_Mode";
            this.MainMenu_Program_Mode.Size = new System.Drawing.Size(159, 36);
            this.MainMenu_Program_Mode.Text = "Mode";
            // 
            // debugToolStripMenuItem
            // 
            this.debugToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MainMenu_Debug_SendOutput,
            this.MainMenu_Debug_Break});
            this.debugToolStripMenuItem.Name = "debugToolStripMenuItem";
            this.debugToolStripMenuItem.Size = new System.Drawing.Size(99, 36);
            this.debugToolStripMenuItem.Text = "Debug";
            // 
            // MainMenu_Debug_SendOutput
            // 
            this.MainMenu_Debug_SendOutput.Name = "MainMenu_Debug_SendOutput";
            this.MainMenu_Debug_SendOutput.Size = new System.Drawing.Size(233, 36);
            this.MainMenu_Debug_SendOutput.Text = "Send Output";
            this.MainMenu_Debug_SendOutput.Click += new System.EventHandler(this.MainMenu_Debug_SendOutput_Click);
            // 
            // MainMenu_Debug_Break
            // 
            this.MainMenu_Debug_Break.Name = "MainMenu_Debug_Break";
            this.MainMenu_Debug_Break.Size = new System.Drawing.Size(233, 36);
            this.MainMenu_Debug_Break.Text = "Break";
            this.MainMenu_Debug_Break.Click += new System.EventHandler(this.MainMenu_Debug_Break_Click);
            // 
            // StatusBar
            // 
            this.StatusBar.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StatusBar.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.StatusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusBar_Mode,
            this.toolStripStatusLabel3,
            this.StatusBar_Input,
            this.toolStripStatusLabel1,
            this.StatusBar_Output,
            this.toolStripStatusLabel2,
            this.StatusBar_FileName,
            this.StatusBar_Measure});
            this.StatusBar.Location = new System.Drawing.Point(0, 782);
            this.StatusBar.Name = "StatusBar";
            this.StatusBar.Padding = new System.Windows.Forms.Padding(2, 0, 23, 0);
            this.StatusBar.Size = new System.Drawing.Size(1248, 37);
            this.StatusBar.TabIndex = 1;
            this.StatusBar.Text = "statusStrip1";
            // 
            // StatusBar_Mode
            // 
            this.StatusBar_Mode.Name = "StatusBar_Mode";
            this.StatusBar_Mode.Size = new System.Drawing.Size(131, 32);
            this.StatusBar_Mode.Text = "(No Mode)";
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(35, 32);
            this.toolStripStatusLabel3.Text = " | ";
            // 
            // StatusBar_Input
            // 
            this.StatusBar_Input.Name = "StatusBar_Input";
            this.StatusBar_Input.Size = new System.Drawing.Size(124, 32);
            this.StatusBar_Input.Text = "(No Input)";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(36, 32);
            this.toolStripStatusLabel1.Text = "↔";
            // 
            // StatusBar_Output
            // 
            this.StatusBar_Output.Name = "StatusBar_Output";
            this.StatusBar_Output.Size = new System.Drawing.Size(144, 32);
            this.StatusBar_Output.Text = "(No Output)";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(35, 32);
            this.toolStripStatusLabel2.Text = " | ";
            // 
            // StatusBar_FileName
            // 
            this.StatusBar_FileName.Name = "StatusBar_FileName";
            this.StatusBar_FileName.Size = new System.Drawing.Size(0, 32);
            // 
            // StatusBar_Measure
            // 
            this.StatusBar_Measure.Name = "StatusBar_Measure";
            this.StatusBar_Measure.Size = new System.Drawing.Size(0, 32);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 99);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(168, 37);
            this.label1.TabIndex = 2;
            this.label1.Text = "Set Measure:";
            // 
            // TextBox_Measure
            // 
            this.TextBox_Measure.Location = new System.Drawing.Point(221, 99);
            this.TextBox_Measure.Name = "TextBox_Measure";
            this.TextBox_Measure.Size = new System.Drawing.Size(141, 43);
            this.TextBox_Measure.TabIndex = 3;
            this.TextBox_Measure.TextChanged += new System.EventHandler(this.TextBox_Measure_TextChanged);
            // 
            // Button_ChangeMeasure
            // 
            this.Button_ChangeMeasure.Location = new System.Drawing.Point(386, 90);
            this.Button_ChangeMeasure.Name = "Button_ChangeMeasure";
            this.Button_ChangeMeasure.Size = new System.Drawing.Size(90, 68);
            this.Button_ChangeMeasure.TabIndex = 4;
            this.Button_ChangeMeasure.Text = "Go";
            this.Button_ChangeMeasure.UseVisualStyleBackColor = true;
            this.Button_ChangeMeasure.Click += new System.EventHandler(this.Button_ChangeMeasure_Click);
            // 
            // FormMain
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 37F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1248, 819);
            this.Controls.Add(this.Button_ChangeMeasure);
            this.Controls.Add(this.TextBox_Measure);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.StatusBar);
            this.Controls.Add(this.MainMenu);
            this.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.MainMenu;
            this.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.MaximizeBox = false;
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SightReader";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.FormMain_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.FormMain_DragEnter);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormMain_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.FormMain_KeyUp);
            this.MainMenu.ResumeLayout(false);
            this.MainMenu.PerformLayout();
            this.StatusBar.ResumeLayout(false);
            this.StatusBar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip MainMenu;
        private System.Windows.Forms.StatusStrip StatusBar;
        private System.Windows.Forms.ToolStripMenuItem mIDIToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem MainMenu_MIDI_Input;
        private System.Windows.Forms.ToolStripMenuItem MainMenu_MIDI_Output;
        private System.Windows.Forms.ToolStripMenuItem programToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem MainMenu_Program_Mode;
        private System.Windows.Forms.ToolStripStatusLabel StatusBar_Mode;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripStatusLabel StatusBar_Input;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel StatusBar_Output;
        private System.Windows.Forms.ToolStripMenuItem debugToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem MainMenu_Debug_SendOutput;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel StatusBar_FileName;
        private System.Windows.Forms.ToolStripStatusLabel StatusBar_Measure;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TextBox_Measure;
        private System.Windows.Forms.ToolStripMenuItem MainMenu_Debug_Break;
        private System.Windows.Forms.Button Button_ChangeMeasure;
    }
}

