using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using System.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace ACDC
{
    public class PowerContext : DbContext
    {
        public PowerContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder
                .UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=EFQuerying;Trusted_Connection=True;ConnectRetryCount=0");
            optionsBuilder.EnableSensitiveDataLogging();
        }

        public DbSet<Company> Companys { get; set; }
        public DbSet<Affiliate> Affiliates { get; set; }
        public DbSet<Consumer> Consumers { get; set; }
        public DbSet<MeasurePoint> MeasurePoints { get; set; }
        public DbSet<MeterType> MeterTypes { get; set; }
        public DbSet<ElectricMeter> ElectricMeters { get; set; }
        public DbSet<CTType> CTTypes { get; set; }
        public DbSet<VTType> VTTypes { get; set; }
        public DbSet<VoltageTransformer> VoltageTransformers { get; set; }
        public DbSet<CurrentTransformer> CurrentTransformers { get; set; }
        public DbSet<SupplyPoint> SupplyPoints { get; set; }
        public DbSet<CalcUnit> CalcUnits { get; set; }
        public DbSet<CalcLink> CalcLinks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // В организации есть дочерние организации
            modelBuilder.Entity<Company>()
                .HasMany(c => c.Affiliates)
                .WithOne(a => a.Company)
                .HasForeignKey(a => a.CompanyID);

            // В дочерней организации есть объекты потребления
            modelBuilder.Entity<Affiliate>()
                .HasMany(a => a.Consumers)
                .WithOne(c => c.Affiliate)
                .HasForeignKey(c => c.AffiliateID);

            // В объекте потребления есть точки измерения
            modelBuilder.Entity<Consumer>()
                .HasMany(c => c.MeasurePoints)
                .WithOne(m => m.Consumer)
                .HasForeignKey(m => m.ConsumerID);

            // В объекте потребления есть точки поставки
            modelBuilder.Entity<Consumer>()
                .HasMany(c => c.SupplyPoints)
                .WithOne(s => s.Consumer)
                .HasForeignKey(s => s.ConsumerID);

            // В точке поставки есть расчетные приборы
            modelBuilder.Entity<SupplyPoint>()
                .HasMany(s => s.CalcUnits)
                .WithOne(c => c.SupplyPoint)
                .HasForeignKey(c => c.SupplyPointID);

            // Расчетные приборы связаны с точками измерения через таблицу связей с соответствующими коэффициентами
            modelBuilder.Entity<CalcUnit>()
                .HasMany(c => c.CalcLinks)
                .WithOne(l => l.CalcUnit)
                .HasForeignKey(l => l.CalcUnitID);

            modelBuilder.Entity<MeasurePoint>()
                .HasMany(c => c.CalcLinks)
                .WithOne(l => l.MeasurePoint)
                .HasForeignKey(l => l.MeasurePointID);

            //Счетчики и трансформаторы привязаны к точке измерения.
            modelBuilder.Entity<MeasurePoint>()
                .HasOne(m => m.ElectricMeter)
                .WithOne(e => e.MeasurePoint)
                .HasForeignKey<ElectricMeter>(e => e.MeasurePointID);

            modelBuilder.Entity<MeasurePoint>()
                .HasOne(m => m.CurrentTransformer)
                .WithOne(c => c.MeasurePoint)
                .HasForeignKey<CurrentTransformer>(c => c.MeasurePointID);

            modelBuilder.Entity<MeasurePoint>()
                .HasOne(m => m.VoltageTransformer)
                .WithOne(v => v.MeasurePoint)
                .HasForeignKey<VoltageTransformer>(v => v.MeasurePointID);

            /*
            modelBuilder.Entity<ElectricMeter>()
                .HasOne(m => m.MeterType);

            modelBuilder.Entity<VoltageTransformer>()
                .HasOne(m => m.VTType);

            modelBuilder.Entity<CurrentTransformer>()
                .HasOne(m => m.CTType);
                */

            modelBuilder.Entity<MeterType>()
                 .HasMany(m => m.ElectricMeters)
                 .WithOne(m => m.MeterType)
                 .HasForeignKey(m => m.MeterTypeID);

             modelBuilder.Entity<CTType>()
                 .HasMany(m => m.CurrentTransformers)
                 .WithOne(m => m.CTType)
                 .HasForeignKey(m => m.CTTypeID);

             modelBuilder.Entity<VTType>()
                 .HasMany(m => m.VoltageTransformers)
                 .WithOne(m => m.VTType)
                 .HasForeignKey(m => m.VTTypeID);
                 
            // Тестовые данные

            // Организации
            var Romashka = new Company() { ID = 1, Title = "ООО Ромашка", Address = "Улица Зеленая д.1" };
            var Drova = new Company() { ID = 2, Title = "ООО Дрова", Address = "Улица Зеленая д.3" };

            modelBuilder.Entity<Company>().HasData(new Company[] { Romashka, Drova });

            // Дочерние организации
            var Lutik = new Affiliate() { ID = 1, Title = "ООО Лютик", Address = "Улица Синяя д.10", CompanyID = 1/*, Company = Romashka */};
            var Sosna = new Affiliate() { ID = 2, Title = "ООО Сосна", Address = "Улица Лесная д.1", CompanyID = 2/*, Company = Drova */};

            modelBuilder.Entity<Affiliate>().HasData(new Affiliate[] { Lutik, Sosna });

            // Объекты потребления
            var Object1 = new Consumer() { ID = 1, Title = "Склад ООО Лютик", Address = "Улица Синяя д.10", AffiliateID = 1/*, Affiliate = Lutik */};
            var Object2 = new Consumer() { ID = 2, Title = "Магазин ООО Сосна", Address = "Улица Лесная д.1", AffiliateID = 2/*, Affiliate = Sosna */};

            modelBuilder.Entity<Consumer>().HasData(new Consumer[] { Object1, Object2 });

            //Точки измерения
            var MPoint11 = new MeasurePoint() { ID = 1, Title = "Лютик точка измерения 1", ElectricMeterID = 1, VoltageTransformerID = 1, CurrentTransformerID = 1, ConsumerID = 1/*, Consumer = Object1 */};
            var MPoint12 = new MeasurePoint() { ID = 2, Title = "Лютик точка измерения 2", ElectricMeterID = 2, VoltageTransformerID = 2, CurrentTransformerID = 2, ConsumerID = 1/*, Consumer = Object1 */};
            var MPoint21 = new MeasurePoint() { ID = 3, Title = "Сосна точка измерения 1", ElectricMeterID = 3, VoltageTransformerID = 3, CurrentTransformerID = 3, ConsumerID = 2/*, Consumer = Object2 */};
            var MPoint22 = new MeasurePoint() { ID = 4, Title = "Сосна точка измерения 2", ElectricMeterID = 4, VoltageTransformerID = 4, CurrentTransformerID = 4, ConsumerID = 2/*, Consumer = Object2 */};

            modelBuilder.Entity<MeasurePoint>().HasData(new MeasurePoint[] { MPoint11, MPoint12, MPoint21, MPoint22 });

            //Точки поставки
            var SPoint11 = new SupplyPoint() { ID = 1, Title = "Лютик точка поставки 1", MaxPower = 50, ConsumerID = 1/*, Consumer = Object1 */};
            var SPoint12 = new SupplyPoint() { ID = 2, Title = "Лютик точка поставки 2", MaxPower = 20, ConsumerID = 1/*, Consumer = Object1 */};
            var SPoint21 = new SupplyPoint() { ID = 3, Title = "Сосна точка поставки 1", MaxPower = 60, ConsumerID = 2/*, Consumer = Object2 */};
            var SPoint22 = new SupplyPoint() { ID = 4, Title = "Сосна точка поставки 2", MaxPower = 70, ConsumerID = 2/*, Consumer = Object2 */};

            modelBuilder.Entity<SupplyPoint>().HasData(new SupplyPoint[] { SPoint11, SPoint12, SPoint21, SPoint22 });


            //Счетчики
            var MeterType1 = new MeterType { ID = 1, Title = "Тип 1" };
            modelBuilder.Entity<MeterType>().HasData(new MeterType[] { MeterType1 });

            var Meter11 = new ElectricMeter() { ID = 1, Number = "12345", /*MeterType = MeterType1,*/ MeterTypeID = 1, CheckDate = new DateTime(2019, 03, 01), /*MeasurePoint = MPoint11,*/ MeasurePointID = 1 };
            var Meter12 = new ElectricMeter() { ID = 2, Number = "12346", /*MeterType = MeterType1,*/ MeterTypeID = 1, CheckDate = new DateTime(2020, 03, 01), /*MeasurePoint = MPoint12,*/ MeasurePointID = 2 };
            var Meter21 = new ElectricMeter() { ID = 3, Number = "12347", /*MeterType = MeterType1,*/ MeterTypeID = 1, CheckDate = new DateTime(2019, 08, 01), /*MeasurePoint = MPoint21,*/ MeasurePointID = 3 };
            var Meter22 = new ElectricMeter() { ID = 4, Number = "12348", /*MeterType = MeterType1,*/ MeterTypeID = 1, CheckDate = new DateTime(2019, 07, 01), /*MeasurePoint = MPoint21,*/ MeasurePointID = 4 };
            modelBuilder.Entity<ElectricMeter>().HasData(new ElectricMeter[] { Meter11, Meter12, Meter21, Meter22 });

            //Трансформаторы тока
            var VTType1 = new VTType { ID = 1, Title = "Тип 1" };
            modelBuilder.Entity<VTType>().HasData(new VTType[] { VTType1 });

            var VT11 = new VoltageTransformer() { ID = 1, Number = "12345", /*VTType = VTType1,*/ VTTypeID = 1, CheckDate = new DateTime(2019, 03, 01), /*MeasurePoint = MPoint11,*/ MeasurePointID = 1 };
            var VT12 = new VoltageTransformer() { ID = 2, Number = "12346", /*VTType = VTType1,*/ VTTypeID = 1, CheckDate = new DateTime(2020, 03, 01), /*MeasurePoint = MPoint12,*/ MeasurePointID = 2 };
            var VT21 = new VoltageTransformer() { ID = 3, Number = "12347", /*VTType = VTType1,*/ VTTypeID = 1, CheckDate = new DateTime(2019, 08, 01), /*MeasurePoint = MPoint21,*/ MeasurePointID = 3 };
            var VT22 = new VoltageTransformer() { ID = 4, Number = "12348", /*VTType = VTType1,*/ VTTypeID = 1, CheckDate = new DateTime(2019, 07, 01), /*MeasurePoint = MPoint22,*/ MeasurePointID = 4 };
            modelBuilder.Entity<VoltageTransformer>().HasData(new VoltageTransformer[] { VT11, VT12, VT21, VT22 });

            //Трансформаторы тока
            var CTType1 = new CTType { ID = 1, Title = "Тип 1" };
            modelBuilder.Entity<CTType>().HasData(new CTType[] { CTType1 });

            var CT11 = new CurrentTransformer() { ID = 1, Number = "12345", /*CTType = CTType1,*/ CTTypeID = 1, CheckDate = new DateTime(2019, 03, 01), /*MeasurePoint = MPoint11,*/ MeasurePointID = 1 };
            var CT12 = new CurrentTransformer() { ID = 2, Number = "12346", /*CTType = CTType1,*/ CTTypeID = 1, CheckDate = new DateTime(2020, 03, 01), /*MeasurePoint = MPoint12,*/ MeasurePointID = 2 };
            var CT21 = new CurrentTransformer() { ID = 3, Number = "12347", /*CTType = CTType1,*/ CTTypeID = 1, CheckDate = new DateTime(2019, 08, 01), /*MeasurePoint = MPoint21,*/ MeasurePointID = 3 };
            var CT22 = new CurrentTransformer() { ID = 4, Number = "12348", /*CTType = CTType1,*/ CTTypeID = 1, CheckDate = new DateTime(2019, 07, 01), /*MeasurePoint = MPoint22,*/ MeasurePointID = 4 };
            modelBuilder.Entity<CurrentTransformer>().HasData(new CurrentTransformer[] { CT11, CT12, CT21, CT22 });

            //Расчетные приборы учета
            var CU1 = new CalcUnit() { ID = 1, TimeFrom = new DateTime(2018, 07, 01), TimeTo = new DateTime(2018, 08, 01), SupplyPointID =1 };
            var CU2 = new CalcUnit() { ID = 2, TimeFrom = new DateTime(2018, 07, 01), TimeTo = new DateTime(2018, 08, 01), SupplyPointID = 2 };
            var CU3 = new CalcUnit() { ID = 3, TimeFrom = new DateTime(2019, 07, 01), TimeTo = new DateTime(2019, 08, 01), SupplyPointID = 3 };
            var CU4 = new CalcUnit() { ID = 4, TimeFrom = new DateTime(2017, 07, 01), TimeTo = new DateTime(2017, 08, 01), SupplyPointID = 4 };
            modelBuilder.Entity<CalcUnit>().HasData(new CalcUnit[] { CU1, CU2, CU3, CU4 });

            //Связка расчетных приборов со счетчиками
            var Link1 = new CalcLink { ID = 1, CalcUnitID = 1, MeasurePointID = 1, Multiplier = 1 };
            modelBuilder.Entity<CalcLink>().HasData(new CalcLink[] { Link1 });

            base.OnModelCreating(modelBuilder);
        }
    }
}
