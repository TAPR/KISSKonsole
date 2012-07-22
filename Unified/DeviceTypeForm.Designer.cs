namespace KISS_Konsole
{
    partial class DeviceTypeForm
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
            this.radioButtonUSB = new System.Windows.Forms.RadioButton();
            this.radioButtonEthernet = new System.Windows.Forms.RadioButton();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // radioButtonUSB
            // 
            this.radioButtonUSB.AutoSize = true;
            this.radioButtonUSB.Location = new System.Drawing.Point(41, 42);
            this.radioButtonUSB.Name = "radioButtonUSB";
            this.radioButtonUSB.Size = new System.Drawing.Size(119, 17);
            this.radioButtonUSB.TabIndex = 0;
            this.radioButtonUSB.TabStop = true;
            this.radioButtonUSB.Text = "USB (Ozy/Magister)";
            this.radioButtonUSB.UseVisualStyleBackColor = true;
            // 
            // radioButtonEthernet
            // 
            this.radioButtonEthernet.AutoSize = true;
            this.radioButtonEthernet.Location = new System.Drawing.Point(41, 65);
            this.radioButtonEthernet.Name = "radioButtonEthernet";
            this.radioButtonEthernet.Size = new System.Drawing.Size(172, 17);
            this.radioButtonEthernet.TabIndex = 1;
            this.radioButtonEthernet.TabStop = true;
            this.radioButtonEthernet.Text = "Ethernet (Metis/Hermes/Griffin)";
            this.radioButtonEthernet.UseVisualStyleBackColor = true;
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(12, 101);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(106, 30);
            this.buttonOK.TabIndex = 2;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(174, 101);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(106, 30);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // DeviceTypeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 158);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.radioButtonEthernet);
            this.Controls.Add(this.radioButtonUSB);
            this.Name = "DeviceTypeForm";
            this.Text = "Choose Device Connection Type";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton radioButtonUSB;
        private System.Windows.Forms.RadioButton radioButtonEthernet;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
    }
}