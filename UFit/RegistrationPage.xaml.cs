 using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace UFit
{	
	public partial class RegistrationPage : ContentPage
	{	
		public RegistrationPage ()
		{
			InitializeComponent ();
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


        async void OnRegisterClicked(object sender, EventArgs e)
        {


            var email = EmailEntry.Text;
            var password = PasswordEntry.Text;
            var name = NameEntry.Text;
            var radio = "";

            if (AthleteRadioButton.IsChecked)
            {
                radio = "Athlete";
            }
            else if (TrainerRadioButton.IsChecked)
            {
                radio = "Trainer";

            }

            if (email == null || password == null || name == null || radio == null)
            {
                Error.Text = "All entries are required";
            }
            else if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                Error.Text = "Invalid email";
                
            }

            // Check if password has letters and numbers and is between 6 and 30 characters
             else if (!Regex.IsMatch(password, @"^(?=.*[a-zA-Z])(?=.*[0-9])(?=.{6,30})"))
            {
                Error.Text = "Invalid Password. Must contain letters numbers, and be between 6 and 30 characters";
                return;
            }

          
            else
            {
                Error.Text = "";


                if(radio == "Athlete")
                {
                    try
                    {
                        string sqlconn = "Server=10.0.0.68;Database=UFit;User Id=sa;Password=reallyStrongPwd123;Persist Security Info=False;Encrypt=False";
                        SqlConnection sqlConnection = new SqlConnection(sqlconn);
                        sqlConnection.Open();

                        string selectSql = "SELECT COUNT(*) FROM users WHERE email = @email";
                        SqlCommand selectCmd = new SqlCommand(selectSql, sqlConnection);
                        selectCmd.Parameters.AddWithValue("@email", email);
                        int count = (int)selectCmd.ExecuteScalar();

                        if (count > 0)
                        {
                            Error.Text = "This email is already registered.";
                        }
                        else
                        {

                            string sql = "INSERT INTO users (email,name,password,role) VALUES (@email, @name, @password, @role)";
                            SqlCommand cmd = new SqlCommand(sql, sqlConnection);
                            cmd.Parameters.AddWithValue("@email", email);
                            cmd.Parameters.AddWithValue("@name", name);
                            cmd.Parameters.AddWithValue("@password", password);
                            cmd.Parameters.AddWithValue("@role", radio);

                            int rowsAffected = cmd.ExecuteNonQuery();

                            sqlConnection.Close();

                            DisplayAlert("Success", "Account has been created", "OK");
                            await Navigation.PushAsync(new MainPage());
                        }
                    }
                    catch (Exception ex)
                    {
                        DisplayAlert("Error", ex.Message, "OK");
                    }



                }
                else if (radio == "Trainer")
                {
                    try
                    {
                        string sqlconn = "Server=10.0.0.68;Database=UFit;User Id=sa;Password=reallyStrongPwd123;Persist Security Info=False;Encrypt=False";
                        SqlConnection sqlConnection = new SqlConnection(sqlconn);
                        sqlConnection.Open();

                        string selectSql = "SELECT COUNT(*) FROM trainers WHERE email = @email";
                        SqlCommand selectCmd = new SqlCommand(selectSql, sqlConnection);
                        selectCmd.Parameters.AddWithValue("@email", email);
                        int count = (int)selectCmd.ExecuteScalar();

                        if (count > 0)
                        {
                            Error.Text = "This email is already registered.";
                        }
                        else
                        {

                            string sql = "INSERT INTO trainers (email,name,password,role) VALUES (@email, @name, @password, @role)";
                            SqlCommand cmd = new SqlCommand(sql, sqlConnection);
                            cmd.Parameters.AddWithValue("@email", email);
                            cmd.Parameters.AddWithValue("@name", name);
                            cmd.Parameters.AddWithValue("@password", password);
                            cmd.Parameters.AddWithValue("@role", radio);

                            int rowsAffected = cmd.ExecuteNonQuery();

                            sqlConnection.Close();

                            DisplayAlert("Success", "Account has been created", "OK");
                            await Navigation.PushAsync(new MainPage());
                        }
                    }
                    catch (Exception ex)
                    {
                        DisplayAlert("Error", ex.Message, "OK");
                    }
                }
                



            }




            //  await Navigation.PushAsync(new Menu());
        }
    }
}

