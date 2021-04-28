using System.Threading;
using System.Threading.Tasks;

using Elecritic.Database;
using Elecritic.Models;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Elecritic.Features.ProductDetails.Commands {
    public class DeleteFavorite {
        public class Command : IRequest<bool> {
            public Favorite Favorite { get; set; }
        }

        public class CommandHandler : IRequestHandler<Command, bool> {
            private readonly IDbContextFactory<ElecriticContext> _factory;
            private readonly ILogger<CommandHandler> _logger;

            public CommandHandler(IDbContextFactory<ElecriticContext> factory, ILogger<CommandHandler> logger) {
                _factory = factory;
                _logger = logger;
            }

            public async Task<bool> Handle(Command request, CancellationToken cancellationToken) {
                _logger.LogInformation($"Handling command {request}: {{@request}}", request);

                try {
                    using var dbContext = _factory.CreateDbContext();

                    dbContext.Entry(request.Favorite).State = EntityState.Deleted;
                    await dbContext.SaveChangesAsync();

                    return true;
                }
                catch (DbUpdateException) {
                    return false;
                }
            }
        }
    }
}
