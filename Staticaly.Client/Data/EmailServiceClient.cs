using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http.Json;
using Staticaly.Client.Models;

namespace Staticaly.Client.Data
{
    public class EmailServiceClient
    {
        private readonly HttpClient httpClient;

        public EmailServiceClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<HttpResponseMessage> SendEmailAsync(EmailDTO email)
        {
            return await httpClient.PostAsJsonAsync("email", email);
        }
    }
}