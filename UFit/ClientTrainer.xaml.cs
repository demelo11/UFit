using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Data.SqlClient;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace UFit
{
    public partial class ClientTrainer : ContentPage
    {
        public string email = "";
        public ClientTrainer(string id)
        {
            email = id;
            InitializeComponent();

            try
            {

             
                string sqlconn = "Server=10.0.0.68;Database=UFit;User Id=sa;Password=reallyStrongPwd123;Persist Security Info=False;Encrypt=False";
                SqlConnection sqlConnection = new SqlConnection(sqlconn);
                sqlConnection.Open();

                SqlCommand sqlCommand = new SqlCommand("SELECT [name], [email] FROM users WHERE  trainer_email = '" + email + "'", sqlConnection);

                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    string names = reader.GetString(0);
                    string emails = reader.GetString(1);

                    Grid rowGrid = new Grid()
                    {
                        Margin = new Thickness(5, 30)

                    };


                    rowGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
                    rowGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
                    rowGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Auto) });
                    rowGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Auto) });

                    Label nameLabel = new Label()
                    {

                        Text = names,
                        HorizontalTextAlignment = TextAlignment.Center,
                        TextColor = Color.White,
                        FontSize = 20
                    };
                    Grid.SetColumn(nameLabel, 0);
                    rowGrid.Children.Add(nameLabel);

                    Label emailLabel = new Label()
                    {

                        Text = emails,
                        TextColor = Color.White,
                        FontSize = 20

                    };
                    Grid.SetColumn(emailLabel, 1);
                    rowGrid.Children.Add(emailLabel);


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

                    deleteButton.Clicked += async (sender, e) =>
                    {
                        using (SqlConnection deleteConnection = new SqlConnection(sqlconn))
                        {
                            deleteConnection.Open();
                            SqlCommand deleteCommand = new SqlCommand("UPDATE users SET trainer_email = null WHERE email = @Email", deleteConnection);
                            deleteCommand.Parameters.AddWithValue("@Email", emails);
                            int rowsAffected = await deleteCommand.ExecuteNonQueryAsync();

                            if (rowsAffected > 0)
                            {
                              
                                Table.Children.Remove(rowGrid);
                            }
                            else
                            {
                                await DisplayAlert("Error", "Failed to delete the course registration", "OK");
                            }
                        }
                    };

                    Table.Children.Add(rowGrid);
                   
                }
                reader.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            
            
        }
        async void BackButtonClicked(object sender, EventArgs e)
        {

            await Navigation.PushAsync(new Menu(email));


        }
    }
}
