using MvcSoftConsult.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcSoftConsult.Controllers
{
    public class EmployeeProfileController : Controller
    {   
        [HttpGet]
        public ActionResult Index()
        {
            EmployeeProfileDropdownViewModel model = new EmployeeProfileDropdownViewModel();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                con.Open();

                model.Designations = GetDropdownItems(con, "SELECT Desig_ID, Desig_Name FROM tblEmpDesig");
                model.Departments = GetDropdownItems(con, "SELECT DptLev_ID, DptLev_Desc FROM tblEmpDptLev");
                model.Statuses = GetDropdownItems(con, "SELECT Emp_Status_ID, Emp_Status_Name FROM tblEmpStatus");
                model.Locations = GetDropdownItems(con, "SELECT Loc_ID, Loc_Name FROM tblEmpLoc");
                model.Salutations = GetDropdownItems(con, "SELECT Sur_ID, Sur_Title FROM tblEmpSur");
                model.Genders = GetDropdownItems(con, "SELECT Gender_ID, Gender_Name FROM tblEmpGender");
                model.Grades = GetDropdownItems(con, "SELECT Grade_ID, Grade_Name FROM tblEmpGrade");
                model.JobStatuses = GetDropdownItems(con, "SELECT JobStatus_ID, JobStatus_Name FROM tblEmpJobStatus");
                model.Leaves = GetDropdownItems(con, "SELECT Leave_ID, Leave_DESC FROM tblEmpLeave");
                model.Nations = GetDropdownItems(con, "SELECT Nation_ID, Nation_Name FROM tblEmpNation");
                model.Religions = GetDropdownItems(con, "SELECT Religion_ID, Religion_Name FROM tblEmpReligion");
                model.Sections = GetDropdownItems(con, "SELECT Sec_ID, Sec_Name FROM tblEmpSec");
            }

            return View(model);
        }


        private List<SelectListItem> GetDropdownItems(SqlConnection con, string query)
        {
            List<SelectListItem> items = new List<SelectListItem>();

            using (SqlCommand cmd = new SqlCommand(query, con))
            using (SqlDataReader rdr = cmd.ExecuteReader())
            {
                while (rdr.Read())
                {
                    items.Add(new SelectListItem
                    {
                        Value = rdr[0].ToString(),
                        Text = rdr[1].ToString()
                    });
                }
            }

            return items;
        }


        [HttpPost]
        public ActionResult Save(EmployeeProfileDropdownViewModel model)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("stpEmp_Employee", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Add parameters
                    cmd.Parameters.AddWithValue("@nIsUpdate", 0); // 0 for insert
                    //cmd.Parameters.AddWithValue("@nS_NO", DBNull.Value);
                    cmd.Parameters.AddWithValue("@sS_ID", model.E_SID ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@sS_Name", model.E_Name ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@sS_FName", model.E_FName ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@sS_IdNo", model.E_IdNo ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@dS_DOB", model.E_DOB ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@dS_IdExpire", model.E_IDExpire ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@sS_Desig", model.E_Desig ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@sS_Depart", model.E_Depart ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@sS_Join", model.E_Join ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@sS_Reason", model.E_Reason ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@dS_CD", model.E_CD ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@dS_DL", model.E_DL ?? (object)DBNull.Value);

                    // Foreign keys
                    cmd.Parameters.AddWithValue("@nEmp_DesigID", model.Emp_DesigID ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@nEmp_DptID", model.Emp_DptID ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@nEmp_StatusID", model.Emp_StatusID ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@nEmp_LocID", model.Emp_LocID ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@nEmp_SurID", model.Emp_SurID ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@nEmp_GenderID", model.Emp_GenderID ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@nEmp_GradeID", model.Emp_GradeID ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@nEmp_JobStatusID", model.Emp_JobStatusID ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@nEmp_LeaveID", model.Emp_LeaveID ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@nEmp_NationID", model.Emp_NationID ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@nEmp_ReligionID", model.Emp_ReligionID ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@nEmp_SecID", model.Emp_SecID ?? (object)DBNull.Value);

                    // Optional Paging Parameters
                    cmd.Parameters.AddWithValue("@PageNumber", DBNull.Value);
                    cmd.Parameters.AddWithValue("@PageSize", DBNull.Value);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            // Redirect or return view
            return RedirectToAction("Index");
        }

        public ActionResult Refresh()
        {
            return RedirectToAction("Index");
        }

        public ActionResult List()
        {
            return RedirectToAction("ShowList");
        }


    }
}