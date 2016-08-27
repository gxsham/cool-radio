using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace CoolRadioV3
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class MainPage : Page
	{
		public SongInfo songInfo;
		public MainPage()
		{
			this.InitializeComponent();
			CheckTrack();
		}

		private void Play_Click(object sender, RoutedEventArgs e)
		{
			mediaElement.Volume = 1;
			Play.Visibility = Visibility.Collapsed;
			Stop.Visibility = Visibility.Visible;
		}

		private void Stop_Click(object sender, RoutedEventArgs e)
		{
			mediaElement.Volume = 0;
			Stop.Visibility = Visibility.Collapsed;
			Play.Visibility = Visibility.Visible;
		}

		public async void CheckTrack()
		{
			var client = new HttpClient();
			var link = "http://coolradio.md/checkTrack";
			string lastSong = "";
			string checkTrack;
			while (true)
			{

				checkTrack = client.GetStringAsync(link).Result;
				if (lastSong != checkTrack)
				{
					//change song name on screen
					songInfo = JsonConvert.DeserializeObject<SongInfo>(checkTrack);
					if (!string.IsNullOrEmpty(songInfo.Cover))
					{
						CurrentSong.Text = $"{songInfo.Track} - {songInfo.Artist}";
						var coverLink = songInfo.Cover;
						Cover.Source = new BitmapImage(new Uri(coverLink, UriKind.RelativeOrAbsolute));
					}
				}
				await Task.Delay(5000);
				lastSong = checkTrack;
			}
		}
	}
}
