using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using Xamarin.Forms;

namespace UFit
{
    public partial class ProgramTrainer : ContentPage
    {
        public string id;
        public ProgramTrainer(string email)
        {
            id = email;
            InitializeComponent();
            Grid headerGrid = new Grid()
            {
                Margin = new Thickness(10, 5, 65, 20)
                 

            };
            headerGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            headerGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });

            Label programHeader = new Label()
            {
                Text = "Program Name",
                HorizontalTextAlignment = TextAlignment.Start,
                TextColor = Color.White,
                FontSize = 20,

                FontAttributes = FontAttributes.Bold
            };
            Grid.SetColumn(programHeader, 0);
            headerGrid.Children.Add(programHeader);

            Label clientHeader = new Label()
            {
                Text = "Client ID",
                HorizontalTextAlignment = TextAlignment.Start,
                TextColor = Color.White,
                FontSize = 20,
                FontAttributes = FontAttributes.Bold
            };
            Grid.SetColumn(clientHeader, 1);
            headerGrid.Children.Add(clientHeader);

            Table.Children.Add(headerGrid);

            try
            {
                string sqlconn = "Server=10.0.0.68;Database=UFit;User Id=sa;Password=reallyStrongPwd123;Persist Security Info=False;Encrypt=False";
                using (SqlConnection sqlConnection = new SqlConnection(sqlconn))
                {
                    sqlConnection.Open();
                    SqlCommand sqlCommand = new SqlCommand("SELECT DISTINCT [ProgramID], [NameProgram], [ClientEmail] FROM Workouts ", sqlConnection);
                    SqlDataReader reader = sqlCommand.ExecuteReader();

                    while (reader.Read())
                    {
                        int idP = reader.GetInt32(0);
                        string program = reader.GetString(1);
                        string client = reader.GetString(2);

                        Grid rowGrid = new Grid()
                        {
                            Margin = new Thickness(10,0,5, 30)
                        };

                        rowGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
                        rowGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
                        rowGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Auto) });
                        rowGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Auto) });

                        Label programLabel = new Label()
                        {
                            Text = program,
                            HorizontalTextAlignment = TextAlignment.Start,
                            TextColor = Color.White,
                            FontSize = 20
                        };
                        Grid.SetColumn(programLabel, 0);
                        rowGrid.Children.Add(programLabel);

                        Label clientLabel = new Label()
                        {
                            Text = client,
                            TextColor = Color.White,
                            FontSize = 20
                        };
                        Grid.SetColumn(clientLabel, 1);
                        rowGrid.Children.Add(clientLabel);

                        Button deleteButton = new Button()
                        {
                            Margin = new Thickness(10, 0, 20, 20),
                            WidthRequest = 20,
                            HeightRequest = 20,
                            FontSize = 16
                        };
                        deleteButton.ImageSource = new FontImageSource()
                        {
                            FontFamily = "Typicons",
                            Glyph = Common.IconFont.Delete,
                            Color = Color.White,
                            Size = 30
                        };
                        Grid.SetColumn(deleteButton, 3);
                        rowGrid.Children.Add(deleteButton);

                        deleteButton.Clicked += async (sender, args) =>
                        {
                            try
                            {
                                using (SqlConnection connection = new SqlConnection(sqlconn))
                                {
                                    await connection.OpenAsync();

                                    SqlCommand command = new SqlCommand("DELETE FROM Workouts WHERE ProgramID = @programId", connection);
                                    command.Parameters.AddWithValue("@programId", idP);
                                    await command.ExecuteNonQueryAsync();
                                }

                                // Remove the row from the grid
                                Table.Children.Remove(rowGrid);
                            }
                            catch (Exception ex)
                            {
                                await DisplayAlert("Error", "Failed to delete program: " + ex.Message, "OK");

                            }
                        };
                        rowGrid.Children.Add(deleteButton);
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
        async void OnAddButtonClicked(object sender, EventArgs e)
        {
            try
            {
                string sqlconn = "Server=10.0.0.68;Database=UFit;User Id=sa;Password=reallyStrongPwd123;Persist Security Info=False;Encrypt=False";

                using (SqlConnection sqlConnection = new SqlConnection(sqlconn))
                {
                    await sqlConnection.OpenAsync();

                    SqlCommand sqlCommand = new SqlCommand("INSERT INTO Programs DEFAULT VALUES;", sqlConnection);
                    await sqlCommand.ExecuteNonQueryAsync();

                    sqlCommand.CommandText = "SELECT @@IDENTITY";
                    int programId = Convert.ToInt32(await sqlCommand.ExecuteScalarAsync());


                    await Navigation.PushAsync(new CreateProgram(id, programId));
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Failed to create program: " + ex.Message, "OK");
            }
        }
        async void BackButtonClicked(object sender, EventArgs e)
        {
           
                    await Navigation.PushAsync(new Menu(id));
            
            
        }

    }
    }

