using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CondorExtreme3.DAL;
using CondorExtreme3.ModelsUser;

/*-----------------------------------+
 * Author: Ilhan Karic               |
 * Module: Users                     |
 * Last Modified: 1/10/2018 @ 19:36  |
 * Copyright: Condor Coorporation    |
 *-----------------------------------*/
namespace CondorExtreme3.Areas.Local.Controllers
{
    public class VirtualPointsController : Controller
    {
        // Global user context
        public CondorDBUsers contextGlobal = new CondorDBUsers();

        /// <summary>
        ///     Gets all available virtual point packs.
        /// </summary>
        /// <returns>Partial view of all available packs.</returns>
        [HttpPost]
        public PartialViewResult GetVirtualPointPacks()
        {
            List<VirtualPointsPack> model = contextGlobal.VirtualPointsPacks.ToList();
            return PartialView("GetVirtualPointPacks", model);
        }

        // NOTE: User shouldn't be able to do this.
        // Migrate to director or employee controlers later!!
        /// <summary>
        ///     Set the virtual point pack.
        /// </summary>
        /// <returns>PartialView result input</returns>
        [HttpPost]
        public PartialViewResult SetVirtualPointPacks()
        {
            return PartialView("SetVirtualPointPacks");
        }

        /// <summary>
        ///     Submit a new virtual point pack.
        /// </summary>
        /// <param name="amount">The amount of VP the pack gives</param>
        /// <returns>ActionResult view to registered user index</returns>
        [HttpPost]
        public ActionResult SubmitVirtualPointPacks(int amount)
        {
            if (contextGlobal == null)
                return Redirect(Url.Content("~/"));

            contextGlobal.VirtualPointsPacks.Add(new VirtualPointsPack { Amount = amount });
            contextGlobal.SaveChanges();

            return RedirectToAction("Index", "RegisteredVisitor");
        }

        /// <summary>
        ///     Buying virtual points
        /// </summary>
        /// <param name="id">The VP pack ID</param>
        /// <returns>ActionResult view to registered user index</returns>
        [HttpPost]
        public ActionResult BuyVirtualPointPacks(int id)
        {
            if (contextGlobal == null)
                return Redirect(Url.Content("~/"));

            int amountPurchased = contextGlobal.VirtualPointsPacks.Find(id).Amount;
            RegisteredVisitor rv = contextGlobal.RegisteredVisitors.Find(Session["UserID"]);

            rv.VirtualPointsTotal += amountPurchased;
            contextGlobal.SaveChanges();

            return RedirectToAction("Index", "RegisteredVisitor");
        }
    }
}