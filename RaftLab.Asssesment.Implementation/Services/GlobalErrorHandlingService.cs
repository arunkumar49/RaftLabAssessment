using NLog;
using RaftLab.Assessment.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaftLabsAssignment.Services
{
    public class GlobalErrorHandlingService : IGlobalErrorHandlingService
    {

        #region class attributes

        private static Logger _log = LogManager.GetCurrentClassLogger();
        //private static IEmailSendingService _emailService; //Implement email service if required

        #endregion

        #region public methods

        public void RegisterGlobalExceptionHandling()
        {

            // Handle unhandled exceptions
            AppDomain.CurrentDomain.UnhandledException += (sender, eventArgs) =>
            {
                Exception exception = (Exception)eventArgs.ExceptionObject;
                _log.Error(exception, "Unhandled exception caught.");
                //_emailService.SendErrorMail(exception); //Implement email service if required
            };


            // Handle unobserved task exceptions
            TaskScheduler.UnobservedTaskException += (sender, eventArgs) =>
            {
                _log.Error(eventArgs.Exception, "Unobserved task exception caught.");
                //_emailService.SendErrorMail(eventArgs.Exception); //Implement email service if required
                eventArgs.SetObserved(); // Mark the exception as observed
            };
        }

        #endregion

    }
}
