using MagicOnion;
using Sample.Shared.MessagePackObjects;
using System.Threading.Tasks;
using UnityEngine;

namespace Sample.Shared.Hubs
{
    /// <summary>
    /// CLient ->Server API
    /// </summary>
    public interface ISampleHub : IStreamingHub<ISampleHub, ISampleHubReceiver>
    {
        /// <summary>
        ///Tell the server to connect to the game
        /// </summary>
        Task JoinAsync(Player player);
        /// <summary>
        ///Tell the server to disconnect from the game
        /// </summary>
        Task LeaveAsync();
        /// <summary>
        ///Send a message to the server
        /// </summary>
        Task SendMessageAsync(string message);
        /// <summary>
        ///Tell the server that you have moved
        /// </summary>
        Task MovePositionAsync(Vector3 position);
    }

    /// <summary>
    /// Server ->Client API
    /// </summary>
    public interface ISampleHubReceiver
    {
        /// <summary>
        ///Tell the client that someone has connected to the game
        /// </summary>
        void OnJoin(string name);
        /// <summary>
        ///Tell the client that someone has disconnected from the game
        /// </summary>
        void OnLeave(string name);
        /// <summary>
        ///Tell the client what someone said
        /// </summary>
        void OnSendMessage(string name, string message);
        /// <summary>
        ///Tell the client that someone has moved
        /// </summary>
        void OnMovePosition(Player player);
    }
}