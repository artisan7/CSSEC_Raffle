using System.Windows;

namespace CSSEC_Raffle
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

        private void btn_connect_Click(object sender, RoutedEventArgs e)
        {
            DatabaseConnect conn = new DatabaseConnect(tb_server.Text, tb_port.Text, tb_database.Text, tb_user.Text, tb_password.Password);

            if (conn.Connected)
            {
                f_view.Content = new EventPage(conn, Event.getAll(conn));
                //EventWindow window = new EventWindow(conn, Event.getAll(conn));
                //window.Show();
            }
        }
    }
}
