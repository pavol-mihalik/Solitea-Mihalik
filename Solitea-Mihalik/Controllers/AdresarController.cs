using AutoMapper;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Solitea_Mihalik.BL;
using Solitea_Mihalik.Models;
using System.Collections.Generic;
using System.Web.Mvc;
using bl = Solitea_Mihalik.BL;

namespace Solitea_Mihalik.Controllers
{
    public class AdresarController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NactiOsoby([DataSourceRequest]DataSourceRequest request)
        {
            AdresarFacade facade = new AdresarFacade();
            List<Osoba> adresar = facade.NactiAdresar();

            MapperConfiguration config = VyvtorMapovani();
            var result = config.CreateMapper().Map<List<OsobaViewModel>>(adresar);

            return Json(result.ToDataSourceResult(request));
        }

        public ActionResult HierarchyBinding_Adresy(int osobaID, [DataSourceRequest] DataSourceRequest request)
        {
            AdresarFacade facade = new AdresarFacade();
            Osoba osoba = facade.NactiOsobu(osobaID);

            MapperConfiguration config = VyvtorMapovani();
            var result = config.CreateMapper().Map<OsobaViewModel>(osoba);

            return Json(result.Adresy.ToDataSourceResult(request));
        }

        public ActionResult VytvorNovouOsobu()
        {
            ViewBag.Title = "Nová osoba";
            return View("Detail", new OsobaViewModel() { Adresy = new List<AdresaViewModel>() { new AdresaViewModel() } });
        }

        public ActionResult VytvorNovouAdresu(int osobaId)
        {
            ViewBag.Title = "Nová adresa";
            return View("NovaAdresa", new AdresaViewModel() { OsobaID = osobaId });
        }

        [HttpPost]
        public ActionResult UlozAdresu(Adresa adresa)
        {
            AdresarFacade facade = new AdresarFacade();
            facade.UlozAdresu(adresa);

            return View("Index", facade.NactiAdresar());
        }

        public ActionResult DetailOsoby(int osobaID)
        {
            ViewBag.Title = "Detail osoby";
            AdresarFacade facade = new AdresarFacade();
            Osoba osoba = facade.NactiOsobu(osobaID);

            MapperConfiguration config = VyvtorMapovani();
            var result = config.CreateMapper().Map<OsobaViewModel>(osoba);

            return View("Detail", result);
        }

        [HttpPost]
        public ActionResult UlozDetail(Osoba osoba)
        {
            AdresarFacade facade = new AdresarFacade();
            facade.UlozOsobu(osoba);

            return View("Index", facade.NactiAdresar());
        }

        public ActionResult SmazOsobu(int osobaId)
        {
            AdresarFacade facade = new AdresarFacade();
            facade.SmazOsobu(osobaId);

            return View("Index", facade.NactiAdresar());
        }

        public ActionResult SmazAdresu(int adresaId)
        {
            AdresarFacade facade = new AdresarFacade();
            facade.SmazAdresu(adresaId);

            return View("Index", facade.NactiAdresar());
        }

        private MapperConfiguration VyvtorMapovani()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<bl.Osoba, OsobaViewModel>();
                cfg.CreateMap<OsobaViewModel, bl.Osoba>();
                cfg.CreateMap<bl.Adresa, AdresaViewModel>();
                cfg.CreateMap<AdresaViewModel, bl.Adresa>();
            });
        }
    }
}