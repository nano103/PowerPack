using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

namespace ACDC.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]

    public class CompanysController : ControllerBase
    {
        // GET api/companys
        [HttpGet]
        public List<Company> Get()
        {
            using (PowerContext db = new PowerContext())
            {
                var Companys = from p in db.Companys
                               select p;
                return Companys.ToList();
            }
        }
    }

    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]

    public class AffiliatesController : ControllerBase
    {
        // GET api/affiliates
        [HttpGet]
        public List<Affiliate> Get()
        {
            using (PowerContext db = new PowerContext())
            {
                var Affiliates = from p in db.Affiliates
                               select p;
                return Affiliates.ToList();
            }
        }
    }

    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]

    public class ConsumersController : ControllerBase
    {
        // GET api/consumers
        [HttpGet]
        public List<Consumer> Get()
        {
            using (PowerContext db = new PowerContext())
            {
                var Consumers = from p in db.Consumers
                                    select p;
                return Consumers.ToList();
            }
        }
    }


    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]

    public class MeasurePointsController : ControllerBase
    {
        // GET api/measurepoints
        [HttpGet]
        //public ActionResult<IEnumerable<MeasurePoint>> Get()
        public JsonResult Get()
        {
            using (PowerContext db = new PowerContext())
            {
                var MeasurePoints = from p in db.MeasurePoints
                                    .Include("ElectricMeter")
                                    .Include("VoltageTransformer")
                                    .Include("CurrentTransformer")
                                    .Include("ElectricMeter.MeterType")
                                    .Include("VoltageTransformer.VTType")
                                    .Include("CurrentTransformer.CTType")
                                    select p;

                return new JsonResult(MeasurePoints.ToList(),
                    new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore});
            }
   
        }

        // POST api/measurepoints
        [HttpPost]
        public IActionResult Post([FromBody] MeasurePoint MeasurePoint)
        {
            //MeasurePoint MeasurePoint = JsonConvert.DeserializeObject<MeasurePoint>(JsonString);
            using (PowerContext db = new PowerContext())
            {
                if (MeasurePoint.ElectricMeter.MeterType != null && MeasurePoint.VoltageTransformer.VTType != null && MeasurePoint.CurrentTransformer.CTType != null)
                {
                    var ExistingMeterName = db.MeterTypes.SingleOrDefault(p => p.Title == MeasurePoint.ElectricMeter.MeterType.Title);
                    if (ExistingMeterName != null)
                    {
                        MeasurePoint.ElectricMeter.MeterType = ExistingMeterName;
                    }
                    else
                    {
                        db.MeterTypes.Add(MeasurePoint.ElectricMeter.MeterType);
                    }

                    var ExistingVTName = db.VTTypes.SingleOrDefault(p => p.Title == MeasurePoint.VoltageTransformer.VTType.Title);
                    if (ExistingVTName != null)
                    {
                        MeasurePoint.VoltageTransformer.VTType = ExistingVTName;
                    }
                    else
                    {
                        db.VTTypes.Add(MeasurePoint.VoltageTransformer.VTType);
                    }

                    var ExistingCTName = db.CTTypes.SingleOrDefault(p => p.Title == MeasurePoint.CurrentTransformer.CTType.Title);
                    if (ExistingCTName != null)
                    {
                        MeasurePoint.CurrentTransformer.CTType = ExistingCTName;
                    }
                    else
                    {
                        db.CTTypes.Add(MeasurePoint.CurrentTransformer.CTType);
                    }

                    db.VoltageTransformers.Add(MeasurePoint.VoltageTransformer);
                    db.CurrentTransformers.Add(MeasurePoint.CurrentTransformer);
                    db.ElectricMeters.Add(MeasurePoint.ElectricMeter);

                    db.MeasurePoints.Add(MeasurePoint);
                    db.SaveChanges();
                }
                return Ok(MeasurePoint);
            }
        }
    }

    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]

    public class SupplyPointsController : ControllerBase
    {
        // GET api/supplypoints
        [HttpGet]
        public List<SupplyPoint> Get()
        {
            using (PowerContext db = new PowerContext())
            {
                var SupplyPoints = from p in db.SupplyPoints
                                   select p;
                return SupplyPoints.ToList();
            }

        }
    }

    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class CalcUnitsController : ControllerBase
    {
        // GET api/calcunits
        [HttpGet]
        public List<CalcUnit> Get()
        {
            using (PowerContext db = new PowerContext())
            {
                var timeFrom = new DateTime(2018, 1, 1);
                var timeTo = new DateTime(2019, 1, 1);

                var CalcUnits = from p in db.CalcUnits
                                where ((p.TimeFrom >= timeFrom) && (p.TimeFrom < timeTo)) || ((p.TimeTo >= timeFrom) && (p.TimeTo < timeTo))
                                select p;
                return CalcUnits.ToList();
            }
        }
    }

    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class OutdatedController : ControllerBase
    {
        // GET api/outdated
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "out", "dated" };
        }

        [HttpGet("meters")]
        public List<ElectricMeter> Meters()
        {
            using (PowerContext db = new PowerContext())
            {
                var ElectricMeters = from p in db.ElectricMeters
                                    where p.CheckDate <= DateTime.Now
                                    select p;
                return ElectricMeters.ToList();
            }
        }

        [HttpGet("vts")]
        public List<VoltageTransformer> VoltagreTransformers()
        {
            using (PowerContext db = new PowerContext())
            {
                var VoltageTransformers = from p in db.VoltageTransformers
                                          where p.CheckDate <= DateTime.Now
                                     select p;
                return VoltageTransformers.ToList();
            }
        }

        [HttpGet("cts")]
        public List<CurrentTransformer> CurrentTransformers()
        {
            using (PowerContext db = new PowerContext())
            {
                var CurrentTransformers = from p in db.CurrentTransformers
                                          where p.CheckDate <= DateTime.Now
                                          select p;
                return CurrentTransformers.ToList();
            }
        }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
