using DbHander;
using DbHander;
using DbHander;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DbHander.EmailService
{
    public class EmailService
    {
        #region Global Variables
        ISmtpDetailsRepository smtpDetailsRepository;
        IContactInfoRepository contactInfoRepository;
        IEmailTemplateRepository emailTemplateRepository;
        IEmailTrackingRepository emailTrackingRepository;
        IRunDetailsRepository runDetailsRepository;
        IUserRepository userRepository;
        IClientRepository clientRepository;
        IApplicationRepository applicationRepository;
        IComponentRepository componentRepository;
        IRawFileRepository rawFileRepository;
        #endregion

        #region Ctor
        public EmailService()
        {
            smtpDetailsRepository = new SmtpDetailsRepository();
            contactInfoRepository = new ContactInfoRepository();
            emailTemplateRepository = new EmailTemplateRepository();
            emailTrackingRepository = new EmailTrackingRepository();
            runDetailsRepository = new RunDetailsRepository();
            userRepository = new UserRepository();
            clientRepository = new ClientRepository();
            applicationRepository = new ApplicationRepository();
            componentRepository = new ComponentRepository();
            rawFileRepository = new RawFileRepository();
        }
        #endregion

        #region IEmailService Members

        public bool SaveEmailTemplate(EmailTemplate template)
        {
            bool checkIfExist = CheckTempdateAlredyExistByTemplateID(template.ClientId, template.ApplicationId, template.ApplicationComponentId, template.EmailToken, template.EmailTemplateId);
            if (checkIfExist == false)
                template.EmailTemplateId = 0;
            emailTemplateRepository.Save(template);
            return true;
        }

        public bool SaveEmailTracking(EmailTracking tracking)
        {
            emailTrackingRepository.Save(tracking);
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="emailIds">Comma Seperated email ids</param>
        /// <returns></returns>
        public List<ContactInfo> GetContactsByCommaSaperatedEmailIds(string emailIds)
        {
            var del = new char[] { ',' };
            var dlList = emailIds.Split(del, StringSplitOptions.RemoveEmptyEntries);
            return GetContactsByEmailIds(dlList);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="emailIds"></param>
        /// <returns></returns>
        public List<ContactInfo> GetContactsByEmailIds(string[] emailIds)
        {
            var emails = contactInfoRepository.GetContactInfoListbyId(emailIds);
            return emails.ToList();
        }

        /// <summary>
        /// Get all emails by current logged in user
        /// </summary>
        /// <param name="userID">user</param>
        /// <returns>return email IDs</returns>
        public string[] GetEmailIDsByUser(int userID)
        {
            //If user id is 0, then return empty list
            if (userID == 0)
                return new List<string>().ToArray();
            //check is user exists
            var userDao = userRepository.FindAll().FirstOrDefault(x=> x.Id == userID.ToString());
            if (userDao == null)
                return new List<string>().ToArray();
            var emailIDs = new List<string>();
            //Get all emails from client table
            //Get all client ids from client user table
            //var clientUsers = (from cu in clientUserRepository.FindAllActive()
            //                   where cu.UserId == userID && cu.ClientId > 0
            //                   select cu.ClientId).ToArray();
            ////if client user have some value then it should get client users
            //if (clientUsers != null)
            //{
            //    var clientEmailIDs = (from client in clientRepository.FindAllActive()
            //                          where clientUsers.Contains(client.ClientId)
            //                          select client.EmailAddress).ToArray();
            //    //Add client email list to collection if not null
            //    if (clientEmailIDs != null)
            //        emailIDs.AddRange(clientEmailIDs);
            //}
            //Get all emails from user table
            var userEmailIDs = (from user in userRepository.FindAllActive()
                                where user.Id == userID.ToString()
                                select user.Email).ToArray();
            //Add user email list to collection if not null
            if (userEmailIDs != null)
                emailIDs.AddRange(userEmailIDs);
            //Get all emails from contact information
            var contactEmailIDs = (from contact in contactInfoRepository.FindAllActive()
                                   select contact.EmailAddress).ToArray();
            //Add contact email list to collection if not null
            if (contactEmailIDs != null)
                emailIDs.AddRange(contactEmailIDs);

            var completeIDs = new List<string>();
            //Split all email IDs as comma saperated
            foreach (var emailID in emailIDs)
                completeIDs.AddRange(emailID.Split(new string[] { ",", ";" }, StringSplitOptions.RemoveEmptyEntries));

            //return list of email IDs
            return completeIDs.ToArray();
        }

        /// <summary>
        /// Get email template  by keywords (e.g. - clientId, applicationId, componentId, token, levelId, runNumber, runNumberId)
        /// <para>Any of these parameters can be empty/null or -1</para>
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="applicationId"></param>
        /// <param name="componentId"></param>
        /// <param name="token"></param>
        /// <param name="levelId"></param>
        /// <param name="runNumber"></param>
        /// <param name="runNumberId"></param>
        /// <returns></returns>
        public EmailTemplate GetEmailTemplate(int? clientId = -1, int? applicationId = -1, int? componentId = -1, string token = null, int? levelId = -1, string runNumber = null, int? runNumberId = -1)
        {
            //Get run number id by runnumber 
            if (!string.IsNullOrEmpty(runNumber))
            {
                var runNumberDetails = runDetailsRepository.GetRunDetailByRunNumber(runNumber);
                runNumberId = runNumberDetails.RunDetailId;
            }
            int nextUserLevel = GetNextLevel(runNumberId, clientId, applicationId, componentId, token);
            if (nextUserLevel == 0)
                return null;

            var emailTemplates = GetOverridedEmailTemplate(clientId, applicationId, componentId, token, nextUserLevel);
            //if template is inactive, return null

            bool templateStatus = Convert.ToBoolean(emailTemplates.Status);
            if (templateStatus)
                return emailTemplates;

            return null; //It seems that something gets wrong, So return null to avoid picking of wrong template
        }

        public List<EmailTemplate> GetEmailTemplates(int? clientId = -1, int? applicationId = -1, int? componentId = -1, string token = null)
        {
            return GetOverridedEmailTemplates(clientId, applicationId, componentId, token);
        }
        public List<EmailTemplate> GetEmailTemplatesForWeb(int? clientId = -1, int? applicationId = -1, int? componentId = -1, string token = null)
        {
            return GetOverridedEmailTemplatesForWeb(clientId, applicationId, componentId, token);
        }
        /// <summary>
        /// Convert email template to email tracking object
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        public EmailTracking ConvertToTracking(EmailTemplate template, int runNumber, EmailStatusType status)
        {
            if (template == null)
                return null;
            EmailTracking tracking = new EmailTracking()
            {
                RunNumberId = runNumber,
                FromEmailId = template.EmailFromSmtpId.ToString(),
                EmailTemplateId = template.EmailTemplateId,
                Subjects = template.Subject,
                Body = template.Body
            };
            var runDetail = runDetailsRepository.Find(runNumber);
            var tokens = new Dictionary<string, string>();
            var client = template.ClientId.Equals(-1) ? null : clientRepository.Find(template.ClientId);
            var application = template.ApplicationId.Equals(-1) ? null : applicationRepository.Find(template.ApplicationId);
            var component = template.ApplicationComponentId.Equals(-1) ? null : componentRepository.Find(template.ApplicationComponentId);
            var inputFileNames = rawFileRepository.GetRawFileListByRunNumberId(runDetail.RunDetailId).Select(x => x.FileName).ToArray();
            var inputFile = string.Join(", \n", inputFileNames);
            tokens.Add("{{DATE_NOW}}", DateTime.Now.ToString("dd/MM/yyyy"));
            if (client == null)
                tokens.Add("{{CLIENT_NAME}}", "N/A");
            else
                tokens.Add("{{CLIENT_NAME}}", client.Name);
            if (application == null)
                tokens.Add("{{APP_NAME}}", "N/A");
            else
                tokens.Add("{{APP_NAME}}", application.Name);
            if (component == null)
            {
                //   tokens.Add("{{COMPONENT_NAME}}", "N/A");
            }
            else
                tokens.Add("{{COMPONENT_NAME}}", component.Name);
            if (runDetail != null)
                tokens.Add("{{RUN_NUMBER}}", runDetail.RunNumber);
            else
                tokens.Add("{{RUN_NUMBER}}", "N/A");

            if (string.IsNullOrEmpty(inputFile))
                tokens.Add("{{FILE_NAME}}", "N/A");
            else
                tokens.Add("{{FILE_NAME}}", inputFile);

            //Replace all tokens with data
            foreach (var token in tokens)
            {
                tracking.Subjects = tracking.Subjects.Replace(token.Key, token.Value);
                tracking.Body = tracking.Body.Replace(token.Key, token.Value);
            }

            //Get email ids from ids list
            List<string> toContacts = new List<string>();
            List<string> ccContacts = new List<string>();

            if (!string.IsNullOrEmpty(template.EmailToIds))
                toContacts = GetContactsByCommaSaperatedEmailIds(template.EmailToIds).Select(x => x.EmailAddress).ToList();

            if (!string.IsNullOrEmpty(template.EmailCcIds))
                ccContacts = GetContactsByCommaSaperatedEmailIds(template.EmailCcIds).Select(x => x.EmailAddress).ToList();

          
            tracking.EmailStatus = (int)status;
            return tracking;
        }

        /// <summary>
        /// Get ready emails 
        /// </summary>
        /// <returns></returns>
        public IQueryable<EmailTracking> GetReadyEmails()
        {
            var list = emailTrackingRepository.GetReadyEmails((int)EmailStatusType.Ready);
            return list;
        }

        public EmailTemplate GetOverridedEmailTemplate(int? clientId = null, int? applicationId = null, int? componentId = null, string token = null, int? levelId = null)
        {
            var query = emailTemplateRepository.FindAllActive();

            if (clientId.HasValue)
                query = query.Where(ET => ET.ClientId == clientId.Value);

            if (applicationId.HasValue)
                query = query.Where(ET => ET.ApplicationId == applicationId.Value);

            if (componentId.HasValue)
                query = query.Where(ET => ET.ApplicationComponentId == componentId.Value);


            if (levelId.HasValue)
                query = query.Where(ET => ET.EmailLevelId == levelId.Value);

            query = query.Where(ET => ET.EmailToken == token);

            var template = query.FirstOrDefault();
            if (template == null)
            {
                //1. component id is greater then -1, other can be not be equal to -1
                if (componentId.Value > -1 && !(applicationId.Value == -1) && !(clientId.Value == -1))
                    return GetOverridedEmailTemplate(clientId, applicationId, -1, token, levelId);

                //2.  component can be -1, check for application id is greater then -1, other can be not be equal to -1
                if (componentId.Value == -1 && (applicationId.Value > -1) && !(clientId.Value == -1))
                    return GetOverridedEmailTemplate(clientId, -1, -1, token, levelId);

                //3.  component can be -1, application can be -1, check for client id is greater then -1
                if ((componentId.Value == -1) && (applicationId.Value == -1) && (clientId.Value > -1))
                    return GetOverridedEmailTemplate(-1, -1, -1, token, levelId);
                else
                    return new EmailTemplate();
            }
            else
                return template;
        }

        public List<EmailTemplate> GetOverridedEmailTemplates(int? clientId = null, int? applicationId = null, int? componentId = null, string token = null)
        {
            var query = emailTemplateRepository.FindAllActive();

            if (clientId.HasValue)
                query = query.Where(ET => ET.ClientId == clientId.Value);

            if (applicationId.HasValue)
                query = query.Where(ET => ET.ApplicationId == applicationId.Value);

            if (componentId.HasValue)
                query = query.Where(ET => ET.ApplicationComponentId == componentId.Value);

            if (!string.IsNullOrEmpty(token))
                query = query.Where(ET => ET.EmailToken == token);
            else
                query = query.Where(ET => string.IsNullOrEmpty(ET.EmailToken));

            var templates = query;
            if (templates.Count() == 0)
            {
                //1. component id is greater then -1, other can be not be equal to -1
                if (componentId.Value > -1 && !(applicationId.Value == -1) && !(clientId.Value == -1))
                    return GetOverridedEmailTemplates(clientId, applicationId, -1, token);

                //2.  component can be -1, check for application id is greater then -1, other can be not be equal to -1
                if (componentId.Value == -1 && (applicationId.Value > -1) && !(clientId.Value == -1))
                    return GetOverridedEmailTemplates(clientId, -1, -1, token);

                //3.  component can be -1, application can be -1, check for client id is greater then -1
                if ((componentId.Value == -1) && (applicationId.Value == -1) && (clientId.Value > -1))
                {
                    var tempTemplates = GetOverridedEmailTemplates(-1, -1, -1, token);
                    if (tempTemplates.Count > 0)
                        return tempTemplates;
                    else
                        return new List<EmailTemplate>();
                }
            }
            else
                return templates.ToList();

            return new List<EmailTemplate>();
        }
        public List<EmailTemplate> GetOverridedEmailTemplatesForWeb(int? clientId = null, int? applicationId = null, int? componentId = null, string token = null)
        {
            var query = emailTemplateRepository.FindAll();

            if (clientId.HasValue)
                query = query.Where(ET => ET.ClientId == clientId.Value);

            if (applicationId.HasValue)
                query = query.Where(ET => ET.ApplicationId == applicationId.Value);

            if (componentId.HasValue)
                query = query.Where(ET => ET.ApplicationComponentId == componentId.Value);

            if (!string.IsNullOrEmpty(token))
                query = query.Where(ET => ET.EmailToken == token);
            else
                query = query.Where(ET => string.IsNullOrEmpty(ET.EmailToken));

            var templates = query;
            if (templates.Count() == 0)
            {
                //1. component id is greater then -1, other can be not be equal to -1
                if (componentId.Value > -1 && !(applicationId.Value == -1) && !(clientId.Value == -1))
                    return GetOverridedEmailTemplatesForWeb(clientId, applicationId, -1, token);

                //2.  component can be -1, check for application id is greater then -1, other can be not be equal to -1
                if (componentId.Value == -1 && (applicationId.Value > -1) && !(clientId.Value == -1))
                    return GetOverridedEmailTemplatesForWeb(clientId, -1, -1, token);

                //3.  component can be -1, application can be -1, check for client id is greater then -1
                if ((componentId.Value == -1) && (applicationId.Value == -1) && (clientId.Value > -1))
                {
                    var tempTemplates = GetOverridedEmailTemplatesForWeb(-1, -1, -1, token);
                    if (tempTemplates.Count > 0)
                        return tempTemplates;
                    else
                        return new List<EmailTemplate>();
                }
            }
            else
                return templates.ToList();

            return new List<EmailTemplate>();
        }

        /// <summary>
        /// Check for template if it is already exists or not, search by keywords
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="applicationId"></param>
        /// <param name="componentId"></param>
        /// <param name="token"></param>
        /// <param name="levelId"></param>
        /// <returns></returns>        
        public bool CheckTempdateAlredyExist(int? clientId = null, int? applicationId = null, int? componentId = null, string token = null, int? levelId = null)
        {
            var query = emailTemplateRepository.FindAllActive();

            if (clientId.HasValue)
                query = query.Where(ET => ET.ClientId == clientId.Value);

            if (applicationId.HasValue)
                query = query.Where(ET => ET.ApplicationId == applicationId.Value);

            if (componentId.HasValue)
                query = query.Where(ET => ET.ApplicationComponentId == componentId.Value);

            if (levelId.HasValue)
                if (levelId.Value > 0)
                    query = query.Where(ET => ET.EmailLevelId == levelId.Value);

            query = query.Where(ET => ET.EmailToken == token);

            var template = query.FirstOrDefault();
            if (template == null)
                return false;
            return true;
        }

        /// <summary>
        /// Check for template if it is already exists or not, search by keywords
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="applicationId"></param>
        /// <param name="componentId"></param>
        /// <param name="token"></param>
        /// <param name="templateID"></param>
        /// <returns></returns>

        public bool CheckTempdateAlredyExistByTemplateID(int? clientId = null, int? applicationId = null, int? componentId = null, string token = null, int? templateID = null)
        {
                var query = emailTemplateRepository.FindAll();

                if (clientId.HasValue)
                    query = query.Where(ET => ET.ClientId == clientId.Value);

                if (applicationId.HasValue)
                    query = query.Where(ET => ET.ApplicationId == applicationId.Value);

                if (componentId.HasValue)
                    query = query.Where(ET => ET.ApplicationComponentId == componentId.Value);


                query = query.Where(ET => ET.EmailTemplateId == templateID.Value);

                query = query.Where(ET => ET.EmailToken == token);

                var template = query.FirstOrDefault();
                if (template == null)
                    return false;
            return true;
        }

        /// <summary>
        /// Get next level of email
        /// <para>1. Check email template table for current template according to keywords, if no template found then return with 0</para>
        /// <para>2. Check for email tracking table for current run number & email template entry. If no tracking found then return with 1, otherwise add 1 into found level</para>         
        /// </summary>
        /// <param name="runNumberId"></param>
        /// <param name="clientId"></param>
        /// <param name="applicationId"></param>
        /// <param name="componentId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public int GetNextLevel(int? runNumberId, int? clientId, int? applicationId, int? componentId, string token)
        {
                var lastEmailLevel = 0;
                DateTime lastEmailSentAt = DateTime.Now;
                var lastTemplateInterval = 0;
                //get all email templates associated to provided keywords
                var emailTemplates = GetOverridedEmailTemplates(clientId, applicationId, componentId, token);
                //no email template found, return 0
                if (emailTemplates.Count == 0)
                    return 0;

                //make email templates order by desc
                emailTemplates = emailTemplates.OrderBy(x => x.EmailLevelId).ToList();


                //Check is entry exists for current run number and component in email tracking
                var emailTrackings = emailTrackingRepository.GetEmailTrackingByKeyword(runNumberId.Value);
                //if email tracking is greater then 0, then check for how many levels 
                if (emailTrackings.Count() > 0)
                {
                    //traverse email templates, like 1,2,3..... so on
                    foreach (var template in emailTemplates)
                    {
                        //calculate max level        
                        //get first from tracking email for template and run number
                        var savedTrackingEmail = emailTrackings.OrderByDescending(x => x.SentDate).FirstOrDefault(T => T.EmailTemplateId == template.EmailTemplateId);
                        //no tracking email found, return level 1
                        if (savedTrackingEmail == null)
                        {
                            if (lastEmailLevel == 0)
                                return 1;
                            else
                            {
                                //return last level                           
                                return lastEmailLevel + 1;
                            }
                        }
                        else
                        {
                            //saved tracking email
                            //if last email level is same as current, then return same                        
                            if (lastEmailLevel == template.EmailLevelId)
                                if (lastEmailSentAt.AddMinutes(template.TimeInterval) < DateTime.Now)
                                    return lastEmailLevel;
                                else
                                    return 0;
                            //assign current level as last, for next iteration
                            lastEmailLevel = template.EmailLevelId;
                            lastEmailSentAt = savedTrackingEmail.SentDate;
                            lastTemplateInterval = template.TimeInterval;
                        }
                    }
                }
                else
                    return 1;

                if (lastEmailSentAt.AddMinutes(lastTemplateInterval) < DateTime.Now)
                    return lastEmailLevel;
                else
                    return 0;
        }

        public EmailTracking GetSentEmail(int? clientId = -1, int? applicationId = -1, string token = null, int? runNumberId = -1)
        {
                var level = GetNextLevel(runNumberId, clientId, applicationId, -1, token) - 1;

                //get any level of email template
                var emailTemplates = GetOverridedEmailTemplate(clientId, applicationId, -1, token, level);
                if (emailTemplates.EmailTemplateId == 0)
                    return null;
                //Check is entry exists for current run number and component in email tracking
                var emailTrackings = emailTrackingRepository.GetEmailTrackingByKeyword(runNumberId.Value);
                //if email tracking is greater then 0, then check for how many levels 
                if (emailTrackings.Count() > 0)
                    return emailTrackings.Where(T => T.EmailTemplateId == emailTemplates.EmailTemplateId).OrderByDescending(x => x.SentDate).FirstOrDefault();
           
            return null;
        }

        public EmailTracking GetLastSentEmail(int? clientId = -1, int? applicationId = -1, int? componentId = -1, string token = null, string runNumber = null, int? runNumberId = -1)
        {
                //Get run number id by runnumber 
                if (!string.IsNullOrEmpty(runNumber))
                {
                    var runNumberDetails = runDetailsRepository.GetRunDetailByRunNumber(runNumber);
                    runNumberId = runNumberDetails.RunDetailId;
                }

                //get any level of email template
                var emailTemplates = GetOverridedEmailTemplate(clientId, applicationId, componentId, token, -1);
                if (emailTemplates.EmailTemplateId == 0)
                    return null;
                //Check is entry exists for current run number and component in email tracking
                var emailTrackings = emailTrackingRepository.GetEmailTrackingByKeyword(runNumberId.Value);
                //if email tracking is greater then 0, then check for how many levels 
                if (emailTrackings.Count() > 0)
                    return emailTrackings.Where(T => T.EmailTemplateId == emailTemplates.EmailTemplateId).OrderByDescending(x => x.SentDate).FirstOrDefault();
           
            return null;
        }

        public EmailTracking GetLastSentEmailByTemplateId(int id, string runNumber = null, int? runNumberId = -1)
        {
                //Get run number id by runnumber 
                if (!string.IsNullOrEmpty(runNumber))
                {
                    var runNumberDetails = runDetailsRepository.GetRunDetailByRunNumber(runNumber);
                    runNumberId = runNumberDetails.RunDetailId;
                }

                //get any level of email template
                var emailTemplates = GetTemplate(id);
                if (emailTemplates.EmailTemplateId == 0)
                    return null;
                //Check is entry exists for current run number and component in email tracking
                var emailTrackings = emailTrackingRepository.GetEmailTrackingByKeyword(runNumberId.Value);
                //if email tracking is greater then 0, then check for how many levels 
                if (emailTrackings.Count() > 0)
                    return emailTrackings.Where(T => T.EmailTemplateId == emailTemplates.EmailTemplateId).OrderByDescending(x => x.SentDate).FirstOrDefault();
           
            return null;
        }

        public EmailTemplate GetTemplate(int id)
        {
            //Template id should be greater then 0
            if (id == 0)
                return null;
             //
                var emailTemplates = emailTemplateRepository.Find(id);
                //if template is inactive, return null
                //try
                //{
                //    //LogManager.log.Debug(emailTemplates.Status.ToString());
                //    bool templateStatus = Convert.ToBoolean(emailTemplates.Status);
                //    //LogManager.log.Debug(templateStatus.ToString());

                //    //if (templateStatus)
                //    //    return emailTemplates;
                //    //else
                //    //    return null;

                //}
                //catch (Exception e) { throw e; }
                return emailTemplates;
        }
        
        public EmailTemplate GetTemplateWithDistributionList1(int? clientId = null, int? applicationId = null, int? appComponentId = null, string token = null, int? levelId = null)
        {
                var query = emailTemplateRepository.FindAllActive();

                if (clientId.HasValue)
                    query = query.Where(ET => ET.ClientId == clientId.Value);

                if (applicationId.HasValue)
                    query = query.Where(ET => ET.ApplicationId == applicationId.Value);

                if (appComponentId.HasValue)
                    query = query.Where(ET => ET.ApplicationComponentId == appComponentId.Value);

                if (levelId.HasValue)
                    if (levelId.Value > 0)
                        query = query.Where(ET => ET.EmailLevelId == levelId.Value);

                query = query.Where(ET => ET.EmailToken == token);

                var template = (query.FirstOrDefault());
                if (template == null)
                {
                    //1. component id is greater then -1, other can be not be equal to -1

                    if (appComponentId.Value > -1 && !(applicationId.Value == -1) && !(clientId.Value == -1))
                        return GetTemplateWithDistributionList1(clientId, applicationId, -1, token, levelId);

                    //2.  component can be -1, check for application id is greater then -1, other can be not be equal to -1

                    if (appComponentId.Value == -1 && (applicationId.Value > -1) && !(clientId.Value == -1))
                        return GetTemplateWithDistributionList1(clientId, -1, -1, token, levelId);

                    //3.  component can be -1, application can be -1, check for client id is greater then -1
                    if ((appComponentId.Value == -1) && (applicationId.Value == -1) && (clientId.Value > -1))
                        return GetTemplateWithDistributionList1(-1, -1, -1, token, levelId);
                    else
                        return new EmailTemplate();
                }
                else
                    return template;
        }

        public EmailTemplate GetCurrentTemplate_New(int? clientId = null, int? applicationId = null, int? appComponentId = null, string token = null)
        {
                var emailTemplates = GetTemplateWithDistributionList1(clientId, appComponentId, appComponentId, token, null);
                return emailTemplates;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="emailIds">Comma Seperated email ids</param>
        /// <returns></returns>
        public List<ContactInfo> GetCurrentLevelContactsByCommaSepEmailIds(string emailIds)
        {
                var del = new char[] { ',' };
                var dlList = emailIds.Split(del, StringSplitOptions.RemoveEmptyEntries);
                return GetCurrentLevelContactsByEmailIdList(dlList);
        }

        public List<ContactInfo> GetCurrentLevelContactsByEmailIdList(string[] emailIds)
        {
                var emails = contactInfoRepository.GetContactInfoListbyId(emailIds);
                return emails.ToList();
        }

        public EmailTemplate GetEmailTemplateByToken(string token)
        {
                var emailTemplates = emailTemplateRepository.FindAllActive().Where(x => x.EmailToken == token).FirstOrDefault();
                return emailTemplates;
        }

        public void CheckLogging()
        {
        }

        #endregion

        public bool DeleteTemplate(int id)
        {
                var result = emailTemplateRepository.DeleteAndResetLevel(id);
                return true;
        }
    }
}
