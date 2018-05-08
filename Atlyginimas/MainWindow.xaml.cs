using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Text.RegularExpressions;

namespace Atlyginimas
{
    public partial class MainWindow : Window
    {
        //statinis sąrašas procentams saugoti
        static List<float> procentai = new List<float>();

        public MainWindow()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Button_Click_(0-2) - šone esantys skirtukai
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            ResultGrid_2.Visibility = Visibility.Collapsed;
            ResultGrid_1.Visibility = Visibility.Collapsed;
            Settings.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Tikrina, ar į laukelius yra įvedami tinkami simboliai (0-9 ir . )
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        //Išsaugo procentus į sąrašą
        private void Set_Percentages()
        {
            procentai = new List<float>();
            float percent = 0;
            IEnumerable<TextBox> collection = Settings.Children.OfType<TextBox>();
            foreach (var set in collection)
            {
                bool success = float.TryParse(Regex.Replace(set.Text, @"\s+", ""), out percent);
                if (success)
                {
                    procentai.Add(percent);
                }
                else
                {
                    if (Regex.Replace(set.Text, @"\s+", "") == "")
                        set.Text = "Nurodykite procentus";
                    else
                        set.Text = "Netinkamas formatas";
                }
            }
        }
        /// <summary>
        /// Skaičiuoja punkto "į rankas" rezultatus
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            if (procentai.Count == 6)
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
                            if (result != last) //tikrina, ar tai paskutinis rezultatų laukas
                            {
                                result.Text = Math.Round((sum * procentai[i] / 100), 2).ToString() + " €"; //formulė: suma * procentai / 100. Apvalinama iki šimtųjų
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
                                result.Text = Math.Round((sum * procentai[i] / 100), 2).ToString() + " €";
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
        // procentų suma. a_s - autorinė sutartis?
        private float Count_Percentages(bool a_s)
        {
            float total = 0;
            if (a_s)
            {
                for (int i = 1; i < 6; i += 2)
                {
                    total += procentai[i];
                }
            }
            else
            {
                for (int i = 0; i < 6; i += 2)
                {
                    total += procentai[i];
                }
            }

            return (total);
        }
        /// <summary>
        /// apskaičiuoja pradinę sumą iš sumos, gautos į rankas.
        /// </summary>
        /// <param name="sum"> - suma </param>
        /// <param name="a_s"> - autorinė sutartis? </param>
        /// <returns></returns>
        private float Count_Result(float sum, bool a_s)
        {
            //ant popieriaus = pradine suma * 100 / (100 - procentu suma)
            return (sum * 100 / (100 - Count_Percentages(a_s)));
        }
        /// <summary>
        /// Skaičiuoja "ant popieriaus" punkto rezultatus
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            if (procentai.Count == 6)
            {
                IEnumerable<TextBlock> collection = ResultGrid_2.Children.OfType<TextBlock>();
                TextBlock last = collection.Last();
                float sodros_suma = 0;
                float autor_suma = 0;
                double bendra_suma = 0;
                bool success1 = float.TryParse(Regex.Replace(input_2.Text, @"\s+", ""), out sodros_suma);
                bool success2 = float.TryParse(Regex.Replace(input_3.Text, @"\s+", ""), out autor_suma);
                float sum = 0;
                //ar įvesta SODROS suma
                if (success1)
                {
                    sum = Count_Result(sodros_suma, false);
                    //ar įvesta ir autorinių sutarčių suma
                    if (success2)
                    {
                        int i = 0;
                        int j = 1;

                        sum += Count_Result(autor_suma, true);

                        foreach (var result in collection)
                        {
                            if (result != last)
                            {
                                bendra_suma = Math.Round((sum * procentai[i] / 100), 2);
                                bendra_suma += Math.Round((sum * procentai[j] / 100), 2);
                                result.Text = bendra_suma.ToString() + " €";
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
                        foreach (var result in collection)
                        {
                            if (result != last)
                            {
                                result.Text = Math.Round((sum * procentai[i] / 100), 2).ToString() + " €";
                                i += 2;
                            }
                            else
                            {
                                result.Text = Math.Round(sum, 2).ToString() + " €";
                            }
                        }
                    }
                }
                //ar įvesta tik autorinių sutarčių suma
                else if (success2)
                {
                    int i = 1;
                    sum = Count_Result(autor_suma, true);

                    foreach (var result in collection)
                    {
                        if (result != last)
                        {
                            result.Text = Math.Round((sum * procentai[i] / 100), 2).ToString() + " €";
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
