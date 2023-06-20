using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace UFit
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
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

        private void OnPasswordEntryFocused(object sender, FocusEventArgs e)
        {
            
            PasswordEntry.BackgroundColor = Color.FromHex("#00b7c0");
            PasswordEntry.PlaceholderColor = Color.White;
        }

        private void OnPasswordEntryUnfocused(object sender, FocusEventArgs e)
        {
            
            PasswordEntry.BackgroundColor = Color.FromHex("#ebfaf9");
            PasswordEntry.PlaceholderColor = Color.LightGray;
        }
    
    async void OnRegisterClicked(object sender, EventArgs e)
    {
        
        await Navigation.PushAsync(new RegistrationPage());
    }

    async void OnLogInClicked(object sender, EventArgs e)
    {
            var email = EmailEntry.Text;
            string id = email;
            var password = PasswordEntry.Text;

            if (email == null || password == null)
            {
                Error.Text = "Email and password are required";
            }
            else
            {
                Error.Text = "";

                try
                {
                    string sqlconn = "Server=10.0.0.68;Database=UFit;User Id=sa;Password=reallyStrongPwd123;Persist Security Info=False;Encrypt=False";
                    SqlConnection sqlConnection = new SqlConnection(sqlconn);
                    sqlConnection.Open();

                    string sql = "SELECT COUNT(*) FROM trainers WHERE email = @email AND password = @password";
                    SqlCommand cmd = new SqlCommand(sql, sqlConnection);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@password", password);

                    int count = (int)cmd.ExecuteScalar();
                    if (count > 0)
                    {
                        await Navigation.PushAsync(new Menu(id));
                       
                    }
                    else
                    {


                        try
                        {
                            sqlconn = "Server=10.0.0.68;Database=UFit;User Id=sa;Password=reallyStrongPwd123;Persist Security Info=False;Encrypt=False";
                            sqlConnection = new SqlConnection(sqlconn);
                            sqlConnection.Open();

                            sql = "SELECT COUNT(*) FROM users WHERE email = @email AND password = @password";
                            cmd = new SqlCommand(sql, sqlConnection);
                            cmd.Parameters.AddWithValue("@email", email);
                            cmd.Parameters.AddWithValue("@password", password);

                            int count2 = (int)cmd.ExecuteScalar();
                            if (count2 > 0)
                            {
                                await Navigation.PushAsync(new MenuClient(id));

                            }
                            else
                            {



                                Error.Text = "Invalid email or password";
                            }

                        }
                        catch (Exception ex)
                        {
                            DisplayAlert("Error", ex.Message, "OK");
                        }
                    }

                    sqlConnection.Close();
                }
                catch (Exception ex)
                {
                    DisplayAlert("Error", ex.Message, "OK");
                }
            }
          
    }
}
}

