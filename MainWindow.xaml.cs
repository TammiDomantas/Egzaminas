using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
namespace Egzaminas
{
    public partial class MainWindow : Window
    {
        public int threads = 3;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Encrypt_Click(object sender, RoutedEventArgs e)
        {
            Uzkodavimas Slaptazodis = new Uzkodavimas();
            string text = PasswordBox.Text;
            if(text.Length == 0)
            {
                MessageBox.Show("Pirma iveskite slaptazodi");
                return;
            }
            if (text.Length > 4)
            {
                MessageBox.Show("Max simboliu kiekis: 4. Jus ivedete " + text.Length);
                return;
            }
            if(text.Any(ch => !char.IsLetterOrDigit(ch))){
                MessageBox.Show("Negalima specialiu simboliu. Tik raides ir skaiciai!");
                return;
            }
            Slaptazodis.Slapt = text;
            Slaptazodis.Hash = Slaptazodis.HashPassword(Slaptazodis.Slapt);
            Slaptazodis.Isvedimas();
            MessageBox.Show("Slaptazodis uzkoduotas!");
        }
        private void button_1t_Checked(object sender, RoutedEventArgs e)
        {
            threads = 1;
        }

        private void button_2t_Checked(object sender, RoutedEventArgs e)
        {
            threads = 2;
        }

        private void button_3t_Checked(object sender, RoutedEventArgs e)
        {
            threads = 3;
        }
        private void Decrypt_Click(object sender, RoutedEventArgs e)
        {
            Atkodavimas Atk = new Atkodavimas();
            Atk.threadcount = threads;
            Atk.Main();
        }
    }
}