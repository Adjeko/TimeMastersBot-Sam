using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace TimeMastersClassLibrary.Database
{
    public class DatabaseClient
    {
        public void Read()
        {
            try
            {
                string connectionString = "Server = tcp:databaserpserver.database.windows.net,1433; Initial Catalog = Database_RP; Persist Security Info = False; User ID = thetimemaster; Password =Thetime983214; MultipleActiveResultSets = False; Encrypt = True; TrustServerCertificate = False; Connection Timeout = 30";
                using (SqlConnection conn =
                    new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd =
                        new SqlCommand("SELECT * FROM EmployeeDetails", conn))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Console.WriteLine("Id = ", reader["Id"]);
                                Console.WriteLine("Name = ", reader["Name"]);
                                Console.WriteLine("Address = ", reader["Address"]);
                            }
                        }

                        reader.Close();
                    }
                }
            }
            catch (SqlException ex)
            {
                //Log exception
                //Display Error message
            }
        }

        public void Insert()
        {
            string connectionString = "Server = tcp:databaserpserver.database.windows.net,1433; Initial Catalog = Database_RP; Persist Security Info = False; User ID = thetimemaster; Password =Thetime983214; MultipleActiveResultSets = False; Encrypt = True; TrustServerCertificate = False; Connection Timeout = 30";
            SqlConnection conn = new SqlConnection(connectionString);
            try
            {
                conn.Open();

                using (SqlCommand cmd =
                    new SqlCommand("INSERT INTO TimeUser VALUES(" +
                        "@Id, @Name, @Address)", conn))
                {
                    cmd.Parameters.AddWithValue("@Id", 3);
                    cmd.Parameters.AddWithValue("@Name", "Roland");
                    cmd.Parameters.AddWithValue("@Address", "GER");

                    int rows = cmd.ExecuteNonQuery();

                    //rows number of record got inserted
                }
                conn.Close();
            }
            catch (SqlException ex)
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }

                //Log exception
                //Display Error message
            }
        }

        public void Update()
        {
            try
            {
                string connectionString = "Server = tcp:databaserpserver.database.windows.net,1433; Initial Catalog = Database_RP; Persist Security Info = False; User ID = thetimemaster; Password =Thetime983214; MultipleActiveResultSets = False; Encrypt = True; TrustServerCertificate = False; Connection Timeout = 30";

                using (SqlConnection conn =
                    new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd =
                        new SqlCommand("UPDATE TimeUser SET Name=@NewName, Address=@NewAddress" +
                            " WHERE Id=@Id", conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", 1);
                        cmd.Parameters.AddWithValue("@Name", "Herbert");
                        cmd.Parameters.AddWithValue("@Address", "RO");

                        int rows = cmd.ExecuteNonQuery();

                        //rows number of record got updated
                    }
                }
            }
            catch (SqlException ex)
            {
                //Log exception
                //Display Error message
            }
        }

        public void Delete()
        {
            try
            {
                string connectionString = "Server = tcp:databaserpserver.database.windows.net,1433; Initial Catalog = Database_RP; Persist Security Info = False; User ID = thetimemaster; Password =Thetime983214; MultipleActiveResultSets = False; Encrypt = True; TrustServerCertificate = False; Connection Timeout = 30";
                using (SqlConnection conn =
                    new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd =
                        new SqlCommand("DELETE FROM TimeUser " +
                            "WHERE Id=@Id", conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", 1);

                        int rows = cmd.ExecuteNonQuery();

                        //rows number of record got deleted
                    }
                }
            }
            catch (SqlException ex)
            {
                //Log exception
                //Display Error message
            }
        }
    }
}
