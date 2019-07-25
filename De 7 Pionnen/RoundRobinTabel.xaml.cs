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
                if (i == 0)
                {
                    Grid.ColumnDefinitions.Add(new ColumnDefinition());
                    Grid.ColumnDefinitions.Add(new ColumnDefinition());
                    Grid.ColumnDefinitions.Add(new ColumnDefinition());
                }

                Border NaamBoven = new Border()
                {
                    BorderBrush = Brushes.Black,
                    BorderThickness = new Thickness(1, 1, 1, 1),
                    Background = Brushes.LightGray,
                    Child = new Viewbox
                    {
                        StretchDirection = StretchDirection.Both,
                        Child = new TextBlock
                        {
                            Text = (i + 1).ToString(),
                            FontSize = 20,
                            Foreground = Brushes.Black
                        }
                    }
                };
                Border NaamLinks = new Border()
                {
                    BorderBrush = Brushes.Black,
                    BorderThickness = new Thickness(1, 1, 1, 1),
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

                Border NummerLinks = new Border()
                {
                    BorderBrush = Brushes.Black,
                    BorderThickness = new Thickness(1, 1, 1, 1),
                    Background = Brushes.White,
                    Child = new Viewbox
                    {
                        StretchDirection = StretchDirection.Both,
                        Child = new TextBlock
                        {
                            Text = (i+1).ToString(),
                            FontSize = 20
                        }
                    }
                };

                Border Leeg = new Border()
                {
                    BorderBrush = Brushes.Black,
                    BorderThickness = new Thickness(1, 1, 1, 1),
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

                Border Score = new Border()
                {
                    BorderBrush = Brushes.Black,
                    BorderThickness = new Thickness(1, 1, 1, 1),
                    Child = new Viewbox
                    {
                        StretchDirection = StretchDirection.Both,
                        Child = new TextBlock
                        {
                            Text = aanwezigePersonen[i].Score.ToString(),
                            FontSize = 20
                        }
                    }
                };

                Border Rating = new Border()
                {
                    BorderBrush = Brushes.Black,
                    BorderThickness = new Thickness(1, 1, 1, 1),
                    Child = new Viewbox
                    {
                        StretchDirection = StretchDirection.Both,
                        Child = new TextBlock
                        {
                            Text = string.Format("{0:##########0.#}", aanwezigePersonen[i].glicko.Rating),
                            FontSize = 20
                        }
                    }
                };

                Grid.SetColumn(Leeg, i + 4);
                Grid.SetRow(Leeg, i + 1);

                Grid.SetColumn(NaamBoven, i + 4);
                Grid.SetRow(NaamBoven, 0);

                Grid.SetColumn(NaamLinks, 1);
                Grid.SetRow(NaamLinks, i + 1);

                Grid.SetColumn(NummerLinks, 0);
                Grid.SetRow(NummerLinks, i + 1);

                Grid.SetColumn(Rating, 2);
                Grid.SetRow(Rating, i + 1);

                Grid.SetColumn(Score, 3);
                Grid.SetRow(Score, i + 1);

                Grid.Children.Add(Leeg);
                Grid.Children.Add(NaamBoven);
                Grid.Children.Add(NaamLinks);
                Grid.Children.Add(NummerLinks);
                Grid.Children.Add(Rating);
                Grid.Children.Add(Score);

                for (int j = 0; j < aanwezigePersonen.Count; j++)
                {
                    Border Cell = new Border()
                    {
                        BorderBrush = Brushes.Black,
                        BorderThickness = new Thickness(1, 1, 1, 1),
                        Child = new Viewbox
                        {
                            StretchDirection = StretchDirection.Both
                        }
                    };
                    Grid.SetColumn(Cell, j + 4);
                    Grid.SetRow(Cell, i+1);
                    Grid.Children.Add(Cell);
                }
            }
        }
    }
}
