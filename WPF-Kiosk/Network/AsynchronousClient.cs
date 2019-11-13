using System;
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

        private static Socket client;

        public AsynchronousClient()
        {
            CreateSocket();
        }

        public static void CreateSocket()
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
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        public static void Send(String data)
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
                        //모든 데이터 도착, response안으로 넣음.
                        //if (state.sb.Length > 1)
                        //{
                        //    response = state.sb.ToString();
                        //}
                    }
                    receiveDone.Set();
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }
    }
}
