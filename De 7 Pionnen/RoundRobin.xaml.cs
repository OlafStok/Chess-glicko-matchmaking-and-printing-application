using System;
using System.Collections;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace De_7_Pionnen
{
    /// <summary>
    /// Interaction logic for RoundRobin.xaml
    /// </summary>
    public partial class RoundRobin : Window
    {
        private List<Versus> versusLijst = new List<Versus>();
        private List<Persoon> aanwezigePersonen = new List<Persoon>();
        private string huidigeNaam;
        public RoundRobin(bool doubleRoundRobin)
        {
            foreach (Persoon p in DataSources.Instance.personen)
            {
                if (p.Aanwezig && p.Id > -1)
                    aanwezigePersonen.Add(p);
            }
            if (doubleRoundRobin)
                DoubleRoundRobin();
            else
                SingleRoundRobin();

            InitializeComponent();

            Style style = new Style();
            style.Setters.Add(new Setter(GridViewColumnHeader.FontSizeProperty, 20.0));

            HuidigeMatches.Columns.Add(new DataGridTextColumn() { Header = "Id", Binding = new Binding("Id"), Visibility = Visibility.Hidden });
            HuidigeMatches.Columns.Add(new DataGridTextColumn() { Header = "Wit", Binding = new Binding("Wit.Naam"), IsReadOnly = true, FontSize = 20, HeaderStyle = style});
            HuidigeMatches.Columns.Add(new DataGridTextColumn() { Header = "Zwart", Binding = new Binding("Zwart.Naam"), IsReadOnly = true, FontSize = 20, HeaderStyle = style });
            HuidigeMatches.Columns.Add(new DataGridTextColumn() { Header = "Uitslag", Binding = new Binding("Uitslag"), IsReadOnly = true, FontSize = 20, HeaderStyle = style });

            HuidigeMatches.MouseDoubleClick += DubbelKlik;

            VulDataGrid();
        }

        public void VulDataGrid()
        {

            HuidigeMatches.Items.Clear();
            foreach (Versus v in versusLijst)
            {
                HuidigeMatches.Items.Add(v);
            }
        }

        private void Print_Click(object sender, RoutedEventArgs e)
        {
            List<List<string>> items = new List<List<string>>();
            foreach (Versus versus in versusLijst)
            {
                items.Add(new List<string> { versus.Wit == null ? "" : versus.Wit.Naam, versus.Zwart == null ? "" : versus.Zwart.Naam, versus.Uitslag == null ? "-" : versus.Uitslag });
            }
            new PrintDG().printDG("Matches: " + DateTime.Now.ToString("dd/MM/yyyy"), new List<string> { "Wit", "Zwart", "Uitslag" }, items);
        }

        private void Match_Toevoegen(object sender, RoutedEventArgs e)
        {
            Versus geselecteerdeVersus = new Versus();
            int id = 0;
            foreach (Versus versus in versusLijst)
            {
                if (id <= versus.Id)
                    id = versus.Id + 1;
            }
            geselecteerdeVersus.Id = id;
            versusLijst.Add(geselecteerdeVersus);
            new VersusAanpassen(geselecteerdeVersus, true).ShowDialog();

            VulDataGrid();
        }

        private void Match_Verwijderen(object sender, RoutedEventArgs e)
        {
            if (HuidigeMatches.SelectedItem == null)
                return;
            if (MessageBox.Show("Weet u zeker dat u de match " + HuidigeMatches.SelectedItem.ToString() + " wilt verwijderen?", "Match verwijderen", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                versusLijst.Remove((Versus)HuidigeMatches.SelectedItem);
                VulDataGrid();
            }
        }

        private void DubbelKlik(object sender, MouseButtonEventArgs e)
        {
            DataGrid grid = (DataGrid)sender;

            if (HuidigeMatches.SelectedCells.Count <= 0)
                return;
            Versus geselecteerdeVersus = ((Versus)HuidigeMatches.SelectedCells[0].Item);

            if (geselecteerdeVersus.Wit.Id < 0)
                return;
            new VersusAanpassen(geselecteerdeVersus, ref versusLijst).ShowDialog();

            VulDataGrid();
        }

        private void Delete_Click(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                Match_Verwijderen(sender, e);
            }
        }

        private void Matchmaking_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SingleRoundRobin()
        {
            List<Persoon> witLijst = new List<Persoon>();
            List<Persoon> zwartLijst = new List<Persoon>();
            Persoon statischePersoon = new Persoon();

            if (aanwezigePersonen.Count % 2 == 1)
            {
                aanwezigePersonen.Add(new Persoon(-999, "GeenTegenstander"));
            }

            for (int i = 0; i < aanwezigePersonen.Count; i++)
            {
                if (i % 2 == 1)
                    witLijst.Add(aanwezigePersonen[i]);
                else
                    zwartLijst.Add(aanwezigePersonen[i]);
            }
            GenereerRoundRobin(witLijst, zwartLijst);
        }

        private void DoubleRoundRobin()
        {
            List<Persoon> witLijst = new List<Persoon>();
            List<Persoon> zwartLijst = new List<Persoon>();
            Persoon statischePersoon = new Persoon();

            if (aanwezigePersonen.Count % 2 == 1)
            {
                aanwezigePersonen.Add(new Persoon(-999, "GeenTegenstander"));
            }

            for (int i = 0; i < aanwezigePersonen.Count; i++)
            {
                if (i % 2 == 1)
                    witLijst.Add(aanwezigePersonen[i]);
                else
                    zwartLijst.Add(aanwezigePersonen[i]);
            }
            SingleRoundRobin();
            var ronde2versuses = new List<Versus>();
            foreach (Versus v in versusLijst)
            {
                if (v.Wit.Id > -1)
                {
                    ronde2versuses.Add(new Versus() { Wit = v.Zwart, Zwart = v.Wit, Id = versusLijst.Count });
                } else
                {
                    //voeg divider toe
                    if (DataSources.Instance.personen.Find(p => p.Id == v.Wit.Id - (aanwezigePersonen.Count-1)) == null)
                        DataSources.Instance.personen.Add(new Persoon(v.Wit.Id - (aanwezigePersonen.Count-1), "Ronde " + (-1 * v.Wit.Id + (aanwezigePersonen.Count-1))));
                    Persoon divider = DataSources.Instance.personen.Find(p => p.Id == v.Wit.Id - (aanwezigePersonen.Count-1));
                    ronde2versuses.Add(new Versus() { Wit = divider, Zwart = divider });
                }
            }
            versusLijst.AddRange(ronde2versuses);
        }

        public void GenereerRoundRobin(List<Persoon> witLijst, List<Persoon> zwartLijst, int startRonde = 0)
        {
            for (int ronde = 1; ronde <= aanwezigePersonen.Count - 1; ronde++)
            {
                //voeg divider toe
                if (DataSources.Instance.personen.Find(p => p.Id == -1 * (ronde + startRonde)) == null)
                    DataSources.Instance.personen.Add(new Persoon(-1 * (ronde + startRonde), "Ronde " + (ronde + startRonde)));
                Persoon divider = DataSources.Instance.personen.Find(p => p.Id == -1 * (ronde + startRonde));
                versusLijst.Add(new Versus() { Wit = divider, Zwart = divider });


                for (int i = 0; i < witLijst.Count; i++)
                {
                    if (zwartLijst[i].Id != -999 && witLijst[i].Id != -999)
                        versusLijst.Add(new Versus() { Id = versusLijst.Count, Wit = witLijst[i], Zwart = zwartLijst[i] });
                }

                //schuif iedereen 1 positie op, behalve de eerste van witlijst
                Persoon bewegendeWit = witLijst[1];
                Persoon bewegendeZwart = zwartLijst[zwartLijst.Count - 1];
                witLijst.RemoveAt(1);
                zwartLijst.RemoveAt(zwartLijst.Count - 1);

                witLijst.Add(bewegendeZwart);
                zwartLijst.Insert(0, bewegendeWit);
            }
        }
    }
}
