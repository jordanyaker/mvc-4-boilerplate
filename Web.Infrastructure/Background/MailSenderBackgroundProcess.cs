//namespace Boilerplate.Background {
//    using System;
//    using System.Diagnostics;
//    using System.Net;
//    using System.Net.Mail;
//    using SendGrid;
//    using SendGrid.Transport;
//    using ServiceStack.Text;
//    using Boilerplate.Events;
//    using NLog;

//    public class MailSenderBackgroundProcess : BackgroundProcess {
//        // -------------------------------------------------------------------------------------
//        // Constants
//        // -------------------------------------------------------------------------------------
//        static readonly string PASSWORD_RESET_REQUEST_SUBJECT = "Password Reset Instructions";
//        static readonly string PASSWORD_RESET_REQUEST_MESSAGE =
//            "We've sent you this message as a result of a recent request that we received to reset the password for your account.\r\n\r\n" +
//            "In order to regain access to your account, please reset your password using the following steps:\r\n\r\n" +
//            "1. Click the link below to open a new and secure browser window.\r\n" +
//            "2. Enter the requested information and follow the instructions to reset your password.\r\n\r\n" +
//            "{0}\r\n\r\n" + 
//            "PLEASE NOTE: This link will only be good for the next hour.\r\n\r\n" +
//            "If you believe that you have received this message in error, please contact us using at contact@Boilerplate.io.\r\n\r\n" +
//            "Thanks and Regards,\r\n" +
//            "Team Boilerplate";
//        static readonly string INVALID_PASSWORD_RESET_REQUEST_SUBJECT = "Account Access Attempted";
//        static readonly string INVALID_PASSWORD_RESET_REQUEST_MESSAGE = 
//            "You (or someone else) entered this email address when trying to change the password of a Boilerplate account.\r\n\r\n" + 
//            "However, this e-mail address is not in our database of registered users and as a result, the attempt has failed.\r\n\r\n" +
//            "If you are a Boilerplate customer and were expecting this email, please try again using the e-mail address you gave when you opened your account.\r\n\r\n" +
//            "If you are not a Boilerplate customer, please ignore this e-mail.\r\n\r\n" +
//            "Thanks and Regards,\r\n" +
//            "Team Boilerplate";

//        // -------------------------------------------------------------------------------------
//        // Fields
//        // -------------------------------------------------------------------------------------
//        private IDisposable _passwordResetRequestEventToken;
//        private IDisposable _invalidPasswordResetRequestEventToken;
//        private ITransport _transport;

//        // -------------------------------------------------------------------------------------
//        // Properties
//        // -------------------------------------------------------------------------------------
//        public IEventAggregator EventAggregator { get; set; }
//        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
//        public string Password { get; set; }
//        public string PostmasterAddress { get; set; }
//        public string PostmasterName { get; set; }
//        public ITransport Transport {
//            get {
//                if (_transport == null) {
//                    var credentials = new NetworkCredential(Username, Password);
//                    _transport = SMTP.GetInstance(credentials);
//                }

//                return _transport;
//            }
//            set {
//                _transport = value;
//            }
//        }
//        public string Username { get; set; }

//        // -------------------------------------------------------------------------------------
//        // Methods
//        // -------------------------------------------------------------------------------------
//        protected override void OnStart() {
//            _passwordResetRequestEventToken = EventAggregator.GetEvent<PasswordResetRequestEvent>().Subscribe(e => OnPasswordResetRequestEvent(e));
//            _invalidPasswordResetRequestEventToken = EventAggregator.GetEvent<InvalidPasswordResetRequestEvent>().Subscribe(e => OnInvalidPasswordResetRequestEvent(e));
//        }
//        protected override void OnStop() {
//            _passwordResetRequestEventToken.Dispose();
//            _invalidPasswordResetRequestEventToken.Dispose();
//        }
//        private void OnInvalidPasswordResetRequestEvent(InvalidPasswordResetRequestEvent e) {
//            var message = Mail.GetInstance();

//            message.AddTo(e.Email);

//            var address = new MailAddress(PostmasterAddress, PostmasterName);
//            message.From = address;
//            message.ReplyTo = new[] { address };

//            message.Subject = INVALID_PASSWORD_RESET_REQUEST_SUBJECT;
//            message.Text = INVALID_PASSWORD_RESET_REQUEST_MESSAGE;
            
//            try {
//                Transport.Deliver(message);
//            } catch (Exception ex) {
//                var error = LogMessageStrings.UNEXPECTED_BACKGROUND_EXCEPTION.FormatWith("MailSenderBackgroundProcess.OnInvalidPasswordResetRequestEvent");
//                Log.ErrorException(error, ex);
//            }
//        }
//        private void OnPasswordResetRequestEvent(PasswordResetRequestEvent e) {
//            var message = Mail.GetInstance();

//            message.AddTo("{0} <{1}>".FormatWith(e.Name, e.Email));

//            var address = new MailAddress(PostmasterAddress, PostmasterName);
//            message.From = address;
//            message.ReplyTo = new[] { address };

//            message.Subject = PASSWORD_RESET_REQUEST_SUBJECT;
//            message.Text = PASSWORD_RESET_REQUEST_MESSAGE.FormatWith(e.Url);
 
//            try {
//                Transport.Deliver(message);
//            } catch (Exception ex) {
//                var error = LogMessageStrings.UNEXPECTED_BACKGROUND_EXCEPTION.FormatWith("MailSenderBackgroundProcess.OnPasswordResetRequestEvent");
//                Log.ErrorException(error, ex);
//            }
//        }
//    }
//}
