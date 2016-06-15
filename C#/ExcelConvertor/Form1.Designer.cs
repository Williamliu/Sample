namespace ExcelConvertor
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
            this.btn_file = new System.Windows.Forms.Button();
            this.txt_file = new System.Windows.Forms.TextBox();
            this.openFileDiag = new System.Windows.Forms.OpenFileDialog();
            this.txt_input = new System.Windows.Forms.TextBox();
            this.lbl_input = new System.Windows.Forms.Label();
            this.btn_load = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label4 = new System.Windows.Forms.Label();
            this.ckl_valinput = new System.Windows.Forms.CheckedListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ckl_input = new System.Windows.Forms.CheckedListBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.dgv_input = new System.Windows.Forms.DataGridView();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.dgv_output = new System.Windows.Forms.DataGridView();
            this.btn_process = new System.Windows.Forms.Button();
            this.btn_output = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_input)).BeginInit();
            this.tabPage5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_output)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_file
            // 
            this.btn_file.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_file.Location = new System.Drawing.Point(722, 8);
            this.btn_file.Name = "btn_file";
            this.btn_file.Size = new System.Drawing.Size(40, 22);
            this.btn_file.TabIndex = 3;
            this.btn_file.Text = "...";
            this.btn_file.UseVisualStyleBackColor = true;
            this.btn_file.Click += new System.EventHandler(this.btn_file_Click);
            // 
            // txt_file
            // 
            this.txt_file.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_file.Location = new System.Drawing.Point(12, 9);
            this.txt_file.Name = "txt_file";
            this.txt_file.Size = new System.Drawing.Size(704, 20);
            this.txt_file.TabIndex = 2;
            // 
            // txt_input
            // 
            this.txt_input.Location = new System.Drawing.Point(120, 41);
            this.txt_input.Name = "txt_input";
            this.txt_input.Size = new System.Drawing.Size(125, 20);
            this.txt_input.TabIndex = 5;
            this.txt_input.Text = "Sheet1";
            // 
            // lbl_input
            // 
            this.lbl_input.AutoSize = true;
            this.lbl_input.Location = new System.Drawing.Point(20, 45);
            this.lbl_input.Name = "lbl_input";
            this.lbl_input.Size = new System.Drawing.Size(97, 13);
            this.lbl_input.TabIndex = 7;
            this.lbl_input.Text = "Input WorkSheet : ";
            // 
            // btn_load
            // 
            this.btn_load.Location = new System.Drawing.Point(275, 38);
            this.btn_load.Name = "btn_load";
            this.btn_load.Size = new System.Drawing.Size(75, 45);
            this.btn_load.TabIndex = 9;
            this.btn_load.Text = "LOAD";
            this.btn_load.UseVisualStyleBackColor = true;
            this.btn_load.Click += new System.EventHandler(this.btn_load_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Location = new System.Drawing.Point(3, 94);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(775, 503);
            this.tabControl1.TabIndex = 14;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.ckl_valinput);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.ckl_input);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(767, 477);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Mapping Settings";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(230, 10);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 13);
            this.label4.TabIndex = 19;
            this.label4.Text = "Value Columns";
            // 
            // ckl_valinput
            // 
            this.ckl_valinput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.ckl_valinput.CheckOnClick = true;
            this.ckl_valinput.FormattingEnabled = true;
            this.ckl_valinput.Location = new System.Drawing.Point(228, 28);
            this.ckl_valinput.Name = "ckl_valinput";
            this.ckl_valinput.Size = new System.Drawing.Size(195, 439);
            this.ckl_valinput.TabIndex = 18;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 13);
            this.label2.TabIndex = 16;
            this.label2.Text = "Row Columns";
            // 
            // ckl_input
            // 
            this.ckl_input.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.ckl_input.CheckOnClick = true;
            this.ckl_input.FormattingEnabled = true;
            this.ckl_input.Location = new System.Drawing.Point(6, 28);
            this.ckl_input.Name = "ckl_input";
            this.ckl_input.Size = new System.Drawing.Size(184, 439);
            this.ckl_input.TabIndex = 14;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.dgv_input);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(767, 477);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Source Data";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // dgv_input
            // 
            this.dgv_input.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgv_input.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_input.Location = new System.Drawing.Point(6, 6);
            this.dgv_input.Name = "dgv_input";
            this.dgv_input.Size = new System.Drawing.Size(758, 468);
            this.dgv_input.TabIndex = 12;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.dgv_output);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Size = new System.Drawing.Size(767, 477);
            this.tabPage5.TabIndex = 2;
            this.tabPage5.Text = "Output Data";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // dgv_output
            // 
            this.dgv_output.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgv_output.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_output.Location = new System.Drawing.Point(4, 4);
            this.dgv_output.Name = "dgv_output";
            this.dgv_output.Size = new System.Drawing.Size(758, 468);
            this.dgv_output.TabIndex = 13;
            // 
            // btn_process
            // 
            this.btn_process.Location = new System.Drawing.Point(370, 38);
            this.btn_process.Name = "btn_process";
            this.btn_process.Size = new System.Drawing.Size(75, 45);
            this.btn_process.TabIndex = 15;
            this.btn_process.Text = "CONVERT";
            this.btn_process.UseVisualStyleBackColor = true;
            this.btn_process.Click += new System.EventHandler(this.btn_process_Click);
            // 
            // btn_output
            // 
            this.btn_output.Location = new System.Drawing.Point(467, 38);
            this.btn_output.Name = "btn_output";
            this.btn_output.Size = new System.Drawing.Size(75, 45);
            this.btn_output.TabIndex = 16;
            this.btn_output.Text = "OUTPUT";
            this.btn_output.UseVisualStyleBackColor = true;
            this.btn_output.Click += new System.EventHandler(this.btn_output_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(779, 598);
            this.Controls.Add(this.btn_output);
            this.Controls.Add(this.btn_process);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.btn_load);
            this.Controls.Add(this.lbl_input);
            this.Controls.Add(this.txt_input);
            this.Controls.Add(this.btn_file);
            this.Controls.Add(this.txt_file);
            this.Name = "Form1";
            this.Text = "Excel Convertor";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_input)).EndInit();
            this.tabPage5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_output)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_file;
        private System.Windows.Forms.TextBox txt_file;
        private System.Windows.Forms.OpenFileDialog openFileDiag;
        private System.Windows.Forms.TextBox txt_input;
        private System.Windows.Forms.Label lbl_input;
        private System.Windows.Forms.Button btn_load;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.CheckedListBox ckl_input;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckedListBox ckl_valinput;
        private System.Windows.Forms.Button btn_process;
        private System.Windows.Forms.DataGridView dgv_input;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.DataGridView dgv_output;
        private System.Windows.Forms.Button btn_output;
    }
}

