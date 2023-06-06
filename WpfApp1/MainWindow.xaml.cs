using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        char[,] matrix = new char[4, 3];                                                          //матрица с символами

        //индексы текущей кнопки
        int indI;                                                                                 
        int indJ;

        //индексы предыдущей кнопки
        int FirstI=-1;                                                                               
        int FirstJ=-1;

        //предыдущая кнопка
        Button FirstBut;

        int k = 0;
        int WinK = 0;

        DispatcherTimer timer = new DispatcherTimer();
        int t = 0;

        public MainWindow()
        {
            InitializeComponent();
            RandomnZnacki();
            timer.Tick += new EventHandler(timer_Click);
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();
        }
        private void timer_Click(object sender, EventArgs e)
        {
            t++;            
            Timer.Header = t.ToString();
        }


        public void RandomnZnacki()
        {
            List<char> znacki = new List<char>() { '!', '!', '@', '@', '*', '*', '+', '+', '=', '=', '-', '-' };
            List<int> index = new List<int>();
            Random random = new Random();
            for (int i = 0; i < 4; i++)
            {
                for(int j = 0; j < 3; j++)
                {
                    int ind = random.Next(0, znacki.Count);
                    matrix[i, j] = znacki[ind];
                    znacki.RemoveAt(ind);
                }
            }
        }


        private void Start_Click(object sender, RoutedEventArgs e)
        {
            NewGame();
        }
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            GiveTag(button);
            button.Content = matrix[indI, indJ];
            k++;
            Check(button);

        }

        public void GiveTag(Button button)
        {
            char[] a = Convert.ToString(button.Tag).ToCharArray();
            indI = int.Parse(a[0].ToString());
            indJ = int.Parse(a[1].ToString());
        }


        public  async void Check(Button button)
        {
            if (indI == FirstI && indJ == FirstJ)                                    //защита от повторных нажатий
                k--;
            if (k == 2)
            {
                if (matrix[indI, indJ] == matrix[FirstI, FirstJ])                   //сверяем символы
                {
                    await Task.Delay(1000);
                    //button.Visibility = Visibility.Collapsed;
                    //FirstBut.Visibility = Visibility.Collapsed;
                    button.IsEnabled = false;
                    FirstBut.IsEnabled = false;
                    WinK += 2;
                }
                else
                { 
                    await Task.Delay(1000);
                    button.Content = "";
                    FirstBut.Content = "";
                                        
                }
                k = 0;
            }
            else
            {
                FirstBut = button;
                FirstI = indI;
                FirstJ = indJ;
            }
            if (WinK == 12)
            {
                timer.Stop();
                MessageBox.Show($"УРА!ПАБЕДА!\n \rВаше время: {t} секунд");
                NewGame();
            }

        }
        public void NewGame()
        {
            RandomnZnacki();
            foreach(var a in grid.Children)
            {
                //((Button)a).Visibility = Visibility.Visible;
                ((Button)a).Content = "";
                ((Button)a).IsEnabled = true;
            }            
            k = 0;
            WinK = 0;
            indI = -1;
            indJ = -1;
            FirstI = -1;
            FirstJ = -1;
            t = 0;
            timer.Start();
        }

    }
}
