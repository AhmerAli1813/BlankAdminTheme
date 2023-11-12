using Salomi.Model.EntityModel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salomi.Model.EntityModel.Models
{
    public class Survey
    {
        public List<SurveyCategory> Categories { get; set; }
        public SurveySetting Setting { get; set; }
        public SurveyType SurveyType { get; set; }
    }
}
