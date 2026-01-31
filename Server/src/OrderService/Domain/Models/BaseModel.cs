namespace Domain.Models
{
    public abstract class BaseModel
    {
        public Guid Id { get; protected set; }

        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public bool IsDeleted { get; private set; } = false;

        protected BaseModel()
        {
            CreatedAt = DateTime.UtcNow;
        }

        public void Touch()
        {
            UpdatedAt = DateTime.UtcNow;
        }

        protected virtual void Delete()
        {
            IsDeleted = true;

            Touch();
        }
    }
}
