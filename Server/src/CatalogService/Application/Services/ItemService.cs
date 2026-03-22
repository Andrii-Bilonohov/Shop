using System.Text.Json;
using Application.Abstractions.Logger;
using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Abstractions.UnitOfWork;
using Application.Filters; 
using Application.Contracts.Base;
using Application.DTOs.Items.Requests;
using Application.DTOs.Items.Responses;
using Application.Mappers;
using Microsoft.Extensions.Caching.Memory;

namespace Application.Services
{
    public sealed class ItemService : IItemService
    {
        private const int MaxLimit = 100;
        private const int MinOffset = 0;
        
        private static string ItemKey(Guid id) => $"item:{id}";

        private const string ListVersionKey = "items:list:version";
        
        private static readonly MemoryCacheEntryOptions ItemCacheOptions = new()
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
            SlidingExpiration = TimeSpan.FromMinutes(1)
        };

        private static readonly MemoryCacheEntryOptions ListCacheOptions = new()
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
        };

        private readonly IItemRepository _itemRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCache _cache;
        private readonly IExceptionLogger _logger;

        public ItemService(IItemRepository itemRepository,
            IUnitOfWork unitOfWork, IMemoryCache cache, IExceptionLogger logger)
        {
            _itemRepository = itemRepository ?? throw new ArgumentNullException(nameof(itemRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Information> CreateAsync(CreateItem request, CancellationToken ct)
        {
            try
            {
                if (request is null)
                    return new Information(Error: "Request is null");

                var existing = await _itemRepository.GetByAsync(i => i.Name == request.Name, ct);
                if (existing is not null)
                    return new Information(Error: "Item with the same name already exists");

                var item = request.ToItem();
                _itemRepository.Add(item);
                await _unitOfWork.SaveChangesAsync(ct);

                _cache.Remove(ItemKey(item.Id));
                BumpListVersion();

                return new Information(Id: item.Id, Message: "Item created");
            }
            catch (OperationCanceledException)
            {
                return new Information(Error: "Request canceled");
            }
            catch (ArgumentNullException ex)
            {
                return new Information(Error: ex.Message);
            }
            catch (ArgumentException ex)
            {
                return new Information(Error: ex.Message);
            }
            catch (Exception ex)
            {
                _logger.Log(ex, "Create item failed");
                return new Information(Error: "Unexpected error: " + ex.Message);
            }
        }

        public async Task<Information> UpdateAsync(Guid id, UpdateItem request, CancellationToken ct)
        {
            try
            {
                if (id == Guid.Empty)
                    return new Information(Error: "Id is invalid");

                if (request is null)
                    return new Information(Error: "Request is null");

                var existingSameName = await _itemRepository.GetByAsync(i => i.Name == request.Name && i.Id != id, ct);
                if (existingSameName is not null)
                    return new Information(Error: "Item with the same name already exists");

                var item = await _itemRepository.GetByIdAsync(id, ct);
                if (item is null)
                    return new Information(Error: $"Item with id {id} not found");

                item.UpdateDetails(
                    request.Name,
                    request.Description,
                    request.Category,
                    request.Price,
                    request.Stock,
                    request.Weight,
                    request.ImageUrl
                );

                _itemRepository.Update(item);
                await _unitOfWork.SaveChangesAsync(ct);
                
                _cache.Remove(ItemKey(id));
                BumpListVersion();

                return new Information(Id: item.Id, Message: "Item updated");
            }
            catch (OperationCanceledException)
            {
                return new Information(Error: "Request canceled");
            }
            catch (ArgumentNullException ex)
            {
                return new Information(Error: ex.Message);
            }
            catch (ArgumentException ex)
            {
                return new Information(Error: ex.Message);
            }
            catch (Exception ex)
            {
                _logger.Log(ex, "Update item failed. Id={Id}", id);
                return new Information(Error: "Unexpected error: " + ex.Message);
            }
        }

        public async Task<Information> DeleteAsync(Guid id, CancellationToken ct)
        {
            try
            {
                if (id == Guid.Empty)
                    return new Information(Error: "Id is invalid");

                var item = await _itemRepository.GetByIdAsync(id, ct);
                if (item is null)
                    return new Information(Error: $"Item with id {id} not found");

                item.Delete();
                _itemRepository.Update(item);
                await _unitOfWork.SaveChangesAsync(ct);
                
                _cache.Remove(ItemKey(id));
                BumpListVersion();

                return new Information(Id: id, Message: "Item deleted");
            }
            catch (OperationCanceledException)
            {
                return new Information(Error: "Request canceled");
            }
            catch (ArgumentNullException ex)
            {
                return new Information(Error: ex.Message);
            }
            catch (ArgumentException ex)
            {
                return new Information(Error: ex.Message);
            }
            catch (Exception ex)
            {
                _logger.Log(ex, "Delete item failed. Id={Id}", id);
                return new Information(Error: "Unexpected error: " + ex.Message);
            }
        }

        public async Task<ResponseList<ItemResponse>> GetAllAsync(int limit, int offset, ItemFilter? filter,
            CancellationToken ct)
        {
            try
            {
                limit = Math.Clamp(limit, 1, MaxLimit);
                offset = Math.Max(MinOffset, offset);

                var key = ListKey(limit, offset, filter);

                if (_cache.TryGetValue(key, out ResponseList<ItemResponse>? cached) && cached is not null)
                    return cached;

                var items = await _itemRepository.GetAllAsync(limit, offset, filter, ct);
                var data = items.Items.ToResponse();

                var totalPages = items.TotalCount == 0 ? 0 : (int)Math.Ceiling(items.TotalCount / (double)limit);

                var result = new ResponseList<ItemResponse>(
                    Limit: limit,
                    Offset: offset,
                    Items: items.TotalCount,
                    Pages: totalPages,
                    Data: data
                );

                _cache.Set(key, result, ListCacheOptions);
                return result;
            }
            catch (OperationCanceledException)
            {
                return new ResponseList<ItemResponse>(Error: "Request canceled");
            }
            catch (ArgumentNullException ex)
            {
                return new ResponseList<ItemResponse>(Error: ex.Message);
            }
            catch (ArgumentException ex)
            {
                return new ResponseList<ItemResponse>(Error: ex.Message);
            }
            catch (Exception ex)
            {
                _logger.Log(ex, "GetAll items failed");
                return new ResponseList<ItemResponse>(Error: "Unexpected error: " + ex.Message);
            }
        }

        public async ValueTask<Response<ItemResponse>> GetByIdAsync(Guid id, CancellationToken ct)
        {
            try
            {
                if (id == Guid.Empty)
                    return new Response<ItemResponse>(Error: "Id is invalid");

                var key = ItemKey(id);

                if (_cache.TryGetValue(key, out ItemResponse? cached) && cached is not null)
                    return new Response<ItemResponse>(Data: cached);

                var item = await _itemRepository.GetByIdAsync(id, ct);
                if (item is null)
                    return new Response<ItemResponse>(Error: $"Item with id {id} not found");

                var dto = item.ToResponse();

                _cache.Set(key, dto, ItemCacheOptions);
                return new Response<ItemResponse>(Data: dto);
            }
            catch (OperationCanceledException)
            {
                return new Response<ItemResponse>(Error: "Request canceled");
            }
            catch (ArgumentNullException ex)
            {
                return new Response<ItemResponse>(Error: ex.Message);
            }
            catch (ArgumentException ex)
            {
                return new Response<ItemResponse>(Error: ex.Message);
            }
            catch (Exception ex)
            {
                _logger.Log(ex, "Get item by id failed. Id={Id}", id);
                return new Response<ItemResponse>(Error: "Unexpected error: " + ex.Message);
            }
        }

        // public async Task<Response<ReviewResponse>> RateAsync(ReviewItemRequest request, CancellationToken ct)
        // {
        //     try
        //     {
        //         if (request is null)
        //             return new Response<ReviewResponse>(Error: "Request is null");
        //
        //         if (request.ItemId == Guid.Empty)
        //             return new Response<ReviewResponse>(Error: "ItemId is required");
        //
        //         if (request.UserId == Guid.Empty)
        //             return new Response<ReviewResponse>(Error: "UserId is required");
        //
        //         var item = await _itemRepository.GetByIdAsync(request.ItemId, ct);
        //         if (item is null)
        //             return new Response<ReviewResponse>(Error: $"Item with id {request.ItemId} not found");
        //
        //         var review = await _reviewRepository.GetByItemAndUserAsync(request.ItemId, request.UserId, ct);
        //
        //         if (review is null)
        //         {
        //             review = new Review(
        //                 request.UserId,
        //                 request.ItemId,
        //                 request.Rating,
        //                 request.Description,
        //                 request.ImageUrl
        //             );
        //
        //             _reviewRepository.Add(review);
        //         }
        //         else
        //         {
        //             review.Update(request.Rating, request.Description, request.ImageUrl);
        //             _reviewRepository.Update(review);
        //         }
        //
        //         await _unitOfWork.SaveChangesAsync(ct);
        //
        //         _cache.Remove(ItemKey(request.ItemId));
        //         BumpListVersion();
        //         
        //         var dto = _mapper.Map<ReviewResponse>(review);
        //         return new Response<ReviewResponse>(Data: dto);
        //     }
        //     catch (OperationCanceledException)
        //     {
        //         return new Response<ReviewResponse>(Error: "Request canceled");
        //     }
        //     catch (Exception ex)
        //     {
        //         _logger.Log(ex, "Rate item failed. ItemId={ItemId} UserId={UserId}", request?.ItemId, request?.UserId);
        //         return new Response<ReviewResponse>(Error: "Unexpected error: " + ex.Message);
        //     }
        // }
        

        private string ListKey(int limit, int offset, ItemFilter? filter)
        {
            var version = GetListVersion();

            var filterKey = filter is null
                ? "nofilter"
                : JsonSerializer.Serialize(filter);

            return $"items:list:v{version}:{limit}:{offset}:{filterKey}";
        }

        private int GetListVersion()
        {
            if (_cache.TryGetValue(ListVersionKey, out int v) && v > 0)
                return v;

            _cache.Set(ListVersionKey, 1);
            return 1;
        }

        private void BumpListVersion()
        {
            var v = GetListVersion() + 1;
            _cache.Set(ListVersionKey, v);
        }
    }
}