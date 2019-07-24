using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace De_7_Pionnen
{
    /// <summary>
    /// Interaction logic for Matchmaking.xaml
    /// </summary>
    public partial class Matchmaking : Window
    {
        MatchLijst huidigeMatchLijst, vorigeMatchLijst;
        int VorigeTegenstanderCount = 3;
        public Matchmaking()
        {
            //kijk of er een huidige lijst is. zoniet, maak er dan een aan
            List<MatchLijst> matchLijsten = DataSources.Instance.matchLijsten;
            if (matchLijsten == null || matchLijsten.Find(matchLijst => matchLijst.huidigeLijst) == null)
            {
                MatchLijst.GenereerMatchLijst();
            }
            huidigeMatchLijst = matchLijsten.Find(matchlijst => matchlijst.huidigeLijst);

            int vorigeNummer = -1;
            vorigeMatchLijst = matchLijsten.IndexOf(huidigeMatchLijst) <= 0 ? new MatchLijst() : matchLijsten[matchLijsten.IndexOf(huidigeMatchLijst) - 1];
            if (vorigeMatchLijst == null)
                vorigeMatchLijst = new MatchLijst();
            InitializeComponent();

            Style style = new Style();
            style.Setters.Add(new Setter(GridViewColumnHeader.FontSizeProperty, 20.0));

            //vul de tabel met huidige matchlijst
            HuidigeMatches.Columns.Add(new DataGridTextColumn() { Header = "Id", Binding = new Binding("Id"), Visibility = Visibility.Hidden });
            HuidigeMatches.Columns.Add(new DataGridTextColumn() { Header = "Wit", Binding = new Binding("Wit.Naam"), IsReadOnly = true, FontSize = 20, HeaderStyle = style });
            HuidigeMatches.Columns.Add(new DataGridTextColumn() { Header = "Zwart", Binding = new Binding("Zwart.Naam"), IsReadOnly = true, FontSize = 20, HeaderStyle = style });
            HuidigeMatches.Columns.Add(new DataGridTextColumn() { Header = "Uitslag", Binding = new Binding("Uitslag"), IsReadOnly = true, FontSize = 20, HeaderStyle = style });

            VorigeMatches.Columns.Add(new DataGridTextColumn() { Header = "Id", Binding = new Binding("Id"), Visibility = Visibility.Hidden });
            VorigeMatches.Columns.Add(new DataGridTextColumn() { Header = "Wit", Binding = new Binding("Wit.Naam"), IsReadOnly = true, FontSize = 20, HeaderStyle = style });
            VorigeMatches.Columns.Add(new DataGridTextColumn() { Header = "Zwart", Binding = new Binding("Zwart.Naam"), IsReadOnly = true, FontSize = 20, HeaderStyle = style });
            VorigeMatches.Columns.Add(new DataGridTextColumn() { Header = "Uitslag", Binding = new Binding("Uitslag"), IsReadOnly = true, FontSize = 20, HeaderStyle = style });

            bool allePersonenBestaan = true;
            foreach (Versus versus in huidigeMatchLijst.versuses)
            {
                if (versus.Wit == null || versus.Zwart == null)
                {
                    allePersonenBestaan = false;
                }
            }

            if (!allePersonenBestaan)
            {
                if (MessageBox.Show("Een aantal matches bevatten personen die verwijderd zijn, wilt u de matches opnieuw aanmaken?", "personen missen in matches", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    matchLijsten.Remove(huidigeMatchLijst);
                    MatchLijst.GenereerMatchLijst();
                    huidigeMatchLijst = matchLijsten.Find(matchlijst => matchlijst.huidigeLijst);
                } else
                {
                    MessageBox.Show("Verander de matches zodat elke match een witte en zwarte speler heeft", "update matches");
                }
            }
            VulDataGrid();
            HuidigeMatches.MouseDoubleClick += DubbelKlik;
        }

        private void Matchmaking_Click(object sender, RoutedEventArgs e)
        {
            bool elkeMatchGespeeld = true;
            foreach (Versus versus in huidigeMatchLijst.versuses)
            {
                if (versus.Uitslag == null)
                    elkeMatchGespeeld = false;
            }
            if (elkeMatchGespeeld)
                if (MessageBox.Show("Weet u zeker dat u deze matches wilt afsluiten?", "Matchmaking afsluiten", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    bool allePersonenBestaan = true;
                    foreach (Versus versus in huidigeMatchLijst.versuses)
                    {
                        if (versus.Wit == null || versus.Zwart == null)
                        {
                            allePersonenBestaan = false;
                        }
                    }
                    if (!allePersonenBestaan)
                    {
                        MessageBox.Show("Verander de matches zodat elke match een witte en zwarte speler heeft", "update matches");
                        return;
                    }
                    huidigeMatchLijst.huidigeLijst = false;

                    foreach (Versus versus in huidigeMatchLijst.versuses)
                    {
                        Persoon WitPersoon = versus.Wit;
                        Persoon ZwartPersoon = versus.Zwart;

                        int OudeWitPositie = WitPersoon.Positie;
                        int OudeZwartPositie = ZwartPersoon.Positie;

                        WitPersoon.OudePositie = OudeWitPositie;
                        ZwartPersoon.OudePositie = OudeZwartPositie;
                    }
                    foreach (Versus versus in huidigeMatchLijst.versuses)
                    {
                        Persoon WitPersoon = versus.Wit;
                        Persoon ZwartPersoon = versus.Zwart;
                        double ScoreWit = versus.Uitslag.Equals("0-1") ? 0.0 : versus.Uitslag.Equals("1-0") ? 1.0 : 0.5;
                        double ScoreZwart = ScoreWit == 0.0 ? 1.0 : ScoreWit == 1.0 ? 0.0 : 0.5;

                        //update glicko rating
                        Glicko2.GlickoOpponent WitTegenstander = new Glicko2.GlickoOpponent(versus.Wit.glicko.GetOriginalGlickoPlayer(), ScoreZwart);
                        Glicko2.GlickoOpponent ZwartTegenstander = new Glicko2.GlickoOpponent(versus.Zwart.glicko.GetOriginalGlickoPlayer(), ScoreWit);

                        Glicko2.GlickoPlayer WitOrigineleSpeler = Glicko2.GlickoCalculator.CalculateRanking(versus.Wit.glicko.GetOriginalGlickoPlayer(), new List<Glicko2.GlickoOpponent>() { ZwartTegenstander });
                        Glicko2.GlickoPlayer ZwartOrigineleSpeler = Glicko2.GlickoCalculator.CalculateRanking(versus.Zwart.glicko.GetOriginalGlickoPlayer(), new List<Glicko2.GlickoOpponent>() { WitTegenstander });

                        WitPersoon.glicko.Rating = WitOrigineleSpeler.Rating;
                        WitPersoon.glicko.RatingDeviation = WitOrigineleSpeler.RatingDeviation;
                        WitPersoon.glicko.Volatility = WitOrigineleSpeler.Volatility;

                        ZwartPersoon.glicko.Rating = ZwartOrigineleSpeler.Rating;
                        ZwartPersoon.glicko.RatingDeviation = ZwartOrigineleSpeler.RatingDeviation;
                        ZwartPersoon.glicko.Volatility = ZwartOrigineleSpeler.Volatility;

                        //update gespeeld, gewonnen, verloren, gelijkspel, score en stijging
                        WitPersoon.Gespeeld += 1;
                        WitPersoon.Gewonnen += ScoreWit == 1 ? 1 : 0;
                        WitPersoon.Verloren += ScoreWit == 0 ? 1 : 0;
                        WitPersoon.Gelijkspel += ScoreWit == 0.5 ? 1 : 0;
                        WitPersoon.Score += ScoreWit == 1 ? 1 : ScoreWit == 0 ? -1 : 0;
                        WitPersoon.Stijging = WitPersoon.OudePositie - DataSources.Instance.personen.Find(persoon => persoon.Id == WitPersoon.Id).Positie;
                        if (WitPersoon.vorigeTegenstanders.Count > VorigeTegenstanderCount)
                        {
                            WitPersoon.vorigeTegenstanders.RemoveAt(VorigeTegenstanderCount - 1);
                        }
                        WitPersoon.vorigeTegenstanders.Add(ZwartPersoon);

                        ZwartPersoon.Gespeeld += 1;
                        ZwartPersoon.Gewonnen += ScoreZwart == 1 ? 1 : 0;
                        ZwartPersoon.Verloren += ScoreZwart == 0 ? 1 : 0;
                        ZwartPersoon.Gelijkspel += ScoreZwart == 0.5 ? 1 : 0;
                        ZwartPersoon.Score += ScoreZwart == 1 ? 1 : ScoreZwart == 0 ? -1 : 0;
                        ZwartPersoon.Stijging = ZwartPersoon.OudePositie - DataSources.Instance.personen.Find(persoon => persoon.Id == ZwartPersoon.Id).Positie;
                        if (ZwartPersoon.vorigeTegenstanders.Count > VorigeTegenstanderCount)
                        {
                            ZwartPersoon.vorigeTegenstanders.RemoveAt(VorigeTegenstanderCount - 1);
                        }
                        ZwartPersoon.vorigeTegenstanders.Add(WitPersoon);
                    }

                    //update aanwezigheid
                    foreach (Persoon persoon in DataSources.Instance.personen)
                        persoon.Aanwezig = true;

                    Close();
                }
                else { }
            else
                Close();
        }

        public void VulDataGrid()
        {
            HuidigeMatches.Items.Clear();
            VorigeMatches.Items.Clear();
            foreach (Versus v in huidigeMatchLijst.versuses)
            {
                HuidigeMatches.Items.Add(v);
            }
            foreach (Versus v in vorigeMatchLijst.versuses)
            {
                VorigeMatches.Items.Add(v);
            }
        }

        private void Print_Click(object sender, RoutedEventArgs e)
        {
            List<List<string>> items = new List<List<string>>();
            foreach (Versus versus in huidigeMatchLijst.versuses)
            {
                items.Add(new List<string> { versus.Wit == null ? "" : versus.Wit.Naam, versus.Zwart == null ? "" : versus.Zwart.Naam, versus.Uitslag == null ? "-" : versus.Uitslag });
            }
            new PrintDG().printDG("Matches: " + DateTime.Now.ToString("dd/MM/yyyy"), new List<string> { "Wit", "Zwart", "Uitslag" }, items);
        }

        private void Match_Toevoegen(object sender, RoutedEventArgs e)
        {
            Versus geselecteerdeVersus = new Versus();
            int id = 0;
            foreach (Versus versus in huidigeMatchLijst.versuses)
            {
                if (id <= versus.Id)
                    id = versus.Id + 1;
            }
            geselecteerdeVersus.Id = id;
            huidigeMatchLijst.versuses.Add(geselecteerdeVersus);
            new VersusAanpassen(geselecteerdeVersus, true).ShowDialog();

            VulDataGrid();
        }

        private void Match_Verwijderen(object sender, RoutedEventArgs e)
        {
            if (HuidigeMatches.SelectedItem == null)
                return;
            if (MessageBox.Show("Weet u zeker dat u de match " + HuidigeMatches.SelectedItem.ToString() + " wilt verwijderen?", "Match verwijderen", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                huidigeMatchLijst.versuses.Remove((Versus)HuidigeMatches.SelectedItem);
                VulDataGrid();
            }
        }

        private void DubbelKlik(object sender, MouseButtonEventArgs e)
        {
            DataGrid grid = (DataGrid)sender;

            if (HuidigeMatches.SelectedCells.Count <= 0)
                return;
            Versus geselecteerdeVersus = ((Versus)HuidigeMatches.SelectedCells[0].Item);

            new VersusAanpassen(geselecteerdeVersus).ShowDialog();

            VulDataGrid();
        }

        private void Delete_Click(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                Match_Verwijderen(sender, e);
            }
        }
    }
}
