using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Data.SqlClient;

namespace UFit
{	
	public partial class MenuClient : ContentPage
	{
		public string email = "";
		public MenuClient (string id)
		{
			email = id;
			InitializeComponent ();
            try
            {

                string sqlconn = "Server=10.0.0.68;Database=UFit;User Id=sa;Password=reallyStrongPwd123;Persist Security Info=False;Encrypt=False";


                SqlConnection sqlConnection = new SqlConnection(sqlconn);
                sqlConnection.Open();

                SqlCommand sqlCommand = new SqlCommand("SELECT name FROM users WHERE [email] = '" + id + "'", sqlConnection);
                string name = (string)sqlCommand.ExecuteScalar();

                Welcome.Text = "Welcome back, " + name + "!";


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }



            try
            {
                string sqlconn = "Server=10.0.0.68;Database=UFit;User Id=sa;Password=reallyStrongPwd123;Persist Security Info=False;Encrypt=False";
                using (SqlConnection sqlConnection = new SqlConnection(sqlconn))
                {
                    sqlConnection.Open();
                    SqlCommand sqlCommand = new SqlCommand("SELECT DISTINCT [ProgramID], [NameProgram], [ClientEmail] FROM Workouts WHERE [ClientEmail] = '" + email + "'", sqlConnection);
                    SqlDataReader reader = sqlCommand.ExecuteReader();

                    while (reader.Read())
                    {
                        int idP = reader.GetInt32(0);
                        string program = reader.GetString(1);
                        string client = reader.GetString(2);

                        Grid rowGrid = new Grid()
                        {
                            Margin = new Thickness(10, 0, 10, 30)
                        };
                        rowGrid.BackgroundColor = Color.FromHex("#99eaee");

                        rowGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
                        rowGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
                        rowGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Auto) });
                        rowGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Auto) });

                        Label programLabel = new Label()
                        {
                            Margin = new Thickness(20, 15, 0, 0),
                            Text = program,
                            HorizontalTextAlignment = TextAlignment.Start,
                            TextColor = Color.White,
                            FontSize = 20
                        };
                        Grid.SetColumn(programLabel, 0);
                        rowGrid.Children.Add(programLabel);

                       

                        Button playButton = new Button()
                        {
                            Margin = new Thickness(10, 20, 40, 30),
                            WidthRequest = 40,
                            HeightRequest = 40,
                            FontSize = 40,
                           
                            
                        };
                        playButton.ImageSource = new FontImageSource()
                        {
                            FontFamily = "Typicons",
                            Glyph = Common.IconFont.MediaPlay,
                            Color = Color.White,
                            Size = 70
                        };
                        Grid.SetColumn(playButton, 3);

                        rowGrid.Children.Add(playButton);
                        playButton.Clicked += async (s, e) =>
                        {
                            await Navigation.PushAsync(new PlayPage(id,idP));
                        };


                        rowGrid.Children.Add(playButton);
                        Table.Children.Add(rowGrid);

                    }
                    reader.Close();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }


        }


        async void LogOutButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MainPage());
        }
    }
}

