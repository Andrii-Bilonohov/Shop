using System.Runtime.Serialization;

namespace Domain.Enums
{
    public enum Role
    {
        [EnumMember(Value = "buyer")]
        Buyer = 1,
        [EnumMember(Value = "seller")]
        Seller = 2,
        [EnumMember(Value = "admin")]
        Admin = 3
    }
}
