using Twilio.Types;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Microsoft.Extensions.Configuration;

namespace BudgetMobApp.Services
{
    public class SmsService
    {
        private readonly string _accountSid;
        private readonly string _authToken;
        private readonly string _twilioPhoneNumber;

        public SmsService(IConfiguration config)
        {
            _accountSid = config["SmsSettings:accountSid"];
            _authToken = config["SmsSettings:authToken"];
            _twilioPhoneNumber = config["SmsSettings:twilioPhoneNumber"];

            TwilioClient.Init(_accountSid, _authToken);
        }

        public void SendSms(List<string> toPhoneNumbers, string message)
        {
            foreach (var number in toPhoneNumbers)
            {
                var formattedNumber = number.StartsWith("+91") ? number : $"+91{number}";

                MessageResource.Create(
                    to: new PhoneNumber(formattedNumber),
                    from: new PhoneNumber(_twilioPhoneNumber),
                    body: message
                );
            }
        }
    }
}
