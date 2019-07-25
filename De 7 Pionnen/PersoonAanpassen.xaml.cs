using System;
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
    /// Interaction logic for PersoonAanpassen.xaml
    /// </summary>
    public partial class PersoonAanpassen : Window
    {
        public PersoonAanpassen(Persoon persoon)
        {
            InitializeComponent();

            //vul textboxes met gegevens
            Naam.Text = persoon.Naam;
            Gespeeld.Text = persoon.Gespeeld.ToString();
            Gewonnen.Text = persoon.Gewonnen.ToString();
            Verloren.Text = persoon.Verloren.ToString();
            Gelijkspel.Text = persoon.Gelijkspel.ToString();
            Rating.Text = persoon.glicko.Rating.ToString();
            Score.Text = persoon.Score.ToString();
            Id.Text = persoon.Id.ToString();

            //format text zodat floats klein lijken
            Rating.Text = string.Format("{0:##########0.#}", float.Parse(Rating.Text));
            Score.Text = string.Format("{0:##########0.#}", float.Parse(Score.Text));

            //check voor nummers
            Gespeeld.PreviewTextInput += IsInt;
            Gewonnen.PreviewTextInput += IsInt;
            Verloren.PreviewTextInput += IsInt;
            Gelijkspel.PreviewTextInput += IsInt;

            Rating.PreviewTextInput += IsFloat;
            Score.PreviewTextInput += IsFloat;
        }

        private void Annuleren_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void Toepassen_Click(object sender, RoutedEventArgs e)
        {
            Persoon geUpdatePersoon = DataSources.Instance.personen.Find(persoonInLijst => persoonInLijst.Id.ToString().Equals(Id.Text));
            geUpdatePersoon.Naam = Naam.Text;
            geUpdatePersoon.Gespeeld = int.Parse(Gespeeld.Text);
            geUpdatePersoon.Gewonnen = int.Parse(Gewonnen.Text);
            geUpdatePersoon.Verloren = int.Parse(Verloren.Text);
            geUpdatePersoon.Gelijkspel = int.Parse(Gelijkspel.Text);
            geUpdatePersoon.glicko.Rating = float.Parse(Rating.Text.Replace('.', ','));
            geUpdatePersoon.Score = double.Parse(Score.Text.Replace('.', ','));
            

            Close();
        }

        private void IsInt(object sender, TextCompositionEventArgs e)
        {
            var chararr = e.Text.ToCharArray();
            char c = chararr[0];
            if (char.IsDigit(c) || c == '-')
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void IsFloat(object sender, TextCompositionEventArgs e)
        {
            var chararr = e.Text.ToCharArray();
            char c = chararr[0];
            if (char.IsDigit(c) || c == '-' || c == '.' || c == ',')
            {
                if (c == '.' || c == ',')
                {
                    if (((TextBox)sender).Text.Contains(".") || ((TextBox)sender).Text.Contains(",")) {
                        e.Handled = true;
                        return;
                    }
                }
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }
    }
}
