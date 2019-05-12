using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using NAudio.Wave;
using System.Threading;
namespace WpfApp53
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	/// 
	public partial class MainWindow : Window
	{
		string soundForWriteMessege = @"C:\Users\MSI\Desktop\MyMessenger\My-Messenger\WpfApp53\WpfApp53\WriteMessege.wav";
		string soundForReadMessege = @"C:\Users\MSI\Desktop\MyMessenger\My-Messenger\WpfApp53\WpfApp53\ReadMessege.wav";
		SoundPlayer player = new SoundPlayer();
		static int port = 23333;
		static IPAddress ipDest = IPAddress.Broadcast;
		string name;
		public MainWindow()
		{
			InitializeComponent();
			Messges.IsEnabled = false;
			Messge.IsEnabled = false;
			SendMessege.IsEnabled = false;
			Task.Factory.StartNew(() => WriteMessge());
		}
		public void Sound(string soundFile,SoundPlayer player)
		{
			player.SoundLocation = soundFile;
			player.Play();
		}
		private void Button_Click(object sender, RoutedEventArgs e)
		{
			name = NikName.Text;
			
			Messges.IsEnabled = true;
			Messge.IsEnabled = true;
			SendMessege.IsEnabled = true;
		}
		public void Send(string messege)
		{
			byte[] bytes = Encoding.Default.GetBytes(messege);
			UdpClient client = new UdpClient();
			client.Send(bytes, bytes.Length, new IPEndPoint(ipDest, port));
			client.Close();

		}
		public void WriteMessge()
		{
			try
			{

				while (true)
				{
					UdpClient client = new UdpClient(port);
					IPEndPoint remoteIP = null;
					byte[] bytes = client.Receive(ref remoteIP);
					string messegeToWrite = Encoding.Default.GetString(bytes);
					Messges.Items.Add($"{Messge.Text}");
					Thread sound = new Thread(() => Sound(soundForWriteMessege, player));
					sound.Start();
					client.Close();
				}
			}
			catch(InvalidOperationException)
			{
				Console.WriteLine("Exeqtion");
			}
		}
		private void SendMessge_Click(object sender, RoutedEventArgs e)
		{
			Send(SendMessege.Content.ToString());
			Messges.Items.Add($"{NikName.Text} || {Messge.Text}					{DateTime.Now}");
			Thread sound = new Thread(() => Sound(soundForReadMessege, player));
			sound.Start();
			Messge.Text = "";
		}
	}
}
