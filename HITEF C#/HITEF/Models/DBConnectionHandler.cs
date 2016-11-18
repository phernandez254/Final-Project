using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HITEF.Models
{
	public class DBConnectionHandler
	{
		enum requestsColumns { Id, Name, Phone, Email, Message }
        enum logInColumns { Id, Username, Password}

		public static SqlConnection Connect()
		{
			string connectionString = @"Data Source=PABLO\SQLEXPRESS; Initial Catalog=HITEF_Db; Integrated Security=True;Connect Timeout=15; Encrypt=False; TrustServerCertificate=False; ApplicationIntent=ReadWrite; MultiSubnetFailover=False";

			return new SqlConnection(connectionString);
		}

		public static List<ContactoFormViewModel> GetAllModels()
		{
			SqlConnection connection = Connect();		
			string query = "SELECT * FROM [HITEF_Db].[dbo].[Requests]";

			List<ContactoFormViewModel> modelList = new List<ContactoFormViewModel>();

			SqlCommand cmd = new SqlCommand(query, connection);

			try
			{
				connection.Open();
				SqlDataReader connectionDataReader = cmd.ExecuteReader();        
				if (connectionDataReader.HasRows) //We check if dataReader is NOT empty by checking if it has any rows.
				{
					while (connectionDataReader.Read()) //So, as long as it can be read...
					{
						ContactoFormViewModel tempModel = new ContactoFormViewModel();  //we populate the properties of a new model
						
						tempModel.Nombre = connectionDataReader.GetValue((int)requestsColumns.Name).ToString();
						tempModel.Telefono = connectionDataReader.GetValue((int)requestsColumns.Phone).ToString();
						tempModel.CorreoElectronico = connectionDataReader.GetValue((int)requestsColumns.Email).ToString();
						tempModel.Mensaje = connectionDataReader.GetValue((int)requestsColumns.Message).ToString();

						modelList.Add(tempModel);    //and finally, we Add that model to the List<> the method will be returning.
                        //At the end, we will have a full List with all the models/records from our Database Requests Table.
					}
				}
			}
			catch (Exception exception)
			{
				throw exception;
			}
			finally
			{
				connection.Close();
			}
			return modelList;
		}
		//________________________________________________________________________________________________

		public static void AddModel(ContactoFormViewModel modelToAdd)
		{
			SqlConnection connection = new SqlConnection();
			connection = Connect();

			string requestsQuery = "INSERT INTO [Requests] (Name, Phone, Email, Message) VALUES (@name, @phone, @email, @message)";

			SqlCommand requestsCommand = new SqlCommand(requestsQuery, connection);

			requestsCommand.Parameters.AddWithValue("@name", modelToAdd.Nombre);
            requestsCommand.Parameters.AddWithValue("@phone", modelToAdd.Telefono);
            requestsCommand.Parameters.AddWithValue("@email", modelToAdd.CorreoElectronico);
            requestsCommand.Parameters.AddWithValue("@message", modelToAdd.Mensaje);

			try
			{
				connection.Open();
                requestsCommand.ExecuteNonQuery();
			}
			catch (Exception exc)
			{
				Console.WriteLine(exc.Message);
				throw exc;
			}
		}
		//________________________________________________________________________________________________

		public static bool ValidateLogIn(LoginFormViewModel modelToCheck)
		{
			bool match = false;

			SqlConnection connection = new SqlConnection();
			connection = Connect();

			string LogInQuery = "SELECT * FROM [HITEF_Db].[dbo].[LogIn]";

			SqlCommand LogInCommand = new SqlCommand(LogInQuery, connection);

			try
			{
				connection.Open();
                SqlDataReader logInDataReader = LogInCommand.ExecuteReader();

                if (logInDataReader.HasRows)
                {
                    LoginFormViewModel myModel = new LoginFormViewModel();

                    while (logInDataReader.Read())
                    {                   
                        myModel.Usuario = logInDataReader.GetValue((int)logInColumns.Username).ToString();
                        myModel.Contraseña = logInDataReader.GetValue((int)logInColumns.Password).ToString();
                    }

                    if (modelToCheck.Usuario == myModel.Usuario && modelToCheck.Contraseña == myModel.Contraseña)
                    {
                        match = true;
                    }
                    else
                    {
                        match = false;
                    }

                }
			}
			catch (Exception exc)
			{

				throw exc;
			}
			finally
			{
				connection.Close();
			}
			return match;			
		}
	}
}