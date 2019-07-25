using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;

namespace De_7_Pionnen
{
    public class PrintDG
    {
        public void PrintRoundRobinOfMatchLijst(string title, List<string> headerList, List<List<string>> cellList)
        {
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                FlowDocument fd = new FlowDocument();

                Paragraph p = new Paragraph(new Run(title));
                p.FontSize = 18;
                fd.Blocks.Add(p);

                Table table = new Table();
                TableRowGroup tableRowGroup = new TableRowGroup();
                TableRow r = new TableRow();
                fd.PageWidth = printDialog.PrintableAreaWidth;
                fd.PageHeight = printDialog.PrintableAreaHeight;
                fd.BringIntoView();

                fd.TextAlignment = TextAlignment.Center;
                fd.ColumnWidth = 500;
                table.CellSpacing = 0;


                for (int j = 0; j < headerList.Count; j++)
                {

                    r.Cells.Add(new TableCell(new Paragraph(new Run(headerList[j]))));
                    r.Cells[j].ColumnSpan = 4;
                    r.Cells[j].Padding = new Thickness(4);



                    r.Cells[j].BorderBrush = Brushes.Black;
                    r.Cells[j].FontWeight = FontWeights.Bold;
                    r.Cells[j].Background = Brushes.LightGray;
                    r.Cells[j].Foreground = Brushes.Black;
                    r.Cells[j].BorderThickness = new Thickness(1, 1, 1, 1);
                }
                tableRowGroup.Rows.Add(r);
                table.RowGroups.Add(tableRowGroup);
                for (int i = 0; i < cellList.Count; i++)
                {
                    TableRowGroup g = new TableRowGroup();
                    TableRow ro = new TableRow();
                    if (cellList[i][0].StartsWith("Ronde "))
                    {
                        Debug.WriteLine("dit is een ronde cell, voeg lege toe..");
                        foreach (var item in cellList[i]) {
                            ro.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                            Debug.WriteLine("lege cell toegevoegd!");
                        }
                        g.Rows.Add(ro);
                        table.RowGroups.Add(g);
                        g = new TableRowGroup();
                        ro = new TableRow();
                    }
                    for (int j = 0; j < cellList[i].Count; j++)
                    {
                        ro.Cells.Add(new TableCell(new Paragraph(new Run(cellList[i][j]))));
                    }

                    for (int k = 0; k < cellList[0].Count; k++)
                    {
                        ro.Cells[k].ColumnSpan = 4;
                        ro.Cells[k].Padding = new Thickness(4);



                        ro.Cells[k].BorderBrush = Brushes.Black;
                        ro.Cells[k].FontWeight = FontWeights.Bold;
                        ro.Cells[k].BorderThickness = new Thickness(1, 1, 1, 1);
                        Debug.WriteLine(cellList[i][k]);
                        if (cellList[i][0].StartsWith("Ronde "))
                        {

                            ro.Cells[k].Background = Brushes.AliceBlue;
                            ro.Cells[2].Foreground = Brushes.AliceBlue;
                        }
                    }


                    g.Rows.Add(ro);
                    table.RowGroups.Add(g);
                }
                fd.Blocks.Add(table);
                try
                {
                    printDialog.PrintDocument(((IDocumentPaginatorSource)fd).DocumentPaginator, "");
                } catch
                {
                    MessageBox.Show("Probleem met afdrukken, herstart het programma en probeer het nog eens!", "Probleem met afdrukken");
                }

            }
        }

    }
}
