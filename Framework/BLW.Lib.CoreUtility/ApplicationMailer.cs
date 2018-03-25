using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLW.Lib.DbHelper;

namespace BLW.Lib.CoreUtility
{
   public class ApplicationMailer
    {
       /// <summary>
        /// Method:SendingMail
       /// Description:Send the mail of the all application files of any application.
       /// </summary>
       /// <param name="appDetails"></param>
       /// <param name="files"></param>
       /// <param name="appTypeid"></param>
       /// <param name="emailType"></param>
       /// <returns></returns>
        public string SendingMail(Application appDetails, List<String> files, int appTypeid, string emailType,string runNumber)
        {
            try
            {
                EmailTemplate templateObj = new EmailTemplate();
                SmtpEmail smtpEmailObj;
                EmailGroup objEmailGroup;
                string mailTo;
                EmailSetting(appDetails.ApplicationsId, appTypeid, emailType, out smtpEmailObj, out objEmailGroup, out mailTo);


                if (!String.IsNullOrEmpty(mailTo) && !String.IsNullOrEmpty(objEmailGroup.EmailFrom) && !String.IsNullOrEmpty(SmtpEmail.SmtpHost))
                {
                    //get email templete
                    var templateDetails = templateObj.AppFilesDetails(appDetails.ApplicationName, files, runNumber);
                    //objEmailGroup.Subject can be changed after checking to ""
                    var errorMgs = smtpEmailObj.SendEmail(mailTo, objEmailGroup.EmailFrom, objEmailGroup.Subject, templateDetails);
                    return errorMgs;

                }
                else
                {
                    return "Not sending the mail";
                }
            }
            catch (Exception)
            {
                
                throw;
            }
            
        }
        /// <summary>
        /// Method:SendingStatusMail
        /// Description:Send the mail of status manager .
        /// </summary>
        /// <param name="appDetails"></param>
        /// <param name="files"></param>
        /// <param name="appTypeid"></param>
        /// <param name="emailType"></param>
        /// <returns></returns>
        public string SendingStatusMail(int appId, Dictionary<string,string> configurationVariables, int appTypeid, string emailType,string subject)
        {
            try
            {
                SmtpEmail smtpEmailObj;
                EmailGroup objEmailGroup;
                string mailTo;
                EmailSetting(appId, appTypeid, emailType, out smtpEmailObj, out objEmailGroup, out mailTo);


                if (!String.IsNullOrEmpty(mailTo) && !String.IsNullOrEmpty(objEmailGroup.EmailFrom) && !String.IsNullOrEmpty(SmtpEmail.SmtpHost))
                {
                    //get email templete

                    //  var templateDetails = objEmailGroup.EmailTemplate;
                    SmtpEmail.TemplatePath = objEmailGroup.EmailTemplate;
                    foreach (var item in configurationVariables)
                    {
                        SmtpEmail.TemplatePath = SmtpEmail.TemplatePath.Replace(item.Key, item.Value);
                    }
                    //   smtpEmailObj.GetProcessedContent(configurationVariables);


                    //objEmailGroup.Subject can be changed after checking to ""
                    var errorMgs = smtpEmailObj.SendEmail(mailTo, objEmailGroup.EmailFrom, subject, SmtpEmail.TemplatePath);
                    return errorMgs;

                }
                else
                {
                    return "Not sending the mail";
                }
            }
            catch (Exception)
            {
                
                throw ;
            }
           
        }
        public static void EmailSetting(int appId, int appTypeid, string emailType, out SmtpEmail smtpEmailObj, out EmailGroup objEmailGroup, out string mailTo)
        {
            
            EmailGroupDal objEmailGroupDal = new EmailGroupDal();
            EmailConfigDetailDal objEmailConfigDetailDal = new EmailConfigDetailDal();
            #region smtp Setting
            SmtpDetailsDal objSmtpDetailsDal = new SmtpDetailsDal();
            try
            {
                SmtpDetail objSmtpDetail = objSmtpDetailsDal.GetAtiveSmtpDetails();
                SmtpEmail.SmtpHost = objSmtpDetail.SmtpHost;
                SmtpEmail.SmtpUser = objSmtpDetail.SmtpUser;
                SmtpEmail.SmtpPwd = objSmtpDetail.Password;

                smtpEmailObj = SmtpEmail.GetSmtpEmailInstance();

            #endregion
                //"Validation" to appconfig
                objEmailGroup = objEmailGroupDal.GetEmailGroupByApptypeIdAndEmailType(appTypeid, emailType);



                #region Emailto Setting
                var toemailList = objEmailConfigDetailDal.GetEmailListByGroupIdAndAppId(objEmailGroup.Id, appId);
                mailTo = "";
                for (int index = 0; index < toemailList.Count; index++)
                {
                    if (index == toemailList.Count - 1)
                        mailTo += toemailList[index];
                    else
                        mailTo += toemailList[index] + ",";
                }

            }
            catch (Exception ex)
            {
                
                throw;
            }
           
            #endregion
        }
       
    }
}
