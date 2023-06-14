namespace NPexwClient
{
    partial class Config
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
            this.radioButtonCross = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.radioButtonNought = new System.Windows.Forms.RadioButton();
            this.buttonConnect = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // radioButtonCross
            // 
            this.radioButtonCross.AutoSize = true;
            this.radioButtonCross.Location = new System.Drawing.Point(12, 27);
            this.radioButtonCross.Name = "radioButtonCross";
            this.radioButtonCross.Size = new System.Drawing.Size(54, 19);
            this.radioButtonCross.TabIndex = 0;
            this.radioButtonCross.TabStop = true;
            this.radioButtonCross.Text = "Cross";
            this.radioButtonCross.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "Your sign:";
            // 
            // radioButtonNought
            // 
            this.radioButtonNought.AutoSize = true;
            this.radioButtonNought.Location = new System.Drawing.Point(72, 27);
            this.radioButtonNought.Name = "radioButtonNought";
            this.radioButtonNought.Size = new System.Drawing.Size(66, 19);
            this.radioButtonNought.TabIndex = 2;
            this.radioButtonNought.TabStop = true;
            this.radioButtonNought.Text = "Nought";
            this.radioButtonNought.UseVisualStyleBackColor = true;
            // 
            // buttonConnect
            // 
            this.buttonConnect.Location = new System.Drawing.Point(12, 52);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(126, 23);
            this.buttonConnect.TabIndex = 3;
            this.buttonConnect.Text = "Connect";
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // Config
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(150, 89);
            this.Controls.Add(this.buttonConnect);
            this.Controls.Add(this.radioButtonNought);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.radioButtonCross);
            this.Name = "Config";
            this.Text = "Config";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private RadioButton radioButtonCross;
        private Label label1;
        private RadioButton radioButtonNought;
        private Button buttonConnect;
    }
}