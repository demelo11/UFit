using System;
using System.Collections.Generic;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;
using Xamarin.Forms;
using Xamarin.CommunityToolkit.UI.Views;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace UFit
{
    public partial class PlayPage : ContentPage
    {
        private List<WorkoutModel> workouts;
        private int currentIndex;
        private string email = "";
        public PlayPage(string emailid, int idp)
        {
            email = emailid;
            InitializeComponent();

            try
            {
                string sqlconn = "Server=10.0.0.68;Database=UFit;User Id=sa;Password=reallyStrongPwd123;Persist Security Info=False;Encrypt=False";
                SqlConnection sqlConnection = new SqlConnection(sqlconn);
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand("SELECT Exercise, Reps, VideoLink FROM Workouts WHERE [ProgramID] = '" + idp + "'", sqlConnection);
                SqlDataReader reader = sqlCommand.ExecuteReader();

                workouts = new List<WorkoutModel>();
                while (reader.Read())
                {
                    workouts.Add(new WorkoutModel
                    {
                        Exercise = reader.GetString(0),
                        Reps = reader.GetString(1),
                        VideoLink = reader.GetString(2)
                    });
                }
                reader.Close();
                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

            currentIndex = 0;
            SetPageContent();
        }

        private void SetPageContent()
        {
            Exercise.Text = workouts[currentIndex].Exercise;
            Reps.Text = workouts[currentIndex].Reps;
            GetVideoContent(workouts[currentIndex].VideoLink);
        }

        private async void GetVideoContent(string videoLink)
        {
            try
            {
                var youtube = new YoutubeClient();
                var video = await youtube.Videos.GetAsync(videoLink);

                var streamManifest = await youtube.Videos.Streams.GetManifestAsync(videoLink);

                var streamInfo = streamManifest.GetMuxedStreams().GetWithHighestVideoQuality();

                if (streamInfo != null)
                {
                    var stream = await youtube.Videos.Streams.GetAsync(streamInfo);

                    MyMediaElement.Source = streamInfo.Url;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await DisplayAlert("Error", "The video is not available.", "OK");
            }
        }
        async void BackClicked(object sender, EventArgs e)
        {

            await Navigation.PushAsync(new MenuClient(email));


        }
        private void NextClicked(object sender, EventArgs e)
        {
           
            currentIndex++;
            if (currentIndex >= workouts.Count)
            {
                currentIndex = 0;
            }
            SetPageContent();
        }

    
    }

    public class WorkoutModel
    {
        public string Exercise { get; set; }
        public string Reps { get; set; }
        public string VideoLink { get; set; }
    }

}
