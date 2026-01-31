using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Abstractions.Services.UnitOfWork;
using Application.Contracts;
using Application.Contracts.Base;
using Application.DTOs.Requests;
using Application.DTOs.Responses;
using Application.Filters;
using AutoMapper;
using Domain.Models;

namespace Application.Services
{
    public sealed class OrderService : IOrderService
    {
        private const int MaxLimit = 100;
        private const int MinOffset = 0;

        private readonly IOrderRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(IOrderRepository repository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _repository = repository ??  throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ??  throw new ArgumentNullException(nameof(mapper));
            _unitOfWork = unitOfWork ??   throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<Information> CreateAsync(CreateOrder request, CancellationToken ct)
        {
            try
            {
                var order = _mapper.Map<Order>(request);

                _repository.Add(order);
                await _unitOfWork.SaveChangesAsync(ct);

                return new Information(Id: order.Id, Message: "Order created");
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
                return new Information(Error: "Unexpected error: " + ex.Message);
            }
        }
        
        public async Task<Information> DeleteAsync(Guid id, CancellationToken ct)
        {
            try
            {
                var exists = await _repository.GetByIdAsync(id, ct);
                if (exists is null)
                    return new Information(Error: $"Order with id {id} not found");

                exists.Remove();
                _repository.Update(exists);
                await _unitOfWork.SaveChangesAsync(ct);

                return new Information(Id: id, Message: "Order deleted");
            }
            catch (Exception ex)
            {
                return new Information(Error: "Unexpected error: " + ex.Message);
            }
        }

        public async Task<ResponseList<OrderResponse>> GetAllAsync(int limit, int offset, OrderFilter filter, CancellationToken ct)
        {
            try
            {
                limit = Math.Clamp(limit, 1, MaxLimit);
                offset = Math.Max(MinOffset, offset);

                var (orders, totalCount) = await _repository.GetAllAsync(limit, offset, filter, ct);

                var data = _mapper.Map<IReadOnlyList<OrderResponse>>(orders);

                var totalPages = totalCount == 0
                    ? 0
                    : (int)Math.Ceiling(totalCount / (double)limit);

                return new ResponseList<OrderResponse>(
                    Limit: limit,
                    Offset: offset,
                    Items: totalCount,
                    Pages: totalPages,
                    Data: data
                );
            }
            catch (Exception ex)
            {
                return new ResponseList<OrderResponse>(
                    Error: "Unexpected error: " + ex.Message
                );
            }
        }

        public async ValueTask<Response<OrderResponse>> GetByIdAsync(Guid id, CancellationToken ct)
        {
            try
            {
                var order = await _repository.GetByIdAsync(id, ct);
                if (order is null)
                    return new Response<OrderResponse>(
                        Error: $"Order with id {id} not found"
                    );

                return new Response<OrderResponse>(
                    Data: _mapper.Map<OrderResponse>(order)
                );
            }
            catch (Exception ex)
            {
                return new Response<OrderResponse>(
                    Error: "Unexpected error: " + ex.Message
                );
            }
        }

        public async Task<Information> UpdateOrderStatusAsync(Guid id, UpdateOrderStatus request, CancellationToken ct)
        {
            try
            {
                var order = await _repository.GetByIdAsync(id, ct);
                if (order is null)
                    return new Information(
                        Error: $"Order with id {id} not found"
                    );

                switch (request.Action)
                {
                    case OrderStatusAction.Pay:
                        order.MarkAsPaid();
                        break;

                    case OrderStatusAction.Ship:
                        order.Ship();
                        break;

                    case OrderStatusAction.Complete:
                        order.Complete();
                        break;

                    case OrderStatusAction.Cancel:
                        order.Cancel();
                        break;

                    default:
                        return new Information(Error: "Invalid order action");
                }

                 _repository.Update(order);
                 await _unitOfWork.SaveChangesAsync(ct);

                 return new Information(Id: order.Id, Message: $"Order status changed: {order.Status}");
            }
            catch (InvalidOperationException ex)
            {
                return new Information(Error: ex.Message);
            }
            catch (ArgumentException ex)
            {
                return new Information(Error: ex.Message);
            }
            catch (Exception ex)
            {
                return new Information(Error: "Unexpected error: " + ex.Message);
            }
        }
    }
}
