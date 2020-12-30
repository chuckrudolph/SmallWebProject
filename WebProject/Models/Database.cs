using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json;
using System.Web.Mvc;
using WebProject.Hubs;
using System.Configuration;

namespace WebProject.Models
{
    public class Database
    {
        string connectionString;

        public Database()
        {
            connectionString = "Data Source=JMORGA-LT1\\SQLEXPRESS;Initial Catalog=People;Integrated Security=True";
        }

        public string getPersonList()
        {
            List<Person> personList = new List<Person>();
            string getPersonTableQuery = "SELECT ID, FirstName, LastName FROM dbo.[Person]";
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(getPersonTableQuery, connection);
            SqlDataReader reader;

            command.Notification = null;
            connection.Open();

            //If a value from the Person table changes, it will fire the dependencyOnChange method to signal the viewer to reload the data
            //SqlDependency dependency = new SqlDependency(command);
            //dependency.OnChange += new OnChangeEventHandler(dependencyOnChange);
            //if (connection.State == ConnectionState.Closed)
            //    connection.Open();

            reader = command.ExecuteReader();

            while (reader.Read()) //Reads a row from a table, creates a person object with the information and adds it into the person list
            {
                personList.Add(new Person(reader.GetInt32(0), reader.GetString(1), reader.GetString(2)));
            }

            connection.Close();

            return JsonConvert.SerializeObject(personList);
        }

        //Updates the Person table from the people database. 
        public void changePersonData(Person changedPerson)
        {
            SqlCommand command;
            
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            command = new SqlCommand("UPDATE Person SET FirstName = @FirstName, LastName = @LastName WHERE ID = @ID", connection);
            command.Parameters.AddWithValue("FirstName", changedPerson.firstName);
            command.Parameters.AddWithValue("LastName", changedPerson.lastName);
            command.Parameters.AddWithValue("ID", changedPerson.id);
            command.ExecuteNonQuery();
            
            connection.Close();
        }

        private void dependencyOnChange(object sender, SqlNotificationEventArgs e)
        {
            if (e.Type == SqlNotificationType.Change)
            {
                TableHub hub = new TableHub();
                hub.updateAvailable();
            }
        }
    }
}