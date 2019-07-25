using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaction logic for VersusAanpassen.xaml
    /// </summary>
    public partial class VersusAanpassen : Window
    {
        public bool toevoegen;
        public List<Versus> RoundRobinLijst = null;
        public Versus nieuweVersus = new Versus();
        public VersusAanpassen(Versus versus, bool toevoegen = false)
        {
            this.toevoegen = toevoegen;
            List<Uitslag> uitslagen = new List<Uitslag>();
            uitslagen.Add(new Uitslag("Wit wint", "1-0"));
            uitslagen.Add(new Uitslag("Zwart wint", "0-1"));
            uitslagen.Add(new Uitslag("Gelijk", "½-½"));

            InitializeComponent();

            Wit.DisplayMemberPath = "Naam";
            Wit.SelectedValuePath = "Id";
            Zwart.DisplayMemberPath = "Naam";
            Zwart.SelectedValuePath = "Id";
            Uitslag.DisplayMemberPath = "Naam";
            Uitslag.SelectedValuePath = "Waarde";

            List<Persoon> persoonItemSource = new List<Persoon>();
            foreach (Persoon p in DataSources.Instance.personen)
            {
                if (p.Id > -1)
                    persoonItemSource.Add(p);
                Debug.WriteLine(p.Id.ToString() + " - " + p.Naam);
            }

            Wit.ItemsSource = persoonItemSource;
            Zwart.ItemsSource = persoonItemSource;
            Uitslag.ItemsSource = uitslagen;

            Id.Text = versus.Id.ToString();
            try
            {
                Wit.SelectedValue = versus.Wit.Id;
            }
            catch { }
            try
            {
                Zwart.SelectedValue = versus.Zwart.Id;
            }
            catch { }
            try
            {
                Uitslag.SelectedValue = versus.Uitslag;
            }
            catch { }


            Wit.SelectionChanged += PersoonVeranderd;
            Zwart.SelectionChanged += PersoonVeranderd;
        }

        public VersusAanpassen(Versus versus, ref List<Versus> roundRobinLijst, bool toevoegen = false)
        {
            RoundRobinLijst = roundRobinLijst;
            this.toevoegen = toevoegen;
            nieuweVersus = versus;
            List<Uitslag> uitslagen = new List<Uitslag>();
            uitslagen.Add(new Uitslag("Wit wint", "1-0"));
            uitslagen.Add(new Uitslag("Zwart wint", "0-1"));
            uitslagen.Add(new Uitslag("Gelijk", "½-½"));

            InitializeComponent();

            Wit.DisplayMemberPath = "Naam";
            Wit.SelectedValuePath = "Id";
            Zwart.DisplayMemberPath = "Naam";
            Zwart.SelectedValuePath = "Id";
            Uitslag.DisplayMemberPath = "Naam";
            Uitslag.SelectedValuePath = "Waarde";

            List<Persoon> persoonItemSource = new List<Persoon>();
            foreach (Persoon p in DataSources.Instance.personen)
            {
                if (p.Id > -1)
                    persoonItemSource.Add(p);
                Debug.WriteLine(p.Id.ToString() + " - " + p.Naam);
            }

            Wit.ItemsSource = persoonItemSource;
            Zwart.ItemsSource = persoonItemSource;
            Uitslag.ItemsSource = uitslagen;

            Id.Text = versus.Id.ToString();
            try
            {
                Wit.SelectedValue = versus.Wit.Id;
            }
            catch { }
            try
            {
                Zwart.SelectedValue = versus.Zwart.Id;
            }
            catch { }
            try
            {
                Uitslag.SelectedValue = versus.Uitslag;
            }
            catch { }


            Wit.SelectionChanged += PersoonVeranderd;
            Zwart.SelectionChanged += PersoonVeranderd;
        }

        private void PersoonVeranderd(object sender, SelectionChangedEventArgs e)
        {
            DependencyObject combo = sender as DependencyObject;
            ComboBox comboBox = sender as ComboBox;
            string naam = combo.GetValue(FrameworkElement.NameProperty) as string;

            if (Zwart.SelectedValue == null || Zwart.SelectedValue.Equals(Wit.SelectedValue))
            {
                if (naam.Equals("Wit"))
                {
                    if (e.RemovedItems.Count > 0)
                        Zwart.SelectedValue = ((Persoon)e.RemovedItems[0]).Id;
                    else
                        Zwart.SelectedValue = null;
                }
                else if (naam.Equals("Zwart"))
                {
                    if (e.RemovedItems.Count > 0)
                        Wit.SelectedValue = ((Persoon)e.RemovedItems[0]).Id;
                    else
                        Wit.SelectedValue = null;
                }
            }
        }

        private void Toepassen_Click(object sender, RoutedEventArgs e)
        {
            if (RoundRobinLijst == null)
            {
                MatchLijst huidigeMatchLijst = DataSources.Instance.matchLijsten.Find(matchlijst => matchlijst.huidigeLijst);
                Versus huidigeVersus = huidigeMatchLijst.versuses.Find(versus => versus.Id == int.Parse(Id.Text));
                if (Wit.SelectedValue != null)
                    huidigeVersus.Wit = DataSources.Instance.personen.Find(p => p.Id == (int)Wit.SelectedValue);
                else
                    huidigeVersus.Wit = null;

                if (Zwart.SelectedValue != null)
                    huidigeVersus.Zwart = DataSources.Instance.personen.Find(p => p.Id == (int)Zwart.SelectedValue);
                else
                    huidigeVersus.Zwart = null;

                if (Uitslag.SelectedValue != null)
                    huidigeVersus.Uitslag = Uitslag.SelectedValue.ToString();

                Close();
            } else
            {
                Versus huidigeVersus = RoundRobinLijst.Find(versus => versus.Id == int.Parse(Id.Text));
                if (huidigeVersus == null)
                    Close();

                if (toevoegen)
                    huidigeVersus = nieuweVersus;
                if (Wit.SelectedValue != null)
                    huidigeVersus.Wit = DataSources.Instance.personen.Find(p => p.Id == (int)Wit.SelectedValue);
                else
                    huidigeVersus.Wit = null;

                if (Zwart.SelectedValue != null)
                    huidigeVersus.Zwart = DataSources.Instance.personen.Find(p => p.Id == (int)Zwart.SelectedValue);
                else
                    huidigeVersus.Zwart = null;

                if (Uitslag.SelectedValue != null)
                    huidigeVersus.Uitslag = Uitslag.SelectedValue.ToString();

                if (toevoegen)
                    RoundRobinLijst.Add(huidigeVersus);

                Close();
            }
        }

        private void Annuleren_Click(object sender, RoutedEventArgs e)
        {
            if (toevoegen)
            {
                MatchLijst huidigeMatchLijst = DataSources.Instance.matchLijsten.Find(matchlijst => matchlijst.huidigeLijst);
                Versus huidigeVersus = huidigeMatchLijst.versuses.Find(versus => versus.Id == int.Parse(Id.Text));

                huidigeMatchLijst.versuses.Remove(huidigeVersus);
            }
                
            Close();
        }
    }
}
