using Salomi.Model.Features.Tickets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salomi.Model.EntityModel.Data
{
    public class TicketExtraDetails
    {
        public TicketExtraDetails()
        {
            this.AccidentData = new AccidentDetails();
            this.ContractorData = new ContractorDetails();
            this.VoilationData = new VoilationDetails();
            this.AccidentTypeData = new AccidentTypeData();
            this.AuditData = new AuditData();
            this.ToolBox = new ToolBox();
        }
        public AccidentDetails AccidentData { get; set; } 
        public ContractorDetails ContractorData { get; set; }
        public VoilationDetails VoilationData { get; set; }
        public AccidentTypeData AccidentTypeData { get; set; }
        public AuditData AuditData { get; set; }

        public ToolBox ToolBox { get; set; }
    }
    public class AccidentDetails
    {
        public string VictimName { get; set; }
        public List<AccidentEmployee> EmployeeData { get; set; }
        //public int Age { get; set; }        

        //public string Work { get; set; }
    }

    public class ToolBox
    {

        public DateTime DateTime { get; set; }
        public List<EmployeesData> Data { get; set; }
        public List<string> Images { get; set; }
        public string Cordinator { get; set; }
        public string ToolBoxName { get; set; }
        public string imageExternal { get; set; }
        public string IsShiftLabourAtt { get; set; }
    }
    public class EmployeesData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Nationality { get; set; }

        public string Department { get; set; }

        public string Designation { get; set; }
    }
    public class AuditData
    {
        public List<Q_Data> Operation { get; set; }
    }
    public class VoilationDetails
    {
        public string VoilationSource { get; set; }
        public string EmployeeName { get; set; }
        public int Age { get; set; }
        
        public string Site { get; set; }
        public int EmployeeNumber { get; set; }
    }
    public class ContractorDetails
    {
        public int Id { get; set; }
        public string ContractorName { get; set; }
    }
    public class AccidentEmployee
    {
        public string EMPID { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public string IQA { get; set; }
        public string Contact { get; set; }
        public string ShiftStart { get; set; }
        public string ShiftEnd { get; set; }
        public string Remarks { get; set; }
        public string Injury { get; set; }
    }
    public class AccidentDriver
    {
        public string EMPID { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public string FleetNo { get; set; }
        public string TPL { get; set; }
        public string OrgLiable { get; set; }
        public string VehicleType { get; set; }
        public string PRA { get; set; }
        public string MedicalReportAvailable { get; set; }
        public string IspersonIsolated { get; set; }
    }
    public class AccidentContractor
    {
        public string StaffNo { get; set; }
        public string Name { get; set; }
        public string CompanyName { get; set; }
        public string Contact { get; set; }
        public string Nationality { get; set; }
        public string Remarks { get; set; }
        public string Injury { get; set; }
    }
    public class OtherPersonData
    {
        public string StaffNo { get; set; }
        public string Name { get; set; }
        public string CompanyName { get; set; }
        public string Contact { get; set; }
        public string Nationality { get; set; }
        public string Remarks { get; set; }
        public string Injury { get; set; }
    }
    public class AccidentTypeData
    {
        public string Accident { get; set; }
        public string ExactLocation { get; set; }
        public string ActualSeverity { get; set; }
        public string PotentialSeverity { get; set; }
        public string ProcessAction { get; set; }
        public List<AccidentEmployee> EmployeeData { get; set; }
        public List<AccidentDriver> TransportRelatedData { get; set; }
        public List<AccidentContractor> ContractorData { get; set; }
        public List<OtherPersonData> OtherPersonData { get; set; }
        public string Remarks { get; set; }
        public string TechnicalInCharge { get; set; }
        public string OPSShiftInCharge { get; set; }
        public string HSSEOfficer { get; set; }
        public DateTime? IncidentDate { get; set; }
        public string PRA { get; set; }
        public string RI { get; set; }
        public string IDetails { get; set; }
        public string IAction { get; set; }
        public string Witness1 { get; set; }
        public string Witness2 { get; set; }
        public string W1Address { get; set; }
        public string W2Address { get; set; }
        public string PI { get; set; }
        public string HC { get; set; }
        public string ECI { get; set; }
    }
}

