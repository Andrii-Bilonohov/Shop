using Domain.Enums;

namespace Domain.Models
{
    public class Payment : BaseModel
    {
        public Guid UserId { get; private set; }
        public Guid OrderId { get; private set; }

        public PaymentMethod Method { get; private set; }
        public PaymentStatus Status { get; private set; }

        public string? ExternalPaymentId { get; private set; }
        public string? ProviderStatus { get; private set; }


        protected Payment() { }

        public Payment(Guid userId, Guid orderId, PaymentMethod method)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException(nameof(userId));

            if (orderId == Guid.Empty)
                throw new ArgumentException(nameof(orderId));

            if (!Enum.IsDefined(method))
                throw new ArgumentException(nameof(method));

            UserId = userId;
            OrderId = orderId;
            Method = method;
            Status = PaymentStatus.Created;

            Touch();
        }

        public void MarkPending(string externalId)
        {
            if (Status == PaymentStatus.Pending)
            {
                if (ExternalPaymentId != externalId)
                    throw new InvalidOperationException("ExternalPaymentId mismatch");

                return;
            }

            if (Status != PaymentStatus.Created)
                throw new InvalidOperationException("Only created payment can be pending");

            ExternalPaymentId = externalId;
            Status = PaymentStatus.Pending;
            Touch();
        }

        public void MarkSucceeded()
        {
            if (Status == PaymentStatus.Succeeded)
                return;

            if (Status == PaymentStatus.Failed || Status == PaymentStatus.Canceled)
                return; 

            if (Status != PaymentStatus.Pending)
                throw new InvalidOperationException("Invalid payment state");

            Status = PaymentStatus.Succeeded;
            Touch();
        }

        public void MarkFailed()
        {
            if (Status == PaymentStatus.Failed)
                return;

            if (IsFinal)
                return;

            Status = PaymentStatus.Failed;
            Touch();
        }
        
        public void MarkCanceled()
        {
            if (Status == PaymentStatus.Canceled)
                return;

            if (IsFinal)
                throw new InvalidOperationException("Final payment cannot be canceled");

            Status = PaymentStatus.Canceled;
            Touch();
        }

        public void MarkRefunded()
        {
            if (Status == PaymentStatus.Refunded)
                return;

            if (Status != PaymentStatus.Succeeded)
                throw new InvalidOperationException("Only succeeded payment can be refunded");

            Status = PaymentStatus.Refunded;
            Touch();
        }

        public void UpdateProviderStatus(string status)
        {
            if (ProviderStatus == status)
                return;

            ProviderStatus = status;
            Touch();
        }
        
        public bool IsFinal => Status 
           is PaymentStatus.Succeeded
           or PaymentStatus.Failed
           or PaymentStatus.Canceled
           or PaymentStatus.Refunded;
    }
}