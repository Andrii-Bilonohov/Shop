using System.Runtime.Serialization;

namespace Domain.Enums
{
    public enum PaymentMethod
    {
        [EnumMember(Value = "card")]
        Card = 1,
        [EnumMember(Value = "cryptocurrency")]
        Cryptocurrency = 2
    }
}
