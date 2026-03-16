using System.Net.Http;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using VinylStorage.Klassen;

namespace VinylStorage
{
    public partial class Edit : Window
    {
        public Vinyl EditVinyl { get; set; }
        private Vinyl _originalVinyl;
        private string _fetchedCoverUrl = "";

        private static readonly HttpClient _http = new HttpClient();

        public Edit(Vinyl vinyl)
        {
            InitializeComponent();

            // MusicBrainz benötigt User-Agent header
            if (!_http.DefaultRequestHeaders.Contains("User-Agent"))
                _http.DefaultRequestHeaders.Add("User-Agent", "VinylCollection/1.0 (christopher.iiams@web2teach.de)");

            _originalVinyl = vinyl;

            inputArtist.Text     = vinyl.Artist;
            inputAlbumTitle.Text = vinyl.AlbumTitle;
            inputYear.Text       = vinyl.Year.ToString();
            inputGenre.Text      = vinyl.Genre;

            foreach (ComboBoxItem item in inputCondition.Items)
            {
                if (item.Content.ToString() == vinyl.Condition)
                {
                    inputCondition.SelectedItem = item;
                    break;
                }
            }

            // Benutz cached URL wenn verfügbra, ansonten fetch
            if (!string.IsNullOrEmpty(vinyl.CoverArtUrl))
                LoadImageFromUrl(vinyl.CoverArtUrl);
            else
                _ = FetchAndShowCoverAsync(vinyl.Artist, vinyl.AlbumTitle);
        }

        private async Task FetchAndShowCoverAsync(string artist, string album)
        {
            coverPlaceholder.Text = "Loading cover...";
            coverImage.Visibility = Visibility.Collapsed;
            coverPlaceholder.Visibility = Visibility.Visible;

            string url = await FetchCoverUrlAsync(artist, album);

            if (!string.IsNullOrEmpty(url))
            {
                _fetchedCoverUrl = url;
                LoadImageFromUrl(url);
            }
            else
            {
                coverPlaceholder.Text = "Kein Plattencover gefunden";
            }
        }

        private void LoadImageFromUrl(string url)
        {
            try
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(url);
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();

                coverImage.Source = bitmap;
                coverImage.Visibility = Visibility.Visible;
                coverPlaceholder.Visibility = Visibility.Collapsed;
                _fetchedCoverUrl = url;
            }
            catch
            {
                coverPlaceholder.Text = "Bild konnte nihct geladen werden";
            }
        }

        private async Task<string> FetchCoverUrlAsync(string artist, string album)
        {
            try
            {
                // Schritt 1: Durchsuche MusicBrainz nach dem Release
                string query = Uri.EscapeDataString($"release:{album} AND artist:{artist}");
                string searchUrl = $"https://musicbrainz.org/ws/2/release/?query={query}&limit=1&fmt=json";

                var response = await _http.GetStringAsync(searchUrl);
                using var doc = JsonDocument.Parse(response);

                var releases = doc.RootElement.GetProperty("releases");
                if (releases.GetArrayLength() == 0) return "";

                string mbid = releases[0].GetProperty("id").GetString() ?? "";
                if (string.IsNullOrEmpty(mbid)) return "";

                // Step 2: Cover von Cover Art Archive
                string coverUrl = $"https://coverartarchive.org/release/{mbid}/front-500";

                // HEAD Request, check it exists
                var headRequest = new HttpRequestMessage(HttpMethod.Head, coverUrl);
                var headResponse = await _http.SendAsync(headRequest);

                return headResponse.IsSuccessStatusCode ? coverUrl : "";
            }
            catch
            {
                return "";
            }
        }

        private void RefreshCover_Click(object sender, RoutedEventArgs e)
        {
            _ = FetchAndShowCoverAsync(inputArtist.Text, inputAlbumTitle.Text);
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(inputYear.Text, out int year))
            {
                MessageBox.Show("Bitte gültiges Jahr eingeben.", "Ungültige Eingabe");
                return;
            }

            string condition = (inputCondition.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "";

            EditVinyl = new Vinyl(
                inputArtist.Text,
                inputAlbumTitle.Text,
                year,
                inputGenre.Text,
                condition,
                _fetchedCoverUrl
            );
            this.Close();
        }
    }
}
