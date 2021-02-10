using Grpc.Core;
using MagicOnion.Client;
using Sample.Shared.Hubs;
using Sample.Shared.MessagePackObjects;
using Sample.Shared.Services;
using UnityEngine;

public class SampleController : MonoBehaviour, ISampleHubReceiver
{
    private Channel channel;
    private ISampleService sampleService;
    private ISampleHub sampleHub;

    void Start()
    {
        this.channel = new Channel("localhost:12345", ChannelCredentials.Insecure);
        this.sampleService = MagicOnionClient.Create<ISampleService>(channel);
        this.sampleHub = StreamingHubClient.Connect<ISampleHub, ISampleHubReceiver>(this.channel, this);

        //Comment out ordinary API calls
        //There is no problem if you leave it (both work with real-time communication)
        //this.SampleServiceTest(1, 2);

        this.SampleHubTest();
    }

    async void OnDestroy()
    {
        await this.sampleHub.DisposeAsync();
        await this.channel.ShutdownAsync();
    }

    /// <summary>
    ///A method for testing normal API communication
    /// </summary>
    async void SampleServiceTest(int x, int y)
    {
        var sumReuslt = await this.sampleService.SumAsync(x, y);
        Debug.Log($"{nameof(sumReuslt)}: {sumReuslt}");

        var productResult = await this.sampleService.ProductAsync(2, 3);
        Debug.Log($"{nameof(productResult)}: {productResult}");
    }

    /// <summary>
    ///Method for testing real-time communication
    /// </summary>
    async void SampleHubTest()
    {
        //Try to create your own player information
        var player = new Player
        {
            Name = "Minami",
            Position = new Vector3(0, 0, 0),
            Rotation = new Quaternion(0, 0, 0, 0)
        };

        //Connect to the game
        await this.sampleHub.JoinAsync(player);

        //Try to speak in chat
        await this.sampleHub.SendMessageAsync("Hello!");

        //Try updating location information
        player.Position = new Vector3(1, 0, 0);
        await this.sampleHub.MovePositionAsync(player.Position);

        //Try disconnecting from the game
        await this.sampleHub.LeaveAsync();
    }

    #region A group of methods called by the server in real-time communication

    public void OnJoin(string name)
    {
        Debug.Log($"{name}Entered the room");
    }

    public void OnLeave(string name)
    {
        Debug.Log($"{name}Has left the room");
    }

    public void OnSendMessage(string name, string message)
    {
        Debug.Log($"{name}: {message}");
    }

    public void OnMovePosition(Player player)
    {
        Debug.Log($"{player.Name}Has moved: {{ x: {player.Position.x}, y: {player.Position.y}, z: {player.Position.z} }}");
    }

    #endregion
}
