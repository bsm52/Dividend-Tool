using System;
using System.Windows;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;

namespace dividend_tool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            string input = DividendInfoTextBox.Text;

            Console.WriteLine(input);
            ProcessInput(input);
        }

        private async void ProcessInput(string input)
        {
            StockProvider stockProvider = new StockProvider();

            //This list contains all of the stocks given, with 
            //the amounts of stocks for each one
            List<KeyValuePair<string, int>> stocks = new List<KeyValuePair<string, int>>();

            using (StringReader reader = new StringReader(input))
            {
                string line = string.Empty, symbol = "";
                //this will help count the 
                int counter = 0, amount;
                do
                {
                    line = reader.ReadLine();
                    if (line != null)
                    {
                        if (line == "" || line == "Holding" || line == "Shares")
                        {
                            //do nothing, we don't want the information from these lines
                        }
                        else if (counter == 0) // then it's stock symbol
                        {
                            symbol = line;
                            counter++;
                        }
                        else if (counter == 1)
                        {
                            //skip since it's the name of the company
                            counter++;
                        }
                        else //it's the amount of the stock
                        {
                            amount = Int32.Parse(line);

                            stocks.Add(new KeyValuePair<string, int>(symbol, amount));
                            counter = 0;
                        }


                        Debug.WriteLine(line);
                        Debug.WriteLine(" ");
                    }

                } while (line != null);
            }


            List<KeyValuePair<string, double>> prices = await stockProvider.GetStockPrices(stocks);


            GetTotalValue(stocks, prices);
            

        }

        private void GetTotalValue(List<KeyValuePair<string, int>> stocksAndAmount, 
            List<KeyValuePair<string, double>> stocksAndPrice)
        {
            double totalValue = 0;
            
            try
            {
                //doesn't matter which we use, since they will have the 
                //same amount of values per list
                for (int i = 0; i < stocksAndAmount.Count; i++)
                {
                    if(stocksAndPrice[i].Key != stocksAndAmount[i].Key)
                    {
                        throw new System.InvalidOperationException("Symbols don't match");
                    }
                    totalValue += stocksAndAmount[i].Value * stocksAndPrice[i].Value;
                }

                TotalValueTextBlock.Text = totalValue.ToString();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            
        }
    }
}
