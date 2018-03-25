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
            this.download1 = new System.Windows.Forms.Button();
            this.download2 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // input1
            // 
            this.input1.Location = new System.Drawing.Point(43, 67);
            this.input1.Name = "input1";
            this.input1.Size = new System.Drawing.Size(456, 22);
            this.input1.TabIndex = 0;
            // 
            // cancel1
            // 
            this.cancel1.Location = new System.Drawing.Point(526, 89);
            this.cancel1.Name = "cancel1";
            this.cancel1.Size = new System.Drawing.Size(117, 31);
            this.cancel1.TabIndex = 1;
            this.cancel1.Text = "Cancel";
            this.cancel1.UseVisualStyleBackColor = true;
            this.cancel1.Click += new System.EventHandler(this.cancel1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(43, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 17);
            this.label1.TabIndex = 2;
            // 
            // input2
            // 
            this.input2.Location = new System.Drawing.Point(40, 168);
            this.input2.Name = "input2";
            this.input2.Size = new System.Drawing.Size(456, 22);
            this.input2.TabIndex = 3;
            // 
            // cancel2
            // 
            this.cancel2.Location = new System.Drawing.Point(523, 184);
            this.cancel2.Name = "cancel2";
            this.cancel2.Size = new System.Drawing.Size(117, 28);
            this.cancel2.TabIndex = 4;
            this.cancel2.Text = "Cancel";
            this.cancel2.UseVisualStyleBackColor = true;
            this.cancel2.Click += new System.EventHandler(this.cancel2_Click);
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
            this.download2.Location = new System.Drawing.Point(523, 142);
            this.download2.Name = "download2";
            this.download2.Size = new System.Drawing.Size(117, 36);
            this.download2.TabIndex = 11;
            this.download2.Text = "Download";
            this.download2.UseVisualStyleBackColor = true;
            this.download2.Click += new System.EventHandler(this.download2_Click);
            // 
            // label2
            // 
            this.label2.AllowDrop = true;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(37, 232);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 17);
            this.label2.TabIndex = 12;
            this.label2.Text = "Text";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(826, 604);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.download2);
            this.Controls.Add(this.download1);
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
        private System.Windows.Forms.Button download1;
        private System.Windows.Forms.Button download2;
        private System.Windows.Forms.Label label2;
    }
}

