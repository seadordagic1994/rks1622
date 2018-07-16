using CondorExtreme3_API.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace CondorExtreme3_API.Controllers
{
    [RoutePrefix("api/VirtualPoints")]
    public class VirtualPointsController : ApiController
    {
        private CondorDBXEntities principal = new CondorDBXEntities();

        [HttpGet]
        [Route("GetVirtualPointsPacks")]
        public IHttpActionResult GetVirtualPointsPacks()
        {
            var packs = principal.VirtualPointsPackets
                .Where(x => !(bool)x.IsDeleted)
                .OrderBy(x => x.Amount)
                .ToList();

            if (!packs.Any())
                return BadRequest("There are no packs available currently!");
            return Ok(packs.Select(x => new
            {
                x.VirtualPointsPacketID,
                x.Amount
            }).ToList());
        }

        [HttpPost]
        [Route("PostBuyVirtualPointPack")]
        public IHttpActionResult PostBuyVirtualPointPack([FromBody]dynamic value)
        {
            try
            {
                JObject jsonBuyOrder = JObject.FromObject(value);
                var salesOrder = new TransactionsVirtualPoints()
                {
                    RVisitorID = int.Parse(jsonBuyOrder["RVisitorID"].ToString()),
                    VirtualPointsPacketID = int.Parse(jsonBuyOrder["VirtualPointsPacketID"].ToString()),
                    TransactionDate = DateTime.Now,
                };

                var user = principal.RVisitors.Find(salesOrder.RVisitorID);
                user. VirtualPointsTotal+= (int)principal.VirtualPointsPackets
                    .Find(salesOrder.VirtualPointsPacketID).Amount;

                principal.TransactionsVirtualPoints.Add(salesOrder);
                principal.SaveChanges();
            }
            catch (Exception ex) { return BadRequest(ex.ToString() + "##" + value); }
            return Ok();
        }
    }
}