using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace LoginwithDB
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        // Connection string to connect to the SQL Server database
        string connector = "Data Source=R103-PC12;Initial Catalog=LoginAgainDB;User ID=sa;Password=sap;Encrypt=False";

        public async Task messsagebox(string LoginMessage)
        {
            ContentDialog messagebox = new ContentDialog()
            {
                Title = "Login Message",
                Content = LoginMessage,
                CloseButtonText = "OK"
            };
            await messagebox.ShowAsync();
        }
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void btnreset_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void btnlogin_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection class_that_connects_to_databse_using_String_connector = new SqlConnection(connector);
            class_that_connects_to_databse_using_String_connector.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = class_that_connects_to_databse_using_String_connector;
            //we are using parameterized query(like @username) to prevent SQL injection
            cmd.CommandText = "SELECT * FROM LoginAgain WHERE Userma = @Username AND Password = @Password";



            SqlParameter Uname = new SqlParameter("@Username", System.Data.SqlDbType.VarChar);
            SqlParameter Upass = new SqlParameter("@Password", System.Data.SqlDbType.VarChar);

            Uname.Value = txtusername.Text;
            Upass.Value = txtpassword.Password;

            cmd.Parameters.Add(Uname);
            cmd.Parameters.Add(Upass);

            //let us read what we have queried from the database
            //we only use the SQLDATAREADER when using the select statement, for other statements like insert,
            //update, delete we use ExecuteNonQuery() method
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                //i will track my combobox items to track the role of the user that is logging in.
                ComboBoxItem selectedRole = txtdropdown.SelectedItem as ComboBoxItem;
                string trackedroles = selectedRole.Content.ToString();
                if (trackedroles.Contains("Admin"))
                {
                    Frame.Navigate(typeof(Admin));
                    return;
                }
                else if (trackedroles.Contains("User"))
                {
                    Frame.Navigate(typeof(User));
                    return;
                }
                else if (trackedroles.Contains("User"))
                {

                    Frame.Navigate(typeof(User));
                    await messsagebox("Please select a role from the dropdown.");
                    return;
                }

                else
                {
                    await messsagebox("Invalid Username or Password");
                }
            }
        }
    }
}
