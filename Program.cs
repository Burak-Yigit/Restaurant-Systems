using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Net.Http;
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
        static int oQuantity = 1; // sipariş ID oluşturmak için
        static int orderRemovalSelection = 0;

        //Masa tutarları için
        static double totalEarnedCash = 0;
        static double totalEarnedTip = 0;

        static void Main(string[] args)
        {

            //Giriş işlemleri ile ilgili bilgiler
            Admin admin = new Admin();
            admin.adminUsername = "admin";
            admin.SetPassword("1234");

            //ürün listesi
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

            //masa listesi
            tables[0] = new Table { tableId = 1 };
            tables[1] = new Table { tableId = 2 };
            tables[2] = new Table { tableId = 3 };
            tables[3] = new Table { tableId = 4 };
            tables[4] = new Table { tableId = 5 };


            //bazı gerekli variables
            bool pwLoop = false;
            //Menülerle ilgili geçiş vb. bu kısmın altında olacak.
            Console.WriteLine("##########################################");
            Console.WriteLine("#                                        #");
            Console.WriteLine("#        Burrino Restoran Adisyon        #");
            Console.WriteLine("#         Sistemine Hoşgeldiniz!         #");
            Console.WriteLine("#   Devam etmek için ENTER tuşuna basın  #");
            Console.WriteLine("#                                        #");
            Console.WriteLine("##########################################");
            Console.ReadLine();
            Console.Clear();
            Console.WriteLine("------------Yönetim Paneli Giriş------------");
            while (!pwLoop)
            {
                Console.Write("Lütfen Kullanıcı Adınızı Giriniz:");
                string username = Console.ReadLine();

                Console.Write("Lütfen şifrenizi giriniz:");
                string password = Console.ReadLine();

                if (username == admin.adminUsername && admin.CheckPassword(password))  //kullanici adi ve şifre kontrolünü sağlayıp döngüyü bitiriyor.
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
            Mainmenu(); //yönetici paneline başarıyla giriş yapılmışsa anamenüye yönlendirilir.

        }


        static void Mainmenu() //anamenü içeriği
        {

            bool menuLoop = false;
            while (!menuLoop)
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
                Console.WriteLine("Seçiminizi tuşlayın");

                ConsoleKeyInfo optKey = Console.ReadKey();
                
                if (optKey.Key == ConsoleKey.D1 || optKey.Key == ConsoleKey.NumPad1)
                {
                    menuLoop = true;
                    SelectTable();
                }
                else if (optKey.Key == ConsoleKey.D2 || optKey.Key == ConsoleKey.NumPad2)
                {
                    menuLoop = true;
                    TableOperation();
                }
                else if (optKey.Key == ConsoleKey.D3 || optKey.Key == ConsoleKey.NumPad3)
                {
                    menuLoop = true;
                    TableBill();
                }
                else if (optKey.Key == ConsoleKey.D4 || optKey.Key == ConsoleKey.NumPad4)
                {
                    menuLoop = true;
                    CashierOperations();
                }
                else if (optKey.Key == ConsoleKey.D0 || optKey.Key == ConsoleKey.NumPad0)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("Çıkış yapmak istediğinizden emin misiniz? [E]vet/[H]ayır");
                    Console.ForegroundColor = ConsoleColor.White;
                    menuLoop = true;
                    bool optLoop = false;
                    while (!optLoop)
                    {
                        optKey = Console.ReadKey();
                        if (optKey.Key == ConsoleKey.E) { optLoop = true; Console.WriteLine("Programdan çıkış yapılıyor..."); return; }
                        else if (optKey.Key == ConsoleKey.H) { optLoop = true; Console.WriteLine("Anamenüye geri dönülüyor..."); Mainmenu(); }
                        else { Console.Clear(); Console.WriteLine("Hatalı bir seçim yaptınız lütfen sadece E/H olarak giriniz"); }
                    }
                }
                else
                {
                    Mainmenu();
                }




            }

        }
        static void SelectTable()
        {
            bool sTable = false;
            while(!sTable)
            {
                try
                {
                    Console.Clear();
                    Console.WriteLine("-------------------------------------");
                    Console.WriteLine("               MASA AÇ               ");
                    Console.WriteLine("-------------------------------------");

                    for (int i = 1; i <= 5; i++)
                    {
                        Table table = tables.FirstOrDefault(t => t.tableId == i);
                        string tableStatus = table.products.Any(p => p.Ready) ? "Dolu" : "Boş";
                        Console.WriteLine($"{i}. Masa // Durumu: {tableStatus}");
                    }
                    Console.WriteLine("-------------------------------------");
                    Console.Write("Masa seçiniz: ");
                    selectedTableId = int.Parse(Console.ReadLine());
                    if (selectedTableId > 0 && selectedTableId <= 5)
                    {
                        
                        sTable = true;
                    }
                    else 
                    { 
                        Console.WriteLine("Böyle bir masa bulunamadı. Tekrar giriniz.");
                    }
                   
                }
                catch
                {
                    Console.WriteLine("Hatalı bir giriş yaptınız!");
                }
            }
            Table selectedTable = tables.FirstOrDefault(x => x.tableId == selectedTableId);
            orders1:
            orders2:
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
                Console.WriteLine($"  {selectedTableId}.MASA - BEKLEYEN SİPARİŞLERİ   ");
                Console.WriteLine("-------------------------------------");

                foreach (var o in selectedTable.products.Where(x => !x.Ready))
                {

                    Console.WriteLine($"Sipariş ID: {o.orderId} {o.tableId}.Masa: Beklemedeki Sipariş // Ürün: {o.selectedProduct.productName} // Adet: {o.quantity} // Ücreti: {o.selectedProduct.productPrice * o.quantity} TL");
                }
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("ONAYLA [SPACE]\nYeni Sipariş Ekle [Yukarı OK]\nAna Menü [ESC]\nGeri Git [Sol OK]");
                Console.ForegroundColor = ConsoleColor.White;


                if (Console.ReadKey().Key == ConsoleKey.Spacebar)
                {
                    foreach (Order o in selectedTable.products.Where(x => !x.Ready))
                    {
                        o.Ready = true;
                    }
                    Console.WriteLine("Tüm bekleyen siparişler hazırlandı!");
                    Console.WriteLine("Enter'a basın");
                    Console.ReadLine();
                    goto orders1;
                }
                else if (Console.ReadKey().Key == ConsoleKey.UpArrow)
                {
                    Console.Clear();
                    AddOrderToTable(selectedTable);
                }
                else if (Console.ReadKey().Key == ConsoleKey.LeftArrow)
                {
                    SelectTable();
                }
                else if (Console.ReadKey().Key == ConsoleKey.Escape)
                {
                    Mainmenu();
                }
                else
                { goto orders2; }


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

            for (int i = 1; i <= 5; i++)
            {
                Table table = tables.FirstOrDefault(t => t.tableId == i);
                string tableStatus = table.products.Any(p => p.Ready) ? "Dolu" : "Boş";
                Console.WriteLine($"{i}. Masa // Durumu: {tableStatus}");
            }
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Masa ID girmek için [Yukarı Yön]");
            Console.WriteLine("Ana Menü [ESC]");
            Console.ForegroundColor = ConsoleColor.White;
            ConsoleKeyInfo optKey = Console.ReadKey();

            if (optKey.Key == ConsoleKey.UpArrow)
            {
                
                bool tOpLoop = false;
                while(!tOpLoop)
                {
                    try
                    {
                        Console.Write("Lütfen Masa ID giriniz:");
                        selectedTableId = int.Parse(Console.ReadLine());

                        if (selectedTableId>0 && selectedTableId <=5)
                        {

                            tOpLoop = true;
                        }
                        else
                        {
                            Console.WriteLine("Böyle bir masa bulunamadı. Tekrar giriniz.");
                        }
                    }
                    catch
                    {

                        Console.WriteLine("Sadece sayı değeri giriniz");
                    }
                }
            }
            else if(optKey.Key == ConsoleKey.Escape)
            {
                Mainmenu();
            }
            else
            {
                TableOperation();
            }

            
            Table selectedTable = tables.FirstOrDefault(x => x.tableId == selectedTableId);

            tOp1:
            
            if (selectedTable != null)
            {
                Console.Clear();
                Console.WriteLine("-------------------------------------");
                Console.WriteLine($"               {selectedTableId}.MASA        ");
                Console.WriteLine("-------------------------------------");
                Console.WriteLine("Onaylanan Siparişler");
               
                double totalEarnedInaTable = 0;
                if (selectedTable.products.Count > 0)
                {
                    foreach (var o in selectedTable.products.Where(x => x.Ready))
                    {

                        Console.WriteLine($"{selectedTable.tableId}.Masa - Ürün:{o.selectedProduct.productName} Adet: {o.quantity} Ücreti: ({o.selectedProduct.productPrice*o.quantity} TL)");
                        totalEarnedInaTable += o.selectedProduct.productPrice * o.quantity;

                    }
                    Console.WriteLine($"Siparişlerin Tutarı:{totalEarnedInaTable} TL");
                }
                else
                {
                    Console.WriteLine("Henüz bir sipariş verilmemiş");
                }
                Console.WriteLine("-------------------------------------");
                Console.WriteLine("Bekleyen Siparişler");
                if(selectedTable.products.Count>0)
                {
                    foreach (var o in selectedTable.products.Where(x => !x.Ready))
                    {

                        Console.WriteLine($"Sipariş ID: {o.orderId} {o.tableId}.Masa: Beklemedeki Sipariş // Ürün: {o.selectedProduct.productName} // Adet: {o.quantity} // Ücreti: {o.selectedProduct.productPrice * o.quantity} TL");
                    }
                }
                else 
                {
                    Console.WriteLine("Seçili masaya ait bekleyen bir sipariş yok.");
                }
            }
            else
            {
                Console.WriteLine($"Masa bulunamadı: {selectedTableId}");
            }
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Sipariş Ekle [1]");
            Console.WriteLine("Sipariş Sil  [2]");
            Console.WriteLine("ONAYLA [SPACE]\nAna Menü [ESC]\nGeri Git [Sol OK]");
            Console.ForegroundColor = ConsoleColor.White;
            ConsoleKeyInfo optKey2 = Console.ReadKey();
            if (optKey2.Key == ConsoleKey.D1 || optKey2.Key == ConsoleKey.D1)
            {
                AddOrderToTable(selectedTable);
                goto tOp1;
            }
            else if (optKey2.Key == ConsoleKey.D2 || optKey2.Key == ConsoleKey.D2)
            {
                DeleteOrderFromTable(selectedTable);
                goto tOp1;
            }
            else if (optKey2.Key == ConsoleKey.Spacebar)
            {
                
                    foreach (Order o in selectedTable.products.Where(x => !x.Ready))
                    {
                        o.Ready = true;
                    }
                    Console.WriteLine("Tüm bekleyen siparişler hazırlandı!");
                    Console.WriteLine("Enter'a basın");
                    Console.ReadLine();
                    goto tOp1;
                
            }
            else if (optKey2.Key == ConsoleKey.Escape)
            {
                Mainmenu();
            }
            else if (optKey2.Key == ConsoleKey.LeftArrow)
            {
                goto tOp1;
            }
            else
            {
                goto tOp1;
            }
            
        }
        static void AddOrderToTable(Table selectedTable) //Masaya sipariş eklemek için method
        {
            
            Console.Clear();
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
            foreach (var o in selectedTable.products.Where(x => !x.Ready)) //beklemedeki siparişleri gösterir
            {

                Console.WriteLine($"Sipariş ID: {o.orderId} {o.tableId}.Masa: Beklemedeki Sipariş // Ürün: {o.selectedProduct.productName} // Adet: {o.quantity} // Ücreti: {o.selectedProduct.productPrice * o.quantity} TL");
            }
            bool orderLoop = false;
            int selectedProductId = 0;
            int selectedProductQuantity=0;
            while (!orderLoop)
            {
                try
                {
                    
                    Console.WriteLine("Ürün idsi giriniz:");
                    selectedProductId = int.Parse(Console.ReadLine());
                    if(selectedProductId >=1 && selectedProductId<=12)
                    {

                        Console.WriteLine("Ürün adetini giriniz:");
                        selectedProductQuantity = int.Parse(Console.ReadLine());
                        if(selectedProductQuantity >0) 
                        {
                            orderLoop = true;
                        }
                    }
                    else { Console.WriteLine("Hatalı bir Ürün Id'si girdiniz.");
                    }
                    
                }
                catch
                {
                    Console.WriteLine("Hatalı bir giriş yaptınız!");
                }
            }

            var order = new Order { quantity = selectedProductQuantity, selectedProduct = products.FirstOrDefault(x => x.productId == selectedProductId), tableId = selectedTable.tableId, Ready = false, orderId = oQuantity };
            selectedTable.products.Add(order);
            oQuantity++;
            Console.WriteLine($"Sipariş ID: {order.orderId} // {order.tableId}. Masa için {selectedProductQuantity} adet {order.selectedProduct.productName} siparişi bekleme listesine eklendi.");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Yeni Sipariş Ekle [Yukarı OK]\nAna Menü [ESC]");
            Console.ForegroundColor = ConsoleColor.White;

            ConsoleKeyInfo optKey3 = Console.ReadKey();
            
           if (optKey3.Key == ConsoleKey.UpArrow)
            {
                Console.Clear();
                AddOrderToTable(selectedTable);
            }
            
            else if (optKey3.Key == ConsoleKey.Escape)
            {
                Mainmenu();
            }
            
        }
        static void DeleteOrderFromTable(Table selectedTable) //Onaylanmış siparişleri silmek için
        {
            Console.Clear();
            Console.WriteLine("-------------------------------------");
            Console.WriteLine($"      {selectedTableId}.MASA  - SİPARİŞ SİL     ");
            Console.WriteLine("-------------------------------------");

            // Masanın hazır siparişlerini listele
            int i = 1;
            foreach (var o in selectedTable.products.Where(x => x.Ready))
            {
                Console.WriteLine($"{i}.Ürün {selectedTable.tableId}.Masa // Ürün:{o.selectedProduct.productName} // Ücreti: ({o.selectedProduct.productPrice} TL)");
                i++;
            }

            // Silinecek siparişi seç
            Console.WriteLine("ANA MENÜ [ESC]\n GERİ DÖN[Sol OK]\nÜrünü Seç[Yukarı OK]");
            ConsoleKeyInfo optKey5 = Console.ReadKey();
            if(optKey5.Key == ConsoleKey.Escape)
            {
                Mainmenu();
            }
            else if (optKey5.Key == ConsoleKey.LeftArrow)
            {
                return;
            }
            else if (optKey5.Key == ConsoleKey.UpArrow)
            {
                bool oRemoval = false;
                while (!oRemoval)
                {
                    try
                    {
                        Console.Write("Lütfen Ürünü seçin:");
                        orderRemovalSelection = int.Parse(Console.ReadLine());
                        if (orderRemovalSelection > i -1)
                        {
                            Console.WriteLine("Geçersiz Seçim.");
                        }
                        else if(orderRemovalSelection == 0)
                        {
                            Console.WriteLine("Geçersiz Seçim.");
                        }
                        else if (orderRemovalSelection > 0)
                        { oRemoval = true; }
                    }
                    catch
                    {
                        Console.WriteLine("Hatalı bir giriş yaptınız.");

                    }
                }
            }
            else
            {

            }
            
            

            // Seçilen siparişi listeden kaldır
            selectedTable.products.Remove(selectedTable.products.FirstOrDefault(x => x.Ready && x.selectedProduct.productName == selectedTable.products.Where(y => y.Ready).ToList()[orderRemovalSelection - 1].selectedProduct.productName));
            Console.WriteLine("Sipariş silindi!");

            
        }

        static void TableBill()
        {
            Console.Clear();
            Console.WriteLine("-------------------------------------");
            Console.WriteLine("              MASA HESAP             ");
            Console.WriteLine("-------------------------------------");
            Console.WriteLine("               MASALAR               ");
            Console.WriteLine("-------------------------------------");
            for (int i = 1; i <= 5; i++) //masanın dolu veya boş olduğunu gösterir
            {
                Table table = tables.FirstOrDefault(t => t.tableId == i);
                
                string tableStatus = table.products.Any(p => p.Ready) ? "Dolu" : "Boş";
                if (tableStatus == "Dolu")
                {
                    Console.WriteLine($"{i}. Masa // Durumu: {tableStatus}");
                }

            }
            Console.WriteLine("Ana Menü [ESC]");
            Console.WriteLine("Masa Seçme [Yukarı OK]");
            ConsoleKeyInfo optKey4 = Console.ReadKey();
            if (optKey4.Key == ConsoleKey.Escape)
            {
                Mainmenu();
            }
            else if (optKey4.Key == ConsoleKey.UpArrow)
            {
                bool tBill = false;
                while(!tBill)
                {
                    try
                    {
                        Console.WriteLine("Masa ID giriniz:");
                        selectedTableId = int.Parse(Console.ReadLine());
                        if (selectedTableId > 0 && selectedTableId <= 5) 
                        {
                            tBill= true;
                        }
                    }
                    catch
                    {
                        Console.WriteLine("Hatalı bir giriş yaptınız.");
                    }
                }
                
            }
            else
            {
                TableBill();
            }
            
            
            Table selectedTable = tables.FirstOrDefault(x => x.tableId == selectedTableId);
            tBill:
            if (selectedTable != null && selectedTable.products.Any(p => p.Ready))
            {

                Console.Clear();
                Console.WriteLine("-------------------------------------");
                Console.WriteLine($"               {selectedTableId}.MASA        ");
                Console.WriteLine("-------------------------------------");
                Console.WriteLine("Mevcut Siparişler");

                if (selectedTable.products.Count > 0)
                {

                    foreach (var o in selectedTable.products.Where(x => x.Ready)) //Onaylanmış siparişleri göstermesi için
                    {
                        int i = 1;
                        Console.WriteLine($"{selectedTable.tableId}.Masa - {i}.Ürün:{o.selectedProduct.productName} - Adet: {o.quantity} Ücreti: ({o.selectedProduct.productPrice * o.quantity} TL)");
                        i++;


                    }
                    Console.WriteLine("-------------------------------------");
                    Console.WriteLine("İndirimli Öde[1]");
                    Console.WriteLine("Normal Öde   [2]");
                    Console.WriteLine("-------------------------------------");
                    Console.WriteLine("ANA MENÜ [ESC]");
                    Console.WriteLine("GERİ GİT [SOL OK]");
                    ConsoleKeyInfo key = Console.ReadKey();
                    if (key.Key == ConsoleKey.D1 || key.Key == ConsoleKey.NumPad1)
                    {
                        discountedPayment(selectedTable);
                    }
                    else if (key.Key == ConsoleKey.D2 || key.Key == ConsoleKey.NumPad2)
                    {
                        Pay(selectedTable);
                    }
                    else if (key.Key == ConsoleKey.Escape)
                    {
                        Mainmenu();
                    }
                    else if (key.Key == ConsoleKey.LeftArrow)
                    {
                        TableBill();
                    }
                    else
                    {
                        goto tBill;
                    }

                }
                else
                { Console.WriteLine("Mevcut bir sipariş yok"); }
            }
            else
            {
                // Dolu olmayan masalar için hata mesajı gösterir
                Console.WriteLine($"Hata: {selectedTableId} ID'li masada herhangi bir onaylanmış sipariş mevcut değil.");
                Console.WriteLine("Devam etmek için Enter'a tıkla");
                Console.ReadLine();
                TableBill();
            }


        }
        static void Pay(Table selectedTable) //Normal ödeme yapmak için
        {
            Console.Clear();
            double totalEarnedInaTable = 0;
            foreach (var o in selectedTable.products.Where(x => x.Ready))
            {
                totalEarnedInaTable += o.selectedProduct.productPrice * o.quantity;
            }
            Console.WriteLine("-------------------------------------");
            Console.WriteLine($"               {selectedTableId}.MASA - ÖDEME       ");
            Console.WriteLine("-------------------------------------");
            Console.WriteLine($"Toplam Tutar:{totalEarnedInaTable}");
            Console.WriteLine("-------------------------------------");
            Console.WriteLine($"Bahşiş:{selectedTable.temporaryTip} TL");
            Console.WriteLine("Bahşiş Ekle[Yukarı OK]");
            Console.WriteLine("Siparişi Onayla[Aşağı OK]");
            Console.WriteLine("ANA MENÜ [ESC]");
            Console.WriteLine("GERİ GİT [SOL OK]");
            ConsoleKeyInfo key = Console.ReadKey();
            if (key.Key == ConsoleKey.UpArrow)
            {
                Tip(selectedTable);
                Pay(selectedTable);
            }
            else if (key.Key == ConsoleKey.Escape)
            {
                Mainmenu();
            }
            else if (key.Key == ConsoleKey.LeftArrow)
            {
                TableBill();
            }
            else if (key.Key == ConsoleKey.DownArrow)
            {
                Console.WriteLine($"Siparişi tutarı:{totalEarnedInaTable + selectedTable.temporaryTip} TL (Bahşiş Dahil: {selectedTable.temporaryTip} TL)  ");
                Console.WriteLine("Onaylamak istiyor musunuz?");
                Console.WriteLine("EVET [E] / HAYIR [H] basın");
                ConsoleKeyInfo confirmKey = Console.ReadKey(); //
                if (confirmKey.Key == ConsoleKey.E)
                {
                    Console.WriteLine($"{totalEarnedInaTable + selectedTable.temporaryTip} TL Tutarındaki siparişiniz {selectedTable.tableId}.masa için onaylandı! ");
                    totalEarnedCash += totalEarnedInaTable;
                    totalEarnedTip += selectedTable.temporaryTip;
                    selectedTable.totalTip += selectedTable.temporaryTip;
                    selectedTable.totalPrice += totalEarnedInaTable;
                    selectedTable.temporaryTip = 0;
                    selectedTable.ClearOrders();
                    Console.WriteLine("Devam etmek için Enter'a tuşla");
                    Console.ReadLine();
                    Mainmenu();
                }
                else if (confirmKey.Key == ConsoleKey.H)
                {
                    Pay(selectedTable);
                }

            }
            else
            {
                Pay(selectedTable);
            }

        }
        static void discountedPayment(Table selectedTable) //İndirimli Ödeme yapmak için
        {
            Console.Clear();
            double totalEarnedInaTable = 0;
            foreach (var o in selectedTable.products.Where(x => x.Ready))
            {
                totalEarnedInaTable += o.selectedProduct.productPrice * o.quantity;
            }
            Console.WriteLine("-------------------------------------");
            Console.WriteLine($"        {selectedTableId}.MASA - İNDİRİMLİ ÖDEME       ");
            Console.WriteLine("-------------------------------------");
            Console.WriteLine($"Siparişi tutarı:{totalEarnedInaTable} TL\nBahşiş: {selectedTable.temporaryTip} TL");
            Console.WriteLine($"%25 İndirimli sipariş tutarı:{totalEarnedInaTable - (totalEarnedInaTable / 100 * 25)} TL");
            Console.WriteLine("-------------------------------------");
            Console.WriteLine("Bahşiş Ekle[Yukarı OK]");
            Console.WriteLine("Siparişi Onayla[SPACE]");
            Console.WriteLine("ANA MENÜ [ESC]");
            Console.WriteLine("GERİ GİT [SOL OK]");
            ConsoleKeyInfo key = Console.ReadKey();
            if (key.Key == ConsoleKey.UpArrow)
            {
                Tip(selectedTable);
                discountedPayment(selectedTable);
            }
            else if (key.Key == ConsoleKey.Escape)
            {
                Mainmenu();
            }
            else if (key.Key == ConsoleKey.LeftArrow)
            {
                TableBill();
            }
            else if (key.Key == ConsoleKey.Spacebar)
            {
                Console.WriteLine($"Siparişi tutarı:{totalEarnedInaTable} TL (Bahşiş: {selectedTable.totalTip} TL)  ");
                Console.WriteLine($"%25 İndirimli sipariş tutarı:{totalEarnedInaTable - (totalEarnedInaTable / 100 * 25)} TL(Bahşiş: {selectedTable.temporaryTip} TL)  ");
                Console.WriteLine("Onaylamak istiyor musunuz?");
                Console.WriteLine("EVET [E] / HAYIR [H] basın");
                ConsoleKeyInfo confirmKey = Console.ReadKey(); //
                if (confirmKey.Key == ConsoleKey.E)
                {
                    Console.WriteLine($"{totalEarnedInaTable + selectedTable.temporaryTip} TL Tutarındaki siparişiniz {selectedTable.tableId}.masa için onaylandı! ");
                    totalEarnedCash += totalEarnedInaTable - (totalEarnedInaTable / 100 * 25);
                    totalEarnedTip += selectedTable.temporaryTip;
                    selectedTable.totalTip += selectedTable.temporaryTip;
                    selectedTable.totalPrice += ((totalEarnedInaTable) - (totalEarnedInaTable / 100 * 25));
                    selectedTable.temporaryTip = 0;
                    selectedTable.ClearOrders();
                    Console.WriteLine("Devam etmek için Enter'a tuşla");
                    Console.ReadLine();
                    Mainmenu();
                }
                else if (confirmKey.Key == ConsoleKey.H)
                {
                    discountedPayment(selectedTable);
                }

            }
            else
            {
                discountedPayment(selectedTable);
            }
        }
        static void Tip(Table selectedTable)
        {

            bool tLoop = false;
            while (!tLoop)
            {
                try
                {
                    Console.WriteLine("Lütfen bahşiş miktarını giriniz:");
                    selectedTable.temporaryTip = Convert.ToDouble(Console.ReadLine());
                    if (selectedTable.temporaryTip >= 0)
                    {
                        tLoop = true;
                        Console.WriteLine($"{selectedTable.temporaryTip} TL bahşiş miktarı masaya eklendi");
                    }
                }
                catch
                {
                    Console.WriteLine("0'dan küçük veya sayı dışında bir karakter girmeyiniz.");

                }
            }
            Console.WriteLine("Geri dönmek için ENTER'a tıkla");
            Console.ReadLine();
            
        }
        
        static void CashierOperations()
        {
            Console.Clear();
            Console.WriteLine("---------------------------------------------------");
            Console.WriteLine("           KASA İŞLEMLERİ           ");
            Console.WriteLine("---------------------------------------------------");
            Console.WriteLine($"Toplam Tutar: {totalEarnedCash} TL");
            Console.WriteLine($"Toplam Bahşiş: {totalEarnedTip} TL");
            Console.WriteLine($"Toplam Verilen Sipariş: {oQuantity}");
            Console.WriteLine("---------------------------------------------------");
            Console.WriteLine("         Masalara Göre Kazanç        ");
            foreach (Table table in tables)
            {
                Console.WriteLine($"{table.tableId}. Masa: Kazanç {table.totalPrice} TL --- Bahşiş {table.totalTip} TL");
            }

            Console.WriteLine("---------------------------------------------------");
            Console.WriteLine("ANA MENÜ [ESC]");
            Console.WriteLine("---------------------------------------------------");
            ConsoleKeyInfo key = Console.ReadKey();
            if (key.Key == ConsoleKey.Escape)
            {
                Mainmenu();

            }
            else
            {
                CashierOperations();
            }
        }

    }
}

