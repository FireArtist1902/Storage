using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Threading.Channels;

namespace Storage
{
    internal class Program
    {
        public static string StrConn => @"Data Source=DESKTOP-TD7GATJ\SQLEXPRESS;Initial Catalog=Storage;Integrated Security=True;Connect Timeout=30;";
        static void Main(string[] args)
        {
            using (SqlConnection con = new SqlConnection(StrConn))
            {
                bool IsWorking = true;
                while (IsWorking)
                {
                    Console.WriteLine("Ви успішно доєднались, виберіть дію");
                    Console.WriteLine("0.Завершення програми");
                    Console.WriteLine("1.Відображення всієї інформації про товар.");
                    Console.WriteLine("2.Відображення всіх типів товарів.");
                    Console.WriteLine("3.Відображення всіх постачальників.");
                    Console.WriteLine("4.Показати товар з максимальною кількістю.");
                    Console.WriteLine("5.Показати товар з мінімальною кількістю.");
                    Console.WriteLine("6.Показати товар з мінімальною собівартістю.");
                    Console.WriteLine("7.Показати товар з максимальною собівартістю.");
                    Console.WriteLine("8.Показати товари заданої категорії.");
                    Console.WriteLine("9.Показати товари заданого постачальника.");
                    Console.WriteLine("10.Показати товар, який знаходиться на складі найдовше з усіх.");
                    int? choice = Convert.ToInt32(Console.ReadLine());

                    switch (choice)
                    {
                        case 0:
                            IsWorking = false;
                            break;
                        case 1:
                            Display(con, "Select * From Product", "Name", "TypeId",
                            "Count", "CostPrice", "DateOfDelivery");
                            break;
                        case 2:
                            Display(con, "Select * From ProdType", "Name");
                            break;
                        case 3:
                            Display(con, "Select * From ProdProvider", "Name");
                            break;
                        case 4:
                            Display(con, "Select Top 1 Name, Max(Count) as 'Max count' From Product Group by Name Order by [Max count] desc", "Name", "Max count");
                            break;
                        case 5:
                            Display(con, "Select Top 1 Name, Min(Count) as 'Min count' From Product Group by Name Order by [Min count] asc", "Name", "Min count");
                            break;
                        case 6:
                            Display(con, "Select Top 1 Name, Min(CostPrice) as 'Min cost price' From Product Group by Name Order by [Min cost price] asc", "Name", "Min cost price");
                            break;
                        case 7:
                            Display(con, "Select Top 1 Name, Max(CostPrice) as 'Max cost price' From Product Group by Name Order by [Max cost price] desc", "Name", "Max cost price");
                            break;
                        case 8:
                            Console.Clear();
                            Console.WriteLine("Введіть потрібний Вам тип товару");
                            Display(con, $"Select Product.Name From Product JOIN ProdType ON Product.TypeId = ProdType.Id Where ProdType.Name = '{Console.ReadLine()}'", "Name");
                            break;
                        case 9:
                            Console.Clear();
                            Console.WriteLine("Введіть потрібного Вам постачальника");
                            Display(con, $"Select Product.Name From Product join ProductToProvider on Product.Id = ProductToProvider.ProductId" +
                                $" join ProdProvider on ProductToProvider.ProviderId = ProdProvider.Id Where ProdProvider.Name = '{Console.ReadLine()}'", "Name");
                            break;
                        case 10:
                            Display(con, "Select Top 1 Name, Min(DateOfDelivery) as 'Max date of delivery' From Product Group by Name Order by [Max date of delivery] asc", "Name", "Max date of delivery");
                            break;
                        default:
                            break;
                    }


                    con.Close();
                }
            }
        }

         static void Display(SqlConnection con, string query, params string[] columns)
        {
            Console.Clear();

            var cmd = new SqlCommand(query, con);

            con.Open();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    if (columns[0] != "")
                    {

                        for (int i = 0; i < columns.Length; i++)
                        {
                            if (i != columns.Length - 1)
                            {
                                Console.Write($"{reader[columns[i]]}; ");
                            }
                            else
                            {
                                Console.Write($"{reader[columns[i]]}");
                            }
                        }
                        Console.WriteLine();
                    }
                }
            }
            Console.WriteLine("Для продовження натсніть ENTER");
            Console.ReadLine();
            Console.Clear();

        }
    }
}