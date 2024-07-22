using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace TudiBarcode.modul
{
    internal class Utils
    {

        public static async Task SendResponse(string _event, int statusCode, string message, dynamic value = null)
        {
            WebSocket _ctxSocketServer = UartWebsocket._webSocket;
            object serializedValue = null;

            if (value != null)
            {
                if (value is string && !string.IsNullOrEmpty((string)value))
                {
                    serializedValue = value;
                }
                else if (value is int)
                {
                    serializedValue = value;
                }
                else if (value is JObject)
                {
                    serializedValue = value.ToString();
                }
            }

            object json = new
            {
                @event = _event,
                statusCode = statusCode,
                message = message,
                value = serializedValue
            };

            string res = JsonConvert.SerializeObject(json, Formatting.None, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
            Form1.Instance.Invoke(new Action(() =>
            {
                Form1.Instance.AppendTextToConsole(res);
            }));
            if (_ctxSocketServer != null && _ctxSocketServer.State == WebSocketState.Open)
            {
                try
                {
                    Console.WriteLine("[send][server local]" + res);

                    await _ctxSocketServer.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(res)), WebSocketMessageType.Text, true, CancellationToken.None);
                   
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[SendResponse] Error: {ex.Message}");
                }
            }
        }

        public static async Task SendJsonResponse(string _event, int statusCode, string message, object value)
        {
            WebSocket _ctxSocketServer = UartWebsocket._webSocket;
            if (_ctxSocketServer != null && _ctxSocketServer.State == WebSocketState.Open)
            {
                object json = new
                {
                    @event = _event,
                    statusCode = statusCode,
                    message = message,
                    value = object.Equals(value, null) ? null : value
                };
                string res = JsonConvert.SerializeObject(json, Formatting.None, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });

                await _ctxSocketServer.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(res)), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }

        public static byte[] StringToByteArray(string hex)
        {
            int numberChars = hex.Length;
            byte[] bytes = new byte[numberChars / 2];
            for (int i = 0; i < numberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }
    }
}
