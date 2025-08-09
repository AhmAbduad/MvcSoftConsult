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
        public int PageSize = 6;

        [HttpGet]
        public ActionResult Index(EmployeeProfileDropdownViewModel model)
        {

            Session["PageNumber"] = 1;

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


        //[HttpPost]
        //public ActionResult Save(EmployeeProfileDropdownViewModel model)
        //{
        //    string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        //    using (SqlConnection conn = new SqlConnection(connectionString))
        //    {
        //        using (SqlCommand cmd = new SqlCommand("stpEmp_Employee", conn))
        //        {
        //            cmd.CommandType = CommandType.StoredProcedure;

        //            // Add parameters
        //            cmd.Parameters.AddWithValue("@nIsUpdate", 0); // 0 for insert
        //            //cmd.Parameters.AddWithValue("@nS_NO", DBNull.Value);
        //            cmd.Parameters.AddWithValue("@sS_ID", model.E_SID ?? (object)DBNull.Value);
        //            cmd.Parameters.AddWithValue("@sS_Name", model.E_Name ?? (object)DBNull.Value);
        //            cmd.Parameters.AddWithValue("@sS_FName", model.E_FName ?? (object)DBNull.Value);
        //            cmd.Parameters.AddWithValue("@sS_IdNo", model.E_IdNo ?? (object)DBNull.Value);
        //            cmd.Parameters.AddWithValue("@dS_DOB", model.E_DOB ?? (object)DBNull.Value);
        //            cmd.Parameters.AddWithValue("@dS_IdExpire", model.E_IDExpire ?? (object)DBNull.Value);
        //            cmd.Parameters.AddWithValue("@sS_Desig", model.E_Desig ?? (object)DBNull.Value);
        //            cmd.Parameters.AddWithValue("@sS_Depart", model.E_Depart ?? (object)DBNull.Value);
        //            cmd.Parameters.AddWithValue("@sS_Join", model.E_Join ?? (object)DBNull.Value);
        //            cmd.Parameters.AddWithValue("@sS_Reason", model.E_Reason ?? (object)DBNull.Value);
        //            cmd.Parameters.AddWithValue("@dS_CD", model.E_CD ?? (object)DBNull.Value);
        //            cmd.Parameters.AddWithValue("@dS_DL", model.E_DL ?? (object)DBNull.Value);

        //            // Foreign keys
        //            cmd.Parameters.AddWithValue("@nEmp_DesigID", model.Emp_DesigID ?? (object)DBNull.Value);
        //            cmd.Parameters.AddWithValue("@nEmp_DptID", model.Emp_DptID ?? (object)DBNull.Value);
        //            cmd.Parameters.AddWithValue("@nEmp_StatusID", model.Emp_StatusID ?? (object)DBNull.Value);
        //            cmd.Parameters.AddWithValue("@nEmp_LocID", model.Emp_LocID ?? (object)DBNull.Value);
        //            cmd.Parameters.AddWithValue("@nEmp_SurID", model.Emp_SurID ?? (object)DBNull.Value);
        //            cmd.Parameters.AddWithValue("@nEmp_GenderID", model.Emp_GenderID ?? (object)DBNull.Value);
        //            cmd.Parameters.AddWithValue("@nEmp_GradeID", model.Emp_GradeID ?? (object)DBNull.Value);
        //            cmd.Parameters.AddWithValue("@nEmp_JobStatusID", model.Emp_JobStatusID ?? (object)DBNull.Value);
        //            cmd.Parameters.AddWithValue("@nEmp_LeaveID", model.Emp_LeaveID ?? (object)DBNull.Value);
        //            cmd.Parameters.AddWithValue("@nEmp_NationID", model.Emp_NationID ?? (object)DBNull.Value);
        //            cmd.Parameters.AddWithValue("@nEmp_ReligionID", model.Emp_ReligionID ?? (object)DBNull.Value);
        //            cmd.Parameters.AddWithValue("@nEmp_SecID", model.Emp_SecID ?? (object)DBNull.Value);

        //            // Optional Paging Parameters
        //            cmd.Parameters.AddWithValue("@PageNumber", DBNull.Value);
        //            cmd.Parameters.AddWithValue("@PageSize", DBNull.Value);

        //            conn.Open();
        //            cmd.ExecuteNonQuery();
        //        }
        //    }

        //    // Redirect or return view
        //    return RedirectToAction("Index");
        //}

        public ActionResult Refresh()
        {
            return RedirectToAction("Index");
        }

        public ActionResult ShowList()
        {

            int PageNumber = Convert.ToInt32(Session["PageNumber"] ?? 1);

            List<EmployeeProfileDropdownViewModel> employees = new List<EmployeeProfileDropdownViewModel>();

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("stpEmp_Employee", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nIsUpdate", 6);
                    cmd.Parameters.AddWithValue("@PageNumber", PageNumber);
                    cmd.Parameters.AddWithValue("@PageSize", PageSize);

                    // dates and text
                    cmd.Parameters.AddWithValue("@nS_NO", DBNull.Value);
                    cmd.Parameters.AddWithValue("@sS_ID", DBNull.Value);
                    cmd.Parameters.AddWithValue("@sS_Name", DBNull.Value);
                    cmd.Parameters.AddWithValue("@sS_FName", DBNull.Value);
                    cmd.Parameters.AddWithValue("@sS_IdNo", DBNull.Value);
                    cmd.Parameters.AddWithValue("@dS_DOB", DBNull.Value);
                    cmd.Parameters.AddWithValue("@dS_IdExpire", DBNull.Value);
                    cmd.Parameters.AddWithValue("@sS_Desig", DBNull.Value);
                    cmd.Parameters.AddWithValue("@sS_Depart", DBNull.Value);
                    cmd.Parameters.AddWithValue("@sS_Join", DBNull.Value);
                    cmd.Parameters.AddWithValue("@sS_Reason", DBNull.Value);
                    cmd.Parameters.AddWithValue("@dS_CD", DBNull.Value);
                    cmd.Parameters.AddWithValue("@dS_DL", DBNull.Value);


                    // Add NULL for dropdown values
                    cmd.Parameters.AddWithValue("@nEmp_DesigID", DBNull.Value);
                    cmd.Parameters.AddWithValue("@nEmp_DptID", DBNull.Value);
                    cmd.Parameters.AddWithValue("@nEmp_StatusID", DBNull.Value);
                    cmd.Parameters.AddWithValue("@nEmp_LocID", DBNull.Value);
                    cmd.Parameters.AddWithValue("@nEmp_SurID", DBNull.Value);
                    cmd.Parameters.AddWithValue("@nEmp_GenderID", DBNull.Value);
                    cmd.Parameters.AddWithValue("@nEmp_GradeID", DBNull.Value);
                    cmd.Parameters.AddWithValue("@nEmp_JobStatusID", DBNull.Value);
                    cmd.Parameters.AddWithValue("@nEmp_LeaveID", DBNull.Value);
                    cmd.Parameters.AddWithValue("@nEmp_NationID", DBNull.Value);
                    cmd.Parameters.AddWithValue("@nEmp_ReligionID", DBNull.Value);
                    cmd.Parameters.AddWithValue("@nEmp_SecID", DBNull.Value);


                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        employees.Add(new EmployeeProfileDropdownViewModel
                        {
                            E_SNO = Convert.ToInt32(reader["E_SNO"]),
                            E_SID = reader["E_SID"].ToString(),
                            E_Name = reader["E_Name"].ToString(),
                            E_FName = reader["E_FName"].ToString(),
                            LocationasString = reader["Location"].ToString(),
                            JobStatusasString = reader["JobStatus"].ToString()

                        }); ;
                    }




                    reader.NextResult();
                    if (reader.Read())
                    {
                        int totalRecords = Convert.ToInt32(reader["TotalRecords"]);
                        Session["TotalRecords"] = totalRecords;
                        // lblPageInfo.Text = $"Showing {(pageNumber - 1) * PageSize + 1} to {Math.Min(pageNumber * PageSize, totalRecords)} of {totalRecords}";
                    }

                }
            }
            return View(employees);
        }

        [HttpGet]
        public ActionResult FirstPage()
        {
            Session["PageNumber"] = 1;
            return RedirectToAction("ShowList");
        }

        [HttpGet]
        public ActionResult PreviousPage(object sender, EventArgs e)
        {
            int pageNumber = Convert.ToInt32(Session["PageNumber"]);
            if (pageNumber > 1)
            {
                Session["PageNumber"] = pageNumber - 1;
                return RedirectToAction("ShowList");
            }

            return RedirectToAction("ShowList");
        }

        [HttpGet]
        public ActionResult NextPage(object sender, EventArgs e)
        {
            int pageNumber = Convert.ToInt32(Session["PageNumber"]);
            int totalRecords = Convert.ToInt32(Session["TotalRecords"]);
            int maxPage = (int)Math.Ceiling((double)totalRecords / PageSize);

            if (pageNumber < maxPage)
            {
                Session["PageNumber"] = pageNumber + 1;
                return RedirectToAction("ShowList");
            }
            return RedirectToAction("ShowList");
        }

        [HttpGet]
        public ActionResult LastPage(object sender, EventArgs e)
        {
            int totalRecords = Convert.ToInt32(Session["TotalRecords"]);
            Session["PageNumber"] = (int)Math.Ceiling((double)totalRecords / PageSize);
            return RedirectToAction("ShowList");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            EmployeeProfileDropdownViewModel model = new EmployeeProfileDropdownViewModel();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                con.Open();

                // Load dropdowns
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

                // Load employee data
                using (SqlCommand cmd = new SqlCommand("stpEmp_Employee", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nIsUpdate", 4); // 4 = Get By ID
                    cmd.Parameters.AddWithValue("@nS_NO", id);

                    // All other parameters must be passed as DBNull
                    // Add others as needed
                    cmd.Parameters.AddWithValue("@PageNumber", DBNull.Value);
                    cmd.Parameters.AddWithValue("@PageSize", DBNull.Value);

                    SqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr.Read())
                    {
                        // text boxes and dates

                        model.E_SNO = Convert.ToInt32(rdr["E_SNO"]);
                        model.E_SID = rdr["E_SID"].ToString();
                        model.E_Name = rdr["E_Name"].ToString();
                        model.E_FName = rdr["E_FName"].ToString();
                        model.E_IdNo = rdr["E_IdNo"].ToString();
                        model.E_DOB = rdr["E_DOB"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(rdr["E_DOB"]) : null;
                        model.E_IDExpire = rdr["E_IDExpire"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(rdr["E_IDExpire"]) : null;
                        model.E_Desig = rdr["E_Desig"].ToString();
                        model.E_Depart = rdr["E_Depart"].ToString();
                        model.E_Join = rdr["E_Join"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(rdr["E_Join"]) : null;
                        model.E_Reason = rdr["E_Reason"].ToString();
                        model.E_CD = rdr["E_CD"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(rdr["E_CD"]) : null;
                        model.E_DL = rdr["E_DL"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(rdr["E_DL"]) : null;
                        // drop downs

                        model.Emp_DesigID = Convert.ToInt32(rdr["Emp_DesigID"]);
                        model.Emp_DptID = Convert.ToInt32(rdr["Emp_DptID"]);
                        model.Emp_StatusID = Convert.ToInt32(rdr["Emp_StatusID"]);
                        model.Emp_LocID = Convert.ToInt32(rdr["Emp_LocID"]);
                        model.Emp_SurID = Convert.ToInt32(rdr["Emp_SurID"]);
                        model.Emp_GenderID = Convert.ToInt32(rdr["Emp_GenderID"]);
                        model.Emp_GradeID = Convert.ToInt32(rdr["Emp_GradeID"]);
                        model.Emp_JobStatusID = Convert.ToInt32(rdr["Emp_JobStatusID"]);
                        model.Emp_LeaveID = Convert.ToInt32(rdr["Emp_LeaveID"]);
                        model.Emp_NationID = Convert.ToInt32(rdr["Emp_NationID"]);
                        model.Emp_ReligionID = Convert.ToInt32(rdr["Emp_ReligionID"]);
                        model.Emp_SecID = Convert.ToInt32(rdr["Emp_SecID"]);

                    }
                }
            }

            return View("Index", model); // Reuse the same form
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

                    bool isUpdate = model.E_SNO > 0;

                    cmd.Parameters.AddWithValue("@nIsUpdate", isUpdate ? 1 : 0); // 0 = insert, 1 = update
                    cmd.Parameters.AddWithValue("@nS_NO", isUpdate ? (object)model.E_SNO : DBNull.Value);

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

                    // other fields...

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


                    cmd.Parameters.AddWithValue("@PageNumber", DBNull.Value);
                    cmd.Parameters.AddWithValue("@PageSize", DBNull.Value);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            EmployeeProfileDropdownViewModel dummymodel = new EmployeeProfileDropdownViewModel();

            return RedirectToAction("Index", dummymodel);
        }


        [HttpGet]
        public ActionResult Delete(int id)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("stpEmp_Employee", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nIsUpdate", 2); // 2 = delete
                    cmd.Parameters.AddWithValue("@nS_NO", id);

                    // All other parameters must be passed as DBNull
                    cmd.Parameters.AddWithValue("@PageNumber", DBNull.Value);
                    cmd.Parameters.AddWithValue("@PageSize", DBNull.Value);
                    // Add remaining parameters as DBNull
                    cmd.Parameters.AddWithValue("@sS_ID", DBNull.Value);
                    cmd.Parameters.AddWithValue("@sS_Name", DBNull.Value);
                    // etc...

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            return RedirectToAction("ShowList");
        }


    }
}