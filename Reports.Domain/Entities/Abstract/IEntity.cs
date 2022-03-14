using System;

namespace Reports.Domain.Entities.Abstract
{
    public interface IEntity
    {
        string Id { get; }
        DateTime CreatedTime { get; }
        DateTime UpdatedTime { get; }
    }
}
