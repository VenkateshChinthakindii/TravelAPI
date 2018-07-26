using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TravelAPI.Models;

namespace TravelAPI.Controllers
{
    public class PackageController : BaseAPIController
    {
        BTEntities BTEntities = new BTEntities();
        // GET: api/Package
        [HttpGet]
        [Route("api/package/GetAllActivePackages")]
        public HttpResponseMessage Get()
        {
            //BTEntities.Configuration.LazyLoadingEnabled = false;
            var packages = BTEntities.Packages.Join(BTEntities.Masters, pg => pg.SubCatID, ms => ms.id, (pg, ms) => new
            {
                id = pg.id,
                code = pg.Code,
                MstTypeID = ms.MstTypeID,
                PackageSubCategoryID = pg.SubCatID,
                PackageSubCategoryName = ms.Name,
                Fare = pg.Fare,
                PackageActive = pg.IsActive,
                MasterActive = ms.IsActive
            }).Where(pgms => pgms.PackageActive == true && pgms.MasterActive == true)
             .Join(BTEntities.MasterTypes, ms => ms.MstTypeID, mt => mt.id,
             (ms, mt) => new
             {
                 id = ms.id,
                 code = ms.code,
                 PackageCategoryID = mt.id,
                 PackageCategoryName = mt.MstType,
                 PackageSubCategoryID = ms.PackageSubCategoryID,
                 PackageSubCategoryName = ms.PackageSubCategoryName,
                 Fare = ms.Fare,
                 PackagePlaces = BTEntities.PackagePlaces.Where(pid=>pid.PackageID==ms.id).
                 Join(BTEntities.Masters,pl=>pl.PlaceID,msn=>msn.id,(plid,msname)=> new{Id=msname.id,Name=msname.Name })
             }
             )
             .GroupBy(x => x.PackageCategoryID, (z, y) => y.GroupBy(x => x.PackageSubCategoryID));       

            return ToJson(packages);
           //return new string[] { "value1", "value2" };
        }

        // GET: api/Package/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Package
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Package/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Package/5
        public void Delete(int id)
        {
        }
    }
}
