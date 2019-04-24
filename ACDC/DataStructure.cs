using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace ACDC
{
    public abstract class BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
    }

    public abstract class NamedEntity : BaseEntity
    {
        public string Title { get; set; }
    }

    public abstract class AddressedEntity : NamedEntity
    {
        public string Address { get; set; }
    }

    public class Company : AddressedEntity
    {
        public ICollection<Affiliate> Affiliates { get; set; }
    }

    public class Affiliate : AddressedEntity
    {
        public int CompanyID { get; set; }
        public virtual Company Company { get; set; }
        public ICollection<Consumer> Consumers { get; set; }
    }

    public class Consumer : AddressedEntity
    {
        public int AffiliateID { get; set; }
        public virtual Affiliate Affiliate { get; set; }
        public ICollection<MeasurePoint> MeasurePoints { get; set; }
        public ICollection<SupplyPoint> SupplyPoints { get; set; }
    }

    public class MeasurePoint : NamedEntity
    {
        public int ConsumerID { get; set; }
        [JsonIgnore]
        public int ElectricMeterID { get; set; }
        [JsonIgnore]
        public int VoltageTransformerID { get; set; }
        [JsonIgnore]
        public int CurrentTransformerID { get; set; }
        [JsonIgnore]
        public virtual Consumer Consumer { get; set; }
        public virtual ElectricMeter ElectricMeter { get; set; }
        [ForeignKey("VoltageTransformerID")]
        public virtual VoltageTransformer VoltageTransformer { get; set; }
        [ForeignKey("CurrentTransformerID")]
        public virtual CurrentTransformer CurrentTransformer { get; set; }
        [JsonIgnore]
        public ICollection<CalcLink> CalcLinks { get; set; }
    }

    public abstract class CheckedEntity : BaseEntity
    {
        public string Number { get; set; }
        public DateTime CheckDate { get; set; }
        public  int MeasurePointID { get; set; }
        [JsonIgnore]
        [ForeignKey("MeasurePointID")]
        public virtual MeasurePoint MeasurePoint { get; set; }
    }

    public class MeterType : NamedEntity
    {
        [JsonIgnore]
        public ICollection<ElectricMeter> ElectricMeters { get; set; }
    }

    public class ElectricMeter : CheckedEntity
    {
        [JsonIgnore]
        public int MeterTypeID { get; set; }
        public virtual MeterType MeterType { get; set; }
    }

    public class CTType : NamedEntity
    {
        [JsonIgnore]
        public ICollection<CurrentTransformer> CurrentTransformers { get; set; }
    }

    public class CurrentTransformer : CheckedEntity
    {
        public double CTC;
        [JsonIgnore]
        public int CTTypeID { get; set; }
        public virtual CTType CTType { get; set; }
    }

    public class VTType : NamedEntity
    {
        [JsonIgnore]
        public ICollection<VoltageTransformer> VoltageTransformers { get; set; }
    }

    public class VoltageTransformer : CheckedEntity
    {
        public double VTC;
        [JsonIgnore]
        public int VTTypeID { get; set; }
        public virtual VTType VTType { get; set; }
    }

    public class SupplyPoint : NamedEntity
    {
        public double MaxPower;
        public int ConsumerID { get; set; }
        [JsonIgnore]
        public virtual Consumer Consumer { get; set; }
        [JsonIgnore]
        public ICollection<CalcUnit> CalcUnits { get; set; }
    }

    public class CalcUnit : BaseEntity
    {
        public DateTime TimeFrom { get; set; }
        public DateTime TimeTo { get; set; }
        public int SupplyPointID { get; set; }
        [JsonIgnore]
        public virtual SupplyPoint SupplyPoint { get; set; }        
        public ICollection<CalcLink> CalcLinks { get; set; }
    }

    public class CalcLink : BaseEntity
    {
        public double Multiplier;
        public int? CalcUnitID { get; set; }
        public int? MeasurePointID { get; set; }
        [JsonIgnore]
        public virtual CalcUnit CalcUnit { get; set; }
        [JsonIgnore]
        public virtual MeasurePoint MeasurePoint { get; set; }
    }
}

