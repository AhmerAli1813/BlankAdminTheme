using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salomi.Model.EntityModel.Data
{
    public class SurveyDetailsRequest
    {
        public SurveyDetailsRequest()
        {
            this.NewInduction = new NewInductionDetails();    
            this.ContractorInduction = new ContractorInductionDetails();
            this.SecurityInduction = new SecurityInductionDetails();
            this.Checklist = new ChecklistDetails();    
            this.Assesment = new AssesmentDetails();

        }
        public List<string> Image { get; set; }
        public NewInductionDetails NewInduction { get; set; }
        public ContractorInductionDetails ContractorInduction { get; set; }
        public SecurityInductionDetails SecurityInduction { get; set; }
        public ChecklistDetails Checklist { get; set; }
        public AssesmentDetails Assesment { get; set; }

    }

    public class ChecklistDetails
    {
        public string InspectedBy { get; set; }
        public DateTime DateTime { get; set; }
        public string Comments { get; set; }
        public string ConductedBy { get; set; }
    }
    public class AssesmentDetails
    {
        public string InspectedBy { get; set; }
        public DateTime DateTime { get; set; }
        public string Comments { get; set; }
        public string Position { get; set; }
        public string QuaryCraneNo { get; set; }
        public List<ExampleImage> Examples { get; set; }
    }


 
    public class Questions
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
    public class ExampleImage
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
    }
    public class VehicleIdentificationDetails
    {
        public string Year { get; set; }
        public string Model { get; set; }
        public string Make { get; set; }
        public string Mileage { get; set; }
    }

   
    public class CategoryRemarks
    {
        public int Id { get; set; }
        public string Remarks { get; set; }
    }


    public class SecurityInductionDetails
    {
        public string Subject { get; set; }
        public DateTime Date { get; set; }
        public string Venue { get; set; }
        public string Inductedby { get; set; }
    }

    public class ContractorInductionDetails
    {
        public string CompanyName { get; set; }
        public string ContactPerson { get; set; }
        public string ContactPhoneNo { get; set; }
        public string Nationality { get; set; }
        public string SupervisorName { get; set; }

        public string Inductedby { get; set; }
        public DateTime StartDate { get; set; }

        public string Duration { get; set; }
        public string Typeofwork { get; set; }
        public string DPWProjectIncharge { get; set; }
    }

    public class NewInductionDetails
    {
        public string InductieeName { get; set; }
        public string Position { get; set; }
        public string Id { get; set; }
        public string Nationality { get; set; }
        public string SupervisorName { get; set; }

        public string  Description { get; set; }

    }
}
