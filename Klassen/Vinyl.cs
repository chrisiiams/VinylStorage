namespace VinylStorage.Klassen
{
    public class Vinyl
    {
        public string Artist { get; set; }
        public string AlbumTitle { get; set; }
        public int Year { get; set; }
        public string Genre { get; set; }
        public string Condition { get; set; }
        public string CoverArtUrl { get; set; }

        public Vinyl(string artist, string albumTitle, int year, string genre, string condition, string coverArtUrl = "")
        {
            Artist      = artist;
            AlbumTitle  = albumTitle;
            Year        = year;
            Genre       = genre;
            Condition   = condition;
            CoverArtUrl = coverArtUrl;
        }
    }
}
