using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Energizer.Models
{
    public abstract class BaseEntity
    {
        public int ID { get; set; }
    }

    public abstract class NamedEntity : BaseEntity
    {
        [Display(Name = "Название")]
        public string Title { get; set; }
    }

    public abstract class AddressedEntity : NamedEntity
    {
        [Display(Name = "Адрес")]
        public string Address { get; set; }
    }

    public class Company : AddressedEntity
    {
    }

    public class Affiliate : AddressedEntity
    {
        public int CompanyID { get; set; }
        public virtual Company Company { get; set; }
    }

    public class Consumer : AddressedEntity
    {
        public int AffiliateID { get; set; }
        public virtual Affiliate Affiliate { get; set; }        
    }

    public class MeasurePoint : NamedEntity
    {
        [Display(Name = "Объект потребления")]
        public int ConsumerID { get; set; }
        [Display(Name = "Электросчетчик")]
        public int ElectricMeterID { get; set; }
        [Display(Name = "Трансформатор напряжения")]
        public int VoltageTransformerID { get; set; }
        [Display(Name = "Трансформатор тока")]
        public int CurrentTransformerID { get; set; }
        [Display(Name = "Объект потребления")]
        public virtual Consumer Consumer { get; set; }
        [Display(Name = "Электросчетчик")]
        public virtual ElectricMeter ElectricMeter { get; set; }
        [Display(Name = "Трансформатор напряжения")]
        public virtual VoltageTransformer VoltageTransformer { get; set; }
        [Display(Name = "Трансформатор тока")]
        public virtual CurrentTransformer CurrentTransformer { get; set; }
        public SelectList Consumers { set; get; }
    }

    public abstract class CheckedEntity : BaseEntity
    {
        [Display(Name = "Серийный номер")]
        public string Number { get; set; }
        [Display(Name = "Дата поверки")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CheckDate { get; set; }
        public int MeasurePointID { get; set; }
        public virtual MeasurePoint MeasurePoint { get; set; }
    }

    public class MeterType : NamedEntity
    {
    }

    public class ElectricMeter : CheckedEntity
    {
        [Display(Name = "Тип счетчика")]
        public int MeterTypeID { get; set; }
        [Display(Name = "Тип счетчика")]
        public virtual MeterType MeterType { get; set; }
    }

    public class CTType : NamedEntity
    {
    }

    public class CurrentTransformer : CheckedEntity
    {
        [Display(Name = "КТТ")]
        public double CTC;
        [Display(Name = "Тип трансформатора тока")]
        public int CTTypeID { get; set; }
        [Display(Name = "Тип трансформатора тока")]
        public virtual CTType CTType { get; set; }
    }

    public class VTType : NamedEntity
    {
    }

    public class VoltageTransformer : CheckedEntity
    {
        [Display(Name = "КТН")]
        public double VTC;
        [Display(Name = "Тип трансформатора напряжения")]
        public int VTTypeID { get; set; }
        [Display(Name = "Тип трансформатора напряжения")]
        public virtual VTType VTType { get; set; }
    }

    public class SupplyPoint : NamedEntity
    {
        public double MaxPower;
        public int ConsumerID { get; set; }
        public virtual Consumer Consumer { get; set; }
    }

    public class CalcUnit : BaseEntity
    {
        [Display(Name = "Время С")]
        public DateTime TimeFrom { get; set; }
        [Display(Name = "Время По")]
        public DateTime TimeTo { get; set; }
        [Display(Name = "Точка поставки")]
        public int SupplyPointID { get; set; }
        [Display(Name = "Точка поставки")]
        public virtual SupplyPoint SupplyPoint { get; set; }
    }

    public class CalcLink : BaseEntity
    {
        public double Multiplier;
        public int? CalcUnitID { get; set; }
        public int? MeasurePointID { get; set; }
        public virtual CalcUnit CalcUnit { get; set; }
        public virtual MeasurePoint MeasurePoint { get; set; }
    }
}
