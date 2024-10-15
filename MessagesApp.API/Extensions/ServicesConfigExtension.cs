using MessagesApp.API.Clients;
using MessagesApp.API.Services;
using MessagesApp.API.Settings;
using Microsoft.Extensions.Options;

namespace MessagesApp.API.Extensions
{
    public static class ServicesConfigExtension
    {
        public static IServiceCollection AddServicesConfig(this IServiceCollection services, IConfiguration configuration)
        {
            var rabbitMQSettings = new RabbitMQSettings();
            new ConfigureFromConfigurationOptions<RabbitMQSettings>
                (configuration.GetSection("RabbitMQSettings"))
                .Configure(rabbitMQSettings);

            services.AddSingleton(rabbitMQSettings);

            var apiEmailsSettings = new ApiEmailsSettings();
            new ConfigureFromConfigurationOptions<ApiEmailsSettings>
                (configuration.GetSection("ApiEmailsSettings"))
                .Configure(apiEmailsSettings);

            services.AddSingleton(apiEmailsSettings);
            services.AddTransient<ApiEmailsClient>();

            return services;
        }
    }

}
