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
    /// Interaction logic for GroteLadder.xaml
    /// </summary>
    public partial class GroteLadder : Window
    {
        List<Persoon> aanwezigePersonen = new List<Persoon>();
        public GroteLadder()
        {
            Style style = new Style();
            style.Setters.Add(new Setter(GridViewColumnHeader.FontSizeProperty, 20.0));
            InitializeComponent();
            foreach (Persoon p in DataSources.Instance.personen)
            {
                if (p.Aanwezig && p.Id > -1)
                    aanwezigePersonen.Add(p);
            }

            Grid.RowDefinitions.Add(new RowDefinition());
            Grid.ColumnDefinitions.Add(new ColumnDefinition());

            for (int i = 0; i < aanwezigePersonen.Count; i++)
            {
                Grid.RowDefinitions.Add(new RowDefinition());
                Grid.ColumnDefinitions.Add(new ColumnDefinition());

                Viewbox headerBox = new Viewbox
                {
                    StretchDirection = StretchDirection.Both,
                    Child = new TextBlock
                    {
                        Text = aanwezigePersonen[i].Naam,
                        FontSize = 20
                    }
            };

                Viewbox cellBox = new Viewbox
                {
                    StretchDirection = StretchDirection.Both,
                    Child = new TextBlock
                    {
                        Text = aanwezigePersonen[i].Naam,
                        FontSize = 20
                    }
                };

                Viewbox textBox = new Viewbox
                {
                    StretchDirection = StretchDirection.Both,
                    Child = new TextBlock
                    {
                        Text = "X",
                        FontSize = 20
                    }
                };

                Grid.SetColumn(textBox, i+1);
                Grid.SetRow(textBox, i+1);
                Grid.SetColumn(headerBox, i+1);
                Grid.SetRow(headerBox, 0);
                Grid.SetColumn(cellBox, 0);
                Grid.SetRow(cellBox, i+1);
                Grid.Children.Add(textBox);
                Grid.Children.Add(headerBox);
                Grid.Children.Add(cellBox);
            }
        }
    }
}
