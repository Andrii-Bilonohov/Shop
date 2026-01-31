using System.Runtime.Serialization;

namespace Domain.Enums
{
    public enum Category
    {
        [EnumMember(Value = "electronics")]
        Electronics = 1,
        [EnumMember(Value = "clothing")]
        Clothing = 2,
        [EnumMember(Value = "home_appliances")]
        HomeAppliances = 3,
        [EnumMember(Value = "books")]
        Books = 4,
        [EnumMember(Value = "toys")]
        Toys = 5,
        [EnumMember(Value = "sports")]
        Sports = 6,
        [EnumMember(Value = "beauty")]
        Beauty = 7,
        [EnumMember(Value = "automotive")]
        Automotive = 8,
        [EnumMember(Value = "grocery")]
        Grocery = 9,
        [EnumMember(Value = "health")]
        Health = 10,
        [EnumMember(Value = "furniture")]
        Furniture = 11
    }
}
