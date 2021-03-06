using System.Collections;
using System;
using System.Net.Sockets;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using ArtDotNet.Packets;

namespace ArtDotNet
{
	public class ArtNetClient : MonoBehaviour
	{


		public byte[] DMXdata = new byte[512];
		public int liscenedUniverse = 0;
		public const int PORT = 6454;
		public const string NAME = "ArtNetServer";		
		int universe, net, subnet;

		public string Name { get; set; }

		public IPAddress Address { get; set; }

		public int Port { get; set; }

		UdpCommunicator communicator;

		public event EventHandler<ArtNetPacket> PacketReceived;
		public event EventHandler<ArtPollPacket> PollPacketReceived;
		public event EventHandler<ArtDmxPacket> DmxPacketReceived;



		public ArtNetClient() : this(NAME)
		{

		}

		public ArtNetClient(string name) : this(name, IPAddress.Any, PORT)
		{

		}

		public ArtNetClient(string name, IPAddress address, int port)
		{
			Name = name;
			Address = address;
			Port = port;
		}

	    void Start()
	    {
		    communicator = new UdpCommunicator();
		    communicator.DataReceived += Communicator_DataReceived;
		    communicator.Start(Address, Port);
	    }
	


		public void Stop()
		{
			communicator.Stop();
		}



		void Communicator_DataReceived(object sender, UdpPacket e)
		{
			var packet = new ArtNetPacket(e.EndPoint, e.RawData);
			//Debug.Log("Received From: " + e.EndPoint);
			if (packet.IsValid)
			{
				universe = packet.RawData[14]%16;
				net = packet.RawData[15];
				subnet = packet.RawData[14]/16;
                // Update DMX Data
                if (universe == liscenedUniverse)
                {
					for (int i = 0; i < 512; i++)
					{
						DMXdata[i] = packet.RawData[18 + i];
					}	
				}
				
				
				

			}
		}
		public void ReloadArtNet()
        {
			Stop();
			communicator = new UdpCommunicator();
			communicator.DataReceived += Communicator_DataReceived;
			communicator.Start(Address, Port);
		}
	}
}