using MagicOnion.Server.Hubs;
using Sample.Shared.Hubs;
using Sample.Shared.MessagePackObjects;
using System;
using System.Threading.Tasks;
using UnityEngine;

public class SampleHub : StreamingHubBase<ISampleHub, ISampleHubReceiver>, ISampleHub
{
    IGroup room;
    Player me;

    public async Task JoinAsync(Player player)
    {
        //All rooms are fixed
        const string roomName = "SampleRoom";
        //Join the room&Hold room
        this.room = await this.Group.AddAsync(roomName);
        //Keep your information
        me = player;
        //Notify all members participating in the room that they have participated
        this.Broadcast(room).OnJoin(me.Name);
    }

    public async Task LeaveAsync()
    {
        //Remove yourself from members in the room
        await room.RemoveAsync(this.Context);
        //Notify all members that they have left the room
        this.Broadcast(room).OnLeave(me.Name);
    }

    public async Task SendMessageAsync(string message)
    {
        //Notify all members of what they said
        this.Broadcast(room).OnSendMessage(me.Name, message);

        await Task.CompletedTask;
    }

    public async Task MovePositionAsync(Vector3 position)
    {
        //Update information on the server
        me.Position = position;

        //Notify all members of updated player information
        this.Broadcast(room).OnMovePosition(me);

        await Task.CompletedTask;
    }

    protected override ValueTask OnConnecting()
    {
        // handle connection if needed.
        Console.WriteLine($"client connected {this.Context.ContextId}");
        return CompletedTask;
    }

    protected override ValueTask OnDisconnected()
    {
        // handle disconnection if needed.
        // on disconnecting, if automatically removed this connection from group.
        return CompletedTask;
    }
}
