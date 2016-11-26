using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Diagnostics;

namespace TimeMastersClassLibrary.Database
{
    public class GoogleCalenderTokens
    {
        private const string Connectionstring = "Server = tcp:databaserpserver.database.windows.net,1433; Initial Catalog = Database_RP; Persist Security Info = False; User ID = thetimemaster; Password =Thetime983214; MultipleActiveResultSets = False; Encrypt = True; TrustServerCertificate = False; Connection Timeout = 30";
        private SqlConnection conn;

        public GoogleCalenderTokens()
        {
             conn = new SqlConnection(Connectionstring);
        }

        public void GetCredential(string id, out string accTo, out string refTo, out long lifetime, out DateTime createDate )
        {
            conn.Open();

            using (SqlCommand cmd = new SqlCommand("Select * FROM Google WHERE id=@Id", conn))
            {
                if (id != null) cmd.Parameters.AddWithValue("@Id", id);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    accTo = (string)reader["AccesToken"];
                    refTo = (string) reader["RefreshToken"];
                    lifetime = (int)reader["Lifetime"];
                    //lifetime = 0;
                   // var temp = reader["Lifetime"];
                    createDate = (DateTime)reader["CreateDate"];
                    conn.Close();
                    return;
                }
            }
            accTo = "Fehler";  //out var need value
            refTo = "Fehler";
            lifetime = 4;
            createDate = DateTime.Now;
            conn.Close();
            
        }

        public void StoreCredential(string id, string accTo, string refTo, long lifetime, DateTime createDate)
        {
            conn.Open();

            using (SqlCommand cmd =
                new SqlCommand("INSERT INTO Google VALUES(" +
                               "@Id, @AccesToken, @RefreshToken, @Lifetime, @CreateDate)", conn))
            {
                if (id != null) cmd.Parameters.AddWithValue("@Id", id);
                if (accTo != null) cmd.Parameters.AddWithValue("@AccesToken", accTo);
                if (refTo != null) cmd.Parameters.AddWithValue("@RefreshToken", refTo);
                cmd.Parameters.AddWithValue("@Lifetime", lifetime);
                cmd.Parameters.AddWithValue("@CreateDate", createDate);

                cmd.ExecuteNonQuery();
            }

            conn.Close();
        }

        public void DeleteCredential(string id)
        {
            conn.Open();

            using (SqlCommand cmd = 
                new SqlCommand("DELETE FROM Google WHERE @Id = id", conn)) 
            {
                if (id != null) cmd.Parameters.AddWithValue("@Id", id);
                cmd.ExecuteNonQuery();
            }
            conn.Close();
        }

        public void UpdateCredential(string id, string accTo, string refTo, long lifetime, DateTime createDate)
        {
            conn.Open();

            using (SqlCommand cmd =
                new SqlCommand("UPDATE Google SET AccesToken = @AccesToken, RefreshToken=@RefreshToken, Lifetime=@Lifetime, CreateDate=@CreateDate "+
                               "Where @Id = id", conn))
            {
               if (id != null) cmd.Parameters.AddWithValue("@Id", id);
               if (accTo != null) cmd.Parameters.AddWithValue("@AccesToken", accTo);
               if (refTo != null) cmd.Parameters.AddWithValue("@RefreshToken", refTo);
               cmd.Parameters.AddWithValue("@Lifetime", lifetime);
               cmd.Parameters.AddWithValue("@CreateDate", createDate);
               cmd.ExecuteNonQuery();
            }
            conn.Close();
        }
    }
}
