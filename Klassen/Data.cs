using Microsoft.Data.Sqlite;

namespace VinylStorage.Klassen
{
    public class Data
    {
        public static SqliteConnection CreateConnection()
        {
            SqliteConnection sqlite_conn = new SqliteConnection("Data Source=database.db;");
            sqlite_conn.Open();
            return sqlite_conn;
        }

        public static void CreateTable(SqliteConnection conn)
        {
            SqliteCommand sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = @"CREATE TABLE IF NOT EXISTS Vinyl (
                vinyl_ID    INTEGER PRIMARY KEY,
                Artist      VARCHAR(100),
                AlbumTitle  VARCHAR(100),
                Year        INTEGER,
                Genre       VARCHAR(50),
                Condition   VARCHAR(20),
                CoverArtUrl VARCHAR(500)
            )";
            sqlite_cmd.ExecuteNonQuery();
        }

        public static void InsertData(SqliteConnection conn, Vinyl vinyl)
        {
            SqliteCommand sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = "INSERT INTO Vinyl (Artist, AlbumTitle, Year, Genre, Condition, CoverArtUrl) VALUES (@Artist, @AlbumTitle, @Year, @Genre, @Condition, @CoverArtUrl);";
            sqlite_cmd.Parameters.AddWithValue("@Artist",      vinyl.Artist);
            sqlite_cmd.Parameters.AddWithValue("@AlbumTitle",  vinyl.AlbumTitle);
            sqlite_cmd.Parameters.AddWithValue("@Year",        vinyl.Year);
            sqlite_cmd.Parameters.AddWithValue("@Genre",       vinyl.Genre);
            sqlite_cmd.Parameters.AddWithValue("@Condition",   vinyl.Condition);
            sqlite_cmd.Parameters.AddWithValue("@CoverArtUrl", vinyl.CoverArtUrl);
            sqlite_cmd.ExecuteNonQuery();
        }

        public static List<Vinyl> ReadData(SqliteConnection conn)
        {
            SqliteCommand sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = "SELECT Artist, AlbumTitle, Year, Genre, Condition, CoverArtUrl FROM Vinyl";
            List<Vinyl> list = new List<Vinyl>();

            var reader = sqlite_cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Vinyl(
                    reader.GetString(0),
                    reader.GetString(1),
                    reader.GetInt32(2),
                    reader.GetString(3),
                    reader.GetString(4),
                    reader.IsDBNull(5) ? "" : reader.GetString(5)
                ));
            }
            return list;
        }

        public static void UpdateData(SqliteConnection conn, Vinyl oldVinyl, Vinyl newVinyl)
        {
            SqliteCommand sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = @"UPDATE Vinyl SET
                Artist     = @Artist,
                AlbumTitle = @AlbumTitle,
                Year       = @Year,
                Genre      = @Genre,
                Condition  = @Condition,
                CoverArtUrl = @CoverArtUrl
                WHERE Artist = @OldArtist AND AlbumTitle = @OldAlbumTitle";
            sqlite_cmd.Parameters.AddWithValue("@Artist",        newVinyl.Artist);
            sqlite_cmd.Parameters.AddWithValue("@AlbumTitle",    newVinyl.AlbumTitle);
            sqlite_cmd.Parameters.AddWithValue("@Year",          newVinyl.Year);
            sqlite_cmd.Parameters.AddWithValue("@Genre",         newVinyl.Genre);
            sqlite_cmd.Parameters.AddWithValue("@Condition",     newVinyl.Condition);
            sqlite_cmd.Parameters.AddWithValue("@CoverArtUrl",   newVinyl.CoverArtUrl);
            sqlite_cmd.Parameters.AddWithValue("@OldArtist",     oldVinyl.Artist);
            sqlite_cmd.Parameters.AddWithValue("@OldAlbumTitle", oldVinyl.AlbumTitle);
            sqlite_cmd.ExecuteNonQuery();
        }

        public static void SaveCoverArtUrl(SqliteConnection conn, Vinyl vinyl)
        {
            SqliteCommand sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = "UPDATE Vinyl SET CoverArtUrl = @CoverArtUrl WHERE Artist = @Artist AND AlbumTitle = @AlbumTitle";
            sqlite_cmd.Parameters.AddWithValue("@CoverArtUrl", vinyl.CoverArtUrl);
            sqlite_cmd.Parameters.AddWithValue("@Artist",      vinyl.Artist);
            sqlite_cmd.Parameters.AddWithValue("@AlbumTitle",  vinyl.AlbumTitle);
            sqlite_cmd.ExecuteNonQuery();
        }

        public static void DeleteData(SqliteConnection conn, Vinyl vinyl)
        {
            SqliteCommand sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = "DELETE FROM Vinyl WHERE Artist = @Artist AND AlbumTitle = @AlbumTitle";
            sqlite_cmd.Parameters.AddWithValue("@Artist",     vinyl.Artist);
            sqlite_cmd.Parameters.AddWithValue("@AlbumTitle", vinyl.AlbumTitle);
            sqlite_cmd.ExecuteNonQuery();
        }
    }
}
