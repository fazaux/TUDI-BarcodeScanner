using TudiBarcode.modul;

namespace TudiBarcode
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            // Start websocket server
            Task.Run(() => UartWebsocket.StartServer());

            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
}