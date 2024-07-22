using System.Security.Cryptography;
using TudiBarcode.modul;

namespace TudiBarcode
{
    public partial class Form1 : Form
    {
        public static Form1 Instance;
        public Form1()
        {
            InitializeComponent();
            Instance = this;

            UpdateBaudRateValue();
            UpdatePortNameValue();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            string imgPath = Path.Combine(Application.StartupPath, "assets", "barcode.png");
            pictureBox1.Image = Image.FromFile(imgPath);
        }
        private void UpdateBaudRateValue()
        {
            comboBoxBaudRate.Items.Add("9600");
            comboBoxBaudRate.Items.Add("19200");
            comboBoxBaudRate.Items.Add("38400");
            comboBoxBaudRate.Items.Add("57600");
            comboBoxBaudRate.Items.Add("115200");
        }
        private void UpdatePortNameValue()
        {
            string[] ports = BarcodeApi.getListPort();
            foreach (string port in ports)
            {
                comboBoxComPort.Items.Add(port);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void richTextBoxUiConsole_TextChanged(object sender, EventArgs e)
        {
        }
        public void AppendTextToConsole(string text)
        {
            richTextBoxUiConsole.AppendText(text + Environment.NewLine);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBoxBaudRate.Text == "" || comboBoxComPort.Text == "")
                {
                    MessageBox.Show("Please select COM Port and Baud Rate");
                    return;
                }
                bool con = BarcodeApi.openPort(comboBoxComPort.Text, int.Parse(comboBoxBaudRate.Text));

                if (con)
                {
                    MessageBox.Show("[Success] Serial port is connected");
                }
                else
                {
                    MessageBox.Show("[Error] Serial port is not connected,\nPlease, check Baudrate & COM");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing serial port: {ex.Message}");
            }
        }
    }
}
