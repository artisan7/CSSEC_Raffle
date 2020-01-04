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

namespace CSSEC_Raffle
{
    /// <summary>
    /// Interaction logic for EventPage.xaml
    /// </summary>
    public partial class EventPage : Page
    {
        DatabaseConnect connection;

        public EventPage(DatabaseConnect conn, List<Event> events)
        {
            InitializeComponent();

            connection = conn;

            foreach (Event e in events)
                lb_event.Items.Add(e);
        }

        private void btn_event_Click(object sender, RoutedEventArgs e)
        {
            List<Event> events = new List<Event>();
            foreach (Object i in lb_event.SelectedItems)
                events.Add( (Event) i );

            if (events.Count > 0)
            {
                RaffleWindow window = new RaffleWindow(connection, events);
                window.Show();
            }
            else MessageBox.Show("No Event Selected.");
        }

        private void lb_event_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Object[] events = (Object[]) e.AddedItems;
            if (events.Length == 0)
                tblk_event.Text = "No Event Selected.";
            else if (events.Length == 1)
                tblk_event.Text = ( (Event) events.Last() ).Name;
            else
                tblk_event.Text = $"{events.Length} Events Selected.";
        }
    }
}
