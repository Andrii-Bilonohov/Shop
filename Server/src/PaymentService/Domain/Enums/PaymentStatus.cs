using System.Runtime.Serialization;

namespace Domain.Enums
{
    public enum PaymentStatus
    {
        [EnumMember(Value = "created")]
        Created = 1,
        [EnumMember(Value = "pending")]
        Pending = 2,
        [EnumMember(Value = "succeeded")]
        Succeeded = 3,
        [EnumMember(Value = "failed")]
        Failed = 4,
        [EnumMember(Value = "canceled")]
        Canceled = 5,
        [EnumMember(Value = "refunded")]
        Refunded = 6
    }
}
