using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using MerchantsGuideApplication.Controllers;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;
using RouteAttribute = System.Web.Http.RouteAttribute;



namespace MerchantsGuideApplication.Controllers
{
    public class GalaxyController : ApiController
    {
       
        public class MerchantMessage {
          public  List<string> abc { get; set; }
        }
        [HttpPost]
        [Route("api/Galaxy")]
        public List<string> GetData(JObject json)
        {
            List<string> InputmsgList = new List<string>();
            foreach (var MerchantMessage in json)
            {               
               InputmsgList.Add((MerchantMessage.Value).ToString());
            }
            var Cal = new Calculation();
            List<string> OutputMessageList = new List<string>();
            for (int i = 0; i < InputmsgList.Count;i++)
            {
                OutputMessageList.Add(Cal.decodeMessages(InputmsgList[i]));
            }
            //return new JsonResult(new { OutputMessageList });

          //  return new Json(OutputMessageList, );
            return OutputMessageList;

        }

    }
}