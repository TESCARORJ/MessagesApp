## Sobre

APi para executar um serviço em segundo plano que consome mensagens de uma fila RabbitMQ e processa notificações de e-mail. 
Ele se conecta ao RabbitMQ, lê as mensagens da fila, envia e-mails utilizando um cliente de API de e-mails, e confirma o processamento das mensagens.

## Funcionalidades

- Conexão com o RabbitMQ utilizando as configurações definidas (host, porta, usuário e senha).
- Leitura de mensagens de uma fila do RabbitMQ.
- Desserialização de notificações do cliente contidas na fila.
- Envio de e-mails utilizando um cliente de API (ApiEmailsClient).
- Confirmação do processamento da mensagem para o RabbitMQ.

## Requisitos

- .NET 6.0 ou superior
- RabbitMQ
- Cliente de API para envio de e-mails (ApiEmailsClient)
- Dependências:
  - `Newtonsoft.Json` para a desserialização das mensagens
  - `RabbitMQ.Client` para a conexão e manipulação da fila RabbitMQ

