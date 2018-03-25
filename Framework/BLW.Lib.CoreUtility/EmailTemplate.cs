using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace BLW.Lib.CoreUtility
{
   public class EmailTemplate
    {

        //******************************************************************
        //--------------- this class contains the email format--------------
        //******************************************************************





        /// <summary>
        /// this function gets the html body for the email notification.
        /// </summary>
        /// <param name="date">downloaded file date</param>
        /// <param name="SourcePath">path from where file is downloaded</param>
        /// <param name="downloadedFiles"> list of downloaded files.</param>
        /// <returns></returns>
        public string AppFilesDetails(string appName,List<string> Files,string runNo)
        {
            StringBuilder objStringBuilder = new StringBuilder();
            string files = string.Empty;
            //Email Salutation
            objStringBuilder.Append("Hi All,<br/> Details of files downloaded from FTP are given as <br/><br/><br/>");
            objStringBuilder.Append("<table style='font-family:Arial, Helvetica, sans-serif; font-size:12px; border:1px solid #CCC;!important' cellpadding='1' cellspacing='0' border='1'>");
            objStringBuilder.Append("<tr bgcolor='#f0f0f0' style='font-weight:bold;'>");
            objStringBuilder.Append("<td  align='left' valign='top' style=' padding:5px;'>");            
            objStringBuilder.Append("Application Name");
            objStringBuilder.Append("</td>");
            objStringBuilder.Append("<td>");
            objStringBuilder.Append("RunNumber");
            objStringBuilder.Append("</td>");
            objStringBuilder.Append("<td>");
            objStringBuilder.Append("Application Files");
            objStringBuilder.Append("</td>");
            objStringBuilder.Append("</tr>");

         
            objStringBuilder.Append("<tr bgcolor='#f0f0f0';'>");
           
            objStringBuilder.Append("<td  align='left' valign='top' style=' border-right:1px solid #CCC; border-bottom:1px solid #CCC; padding:5px;'>");
            objStringBuilder.Append(appName+"</td>");

            objStringBuilder.Append("<td  align='left' valign='top' style=' border-right:1px solid #CCC; border-bottom:1px solid #CCC; padding:5px;'>");
            objStringBuilder.Append(runNo + "</td>");

            objStringBuilder.Append("<td  align='left' valign='top' style=' border-right:1px solid #CCC; border-bottom:1px solid #CCC; padding:5px;'>");
         
            foreach (string file in Files)
            {
              objStringBuilder.Append("<li>"+ file +"</li>");
                
            }
            //table body
                            
            objStringBuilder.Append( "</td>");
            objStringBuilder.Append("</tr>");
            objStringBuilder.Append("</table>");

            objStringBuilder.Append(" <br />");
            //Create table for file report

            //
            objStringBuilder.Append("<table style='font-family:Arial, Helvetica, sans-serif; font-size:12px;' cellpadding='0' cellspacing='0' border='0'>");

            objStringBuilder.Append("<tr>");
            objStringBuilder.Append("<td colspan='2' valigh='top' align='left'  style='padding:5px;'>");
            objStringBuilder.Append("Thank You");
            objStringBuilder.Append("</td>");
            objStringBuilder.Append("</tr>");
            objStringBuilder.Append("<tr>");
            objStringBuilder.Append("<td  align='left' valign='top' style='padding:5px;'>");
            objStringBuilder.Append("HC Automation");
            objStringBuilder.Append("</td>");
            objStringBuilder.Append("</tr>");
            objStringBuilder.Append("<tr>");
            objStringBuilder.Append("<td style='padding:5px;'>");
            objStringBuilder.Append("&nbsp;</td>");
            objStringBuilder.Append("</tr>");
            objStringBuilder.Append("</table>");

            return objStringBuilder.ToString();
        }

        /// <summary>
        /// this function gets the html body for file validation.
        /// </summary>
        /// <param name="date"></param>
        /// <param name="ValidatedFileName"></param>
        /// <param name="mask"></param>
        /// <returns></returns>
        public string GetHTMLBodyForFiles(string FileName,string heading, string applicationName = "",string date = "")
        {
            StringBuilder objStringBuilder = new StringBuilder();
            //Email Salutation
            objStringBuilder.Append("<table style='font-family:Arial, Helvetica, sans-serif; font-size:12px;' cellpadding='0' cellspacing='0' border='0'>");
            objStringBuilder.Append("<tr>");
            objStringBuilder.Append("<td  align='left' valign='top' style=' padding:5px;'>");
            objStringBuilder.Append(heading);
            objStringBuilder.Append("</td>");
            objStringBuilder.Append("</tr>");
            objStringBuilder.Append("</table>");
            //Define table header
            objStringBuilder.Append("<br />");
            objStringBuilder.Append("<table style='font-family:Arial, Helvetica, sans-serif; font-size:12px; border:1px solid #CCC;' cellpadding='1' cellspacing='0' border='1'>");
            objStringBuilder.Append("<tr bgcolor='#f0f0f0' style='font-weight:bold;'>");
            if (date != string.Empty)
            {
                objStringBuilder.Append("<td  align='center' valign='top' style=' border-right:1px solid #CCC; border-bottom:1px solid #CCC; padding:5px;'>");
                objStringBuilder.Append("Date</td>");
            }
            objStringBuilder.Append("<td  align='center' valign='top' style=' border-right:1px solid #CCC; border-bottom:1px solid #CCC; padding:5px;'>");
            objStringBuilder.Append(" File Name</td>");
            if (applicationName != string.Empty)
            {
                objStringBuilder.Append("<td  align='center' valign='top' style=' border-right:1px solid #CCC; border-bottom:1px solid #CCC; padding:5px;'>");
                objStringBuilder.Append("Application Name</td>");
            }
            objStringBuilder.Append("</tr>");
            
            //table body
            objStringBuilder.Append("<tr>");
            if (date != string.Empty)
            {
                objStringBuilder.Append("<td  align='center' valign='top' style='border-right:1px solid #CCC; padding:5px;'>");
                objStringBuilder.Append(date + "</td>");
            }
            objStringBuilder.Append("<td  align='center' valign='top' style=' border-right:1px solid #CCC; padding:5px;'>");
            objStringBuilder.Append(FileName + "</td>");
            if (applicationName != string.Empty)
            {
                objStringBuilder.Append("<td  align='center' valign='top' style=' border-right:1px solid #CCC; padding:5px;'>");
                objStringBuilder.Append(applicationName + "</td>");
            }
            objStringBuilder.Append("</tr>");
            objStringBuilder.Append("</table>");
            objStringBuilder.Append(" <br />");
            //Create table for file report
            objStringBuilder.Append("<table style='font-family:Arial, Helvetica, sans-serif; font-size:12px;' cellpadding='0' cellspacing='0' border='0'>");
            objStringBuilder.Append("<tr>");
            objStringBuilder.Append("<td colspan='2' valigh='top' align='left'  style='padding:5px;'>");
            objStringBuilder.Append("Thank You");
            objStringBuilder.Append("</td>");
            objStringBuilder.Append("</tr>");
            objStringBuilder.Append("<tr>");
            objStringBuilder.Append("<td  align='left' valign='top' style='padding:5px;'>");
            objStringBuilder.Append("HCA Automation");
            objStringBuilder.Append("</td>");
            objStringBuilder.Append("</tr>");
            objStringBuilder.Append("<tr>");
            objStringBuilder.Append("<td style='padding:5px;'>");
            objStringBuilder.Append("&nbsp;</td>");
            objStringBuilder.Append("</tr>");
            objStringBuilder.Append("</table>");

            return objStringBuilder.ToString();
        }


        /// <summary>
        /// this function contains the error email template.
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
     

      
        public string GetSplitedFiles(List<string> fileNameArr,string heading)
        {
            StringBuilder objStringBuilder = new StringBuilder();
            //Email Salutation
            objStringBuilder.Append("<table style='font-family:Arial, Helvetica, sans-serif; font-size:12px;' cellpadding='0' cellspacing='0' border='0'>");
            objStringBuilder.Append("<tr>");
            objStringBuilder.Append("<td  align='left' valign='top' style=' padding:5px;'>");
            objStringBuilder.Append(heading);
            objStringBuilder.Append("</td>");
            objStringBuilder.Append("</tr>");
            objStringBuilder.Append("</table>");
            //Define table header
            objStringBuilder.Append("<br />");
            objStringBuilder.Append("<table style='font-family:Arial, Helvetica, sans-serif; font-size:12px; border:1px solid #CCC;!important' cellpadding='1' cellspacing='0' border='0'>");
            objStringBuilder.Append("<tr bgcolor='#f0f0f0' style='font-weight:bold;'>");

            objStringBuilder.Append("<td  align='center' valign='top' style=' border-right:1px solid #CCC; border-bottom:1px solid #CCC; padding:5px;'>");
            objStringBuilder.Append("FileName</td>");
            objStringBuilder.Append("</tr>");
            //table body
           
            foreach (var item in fileNameArr)
            {
                objStringBuilder.Append("<tr>");
                objStringBuilder.Append("<td  align='center' valign='top' style=' border-right:1px solid #CCC; padding:5px;'>");
                objStringBuilder.Append(Path.GetFileName(item).ToString() + "</td>");
                objStringBuilder.Append("</tr>"); 
            }
            

           
            objStringBuilder.Append("</table>");
            objStringBuilder.Append(" <br />");
            //Create table for file report
            objStringBuilder.Append("<table style='font-family:Arial, Helvetica, sans-serif; font-size:12px;' cellpadding='0' cellspacing='0' border='0'>");
            objStringBuilder.Append("<tr>");
            objStringBuilder.Append("<td colspan='2' valigh='top' align='left'  style='padding:5px;'>");
            objStringBuilder.Append("Thank You");
            objStringBuilder.Append("</td>");
            objStringBuilder.Append("</tr>");
            objStringBuilder.Append("<tr>");
            objStringBuilder.Append("<td  align='left' valign='top' style='padding:5px;'>");
            objStringBuilder.Append("HCA Automation");
            objStringBuilder.Append("</td>");
            objStringBuilder.Append("</tr>");
            objStringBuilder.Append("<tr>");
            objStringBuilder.Append("<td style='padding:5px;'>");
            objStringBuilder.Append("&nbsp;</td>");
            objStringBuilder.Append("</tr>");
            objStringBuilder.Append("</table>");
            return objStringBuilder.ToString();
        }

        
    }
}