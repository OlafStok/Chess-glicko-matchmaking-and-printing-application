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
    /// Interaction logic for PersoonToevoegen.xaml
    /// </summary>
    public partial class PersoonToevoegen : Window
    {
        public PersoonToevoegen()
        {
            InitializeComponent();
            naam.Focus();
            rating.PreviewTextInput += IsFloat;
            rating.Text = 1500.ToString();
        }

        private void Toevoegen_Click(object sender, RoutedEventArgs e)
        {
            DataSources.Instance.personen.Add(new Persoon(DataSources.Instance.GenereerId(), naam.Text) {glicko = new WindowsFormsApp2.GlickoPlayer(float.Parse(rating.Text.Replace('.', ','))) });
            Close();
        }

        private void Annuleren_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void IsFloat(object sender, TextCompositionEventArgs e)
        {
            var chararr = e.Text.ToCharArray();
            char c = chararr[0];
            if (char.IsDigit(c) || c == '-' || c == '.' || c == ',')
            {
                if (c == '.' || c == ',')
                {
                    if (((TextBox)sender).Text.Contains(".") || ((TextBox)sender).Text.Contains(","))
                    {
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
