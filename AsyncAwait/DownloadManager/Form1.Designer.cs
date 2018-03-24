using System.Configuration;

namespace DownloadManager
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
            this.input1 = new System.Windows.Forms.TextBox();
            this.cancel1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.input2 = new System.Windows.Forms.TextBox();
            this.cancel2 = new System.Windows.Forms.Button();
            this.cancel3 = new System.Windows.Forms.Button();
            this.input3 = new System.Windows.Forms.TextBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.progressBar2 = new System.Windows.Forms.ProgressBar();
            this.progressBar3 = new System.Windows.Forms.ProgressBar();
            this.download1 = new System.Windows.Forms.Button();
            this.download2 = new System.Windows.Forms.Button();
            this.download3 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // input1
            // 
            this.input1.Location = new System.Drawing.Point(43, 60);
            this.input1.Name = "input1";
            this.input1.Size = new System.Drawing.Size(456, 22);
            this.input1.TabIndex = 0;
            // 
            // cancel1
            // 
            this.cancel1.Location = new System.Drawing.Point(526, 89);
            this.cancel1.Name = "cancel1";
            this.cancel1.Size = new System.Drawing.Size(117, 23);
            this.cancel1.TabIndex = 1;
            this.cancel1.Text = "Cancel";
            this.cancel1.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(40, 379);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = $"Download files you can fined at {ConfigurationManager.AppSettings["FilePath"]}";
            // 
            // input2
            // 
            this.input2.Location = new System.Drawing.Point(43, 151);
            this.input2.Name = "input2";
            this.input2.Size = new System.Drawing.Size(456, 22);
            this.input2.TabIndex = 3;
            // 
            // cancel2
            // 
            this.cancel2.Location = new System.Drawing.Point(526, 180);
            this.cancel2.Name = "cancel2";
            this.cancel2.Size = new System.Drawing.Size(117, 22);
            this.cancel2.TabIndex = 4;
            this.cancel2.Text = "Cancel";
            this.cancel2.UseVisualStyleBackColor = true;
            // 
            // cancel3
            // 
            this.cancel3.Location = new System.Drawing.Point(526, 283);
            this.cancel3.Name = "cancel3";
            this.cancel3.Size = new System.Drawing.Size(117, 22);
            this.cancel3.TabIndex = 6;
            this.cancel3.Text = "Cancel";
            this.cancel3.UseVisualStyleBackColor = true;
            // 
            // input3
            // 
            this.input3.Location = new System.Drawing.Point(43, 253);
            this.input3.Name = "input3";
            this.input3.Size = new System.Drawing.Size(456, 22);
            this.input3.TabIndex = 5;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(43, 89);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(456, 23);
            this.progressBar1.TabIndex = 7;
            // 
            // progressBar2
            // 
            this.progressBar2.Location = new System.Drawing.Point(43, 180);
            this.progressBar2.Name = "progressBar2";
            this.progressBar2.Size = new System.Drawing.Size(456, 23);
            this.progressBar2.TabIndex = 8;
            // 
            // progressBar3
            // 
            this.progressBar3.Location = new System.Drawing.Point(43, 282);
            this.progressBar3.Name = "progressBar3";
            this.progressBar3.Size = new System.Drawing.Size(456, 23);
            this.progressBar3.TabIndex = 9;
            // 
            // download1
            // 
            this.download1.Location = new System.Drawing.Point(526, 53);
            this.download1.Name = "download1";
            this.download1.Size = new System.Drawing.Size(117, 36);
            this.download1.TabIndex = 10;
            this.download1.Text = "Download";
            this.download1.UseVisualStyleBackColor = true;
            this.download1.Click += new System.EventHandler(this.download1_Click);
            // 
            // download2
            // 
            this.download2.Location = new System.Drawing.Point(526, 144);
            this.download2.Name = "download2";
            this.download2.Size = new System.Drawing.Size(117, 36);
            this.download2.TabIndex = 11;
            this.download2.Text = "Download";
            this.download2.UseVisualStyleBackColor = true;
            // 
            // download3
            // 
            this.download3.Location = new System.Drawing.Point(526, 246);
            this.download3.Name = "download3";
            this.download3.Size = new System.Drawing.Size(117, 36);
            this.download3.TabIndex = 12;
            this.download3.Text = "Download";
            this.download3.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(674, 434);
            this.Controls.Add(this.download3);
            this.Controls.Add(this.download2);
            this.Controls.Add(this.download1);
            this.Controls.Add(this.progressBar3);
            this.Controls.Add(this.progressBar2);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.cancel3);
            this.Controls.Add(this.input3);
            this.Controls.Add(this.cancel2);
            this.Controls.Add(this.input2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cancel1);
            this.Controls.Add(this.input1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox input1;
        private System.Windows.Forms.Button cancel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox input2;
        private System.Windows.Forms.Button cancel2;
        private System.Windows.Forms.Button cancel3;
        private System.Windows.Forms.TextBox input3;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.ProgressBar progressBar2;
        private System.Windows.Forms.ProgressBar progressBar3;
        private System.Windows.Forms.Button download1;
        private System.Windows.Forms.Button download2;
        private System.Windows.Forms.Button download3;
    }
}

