using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Data.SqlClient;

namespace UFit
{
    public partial class AddUser : ContentPage
    {
        public string id;
        public AddUser(string email)
        {
            InitializeComponent();
            id = email;
        }



        private void OnEmailEntryFocused(object sender, FocusEventArgs e)
        {

            EmailEntry.BackgroundColor = Color.FromHex("#00b7c0");
            EmailEntry.PlaceholderColor = Color.White;
        }

        private void OnEmailEntryUnfocused(object sender, FocusEventArgs e)
        {

            EmailEntry.BackgroundColor = Color.FromHex("#ebfaf9");
            EmailEntry.PlaceholderColor = Color.LightGray;
        }

        private void OnNameEntryFocused(object sender, FocusEventArgs e)
        {

            NameEntry.BackgroundColor = Color.FromHex("#00b7c0");
            NameEntry.PlaceholderColor = Color.White;
        }

        private void OnNameEntryUnfocused(object sender, FocusEventArgs e)
        {

            NameEntry.BackgroundColor = Color.FromHex("#ebfaf9");
            NameEntry.PlaceholderColor = Color.LightGray;
        }

        async void OnAddClicked(object sender, EventArgs e)
        {
            var email = EmailEntry.Text;
            try
            {
                string sqlconn = "Server=10.0.0.68;Database=UFit;User Id=sa;Password=reallyStrongPwd123;Persist Security Info=False;Encrypt=False";
                SqlConnection sqlConnection = new SqlConnection(sqlconn);
                sqlConnection.Open();

                string sql = "SELECT COUNT(*) FROM users WHERE email = @email";
                SqlCommand cmd = new SqlCommand(sql, sqlConnection);
                cmd.Parameters.AddWithValue("@email", email);

                int count = (int)cmd.ExecuteScalar();
                if (count > 0)
                {
                    sql = "UPDATE users SET trainer_email = @trainer_email WHERE email = @email AND trainer_email IS NULL";
                    cmd = new SqlCommand(sql, sqlConnection);
                    cmd.Parameters.AddWithValue("@trainer_email", "mdemelo@algomau.ca");
                    cmd.Parameters.AddWithValue("@email", email);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {

                    }
                    else
                    {
                        Error.Text = "Client already has a trainer";
                    }
                }
                else
                {
                    Error.Text = "Client does not exist";
                }

                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", ex.Message, "OK");
            }
        }


        async void OnBackClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Menu(id));
        }
    }
}

