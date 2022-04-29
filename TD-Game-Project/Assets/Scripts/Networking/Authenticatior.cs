using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using System.Linq;

/*
    Documentation: https://mirror-networking.gitbook.io/docs/components/network-authenticators
    API Reference: https://mirror-networking.com/docs/api/Mirror.NetworkAuthenticator.html
*/

public class Authenticatior : NetworkAuthenticator
{
    [SerializeField] NetworkRoomManager Room;
    readonly HashSet<NetworkConnection> connectionsPendingDisconnect = new HashSet<NetworkConnection>();
    public string PlayerName { get; set; }

    #region Messages

    public struct AuthRequestMessage : NetworkMessage {
        public string authUsername;
    }

    public struct AuthResponseMessage : NetworkMessage {
        public Response code;
    }

    #endregion

    #region Server

    /// <summary>
    /// Called on server from StartServer to initialize the Authenticator
    /// <para>Server message handlers should be registered in this method.</para>
    /// </summary>
    public override void OnStartServer()
    {
        // register a handler for the authentication request we expect from client
        NetworkServer.RegisterHandler<AuthRequestMessage>(OnAuthRequestMessage, false);
    }

    /// <summary>
    /// Called on server from StopServer to reset the Authenticator
    /// <para>Server message handlers should be registered in this method.</para>
    /// </summary>
    public override void OnStopServer()
    {
        // unregister the handler for the authentication request
        NetworkServer.UnregisterHandler<AuthRequestMessage>();
    }
    /// <summary>
    /// Called on server from OnServerAuthenticateInternal when a client needs to authenticate
    /// </summary>
    /// <param name="conn">Connection to client.</param>
    public override void OnServerAuthenticate(NetworkConnectionToClient conn) { }

    /// <summary>
    /// Called on server when the client's AuthRequestMessage arrives
    /// </summary>
    /// <param name="conn">Connection to client.</param>
    /// <param name="msg">The message payload</param>
    public void OnAuthRequestMessage(NetworkConnectionToClient conn, AuthRequestMessage msg)
    {
        Debug.Log($"Authentication Request: {msg.authUsername}");

        if (connectionsPendingDisconnect.Contains(conn)) return;

        // check the credentials by calling your web server, database table, playfab api, or any method appropriate.
        //if(NetworkServer.connections.Values.Count(conn => msg.authUsername==(string)conn.authenticationData) == 0)
        if (true)//Most mindenkit beenged�nk
        {

            // Add the name to the HashSet
            //Player.playerNames.Add(msg.authUsername);
            // Not here thank you


            // Store username in authenticationData
            // This will be read in RoomUIManager.HandleClientConnected
            // to set the playerName
            conn.authenticationData = msg.authUsername;

            // create and send msg to client so it knows to proceed
            AuthResponseMessage authResponseMessage = new AuthResponseMessage
            {
                code = Response.Success,
            };

            conn.Send(authResponseMessage);

            // Accept the successful authentication
            ServerAccept(conn);
        }
        /*
        else
        {
            connectionsPendingDisconnect.Add(conn);

            // create and send msg to client so it knows to disconnect
            AuthResponseMessage authResponseMessage = new AuthResponseMessage
            {
                code = Response.NameTaken,
            };

            conn.Send(authResponseMessage);

            // must set NetworkConnection isAuthenticated = false
            conn.isAuthenticated = false;

            // disconnect the client after 1 second so that response message gets delivered
            StartCoroutine(DelayedDisconnect(conn, 1f));
        }
        */
    }

    IEnumerator DelayedDisconnect(NetworkConnectionToClient conn, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        // Reject the unsuccessful authentication
        ServerReject(conn);

        yield return null;

        // remove conn from pending connections
        connectionsPendingDisconnect.Remove(conn);
    }

    #endregion

    #region Client

    /// <summary>
    /// Called on client from StartClient to initialize the Authenticator
    /// <para>Client message handlers should be registered in this method.</para>
    /// </summary>
    public override void OnStartClient()
    {
        // register a handler for the authentication response we expect from server
        NetworkClient.RegisterHandler<AuthResponseMessage>(OnAuthResponseMessage, false);
    }
    public override void OnStopClient()
    {
        // unregister the handler for the authentication response
        NetworkClient.UnregisterHandler<AuthResponseMessage>();
    }

    /// <summary>
    /// Called on client from OnClientAuthenticateInternal when a client needs to authenticate
    /// </summary>
    public override void OnClientAuthenticate()
    {
        AuthRequestMessage authRequestMessage = new AuthRequestMessage
        {
            authUsername = PlayerName,
        };

        NetworkClient.connection.Send(authRequestMessage);
    }

    /// <summary>
    /// Called on client when the server's AuthResponseMessage arrives
    /// </summary>
    /// <param name="msg">The message payload</param>
    public void OnAuthResponseMessage(AuthResponseMessage msg)
    {
        if (msg.code == Response.Success)
        {
            Debug.Log($"Authentication Response: Success");

            // Authentication has been accepted
            ClientAccept();
        }
        else
        {
            Debug.LogError($"Authentication Response: Failed");

            // Authentication has been rejected
            // StopHost works for both host client and remote clients
            Room.StopHost();

            // -------USE UI TO TELL USER WHATS WRONG HERE-------
        }
    }

    #endregion

}