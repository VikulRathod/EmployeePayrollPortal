using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace EmployeeManagement.Models
{
    public class SqlDataHelper
    {
        public List<string> GetEmployeeColumnNames()
        {
            List<string> columns = new List<string>();

            string connectionString = ConfigurationManager.ConnectionStrings["EmployeeManagementDB"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("usp_GetEmployeeTableColumns", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var column = reader["Column_Name"].ToString();
                    columns.Add(column);
                }
            }
            return columns;
        }
    }
}