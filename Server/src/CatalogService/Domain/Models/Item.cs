using Domain.Enums;

namespace Domain.Models
{

    public class Item : BaseModel
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public Category Category { get; private set; }
        public decimal Price { get; private set; }
        public int Stock { get; private set; }
        public double Weight { get; private set; }
        public string ImageUrl { get; private set; }
        
        public ICollection<Review> Reviews { get; private set; } =  new List<Review>();

        protected Item() { }

        public Item(string name, string description, Category category, decimal price, int stock, double weight, string imageUrl)
        {
            UpdateDetails(name, description, category, price, stock, weight, imageUrl);
        }
        
        public void UpdateDetails(string name, string description, Category category, decimal price, int stock, double weight, string imageUrl)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentNullException(nameof(description));

            if (!Enum.IsDefined(typeof(Category), category))
                throw new ArgumentException("Invalid category", nameof(category));

            if (price < 0)
                throw new ArgumentException(nameof(price), "Price must be positive");

            if (stock < 0)
                throw new ArgumentException(nameof(stock), "Stock cannot be negative");

            if (weight < 0)
                throw new ArgumentException(nameof(weight), "Weight cannot be negative");

            if (string.IsNullOrWhiteSpace(imageUrl))
                throw new ArgumentNullException(nameof(imageUrl));

            Name = name;
            Description = description;
            Category = category;
            Price = price;
            Stock = stock;
            Weight = weight;
            ImageUrl = imageUrl;

            Touch();
        }
    }
}
