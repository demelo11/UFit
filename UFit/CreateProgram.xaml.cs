using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Entry = Xamarin.Forms.Entry;

namespace UFit
{
    public partial class CreateProgram : ContentPage
    {
        private Xamarin.Forms.Picker partPicker;
        private Xamarin.Forms.Picker exercisePicker;
        private string num = "";
        private Xamarin.Forms.Entry repsEntry;
        private List<Xamarin.Forms.Picker> pickers = new List<Xamarin.Forms.Picker>();

        public string email;
        public int ProgramId;
        public CreateProgram(string id, int pId)
        {
           email = id;

           ProgramId = pId;


           

            InitializeComponent();
            AddRow();
          

                try
            {

                string sqlconn = "Server=10.0.0.68;Database=UFit;User Id=sa;Password=reallyStrongPwd123;Persist Security Info=False;Encrypt=False";

                SqlConnection sqlConnection = new SqlConnection(sqlconn);
                sqlConnection.Open();


                SqlCommand sqlCommand = new SqlCommand("SELECT email FROM users WHERE [trainer_email] = '" + email + "'", sqlConnection);
                SqlDataReader reader = sqlCommand.ExecuteReader();

                var name = new List<string>();
                while (reader.Read())
                {
                    name.Add(reader.GetString(0));

                }
                Client.ItemsSource = name;

                reader.Close();




            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

        }

        private void AddRow()
        {
            var rowGrid = new Grid
            {
                ColumnDefinitions =
        {
            new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
            new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
            new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
            new ColumnDefinition { Width = new GridLength(50) }
        }
            };

            // Create the part picker
           partPicker = new Xamarin.Forms.Picker
            {
                Title = "Part",
                FontSize = 20,
                Margin = new Thickness(10, 0, 0, 0),
               
            };
            pickers.Add(partPicker);
            partPicker.SelectedIndexChanged += Part_SelectedIndexChanged;
            partPicker.ItemsSource = new List<string> { "Chest", "Back", "Legs", "Arms", "Shoulders" , "Core"};
            
            Grid.SetColumn(partPicker, 0);
            rowGrid.Children.Add(partPicker);

           exercisePicker = new Xamarin.Forms.Picker
            {
                Title = "Exercise",
                FontSize = 20,
                Margin = new Thickness(0, 0, 0, 0)
            };

            


            Grid.SetColumn(exercisePicker, 1);
            rowGrid.Children.Add(exercisePicker);

            try
            {

                string sqlconn = "Server=10.0.0.68;Database=UFit;User Id=sa;Password=reallyStrongPwd123;Persist Security Info=False;Encrypt=False";


                SqlConnection sqlConnection = new SqlConnection(sqlconn);
                sqlConnection.Open();



                SqlCommand sqlCommand = new SqlCommand("SELECT Exercise FROM Exercises", sqlConnection);


                SqlDataReader reader = sqlCommand.ExecuteReader();

                var exer = new List<string>();
                while (reader.Read())
                {
                    exer.Add(reader.GetString(0));
                }
                exercisePicker.ItemsSource = exer;

                reader.Close();






            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
             

             repsEntry = new Xamarin.Forms.Entry
            {
                Placeholder = "Sets/Reps",
                TextColor = Color.Black,
                FontSize = 20
            };
            Grid.SetColumn(repsEntry, 2);
            rowGrid.Children.Add(repsEntry);

            var deleteButton = new Button
            {
                BackgroundColor = Color.Transparent,
                TextColor = Color.Red,
                Text = "X",
                FontSize = 20,
                FontAttributes = FontAttributes.Bold,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                HeightRequest = 40,
                WidthRequest = 40
            };
            deleteButton.Clicked += DeleteButton_Clicked;
            Grid.SetColumn(deleteButton, 3);
            rowGrid.Children.Add(deleteButton);

            Table.Children.Add(rowGrid);

         



        }

        private void DeleteButton_Clicked(object sender, EventArgs e)
        {
            var deleteButton = (Button)sender;
            var rowGrid = (Grid)deleteButton.Parent;

            Table.Children.Remove(rowGrid);


        }

        private void OnAddButtonClicked(object sender, EventArgs e)
        {
            AddRow();
        }

       
        private void Client_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
        private void Part_SelectedIndexChanged(object sender, EventArgs e)
        {

            var picker = (Xamarin.Forms.Picker)sender;
            if (picker.SelectedItem != null)
            {
                num = (string)picker.SelectedItem;


                string sqlconn = "Server=10.0.0.68;Database=UFit;User Id=sa;Password=reallyStrongPwd123;Persist Security Info=False;Encrypt=False";


                SqlConnection sqlConnection = new SqlConnection(sqlconn);
                sqlConnection.Open();




                SqlCommand sqlCommand = new SqlCommand("SELECT Exercise FROM Exercises WHERE  [Part] = '" + num + "'", sqlConnection);


                SqlDataReader reader = sqlCommand.ExecuteReader();

                var course = new List<string>();
                while (reader.Read())
                {
                    course.Add(reader.GetString(0));
                }


                exercisePicker.ItemsSource = course;

                reader.Close();

            }
        }


        private async void ConfirmButtonClicked(object sender, EventArgs e)
        {

            try {
                string sqlconn = "Server=10.0.0.68;Database=UFit;User Id=sa;Password=reallyStrongPwd123;Persist Security Info=False;Encrypt=False";
                SqlConnection sqlConnection = new SqlConnection(sqlconn);
                sqlConnection.Open();


                foreach (Grid rowGrid in Table.Children)
                {
                    if (((Xamarin.Forms.Picker)rowGrid.Children[0]).SelectedItem == null ||
                        ((Xamarin.Forms.Picker)rowGrid.Children[1]).SelectedItem == null ||
                        string.IsNullOrEmpty(((Entry)rowGrid.Children[2]).Text) || Client.SelectedItem == null || ProgramName.Text == null)
                    {
                        await DisplayAlert("Validation Error", "Please make sure all fields are filled in for all rows.", "OK");
                        return;
                    }
                }

                foreach (Grid rowGrid in Table.Children)
                {
                    string part = ((Xamarin.Forms.Picker)rowGrid.Children[0]).SelectedItem.ToString();
                    string exercise = ((Xamarin.Forms.Picker)rowGrid.Children[1]).SelectedItem.ToString();
                    string reps = ((Entry)rowGrid.Children[2]).Text;
                    string client = Client.SelectedItem.ToString();
                    string programName = ProgramName.Text;





                    string sql = "SELECT VideoLink FROM Exercises WHERE [exercise] = '" + exercise + "'";
                    SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection);
                    SqlDataReader reader = sqlCommand.ExecuteReader();

                    string link = string.Empty;
                    if (reader.HasRows)
                    {
                        reader.Read();
                        link = reader.GetString(0);
                    }
                    reader.Close();


                    sql = "INSERT INTO Workouts (ProgramID, Exercise, Reps, VideoLink, NameProgram, ClientEmail) VALUES (@programID, @exercise, @reps, @videoLink, @nameProgram, @clientEmail)";
                    sqlCommand = new SqlCommand(sql, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("programID", ProgramId);
                    sqlCommand.Parameters.AddWithValue("@exercise", exercise);
                    sqlCommand.Parameters.AddWithValue("@reps", reps);
                    sqlCommand.Parameters.AddWithValue("@videoLink", link);
                    sqlCommand.Parameters.AddWithValue("@nameProgram", programName);
                    sqlCommand.Parameters.AddWithValue("@clientEmail", client);

                    int rowsAffected = sqlCommand.ExecuteNonQuery();

                    reader.Close();
                    

                }
                        await DisplayAlert("Success", "Program created successfully!", "OK");
                        
                        await Navigation.PushAsync(new ProgramTrainer(email));





            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        async void BackButtonClicked(object sender, EventArgs e)
        {

            await Navigation.PushAsync(new ProgramTrainer(email));


        }

    }




}

