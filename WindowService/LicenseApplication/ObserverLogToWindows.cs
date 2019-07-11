
using System;
using System.Windows;
using System.Windows.Forms;
using System.Drawing;

namespace BLW.Lib.Log
{
    /// <summary>
    /// Writes log events to the diagnostic trace.
    /// </summary>
    /// <remarks>
    /// GoF Design Pattern: Observer.
    /// The Observer Design Pattern allows this class to attach itself to an
    /// the logger and 'listen' to certain events and be notified of the event. 
    /// </remarks>
    public class ObserverLogToWindows : ILog
    {
        public ObserverLogToWindows(Label p_lblMessage)
        {
            lblMessage = p_lblMessage;
        }

        /// <summary>
        /// Writes a log request to the diagnostic trace on the page.
        /// </summary>
        /// <param name="sender">Sender of the log request.</param>
        /// <param name="e">Parameters of the log request.</param>
        public void Log(object sender, LogEventArgs e)
        {
            try
            {
                lblMessage.Text = string.Empty;
                lblMessage.Visible = true;
                // Example code of entering a log event to output console
                string message = e.SeverityString + ": " + e.Message;
                switch (e.Severity)
                {
                    case LogSeverity.Info:
                        lblMessage.ForeColor = Color.Green;
                        lblMessage.Text = message;
                        break;
                    case LogSeverity.Error:
                    case LogSeverity.Fatal:
                        lblMessage.ForeColor = Color.Red;
                        lblMessage.Text = message;
                        break;
                }
            }
            catch (InvalidOperationException ex)
            {
                throw ex;
            }

        }
        /// <summary>
        /// 
        /// </summary>
        Label lblMessage;
    }
}
