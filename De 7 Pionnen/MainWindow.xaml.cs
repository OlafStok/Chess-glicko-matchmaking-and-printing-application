using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WindowsFormsApp2;

namespace De_7_Pionnen
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string BestandsNaam, BestandsPad;

        public MainWindow()
        {
            InitializeComponent();
            Style style = new Style();
            style.Setters.Add(new Setter(GridViewColumnHeader.FontSizeProperty, 20.0));

            PersonenTabel.Columns.Add(new DataGridTextColumn() { Header = "Id", Binding = new Binding("Id"), Visibility = Visibility.Hidden });
            PersonenTabel.Columns.Add(new DataGridTextColumn() { Header = "#", Binding = new Binding("Positie"), IsReadOnly = true, FontSize = 20, HeaderStyle = style });
            PersonenTabel.Columns.Add(new DataGridTextColumn() { Header = "+/-", Binding = new Binding("Stijging"), IsReadOnly = true, FontSize = 20, HeaderStyle = style });
            PersonenTabel.Columns.Add(new DataGridTextColumn() { Header = "Naam", Binding = new Binding("Naam"), IsReadOnly = true, FontSize = 20, HeaderStyle = style });
            PersonenTabel.Columns.Add(new DataGridTextColumn() { Header = "Rating", Binding = new Binding("glicko.Rating") { StringFormat = "##########0.###" }, IsReadOnly = true, FontSize = 20, HeaderStyle = style });
            PersonenTabel.Columns.Add(new DataGridTextColumn() { Header = "Gespeeld", Binding = new Binding("Gespeeld"), IsReadOnly = true, FontSize = 20, HeaderStyle = style });
            PersonenTabel.Columns.Add(new DataGridTextColumn() { Header = "Gewonnen", Binding = new Binding("Gewonnen"), IsReadOnly = true, FontSize = 20, HeaderStyle = style });
            PersonenTabel.Columns.Add(new DataGridTextColumn() { Header = "Verloren", Binding = new Binding("Verloren"), IsReadOnly = true, FontSize = 20, HeaderStyle = style });
            PersonenTabel.Columns.Add(new DataGridTextColumn() { Header = "Gelijk", Binding = new Binding("Gelijkspel"), IsReadOnly = true, FontSize = 20, HeaderStyle = style });
            PersonenTabel.Columns.Add(new DataGridTextColumn() { Header = "Score", Binding = new Binding("Score") { StringFormat = "##########0.###" }, IsReadOnly = true, FontSize = 20, HeaderStyle = style });
            PersonenTabel.Columns.Add(new DataGridCheckBoxColumn() { Header = "Aanwezig", Binding = new Binding("Aanwezig"), IsReadOnly = true, HeaderStyle = style});

            PersonenTabel.MouseDoubleClick += DubbelKlik;

            VulDataGrid();
        }

        private void DubbelKlik(object sender, MouseButtonEventArgs e)
        {
            DataGrid grid = (DataGrid)sender;
            if (grid.CurrentCell.Column == null)
                return;
            if (grid.CurrentCell.Column.GetType().Equals(typeof(DataGridCheckBoxColumn)))
            {
                Persoon geselecteerdePersoonInLijst = DataSources.Instance.personen.Find(persoon => persoon.Id == ((Persoon)grid.CurrentCell.Item).Id);
                geselecteerdePersoonInLijst.Aanwezig = !geselecteerdePersoonInLijst.Aanwezig;
                grid.UnselectAllCells();

                Persistentie.Opslaan(DataSources.Instance.personen, "personen");
                VulDataGrid();
                return;
            }

            if (PersonenTabel.SelectedCells.Count <= 0)
                return;
            Persoon geselecteerdPersoon = ((Persoon)PersonenTabel.SelectedCells[0].Item);

            new PersoonAanpassen(geselecteerdPersoon).ShowDialog();

            Persistentie.Opslaan(DataSources.Instance.personen, "personen");
            VulDataGrid();
        }

        private void Persoon_Toevoegen(object sender, RoutedEventArgs e)
        {
            new PersoonToevoegen().ShowDialog();

            Persistentie.Opslaan(DataSources.Instance.personen, "personen");
            VulDataGrid();
        }

        public void VulDataGrid()
        {
            PersonenTabel.Items.Clear();
            foreach (Persoon p in DataSources.Instance.personen)
            {
                if (p.Id > -1)
                    PersonenTabel.Items.Add(p);
            }
        }

        private void Persoon_Verwijderen(object sender, RoutedEventArgs e)
        {
            if (PersonenTabel.SelectedCells.Count <= 0)
                return;
            Persoon geselecteerdpersoon = (Persoon)PersonenTabel.SelectedCells[0].Item;
            if (MessageBox.Show("Weet u zeker dat u " + geselecteerdpersoon.Naam + " wilt verwijderen? ", "Verwijder persoon", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                DataSources.Instance.personen.Remove(geselecteerdpersoon);
            }

            Persistentie.Opslaan(DataSources.Instance.personen, "personen");
            VulDataGrid();
        }

        private void Matchmaking_Click(object sender, RoutedEventArgs e)
        {
            new Matchmaking().ShowDialog();
            Persistentie.Opslaan(DataSources.Instance.personen, "personen");
            VulDataGrid();
        }

        private void Delete_Click(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                Persoon_Verwijderen(sender, e);
            }
        }

        private void DubbelRoundRobin_Click(object sender, RoutedEventArgs e)
        {
            new RoundRobin(true).ShowDialog();
            Persistentie.Opslaan(DataSources.Instance.personen, "personen");
            VulDataGrid();
        }

        private void RoundRobin_Click(object sender, RoutedEventArgs e)
        {
            new RoundRobin(false).ShowDialog();
            Persistentie.Opslaan(DataSources.Instance.personen, "personen");
            VulDataGrid();
        }

        private void Nieuw_Click(object sender, RoutedEventArgs e)
        {
            DataSources.Instance.personen.Clear();
            DataSources.Instance.matchLijsten.Clear();

            Persistentie.Opslaan(DataSources.Instance.personen, "personen");
            Persistentie.Opslaan(DataSources.Instance.matchLijsten, "matchLijsten");

            VulDataGrid();
        }

        private void Openen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog() {Filter = "d7p files(*.d7p) | *.d7p", DefaultExt="d7p" };
            if (ofd.ShowDialog() == true)
            {
                BestandsNaam = ofd.FileName.Split('\\')[ofd.FileName.Split('\\').Length - 1];
                BestandsPad = ofd.FileName;
                BestandsPad = BestandsPad.Replace(BestandsNaam, "");

                string dataNaam = (string)Persistentie.Laad(BestandsPad + BestandsNaam);
                DataSources.Instance.personen = (List<Persoon>)Persistentie.Laad(dataNaam + "_personen");
                DataSources.Instance.matchLijsten = (List<MatchLijst>)Persistentie.Laad(dataNaam + "_matchLijsten");

                VulDataGrid();
            }
        }

        private void Opslaan_Click(object sender, RoutedEventArgs e)
        {
            if (BestandsNaam == null)
            {
                OpslaanAls_Click(sender, e);
            } else
            {
                Persistentie.Opslaan(DataSources.Instance.personen, BestandsNaam + "_personen");
                Persistentie.Opslaan(DataSources.Instance.matchLijsten, BestandsNaam + "_matchLijsten");
                Persistentie.Opslaan(BestandsNaam, BestandsPad + BestandsNaam);
            }
        }

        private void OpslaanAls_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog() { Filter = "d7p files(*.d7p) | *.d7p", DefaultExt = "d7p", AddExtension=true };
            if (sfd.ShowDialog() == true)
            {
                BestandsNaam = sfd.FileName.Split('\\')[sfd.FileName.Split('\\').Length - 1];
                BestandsPad = sfd.FileName;
                BestandsPad = BestandsPad.Replace(BestandsNaam, "");
                Opslaan_Click(sender, e);
            }
        }
    }
}
