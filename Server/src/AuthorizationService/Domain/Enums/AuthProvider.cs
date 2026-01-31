using System.Runtime.Serialization;

namespace Domain.Enums
{
    public enum AuthProvider
    {
        [EnumMember(Value = "google")]
        Google = 1,
        [EnumMember(Value = "local")]
        Local = 2
    }
}
