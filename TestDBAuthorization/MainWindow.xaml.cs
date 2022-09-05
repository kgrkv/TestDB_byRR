using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace TestDBAuthorization
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private string sumbol1; // символы капчи 
        private string sumbol2;
        private string sumbol3;

        private string[] sumbolArrya = new string[] { "A", "B", "C", "D", "F", "G", "I" }; // допустимые  символы для капчи 
         

        private int countLogin = 0; // переменная для подсчета  неудачных попыток

        public MainWindow()
        {
            InitializeComponent();
            gridCaptcha.Visibility = Visibility.Collapsed; // при  старте без  капчи 
        }

        private void btGo_Click(object sender, RoutedEventArgs e)
        {

            #region валидация

            if (string.IsNullOrEmpty(tbLogin.Text))
            {
                MessageBox.Show("Введите логин"); // если не ввели пароль
                return; // выход из  метода  сразу 
            }

            if (string.IsNullOrEmpty(pbPassword.Password))
            {
                MessageBox.Show("Введите пароль");
                return;
            }
            #endregion

            if (countLogin > 1) // если превысили лимит   входов 
            {

                if (tbCaptcha.Text != sumbol1 + sumbol2 + sumbol3)
                {
                    MessageBox.Show("Неверная капча - введите  капчу - блокировка  на  5 секунд ");

                    this.Title = "Блокировка"; // помянем  шапку 

                    Thread.Sleep(5000); // спать на  5 секунд; 
                    this.Title = "Авторизация"; // вернем шапку 
                    return; //выход если ввели не верно 
                }
            }
            
             var us = Logic.AthorizationService.AthorizationUser(tbLogin.Text, pbPassword.Password); // Делаем  автоионизацию 

             if (us == null) // если  вернулся null 
             {
                 countLogin++;
                 MessageBox.Show("error");
                 isOnCaptcha();
                 return;
             }

             switch (us.Accounting) // если есть  ответ  
             {
                 case "admin": MessageBox.Show("Привет " + us.Name + "Вы " + us.Accounting); countLogin = 0; break; 
                 case "user": MessageBox.Show("Привет " + us.Name + " Вы " + us.Accounting); countLogin = 0; break;
                 case "NoAthorization": MessageBox.Show("Неверный  логин  или  пароль"); countLogin++;  break;
             }
               isOnCaptcha();
        }

        /// <summary>
        /// показывает  или  убирает   капчу
        /// </summary>
        private void isOnCaptcha()
        {
          if(countLogin>1)
          {
              btnRefresh_Click( this, null);  // генерируем  капчу 
              gridCaptcha.Visibility = Visibility.Visible; // делаем ее  видимой 
          }
          else
          {
                gridCaptcha.Visibility = Visibility.Collapsed;
          }
        }

        /// <summary>
        /// обновление  капчи 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in gridCaptcha.Children)  // прежде чем  обновить переберем  весь  грид  и убираем  старые  линии 
            {
                var s = item as Line; // если линия 

                if (s != null)
                {
                    s.Visibility = Visibility.Collapsed; // делаем невидимой 
                }

            }
            GetCaptha(); // генерируем  капчу 
        }


        /// <summary>
        /// алгоритм генерации  капчи
        /// </summary>
        private void GetCaptha()
        {
            string[] badStrings = new string[] { "`", "-", ".", "," }; // символы для шума
            Random random = new Random();
            sumbol1 = sumbolArrya[random.Next(0, sumbolArrya.Length)]; // первый символ 
            sumbol2 = sumbolArrya[random.Next(0, sumbolArrya.Length)]; // второй символ 
            sumbol3 = sumbolArrya[random.Next(0, sumbolArrya.Length)]; // третий  символ 

            labelsymbol1.Margin = new Thickness { Bottom = random.Next(0, 20), Left = random.Next(0, 20), Right = random.Next(0, 20), Top = random.Next(0, 20) };
            labelsymbol2.Margin = new Thickness { Bottom = random.Next(0, 20), Left = random.Next(0, 20), Right = random.Next(0, 20), Top = random.Next(0, 20) };
            labelsymbol3.Margin = new Thickness { Bottom = random.Next(0, 20), Left = random.Next(0, 20), Right = random.Next(0, 20), Top = random.Next(0, 20) };

            labelsymbol1.FontSize = random.Next(14, 25); // Размер  шрифта  первого символа
            labelsymbol2.FontSize = random.Next(14, 25);
            labelsymbol3.FontSize = random.Next(14, 25);

            labelsymbol1.Content = sumbol1 + badStrings[random.Next(0, badStrings.Length)]; // добавляем  шум
            labelsymbol2.Content = sumbol2 + badStrings[random.Next(0, badStrings.Length)]; ;
            labelsymbol3.Content = sumbol3 + badStrings[random.Next(0, badStrings.Length)]; ;

            for (int i = 0; i < 10; i++)
            {
                gridCaptcha.Children.Add(NewLine()); // Добавляем  линии  в  грид
            }

        }

        /// <summary>
        /// Генерация случайной  линии  
        /// </summary>
        /// <returns></returns>
        private UIElement NewLine()
        {
            SolidColorBrush[] brushes = new SolidColorBrush[] { Brushes.Red, Brushes.Black, Brushes.Azure }; // цвета  линий 

            Random random = new Random();

            var l = new Line()
            {
                X1 = random.Next(0, 100), // координаты  линии 
                X2 = random.Next(0, 100),
                Y1 = random.Next(0, 100),
                Y2 = 0,
                Stroke = brushes[random.Next(0, brushes.Length)],  // цвет  линии 
                StrokeThickness = random.Next(1, 3) // толчена  линии 
            };

            return l;
        }

        /// <summary>
        /// показать  пароль 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btShowPassword_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(pbPassword.Password, "Вы ввели пароль");
        }
    }
}
