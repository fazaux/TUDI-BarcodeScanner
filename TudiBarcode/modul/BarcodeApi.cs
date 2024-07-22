using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Text.RegularExpressions;

namespace TudiBarcode.modul
{
    internal class BarcodeApi
    {
        private static SerialPort serialPort;
        private static CancellationTokenSource cancellationToken = new CancellationTokenSource();
        public static bool IsBarcodeConected = false;
        public static string[] getListPort()
        {
            return SerialPort.GetPortNames();
        }

        public static bool openPort(string portName, int baud)
        {
            serialPort = new SerialPort(portName, baud, Parity.None, 8, StopBits.One);
            serialPort.Open();
            Task.Run(() => ListenSerial());
            if (serialPort.IsOpen)
            {
                Console.WriteLine("[System] Barcode is connected!");
                IsBarcodeConected = true;
            }

            return serialPort.IsOpen;
        }

        public static bool closePort()
        {
            try
            {
                cancellationToken.Cancel();
                serialPort.Close();
                return !serialPort.IsOpen;
            }catch(Exception ex)
            {
                return false;
            }
        }
        public static async Task ListenSerial()
        {
            StringBuilder dataBuffer = new StringBuilder();

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    byte[] buffer = new byte[512];  // Adjust buffer size as needed
                    int bytesRead = await serialPort.BaseStream.ReadAsync(buffer, 0, buffer.Length, cancellationToken.Token);

                    if (bytesRead > 0)
                    {
                        // Convert received bytes directly to ASCII string
                        string receivedAsciiData = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                        dataBuffer.Append(receivedAsciiData);

                        // Process complete lines
                        while (true)
                        {
                            string data = dataBuffer.ToString();
                            int newlineIndex = data.IndexOf('\n');

                            if (newlineIndex == -1)
                            {
                                break;  // No complete line found, exit loop
                            }

                            string line = data.Substring(0, newlineIndex).Trim();
                            dataBuffer.Remove(0, newlineIndex + 1);

                            string cleanRx = Regex.Replace(line, @"[^A-Za-z0-9]", "");

                            await Utils.SendResponse("response-barcode-event", 1, "success", cleanRx);
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error reading from serial port: {ex.Message}");
                }
            }
        }

        public static void SendCommands(string command)
        {
            try
            {
                if (serialPort != null && serialPort.IsOpen)
                {
                    byte[] commandBytes = Utils.StringToByteArray(command.Replace(" ", ""));
                    serialPort.Write(commandBytes, 0, commandBytes.Length);
                    Utils.SendResponse("response-barcode-config", 1, "success");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending command to serial port: {ex.Message}");
            }
        }

        public class InterfaceCommnand
        {
            public string key { get; set; }
            public string description { get; set; }
            public string code { get; set; }
        }

        public static List<InterfaceCommnand> BarcodeJsonCommand()
        {
            try
            {
                // Read the JSON file
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Modul", "barcode", "CommandCode.json");
                string jsonData = File.ReadAllText(filePath);
                // Deserialize JSON data to a list of InterfaceCommand objects
                List<InterfaceCommnand> commands = JsonConvert.DeserializeObject<List<InterfaceCommnand>>(jsonData);

                return commands;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading JSON data from file: {ex.Message}");
                return new List<InterfaceCommnand>(); // Return an empty list in case of an error
            }
        }
    }
}
