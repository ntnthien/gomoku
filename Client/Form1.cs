using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.IO;

using Gomoku;

namespace Client
{
    public partial class Form1 : Form
    {
        #region Atrributes
        #region Thread
        private Thread tRunning = null;
        #endregion Thread

        #region Clients data
        private TcpClient client;

        GomokuBoard board = new GomokuBoard();
        GomokuTeam team = new GomokuTeam();
        #endregion Clients data

        #region Sender and Receiver
        NetworkStream networkStream;
        #endregion Sender and Receiver

        #region Template Message
        string received_message = "";
        string prepared_message = "Ok";
        string command;

        const String get_name_incommand = "GetName";
        const String set_flag_incommand = "Flag";
        const String start_incommand = "Start";
        const String stop_incommand = "Break";

        const String team_name_outcommand = "TeamName:";
        const String point_outcommand = "Point:";
        const String donebreak_outcommand = "DoneBreak:";

        #endregion Template Message

        #region Default Port
        int serverPort = 8001;
        #endregion Default Port

        #region Temporal Variables
        int first_separate_index, second_separate_index, end_index;
        #endregion Temporal_Variables
        #endregion Attributes

        #region initialization
        public Form1()
        {
            InitializeComponent();

            team.Team_Name = GetLetter().ToString(); //Ghi ten nhom cua cac em vao day nhe
        }
        #endregion initialization

        #region Event
        #region Button
        /// <summary>
        /// Button Connect Click Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btConnect_Click(object sender, EventArgs e)
        {
            client = new TcpClient();
            IPEndPoint IP_End = new IPEndPoint(IPAddress.Parse(this.txtIP.Text), this.serverPort);

            try
            {
                client.Connect(IP_End);

                if (client.Connected)
                {
                    lblStatus.Text = "Connected";

                    networkStream = this.client.GetStream();

                    bgwReceiveData.RunWorkerAsync(); //start receiving data in background
                }
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message.ToString());
            }
        }
        #endregion Button

        #region Background Worker
        /// <summary>
        /// Message Receiving thread
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bgwReceiveData_DoWork(object sender, DoWorkEventArgs e)
        {
            while (client.Connected)
            {
                try
                {
                    byte[] buffer = new byte[100];
                    this.networkStream.Read(buffer, 0, buffer.Length);
                    this.received_message = System.Text.Encoding.Default.GetString(buffer);

                    if (received_message != null && received_message.Length > 0)
                    {
                        received_message_processing();
                    }
                }
                catch (Exception x)
                {
                    MessageBox.Show(x.ToString());
                }
            }
        }
        #endregion Background Worker

        #region Form Event
        /// <summary>
        /// Form Closing Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopGame();
        }
        #endregion Form Event
        #endregion Event

        #region Method
        #region Receiver and Sender
        /// <summary>
        /// decide to process when received the message
        /// </summary>
        private void received_message_processing()
        {
            first_separate_index = received_message.IndexOf(':');

            if (first_separate_index <= 0)
            {
                return;
            }

            command = received_message.Substring(0, first_separate_index);

            this.lblStatus.Invoke((MethodInvoker)delegate { this.lblStatus.Text = "Rec: " + received_message; });

            if (command == get_name_incommand)
            {
                prepared_message = team_name_outcommand + team.Team_Name;

                send_message();
            }
            else if (command == set_flag_incommand)
            {
                team.Flag = received_message.Substring(first_separate_index + 1)[0];
                txtPort.Text = Convert.ToString(team.Flag);
            }
            else if (command == start_incommand)
            {
                get_opponent_turn();
                StartClientThread();
            }
            else if (command == stop_incommand)
            {
                get_opponent_turn();
                StopClientThread();

                prepared_message = donebreak_outcommand;
                send_message();
            }
        }

        /// <summary>
        /// Send Message
        /// </summary>
        public void send_message()
        {
            if (client.Connected)
            {
                byte[] tmp = Encoding.UTF8.GetBytes(this.prepared_message);
                this.networkStream.Write(tmp, 0, prepared_message.Length);
            }
            else
            {
                MessageBox.Show("Fail...");
            }
        }
        #endregion Receiver and Sender

        #region Thread
        /// <summary>
        /// Bắt đầu thread liên lạc của client
        /// </summary>
        public void StartClientThread()
        {
            try
            {
                tRunning = new Thread(new ThreadStart(send_think_result));
                tRunning.Start();
            }
            catch (Exception x)
            {
                MessageBox.Show(x.ToString());
            }
        }

        /// <summary>
        /// Tắt thread liên lạc của client
        /// </summary>
        public void StopClientThread()
        {
            try
            {
                tRunning.Abort("Client: Terminated");
                tRunning.Join();
            }
            catch {};
        }

        public void StopGame()
        {
            StopClientThread();
        }
        #endregion Thread

        #region Perform The Turn
        /// <summary>
        /// Save your opponent turn
        /// </summary>
        public void get_opponent_turn()
        {
            Point p = new Point();

            second_separate_index = received_message.LastIndexOf(':');
            end_index = received_message.LastIndexOf('.');

            p.X = Convert.ToInt32(received_message.Substring(first_separate_index + 1, second_separate_index - first_separate_index - 1));
            p.Y = Convert.ToInt32(received_message.Substring(second_separate_index + 1, this.end_index - second_separate_index - 1));

            if (p.X != -1 && p.Y != -1)
            {
                this.board.select(p, this.team.Flag);
            }
        }

        /// <summary>
        /// Send the result of your turn
        /// </summary>
        public void send_think_result()
        {
            Point p = new Point();

            p = Think();

            this.board.select(p, this.team.Flag);
            prepared_message = point_outcommand + p.X + ":" + p.Y + ".";
            send_message();

            StopClientThread();
        }
        #endregion Perform The Turn

        /// Cac em se tien hanh chinh sua code trong region Your Work nay
        /// Cac region khac khong can chu tam den nhe
        #region Your Work
		int max_depth = 3;

        /// <summary>
        /// Think
        /// </summary>
        /// <returns></returns>
        private Point Think()
        {
            Point result = board.getRandomFreeCell();
            String text = result.X + "," + result.Y;
            /*
            string fileName = "C:\\txt\\" + team.Flag.ToString() + ".txt";

            using (StreamWriter writer = new StreamWriter(fileName))
            {
                writer.WriteLine(text);
            }*/
            return result;
        }

        public static char GetLetter()
        {
	    // This method returns a random lowercase letter.
	    // ... Between 'a' and 'z' inclusize.
            Random _random = new Random();
	        int num = _random.Next(0, 26); // Zero to 25
	        char let = (char)('a' + num);
	        return let;
        }

		struct Selected_cell {
			public Point p;
			public int v;
		}

		private int Valuate (GomokuBoard b) {
			return 1;
		}

        private Selected_cell MaxValue(Selected_cell[] a) {
            Selected_cell max = a[0];

            for (int i = 1; i < a.Length; i++)
            {
                if (max.v < a[i].v)
                    max = a[i];
            }

            return max;
        }

        private Selected_cell MinValue(Selected_cell[] a)
        {
            Selected_cell min = a[0];

            for (int i = 1; i < a.Length; i++)
            {
                if (min.v > a[i].v)
                    min = a[i];
            }

            return min;
        }

        private Selected_cell[] Available_cell(GomokuBoard b)
        {
			Selected_cell[] result = new Selected_cell[20];

			return result;
		}

		private GomokuBoard Clone(GomokuBoard b) {
			GomokuBoard result = new GomokuBoard ();

			result.Board = b.Board;
			result.Last_Selected_Position = b.Last_Selected_Position;

			return result;
		}

		private Selected_cell Minimax(GomokuBoard b, int depth) {
			return Max (b, depth);
		}

		private Selected_cell Max(GomokuBoard b, int depth) 
		{
			Selected_cell result;
			Selected_cell[] a = Available_cell(b);
			if (depth == max_depth) {

				for (int i = 0; i < a.Length; i++) {
					a[i].v = Valuate(b);
                    
				}
                result = MaxValue(a);
				return result;
			}

			List<Selected_cell> list_cell = new List<Selected_cell>();

			foreach (Selected_cell x in a) {
				GomokuBoard tmp = Clone(b);

                tmp.select(x.p, this.team.Flag);
				list_cell.Add(Max(tmp, depth + 1));

                result = Min(tmp, depth + 1);
			}
            result = MaxValue(list_cell.ToArray());
			return result;
		}

		private Selected_cell Min(GomokuBoard b, int depth) 
		{
            Selected_cell result;
            Selected_cell[] a = Available_cell(b);
            if (depth == max_depth)
            {

                for (int i = 0; i < a.Length; i++)
                {
                    a[i].v = Valuate(b);

                }
                result = MaxValue(a);
                return result;
            }

            List<Selected_cell> list_cell = new List<Selected_cell>();

            foreach (Selected_cell x in a)
            {
                GomokuBoard tmp = Clone(b);

                tmp.select(x.p, this.team.Flag);
                list_cell.Add(Max(tmp, depth + 1));

                result = Min(tmp, depth + 1);
            }
            result = MaxValue(list_cell.ToArray());
            return result;
		}




        #endregion Your Work
        #endregion Method
        //ends--
    }
}
