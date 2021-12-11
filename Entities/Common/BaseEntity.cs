using System;

namespace Entities.Common
{
    #region IEntity

    public interface IEntity
    {
    }

    #endregion IEntity

    #region BaseEntity

    public abstract class BaseEntity<TKey> : IEntity
    {
        public TKey Id { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime LastUpdate { get; set; }

        public BaseEntity()
        {
            CreateDate = DateTime.Now;
            LastUpdate = DateTime.Now;
        }
    }

    public abstract class BaseEntity : BaseEntity<long>
    {
    }

    #endregion BaseEntity
}