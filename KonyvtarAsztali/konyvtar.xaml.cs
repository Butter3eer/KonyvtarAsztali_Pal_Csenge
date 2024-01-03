using System;
using System.Collections.Generic;
using System.Data;
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

namespace KonyvtarAsztali
{
    /// <summary>
    /// Interaction logic for konyvtar.xaml
    /// </summary>
    public partial class konyvtar : Window
    {
        Statisztika stat = new Statisztika();

        public konyvtar()
        {
            InitializeComponent();

            if (stat.Beolvas("vizsga-2022-14s-wip-db"))
            {       
                dgKonyvtar.ItemsSource = stat.Konyvek;
            }
            else
            {
                MessageBox.Show("A csatlakozás vagy beolvasás közben hiba történt", "", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (dgKonyvtar.SelectedItems.Count > 0)
            {
                if (MessageBox.Show("Biztos szeretné törölni a kiválasztott könyvet?", "", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        stat.AdatbazisbolTorles((dgKonyvtar.SelectedItems[0] as Konyv).Id);
                        MessageBox.Show("A törlés sikeres volt.", "", MessageBoxButton.OK);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("A törlés sikertelen volt.", ex.Message, MessageBoxButton.OK);
                    }
                }
            }
            else
            {
                MessageBox.Show("Törléshez előbb válasszon ki könyvet.", "", MessageBoxButton.OK);
            }
            dgKonyvtar.Items.Refresh();
            dgKonyvtar.ItemsSource = stat.Konyvek;
        }
    }
}
