using System;

namespace Api.Repositories.Models
{
    public interface IEntity
    {
        Guid Id { get; set; }
    }
}
