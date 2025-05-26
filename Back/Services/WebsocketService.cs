using System.Net.WebSockets;
using System.Text;

namespace Back.Services;
public class WebSocketService
{
    private readonly Dictionary<int, WebSocket> _sockets = new();

    public void AddSocket(int userId, WebSocket socket)
    {
        _sockets[userId] = socket;
    }

    public void RemoveSocket(int userId)
    {
        _sockets.Remove(userId);
    }

    public async Task SendToUserAsync(int userId, string message)
    {
        if (_sockets.TryGetValue(userId, out var socket) && socket.State == WebSocketState.Open)
        {
            var bytes = Encoding.UTF8.GetBytes(message);
            var buffer = new ArraySegment<byte>(bytes);

            await socket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }

    public async Task BroadcastAsync(string message)
    {
        var tasks = _sockets.Values
            .Where(s => s.State == WebSocketState.Open)
            .Select(socket =>
            {
                var buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(message));
                return socket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
            });

        await Task.WhenAll(tasks);
    }
}
