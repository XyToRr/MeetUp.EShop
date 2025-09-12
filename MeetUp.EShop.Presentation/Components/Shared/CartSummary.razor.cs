using MeetUp.EShop.Presentation.Models.Product;
using Microsoft.AspNetCore.Components;

namespace MeetUp.EShop.Presentation.Components.Shared
{
    public partial class CartSummary
    {
        [Parameter] public List<Product> Products { get; set; } = new List<Product>();

        private decimal TotalPrice => Products.Sum(p => p.Price);

    }
}
