using System.Net.WebSockets;
using System.Security.Claims;
using System.Text;
using Back.Services;

namespace Back.Services
{
    public class WebSocketMiddleware
    {
        private readonly RequestDelegate next;
        private readonly WebSocketService webSocketService;

        public WebSocketMiddleware(RequestDelegate next, WebSocketService webSocketService)
        {
            this.next = next;
            this.webSocketService = webSocketService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Обработка только на пути /ws
            if (context.Request.Path == "/ws")
            {
                if (context.WebSockets.IsWebSocketRequest)
                {
                    // Получаем userId из Claims (JWT), который уже валидирован AuthMiddleware
                    var userId = GetUserIdFromContext(context);

                    if (userId == null)
                    {
                        context.Response.StatusCode = 401; // Unauthorized
                        return;
                    }

                    var socket = await context.WebSockets.AcceptWebSocketAsync();

                    // Регистрируем подключение
                    webSocketService.AddSocket(userId.Value, socket);

                    // Слушаем сообщения (запускаем цикл получения сообщений)
                    await Listen(socket, userId.Value);

                    // По окончании (если соединение закрылось) удаляем из сервиса
                    webSocketService.RemoveSocket(userId.Value);

                    return; // завершаем middleware, так как обработали WS
                }
                else
                {
                    context.Response.StatusCode = 400; // Bad Request - не WebSocket запрос
                    return;
                }
            }

            // Все остальные запросы передаем дальше в pipeline
            await next(context);
        }

        private int? GetUserIdFromContext(HttpContext context)
        {
            if (context.User?.Identity?.IsAuthenticated == true)
            {
                var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier) 
                                  ?? context.User.FindFirst("userId") 
                                  ?? context.User.FindFirst("id");

                if (userIdClaim != null && int.TryParse(userIdClaim.Value, out var userId))
                    return userId;
            }
            return null;
        }

        private async Task Listen(WebSocket socket, int userId)
        {
            var buffer = new byte[4096];

            while (socket.State == WebSocketState.Open)
            {
                var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
                    break;
                }

                if (result.MessageType == WebSocketMessageType.Text)
                {
                    var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                   
                }
            }
        }
    }
}
