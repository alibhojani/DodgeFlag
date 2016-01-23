using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {

	private string typeName = "DodgeFlag"; 
	private string roomName = "room1"; 

	HostData[] hostList; 

	void StartServer () { 
		Network.InitializeServer (4, 2500, !Network.HavePublicAddress()); 
		MasterServer.ipAddress = "127.0.0.1";
		MasterServer.port = 23466;
		MasterServer.RegisterHost(typeName, roomName);
	}

	void OnServerInitialized () { 
		Debug.Log("Server Initialized!"); 
		//SpawnPlayer
	}

	void RefreshHostList() { 
		MasterServer.ipAddress = "127.0.0.1";
		MasterServer.port = 23466;
		MasterServer.RequestHostList(typeName); 
	}

	void OnMasterServerEvent (MasterServerEvent msEvent) { 
		if (msEvent == MasterServerEvent.HostListReceived) { 
			hostList = MasterServer.PollHostList(); 
		}
	}

	private void JoinServer (HostData hostData) { 
		Network.Connect(hostData); 
	}

	void OnConnectToServer () { 
		Debug.Log ("Server Joined"); 
		//Spawn player
	}

	//add GUI to start game

}
