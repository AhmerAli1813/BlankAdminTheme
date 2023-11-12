using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salomi.Model.EntityModel.Data
{
    public class SurveyResponseExtraDetails
    {
        public SurveyResponseExtraDetails()
        {
            this.EmployeeDetails = new List<EmployeeDetails>();
            this.ContractorDetails = new List<ContractorIDetails>();

        }
        public List<EmployeeDetails> EmployeeDetails { get; set; }
        public List<ContractorIDetails> ContractorDetails { get; set; }
    }

    public class EmployeeDetails
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public string Department { get; set; }
    }

    public class ContractorIDetails
    {
        public string IqamaNo { get; set; }
        public string Name { get; set; }
        public string NameofCompany { get; set; }
        public string SiteLocation { get; set; }
    }
}
