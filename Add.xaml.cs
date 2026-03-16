using System.Windows;
using System.Windows.Controls;
using VinylStorage.Klassen;

namespace VinylStorage
{
    public partial class Add : Window
    {
        public Vinyl NewVinyl { get; set; }

        public Add()
        {
            InitializeComponent();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(inputYear.Text, out int year))
            {
                MessageBox.Show("Please enter a valid year.", "Invalid Input");
                return;
            }

            string condition = (inputCondition.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "";

            NewVinyl = new Vinyl(
                inputArtist.Text,
                inputAlbumTitle.Text,
                year,
                inputGenre.Text,
                condition
            );
            this.Close();
        }
    }
}
