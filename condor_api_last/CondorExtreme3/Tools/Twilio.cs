using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Twilio;
using Twilio.Types;
using Twilio.Rest.Api.V2010.Account;

/*-----------------------------------+
 * Author: Ilhan Karic               |
 * Module: Users                     |
 * Last Modified: 1/10/2018 @ 19:36  |
 * Copyright: Condor Coorporation    |
 *-----------------------------------*/
namespace CondorExtreme3.Tools
{
    public static class TwilioHelper
    {
        // Sensitive information
        static string TwilioNumber = "+17792054375";
        static string AccountSid = "ACb960b03afb26043f70f5ddf899c66109";
        static string AuthToken = "bd0bf130f31dfd97aa500cc215e67738";

        /// <summary>
        ///     Sends an SMS to the given phone number with the given text.
        /// </summary>
        /// <param name="PhoneNumber">The phone number to send SMS to</param>
        /// <param name="Message">The message to send</param>
        public static void SendSMS(string PhoneNumber, string Message)
        {
            TwilioClient.Init(AccountSid, AuthToken);
            // Le C# wizzardry
            var message = MessageResource.Create(
                    to: new PhoneNumber(PhoneNumber),
                    from: new PhoneNumber(TwilioNumber),
                    body: Message
                );
        }
    }
}