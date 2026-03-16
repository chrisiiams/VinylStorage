using System.Windows;
using System.Windows.Controls;
using Microsoft.Data.Sqlite;
using VinylStorage.Klassen;

namespace VinylStorage
{
    public partial class MainWindow : Window
    {
        List<Vinyl> vinyls = new List<Vinyl>();
        SqliteConnection connection { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            connection = Data.CreateConnection();
            Data.CreateTable(connection);
            vinyls = Data.ReadData(connection);

            output.ItemsSource = vinyls;
        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new Add();
            addWindow.ShowDialog();

            if (addWindow.NewVinyl != null)
            {
                vinyls.Add(addWindow.NewVinyl);
                Data.InsertData(connection, addWindow.NewVinyl);
                RefreshList();
            }
        }

        private void edit_Click(object sender, RoutedEventArgs e)
        {
            if (output.SelectedItem == null) return;

            Vinyl selected = (Vinyl)output.SelectedItem;
            var editWindow = new Edit(selected);
            editWindow.ShowDialog();

            if (editWindow.EditVinyl != null)
            {
                Data.UpdateData(connection, selected, editWindow.EditVinyl);
                int index = vinyls.IndexOf(selected);
                vinyls[index] = editWindow.EditVinyl;
                RefreshList();
            }
        }

        private void DelButton_Click(object sender, RoutedEventArgs e)
        {
            if (output.SelectedItem == null) return;

            Vinyl selected = (Vinyl)output.SelectedItem;
            Data.DeleteData(connection, selected);
            vinyls.Remove(selected);
            RefreshList();
        }

        private void input_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (output == null) return;

            var filtered = vinyls.Where(v =>
                v.Artist.ToUpper().Contains(input.Text.ToUpper()) ||
                v.AlbumTitle.ToUpper().Contains(input.Text.ToUpper())).ToList();

            output.ItemsSource = filtered;
        }

        private void RefreshList()
        {
            output.ItemsSource = null;
            output.ItemsSource = vinyls;
        }
    }
}
