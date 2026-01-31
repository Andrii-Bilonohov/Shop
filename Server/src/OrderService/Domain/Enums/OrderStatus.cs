using System.Runtime.Serialization;

namespace Domain.Enums
{
    public enum OrderStatus
    {
        [EnumMember(Value = "created")]
        Created = 1,
        [EnumMember(Value = "paid")]
        Paid = 2,
        [EnumMember(Value = "shipped")]
        Shipped = 3,
        [EnumMember(Value = "completed")]
        Completed = 4,
        [EnumMember(Value = "cancelled")]
        Cancelled = 5
    }
}
