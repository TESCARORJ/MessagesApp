using MessagesApp.API.Dto;
using MessagesApp.API.Settings;
using Newtonsoft.Json;
using System.Text;

namespace MessagesApp.API.Clients
{
    /// <summary>
    /// Classe para envio de requisições para API de entrega de emails
    /// </summary>
    public class ApiEmailsClient
    {
        private readonly ApiEmailsSettings _apiEmailsSettings;

        public ApiEmailsClient(ApiEmailsSettings apiEmailsSettings)
        {
            _apiEmailsSettings = apiEmailsSettings;
        }

        public async Task<ApiEmailsResponse?> Send(ApiEmailsRequest request)
        {
            //instanciando o HttpClient
            using (var client = new HttpClient())
            {
                //serializando os dados da requisição para JSON
                var json = JsonConvert.SerializeObject(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                //definindo os parametros para autenticação
                var auth = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_apiEmailsSettings.User}:{_apiEmailsSettings.Pass}"));
                client.DefaultRequestHeaders.Add("Authorization", $"Basic {auth}");

                //fazendo a chamada para o endpoint POST /api/email/send
                var result = await client.PostAsync($"{_apiEmailsSettings.Endpoint}api/email/send", content);

                //verificando se o retorno não foi sucesso
                if (!result.IsSuccessStatusCode)
                    throw new ApplicationException("Falha ao enviar email.");

                //deserializando e retornar a resposta
                var response = await result.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ApiEmailsResponse>(response);
            }
        }
    }


}

