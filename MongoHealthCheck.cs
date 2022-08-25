using Microsoft.Extensions.Diagnostics.HealthChecks;

public class MongoHealthCheck : IHealthCheck
{
    public MongoHealthCheck()
    {

    }

    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}