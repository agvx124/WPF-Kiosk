﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace WPF_Kiosk.Network
{
    public class StateObject
    {
        //클라이언트 소켓
        public Socket workSocket = null;
        //receive buffer Size
        public const int BufferSize = 256;
        // receive buffer.
        public byte[] buffer = new byte[BufferSize];
        // receive data string.
        public StringBuilder sb = new StringBuilder();
    }
    public class AsynchronousClient
    {
        private const int port = 80;

        private static ManualResetEvent connectDone =
                new ManualResetEvent(false);
        private static ManualResetEvent sendDone =
                new ManualResetEvent(false);
        private static ManualResetEvent receiveDone =
                new ManualResetEvent(false);

        public static bool isLogin = false;
        private static Socket client;
        private static String response = String.Empty;

        public static void loginSocket(String message)
        {
            try
            {
                IPAddress ipAddress = IPAddress.Parse("10.80.163.138");
                IPEndPoint remoteEp = new IPEndPoint(ipAddress, port);

                // TCP/IP Socket 생성
                client = new Socket(ipAddress.AddressFamily,
                    SocketType.Stream, ProtocolType.Tcp);

                // 원거리 endpoint 연결
                client.BeginConnect(remoteEp,
                    new AsyncCallback(ConnectCallback), client);
                connectDone.WaitOne();
                isLogin = true;

                Send(message);
                sendDone.WaitOne();

                // Receive the response from the remote device.  
                Receive(client);
                receiveDone.WaitOne();

                Console.WriteLine("Response received : {0}", response);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        public static void SendMessage(string message)
        {
            if (isLogin)
            {
                Send(message);

                sendDone.WaitOne();

                Receive(client);
                receiveDone.WaitOne();
                Console.WriteLine("Response received : {0}", response);
            }
            else
            {
                MessageBox.Show("서버가 종료되어 있습니다. 다시 연결하여 주세요");
            }
        }

        private static void Send(String data)
        {
            //데이터 byte형으로 변환
            byte[] byteData = Encoding.UTF8.GetBytes(data);

            //데이터 전송 시작
            try
            {
                client.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), client);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }

        }

        // 소켓 서버와 연결되어 있는지 확인하는 함수
        public static bool IsConnected()
        {
            return isLogin;
        }

        // 소켓 서버와 연결을 끊는 함수
        public static void UnConnected()
        {
            try
            {
                client.Shutdown(SocketShutdown.Both);
                client.Close();
            } catch(Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            
        }

        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                Socket client = (Socket)ar.AsyncState;


                int byteSent = client.EndSend(ar);
                Console.WriteLine("Sent {0} bytes to server.", byteSent);

                // Signal that all bytes have been sent.
                sendDone.Set();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        //IAsyncResult == 비동기 작업의 상태를 나타내는 인터페이스
        private static void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                //state object로부터 socket 검색?
                Socket client = (Socket)ar.AsyncState;

                // 연결 완료
                client.EndConnect(ar);

                Console.WriteLine("Socket connected to {0}",
                    client.RemoteEndPoint.ToString());

                // Signa that the connection has been made.
                connectDone.Set();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        private static void Receive(Socket client)
        {
            try
            {
                StateObject state = new StateObject();
                state.workSocket = client;

                //원거리 기기로부터 데이터 받아오기 시작
                client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReceiveCallback), state);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        private static void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                //asynchronous state object에서
                //state object와 client socket 검색
                StateObject state = (StateObject)ar.AsyncState;
                Socket client = state.workSocket;

                //원거리 기기에서 온 데이터 읽음
                if (client.Connected)
                {
                    int bytesRead = client.EndReceive(ar);

                    if (bytesRead > 0)
                    {
                        //더 많은 데이터가 있을 수도 있으므로 지금까지 받아온 데이터 저장.
                        state.sb.Append(Encoding.UTF8.GetString(state.buffer, 0, bytesRead));

                        //나머지 데이터 받아옴
                        client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                            new AsyncCallback(ReceiveCallback), state);
                    }
                    else
                    {
                        //서버 종료 상태
                        isLogin = false;
                    }

                    if (state.sb.Length > 1)
                    {
                        response = state.sb.ToString();
                    }

                    receiveDone.Set();
                }
                else
                {
                    isLogin = false;
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }
    }
}
