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
        }

        private void Toevoegen_Click(object sender, RoutedEventArgs e)
        {
            DataSources.Instance.personen.Add(new Persoon(DataSources.Instance.GenereerId(), naam.Text));
            Close();
        }

        private void Annuleren_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
