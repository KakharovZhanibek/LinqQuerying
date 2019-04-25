using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqQuerying
{
    public class NorthwindDbLinqQuerying
    {
        public void Task_1()
        {
            NorthwindEntities dbContext = new NorthwindEntities();

            var customerInfo = dbContext.Customers
                .ToList()
                .Select(p => new
                {
                    CompanyName = p.CompanyName,
                    Address = $"{p.Country}, {p.City}, {p.Address}"
                });

            var customerInfo2 = from p in dbContext.Customers.ToList()
                                select new
                                {
                                    CompanyName = p.CompanyName,
                                    Address = $"{p.Country}, {p.City}, {p.Address}"
                                };

            foreach (var item in customerInfo)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();

            foreach (var item in customerInfo2)
            {
                Console.WriteLine(item);
            }
        }

        public void Task_2()
        {
            NorthwindEntities dbContext = new NorthwindEntities();
            var OrderInfo = dbContext.Orders
                .Where(p => p.ShippedDate.HasValue)
                .ToList().Select(p => new
                {
                    OrderId = p.OrderID,
                    Address = $"{p.ShipCountry}, {p.ShipCity}, {p.ShipAddress}",
                    CountDays = (p.ShippedDate - p.OrderDate).Value.Days
                });
            var OrderInfo2 = from p in dbContext.Orders.ToList()
                             where p.ShippedDate.HasValue
                             select new
                             {
                                 OrderId = p.OrderID,
                                 Address = $"{p.ShipCountry}, {p.ShipCity}, {p.ShipAddress}",
                                 CountDays = (p.ShippedDate - p.OrderDate).Value.Days
                             };
            foreach (var item in OrderInfo)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            foreach (var item in OrderInfo2)
            {
                Console.WriteLine(item);
            }
        }

        public void Task_3()
        {
            NorthwindEntities dbContext = new NorthwindEntities();
            var ProductInfo = dbContext.Products
                .ToList().Select(p => new
                {
                    ProductName = p.ProductName,
                    ProductTotalCost = String.Format("{0:C}", (p.UnitsInStock * p.UnitPrice))
                });

            var ProductInfo2 = from p in dbContext.Products.ToList()
                               select new
                               {
                                   ProductName = p.ProductName,
                                   ProductTotalCost = p.UnitsInStock * p.UnitPrice
                               };
            foreach (var item in ProductInfo)
            {
                Console.WriteLine(item);
            }
            //Console.WriteLine();
            //Console.WriteLine();
            //Console.WriteLine();
            //foreach (var item in ProductInfo2)
            //{
            //    Console.WriteLine(item);
            //}
        }

        public void Task_4()
        {
            NorthwindEntities dbContext = new NorthwindEntities();
            var EmployeesInUSA = dbContext.Employees
                .Where(w => w.Country == "USA")
                .ToList().Select(p => new
                {
                    EmployeeFName = p.FirstName,
                    EmployeeLName = p.LastName
                });

            foreach (var item in EmployeesInUSA)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();

            var EmployeeInUSA2 = from p in dbContext.Employees.ToList()
                                 where p.Country == "USA"
                                 select p;

            var EmployeeInUSA22 = from p in dbContext.Employees.ToList()
                                  where p.Country == "USA"
                                  select new { p.FirstName, p.Address };

            foreach (var item in EmployeeInUSA2)
            {
                Console.WriteLine(item);
            }

            foreach (var item in EmployeeInUSA22)
            {
                Console.WriteLine(item);
            }
        }

        public void Task_5()
        {
            NorthwindEntities dbContext = new NorthwindEntities();
            var EmployeesInUSA = dbContext.Employees
                .ToList()
                .Where(w => (DateTime.Now - w.BirthDate).Value.Days / 365 > 50)
                .Select(p => new
                {
                    EmployeeFName = p.FirstName,
                    EmployeeLName = p.LastName,
                    EmployeeAge = (DateTime.Now - p.BirthDate).Value.Days / 365
                });

            foreach (var item in EmployeesInUSA)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();

            IEnumerable<Employee> EmployeeInUSA2 = from p in dbContext.Employees.ToList()
                                                   where (DateTime.Now - p.BirthDate).Value.Days / 365 > 50
                                                   select p;

            var EmployeeInUSA22 = from p in dbContext.Employees.ToList()
                                  where (DateTime.Now - p.BirthDate).Value.Days / 365 > 50
                                  select new { p.FirstName, p.Address };

            foreach (var item in EmployeeInUSA2)
            {
                Console.WriteLine(item);
            }

            foreach (var item in EmployeeInUSA22)
            {
                Console.WriteLine(item);
            }
        }

        public void Task_6()
        {
            //6.	Найти всех сотрудников, которые оформили заказы в Бельгию.
            NorthwindEntities dbContext = new NorthwindEntities();

            var emps = dbContext.Orders.Where(p => p.ShipCountry == "Belgium").Select(p => new
            {
                employee = p.Employee.FirstName + " " + p.Employee.LastName
            }).GroupBy(p => p.employee);

            foreach (var item in emps)
            {
                Console.WriteLine(item.Key);
            }

        }

        public void Task_7()
        {
            //7.	Для каждого продукта определить, популярен ли он. 
            //Продукт будет считаться популярным, если кол-во оставшегося товара на складе меньше, чем кол-во товара, оформленных в заказах.
            NorthwindEntities dbContext = new NorthwindEntities();

            var popProducts = dbContext.Products.Where(p => p.UnitsInStock < p.UnitsOnOrder).Select(p => p.ProductName);
            foreach (var item in popProducts)
            {
                Console.WriteLine(item);
            }
        }

        public void Task_8()
        {
            //8.	Для каждого сотрудника посчитать, сколько он обслужил заказов за  время работы в компании.
            NorthwindEntities dbContext = new NorthwindEntities();

            var ordersCntPerEmployee = from emp in dbContext.Employees
                                       join order in dbContext.Orders
                                       on emp.EmployeeID equals order.EmployeeID
                                       group emp by emp.FirstName + " " + emp.LastName
                                     into grp
                                       select new
                                       {
                                           Name = grp.Key,
                                           Cnt = grp.Count()
                                       };
            foreach (var item in ordersCntPerEmployee)
            {
                Console.WriteLine(item);
            }
        }

        public void Task_9()
        {
            //9.	Для каждого клиента посчитать, сколько он выплатил по всем заказам.
            NorthwindEntities dbContext = new NorthwindEntities();
            var sumPerCustomer = from cust in dbContext.Customers
                                 from order in dbContext.Orders.Where(p => p.CustomerID == cust.CustomerID)
                                 from detail in dbContext.Order_Details.Where(p => p.OrderID == order.OrderID)
                                 select new
                                 {
                                     cust.CompanyName,
                                     detail.Quantity,
                                     detail.UnitPrice
                                 } into s
                                 group s by new { s.CompanyName }
                                 into grp
                                 select new
                                 {
                                     Name = grp.Key.CompanyName,
                                     Sum = grp.Sum(p => p.Quantity * p.UnitPrice)
                                 };

            foreach (var item in sumPerCustomer)
            {
                Console.WriteLine(item);
            }
        }

        public void Task_10()
        {
            //10.	Для каждого продукта посчитать, сколько единиц товара было продано и сколько заработала компания на этом товаре.
            NorthwindEntities dbContext = new NorthwindEntities();
            var productsInfo = from product in dbContext.Products
                               from detail in dbContext.Order_Details.Where(p => p.ProductID == product.ProductID)
                               from order in dbContext.Orders.Where(p => p.OrderID == detail.OrderID)
                               select new
                               {
                                   product.ProductName,
                                   detail.Quantity,
                                   detail.UnitPrice
                               } into s
                               group s by new { s.ProductName } into grp
                               select new
                               {
                                   Name = grp.Key.ProductName,
                                   Cnt = grp.Count(),
                                   Amount = grp.Sum(p => p.Quantity * p.UnitPrice)
                               };

            foreach (var item in productsInfo)
            {
                Console.WriteLine(item);
            }
        }

        public void Task_11()
        {
            //11.	Для каждой страны посчитать кол-во клиентов в этой стране.
            NorthwindEntities dbContext = new NorthwindEntities();

            var customersPreCountry = from cust in dbContext.Customers
                                      select cust into s
                                      group s by new { s.Country } into g
                                      select new
                                      {
                                          Country = g.Key.Country,
                                          Cnt = g.Count()
                                      };

            foreach (var item in customersPreCountry)
            {
                Console.WriteLine(item);
            }
        }

        public void Task_13()
        {
            //13.	 Для каждого клиента найти его самый дорогой и самый дешевый его заказ.
            NorthwindEntities dbContext = new NorthwindEntities();

            var customersMinMax = from cust in dbContext.Customers
                                  from order in dbContext.Orders.Where(p => p.CustomerID == cust.CustomerID)
                                  from detail in dbContext.Order_Details.Where(p => p.OrderID == order.OrderID)
                                  select new
                                  {
                                      cust.CompanyName,
                                      detail.Quantity,
                                      detail.UnitPrice
                                  } into s
                                  group s by new { s.CompanyName } into g
                                  select new
                                  {
                                      Name = g.Key.CompanyName,
                                      Min = g.Min(p => p.Quantity * p.UnitPrice),
                                      Max = g.Max(p => p.Quantity * p.UnitPrice)
                                  };

            foreach (var item in customersMinMax)
            {
                Console.WriteLine(item);
            }
        }

        public void Task_14()
        {
            NorthwindEntities dbContext = new NorthwindEntities();

            var complaintManagerInfoPerOrder1 =
                from order in dbContext.Orders
                select new
                {
                    OrderId = order.OrderID,
                    ComplaintManagerId = (
                        from emp in dbContext.Employees
                        where emp.EmployeeID == order.EmployeeID
                        select emp)
                        .FirstOrDefault().ReportsTo
                };

            var complaintManagerInfoPerOrder2 =
                from order in dbContext.Orders
                join employee in dbContext.Employees
                on order.EmployeeID equals employee.EmployeeID
                select new { order.OrderID, employee.ReportsTo };

            var complaintManagerInfoPerOrder3 =
                dbContext.Orders.Select(p => new
                {
                    OrderId = p.OrderID,
                    ComplaintManagerId =
                        p.Employee.ReportsTo.HasValue ?
                        p.Employee.Employee1.EmployeeID : p.EmployeeID
                });

            foreach (var item in complaintManagerInfoPerOrder3)
            {
                Console.WriteLine(item);
            }
        }

        public void Task_15()
        {
            //15.	 Вывести ТОП-3 городов, в которые доставляется наибольшее кол-во заказов.
            NorthwindEntities dbContext = new NorthwindEntities();

            var top3 = (from order in dbContext.Orders
                        group order by new { order.ShipCity } into g
                        select new
                        {
                            g.Key.ShipCity,
                            Cnt = g.Count()
                        } into s
                        orderby s.Cnt descending
                        select s).Take(3);

            foreach (var item in top3)
            {
                Console.WriteLine(item);
            }
        }

        public void Task_17()
        {
            //Определить, существуют ли заказы, в которых клиент заказал товар, который поставляется компанией, находящейся в том же городе, что и клиент.
            NorthwindEntities dbContext = new NorthwindEntities();

            var customerSupplierCity = from order in dbContext.Orders
                                       from cust in dbContext.Customers.Where(p => p.CustomerID == order.CustomerID)
                                       from detail in dbContext.Order_Details.Where(p => p.OrderID == order.OrderID)
                                       from prod in dbContext.Products.Where(p => p.ProductID == detail.ProductID)
                                       from sup in dbContext.Suppliers.Where(p => p.SupplierID == prod.SupplierID)
                                       where sup.City == cust.City
                                       select new
                                       {
                                           order.OrderID,
                                           SupplierCiry = sup.City,
                                           CustomerCity = cust.City
                                       };

            foreach (var item in customerSupplierCity)
            {
                Console.WriteLine(item);
            }
        }

        public void Task_18()
        {
            //18.Дать информацию о сотруднике и о клиенте для заказов, которые были доставлены компанией “Speedy Express” для клиентов в Брюсселе.
            NorthwindEntities dbContext = new NorthwindEntities();

            var empCustInfoInBruxelles = from order in dbContext.Orders
                                         from cust in dbContext.Customers.Where(p => p.CustomerID == order.CustomerID)
                                         from emp in dbContext.Employees.Where(p => p.EmployeeID == order.EmployeeID)
                                         from ship in dbContext.Shippers.Where(p => p.ShipperID == order.ShipVia)
                                         where ship.CompanyName == "Speedy Express" && cust.City == "Bruxelles"
                                         select new
                                         {
                                             EmployeeName = emp.FirstName + " " + emp.LastName,
                                             CustomerName = cust.CompanyName
                                         };

            foreach (var item in empCustInfoInBruxelles)
            {
                Console.WriteLine(item);
            }
        }


    }


    class Program
    {
        static void Main(string[] args)
        {
            NorthwindDbLinqQuerying LQ = new NorthwindDbLinqQuerying();

            Console.WriteLine("Выберите задание");
            switch (GetPunctMenu())
            {
                case 1:
                    {
                        Console.Clear();
                        LQ.Task_1();
                        Console.WriteLine("Нажмите любую клавишу...");
                        Console.ReadKey();
                        Console.Clear();
                    }
                    break;
                case 2:
                    {
                        Console.Clear();
                        LQ.Task_2();
                        Console.WriteLine("Нажмите любую клавишу...");
                        Console.ReadKey();
                        Console.Clear();
                    }
                    break;
                case 3:
                    {
                        Console.Clear();
                        LQ.Task_3();
                        Console.WriteLine("Нажмите любую клавишу...");
                        Console.ReadKey();
                        Console.Clear();
                    }
                    break;
                case 4:
                    {
                        Console.Clear();
                        LQ.Task_4();
                        Console.WriteLine("Нажмите любую клавишу...");
                        Console.ReadKey();
                        Console.Clear();
                    }
                    break;
                case 5:
                    {
                        Console.Clear();
                        LQ.Task_5();
                        Console.WriteLine("Нажмите любую клавишу...");
                        Console.ReadKey();
                        Console.Clear();
                    }
                    break;
                case 6:
                    {
                        Console.Clear();
                        LQ.Task_6();
                        Console.WriteLine("Нажмите любую клавишу...");
                        Console.ReadKey();
                        Console.Clear();
                    }
                    break;
                case 7:
                    {
                        Console.Clear();
                        LQ.Task_7();
                        Console.WriteLine("Нажмите любую клавишу...");
                        Console.ReadKey();
                        Console.Clear();
                    }
                    break;
                case 8:
                    {
                        Console.Clear();
                        LQ.Task_8();
                        Console.WriteLine("Нажмите любую клавишу...");
                        Console.ReadKey();
                        Console.Clear();
                    }
                    break;
                case 9:
                    {
                        Console.Clear();
                        LQ.Task_9();
                        Console.WriteLine("Нажмите любую клавишу...");
                        Console.ReadKey();
                        Console.Clear();
                    }
                    break;
                case 10:
                    {
                        Console.Clear();
                        LQ.Task_10();
                        Console.WriteLine("Нажмите любую клавишу...");
                        Console.ReadKey();
                        Console.Clear();
                    }
                    break;
                case 11:
                    {
                        Console.Clear();
                        LQ.Task_11();
                        Console.WriteLine("Нажмите любую клавишу...");
                        Console.ReadKey();
                        Console.Clear();
                    }
                    break;
                case 13:
                    {
                        Console.Clear();
                        LQ.Task_13();
                        Console.WriteLine("Нажмите любую клавишу...");
                        Console.ReadKey();
                        Console.Clear();
                    }
                    break;
                case 14:
                    {
                        Console.Clear();
                        LQ.Task_14();
                        Console.WriteLine("Нажмите любую клавишу...");
                        Console.ReadKey();
                        Console.Clear();
                    }
                    break;
                case 15:
                    {
                        Console.Clear();
                        LQ.Task_15();
                        Console.WriteLine("Нажмите любую клавишу...");
                        Console.ReadKey();
                        Console.Clear();
                    }
                    break;
                case 17:
                    {
                        Console.Clear();
                        LQ.Task_17();
                        Console.WriteLine("Нажмите любую клавишу...");
                        Console.ReadKey();
                        Console.Clear();
                    }
                    break;
                case 18:
                    {
                        Console.Clear();
                        LQ.Task_18();
                        Console.WriteLine("Нажмите любую клавишу...");
                        Console.ReadKey();
                        Console.Clear();
                    }
                    break;
            }
        }
        public static int GetPunctMenu()
        {
            return Int32.Parse(Console.ReadLine());
        }
    }
}
