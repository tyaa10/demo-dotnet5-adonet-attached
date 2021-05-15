﻿using System;
using System.Data.SqlClient;
using NickBuhro.Translit;

namespace first
{
    class Program
    {
        static void Main(string[] args)
        {
            // test changes
            using (SqlConnection connection =
                new SqlConnection(@"Data Source=localhost,1433\sql1;Initial Catalog=demo2021;User ID=sa;Password=Passw0rd%"))
                // new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=demo2021;Integrated Security=True"))
            {
                connection.Open();

                SqlCommand selectRoleCount = connection.CreateCommand();
                selectRoleCount.CommandText = "SELECT COUNT(*) FROM Roles";
                object roleCount = selectRoleCount.ExecuteScalar();
                Console.WriteLine($"Total row count: {roleCount}");

                if (roleCount == null || (int)roleCount == 0)
                {
                    SqlCommand initInsertRole = connection.CreateCommand();
                    initInsertRole.CommandText =
                        "INSERT INTO [dbo].[Roles] ([name]) VALUES ('User'), ('Admin'), (N'Контент менеджер');";
                    int insertedRoleCount = initInsertRole.ExecuteNonQuery();
                    Console.WriteLine($"Total inserted row count: {insertedRoleCount}");
                }
                ShowRoles(connection);
            }
        }
        private static void ShowRoles(SqlConnection connection)
        {
            SqlCommand selectRoles = connection.CreateCommand();
            selectRoles.CommandText = "SELECT * FROM Roles ORDER BY id";
            using (SqlDataReader reader =
                selectRoles.ExecuteReader())
            {
                int fieldCount = reader.FieldCount;
                for (int i = 0; i < fieldCount; i++)
                {
                    Console.Write(reader.GetName(i) + " ");
                }
                Console.WriteLine();
                while (reader.Read())
                {
                    Console.WriteLine(reader.GetInt32(0) + " " + reader.GetString(1));
                    foreach (Char item in reader.GetString(1))
                    {
                        // Console.WriteLine(item + 0);
                        // Console.WriteLine((ushort)item);
                        if ((ushort)item >= 1000)
                        {
                            Console.WriteLine(Transliteration.CyrillicToLatin(reader.GetString(1), Language.Russian));
                            break;
                        }
                    }
                }
            }
        }
    }
}