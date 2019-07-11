using DbHander;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace MobileService.Controllers
{
    public class MasterController : BaseController
    {
        [HttpGet]
        public IHttpActionResult RunNumberStatus()
        {
            return Ok(Enum.GetNames(typeof(RunNumberStatusType)));
        }

        [HttpGet]
        public IHttpActionResult QueueType()
        {
            List<QueueType> result = new List<QueueType>();
            foreach (var item in Enum.GetValues(typeof(QueueTypes)))
            {
                result.Add(new QueueType() { QueueTypeId = Convert.ToByte(item), Status = item.ToString() });
            }
            return Ok(result);
        }

        [HttpGet]
        public IHttpActionResult EmailToken()
        {
            List<EmailToken> result = new List<EmailToken>();
            foreach (var item in Enum.GetValues(typeof(EmailKeyword)))
            {
                result.Add(new EmailToken() { EmailTokenId = Convert.ToByte(item), Keyword = "{{" + item.ToString() + "}}",Note = EnumHelper.GetEnumDescription((EmailKeyword)item) });
            }
            return Ok(result);
        }

        [HttpGet]
        public IHttpActionResult FileKeyword()
        {
            var result = Enum.GetNames(typeof(FileKeyword));
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = "{{" + result[i] + "}}";
            }
            return Ok(result);
        }
    }
}
