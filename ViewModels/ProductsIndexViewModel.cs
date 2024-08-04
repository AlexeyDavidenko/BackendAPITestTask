using BackendAPIDevelopmentTask.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Configuration;
using System.Text.Json.Serialization;

namespace BackendAPIDevelopmentTask.ViewModels
{
    public class ProductsIndexViewModel
    {
        private readonly AppCustomSettings _customSettings;
        public ProductsIndexViewModel(Product product) 
        {

            Title = product.Title;
            Quantity = product.Quantity;
            Price = product.Price;
            //"Total price with VAT" is calculated using the formula: 
            //Total price with VAT = (Item amount * Price per item) * (1 + VAT)
            TotalVAT = (product.Quantity * product.Price) * (decimal)(1 + AppCustomSettings.VAT);
        }
        [BindProperty(Name = "Item name")]
        [JsonPropertyName("Item name")]
        public string Title { get; set; }
        [BindProperty(Name = "Quantity")]
        [JsonPropertyName("Quantity")]
        public int Quantity { get; set; }
        [BindProperty(Name = "Price")]
        [JsonPropertyName("Price")]
        public decimal Price { get; set; }
        [BindProperty(Name = "Total price with VAT")]
        [JsonPropertyName("Total price with VAT")]
        public decimal TotalVAT { get; set; }
    }
}
