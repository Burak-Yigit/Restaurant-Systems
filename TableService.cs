using Restaurant_Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_Systems
{
    class Product :Order
    {
        public int productId { get; set; }
        public string productName { get; set; }

        public double productPrice { get; set; }


    }
    class Order : Table
    {
        public Product selectedProduct { get; set; }
        public int quantity { get; set; }
        
        public bool Ready { get; set; } // Masada sipariş bekleme - onaylanma durumuyla ilgilenir // ayrıyeten boş ve dolu masaları gösterir.
        
        public int orderId { get; set; }
        
    }


}
class Table
{
    public int tableId { get; set; }
    public double totalPrice = 0;
    public double totalTip = 0;
    public double temporaryTip = 0;
    
    public List<Order> products { get; set; } = new List<Order>();
    public void ClearOrders()
    {
        products.Clear();
    }



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

