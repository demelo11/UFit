using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Xamarin.Forms;


namespace UFit
{
    public partial class Menu : ContentPage
    {
        public string id;
        public Menu(string email)
        {
            InitializeComponent();

            id = email;
            try
            {

                string sqlconn = "Server=10.0.0.68;Database=UFit;User Id=sa;Password=reallyStrongPwd123;Persist Security Info=False;Encrypt=False";


                SqlConnection sqlConnection = new SqlConnection(sqlconn);
                sqlConnection.Open();

                SqlCommand sqlCommand = new SqlCommand("SELECT name FROM trainers WHERE [email] = '" + id + "'", sqlConnection);
                string name = (string)sqlCommand.ExecuteScalar();

                Welcome.Text = "Welcome back, " + name + "!" ;


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }




        }



        async void OnAddButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddUser(id));
        }


        async void LogoutButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MainPage());
        }

       

        async void ClientsButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ClientTrainer(id));
        }

        async void ProgramButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ProgramTrainer(id));
        }
    }


}

