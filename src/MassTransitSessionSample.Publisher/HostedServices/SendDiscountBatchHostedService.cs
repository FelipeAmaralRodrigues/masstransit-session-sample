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

            var enrollmentId1 = Guid.NewGuid();
            var list1 = new List<SendDiscount>
            {
                new SendDiscount { EnrollmentId = enrollmentId1, PercentDiscount = 50, Creation = DateTime.UtcNow },
                new SendDiscount { EnrollmentId = enrollmentId1, PercentDiscount = 50, Creation = DateTime.UtcNow },
                new SendDiscount { EnrollmentId = enrollmentId1, PercentDiscount = 50, Creation = DateTime.UtcNow },
                new SendDiscount { EnrollmentId = enrollmentId1, PercentDiscount = 50, Creation = DateTime.UtcNow },
                new SendDiscount { EnrollmentId = enrollmentId1, PercentDiscount = 50, Creation = DateTime.UtcNow },
                new SendDiscount { EnrollmentId = enrollmentId1, PercentDiscount = 50, Creation = DateTime.UtcNow },
                new SendDiscount { EnrollmentId = enrollmentId1, PercentDiscount = 50, Creation = DateTime.UtcNow },
                new SendDiscount { EnrollmentId = enrollmentId1, PercentDiscount = 50, Creation = DateTime.UtcNow },
                new SendDiscount { EnrollmentId = enrollmentId1, PercentDiscount = 50, Creation = DateTime.UtcNow },
                new SendDiscount { EnrollmentId = enrollmentId1, PercentDiscount = 50, Creation = DateTime.UtcNow }
            };

            var e1 = await bus.GetSendEndpoint(new Uri("queue:send-discount-with-session"));
            await e1.SendBatch<SendDiscount>(list1, context => context.SetSessionId(enrollmentId1.ToString()), cancellationToken);

            Thread.Sleep(2000);

            var enrollmentId2 = Guid.NewGuid();
            var list2 = new List<SendDiscount>
            {
                new SendDiscount { EnrollmentId = enrollmentId2, PercentDiscount = 50, Creation = DateTime.UtcNow },
                new SendDiscount { EnrollmentId = enrollmentId2, PercentDiscount = 50, Creation = DateTime.UtcNow },
                new SendDiscount { EnrollmentId = enrollmentId2, PercentDiscount = 50, Creation = DateTime.UtcNow },
                new SendDiscount { EnrollmentId = enrollmentId2, PercentDiscount = 50, Creation = DateTime.UtcNow },
                new SendDiscount { EnrollmentId = enrollmentId2, PercentDiscount = 50, Creation = DateTime.UtcNow },
                new SendDiscount { EnrollmentId = enrollmentId2, PercentDiscount = 50, Creation = DateTime.UtcNow },
                new SendDiscount { EnrollmentId = enrollmentId2, PercentDiscount = 50, Creation = DateTime.UtcNow },
                new SendDiscount { EnrollmentId = enrollmentId2, PercentDiscount = 50, Creation = DateTime.UtcNow },
                new SendDiscount { EnrollmentId = enrollmentId2, PercentDiscount = 50, Creation = DateTime.UtcNow },
                new SendDiscount { EnrollmentId = enrollmentId2, PercentDiscount = 50, Creation = DateTime.UtcNow }
            };
            var e2 = await bus.GetSendEndpoint(new Uri("queue:send-discount-without-session"));
            await e2.SendBatch<SendDiscount>(list2, cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
