using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BackendAPIDevelopmentTask.ViewModels
{
    public class AddEditProductViewModel
    {
        [Required]
        [BindProperty(Name = "Item name")]
        [JsonPropertyName("Item name")]
        public string Title { get; set; }
        [Required]
        [BindProperty(Name = "Quantity")]
        [JsonPropertyName("Quantity")]
        public int Quantity { get; set; }
        [Required]
        [BindProperty(Name = "Price")]
        [JsonPropertyName("Price")]
        public decimal Price { get; set; }
    }
}
