using Newtonsoft.Json;
using Server.Common;
using Server.Models;
using System;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Server.Controllers
{
    public class ServiceController : ApiController
    {
        /// <summary>
        /// 根据用户名获取token
        /// </summary>
        /// <param name="staffId"></param>
        /// <returns></returns>
        public HttpResponseMessage GetToken(string staffId)
        {
            ResultMsg resultMsg = null;
            try
            {
                string id = "";

                //判断参数是否合法
                if (string.IsNullOrEmpty(staffId))
                {
                    resultMsg = new ResultMsg();
                    resultMsg.StatusCode = (int)StatusCodeEnum.ParameterError;
                    resultMsg.Info = StatusCodeEnum.ParameterError.GetEnumText();
                    resultMsg.Data = "";
                    Console.WriteLine(resultMsg.ToString());
                    return HttpResponseExtension.toJson(JsonConvert.SerializeObject(resultMsg));
                }

                //插入缓存
                Token token = (Token)HttpRuntime.Cache.Get(staffId);
                //Console.WriteLine(token.ToString());
                if (HttpRuntime.Cache.Get(staffId) == null)
                {
                    token = new Token();
                    token.StaffId = staffId;
                    token.SignToken = Guid.NewGuid();
                    token.ExpireTime = DateTime.Now.AddDays(1);
                    //Console.WriteLine(resultMsg.ToString());
                    HttpRuntime.Cache.Insert(token.StaffId, token, null, token.ExpireTime, TimeSpan.Zero);
                }

                //返回token信息
                resultMsg = new ResultMsg();
                resultMsg.StatusCode = (int)StatusCodeEnum.Success;
                resultMsg.Info = "";
                resultMsg.Data = token;
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message + "\t" + ex.StackTrace);
            }
            Console.WriteLine(resultMsg.ToString());
            return HttpResponseExtension.toJson(JsonConvert.SerializeObject(resultMsg));
        }
    }
}
