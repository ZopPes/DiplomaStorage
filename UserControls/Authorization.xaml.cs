using System;
using System.Data.SqlClient;
using System.Windows;

namespace DiplomaStorage.UserControls
{
    /// <summary>
    /// Логика взаимодействия для Authorization.xaml
    /// </summary>
    public partial class Authorization : Window
    {
        public SqlConnection sql { get; set; }

        public Authorization()
        {
            InitializeComponent();
            falseopen.Visibility = Visibility.Collapsed;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var pass = pas.SecurePassword;
            pass.MakeReadOnly();
            sql = new SqlConnection(
               @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=DiplomaStorage;"
               ,
               new SqlCredential(log.Text, pass));
            try
            {
                sql.Open();
                falseopen.Visibility = Visibility.Visible;
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                falseopen.Visibility = Visibility.Visible;
            }
        }
    }
}