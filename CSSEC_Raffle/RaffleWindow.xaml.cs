using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace CSSEC_Raffle
{
    public partial class RaffleWindow : Window
    {
        private List<Student> allStudents, remainingStudents;
        private DispatcherTimer shuffleTimer, revealTimer;
        private string chosen, nonRandomString;

        private Image playIcon, stopIcon, refreshIcon;

        public RaffleWindow(DatabaseConnect conn, List<Event> evts)
        {
            InitializeComponent();

            // Adjust font size based on display screen
            tblk_name.FontSize = SystemParameters.VirtualScreenHeight * 0.12;

            // Get students from Database
            allStudents = new List<Student>();
            foreach (Event evt in evts)
                allStudents.InsertRange( 0, Student.getAll(conn, evt) );

            // Create a Deeeeeeep Copy of allStudents to remainingStudents
            remainingStudents = allStudents.ConvertAll( s => new Student(s.ID, s.LastName, s.FirstName) );

            shuffleTimer = new DispatcherTimer();
            shuffleTimer.Interval = TimeSpan.FromMilliseconds(40);
            shuffleTimer.Tick += shuffleTimer_Tick;

            revealTimer = new DispatcherTimer();
            revealTimer.Interval = TimeSpan.FromMilliseconds(60);
            revealTimer.Tick += revealTimer_Tick;

            // Preload all icons
            playIcon = new Image { Source = new BitmapImage( new Uri("res/green play icon.png", UriKind.Relative) ) };
            stopIcon = new Image { Source = new BitmapImage( new Uri("res/green stop icon.png", UriKind.Relative) ) };
            refreshIcon = new Image { Source = new BitmapImage( new Uri("res/green refresh icon.png", UriKind.Relative) ) };
        }

        private void shuffleTimer_Tick(object sender, EventArgs e)
        {
            string randomString = "";
            if (nonRandomString.Length < chosen.Length)
            {
                // produce random bullshit to display
                Random rand = new Random();

                for (int i = 0; i < Math.Max(8 - nonRandomString.Length, 1); i++)
                    randomString += (char)rand.Next(32, 127);
            }

            tblk_name.Text = nonRandomString + randomString;
        }

        private void revealTimer_Tick(object sender, EventArgs e)
        {
            // reveal a character each tick
            int index = nonRandomString.Length;

            try
            {
                nonRandomString += chosen[index];
            }
            catch
            {
                revealTimer.Stop();
                shuffleTimer.Stop();

                if (remainingStudents.Count > 0)
                    btn_raffleStart.Content = playIcon;
                else btn_raffleStart.Content = refreshIcon;

                btn_raffleStart.Visibility = Visibility.Visible;
            }
        }

        private void btn_raffleStart_Click(object sender, RoutedEventArgs e)
        {
            if (!shuffleTimer.IsEnabled)
            {
                try
                {
                    Random rand = new Random();
                    Student chosenStudent = remainingStudents[rand.Next(remainingStudents.Count)];
                    chosen = chosenStudent.getFullName();
                    remainingStudents.Remove(chosenStudent);

                    nonRandomString = "";
                    shuffleTimer.Start();
                    btn_raffleStart.Content = stopIcon;
                }
                catch   // if remainingStudents is empty
                {
                    // Create a Deeeeeeep Copy of allStudents to remainingStudents
                    remainingStudents = allStudents.ConvertAll(s => new Student(s.ID, s.LastName, s.FirstName));

                    btn_raffleStart.Content = playIcon;
                }
            }
            else
            {
                // start revealing chosen student
                revealTimer.Start();
                btn_raffleStart.Visibility = Visibility.Hidden;
            }
        }

        private void btn_exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
