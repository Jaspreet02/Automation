using System;
using System.Collections.Generic;
using System.Linq;

namespace DbHander
{
    public class EmailTemplateRepository : AbstractRepository, IEmailTemplateRepository
    {
        public IQueryable<EmailTemplate> FindAll()
        {
                var emailTemplates = objDataContext.EmailTemplates;
                return emailTemplates;
        }

        public IQueryable<EmailTemplate> FindAllActive()
        {
                var emailTemplates = objDataContext.EmailTemplates.Where(x => x.Status == true);
                return emailTemplates;
        }

        public EmailTemplate Find(int id)
        {
                var result = objDataContext.EmailTemplates.SingleOrDefault(x => x.EmailTemplateId.Equals(id));
                return result;
          }

        public int Save(EmailTemplate dao)
        {
                EmailTemplate dbEntity = objDataContext.EmailTemplates.SingleOrDefault(x => x.EmailTemplateId.Equals(dao.EmailTemplateId));
                if (dbEntity != null)
                {
                    dao.ModifiedAt = DateTimeOffset.Now;
                    objDataContext.Entry(dbEntity).CurrentValues.SetValues(dao);
                }
                else
                {
                    dao.CreatedAt = DateTimeOffset.Now;
                    dao.ModifiedAt = dao.CreatedAt;
                    objDataContext.EmailTemplates.Add(dao);
                }
                objDataContext.SaveChanges();
           
            return dao.EmailTemplateId;
        }

        public bool Delete(int id)
        {
                EmailTemplate dbEntity = dbEntity = objDataContext.EmailTemplates.Where(x => x.EmailTemplateId == id).FirstOrDefault();
                if (dbEntity != null)
                {
                    objDataContext.EmailTemplates.Remove(dbEntity);
                    objDataContext.SaveChanges();
                }
                return true;
        }

        public bool DeleteAndResetLevel(int templateId)
        {
            #region resetLaevel of other templates for that app,client,token

            EmailTemplate templ = Find(templateId);
            var result = Delete(templateId);
            if (templ != null)
            {
                var query = objDataContext.EmailTemplates.Where(ET => ET.ClientId == templ.ClientId && ET.ApplicationId == templ.ApplicationId && ET.ApplicationComponentId == templ.ApplicationComponentId && ET.EmailToken == templ.EmailToken);
                var all = query.Where(x => x.EmailLevelId > templ.EmailLevelId && x.EmailTemplateId != templ.EmailTemplateId).OrderBy(x => x.EmailLevelId).ToList();
                int levelid = templ.EmailLevelId;
                foreach (var item in all)
                {
                    item.EmailLevelId = levelid;
                    objDataContext.SaveChanges();
                    levelid++;
                }
            }
            return result;
            #endregion

        }
        
        #region IEmailTemplateRepository Members

        public IEnumerable<EmailTemplate> GetEmailTemplatesByKeyword(int? clientId = null, int? applicationId = null, int? appComponentId = null, string token = null, int? levelId = null)
        {
                var query = FindAll();

                if (clientId.HasValue)
                    if (clientId.Value > 0)
                        query = query.Where(ET => ET.ClientId == clientId.Value);

                if (applicationId.HasValue)
                    if (applicationId.Value > 0)
                        query = query.Where(ET => ET.ApplicationId == applicationId.Value);

                if (appComponentId.HasValue)
                    if (appComponentId.Value > 0)
                        query = query.Where(ET => ET.ApplicationComponentId == appComponentId.Value);


                if (levelId.HasValue)
                    if (levelId.Value > 0)
                        query = query.Where(ET => ET.EmailLevelId == levelId.Value);


                if (!string.IsNullOrEmpty(token))
                    query = query.Where(ET => ET.EmailToken == token);

                return query.ToArray();
        }

        
        public bool UpdateStatus(int id)
        {
            throw new NotImplementedException();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    objDataContext.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~EmailTemplateRepository() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion

        #endregion
    }

}
