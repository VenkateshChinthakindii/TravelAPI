using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TravelAPI.Models;

namespace TravelAPI.Controllers
{
    enum masterTypes
    {
        Place = 1,
        RegularTour = 2,
        SpecialTour = 3
    }

    public class Place
    {
        public int Id;
        public string Name;
        public bool IsActive;
    }
    public class PackageMapPlace
    {
        public int PackageID;
        public Place[] Places;
    }
    public class MasterController : BaseAPIController
    {

        BTEntities BTEntities = new BTEntities();
        // GET: api/Package/5
        [HttpGet]
        [Route("api/master/getAllPackageSubCategories")]
        public HttpResponseMessage GetAllPackageSubCategories()
        {
            var packageSubCategories = BTEntities.Masters.
                Where(mt => mt.MstTypeID == (int)masterTypes.RegularTour ||
                mt.MstTypeID == (int)masterTypes.SpecialTour).Select(psub => new { Id = psub.id, Name = psub.Name });
            return ToJson(packageSubCategories);
        }

        [HttpGet]
        [Route("api/master/getAllPlaces")]
        public HttpResponseMessage GetAllPlaces()
        {
            var places = BTEntities.Masters.
                Where(mt => mt.MstTypeID == (int)masterTypes.Place).
                Select(psub => new { Id = psub.id, Name = psub.Name, IsActive = true });
            return ToJson(places);
        }
        [HttpGet]
        [Route("api/master/GetAllActivePlaces")]
        public HttpResponseMessage GetAllActivePlaces()
        {
            var places = BTEntities.Masters.
                Where(mt => mt.MstTypeID == (int)masterTypes.Place).
                Select(psub => new { Id = psub.id, Name = psub.Name, IsActive = psub.IsActive });
            return ToJson(places);
        }
        [HttpGet]
        [Route("api/master/getAllPackages")]
        public HttpResponseMessage GetAllPackages()
        {
            BTEntities.Configuration.LazyLoadingEnabled = false;
            var packages = BTEntities.Masters.Include("Packages").Where(x => x.Packages.Count > 0).
                Select(ma => new
                {
                    PackageSubCategory = ma.Name,
                    Packages = ma.Packages.
                Select(pa => new { Id = pa.id, Name = pa.Code })
                }).GroupBy(x => x.PackageSubCategory);
            return ToJson(packages);
        }


        [HttpGet]
        [Route("api/master/getPackagePlaces/{id}")]
        public HttpResponseMessage GetPackagePlaces(int id)
        {
            var places = BTEntities.PackagePlaces.Where(pk => pk.PackageID == id).Select(pl =>
                 new
                 {
                     Id = pl.PlaceID,
                     IsActive = pl.IsActive
                 });
            return ToJson(places);
        }



        [HttpPost]
        [Route("api/master/MapPackagePlace")]
        public HttpResponseMessage MapPackagePlace(PackageMapPlace obj)
        {
            using (DbContextTransaction transaction = BTEntities.Database.BeginTransaction())
            {
                try
                {
                    DateTime modifiedDate = DateTime.Now;
                    PackagePlace packageplace = null;
                    foreach (var item in obj.Places)
                    {

                        if (BTEntities.PackagePlaces.Any(pp => pp.PackageID == obj.PackageID && pp.PlaceID == item.Id))
                        {
                            packageplace = BTEntities.PackagePlaces.Where(pp => pp.PackageID == obj.PackageID && pp.PlaceID == item.Id).FirstOrDefault();
                            packageplace.IsActive = item.IsActive;
                            packageplace.LastModifiedDate = modifiedDate;
                        }
                        else
                        {
                            BTEntities.PackagePlaces.Add(new PackagePlace()
                            {
                                PackageID = obj.PackageID,
                                PlaceID = item.Id,
                                CreatedDate = modifiedDate,
                                LastModifiedDate = modifiedDate,
                                IsActive = item.IsActive
                            });
                        }
                    }
                    IEnumerable<int> placeIds = obj.Places.Select(pl => pl.Id);
                    IEnumerable<PackagePlace> packagePlaceIds = BTEntities.PackagePlaces.
                        Where(pp => (pp.PackageID == obj.PackageID) &&
                       (!placeIds.Contains(pp.PlaceID)));
                        BTEntities.PackagePlaces.RemoveRange(packagePlaceIds);
                    BTEntities.SaveChanges();
                    transaction.Commit();
                    return ToJson(1, HttpStatusCode.OK);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return ToJson(ex, HttpStatusCode.BadRequest);
                    //Console.WriteLine("Error occurred.");
                }
            }
        }

      

        [HttpPost]
        [Route("api/master/AddNewPlace")]
        public HttpResponseMessage AddNewPlace(Master[] masters)
        {
            using (DbContextTransaction transaction = BTEntities.Database.BeginTransaction())
            {
                try
                {
                    Master masterobj = null;
                    DateTime modifiedDate = DateTime.Now;
                    foreach (Master master in masters)
                    {
                        if (master.id > 0)
                        {
                            masterobj = BTEntities.Masters.Where(x => x.id == master.id).FirstOrDefault();
                            masterobj.IsActive = master.IsActive;
                            masterobj.Name = master.Name;
                            masterobj.LastModifiedDate = modifiedDate;
                        }
                        else
                        {
                            BTEntities.Masters.Add(new Master()
                            {
                                Name = master.Name,
                                IsActive = master.IsActive,
                                MstTypeID = master.MstTypeID,
                                CreatedDate = modifiedDate,
                                LastModifiedDate = modifiedDate
                            });
                        }
                    }
                    BTEntities.SaveChanges();
                    transaction.Commit();
                    return ToJson(1, HttpStatusCode.OK);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return ToJson(ex, HttpStatusCode.BadRequest);
                }
            }
        }


        [HttpPost]
        [Route("api/master/AddNewPackage")]
        public HttpResponseMessage AddNewPackage(Package[] packages)
        {
            using (DbContextTransaction transaction = BTEntities.Database.BeginTransaction())
            {
                try
                {
                    Package packageObj = null;
                    DateTime modifiedDate = DateTime.Now;
                    foreach (Package package in packages)
                    {
                        if (package.id > 0)
                        {
                            packageObj = BTEntities.Packages.Where(x => x.id == package.id).FirstOrDefault();
                            packageObj.IsActive = package.IsActive;
                            packageObj.Code = package.Code;
                            packageObj.Fare = package.Fare;
                            packageObj.LastModifiedDate = modifiedDate;
                        }
                        else
                        {
                            BTEntities.Packages.Add(new Package()
                            {
                                Code = package.Code,
                                IsActive = package.IsActive,
                                SubCatID = package.SubCatID,
                                Fare = package.Fare,
                                CreatedDate = modifiedDate,
                                LastModifiedDate = modifiedDate
                            });
                        }
                    }
                    BTEntities.SaveChanges();
                    transaction.Commit();
                    return ToJson(1, HttpStatusCode.OK);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return ToJson(ex, HttpStatusCode.BadRequest);
                }
            }
        }
    }
}
