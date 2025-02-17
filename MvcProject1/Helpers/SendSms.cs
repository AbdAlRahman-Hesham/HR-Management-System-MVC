using Microsoft.Extensions.Options;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.TwiML.Voice;

namespace MvcProject1.PL.Helpers
{
    public interface ISendSms
    {
        public MessageResource Send(Sms sms); // Change ResourceMessage to MessageResource
    }
    
    public class SendSms: ISendSms
    {
        private readonly TwilioAppSetting options;

        public SendSms(IOptions<TwilioAppSetting> options)
        {
            this.options = options.Value;
        }
        public MessageResource Send(Sms sms)
        {
            TwilioClient.Init(options.AccountSID, options.AuthToken);
            return MessageResource.Create(
                body: sms.Message,
                to: sms.To,
                from: new Twilio.Types.PhoneNumber(options.PhoneNumber)
                );


        }
    }
}
