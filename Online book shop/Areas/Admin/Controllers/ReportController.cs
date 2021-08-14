using OfficeOpenXml;
using Online_book_shop.Handlers.Business;
using Online_book_shop.Handlers.Database;
using Online_book_shop.Models;
using Online_book_shop.Models.ViewModel.Report;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Online_book_shop.Areas.Admin.Controllers
{
    public class ReportController : Controller
    {
        // GET: Admin/Report
        public ActionResult Index()
        {
            return View();
        }
        //public ActionResult GetReport()
        //{
        //    //Create the data set and table
        //    DataSet ds = new DataSet("New_DataSet");
        //    DataTable dt = new DataTable("New_DataTable");
        //    List <DeliveryReport> orders= BusinessHandlerReport.GetTodayHandOverItems();
        //    dt = orders != null ? BusinessHandlerReport.ToDataTable(orders) : null;
        //    ds.Tables.Add(dt);
        //    //Here's the easy part. Create the Excel worksheet from the data set
        //    ExcelLibrary.DataSetHelper.CreateWorkbook("C:\\Users\\HI-TEC\\Desktop\\Med\\MyExcelFile.xls", ds);
        //    return RedirectToAction("MyExcelFile.xls");
        //}
        public void DownloadExcell(int deliveryStatus, int OrderType, string StartDate, string EndDate)
        {
            DataTable dt = new DataTable("New_DataTable");
            List<Order> List = BusinessHandlerOrder.GetFiltered(deliveryStatus, OrderType, StartDate, EndDate);
            //dt = List != null ? BusinessHandlerReport.ToDataTable(BusinessHandlerReport.GetWebOrders(List)) : null;
            string fileName = string.Format("status_{0}_type_{1}_start{2}_end_{3}", deliveryStatus.ToString(), OrderType.ToString(), StartDate.ToString(),EndDate.ToString());
            DownloadExcel(BusinessHandlerReport.GetWebOrders(List), fileName);
            //CreateExcell(dt);
            //return "";
        }
        public ActionResult AdvancedSearch()
        {
            return View();
        }
        public string CreateExcell(DataTable dt)
        {
            DataSet ds = new DataSet("New_DataSet");
            string fileName = string.Format("{0}.xls", (new Guid().ToString()));
            ds.Tables.Add(dt);
            ExcelLibrary.DataSetHelper.CreateWorkbook("C:\\Users\\HI-TEC\\Desktop\\Med\\"+ fileName, ds);
            return fileName;
        }
        public void DownloadExcel(List<WebOrder> collection,string fileName="")
        {

            ExcelPackage Ep = new ExcelPackage();
            ExcelWorksheet Sheet = Ep.Workbook.Worksheets.Add("Report");
            Sheet.Cells["A1"].Value = "Date";
            Sheet.Cells["B1"].Value = "Waybill ID";
            Sheet.Cells["C1"].Value = "Order Number";
            Sheet.Cells["D1"].Value = "Invoice Number";
            Sheet.Cells["E1"].Value = "Receiver Name";
            Sheet.Cells["F1"].Value = "Delivery Address";
            Sheet.Cells["G1"].Value = "District Name";
            Sheet.Cells["H1"].Value = "Receiver Phone";
            Sheet.Cells["I1"].Value = "Email";
            Sheet.Cells["J1"].Value = "COD";
            Sheet.Cells["K1"].Value = "Description";
            Sheet.Cells["L1"].Value = "Payer's Name";
            Sheet.Cells["M1"].Value = "Billing Address";
            Sheet.Cells["N1"].Value = "Payer's Phone";
            Sheet.Cells["O1"].Value = "Special Note";
            Sheet.Cells["P1"].Value = "Payment Method";
            Sheet.Cells["Q1"].Value = "Payment Status";
            Sheet.Cells["R1"].Value = "Delivery Method";
            Sheet.Cells["S1"].Value = "Total";

            int row = 2;
            foreach (var item in collection)
            {

                Sheet.Cells[string.Format("A{0}", row)].Value = item.Date;
                Sheet.Cells[string.Format("B{0}", row)].Value = item.WayBillId;
                Sheet.Cells[string.Format("C{0}", row)].Value = item.OrderNumber;
                Sheet.Cells[string.Format("D{0}", row)].Value = item.InvoiceNumber;
                Sheet.Cells[string.Format("E{0}", row)].Value = item.ReceiverName;
                Sheet.Cells[string.Format("F{0}", row)].Value = item.DeliveryAddress;
                Sheet.Cells[string.Format("G{0}", row)].Value = item.DistrictName;
                Sheet.Cells[string.Format("H{0}", row)].Value = item.ReceiverPhone;
                Sheet.Cells[string.Format("I{0}", row)].Value = item.Email;
                Sheet.Cells[string.Format("J{0}", row)].Value = item.COD;
                Sheet.Cells[string.Format("K{0}", row)].Value = item.Description;
                Sheet.Cells[string.Format("L{0}", row)].Value = item.PayerName;
                Sheet.Cells[string.Format("M{0}", row)].Value = item.PayerAddress; 
                Sheet.Cells[string.Format("N{0}", row)].Value = item.PayerPhone;
                Sheet.Cells[string.Format("O{0}", row)].Value = item.SpecialNote;
                Sheet.Cells[string.Format("P{0}", row)].Value = item.PaymentMethod;
                Sheet.Cells[string.Format("Q{0}", row)].Value = item.PaymentStatus;
                Sheet.Cells[string.Format("R{0}", row)].Value = item.DeliveryMethod;
                Sheet.Cells[string.Format("S{0}", row)].Value = item.Total;
                
                row++;
            }


            Sheet.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment: filename=" + fileName+".xlsx");
            Response.BinaryWrite(Ep.GetAsByteArray());
            Response.End();
        }

    }
}