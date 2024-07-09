using Microsoft.EntityFrameworkCore.Infrastructure;

namespace ArchitectureSample.Infrastructure.Persistence;

public interface IDbFacadeResolver
{
	DatabaseFacade Database { get; }
}