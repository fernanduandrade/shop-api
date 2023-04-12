using Microsoft.EntityFrameworkCore;
using Entities = Shop.Domain.Entities;

namespace Shop.Application.Common.Interfaces;

public interface IAppDbContext
{
    DbSet<Entities.Client> Clients {get;}
    DbSet<Entities.Product> Products {get;}
    DbSet<Entities.User> Users {get;}
}
