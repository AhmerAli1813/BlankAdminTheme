﻿using System.ComponentModel.DataAnnotations;
using System.Web;

namespace DPWVessel.Web.Core.Model
{
    public class ImportExcel
    {
        [Required(ErrorMessage = "Please select file")]
        public HttpPostedFileBase file { get; set; }
        [Required(ErrorMessage = "Please select LocationId")]
        public int[] LocationId { get; set; }
        [Required(ErrorMessage = "Please select OrganizationId")]
        public int OrganizationId { get; set; }
        [Required(ErrorMessage = "Please select DepartmentId")]
        public int DepartmentId { get; set; }
        [Required(ErrorMessage = "Please select UserType")]
        public int UserType { get; set; }
    }
}