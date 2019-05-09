using MD.ApkMAP.AuthHelper.OverWrite;
using MD.ApkMAP.IServices;
using MD.ApkMAP.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MD.ApkMAP.Controllers
{
    [Produces("application/json")]
    [Route("api/Blog")]
    //[BearerAuthorize]
    [Authorize(Policy = "Admin")]
    //[Authorize(Policy = "Client")]
    //[Authorize("Bearer")]
    //[Authorize]
    public class BlogController: Controller
    {
        IAdvertisementServices _advertisementServices;
        public BlogController(IAdvertisementServices advertisementServices)
        {
            _advertisementServices = advertisementServices;
        }
        // GET: api/Blog
        /// <summary>
        /// Sum接口
        /// </summary>
        /// <param name="i">参数i</param>
        /// <param name="j">参数j</param>
        /// <returns></returns>
        [HttpGet]
        public int Get(int i, int j)
        {
            return _advertisementServices.Sum(i, j);
        }

    }
}
