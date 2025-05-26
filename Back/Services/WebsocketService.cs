using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using Back.Dtos;

namespace Back.Services;

public class WebSocketService
{
    private readonly ConcurrentDictionary<int, WebSocket> _sockets = new();

    public void AddSocket(int userId, WebSocket socket)
    {
        _sockets[userId] = socket;
    }

    public void RemoveSocket(int userId)
    {
        _sockets.TryRemove(userId, out _);
    }
    public int GetConnectionsCount()
    {
        lock(_sockets)
        {
            return _sockets.Count;
        }
    }
    public async Task SendToUserAsync(int userId, string message)
    {
        if (_sockets.TryGetValue(userId, out var socket) &&
            socket.State == WebSocketState.Open)
        {
            var buffer = Encoding.UTF8.GetBytes(message);
            await socket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }

    public async Task BroadcastToUsersAsync(IEnumerable<int> userIds, ShareNotificationDTO dto)
    {
        var json = JsonSerializer.Serialize(dto, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        var buffer = Encoding.UTF8.GetBytes(json);

        foreach (var id in userIds)
        {
            if (_sockets.TryGetValue(id, out var socket) && socket.State == WebSocketState.Open)
            {
                Console.WriteLine($"Отправляю сообщение пользователю {id}: {json}");
                await socket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
            }
            else
            {
                Console.WriteLine($"Пользователь {id} не подключён или сокет не открыт");
            }
        }
    }
    public bool IsUserConnected(int userId)
    {
        return _sockets.TryGetValue(userId, out var socket) && socket.State == WebSocketState.Open;
    }
}

