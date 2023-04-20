using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_Systems
{
    internal class Program
    {
        static Order order = new Order();
        static int selectedTableId;
        static Table[] tables = new Table[5];
        static Product[] products = new Product[12];
        static List<Order> waitingOrders = new List<Order>();

        static void Main(string[] args)
        {

            //Giriş işlemleri burada olacak
            Admin admin = new Admin();
            admin.adminUsername = "admin";
            admin.SetPassword("1234");


            products[0] = new Product { productId = 1, productName = "Tavuk Döner", productPrice = 45 };
            products[1] = new Product { productId = 2, productName = "Barbekü Soslu Tavuk", productPrice = 115 };
            products[2] = new Product { productId = 3, productName = "İskender Et Dönerden", productPrice = 135 };
            products[3] = new Product { productId = 4, productName = "Pizza Margarita", productPrice = 120 };
            products[4] = new Product { productId = 5, productName = "Pizza Karışık", productPrice = 165 };
            products[5] = new Product { productId = 6, productName = "Pizza Mexicano", productPrice = 165 };
            products[6] = new Product { productId = 7, productName = "Çay", productPrice = 45 };
            products[7] = new Product { productId = 8, productName = "Türk Kahvesi", productPrice = 35 };
            products[8] = new Product { productId = 9, productName = "Espresso", productPrice = 45 };
            products[9] = new Product { productId = 10, productName = "Pepsi Max", productPrice = 20 };
            products[10] = new Product { productId = 11, productName = "IceTea Limon", productPrice = 15 };
            products[11] = new Product { productId = 12, productName = "Soda", productPrice = 12 };


            tables[0] = new Table { tableId = 1, isTableOccupied = false };
            tables[1] = new Table { tableId = 2, isTableOccupied = false };
            tables[2] = new Table { tableId = 3, isTableOccupied = false };
            tables[3] = new Table { tableId = 4, isTableOccupied = false };
            tables[4] = new Table { tableId = 5, isTableOccupied = false };


            //bazı gerekli variables
            bool pwLoop = false;
            //Menülerle ilgili geçiş vb. bu kısmın altında olacak.
            Console.WriteLine("Burrino Restorantına Hoşgeldiniz!");
            Console.WriteLine("Devam etmek için lütfen ENTER tuşuna basın");
            Console.ReadLine();
            Console.Clear();
            Console.WriteLine("------------Yönetim Paneli Giriş------------");
            while (!pwLoop)
            {
                Console.Write("Lütfen Kullanıcı Adınızı Giriniz:");
                string username = Console.ReadLine();

                Console.Write("Lütfen şifrenizi giriniz:");
                string password = Console.ReadLine();

                if (username == admin.adminUsername && admin.CheckPassword(password))
                {

                    pwLoop = true;
                    Console.WriteLine("Giriş başarılı!\nDevam etmek için ENTER tuşuna basın");
                    Console.ReadLine();
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("------------Yönetim Paneli Giriş------------");
                    Console.WriteLine("Kullanıcı adı veya şifre yanlış!");
                }
            }
            Mainmenu();

        }

        class Product
        {
            public int productId { get; set; }
            public string productName { get; set; }

            public double productPrice { get; set; }


        }
        class Order
        {
            public Product selectedProduct { get; set; }
            public int tableId { get; set; }
            public int quantity { get; set; }

            

        }
        class Table
        {
            public int tableId { get; set; }
            public double totalPrice { get; set; }
            public bool isTableOccupied { get; set; }
            public List<Order> products { get; set; } = new List<Order>();

                
        }
        class Admin //Giriş sistemi için class
        {
            public string adminUsername { get; set; }
            private string adminPassword;
            public bool CheckPassword(string password)
            {
                return password == adminPassword;
            }

            public void SetPassword(string password)
            {
                adminPassword = password;
            }
        }
        static void Mainmenu()
        {
            Console.Clear();
            Console.WriteLine("##########################################");
            Console.WriteLine("#             ANA MENÜ                   #");
            Console.WriteLine("#  ------------------------------------  #");
            Console.WriteLine("#  Masa Aç        [1]                    #");
            Console.WriteLine("#  Masa İşlem     [2]                    #");
            Console.WriteLine("#  Masa Hesap     [3]                    #");
            Console.WriteLine("#  Kasa İşlemleri [4]                    #");
            Console.WriteLine("#  ------------------------------------  #");
            Console.WriteLine("#  ÇIKIŞ YAP      [0]                    #");
            Console.WriteLine("##########################################");
            Console.Write("Seçiminiz:");
            string opt = Console.ReadLine();
            switch (opt)
            {
                case "0":
                    Console.WriteLine("Çıkış yapmak istediğinizden emin misiniz? E/H");
                    bool optLoop = false;
                    while (!optLoop)
                    {
                        opt = Console.ReadLine().ToLower();
                        if (opt == "e") { optLoop = true; Console.WriteLine("Programdan çıkış yapılıyor..."); return; }
                        else if (opt == "h") { optLoop = true; Console.WriteLine("Anamenüye geri dönülüyor..."); Mainmenu(); }
                        else { Console.Clear(); Console.WriteLine("Hatalı bir seçim yaptınız lütfen sadece E/H olarak giriniz"); }
                    }
                    break;
                case "1":
                    SelectTable();
                    break;
                case "2":
                    TableOperation();
                    break;
                case "3":

                    break;
                case "4":

                    break;
                default:
                    Console.WriteLine("Hatalı bir giriş yaptınız tekrar deneyin:");
                    break;

            }



        }
        static void SelectTable()
        {

            Console.Clear();
            Console.WriteLine("-------------------------------------");
            Console.WriteLine("               MASA AÇ               ");
            Console.WriteLine("-------------------------------------");

            foreach (Table table in tables)
            {
                string tableStatus = table.isTableOccupied ? "DOLU" : "BOŞ";
                Console.WriteLine($"{table.tableId}. Masa // Durumu: {tableStatus}");

            }
            Console.WriteLine("-------------------------------------");
            Console.Write("Masa seçiniz: ");
            selectedTableId = int.Parse(Console.ReadLine());
            Table selectedTable = tables.FirstOrDefault(x => x.tableId == selectedTableId);
            if (selectedTable != null)
            {
                Console.Clear();
                Console.WriteLine("-------------------------------------");
                Console.WriteLine("               MASA AÇ               ");
                Console.WriteLine("-------------------------------------");
                Console.WriteLine("               MENU                  ");
                Console.WriteLine("-------------------------------------");
                foreach (Product p in products)
                {
                    Console.WriteLine($"Ürün ID:{p.productId} Ürün Adı: {p.productName} Fiyatı: {p.productPrice}");
                }
                Console.WriteLine("-------------------------------------");
                Console.WriteLine($"      {selectedTableId}.MASA - SİPARİŞLERİ   ");
                Console.WriteLine("-------------------------------------");
                
                foreach (Order o in waitingOrders)
                {
                    
                    Console.WriteLine($"{o.tableId}.Masa: Beklemedeki Sipariş // Ürün: {o.selectedProduct.productName} // Adet: {o.quantity} // Ücreti: {o.selectedProduct.productPrice * o.quantity} TL");
                }
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("ONAYLA [SPACE]\nAna Menü [ESC]\nGeri Git [Sol OK]");
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                Console.WriteLine($"Masa bulunamadı: {selectedTableId}");
            }
            
        }
        static void TableOperation()
        {
            Console.Clear();
            Console.WriteLine("-------------------------------------");
            Console.WriteLine("              MASA İŞLEM             ");
            Console.WriteLine("-------------------------------------");

            foreach (Table t in tables)
            {
                string tableStatus = t.isTableOccupied ? "DOLU" : "BOŞ";

                Console.WriteLine($"{t.tableId}. Masa // Durumu: {tableStatus}");

            }
            Console.WriteLine("Ana Menü [ESC]");
            Console.Write("Masa ID giriniz:");
            int selectedTableId = int.Parse(Console.ReadLine());
            Table selectedTable = tables.FirstOrDefault(x => x.tableId == selectedTableId);
            basa:

            if (selectedTable != null)
            {
                Console.WriteLine("-------------------------------------");
                Console.WriteLine($"      {selectedTableId}.MASA        ");
                Console.WriteLine("-------------------------------------");
                Console.WriteLine("Mevcut Siparişler");
                if(selectedTable.products.Count>0)
                {
                    foreach (Order o in selectedTable.products)
                    {
                        int i = 0;
                        Console.WriteLine($"{i + 1}.Ürün:{o.selectedProduct.productName} Ücreti: ({o.selectedProduct.productPrice} TL)");
                        i++;
                    }
                }
                else
                {
                    Console.WriteLine("Henüz bir sipariş verilmemiş");
                }
                Console.WriteLine("-------------------------------------");

            }
            else
            {
                Console.WriteLine($"Masa bulunamadı: {selectedTableId}");
            }
            Console.WriteLine("Sipariş Ekle [1]");
            Console.WriteLine("Sipariş Sil  [2]");
            Console.WriteLine("ONAYLA [SPACE]\nAna Menü [ESC]\nGeri Git [Sol OK]");
            Console.Write("İşleminiz:");
            int selection = int.Parse(Console.ReadLine());
            Console.Clear();
            if (selection == 1)
            {
                Console.WriteLine("-------------------------------------");
                Console.WriteLine($"      {selectedTableId}.MASA  - SİPARİŞ EKLE     ");
                Console.WriteLine("-------------------------------------");
                foreach (Product p in products)
                {
                    Console.WriteLine($"Ürün ID:{p.productId} Ürün Adı: {p.productName} Ücreti: {p.productPrice}");
                }
                Console.WriteLine("-------------------------------------");
                Console.WriteLine("             SİPARİŞLER              ");
                Console.WriteLine("-------------------------------------");
                foreach (Order o in waitingOrders)
                {
                    
                    Console.WriteLine($"{o.tableId}.Masa: Beklemedeki Sipariş // Ürün: {o.selectedProduct.productName} // Adet: {o.quantity} // Ücreti: {o.selectedProduct.productPrice*o.quantity} TL");
                }
                Console.WriteLine("Ürün idsi giriniz:");
                int selectedProductId = int.Parse(Console.ReadLine());
                Console.WriteLine("Ürün adetini giriniz:");
                int selectedProductQuantity = int.Parse (Console.ReadLine());
                var order = new Order{ quantity = selectedProductQuantity,selectedProduct = products.FirstOrDefault(x => x.productId == selectedProductId),tableId = selectedTableId };
                waitingOrders.Add(order);
                Console.WriteLine($"{order.tableId}. Masa için {selectedProductQuantity} adet {order.selectedProduct.productName} siparişi bekleme listesine eklendi. Sipariş ID: {waitingOrders.IndexOf(order) + 1}");
                Mainmenu();
                goto basa;



            }
            else if (selection == 2)
            {
                Console.Clear();
                Console.WriteLine("-------------------------------------");
                Console.WriteLine($"      {selectedTableId}.MASA  - SİPARİŞ SİL     ");
                Console.WriteLine("-------------------------------------");
                Console.WriteLine("BURASI DÜZENLENECEK*******");
            }
            else
            {
                Console.WriteLine("Hatalı bir giriş yaptınız!");
            }
        }
        static void AddOrderToWaitingList(Table selectedTable, Product selectedProduct, int quantity)
{
    waitingOrders.Add(new Order { selectedProduct = selectedProduct, quantity = quantity });
    Console.WriteLine($"{selectedTable.tableId}. Masa için {quantity} adet {selectedProduct.productName} siparişi bekleme listesine eklendi.");
}

    }
}
