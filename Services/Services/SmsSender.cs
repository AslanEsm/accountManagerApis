using Common;
using Microsoft.Extensions.Options;
using Services.Interfaces;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Services.Services
{
    public class SmsSender : ISmsSender
    {
        private readonly SiteSettings _siteSetting;

        public SmsSender(IOptionsSnapshot<SiteSettings> settings)
        {
            _siteSetting = settings.Value;
        }

        public async Task<string> SendAuthSmsAsync(string code, string phoneNumber)
        {
            var apiKey = _siteSetting.AuthSms.ApiKey;
            var template = _siteSetting.AuthSms.Template;
            HttpClient httpClient = new HttpClient();
            var httpResponse = await httpClient.GetAsync($"https://api.kavenegar.com/v1/{apiKey}/verify/lookup.json?receptor={phoneNumber}&token={code}&template={template}");
            return httpResponse.StatusCode == HttpStatusCode.OK ? "Success" : "Fail";
        }
    }
}