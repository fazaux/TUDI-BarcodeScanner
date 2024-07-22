using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.Logging;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace TudiBarcode.modul
{
    internal class UartWebsocket
    {
        private static bool keepscan = false;
        private static HttpListener _server;
        public static WebSocket _webSocket;
        private static bool _isOnProcess = false;


        public static async Task StartServer()
        {
            _server = new HttpListener();
            _server.Prefixes.Add("http://localhost:8383/");
            _server.Start();
            Console.WriteLine("Server started");
            while (true)
            {
                var context = await _server.GetContextAsync();
                if (context.Request.IsWebSocketRequest)
                {
                    _ = Task.Run(() => ProcessWebSocketRequest(context));
                }
                else
                {
                    context.Response.StatusCode = 400;
                    context.Response.Close();
                }
            }
        }

        private static async Task ProcessWebSocketRequest(HttpListenerContext context)
        {
            var listener = context.Response;
            WebSocketContext webSocketContext = await context.AcceptWebSocketAsync(null);

            _webSocket = webSocketContext.WebSocket;

            try
            {
                byte[] buffer = new byte[1024];
                WebSocketReceiveResult result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                //_ = Task.Run(() => CheckIfUserConnected());

                while (_webSocket.State == WebSocketState.Open)
                {
                    Console.WriteLine("[+][server-received]: " + Encoding.UTF8.GetString(buffer, 0, result.Count));

                    if (_webSocket.State == WebSocketState.Open)
                    {
                        if (result.MessageType == WebSocketMessageType.Text)
                        {
                            string req = Encoding.UTF8.GetString(buffer, 0, result.Count);
                            await ReplyClientHandle(req, buffer);
                            //var responseMessage = Encoding.UTF8.GetBytes("pong");
                            //await PublicVariable._ctxScokerServer.SendAsync(new ArraySegment<byte>(responseMessage), WebSocketMessageType.Text, true, CancellationToken.None);
                        }
                    }
                    else
                    {
                        Console.WriteLine("[Debug]:" + _webSocket.State);
                    }

                    result = await Task.Run(async () =>
                    {
                        return await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                    });
                }
                await _webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"WebSocket error: {ex}");
            }
            finally
            {
                if (_webSocket != null)
                    _webSocket.Dispose();
            }
        }

        private static async Task ReplyClientHandle(string req, byte[] buffer)
        {

            if (IsJson(req))
            {
                JObject json = JObject.Parse(req);
                string @event = (string)json["event"];
                string value = "";

                if (json["value"] != null && json["value"].Type == JTokenType.String)
                {
                    value = (string)json["value"];
                }

                if (@event == "ping")
                {
                    await Utils.SendResponse("pong", 1, "success");
                }// #######################################################################
                 // # [Tudi] Barcode configuration!
                 // #######################################################################

                else if (@event == "barcode-open-connection")
                {
                    dynamic jsonObject = JsonConvert.DeserializeObject(req);
                    string ComPort = jsonObject?.value?.com ?? throw new Exception("COM is null");
                    int BaudRate = jsonObject?.value?.baudRate ?? throw new Exception("baudRate Port is null");

                    bool res = BarcodeApi.openPort(ComPort, BaudRate);
                    if (res)
                    {
                        await Utils.SendResponse( "response-barcode-open-connection", 1, "success");
                    }
                    else
                    {
                        await Utils.SendResponse( "response-barcode-open-connection", 0, "error: open connection");
                    }

                }
                else if (@event == "barcode-get-list-com-port")
                {
                    string[] barcode = BarcodeApi.getListPort();
                    await Utils.SendJsonResponse("response-barcode-get-list-com-port", 1, "success", value: barcode);
                }
                else if (@event == "barcode-close-connection")
                {
                    bool res = BarcodeApi.closePort();
                    if (res)
                    {
                        await Utils.SendResponse("response-barcode-close-connection", 1, "success");
                    }
                    else
                    {
                        await Utils.SendResponse("response-barcode-close-connection", 0, "error: close connection");
                    }
                }
                else if (@event == "barcode-send-raw-command")
                {
                    if (BarcodeApi.IsBarcodeConected)
                    {
                        dynamic jsonObject = JsonConvert.DeserializeObject(req);
                        string commandCode = jsonObject?.value?.commandCode ?? throw new Exception("Command is null");
                        BarcodeApi.SendCommands( commandCode);
                    }
                    else
                    {
                        await Utils.SendResponse("response-barcode-send-raw-command", 0, "error: Barcode is not connected!");
                    }
                }
                else if (@event == "barcode-config")
                {
                    if (BarcodeApi.IsBarcodeConected)
                    {
                        dynamic jsonObject = JsonConvert.DeserializeObject(req);
                        string key = jsonObject?.value?.key ?? throw new Exception("Command is null");

                        List<BarcodeApi.InterfaceCommnand> commands = BarcodeApi.BarcodeJsonCommand();
                        BarcodeApi.InterfaceCommnand command = commands.Find(x => x.key == key);
                        BarcodeApi.SendCommands(command.code);
                    }
                    else
                    {
                        await Utils.SendResponse("response-barcode-config", 0, "error: Barcode is not connected!");
                    }

                }
                else if (@event == "barcode-tone-success")
                {
                    BarcodeApi.SendCommands("5700 17 1300 0200 01 005500");
                    await Task.Delay(200);
                    BarcodeApi.SendCommands("5700 17 1300 0200 01 005500");
                    await Task.Delay(200);
                    BarcodeApi.SendCommands("5700 17 1300 0200 01 005500");
                    await Task.Delay(200);
                    BarcodeApi.SendCommands("5700 17 1300 0200 01 005500");
                    await Task.Delay(200);
                    BarcodeApi.SendCommands("5700 17 1300 0200 01 005500");
                    await Task.Delay(200);
                    BarcodeApi.SendCommands("5700 17 1300 0200 01 005500");
                    await Task.Delay(200);
                    BarcodeApi.SendCommands("5700 17 1300 0100 01 005500");
                }
                else if (@event == "barcode-tone-warning")
                {
                    BarcodeApi.SendCommands("5700 17 1300 0300 01 005500");
                    await Task.Delay(1000);
                    BarcodeApi.SendCommands("5700 17 1300 0300 01 005500");
                    await Task.Delay(1000);
                    BarcodeApi.SendCommands("5700 17 1300 0300 01 005500");
                    await Task.Delay(1000);
                    BarcodeApi.SendCommands("5700 17 1300 0100 01 005500");

                }
                else if (@event == "barcode-tone-failed")
                {
                    BarcodeApi.SendCommands("5700 17 1300 0100 01 005500");
                    await Task.Delay(200);
                    BarcodeApi.SendCommands("5700 17 1300 0200 01 005500");
                    await Task.Delay(200);
                    BarcodeApi.SendCommands("5700 17 1300 0100 01 005500");
                    await Task.Delay(200);
                    BarcodeApi.SendCommands("5700 17 1300 0200 01 005500");
                    await Task.Delay(200);
                    BarcodeApi.SendCommands("5700 17 1300 0100 01 005500");
                    await Task.Delay(200);
                    BarcodeApi.SendCommands("5700 17 1300 0200 01 005500");
                    await Task.Delay(200);
                    BarcodeApi.SendCommands("5700 17 1300 0100 01 005500");
                }
                else
                {
                    await Utils.SendResponse( "error", 0, "Command is unexpected!");
                    Console.WriteLine("Command is unexpected!");
                }
            }
        }

        //Helper 
        public static bool IsJson(string str)
        {
            try
            {
                JToken.Parse(str);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
