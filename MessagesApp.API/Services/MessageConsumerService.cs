


using MessagesApp.API.Clients;
using MessagesApp.API.Dto;
using MessagesApp.API.Settings;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace MessagesApp.API.Services
{
    /// <summary>
    /// Serviço de segundo plano para processar a fila do RabbitMQ
    /// </summary>
    public class MessageConsumerService : BackgroundService
    {
        private readonly RabbitMQSettings _rabbitmqSettings;
        private readonly ApiEmailsClient _apiEmailsClient;
        private readonly IServiceProvider _serviceProvider;

        private IConnection _connection;
        private IModel _model;

        public MessageConsumerService(RabbitMQSettings rabbitmqSettings, ApiEmailsClient apiEmailsClient, IServiceProvider serviceProvider)
        {
            _rabbitmqSettings = rabbitmqSettings;
            _apiEmailsClient = apiEmailsClient;
            _serviceProvider = serviceProvider;

            var factory = new ConnectionFactory
            {
                HostName = _rabbitmqSettings.Host,
                Port = _rabbitmqSettings.Port,
                UserName = _rabbitmqSettings.Username,
                Password = _rabbitmqSettings.Password,
            };

            _connection = factory.CreateConnection();
            _model = _connection.CreateModel();

            _model.QueueDeclare(
                    queue: _rabbitmqSettings.Queue,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null
                );
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_model);

            //rotina para ler e processar o conteúdo da fila
            consumer.Received += async (sender, args) =>
            {
                //lendo cada registro contido na fila
                var contentArray = args.Body.ToArray();
                var contentString = Encoding.UTF8.GetString(contentArray);

                //deserializando os dados da fila..
                var notificacao = JsonConvert.DeserializeObject<NotificacaoCliente>(contentString);

                //fazendo o processamento do item lido da fila
                using (var scope = _serviceProvider.CreateScope())
                {
                    var response = await _apiEmailsClient.Send(new ApiEmailsRequest
                    {
                        To = notificacao.Email,
                        Subject = notificacao.MensagemCadastro,
                        Body = notificacao.MensagemCadastro
                    });

                    //confirmando para o RabbitMQ que a mensagem foi processada com sucesso.
                    _model.BasicAck(args.DeliveryTag, false);
                }
            };

            //executar o Received..
            _model?.BasicConsume(_rabbitmqSettings.Queue, false, consumer);
            return Task.CompletedTask;
        }
    }

}




