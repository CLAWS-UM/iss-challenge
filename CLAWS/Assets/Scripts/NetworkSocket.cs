using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Net.Sockets;


public class NetworkSocket : MonoBehaviour
{
    public String host = "localhost";
    public Int32 port = 50000;
    public String file_path = "C:\\Users\roger\\Music\\MicStreamRecordings\\MicrophoneTest.wav";

    private const int BufferSize = 1024;
    private byte[] SendingBuffer = null;

    internal Boolean socket_ready = false;
    internal String input_buffer = "";
    TcpClient tcp_socket;
    NetworkStream net_stream;
    FileStream file_stream;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (!File.Exists(file_path))
            {
                Debug.Log("Audio recording does not exist at " + file_path);
            }
            else 
            {
                writeSocket();
                Debug.Log("Audio recording sent to " + host + ":" + port);

            }
        }
    }


    void Awake()
    {
        setupSocket();
    }

    void OnApplicationQuit()
    {
        closeSocket();
    }

    public void setupSocket()
    {
        try
        {
            tcp_socket = new TcpClient(host, port);
            net_stream = tcp_socket.GetStream();
            file_stream = new FileStream(file_path, FileMode.Open, FileAccess.Read);
            socket_ready = true;
        }
        catch (Exception e)
        {
          // Something went wrong
            Debug.Log("Socket error: " + e);
        }
    }

    public void writeSocket()
    {
        int NoOfPackets = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(file_stream.Length)
                        / Convert.ToDouble(BufferSize)));
        int TotalLength = (int)file_stream.Length, CurrentPacketLength = 0;
        for (int i = 0; i < NoOfPackets; i++)
        {
            if (TotalLength > BufferSize)
            {
                CurrentPacketLength = BufferSize;
                TotalLength = TotalLength - CurrentPacketLength;
            }
            else
            {
                CurrentPacketLength = TotalLength;
            }
            SendingBuffer = new byte[CurrentPacketLength];
            file_stream.Read(SendingBuffer, 0, CurrentPacketLength);
            net_stream.Write(SendingBuffer, 0, (int)SendingBuffer.Length);
        }
    }

    public void closeSocket()
    {
        if (!socket_ready)
        {
            return;
        }

        file_stream.Close();
        net_stream.Close();
        tcp_socket.Close();
        socket_ready = false;
    }

}