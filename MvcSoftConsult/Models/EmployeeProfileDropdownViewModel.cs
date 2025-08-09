using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcSoftConsult.Models
{
    public class EmployeeProfileDropdownViewModel
    {
        public int E_SNO {  get; set; }

        public string E_SID { get; set; }

        public string E_Name { get; set; }

        public string E_FName { get; set; }

        public string E_IdNo { get; set; }

        public DateTime? E_DOB { get; set; }

        public DateTime? E_IDExpire { get; set; }

        public string E_Desig { get; set; }

        public string E_Depart { get; set; }

        public DateTime? E_Join { get; set; }

        public string E_Reason { get; set; }

        public DateTime? E_CD { get; set; }

        public DateTime? E_DL { get; set; }

        public int? Emp_DesigID { get; set; }
        public int? Emp_DptID { get; set; }
        public int? Emp_StatusID { get; set; }
        public int? Emp_LocID { get; set; }
        public int? Emp_SurID { get; set; }
        public int? Emp_GenderID { get; set; }
        public int? Emp_GradeID { get; set; }
        public int? Emp_JobStatusID { get; set; }
        public int? Emp_LeaveID { get; set; }
        public int? Emp_NationID { get; set; }
        public int? Emp_ReligionID { get; set; }
        public int? Emp_SecID { get; set; }


        public string LocationasString { get; set; }

        public string JobStatusasString { get; set; }



        public IEnumerable<SelectListItem> Designations { get; set; }
        public IEnumerable<SelectListItem> Departments { get; set; }
        public IEnumerable<SelectListItem> Statuses { get; set; }
        public IEnumerable<SelectListItem> Locations { get; set; }
        public IEnumerable<SelectListItem> Salutations { get; set; }
        public IEnumerable<SelectListItem> Genders { get; set; }
        public IEnumerable<SelectListItem> Grades { get; set; }
        public IEnumerable<SelectListItem> JobStatuses { get; set; }
        public IEnumerable<SelectListItem> Leaves { get; set; }
        public IEnumerable<SelectListItem> Nations { get; set; }
        public IEnumerable<SelectListItem> Religions { get; set; }
        public IEnumerable<SelectListItem> Sections { get; set; }
    }
}