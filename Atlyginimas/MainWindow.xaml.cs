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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;

namespace Atlyginimas
{
    public partial class MainWindow : Window
    {
        static List<float> percentages = new List<float>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ResultGrid_1.Visibility = Visibility.Visible;
            ResultGrid_2.Visibility = Visibility.Collapsed;
            Settings.Visibility = Visibility.Collapsed;
            Set_Percentages();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ResultGrid_2.Visibility = Visibility.Visible;
            ResultGrid_1.Visibility = Visibility.Collapsed;
            Settings.Visibility = Visibility.Collapsed;
            Set_Percentages();
        }
        /// <summary>
        /// Procentų nustatymui
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            ResultGrid_2.Visibility = Visibility.Collapsed;
            ResultGrid_1.Visibility = Visibility.Collapsed;
            Settings.Visibility = Visibility.Visible;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            mok_1.Text = "haha";
        }

        private void Set_Percentages()
        {
            percentages = new List<float>();
            float percent = 0;
            IEnumerable<TextBox> collection = Settings.Children.OfType<TextBox>();
            foreach (var set in collection)
            {
                bool success = float.TryParse(Regex.Replace(set.Text, @"\s+", ""), out percent);
                if (success)
                {
                    percentages.Add(percent);
                }
                else
                {
                    set.Text = "Netinkamas formatas";
                }
            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            if (percentages.Count == 6)
            {
                IEnumerable<TextBlock> collection = ResultGrid_1.Children.OfType<TextBlock>();
                TextBlock last = collection.Last();
                float sum = 0;
                bool success = float.TryParse(Regex.Replace(input_1.Text, @"\s+", ""), out sum);
                if (success)
                {
                    if (autorine.IsChecked == true)
                    {
                        int i = 1;
                        foreach (var result in collection)
                        {
                            if (result != last)
                            {
                                result.Text = Math.Round((sum * percentages[i] / 100), 2).ToString() + " €";
                                i += 2;
                            }
                            else
                            {
                                result.Text = Math.Round(sum - (sum * Count_Percentages(true)) / 100, 2).ToString() + " €";
                            }
                        }
                    }
                    else
                    {
                        int i = 0;
                        foreach (var result in collection)
                        {
                            if (result != last)
                            {
                                result.Text = Math.Round((sum * percentages[i] / 100), 2).ToString() + " €";
                                i += 2;
                            }
                            else
                            {
                                result.Text = Math.Round(sum - (sum * Count_Percentages(false)) / 100, 2).ToString() + " €";
                            }
                        }
                    }
                }
            }
            else
            {
                ResultGrid_2.Visibility = Visibility.Collapsed;
                ResultGrid_1.Visibility = Visibility.Collapsed;
                Settings.Visibility = Visibility.Visible;
            }
        }

        private float Count_Percentages(bool a_s)
        {
            float total = 0;
            if (a_s)
            {
                for (int i = 1; i < 6; i += 2)
                {
                    total += percentages[i];
                }
            }
            else
            {
                for (int i = 0; i < 6; i += 2)
                {
                    total += percentages[i];
                }
            }

            return (total);
        }

        private float Count_Result(float sum, bool a_s)
        {
            //pradine suma * 100 / (100 - procentu suma)
            float result = sum * 100 / (100 - Count_Percentages(a_s));
            return result;
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            if (percentages.Count == 6)
            {
                IEnumerable<TextBlock> collection = ResultGrid_2.Children.OfType<TextBlock>();
                TextBlock last = collection.Last();
                float sum1 = 0;
                float sum2 = 0;
                double totalSum = 0;
                bool success1 = float.TryParse(Regex.Replace(input_2.Text, @"\s+", ""), out sum1);
                bool success2 = float.TryParse(Regex.Replace(input_3.Text, @"\s+", ""), out sum2);
                float sum = 0;

                if (success1)
                {
                    if (success2)
                    {
                        int i = 0;
                        int j = 1;

                        sum += Count_Result(sum2, true);
                        sum += Count_Result(sum1, false);

                        foreach (var result in collection)
                        {
                            if (result != last)
                            {
                                totalSum = Math.Round((sum * percentages[i] / 100), 2);
                                totalSum += Math.Round((sum * percentages[j] / 100), 2);
                                result.Text = totalSum.ToString() + " €";
                                i += 2;
                                j += 2;
                            }
                            else
                            {
                                result.Text = Math.Round(sum, 2).ToString() + " €";
                            }
                        }
                    }
                    else
                    {
                        int i = 0;
                        sum = Count_Result(sum1, false);
                        foreach (var result in collection)
                        {
                            if (result != last)
                            {
                                result.Text = Math.Round((sum * percentages[i] / 100), 2).ToString() + " €";
                                i += 2;
                            }
                            else
                            {
                                result.Text = Math.Round(sum, 2).ToString() + " €";
                            }
                        }
                    }
                }

                else if (success1)
                {
                    int i = 1;
                    sum = Count_Result(sum2, true);

                    foreach (var result in collection)
                    {
                        if (result != last)
                        {
                            result.Text = Math.Round((sum * percentages[i] / 100), 2).ToString() + " €";
                            i += 2;
                        }
                        else
                        {
                            result.Text = Math.Round(sum, 2).ToString() + " €";
                        }
                    }
                }
            }

            else
            {
                ResultGrid_2.Visibility = Visibility.Collapsed;
                ResultGrid_1.Visibility = Visibility.Collapsed;
                Settings.Visibility = Visibility.Visible;
            }
        }
    }
}
