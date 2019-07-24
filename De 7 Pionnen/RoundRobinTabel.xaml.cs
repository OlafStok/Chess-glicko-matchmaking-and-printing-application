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
    public partial class RoundRobinTabel : Window
    {
        List<Persoon> aanwezigePersonen = new List<Persoon>();
        public RoundRobinTabel()
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

                Border NaamBoven = new Border()
                {
                    Background = Brushes.Black,
                    Child = new Viewbox
                    {
                        StretchDirection = StretchDirection.Both,
                        Child = new TextBlock
                        {
                            Text = aanwezigePersonen[i].Naam,
                            FontSize = 20,
                            Foreground = Brushes.White
                        }
                    }
                };
                Border NaamLinks = new Border()
                {
                    Background = Brushes.White,
                    Child = new Viewbox
                    {
                        StretchDirection = StretchDirection.Both,
                        Child = new TextBlock
                        {
                            Text = aanwezigePersonen[i].Naam,
                            FontSize = 20
                        }
                    }
                };

                Border Score = new Border()
                {
                    Child = new Viewbox
                    {
                        StretchDirection = StretchDirection.Both,
                        Child = new TextBlock
                        {
                            Text = "X",
                            FontSize = 20
                        }
                    }
                };

                Grid.SetColumn(Score, i + 1);
                Grid.SetRow(Score, i + 1);

                Grid.SetColumn(NaamBoven, i + 1);
                Grid.SetRow(NaamBoven, 0);

                Grid.SetColumn(NaamLinks, 0);
                Grid.SetRow(NaamLinks, i + 1);

                Grid.Children.Add(Score);
                Grid.Children.Add(NaamBoven);
                Grid.Children.Add(NaamLinks);
            }
        }
    }
}
