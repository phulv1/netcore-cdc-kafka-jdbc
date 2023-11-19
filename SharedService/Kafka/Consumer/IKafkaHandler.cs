﻿namespace SharedService.Kafka.Consumer;

public interface IKafkaHandler<Tk, Tv>
{
    Task HandleAsync(Tk key, Tv value);
}
