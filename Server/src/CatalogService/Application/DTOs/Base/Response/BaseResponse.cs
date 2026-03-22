namespace Application.DTOs.Base.Response;

public record BaseResponse<T>
(
    ICollection<T> Items,
    int TotalCount
);