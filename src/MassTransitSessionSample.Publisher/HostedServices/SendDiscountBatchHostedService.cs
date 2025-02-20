using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MassTransitSessionSample.Publisher.HostedServices
{
    public class SendDiscountBatchHostedService : IHostedService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public SendDiscountBatchHostedService(
            IServiceScopeFactory serviceScopeFactory, 
            IConfiguration configuration)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var serviceProvider = scope.ServiceProvider;
            var bus = serviceProvider.GetRequiredService<IBus>();

            var enrollmentId = Guid.NewGuid();
            var list = new List<SendDiscount>
            {
                new SendDiscount { EnrollmentId = enrollmentId, PercentDiscount = 50, Creation = DateTime.UtcNow },
                new SendDiscount { EnrollmentId = enrollmentId, PercentDiscount = 50, Creation = DateTime.UtcNow },
                new SendDiscount { EnrollmentId = enrollmentId, PercentDiscount = 50, Creation = DateTime.UtcNow },
                new SendDiscount { EnrollmentId = enrollmentId, PercentDiscount = 50, Creation = DateTime.UtcNow },
                new SendDiscount { EnrollmentId = enrollmentId, PercentDiscount = 50, Creation = DateTime.UtcNow },
                new SendDiscount { EnrollmentId = enrollmentId, PercentDiscount = 50, Creation = DateTime.UtcNow },
                new SendDiscount { EnrollmentId = enrollmentId, PercentDiscount = 50, Creation = DateTime.UtcNow },
                new SendDiscount { EnrollmentId = enrollmentId, PercentDiscount = 50, Creation = DateTime.UtcNow },
                new SendDiscount { EnrollmentId = enrollmentId, PercentDiscount = 50, Creation = DateTime.UtcNow },
                new SendDiscount { EnrollmentId = enrollmentId, PercentDiscount = 50, Creation = DateTime.UtcNow },
                new SendDiscount { EnrollmentId = enrollmentId, PercentDiscount = 50, Creation = DateTime.UtcNow },
                new SendDiscount { EnrollmentId = enrollmentId, PercentDiscount = 50, Creation = DateTime.UtcNow },
                new SendDiscount { EnrollmentId = enrollmentId, PercentDiscount = 50, Creation = DateTime.UtcNow },
                new SendDiscount { EnrollmentId = enrollmentId, PercentDiscount = 50, Creation = DateTime.UtcNow },
                new SendDiscount { EnrollmentId = enrollmentId, PercentDiscount = 50, Creation = DateTime.UtcNow },
                new SendDiscount { EnrollmentId = enrollmentId, PercentDiscount = 50, Creation = DateTime.UtcNow },
                new SendDiscount { EnrollmentId = enrollmentId, PercentDiscount = 50, Creation = DateTime.UtcNow },
                new SendDiscount { EnrollmentId = enrollmentId, PercentDiscount = 50, Creation = DateTime.UtcNow },
                new SendDiscount { EnrollmentId = enrollmentId, PercentDiscount = 50, Creation = DateTime.UtcNow },
                new SendDiscount { EnrollmentId = enrollmentId, PercentDiscount = 50, Creation = DateTime.UtcNow },
            };

            await bus.PublishBatch<SendDiscount>(list, context => context.SetSessionId(enrollmentId.ToString()), cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
