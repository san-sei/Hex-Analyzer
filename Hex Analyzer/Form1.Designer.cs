﻿namespace Hex_Analyzer
{
    partial class Form1
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
            this.FileLoader = new System.Windows.Forms.OpenFileDialog();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.text_compiled = new System.Windows.Forms.TextBox();
            this.label_compiled = new System.Windows.Forms.Label();
            this.txt_compiled_file = new System.Windows.Forms.TextBox();
            this.btn_compiled = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label_flash = new System.Windows.Forms.Label();
            this.text_flash = new System.Windows.Forms.TextBox();
            this.txt_flash_file = new System.Windows.Forms.TextBox();
            this.btn_load_flash = new System.Windows.Forms.Button();
            this.cmbprogram_section = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.cmbChipModel = new System.Windows.Forms.ComboBox();
            this.txtDiscrepancies = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.text_compiled);
            this.groupBox1.Controls.Add(this.label_compiled);
            this.groupBox1.Controls.Add(this.txt_compiled_file);
            this.groupBox1.Controls.Add(this.btn_compiled);
            this.groupBox1.Location = new System.Drawing.Point(307, 57);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(269, 227);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Compiled Hex";
            // 
            // text_compiled
            // 
            this.text_compiled.Location = new System.Drawing.Point(6, 59);
            this.text_compiled.Multiline = true;
            this.text_compiled.Name = "text_compiled";
            this.text_compiled.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.text_compiled.Size = new System.Drawing.Size(256, 132);
            this.text_compiled.TabIndex = 10;
            this.text_compiled.WordWrap = false;
            // 
            // label_compiled
            // 
            this.label_compiled.AutoSize = true;
            this.label_compiled.Location = new System.Drawing.Point(6, 194);
            this.label_compiled.Name = "label_compiled";
            this.label_compiled.Size = new System.Drawing.Size(43, 13);
            this.label_compiled.TabIndex = 9;
            this.label_compiled.Text = "------------";
            // 
            // txt_compiled_file
            // 
            this.txt_compiled_file.Location = new System.Drawing.Point(6, 33);
            this.txt_compiled_file.Name = "txt_compiled_file";
            this.txt_compiled_file.Size = new System.Drawing.Size(212, 20);
            this.txt_compiled_file.TabIndex = 5;
            // 
            // btn_compiled
            // 
            this.btn_compiled.Location = new System.Drawing.Point(224, 33);
            this.btn_compiled.Name = "btn_compiled";
            this.btn_compiled.Size = new System.Drawing.Size(38, 20);
            this.btn_compiled.TabIndex = 4;
            this.btn_compiled.Text = "...";
            this.btn_compiled.UseVisualStyleBackColor = true;
            this.btn_compiled.Click += new System.EventHandler(this.btn_compiled_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label_flash);
            this.groupBox2.Controls.Add(this.text_flash);
            this.groupBox2.Controls.Add(this.txt_flash_file);
            this.groupBox2.Controls.Add(this.btn_load_flash);
            this.groupBox2.Location = new System.Drawing.Point(12, 57);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(271, 227);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Read-back Hex";
            this.groupBox2.Enter += new System.EventHandler(this.groupBox2_Enter);
            // 
            // label_flash
            // 
            this.label_flash.AutoSize = true;
            this.label_flash.Location = new System.Drawing.Point(6, 194);
            this.label_flash.Name = "label_flash";
            this.label_flash.Size = new System.Drawing.Size(43, 13);
            this.label_flash.TabIndex = 10;
            this.label_flash.Text = "------------";
            // 
            // text_flash
            // 
            this.text_flash.Location = new System.Drawing.Point(6, 59);
            this.text_flash.Multiline = true;
            this.text_flash.Name = "text_flash";
            this.text_flash.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.text_flash.Size = new System.Drawing.Size(256, 132);
            this.text_flash.TabIndex = 7;
            this.text_flash.WordWrap = false;
            // 
            // txt_flash_file
            // 
            this.txt_flash_file.Location = new System.Drawing.Point(6, 33);
            this.txt_flash_file.Name = "txt_flash_file";
            this.txt_flash_file.Size = new System.Drawing.Size(212, 20);
            this.txt_flash_file.TabIndex = 3;
            // 
            // btn_load_flash
            // 
            this.btn_load_flash.Location = new System.Drawing.Point(224, 33);
            this.btn_load_flash.Name = "btn_load_flash";
            this.btn_load_flash.Size = new System.Drawing.Size(38, 20);
            this.btn_load_flash.TabIndex = 2;
            this.btn_load_flash.Text = "...";
            this.btn_load_flash.UseVisualStyleBackColor = true;
            this.btn_load_flash.Click += new System.EventHandler(this.btn_load_flash_Click);
            // 
            // cmbprogram_section
            // 
            this.cmbprogram_section.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbprogram_section.FormattingEnabled = true;
            this.cmbprogram_section.Items.AddRange(new object[] {
            "Code",
            "Configuration"});
            this.cmbprogram_section.Location = new System.Drawing.Point(143, 12);
            this.cmbprogram_section.Name = "cmbprogram_section";
            this.cmbprogram_section.Size = new System.Drawing.Size(119, 21);
            this.cmbprogram_section.TabIndex = 4;
            this.cmbprogram_section.SelectedIndexChanged += new System.EventHandler(this.program_section_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(125, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Program Section/Model :";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 303);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "Varify";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // cmbChipModel
            // 
            this.cmbChipModel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbChipModel.FormattingEnabled = true;
            this.cmbChipModel.Items.AddRange(new object[] {
            "Nrf51x FOB",
            "CC2540 Terminal"});
            this.cmbChipModel.Location = new System.Drawing.Point(280, 12);
            this.cmbChipModel.Name = "cmbChipModel";
            this.cmbChipModel.Size = new System.Drawing.Size(120, 21);
            this.cmbChipModel.TabIndex = 7;
            // 
            // txtDiscrepancies
            // 
            this.txtDiscrepancies.Location = new System.Drawing.Point(237, 298);
            this.txtDiscrepancies.Multiline = true;
            this.txtDiscrepancies.Name = "txtDiscrepancies";
            this.txtDiscrepancies.Size = new System.Drawing.Size(332, 32);
            this.txtDiscrepancies.TabIndex = 8;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(121, 308);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(110, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "All discrepancies are :";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 334);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtDiscrepancies);
            this.Controls.Add(this.cmbChipModel);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbprogram_section);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Hex Verificator";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.OpenFileDialog FileLoader;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txt_flash_file;
        private System.Windows.Forms.Button btn_load_flash;
        private System.Windows.Forms.TextBox txt_compiled_file;
        private System.Windows.Forms.Button btn_compiled;
        private System.Windows.Forms.TextBox text_flash;
        private System.Windows.Forms.ComboBox cmbprogram_section;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label_compiled;
        private System.Windows.Forms.Label label_flash;
        private System.Windows.Forms.TextBox text_compiled;
        private System.Windows.Forms.ComboBox cmbChipModel;
        private System.Windows.Forms.TextBox txtDiscrepancies;
        private System.Windows.Forms.Label label2;
    }
}

