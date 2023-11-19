using SharedService.Kafka.Consumer;

namespace SharedService.Kafka;

public static class RegisterServiceExtensions
{
    public static IServiceCollection AddKafkaConsumer<Tk, Tv, THandler>(this IServiceCollection services,
        Action<KafkaConsumerConfig<Tk, Tv>> configAction) where THandler : class, IKafkaHandler<Tk, Tv>
    {
        services.AddScoped<IKafkaHandler<Tk, Tv>, THandler>();

        services.AddHostedService<BackgroundKafkaConsumer<Tk, Tv>>();

        services.Configure(configAction);

        return services;
    }
}