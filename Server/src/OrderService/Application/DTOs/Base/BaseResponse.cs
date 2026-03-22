namespace Application.DTOs.Base;

public record BaseResponse<T>
(
    ICollection<T> Items,
    int TotalCount
);