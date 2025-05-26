using System.Net.WebSockets;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Back.Services;
using Microsoft.Extensions.Configuration;

namespace Back.Services
{
    public class WebSocketMiddleware
    {
        private readonly RequestDelegate next;
        private readonly WebSocketService webSocketService;
        private readonly IConfiguration configuration;

        public WebSocketMiddleware(RequestDelegate next, WebSocketService webSocketService, IConfiguration configuration)
        {
            this.next = next;
            this.webSocketService = webSocketService;
            this.configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            Console.WriteLine($"Request to {context.Request.Path}, IsWebSocketRequest: {context.WebSockets.IsWebSocketRequest}");

            if (context.Request.Path == "/ws")
            {
                if (context.WebSockets.IsWebSocketRequest)
                {
                    Console.WriteLine("Accepting WebSocket connection");

                    var socket = await context.WebSockets.AcceptWebSocketAsync();

                    var userId = GetUserIdFromJwtCookie(context) ?? -1; // или как будешь обрабатывать анонимных

                    await Listen(socket, userId);

                    return; // Важно — не передавать дальше по конвейеру
                }
                else
                {
                    Console.WriteLine("Rejected: not WebSocket request");
                    context.Response.StatusCode = 400;
                    return;
                }
            }

            await next(context);
        }

        private int? GetUserIdFromJwtCookie(HttpContext context)
        {
            var token = context.Request.Cookies["jwt"];
            if (string.IsNullOrEmpty(token))
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(configuration["JwtSettings:SecretKey"] ?? "default_secret_key");

            try
            {
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier)
                                ?? principal.FindFirst("userId")
                                ?? principal.FindFirst("id");

                if (userIdClaim != null && int.TryParse(userIdClaim.Value, out var userId))
                    return userId;
            }
            catch
            {
                // Неверный токен
            }

            return null;
        }

        private async Task Listen(WebSocket socket, int userId)
        {
            // Добавляем в словарь подключений
            webSocketService.AddSocket(userId, socket);
            
            Console.WriteLine($"Пользователь подключился с userId = {userId}");
            Console.WriteLine($"Всего подключений: {webSocketService.GetConnectionsCount()}");

            var buffer = new byte[4096];

            while (socket.State == WebSocketState.Open)
            {
                var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    Console.WriteLine($"Пользователь {userId} отключился.");
                    await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);

                    // Удаляем из словаря при отключении
                    webSocketService.RemoveSocket(userId);
                    Console.WriteLine($"Всего подключений: {webSocketService.GetConnectionsCount()}");

                    break;
                }

                if (result.MessageType == WebSocketMessageType.Text)
                {
                    var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    Console.WriteLine($"Сообщение от пользователя {userId}: {message}");

                    var response = Encoding.UTF8.GetBytes("Сервер получил: " + message);
                    await socket.SendAsync(new ArraySegment<byte>(response), WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
        }
    }
}
