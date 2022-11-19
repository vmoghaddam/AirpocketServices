using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiPlanning.ViewModels
{
    public class FlightDto
    {
        public int ID { get; set; }
        public int? TypeID { get; set; }
        public int? RegisterID { get; set; }
        public int? FlightTypeID { get; set; }
        public int? FlightStatusID { get; set; }
        public int? AirlineOperatorsID { get; set; }
        public int? FlightGroupID { get; set; }
        public string FlightNumber { get; set; }
        public int? FromAirportId { get; set; }
        public int? ToAirportId { get; set; }
        public string STDRAW { get; set; }
        public string STARAW { get; set; }
        public DateTime? STD { get; set; }
        public DateTime? STA { get; set; }
        public DateTime? ChocksOut { get; set; }
        public DateTime? Takeoff { get; set; }
        public DateTime? Landing { get; set; }
        public DateTime? ChocksIn { get; set; }
        public int? FlightH { get; set; }
        public byte? FlightM { get; set; }
        public int? BlockH { get; set; }
        public byte? BlockM { get; set; }
        public decimal? GWTO { get; set; }
        public decimal? GWLand { get; set; }
        public decimal? FuelPlanned { get; set; }
        public decimal? FuelActual { get; set; }
        public decimal? FuelDeparture { get; set; }
        public decimal? FuelArrival { get; set; }
        public int? PaxAdult { get; set; }
        public int? NightTime { get; set; }
        public int? PaxInfant { get; set; }
        public int? PaxChild { get; set; }
        public int? CargoWeight { get; set; }
        public int? CargoUnitID { get; set; }
        public int? BaggageCount { get; set; }
        public int? CustomerId { get; set; }
        public int? FlightPlanId { get; set; }
        public DateTime? DateCreate { get; set; }
        public int? CargoCount { get; set; }
        public int? BaggageWeight { get; set; }
        public int? FuelUnitID { get; set; }
        public string ArrivalRemark { get; set; }
        public string DepartureRemark { get; set; }
        public int? EstimatedDelay { get; set; }
        public int? FlightStatusUserId { get; set; }

        public int? LinkedFlight { get; set; }
        public int? LinkedReason { get; set; }
        public string LinkedRemark { get; set; }

        public int? BoxId { get; set; }
        public Nullable<int> CPCrewId { get; set; }
        public string CPRegister { get; set; }
        public Nullable<int> CPPositionId { get; set; }
        public Nullable<int> CPFlightTypeId { get; set; }
        public Nullable<int> CPFDPItemId { get; set; }
        public Nullable<bool> CPDH { get; set; }

        public Nullable<int> PFLR { get; set; }
        public Nullable<int> CPFDPId { get; set; }
        public string CPInstructor { get; set; }
        public string CPP1 { get; set; }
        public string CPP2 { get; set; }
        public string CPSCCM { get; set; }
        public string CPISCCM { get; set; }

        public int? SMSNira { get; set; }
        public string UserName { get; set; }

        public int? Interval { get; set; }
        public DateTime? IntervalFrom { get; set; }
        public DateTime? IntervalTo { get; set; }
        public string IntervalFromRAW { get; set; }
        public string IntervalToRAW { get; set; }
        public List<int> Days { get; set; }
        public List<int> DaysUTC { get; set; }
        public DateTime? RefDate { get; set; }
        public string RefDateRAW { get; set; }
        public int? RefDays { get; set; }
        public int? CheckTime { get; set; }

        public int? STDHH { get; set; }
        public int? STDMM { get; set; }

        public string ChrCode { get; set; }
        public string ChrTitle { get; set; }




        public static void Fill(Models.FlightInformation entity, ViewModels.FlightDto flightinformation)
        {
            entity.ID = flightinformation.ID;
            entity.TypeID = flightinformation.TypeID;
            entity.RegisterID = flightinformation.RegisterID;
            entity.FlightTypeID = flightinformation.FlightTypeID;
            entity.FlightStatusID = flightinformation.FlightStatusID;
            entity.AirlineOperatorsID = flightinformation.AirlineOperatorsID;
            entity.FlightGroupID = flightinformation.FlightGroupID;
            entity.FlightNumber = flightinformation.FlightNumber;
            entity.FromAirportId = flightinformation.FromAirportId;
            entity.ToAirportId = flightinformation.ToAirportId;
            entity.STD = flightinformation.STD;
            entity.STA = flightinformation.STA;
            entity.ChocksOut = flightinformation.ChocksOut;
            entity.Takeoff = flightinformation.Takeoff;
            entity.Landing = flightinformation.Landing;
            entity.ChocksIn = flightinformation.ChocksIn;
            entity.FlightH = flightinformation.FlightH;
            entity.FlightM = flightinformation.FlightM;
            entity.BlockH = flightinformation.BlockH;
            entity.BlockM = flightinformation.BlockM;
            entity.GWTO = flightinformation.GWTO;
            entity.GWLand = flightinformation.GWLand;
            entity.FuelPlanned = flightinformation.FuelPlanned;
            entity.FuelActual = flightinformation.FuelActual;
            entity.FuelDeparture = flightinformation.FuelDeparture;
            entity.FuelArrival = flightinformation.FuelArrival;
            entity.PaxAdult = flightinformation.PaxAdult;
            entity.PaxInfant = flightinformation.PaxInfant;
            entity.PaxChild = flightinformation.PaxChild;
            entity.CargoWeight = flightinformation.CargoWeight;
            entity.CargoUnitID = flightinformation.CargoUnitID;
            entity.BaggageCount = flightinformation.BaggageCount;
            entity.CustomerId = flightinformation.CustomerId;
            entity.FlightPlanId = flightinformation.FlightPlanId;
            entity.DateCreate = flightinformation.DateCreate;
            entity.CargoCount = flightinformation.CargoCount;
            entity.BaggageWeight = flightinformation.BaggageWeight;
            entity.FuelUnitID = flightinformation.FuelUnitID;
            entity.ArrivalRemark = flightinformation.ArrivalRemark;
            entity.DepartureRemark = flightinformation.DepartureRemark;
            entity.EstimatedDelay = flightinformation.EstimatedDelay;
            entity.FlightStatusUserId = flightinformation.FlightStatusUserId;
            entity.ChrCode = flightinformation.ChrCode;
            entity.ChrTitle = flightinformation.ChrTitle;
        }

        public static void FillNotID(Models.FlightInformation entity, ViewModels.FlightDto flightinformation)
        {
            //entity.ID = flightinformation.ID;
            entity.TypeID = flightinformation.TypeID;
            entity.RegisterID = flightinformation.RegisterID;
            entity.FlightTypeID = flightinformation.FlightTypeID;
            entity.FlightStatusID = flightinformation.FlightStatusID;
            entity.AirlineOperatorsID = flightinformation.AirlineOperatorsID;
            entity.FlightGroupID = flightinformation.FlightGroupID;
            entity.FlightNumber = flightinformation.FlightNumber;
            entity.FromAirportId = flightinformation.FromAirportId;
            entity.ToAirportId = flightinformation.ToAirportId;
            entity.STD = flightinformation.STD;
            entity.STA = flightinformation.STA;
            entity.ChocksOut = flightinformation.ChocksOut;
            entity.Takeoff = flightinformation.Takeoff;
            entity.Landing = flightinformation.Landing;
            entity.ChocksIn = flightinformation.ChocksIn;
            entity.FlightH = flightinformation.FlightH;
            entity.FlightM = flightinformation.FlightM;
            entity.BlockH = flightinformation.BlockH;
            entity.BlockM = flightinformation.BlockM;
            entity.GWTO = flightinformation.GWTO;
            entity.GWLand = flightinformation.GWLand;
            entity.FuelPlanned = flightinformation.FuelPlanned;
            entity.FuelActual = flightinformation.FuelActual;
            entity.FuelDeparture = flightinformation.FuelDeparture;
            entity.FuelArrival = flightinformation.FuelArrival;
            entity.PaxAdult = flightinformation.PaxAdult;
            entity.PaxInfant = flightinformation.PaxInfant;
            entity.PaxChild = flightinformation.PaxChild;
            entity.CargoWeight = flightinformation.CargoWeight;
            entity.CargoUnitID = flightinformation.CargoUnitID;
            entity.BaggageCount = flightinformation.BaggageCount;
            entity.CustomerId = flightinformation.CustomerId;
            entity.FlightPlanId = flightinformation.FlightPlanId;
            entity.DateCreate = flightinformation.DateCreate;
            entity.CargoCount = flightinformation.CargoCount;
            entity.BaggageWeight = flightinformation.BaggageWeight;
            entity.FuelUnitID = flightinformation.FuelUnitID;
            entity.ArrivalRemark = flightinformation.ArrivalRemark;
            entity.DepartureRemark = flightinformation.DepartureRemark;
            entity.EstimatedDelay = flightinformation.EstimatedDelay;
            entity.FlightStatusUserId = flightinformation.FlightStatusUserId;
        }

        public static void FillForGroupUpdate(Models.FlightInformation entity, ViewModels.FlightDto flightinformation)
        {
            //entity.ID = flightinformation.ID;
            //entity.TypeID = flightinformation.TypeID;
            //entity.RegisterID = flightinformation.RegisterID;
            entity.FlightTypeID = flightinformation.FlightTypeID;
            entity.FlightStatusID = flightinformation.FlightStatusID;
            entity.AirlineOperatorsID = flightinformation.AirlineOperatorsID;
            entity.FlightGroupID = flightinformation.FlightGroupID;
            entity.FlightNumber = flightinformation.FlightNumber;
            entity.FromAirportId = flightinformation.FromAirportId;
            entity.ToAirportId = flightinformation.ToAirportId;
            entity.STD = flightinformation.STD;
            entity.STA = flightinformation.STA;
            entity.ChocksOut = flightinformation.ChocksOut;
            entity.Takeoff = flightinformation.Takeoff;
            entity.Landing = flightinformation.Landing;
            entity.ChocksIn = flightinformation.ChocksIn;
            entity.FlightH = flightinformation.FlightH;
            entity.FlightM = flightinformation.FlightM;
            entity.BlockH = flightinformation.BlockH;
            entity.BlockM = flightinformation.BlockM;
            entity.GWTO = flightinformation.GWTO;
            entity.GWLand = flightinformation.GWLand;
            entity.FuelPlanned = flightinformation.FuelPlanned;
            entity.FuelActual = flightinformation.FuelActual;
            entity.FuelDeparture = flightinformation.FuelDeparture;
            entity.FuelArrival = flightinformation.FuelArrival;
            entity.PaxAdult = flightinformation.PaxAdult;
            entity.PaxInfant = flightinformation.PaxInfant;
            entity.PaxChild = flightinformation.PaxChild;
            entity.CargoWeight = flightinformation.CargoWeight;
            entity.CargoUnitID = flightinformation.CargoUnitID;
            entity.BaggageCount = flightinformation.BaggageCount;
            entity.CustomerId = flightinformation.CustomerId;
            entity.FlightPlanId = flightinformation.FlightPlanId;
            entity.DateCreate = flightinformation.DateCreate;
            entity.CargoCount = flightinformation.CargoCount;
            entity.BaggageWeight = flightinformation.BaggageWeight;
            entity.FuelUnitID = flightinformation.FuelUnitID;
            entity.ArrivalRemark = flightinformation.ArrivalRemark;
            entity.DepartureRemark = flightinformation.DepartureRemark;
            entity.EstimatedDelay = flightinformation.EstimatedDelay;
            entity.FlightStatusUserId = flightinformation.FlightStatusUserId;
            entity.ChrCode = flightinformation.ChrCode;
            entity.ChrTitle = flightinformation.ChrTitle;
        }
        public static void FillDto(Models.FlightInformation entity, ViewModels.FlightDto flightinformation)
        {
            flightinformation.ID = entity.ID;
            flightinformation.TypeID = entity.TypeID;
            flightinformation.RegisterID = entity.RegisterID;
            flightinformation.FlightTypeID = entity.FlightTypeID;
            flightinformation.FlightStatusID = entity.FlightStatusID;
            flightinformation.AirlineOperatorsID = entity.AirlineOperatorsID;
            flightinformation.FlightGroupID = entity.FlightGroupID;
            flightinformation.FlightNumber = entity.FlightNumber;
            flightinformation.FromAirportId = entity.FromAirportId;
            flightinformation.ToAirportId = entity.ToAirportId;
            flightinformation.STD = entity.STD;
            flightinformation.STA = entity.STA;
            flightinformation.ChocksOut = entity.ChocksOut;
            flightinformation.Takeoff = entity.Takeoff;
            flightinformation.Landing = entity.Landing;
            flightinformation.ChocksIn = entity.ChocksIn;
            flightinformation.FlightH = entity.FlightH;
            flightinformation.FlightM = entity.FlightM;
            flightinformation.BlockH = entity.BlockH;
            flightinformation.BlockM = entity.BlockM;
            flightinformation.GWTO = entity.GWTO;
            flightinformation.GWLand = entity.GWLand;
            flightinformation.FuelPlanned = entity.FuelPlanned;
            flightinformation.FuelActual = entity.FuelActual;
            flightinformation.FuelDeparture = entity.FuelDeparture;
            flightinformation.FuelArrival = entity.FuelArrival;
            flightinformation.PaxAdult = entity.PaxAdult;
            flightinformation.PaxInfant = entity.PaxInfant;
            flightinformation.PaxChild = entity.PaxChild;
            flightinformation.CargoWeight = entity.CargoWeight;
            flightinformation.CargoUnitID = entity.CargoUnitID;
            flightinformation.BaggageCount = entity.BaggageCount;
            flightinformation.CustomerId = entity.CustomerId;
            flightinformation.FlightPlanId = entity.FlightPlanId;
            flightinformation.DateCreate = entity.DateCreate;
            flightinformation.CargoCount = entity.CargoCount;
            flightinformation.BaggageWeight = entity.BaggageWeight;
            flightinformation.FuelUnitID = entity.FuelUnitID;
            flightinformation.ArrivalRemark = entity.ArrivalRemark;
            flightinformation.DepartureRemark = entity.DepartureRemark;
            flightinformation.EstimatedDelay = entity.EstimatedDelay;
            flightinformation.FlightStatusUserId = entity.FlightStatusUserId;
        }
    }
    public class ViewFlightsGanttDto
    {
        public int ID { get; set; }
        public string ResKey { get; set; }
        public DateTime? DutyStartLocal { get; set; }
        public DateTime? DutyEndLocal { get; set; }
        public string ResTitle { get; set; }
        public int Id { get; set; }
        public bool IsBox { get; set; }
        public int? Duty { get; set; }
        public double? MaxFDPExtended { get; set; }



        public int IsDutyOver { get; set; }
        public int WOCLError { get; set; }
        public int? Flight { get; set; }
        public bool HasCrew { get; set; }
        public bool HasCrewProblem { get; set; }
        public bool ExtendedBySplitDuty { get; set; }
        // public bool SplitDuty { get; set; }

        public bool AllCrewAssigned { get; set; }
        public int? BoxId { get; set; }
        public string Flights { get; set; }
        public int? CalendarId { get; set; }
        public DateTime? Date { get; set; }
        public List<ViewFlightInformationDto> BoxItems = new List<ViewFlightInformationDto>();
        public int taskID { get; set; }
        public int? FlightPlanId { get; set; }
        public int? BaggageCount { get; set; }
        public int? CargoUnitID { get; set; }
        public string CargoUnit { get; set; }
        public string FuelUnit { get; set; }
        public int? CargoWeight { get; set; }
        public int? PaxChild { get; set; }
        public int? PaxInfant { get; set; }
        public int? FlightStatusUserId { get; set; }
        public int? PaxAdult { get; set; }
        public int? NightTime { get; set; }
        public int? TotalPax { get; set; }
        public int? PaxOver { get; set; }
        public decimal? FuelArrival { get; set; }
        public decimal? FuelDeparture { get; set; }
        public decimal? FuelActual { get; set; }
        public decimal? FuelPlanned { get; set; }
        public decimal? GWLand { get; set; }
        public decimal? GWTO { get; set; }
        public byte? BlockM { get; set; }
        public int? BlockH { get; set; }
        public int? FlightH { get; set; }
        public byte? FlightM { get; set; }
        public DateTime? ChocksIn { get; set; }
        public DateTime? DateStatus { get; set; }
        public DateTime? Landing { get; set; }
        public DateTime? Takeoff { get; set; }
        public DateTime? ChocksOut { get; set; }
        public DateTime? STD { get; set; }
        public DateTime? STA { get; set; }
        public DateTime STDDay { get; set; }
        public int FlightStatusID { get; set; }
        public int? RegisterID { get; set; }
        public int? FlightTypeID { get; set; }
        public int? TypeId { get; set; }
        public int? AirlineOperatorsID { get; set; }
        public string FlightNumber { get; set; }
        public int? FromAirport { get; set; }
        public int? ToAirport { get; set; }
        public DateTime? STAPlanned { get; set; }
        public DateTime? STDPlanned { get; set; }
        public int? FlightHPlanned { get; set; }
        public int? FlightMPlanned { get; set; }
        public string FlightPlan { get; set; }
        public int? CustomerId { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? DateActive { get; set; }
        public string FromAirportName { get; set; }
        public string FromAirportIATA { get; set; }
        public int? FromAirportCityId { get; set; }
        public string ToAirportName { get; set; }
        public string ToAirportIATA { get; set; }
        public int? ToAirportCityId { get; set; }
        public string FromAirportCity { get; set; }
        public string ToAirportCity { get; set; }
        public string AircraftType { get; set; }
        public string Register { get; set; }
        public int? MSN { get; set; }
        public string FlightStatus { get; set; }
        public string FlightStatusBgColor { get; set; }
        public string FlightStatusColor { get; set; }
        public string FlightStatusClass { get; set; }
        public string from { get; set; }
        public string to { get; set; }
        public string notes { get; set; }
        public int status { get; set; }
        public int progress { get; set; }
        public string taskName { get; set; }
        public DateTime? startDate { get; set; }
        public decimal? duration { get; set; }
        public int taskId { get; set; }
        public int? FlightGroupID { get; set; }
        public int? PlanId { get; set; }
        public int? ManufacturerId { get; set; }
        public string Manufacturer { get; set; }
        public string ToCountry { get; set; }
        public string ToSortName { get; set; }
        public string ToCity { get; set; }
        public string FromSortName { get; set; }
        public string FromContry { get; set; }
        public string FromCity { get; set; }
        public double? FromLatitude { get; set; }
        public double? FromLongitude { get; set; }
        public double? ToLatitude { get; set; }
        public double? ToLongitude { get; set; }
        public int? CargoCount { get; set; }
        public int? BaggageWeight { get; set; }
        public int? FuelUnitID { get; set; }
        public string ArrivalRemark { get; set; }
        public string DepartureRemark { get; set; }
        public int? TotalSeat { get; set; }
        public int? EstimatedDelay { get; set; }
        public int? CancelReasonId { get; set; }
        public string CancelReason { get; set; }
        public string CancelRemark { get; set; }
        public DateTime? CancelDate { get; set; }

        public int? RedirectReasonId { get; set; }
        public string RedirectReason { get; set; }
        public string RedirectRemark { get; set; }
        public DateTime? RedirectDate { get; set; }
        public int? RampReasonId { get; set; }
        public string RampReason { get; set; }
        public string RampRemark { get; set; }
        public DateTime? RampDate { get; set; }

        public int? OToAirportId { get; set; }
        public string OToAirportIATA { get; set; }
        public DateTime? OSTA { get; set; }
        public List<int> resourceId { get; set; }

        public int? BaseId { get; set; }
        public string BaseIATA { get; set; }

        public string BaseName { get; set; }

        public decimal? Defuel { get; set; }
        public decimal? FPFuel { get; set; }
        public bool? SplitDuty { get; set; }
        public bool? IsPositioning { get; set; }
        public int? FPFlightHH { get; set; }
        public int? FPFlightMM { get; set; }

        public int MaleFemalError { get; set; }
        public int? MatchingListError { get; set; }
        public int? LinkedFlight { get; set; }
        public string LinkedFlightNumber { get; set; }
        public decimal? UsedFuel { get; set; }
        public int? JLBLHH { get; set; }
        public int? JLBLMM { get; set; }
        public int? PFLR { get; set; }
        public int? OTypeId { get; set; }

        public int? ChrAdult { get; set; }
        public int? ChrChild { get; set; }
        public int? ChrInfant { get; set; }
        public int? ChrCapacity { get; set; }

        public string ChrCode { get; set; }
        public string ChrTitle { get; set; }


        public int? DefaultChrId { get; set; }
        public string OAircraftType { get; set; }
        public static void Fill(Models.ViewFlightsGantt entity, ViewModels.ViewFlightsGanttDto viewflightinformation)
        {
            entity.ID = viewflightinformation.ID;

            entity.FlightPlanId = viewflightinformation.FlightPlanId;
            entity.BaggageCount = viewflightinformation.BaggageCount;
            entity.CargoUnitID = viewflightinformation.CargoUnitID;
            entity.CargoUnit = viewflightinformation.CargoUnit;
            entity.CargoWeight = viewflightinformation.CargoWeight;
            entity.PaxChild = viewflightinformation.PaxChild;
            entity.PaxInfant = viewflightinformation.PaxInfant;
            entity.PaxAdult = viewflightinformation.PaxAdult;
            entity.FuelArrival = viewflightinformation.FuelArrival;
            entity.FuelDeparture = viewflightinformation.FuelDeparture;
            entity.FuelActual = viewflightinformation.FuelActual;
            entity.FuelPlanned = viewflightinformation.FuelPlanned;
            entity.GWLand = viewflightinformation.GWLand;
            entity.GWTO = viewflightinformation.GWTO;
            entity.BlockM = viewflightinformation.BlockM;
            entity.BlockH = viewflightinformation.BlockH;
            entity.FlightH = viewflightinformation.FlightH;
            entity.FlightM = viewflightinformation.FlightM;
            entity.ChocksIn = viewflightinformation.ChocksIn;
            entity.Landing = viewflightinformation.Landing;
            entity.Takeoff = viewflightinformation.Takeoff;
            entity.ChocksOut = viewflightinformation.ChocksOut;
            entity.STD = viewflightinformation.STD;
            entity.STA = viewflightinformation.STA;
            entity.FlightStatusID = viewflightinformation.FlightStatusID;
            entity.RegisterID = viewflightinformation.RegisterID;
            entity.FlightTypeID = viewflightinformation.FlightTypeID ?? 0;
            entity.TypeId = (int)viewflightinformation.TypeId;
            entity.AirlineOperatorsID = viewflightinformation.AirlineOperatorsID;
            entity.FlightNumber = viewflightinformation.FlightNumber;
            entity.FromAirport = viewflightinformation.FromAirport;
            entity.ToAirport = viewflightinformation.ToAirport;
            entity.STAPlanned = viewflightinformation.STAPlanned;
            entity.STDPlanned = viewflightinformation.STDPlanned;
            entity.FlightHPlanned = viewflightinformation.FlightHPlanned;
            entity.FlightMPlanned = viewflightinformation.FlightMPlanned;
            entity.FlightPlan = viewflightinformation.FlightPlan;
            entity.CustomerId = viewflightinformation.CustomerId;
            entity.IsActive = viewflightinformation.IsActive;
            entity.DateActive = viewflightinformation.DateActive;
            entity.FromAirportName = viewflightinformation.FromAirportName;
            entity.FromAirportIATA = viewflightinformation.FromAirportIATA;
            //entity.FromAirportCityId = viewflightinformation.FromAirportCityId;
            entity.ToAirportName = viewflightinformation.ToAirportName;
            entity.ToAirportIATA = viewflightinformation.ToAirportIATA;
            //  entity.ToAirportCityId = viewflightinformation.ToAirportCityId;
            //  entity.FromAirportCity = viewflightinformation.FromAirportCity;
            //  entity.ToAirportCity = viewflightinformation.ToAirportCity;
            entity.AircraftType = viewflightinformation.AircraftType;
            entity.Register = viewflightinformation.Register;
            entity.MSN = viewflightinformation.MSN;
            entity.FlightStatus = viewflightinformation.FlightStatus;
            entity.FlightStatusBgColor = viewflightinformation.FlightStatusBgColor;
            entity.FlightStatusColor = viewflightinformation.FlightStatusColor;
            entity.FlightStatusClass = viewflightinformation.FlightStatusClass;
            entity.from = viewflightinformation.from;
            entity.to = viewflightinformation.to;
            entity.notes = viewflightinformation.notes;
            entity.status = viewflightinformation.status;
            entity.progress = viewflightinformation.progress;
            entity.taskName = viewflightinformation.taskName;
            entity.startDate = viewflightinformation.startDate;
            entity.duration = viewflightinformation.duration;
            entity.taskId = viewflightinformation.taskId;
            entity.FlightGroupID = viewflightinformation.FlightGroupID;
           // entity.PlanId = viewflightinformation.PlanId;
            entity.ManufacturerId = viewflightinformation.ManufacturerId;
            entity.Manufacturer = viewflightinformation.Manufacturer;
            // entity.ToCountry = viewflightinformation.ToCountry;
            // entity.ToSortName = viewflightinformation.ToSortName;
            // entity.ToCity = viewflightinformation.ToCity;
            entity.FromSortName = viewflightinformation.FromSortName;
            entity.FromContry = viewflightinformation.FromContry;
            //  entity.FromCity = viewflightinformation.FromCity;
            entity.FromLatitude = viewflightinformation.FromLatitude;
            entity.FromLongitude = viewflightinformation.FromLongitude;
            entity.ToLatitude = viewflightinformation.ToLatitude;
            entity.ToLongitude = viewflightinformation.ToLongitude;
            entity.CargoCount = viewflightinformation.CargoCount;
            entity.BaggageWeight = viewflightinformation.BaggageWeight;
            entity.FuelUnitID = viewflightinformation.FuelUnitID;
            entity.ArrivalRemark = viewflightinformation.ArrivalRemark;
            entity.DepartureRemark = viewflightinformation.DepartureRemark;
            entity.TotalSeat = viewflightinformation.TotalSeat;
            entity.EstimatedDelay = viewflightinformation.EstimatedDelay ?? 0;
            entity.PaxOver = viewflightinformation.PaxOver ?? 0;
            entity.TotalPax = viewflightinformation.TotalPax;
            entity.FuelUnit = viewflightinformation.FuelUnit;
            entity.DateStatus = viewflightinformation.DateStatus;
            entity.FlightStatusUserId = viewflightinformation.FlightStatusUserId;
            entity.CancelDate = viewflightinformation.CancelDate;
            entity.CancelReasonId = viewflightinformation.CancelReasonId;
            entity.CancelReason = viewflightinformation.CancelReason;
            entity.CancelRemark = viewflightinformation.CancelRemark;
            entity.RedirectDate = viewflightinformation.RedirectDate;
            entity.RedirectReasonId = viewflightinformation.RedirectReasonId;
            entity.RedirectReason = viewflightinformation.RedirectReason;
            entity.RedirectRemark = viewflightinformation.RedirectRemark;
            entity.OSTA = viewflightinformation.OSTA;
            entity.OToAirportIATA = viewflightinformation.OToAirportIATA;
            entity.OToAirportId = viewflightinformation.OToAirportId;

            entity.RampDate = viewflightinformation.RampDate;
            entity.RampReasonId = viewflightinformation.RampReasonId;
            entity.RampReason = viewflightinformation.RampReason;
            entity.RampRemark = viewflightinformation.RampRemark;

            entity.FPFlightHH = viewflightinformation.FPFlightHH;
            entity.FPFlightMM = viewflightinformation.FPFlightMM;
            entity.Defuel = viewflightinformation.Defuel;
            entity.FPFuel = viewflightinformation.FPFuel;





        }
        public static void FillDto(Models.ViewFlightsGantt entity, ViewModels.ViewFlightsGanttDto viewflightinformation, int tzoffset, int? utc = 0)
        {

            tzoffset = Helper.GetTimeOffset((DateTime)entity.STD);
            var tzoffset2 = Helper.GetTimeOffset((DateTime)entity.STD);
            var tzoffset3 = TimeZoneInfo.Local.GetUtcOffset((DateTime)entity.STD).TotalMinutes;
            if (utc == 1)
            { tzoffset = 0; tzoffset3 = 0; }
            viewflightinformation.Date = entity.Date;
            viewflightinformation.OTypeId = entity.OTypeId;
            viewflightinformation.OAircraftType = entity.OAircraftType;
            viewflightinformation.resourceId = new List<int>();
            viewflightinformation.ID = entity.ID;
            viewflightinformation.Id = entity.ID;
            viewflightinformation.IsBox = false;
            viewflightinformation.HasCrew = false;
            viewflightinformation.BoxId = entity.BoxId;
            viewflightinformation.CalendarId = entity.CalendarId;
            viewflightinformation.HasCrewProblem = false;
            viewflightinformation.AllCrewAssigned = false;
            viewflightinformation.FlightPlanId = entity.FlightPlanId;
            viewflightinformation.BaggageCount = entity.BaggageCount;
            viewflightinformation.CargoUnitID = entity.CargoUnitID;
            viewflightinformation.CargoUnit = entity.CargoUnit;
            viewflightinformation.CargoWeight = entity.CargoWeight;
            viewflightinformation.PaxChild = entity.PaxChild;
            viewflightinformation.PaxInfant = entity.PaxInfant;
            viewflightinformation.PaxAdult = entity.PaxAdult;
            viewflightinformation.FuelArrival = entity.FuelArrival;
            viewflightinformation.FuelDeparture = entity.FuelDeparture;
            viewflightinformation.FuelActual = entity.FuelActual;
            viewflightinformation.FuelPlanned = entity.FuelPlanned;
            viewflightinformation.GWLand = entity.GWLand;
            viewflightinformation.GWTO = entity.GWTO;
            viewflightinformation.BlockM = entity.BlockM;
            viewflightinformation.BlockH = entity.BlockH;
            if (entity.FromAirportIATA == "NJF" || entity.FromAirportIATA == "BSR")
                viewflightinformation.GWLand = 180;
            else
                viewflightinformation.GWLand = tzoffset2;

            if (entity.ToAirportIATA == "NJF" || entity.ToAirportIATA == "BSR")
                viewflightinformation.GWTO = 180;
            else
                viewflightinformation.GWTO = tzoffset2;


            viewflightinformation.FlightH = entity.FlightH;
            viewflightinformation.FlightM = entity.FlightM;
            viewflightinformation.ChocksIn = entity.ChocksIn == null ? null : (Nullable<DateTime>)((DateTime)entity.ChocksIn).AddMinutes(tzoffset3);
            viewflightinformation.Landing = entity.Landing == null ? null : (Nullable<DateTime>)((DateTime)entity.Landing).AddMinutes(tzoffset3); ;
            viewflightinformation.Takeoff = entity.Takeoff == null ? null : (Nullable<DateTime>)((DateTime)entity.Takeoff).AddMinutes(tzoffset3);
            viewflightinformation.ChocksOut = entity.ChocksOut == null ? null : (Nullable<DateTime>)((DateTime)entity.ChocksOut).AddMinutes(tzoffset3);
            viewflightinformation.STD = entity.STD == null ? null : (Nullable<DateTime>)((DateTime)entity.STD).AddMinutes(tzoffset3);
            viewflightinformation.STA = entity.STA == null ? null : (Nullable<DateTime>)((DateTime)entity.STA).AddMinutes(tzoffset3);
            viewflightinformation.RampDate = entity.RampDate == null ? null : (Nullable<DateTime>)((DateTime)entity.RampDate).AddMinutes(tzoffset3);
            viewflightinformation.FlightStatusID = (int)entity.FlightStatusID;
            viewflightinformation.RegisterID = entity.RegisterID;
            viewflightinformation.FlightTypeID = entity.FlightTypeID;
            viewflightinformation.TypeId = entity.TypeId;
            viewflightinformation.AirlineOperatorsID = entity.AirlineOperatorsID;
            viewflightinformation.FlightNumber = entity.FlightNumber;
            viewflightinformation.FromAirport = entity.FromAirport;
            viewflightinformation.ToAirport = entity.ToAirport;
            viewflightinformation.STAPlanned = entity.STAPlanned == null ? null : (Nullable<DateTime>)((DateTime)entity.STAPlanned).AddMinutes(tzoffset3);
            viewflightinformation.STDPlanned = entity.STDPlanned == null ? null : (Nullable<DateTime>)((DateTime)entity.STDPlanned).AddMinutes(tzoffset3);
            viewflightinformation.FlightHPlanned = entity.FlightHPlanned;
            viewflightinformation.FlightMPlanned = entity.FlightMPlanned;
            viewflightinformation.FlightPlan = entity.FlightPlan;
            viewflightinformation.CustomerId = entity.CustomerId;
            viewflightinformation.IsActive = entity.IsActive;
            viewflightinformation.DateActive = entity.DateActive;
            viewflightinformation.FromAirportName = entity.FromAirportName;
            viewflightinformation.FromAirportIATA = entity.FromAirportIATA;
            // viewflightinformation.FromAirportCityId = entity.FromAirportCityId;
            viewflightinformation.ToAirportName = entity.ToAirportName;
            viewflightinformation.ToAirportIATA = entity.ToAirportIATA;
            //viewflightinformation.ToAirportCityId = entity.ToAirportCityId;
            //viewflightinformation.FromAirportCity = entity.FromAirportCity;
            // viewflightinformation.ToAirportCity = entity.ToAirportCity;
            viewflightinformation.AircraftType = entity.AircraftType;
            viewflightinformation.Register = entity.Register;
            viewflightinformation.MSN = entity.MSN;
            viewflightinformation.FlightStatus = entity.FlightStatus;
            viewflightinformation.FlightStatusBgColor = entity.FlightStatusBgColor;
            viewflightinformation.FlightStatusColor = entity.FlightStatusColor;
            viewflightinformation.FlightStatusClass = entity.FlightStatusClass;
            viewflightinformation.from = entity.from;
            viewflightinformation.to = entity.to;
            viewflightinformation.notes = entity.notes;
            viewflightinformation.status = entity.status;
            viewflightinformation.progress = entity.progress;
            viewflightinformation.taskName = entity.taskName;
            viewflightinformation.startDate = entity.startDate == null ? null : (Nullable<DateTime>)((DateTime)entity.startDate).AddMinutes(tzoffset3);
            viewflightinformation.duration = entity.duration;
            viewflightinformation.taskId = entity.taskId;
            viewflightinformation.taskID = entity.taskId;
            viewflightinformation.FlightGroupID = entity.FlightGroupID;
            viewflightinformation.PlanId = entity.PlanId;
            viewflightinformation.ManufacturerId = entity.ManufacturerId;
            viewflightinformation.Manufacturer = entity.Manufacturer;
            //viewflightinformation.ToCountry = entity.ToCountry;
            //viewflightinformation.ToSortName = entity.ToSortName;
            //viewflightinformation.ToCity = entity.ToCity;
            viewflightinformation.FromSortName = entity.FromSortName;
            viewflightinformation.FromContry = entity.FromContry;
            //viewflightinformation.FromCity = entity.FromCity;
            viewflightinformation.FromLatitude = entity.FromLatitude;
            viewflightinformation.FromLongitude = entity.FromLongitude;
            viewflightinformation.ToLatitude = entity.ToLatitude;
            viewflightinformation.ToLongitude = entity.ToLongitude;
            viewflightinformation.CargoCount = entity.CargoCount;
            viewflightinformation.BaggageWeight = entity.BaggageWeight;
            viewflightinformation.FuelUnitID = entity.FuelUnitID;
            viewflightinformation.ArrivalRemark = entity.ArrivalRemark;
            viewflightinformation.DepartureRemark = entity.DepartureRemark;
            viewflightinformation.TotalSeat = entity.TotalSeat;
            viewflightinformation.EstimatedDelay = entity.EstimatedDelay;
            viewflightinformation.PaxOver = entity.PaxOver;
            viewflightinformation.TotalPax = entity.TotalPax;
            viewflightinformation.NightTime = entity.NightTime;


            viewflightinformation.FuelUnit = entity.FuelUnit;
            viewflightinformation.DateStatus = entity.DateStatus == null ? null : (Nullable<DateTime>)((DateTime)entity.DateStatus).AddMinutes(tzoffset3);
            viewflightinformation.FlightStatusUserId = entity.FlightStatusUserId;

            viewflightinformation.CancelDate = entity.CancelDate == null ? null : (Nullable<DateTime>)((DateTime)entity.CancelDate).AddMinutes(tzoffset3);
            viewflightinformation.CancelReasonId = entity.CancelReasonId;
            viewflightinformation.CancelReason = entity.CancelReason;
            viewflightinformation.CancelRemark = entity.CancelRemark;



            viewflightinformation.RampReasonId = entity.RampReasonId;
            viewflightinformation.RampReason = entity.RampReason;
            viewflightinformation.RampRemark = entity.RampRemark;

            viewflightinformation.RedirectDate = entity.RedirectDate == null ? null : (Nullable<DateTime>)((DateTime)entity.RedirectDate).AddMinutes(tzoffset3); ;
            viewflightinformation.RedirectReasonId = entity.RedirectReasonId;
            viewflightinformation.RedirectReason = entity.RedirectReason;
            viewflightinformation.RedirectRemark = entity.RedirectRemark;
            viewflightinformation.OSTA = entity.OSTA;
            viewflightinformation.OToAirportIATA = entity.OToAirportIATA;
            viewflightinformation.OToAirportId = entity.OToAirportId;

            viewflightinformation.BaseIATA = entity.BaseIATA;
            viewflightinformation.BaseId = entity.BaseId;
            viewflightinformation.BaseName = entity.BaseName;

            viewflightinformation.FPFlightHH = entity.FPFlightHH;
            viewflightinformation.FPFlightMM = entity.FPFlightMM;
            viewflightinformation.Defuel = entity.Defuel;
            viewflightinformation.FPFuel = entity.FPFuel;

            viewflightinformation.SplitDuty = entity.SplitDuty;
            viewflightinformation.MaleFemalError = entity.MaleFemalError == null ? 0 : (int)entity.MaleFemalError;
            viewflightinformation.MatchingListError = entity.MatchingListError == null ? 0 : (int)entity.MatchingListError;
            viewflightinformation.LinkedFlight = entity.LinkedFlight;
            viewflightinformation.LinkedFlightNumber = entity.LinkedFlightNumber;
            viewflightinformation.UsedFuel = entity.UsedFuel;
            viewflightinformation.JLBLHH = entity.JLBLHH;
            viewflightinformation.JLBLMM = entity.JLBLMM;
            viewflightinformation.PFLR = entity.PFLR;

            viewflightinformation.ChrAdult = entity.ChrAdult;
            viewflightinformation.ChrChild = entity.ChrChild;
            viewflightinformation.ChrInfant = entity.ChrInfant;
            viewflightinformation.ChrCapacity = entity.ChrCapacity;
            viewflightinformation.ChrCode = entity.ChrCode;
            viewflightinformation.ChrTitle = entity.ChrTitle;

            viewflightinformation.DefaultChrId = entity.DefaultChrId;


        }
        public static void FillDto(Models.ViewLegTime entity, ViewModels.ViewFlightInformationDto viewflightinformation, int tzoffset)
        {
            viewflightinformation.Date = entity.Date;
            viewflightinformation.resourceId = new List<int>();
            viewflightinformation.ID = entity.ID;
            viewflightinformation.Id = entity.ID;
            viewflightinformation.IsBox = false;
            viewflightinformation.HasCrew = false;

            viewflightinformation.HasCrewProblem = false;
            viewflightinformation.AllCrewAssigned = false;
            viewflightinformation.FlightPlanId = entity.FlightPlanId;

            viewflightinformation.BlockM = entity.BlockM;
            viewflightinformation.BlockH = entity.BlockH;
            viewflightinformation.FlightH = entity.FlightH;
            viewflightinformation.FlightM = entity.FlightM;
            viewflightinformation.ChocksIn = entity.ChocksIn == null ? null : (Nullable<DateTime>)((DateTime)entity.ChocksIn).AddMinutes(tzoffset);
            viewflightinformation.Landing = entity.Landing == null ? null : (Nullable<DateTime>)((DateTime)entity.Landing).AddMinutes(tzoffset); ;
            viewflightinformation.Takeoff = entity.Takeoff == null ? null : (Nullable<DateTime>)((DateTime)entity.Takeoff).AddMinutes(tzoffset);
            viewflightinformation.ChocksOut = entity.ChocksOut == null ? null : (Nullable<DateTime>)((DateTime)entity.ChocksOut).AddMinutes(tzoffset);
            viewflightinformation.STD = entity.STD == null ? null : (Nullable<DateTime>)((DateTime)entity.STD).AddMinutes(tzoffset);
            viewflightinformation.STA = entity.STA == null ? null : (Nullable<DateTime>)((DateTime)entity.STA).AddMinutes(tzoffset);

            viewflightinformation.FlightStatusID = (int)entity.FlightStatusID;
            viewflightinformation.RegisterID = entity.RegisterID;
            viewflightinformation.FlightTypeID = entity.FlightTypeID;
            viewflightinformation.TypeId = entity.TypeId;

            viewflightinformation.FlightNumber = entity.FlightNumber;
            viewflightinformation.FromAirport = entity.FromAirport;
            viewflightinformation.ToAirport = entity.ToAirport;
            viewflightinformation.STAPlanned = entity.STAPlanned == null ? null : (Nullable<DateTime>)((DateTime)entity.STAPlanned).AddMinutes(tzoffset);
            viewflightinformation.STDPlanned = entity.STDPlanned == null ? null : (Nullable<DateTime>)((DateTime)entity.STDPlanned).AddMinutes(tzoffset);
            viewflightinformation.FlightHPlanned = entity.FlightHPlanned;
            viewflightinformation.FlightMPlanned = entity.FlightMPlanned;
            viewflightinformation.FlightPlan = entity.FlightPlan;
            viewflightinformation.CustomerId = entity.CustomerId;

            viewflightinformation.FromAirportIATA = entity.FromAirportIATA;
            // viewflightinformation.FromAirportCityId = entity.FromAirportCityId;

            viewflightinformation.ToAirportIATA = entity.ToAirportIATA;
            //viewflightinformation.ToAirportCityId = entity.ToAirportCityId;
            //viewflightinformation.FromAirportCity = entity.FromAirportCity;
            // viewflightinformation.ToAirportCity = entity.ToAirportCity;
            viewflightinformation.AircraftType = entity.AircraftType;
            viewflightinformation.Register = entity.Register;
            viewflightinformation.MSN = entity.MSN;
            viewflightinformation.FlightStatus = entity.FlightStatus;

            viewflightinformation.from = entity.from;
            viewflightinformation.to = entity.to;
            viewflightinformation.notes = entity.notes;
            viewflightinformation.status = (int)entity.status;
            viewflightinformation.progress = entity.progress;
            viewflightinformation.taskName = entity.taskName;
            viewflightinformation.duration = entity.duration;
            viewflightinformation.taskId = entity.taskId;
            viewflightinformation.taskID = entity.taskId;


            viewflightinformation.startDate = entity.startDate == null ? null : (Nullable<DateTime>)((DateTime)entity.startDate).AddMinutes(tzoffset);



            viewflightinformation.ArrivalRemark = entity.ArrivalRemark;
            viewflightinformation.DepartureRemark = entity.DepartureRemark;

            viewflightinformation.EstimatedDelay = entity.EstimatedDelay;





            viewflightinformation.OSTA = entity.OSTA;
            viewflightinformation.OToAirportIATA = entity.OToAirportIATA;
            viewflightinformation.OToAirportId = entity.OToAirportId;



            viewflightinformation.FPFlightHH = entity.FPFlightHH;
            viewflightinformation.FPFlightMM = entity.FPFlightMM;





        }
      

        public static ViewFlightsGanttDto GetDto(Models.ViewFlightsGantt entity, int tzoffset)
        {
            ViewModels.ViewFlightsGanttDto viewflightinformation = new ViewFlightsGanttDto();
            viewflightinformation.resourceId = new List<int>();
            viewflightinformation.ID = entity.ID;
            viewflightinformation.FlightPlanId = entity.FlightPlanId;
            viewflightinformation.BaggageCount = entity.BaggageCount;
            viewflightinformation.CargoUnitID = entity.CargoUnitID;
            viewflightinformation.CargoUnit = entity.CargoUnit;
            viewflightinformation.CargoWeight = entity.CargoWeight;
            viewflightinformation.PaxChild = entity.PaxChild;
            viewflightinformation.PaxInfant = entity.PaxInfant;
            viewflightinformation.PaxAdult = entity.PaxAdult;
            viewflightinformation.FuelArrival = entity.FuelArrival;
            viewflightinformation.FuelDeparture = entity.FuelDeparture;
            viewflightinformation.FuelActual = entity.FuelActual;
            viewflightinformation.FuelPlanned = entity.FuelPlanned;
            viewflightinformation.GWLand = entity.GWLand;
            viewflightinformation.GWTO = entity.GWTO;
            viewflightinformation.BlockM = entity.BlockM;
            viewflightinformation.BlockH = entity.BlockH;
            viewflightinformation.FlightH = entity.FlightH;
            viewflightinformation.FlightM = entity.FlightM;
            viewflightinformation.ChocksIn = entity.ChocksIn == null ? null : (Nullable<DateTime>)((DateTime)entity.ChocksIn).AddMinutes(tzoffset);
            viewflightinformation.Landing = entity.Landing == null ? null : (Nullable<DateTime>)((DateTime)entity.Landing).AddMinutes(tzoffset); ;
            viewflightinformation.Takeoff = entity.Takeoff == null ? null : (Nullable<DateTime>)((DateTime)entity.Takeoff).AddMinutes(tzoffset);
            viewflightinformation.ChocksOut = entity.ChocksOut == null ? null : (Nullable<DateTime>)((DateTime)entity.ChocksOut).AddMinutes(tzoffset);
            viewflightinformation.STD = entity.STD == null ? null : (Nullable<DateTime>)((DateTime)entity.STD).AddMinutes(tzoffset);
            viewflightinformation.STA = entity.STA == null ? null : (Nullable<DateTime>)((DateTime)entity.STA).AddMinutes(tzoffset);
            viewflightinformation.FlightStatusID = (int)entity.FlightStatusID;
            viewflightinformation.RegisterID = entity.RegisterID;
            viewflightinformation.FlightTypeID = entity.FlightTypeID;
            viewflightinformation.TypeId = entity.TypeId;
            viewflightinformation.AirlineOperatorsID = entity.AirlineOperatorsID;
            viewflightinformation.FlightNumber = entity.FlightNumber;
            viewflightinformation.FromAirport = entity.FromAirport;
            viewflightinformation.ToAirport = entity.ToAirport;
            viewflightinformation.STAPlanned = entity.STAPlanned == null ? null : (Nullable<DateTime>)((DateTime)entity.STAPlanned).AddMinutes(tzoffset);
            viewflightinformation.STDPlanned = entity.STDPlanned == null ? null : (Nullable<DateTime>)((DateTime)entity.STDPlanned).AddMinutes(tzoffset);
            viewflightinformation.FlightHPlanned = entity.FlightHPlanned;
            viewflightinformation.FlightMPlanned = entity.FlightMPlanned;
            viewflightinformation.FlightPlan = entity.FlightPlan;
            viewflightinformation.CustomerId = entity.CustomerId;
            viewflightinformation.IsActive = entity.IsActive;
            viewflightinformation.DateActive = entity.DateActive;
            viewflightinformation.FromAirportName = entity.FromAirportName;
            viewflightinformation.FromAirportIATA = entity.FromAirportIATA;
            // viewflightinformation.FromAirportCityId = entity.FromAirportCityId;
            viewflightinformation.ToAirportName = entity.ToAirportName;
            viewflightinformation.ToAirportIATA = entity.ToAirportIATA;
            // viewflightinformation.ToAirportCityId = entity.ToAirportCityId;
            // viewflightinformation.FromAirportCity = entity.FromAirportCity;
            // viewflightinformation.ToAirportCity = entity.ToAirportCity;
            viewflightinformation.AircraftType = entity.AircraftType;
            viewflightinformation.Register = entity.Register;
            viewflightinformation.MSN = entity.MSN;
            viewflightinformation.FlightStatus = entity.FlightStatus;
            viewflightinformation.FlightStatusBgColor = entity.FlightStatusBgColor;
            viewflightinformation.FlightStatusColor = entity.FlightStatusColor;
            viewflightinformation.FlightStatusClass = entity.FlightStatusClass;
            viewflightinformation.from = entity.from;
            viewflightinformation.to = entity.to;
            viewflightinformation.notes = entity.notes;
            viewflightinformation.status = entity.status;
            viewflightinformation.progress = entity.progress;
            viewflightinformation.taskName = entity.taskName;
            viewflightinformation.startDate = entity.startDate == null ? null : (Nullable<DateTime>)((DateTime)entity.startDate).AddMinutes(tzoffset);
            viewflightinformation.duration = entity.duration;
            viewflightinformation.taskId = entity.taskId;
            viewflightinformation.FlightGroupID = entity.FlightGroupID;
            viewflightinformation.PlanId = entity.PlanId;
            viewflightinformation.ManufacturerId = entity.ManufacturerId;
            viewflightinformation.Manufacturer = entity.Manufacturer;
            //viewflightinformation.ToCountry = entity.ToCountry;
            //viewflightinformation.ToSortName = entity.ToSortName;
            //viewflightinformation.ToCity = entity.ToCity;
            viewflightinformation.FromSortName = entity.FromSortName;
            viewflightinformation.FromContry = entity.FromContry;
            //viewflightinformation.FromCity = entity.FromCity;
            viewflightinformation.FromLatitude = entity.FromLatitude;
            viewflightinformation.FromLongitude = entity.FromLongitude;
            viewflightinformation.ToLatitude = entity.ToLatitude;
            viewflightinformation.ToLongitude = entity.ToLongitude;
            viewflightinformation.CargoCount = entity.CargoCount;
            viewflightinformation.BaggageWeight = entity.BaggageWeight;
            viewflightinformation.FuelUnitID = entity.FuelUnitID;
            viewflightinformation.ArrivalRemark = entity.ArrivalRemark;
            viewflightinformation.DepartureRemark = entity.DepartureRemark;
            viewflightinformation.TotalSeat = entity.TotalSeat;
            viewflightinformation.EstimatedDelay = entity.EstimatedDelay;
            viewflightinformation.PaxOver = entity.PaxOver;
            viewflightinformation.TotalPax = entity.TotalPax;
            viewflightinformation.FuelUnit = entity.FuelUnit;
            viewflightinformation.DateStatus = entity.DateStatus == null ? null : (Nullable<DateTime>)((DateTime)entity.DateStatus).AddMinutes(tzoffset);
            viewflightinformation.FlightStatusUserId = entity.FlightStatusUserId;
            viewflightinformation.CancelDate = entity.CancelDate;
            viewflightinformation.CancelReasonId = entity.CancelReasonId;
            viewflightinformation.CancelReason = entity.CancelReason;
            viewflightinformation.CancelRemark = entity.CancelRemark;



            viewflightinformation.RampDate = entity.RampDate == null ? null : (Nullable<DateTime>)((DateTime)entity.RampDate).AddMinutes(tzoffset);
            viewflightinformation.RampReasonId = entity.RampReasonId;
            viewflightinformation.RampReason = entity.RampReason;
            viewflightinformation.RampRemark = entity.RampRemark;

            viewflightinformation.RedirectDate = entity.RedirectDate;
            viewflightinformation.RedirectReasonId = entity.RedirectReasonId;
            viewflightinformation.RedirectReason = entity.RedirectReason;
            viewflightinformation.RedirectRemark = entity.RedirectRemark;
            viewflightinformation.OSTA = entity.OSTA;
            viewflightinformation.OToAirportIATA = entity.OToAirportIATA;
            viewflightinformation.OToAirportId = entity.OToAirportId;

            return viewflightinformation;
        }
    }

    public class ViewFlightInformationDto
    {
        public int ID { get; set; }
        public string ResKey { get; set; }
        public DateTime? DutyStartLocal { get; set; }
        public DateTime? DutyEndLocal { get; set; }
        public string ResTitle { get; set; }
        public int Id { get; set; }
        public bool IsBox { get; set; }
        public int? Duty { get; set; }
        public double? MaxFDPExtended { get; set; }



        public int IsDutyOver { get; set; }
        public int WOCLError { get; set; }
        public int? Flight { get; set; }
        public bool HasCrew { get; set; }
        public bool HasCrewProblem { get; set; }
        public bool ExtendedBySplitDuty { get; set; }
        // public bool SplitDuty { get; set; }

        public bool AllCrewAssigned { get; set; }
        public int? BoxId { get; set; }
        public string Flights { get; set; }
        public int? CalendarId { get; set; }
        public DateTime? Date { get; set; }
        public List<ViewFlightInformationDto> BoxItems = new List<ViewFlightInformationDto>();
        public int taskID { get; set; }
        public int? FlightPlanId { get; set; }
        public int? BaggageCount { get; set; }
        public int? CargoUnitID { get; set; }
        public string CargoUnit { get; set; }
        public string FuelUnit { get; set; }
        public int? CargoWeight { get; set; }
        public int? PaxChild { get; set; }
        public int? PaxInfant { get; set; }
        public int? FlightStatusUserId { get; set; }
        public int? PaxAdult { get; set; }
        public int? NightTime { get; set; }
        public int? TotalPax { get; set; }
        public int? PaxOver { get; set; }
        public decimal? FuelArrival { get; set; }
        public decimal? FuelDeparture { get; set; }
        public decimal? FuelActual { get; set; }
        public decimal? FuelPlanned { get; set; }
        public decimal? GWLand { get; set; }
        public decimal? GWTO { get; set; }
        public byte? BlockM { get; set; }
        public int? BlockH { get; set; }
        public int? FlightH { get; set; }
        public byte? FlightM { get; set; }
        public DateTime? ChocksIn { get; set; }
        public DateTime? DateStatus { get; set; }
        public DateTime? Landing { get; set; }
        public DateTime? Takeoff { get; set; }
        public DateTime? ChocksOut { get; set; }
        public DateTime? STD { get; set; }
        public DateTime? STA { get; set; }
        public DateTime STDDay { get; set; }
        public int FlightStatusID { get; set; }
        public int? RegisterID { get; set; }
        public int? FlightTypeID { get; set; }
        public int? TypeId { get; set; }
        public int? AirlineOperatorsID { get; set; }
        public string FlightNumber { get; set; }
        public int? FromAirport { get; set; }
        public int? ToAirport { get; set; }
        public DateTime? STAPlanned { get; set; }
        public DateTime? STDPlanned { get; set; }
        public int? FlightHPlanned { get; set; }
        public int? FlightMPlanned { get; set; }
        public string FlightPlan { get; set; }
        public int? CustomerId { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? DateActive { get; set; }
        public string FromAirportName { get; set; }
        public string FromAirportIATA { get; set; }
        public int? FromAirportCityId { get; set; }
        public string ToAirportName { get; set; }
        public string ToAirportIATA { get; set; }
        public int? ToAirportCityId { get; set; }
        public string FromAirportCity { get; set; }
        public string ToAirportCity { get; set; }
        public string AircraftType { get; set; }
        public string Register { get; set; }
        public int? MSN { get; set; }
        public string FlightStatus { get; set; }
        public string FlightStatusBgColor { get; set; }
        public string FlightStatusColor { get; set; }
        public string FlightStatusClass { get; set; }
        public string from { get; set; }
        public string to { get; set; }
        public string notes { get; set; }
        public int status { get; set; }
        public int progress { get; set; }
        public string taskName { get; set; }
        public DateTime? startDate { get; set; }
        public decimal? duration { get; set; }
        public int taskId { get; set; }
        public int? FlightGroupID { get; set; }
        public int? PlanId { get; set; }
        public int? ManufacturerId { get; set; }
        public string Manufacturer { get; set; }
        public string ToCountry { get; set; }
        public string ToSortName { get; set; }
        public string ToCity { get; set; }
        public string FromSortName { get; set; }
        public string FromContry { get; set; }
        public string FromCity { get; set; }
        public decimal? FromLatitude { get; set; }
        public decimal? FromLongitude { get; set; }
        public decimal? ToLatitude { get; set; }
        public decimal? ToLongitude { get; set; }
        public int? CargoCount { get; set; }
        public int? BaggageWeight { get; set; }
        public int? FuelUnitID { get; set; }
        public string ArrivalRemark { get; set; }
        public string DepartureRemark { get; set; }
        public int? TotalSeat { get; set; }
        public int? EstimatedDelay { get; set; }
        public int? CancelReasonId { get; set; }
        public string CancelReason { get; set; }
        public string CancelRemark { get; set; }
        public DateTime? CancelDate { get; set; }

        public int? RedirectReasonId { get; set; }
        public string RedirectReason { get; set; }
        public string RedirectRemark { get; set; }
        public DateTime? RedirectDate { get; set; }
        public int? RampReasonId { get; set; }
        public string RampReason { get; set; }
        public string RampRemark { get; set; }
        public DateTime? RampDate { get; set; }

        public int? OToAirportId { get; set; }
        public string OToAirportIATA { get; set; }
        public DateTime? OSTA { get; set; }
        public List<int> resourceId { get; set; }

        public int? BaseId { get; set; }
        public string BaseIATA { get; set; }

        public string BaseName { get; set; }

        public decimal? Defuel { get; set; }
        public decimal? FPFuel { get; set; }
        public bool? SplitDuty { get; set; }
        public bool? IsPositioning { get; set; }
        public int? FPFlightHH { get; set; }
        public int? FPFlightMM { get; set; }

        public int MaleFemalError { get; set; }
        public int? MatchingListError { get; set; }
        public int? LinkedFlight { get; set; }
        public string LinkedFlightNumber { get; set; }
        public decimal? UsedFuel { get; set; }
        public int? JLBLHH { get; set; }
        public int? JLBLMM { get; set; }
        public int? PFLR { get; set; }
        public static void Fill(Models.ViewFlightInformation entity, ViewModels.ViewFlightInformationDto viewflightinformation)
        {
            entity.ID = viewflightinformation.ID;

            entity.FlightPlanId = viewflightinformation.FlightPlanId;
            entity.BaggageCount = viewflightinformation.BaggageCount;
            entity.CargoUnitID = viewflightinformation.CargoUnitID;
            entity.CargoUnit = viewflightinformation.CargoUnit;
            entity.CargoWeight = viewflightinformation.CargoWeight;
            entity.PaxChild = viewflightinformation.PaxChild;
            entity.PaxInfant = viewflightinformation.PaxInfant;
            entity.PaxAdult = viewflightinformation.PaxAdult;
            entity.FuelArrival = viewflightinformation.FuelArrival;
            entity.FuelDeparture = viewflightinformation.FuelDeparture;
            entity.FuelActual = viewflightinformation.FuelActual;
            entity.FuelPlanned = viewflightinformation.FuelPlanned;
            entity.GWLand = viewflightinformation.GWLand;
            entity.GWTO = viewflightinformation.GWTO;
            entity.BlockM = viewflightinformation.BlockM;
            entity.BlockH = viewflightinformation.BlockH;
            entity.FlightH = viewflightinformation.FlightH;
            entity.FlightM = viewflightinformation.FlightM;
            entity.ChocksIn = viewflightinformation.ChocksIn;
            entity.Landing = viewflightinformation.Landing;
            entity.Takeoff = viewflightinformation.Takeoff;
            entity.ChocksOut = viewflightinformation.ChocksOut;
            entity.STD = viewflightinformation.STD;
            entity.STA = viewflightinformation.STA;
            entity.FlightStatusID = viewflightinformation.FlightStatusID;
            entity.RegisterID = viewflightinformation.RegisterID;
            entity.FlightTypeID = viewflightinformation.FlightTypeID;
            entity.TypeId = viewflightinformation.TypeId;
            entity.AirlineOperatorsID = viewflightinformation.AirlineOperatorsID;
            entity.FlightNumber = viewflightinformation.FlightNumber;
            entity.FromAirport = viewflightinformation.FromAirport;
            entity.ToAirport = viewflightinformation.ToAirport;
            entity.STAPlanned = viewflightinformation.STAPlanned;
            entity.STDPlanned = viewflightinformation.STDPlanned;
            entity.FlightHPlanned = viewflightinformation.FlightHPlanned;
            entity.FlightMPlanned = viewflightinformation.FlightMPlanned;
            entity.FlightPlan = viewflightinformation.FlightPlan;
            entity.CustomerId = viewflightinformation.CustomerId;
            entity.IsActive = viewflightinformation.IsActive;
            entity.DateActive = viewflightinformation.DateActive;
            entity.FromAirportName = viewflightinformation.FromAirportName;
            entity.FromAirportIATA = viewflightinformation.FromAirportIATA;
            //entity.FromAirportCityId = viewflightinformation.FromAirportCityId;
            entity.ToAirportName = viewflightinformation.ToAirportName;
            entity.ToAirportIATA = viewflightinformation.ToAirportIATA;
            //  entity.ToAirportCityId = viewflightinformation.ToAirportCityId;
            //  entity.FromAirportCity = viewflightinformation.FromAirportCity;
            //  entity.ToAirportCity = viewflightinformation.ToAirportCity;
            entity.AircraftType = viewflightinformation.AircraftType;
            entity.Register = viewflightinformation.Register;
            entity.MSN = viewflightinformation.MSN;
            entity.FlightStatus = viewflightinformation.FlightStatus;
            entity.FlightStatusBgColor = viewflightinformation.FlightStatusBgColor;
            entity.FlightStatusColor = viewflightinformation.FlightStatusColor;
            entity.FlightStatusClass = viewflightinformation.FlightStatusClass;
            entity.from = viewflightinformation.from;
            entity.to = viewflightinformation.to;
            entity.notes = viewflightinformation.notes;
            entity.status = viewflightinformation.status;
            entity.progress = viewflightinformation.progress;
            entity.taskName = viewflightinformation.taskName;
            entity.startDate = viewflightinformation.startDate;
            entity.duration = viewflightinformation.duration;
            entity.taskId = viewflightinformation.taskId;
            entity.FlightGroupID = viewflightinformation.FlightGroupID;
            entity.PlanId = viewflightinformation.PlanId;
            entity.ManufacturerId = viewflightinformation.ManufacturerId;
            entity.Manufacturer = viewflightinformation.Manufacturer;
            // entity.ToCountry = viewflightinformation.ToCountry;
            // entity.ToSortName = viewflightinformation.ToSortName;
            // entity.ToCity = viewflightinformation.ToCity;
            entity.FromSortName = viewflightinformation.FromSortName;
            entity.FromContry = viewflightinformation.FromContry;
            //  entity.FromCity = viewflightinformation.FromCity;
            entity.FromLatitude = viewflightinformation.FromLatitude;
            entity.FromLongitude = viewflightinformation.FromLongitude;
            entity.ToLatitude = viewflightinformation.ToLatitude;
            entity.ToLongitude = viewflightinformation.ToLongitude;
            entity.CargoCount = viewflightinformation.CargoCount;
            entity.BaggageWeight = viewflightinformation.BaggageWeight;
            entity.FuelUnitID = viewflightinformation.FuelUnitID;
            entity.ArrivalRemark = viewflightinformation.ArrivalRemark;
            entity.DepartureRemark = viewflightinformation.DepartureRemark;
            entity.TotalSeat = viewflightinformation.TotalSeat;
            entity.EstimatedDelay = viewflightinformation.EstimatedDelay;
            entity.PaxOver = viewflightinformation.PaxOver;
            entity.TotalPax = viewflightinformation.TotalPax;
            entity.FuelUnit = viewflightinformation.FuelUnit;
            entity.DateStatus = viewflightinformation.DateStatus;
            entity.FlightStatusUserId = viewflightinformation.FlightStatusUserId;
            entity.CancelDate = viewflightinformation.CancelDate;
            entity.CancelReasonId = viewflightinformation.CancelReasonId;
            entity.CancelReason = viewflightinformation.CancelReason;
            entity.CancelRemark = viewflightinformation.CancelRemark;
            entity.RedirectDate = viewflightinformation.RedirectDate;
            entity.RedirectReasonId = viewflightinformation.RedirectReasonId;
            entity.RedirectReason = viewflightinformation.RedirectReason;
            entity.RedirectRemark = viewflightinformation.RedirectRemark;
            entity.OSTA = viewflightinformation.OSTA;
            entity.OToAirportIATA = viewflightinformation.OToAirportIATA;
            entity.OToAirportId = viewflightinformation.OToAirportId;

            entity.RampDate = viewflightinformation.RampDate;
            entity.RampReasonId = viewflightinformation.RampReasonId;
            entity.RampReason = viewflightinformation.RampReason;
            entity.RampRemark = viewflightinformation.RampRemark;

            entity.FPFlightHH = viewflightinformation.FPFlightHH;
            entity.FPFlightMM = viewflightinformation.FPFlightMM;
            entity.Defuel = viewflightinformation.Defuel;
            entity.FPFuel = viewflightinformation.FPFuel;


        }
        public static void FillDto(Models.ViewFlightInformation entity, ViewModels.ViewFlightInformationDto viewflightinformation, int tzoffset, int? utc = 0)
        {

            tzoffset = Helper.GetTimeOffset((DateTime)entity.STD);
            if (utc == 1)
                tzoffset = 0;
            viewflightinformation.Date = entity.Date;
            viewflightinformation.resourceId = new List<int>();
            viewflightinformation.ID = entity.ID;
            viewflightinformation.Id = entity.ID;
            viewflightinformation.IsBox = false;
            viewflightinformation.HasCrew = false;
            viewflightinformation.BoxId = entity.BoxId;
            viewflightinformation.CalendarId = entity.CalendarId;
            viewflightinformation.HasCrewProblem = false;
            viewflightinformation.AllCrewAssigned = false;
            viewflightinformation.FlightPlanId = entity.FlightPlanId;
            viewflightinformation.BaggageCount = entity.BaggageCount;
            viewflightinformation.CargoUnitID = entity.CargoUnitID;
            viewflightinformation.CargoUnit = entity.CargoUnit;
            viewflightinformation.CargoWeight = entity.CargoWeight;
            viewflightinformation.PaxChild = entity.PaxChild;
            viewflightinformation.PaxInfant = entity.PaxInfant;
            viewflightinformation.PaxAdult = entity.PaxAdult;
            viewflightinformation.FuelArrival = entity.FuelArrival;
            viewflightinformation.FuelDeparture = entity.FuelDeparture;
            viewflightinformation.FuelActual = entity.FuelActual;
            viewflightinformation.FuelPlanned = entity.FuelPlanned;
            viewflightinformation.GWLand = entity.GWLand;
            viewflightinformation.GWTO = entity.GWTO;
            viewflightinformation.BlockM = entity.BlockM;
            viewflightinformation.BlockH = entity.BlockH;
            viewflightinformation.FlightH = entity.FlightH;
            viewflightinformation.FlightM = entity.FlightM;
            viewflightinformation.ChocksIn = entity.ChocksIn == null ? null : (Nullable<DateTime>)((DateTime)entity.ChocksIn).AddMinutes(tzoffset);
            viewflightinformation.Landing = entity.Landing == null ? null : (Nullable<DateTime>)((DateTime)entity.Landing).AddMinutes(tzoffset); ;
            viewflightinformation.Takeoff = entity.Takeoff == null ? null : (Nullable<DateTime>)((DateTime)entity.Takeoff).AddMinutes(tzoffset);
            viewflightinformation.ChocksOut = entity.ChocksOut == null ? null : (Nullable<DateTime>)((DateTime)entity.ChocksOut).AddMinutes(tzoffset);
            viewflightinformation.STD = entity.STD == null ? null : (Nullable<DateTime>)((DateTime)entity.STD).AddMinutes(tzoffset);
            viewflightinformation.STA = entity.STA == null ? null : (Nullable<DateTime>)((DateTime)entity.STA).AddMinutes(tzoffset);
            viewflightinformation.RampDate = entity.RampDate == null ? null : (Nullable<DateTime>)((DateTime)entity.RampDate).AddMinutes(tzoffset);
            viewflightinformation.FlightStatusID = (int)entity.FlightStatusID;
            viewflightinformation.RegisterID = entity.RegisterID;
            viewflightinformation.FlightTypeID = entity.FlightTypeID;
            viewflightinformation.TypeId = entity.TypeId;
            viewflightinformation.AirlineOperatorsID = entity.AirlineOperatorsID;
            viewflightinformation.FlightNumber = entity.FlightNumber;
            viewflightinformation.FromAirport = entity.FromAirport;
            viewflightinformation.ToAirport = entity.ToAirport;
            viewflightinformation.STAPlanned = entity.STAPlanned == null ? null : (Nullable<DateTime>)((DateTime)entity.STAPlanned).AddMinutes(tzoffset);
            viewflightinformation.STDPlanned = entity.STDPlanned == null ? null : (Nullable<DateTime>)((DateTime)entity.STDPlanned).AddMinutes(tzoffset);
            viewflightinformation.FlightHPlanned = entity.FlightHPlanned;
            viewflightinformation.FlightMPlanned = entity.FlightMPlanned;
            viewflightinformation.FlightPlan = entity.FlightPlan;
            viewflightinformation.CustomerId = entity.CustomerId;
            viewflightinformation.IsActive = entity.IsActive;
            viewflightinformation.DateActive = entity.DateActive;
            viewflightinformation.FromAirportName = entity.FromAirportName;
            viewflightinformation.FromAirportIATA = entity.FromAirportIATA;
            // viewflightinformation.FromAirportCityId = entity.FromAirportCityId;
            viewflightinformation.ToAirportName = entity.ToAirportName;
            viewflightinformation.ToAirportIATA = entity.ToAirportIATA;
            //viewflightinformation.ToAirportCityId = entity.ToAirportCityId;
            //viewflightinformation.FromAirportCity = entity.FromAirportCity;
            // viewflightinformation.ToAirportCity = entity.ToAirportCity;
            viewflightinformation.AircraftType = entity.AircraftType;
            viewflightinformation.Register = entity.Register;
            viewflightinformation.MSN = entity.MSN;
            viewflightinformation.FlightStatus = entity.FlightStatus;
            viewflightinformation.FlightStatusBgColor = entity.FlightStatusBgColor;
            viewflightinformation.FlightStatusColor = entity.FlightStatusColor;
            viewflightinformation.FlightStatusClass = entity.FlightStatusClass;
            viewflightinformation.from = entity.from;
            viewflightinformation.to = entity.to;
            viewflightinformation.notes = entity.notes;
            viewflightinformation.status = entity.status;
            viewflightinformation.progress = entity.progress;
            viewflightinformation.taskName = entity.taskName;
            viewflightinformation.startDate = entity.startDate == null ? null : (Nullable<DateTime>)((DateTime)entity.startDate).AddMinutes(tzoffset);
            viewflightinformation.duration = entity.duration;
            viewflightinformation.taskId = entity.taskId;
            viewflightinformation.taskID = entity.taskId;
            viewflightinformation.FlightGroupID = entity.FlightGroupID;
            viewflightinformation.PlanId = entity.PlanId;
            viewflightinformation.ManufacturerId = entity.ManufacturerId;
            viewflightinformation.Manufacturer = entity.Manufacturer;
            //viewflightinformation.ToCountry = entity.ToCountry;
            //viewflightinformation.ToSortName = entity.ToSortName;
            //viewflightinformation.ToCity = entity.ToCity;
            viewflightinformation.FromSortName = entity.FromSortName;
            viewflightinformation.FromContry = entity.FromContry;
            //viewflightinformation.FromCity = entity.FromCity;
            viewflightinformation.FromLatitude = entity.FromLatitude;
            viewflightinformation.FromLongitude = entity.FromLongitude;
            viewflightinformation.ToLatitude = entity.ToLatitude;
            viewflightinformation.ToLongitude = entity.ToLongitude;
            viewflightinformation.CargoCount = entity.CargoCount;
            viewflightinformation.BaggageWeight = entity.BaggageWeight;
            viewflightinformation.FuelUnitID = entity.FuelUnitID;
            viewflightinformation.ArrivalRemark = entity.ArrivalRemark;
            viewflightinformation.DepartureRemark = entity.DepartureRemark;
            viewflightinformation.TotalSeat = entity.TotalSeat;
            viewflightinformation.EstimatedDelay = entity.EstimatedDelay;
            viewflightinformation.PaxOver = entity.PaxOver;
            viewflightinformation.TotalPax = entity.TotalPax;
            viewflightinformation.NightTime = entity.NightTime;


            viewflightinformation.FuelUnit = entity.FuelUnit;
            viewflightinformation.DateStatus = entity.DateStatus == null ? null : (Nullable<DateTime>)((DateTime)entity.DateStatus).AddMinutes(tzoffset);
            viewflightinformation.FlightStatusUserId = entity.FlightStatusUserId;

            viewflightinformation.CancelDate = entity.CancelDate == null ? null : (Nullable<DateTime>)((DateTime)entity.CancelDate).AddMinutes(tzoffset);
            viewflightinformation.CancelReasonId = entity.CancelReasonId;
            viewflightinformation.CancelReason = entity.CancelReason;
            viewflightinformation.CancelRemark = entity.CancelRemark;



            viewflightinformation.RampReasonId = entity.RampReasonId;
            viewflightinformation.RampReason = entity.RampReason;
            viewflightinformation.RampRemark = entity.RampRemark;

            viewflightinformation.RedirectDate = entity.RedirectDate == null ? null : (Nullable<DateTime>)((DateTime)entity.RedirectDate).AddMinutes(tzoffset); ;
            viewflightinformation.RedirectReasonId = entity.RedirectReasonId;
            viewflightinformation.RedirectReason = entity.RedirectReason;
            viewflightinformation.RedirectRemark = entity.RedirectRemark;
            viewflightinformation.OSTA = entity.OSTA;
            viewflightinformation.OToAirportIATA = entity.OToAirportIATA;
            viewflightinformation.OToAirportId = entity.OToAirportId;

            viewflightinformation.BaseIATA = entity.BaseIATA;
            viewflightinformation.BaseId = entity.BaseId;
            viewflightinformation.BaseName = entity.BaseName;

            viewflightinformation.FPFlightHH = entity.FPFlightHH;
            viewflightinformation.FPFlightMM = entity.FPFlightMM;
            viewflightinformation.Defuel = entity.Defuel;
            viewflightinformation.FPFuel = entity.FPFuel;

            viewflightinformation.SplitDuty = entity.SplitDuty;
            viewflightinformation.MaleFemalError = entity.MaleFemalError == null ? 0 : (int)entity.MaleFemalError;
            viewflightinformation.MatchingListError = entity.MatchingListError == null ? 0 : (int)entity.MatchingListError;
            viewflightinformation.LinkedFlight = entity.LinkedFlight;
            viewflightinformation.LinkedFlightNumber = entity.LinkedFlightNumber;
            viewflightinformation.UsedFuel = entity.UsedFuel;
            viewflightinformation.JLBLHH = entity.JLBLHH;
            viewflightinformation.JLBLMM = entity.JLBLMM;
            viewflightinformation.PFLR = entity.PFLR;


        }
        public static void FillDto(Models.ViewLegTime entity, ViewModels.ViewFlightInformationDto viewflightinformation, int tzoffset)
        {
            viewflightinformation.Date = entity.Date;
            viewflightinformation.resourceId = new List<int>();
            viewflightinformation.ID = entity.ID;
            viewflightinformation.Id = entity.ID;
            viewflightinformation.IsBox = false;
            viewflightinformation.HasCrew = false;

            viewflightinformation.HasCrewProblem = false;
            viewflightinformation.AllCrewAssigned = false;
            viewflightinformation.FlightPlanId = entity.FlightPlanId;

            viewflightinformation.BlockM = entity.BlockM;
            viewflightinformation.BlockH = entity.BlockH;
            viewflightinformation.FlightH = entity.FlightH;
            viewflightinformation.FlightM = entity.FlightM;
            viewflightinformation.ChocksIn = entity.ChocksIn == null ? null : (Nullable<DateTime>)((DateTime)entity.ChocksIn).AddMinutes(tzoffset);
            viewflightinformation.Landing = entity.Landing == null ? null : (Nullable<DateTime>)((DateTime)entity.Landing).AddMinutes(tzoffset); ;
            viewflightinformation.Takeoff = entity.Takeoff == null ? null : (Nullable<DateTime>)((DateTime)entity.Takeoff).AddMinutes(tzoffset);
            viewflightinformation.ChocksOut = entity.ChocksOut == null ? null : (Nullable<DateTime>)((DateTime)entity.ChocksOut).AddMinutes(tzoffset);
            viewflightinformation.STD = entity.STD == null ? null : (Nullable<DateTime>)((DateTime)entity.STD).AddMinutes(tzoffset);
            viewflightinformation.STA = entity.STA == null ? null : (Nullable<DateTime>)((DateTime)entity.STA).AddMinutes(tzoffset);

            viewflightinformation.FlightStatusID = (int)entity.FlightStatusID;
            viewflightinformation.RegisterID = entity.RegisterID;
            viewflightinformation.FlightTypeID = entity.FlightTypeID;
            viewflightinformation.TypeId = entity.TypeId;

            viewflightinformation.FlightNumber = entity.FlightNumber;
            viewflightinformation.FromAirport = entity.FromAirport;
            viewflightinformation.ToAirport = entity.ToAirport;
            viewflightinformation.STAPlanned = entity.STAPlanned == null ? null : (Nullable<DateTime>)((DateTime)entity.STAPlanned).AddMinutes(tzoffset);
            viewflightinformation.STDPlanned = entity.STDPlanned == null ? null : (Nullable<DateTime>)((DateTime)entity.STDPlanned).AddMinutes(tzoffset);
            viewflightinformation.FlightHPlanned = entity.FlightHPlanned;
            viewflightinformation.FlightMPlanned = entity.FlightMPlanned;
            viewflightinformation.FlightPlan = entity.FlightPlan;
            viewflightinformation.CustomerId = entity.CustomerId;

            viewflightinformation.FromAirportIATA = entity.FromAirportIATA;
            // viewflightinformation.FromAirportCityId = entity.FromAirportCityId;

            viewflightinformation.ToAirportIATA = entity.ToAirportIATA;
            //viewflightinformation.ToAirportCityId = entity.ToAirportCityId;
            //viewflightinformation.FromAirportCity = entity.FromAirportCity;
            // viewflightinformation.ToAirportCity = entity.ToAirportCity;
            viewflightinformation.AircraftType = entity.AircraftType;
            viewflightinformation.Register = entity.Register;
            viewflightinformation.MSN = entity.MSN;
            viewflightinformation.FlightStatus = entity.FlightStatus;

            viewflightinformation.from = entity.from;
            viewflightinformation.to = entity.to;
            viewflightinformation.notes = entity.notes;
            viewflightinformation.status = (int)entity.status;
            viewflightinformation.progress = entity.progress;
            viewflightinformation.taskName = entity.taskName;
            viewflightinformation.duration = entity.duration;
            viewflightinformation.taskId = entity.taskId;
            viewflightinformation.taskID = entity.taskId;


            viewflightinformation.startDate = entity.startDate == null ? null : (Nullable<DateTime>)((DateTime)entity.startDate).AddMinutes(tzoffset);



            viewflightinformation.ArrivalRemark = entity.ArrivalRemark;
            viewflightinformation.DepartureRemark = entity.DepartureRemark;

            viewflightinformation.EstimatedDelay = entity.EstimatedDelay;





            viewflightinformation.OSTA = entity.OSTA;
            viewflightinformation.OToAirportIATA = entity.OToAirportIATA;
            viewflightinformation.OToAirportId = entity.OToAirportId;



            viewflightinformation.FPFlightHH = entity.FPFlightHH;
            viewflightinformation.FPFlightMM = entity.FPFlightMM;





        }
      
        public static ViewFlightInformationDto GetDto(Models.ViewFlightInformation entity, int tzoffset)
        {
            ViewModels.ViewFlightInformationDto viewflightinformation = new ViewFlightInformationDto();
            viewflightinformation.resourceId = new List<int>();
            viewflightinformation.ID = entity.ID;
            viewflightinformation.FlightPlanId = entity.FlightPlanId;
            viewflightinformation.BaggageCount = entity.BaggageCount;
            viewflightinformation.CargoUnitID = entity.CargoUnitID;
            viewflightinformation.CargoUnit = entity.CargoUnit;
            viewflightinformation.CargoWeight = entity.CargoWeight;
            viewflightinformation.PaxChild = entity.PaxChild;
            viewflightinformation.PaxInfant = entity.PaxInfant;
            viewflightinformation.PaxAdult = entity.PaxAdult;
            viewflightinformation.FuelArrival = entity.FuelArrival;
            viewflightinformation.FuelDeparture = entity.FuelDeparture;
            viewflightinformation.FuelActual = entity.FuelActual;
            viewflightinformation.FuelPlanned = entity.FuelPlanned;
            viewflightinformation.GWLand = entity.GWLand;
            viewflightinformation.GWTO = entity.GWTO;
            viewflightinformation.BlockM = entity.BlockM;
            viewflightinformation.BlockH = entity.BlockH;
            viewflightinformation.FlightH = entity.FlightH;
            viewflightinformation.FlightM = entity.FlightM;
            viewflightinformation.ChocksIn = entity.ChocksIn == null ? null : (Nullable<DateTime>)((DateTime)entity.ChocksIn).AddMinutes(tzoffset);
            viewflightinformation.Landing = entity.Landing == null ? null : (Nullable<DateTime>)((DateTime)entity.Landing).AddMinutes(tzoffset); ;
            viewflightinformation.Takeoff = entity.Takeoff == null ? null : (Nullable<DateTime>)((DateTime)entity.Takeoff).AddMinutes(tzoffset);
            viewflightinformation.ChocksOut = entity.ChocksOut == null ? null : (Nullable<DateTime>)((DateTime)entity.ChocksOut).AddMinutes(tzoffset);
            viewflightinformation.STD = entity.STD == null ? null : (Nullable<DateTime>)((DateTime)entity.STD).AddMinutes(tzoffset);
            viewflightinformation.STA = entity.STA == null ? null : (Nullable<DateTime>)((DateTime)entity.STA).AddMinutes(tzoffset);
            viewflightinformation.FlightStatusID = (int)entity.FlightStatusID;
            viewflightinformation.RegisterID = entity.RegisterID;
            viewflightinformation.FlightTypeID = entity.FlightTypeID;
            viewflightinformation.TypeId = entity.TypeId;
            viewflightinformation.AirlineOperatorsID = entity.AirlineOperatorsID;
            viewflightinformation.FlightNumber = entity.FlightNumber;
            viewflightinformation.FromAirport = entity.FromAirport;
            viewflightinformation.ToAirport = entity.ToAirport;
            viewflightinformation.STAPlanned = entity.STAPlanned == null ? null : (Nullable<DateTime>)((DateTime)entity.STAPlanned).AddMinutes(tzoffset);
            viewflightinformation.STDPlanned = entity.STDPlanned == null ? null : (Nullable<DateTime>)((DateTime)entity.STDPlanned).AddMinutes(tzoffset);
            viewflightinformation.FlightHPlanned = entity.FlightHPlanned;
            viewflightinformation.FlightMPlanned = entity.FlightMPlanned;
            viewflightinformation.FlightPlan = entity.FlightPlan;
            viewflightinformation.CustomerId = entity.CustomerId;
            viewflightinformation.IsActive = entity.IsActive;
            viewflightinformation.DateActive = entity.DateActive;
            viewflightinformation.FromAirportName = entity.FromAirportName;
            viewflightinformation.FromAirportIATA = entity.FromAirportIATA;
            // viewflightinformation.FromAirportCityId = entity.FromAirportCityId;
            viewflightinformation.ToAirportName = entity.ToAirportName;
            viewflightinformation.ToAirportIATA = entity.ToAirportIATA;
            // viewflightinformation.ToAirportCityId = entity.ToAirportCityId;
            // viewflightinformation.FromAirportCity = entity.FromAirportCity;
            // viewflightinformation.ToAirportCity = entity.ToAirportCity;
            viewflightinformation.AircraftType = entity.AircraftType;
            viewflightinformation.Register = entity.Register;
            viewflightinformation.MSN = entity.MSN;
            viewflightinformation.FlightStatus = entity.FlightStatus;
            viewflightinformation.FlightStatusBgColor = entity.FlightStatusBgColor;
            viewflightinformation.FlightStatusColor = entity.FlightStatusColor;
            viewflightinformation.FlightStatusClass = entity.FlightStatusClass;
            viewflightinformation.from = entity.from;
            viewflightinformation.to = entity.to;
            viewflightinformation.notes = entity.notes;
            viewflightinformation.status = entity.status;
            viewflightinformation.progress = entity.progress;
            viewflightinformation.taskName = entity.taskName;
            viewflightinformation.startDate = entity.startDate == null ? null : (Nullable<DateTime>)((DateTime)entity.startDate).AddMinutes(tzoffset);
            viewflightinformation.duration = entity.duration;
            viewflightinformation.taskId = entity.taskId;
            viewflightinformation.FlightGroupID = entity.FlightGroupID;
            viewflightinformation.PlanId = entity.PlanId;
            viewflightinformation.ManufacturerId = entity.ManufacturerId;
            viewflightinformation.Manufacturer = entity.Manufacturer;
            //viewflightinformation.ToCountry = entity.ToCountry;
            //viewflightinformation.ToSortName = entity.ToSortName;
            //viewflightinformation.ToCity = entity.ToCity;
            viewflightinformation.FromSortName = entity.FromSortName;
            viewflightinformation.FromContry = entity.FromContry;
            //viewflightinformation.FromCity = entity.FromCity;
            viewflightinformation.FromLatitude = entity.FromLatitude;
            viewflightinformation.FromLongitude = entity.FromLongitude;
            viewflightinformation.ToLatitude = entity.ToLatitude;
            viewflightinformation.ToLongitude = entity.ToLongitude;
            viewflightinformation.CargoCount = entity.CargoCount;
            viewflightinformation.BaggageWeight = entity.BaggageWeight;
            viewflightinformation.FuelUnitID = entity.FuelUnitID;
            viewflightinformation.ArrivalRemark = entity.ArrivalRemark;
            viewflightinformation.DepartureRemark = entity.DepartureRemark;
            viewflightinformation.TotalSeat = entity.TotalSeat;
            viewflightinformation.EstimatedDelay = entity.EstimatedDelay;
            viewflightinformation.PaxOver = entity.PaxOver;
            viewflightinformation.TotalPax = entity.TotalPax;
            viewflightinformation.FuelUnit = entity.FuelUnit;
            viewflightinformation.DateStatus = entity.DateStatus == null ? null : (Nullable<DateTime>)((DateTime)entity.DateStatus).AddMinutes(tzoffset);
            viewflightinformation.FlightStatusUserId = entity.FlightStatusUserId;
            viewflightinformation.CancelDate = entity.CancelDate;
            viewflightinformation.CancelReasonId = entity.CancelReasonId;
            viewflightinformation.CancelReason = entity.CancelReason;
            viewflightinformation.CancelRemark = entity.CancelRemark;



            viewflightinformation.RampDate = entity.RampDate == null ? null : (Nullable<DateTime>)((DateTime)entity.RampDate).AddMinutes(tzoffset);
            viewflightinformation.RampReasonId = entity.RampReasonId;
            viewflightinformation.RampReason = entity.RampReason;
            viewflightinformation.RampRemark = entity.RampRemark;

            viewflightinformation.RedirectDate = entity.RedirectDate;
            viewflightinformation.RedirectReasonId = entity.RedirectReasonId;
            viewflightinformation.RedirectReason = entity.RedirectReason;
            viewflightinformation.RedirectRemark = entity.RedirectRemark;
            viewflightinformation.OSTA = entity.OSTA;
            viewflightinformation.OToAirportIATA = entity.OToAirportIATA;
            viewflightinformation.OToAirportId = entity.OToAirportId;

            return viewflightinformation;
        }
    }

}