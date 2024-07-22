namespace TudiBarcode
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pictureBox1 = new PictureBox();
            label1 = new Label();
            label2 = new Label();
            comboBoxBaudRate = new ComboBox();
            groupBox1 = new GroupBox();
            button1 = new Button();
            label4 = new Label();
            comboBoxComPort = new ComboBox();
            label3 = new Label();
            richTextBoxUiConsole = new RichTextBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(12, 29);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(323, 143);
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            pictureBox1.Click += pictureBox1_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(92, 17);
            label1.TabIndex = 1;
            label1.Text = "TUDI Barcode";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 9F, FontStyle.Italic, GraphicsUnit.Point, 0);
            label2.Location = new Point(12, 175);
            label2.Name = "label2";
            label2.Size = new Size(272, 15);
            label2.TabIndex = 2;
            label2.Text = "* Please Scan this before connect via Serial Monitor";
            label2.Click += label2_Click;
            // 
            // comboBoxBaudRate
            // 
            comboBoxBaudRate.FormattingEnabled = true;
            comboBoxBaudRate.Location = new Point(23, 51);
            comboBoxBaudRate.Name = "comboBoxBaudRate";
            comboBoxBaudRate.Size = new Size(154, 23);
            comboBoxBaudRate.TabIndex = 3;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(button1);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(comboBoxComPort);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(comboBoxBaudRate);
            groupBox1.Location = new Point(359, 29);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(429, 143);
            groupBox1.TabIndex = 4;
            groupBox1.TabStop = false;
            groupBox1.Text = "Connection";
            // 
            // button1
            // 
            button1.Location = new Point(310, 51);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 7;
            button1.Text = "Connect";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(183, 33);
            label4.Name = "label4";
            label4.Size = new Size(35, 15);
            label4.TabIndex = 6;
            label4.Text = "COM";
            // 
            // comboBoxComPort
            // 
            comboBoxComPort.FormattingEnabled = true;
            comboBoxComPort.Location = new Point(183, 51);
            comboBoxComPort.Name = "comboBoxComPort";
            comboBoxComPort.Size = new Size(121, 23);
            comboBoxComPort.TabIndex = 5;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(23, 33);
            label3.Name = "label3";
            label3.Size = new Size(60, 15);
            label3.TabIndex = 4;
            label3.Text = "Baud Rate";
            // 
            // richTextBoxUiConsole
            // 
            richTextBoxUiConsole.Location = new Point(12, 201);
            richTextBoxUiConsole.Name = "richTextBoxUiConsole";
            richTextBoxUiConsole.Size = new Size(776, 237);
            richTextBoxUiConsole.TabIndex = 5;
            richTextBoxUiConsole.Text = "";
            richTextBoxUiConsole.TextChanged += richTextBoxUiConsole_TextChanged;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(richTextBoxUiConsole);
            Controls.Add(groupBox1);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(pictureBox1);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBox1;
        private Label label1;
        private Label label2;
        private ComboBox comboBoxBaudRate;
        private GroupBox groupBox1;
        private Button button1;
        private Label label4;
        private ComboBox comboBoxComPort;
        private Label label3;
        private RichTextBox richTextBoxUiConsole;
    }
}
