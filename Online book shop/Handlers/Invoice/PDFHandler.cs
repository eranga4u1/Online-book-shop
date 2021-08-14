using iTextSharp.text;
using iTextSharp.text.html;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using Online_book_shop.Handlers.Business;
using Online_book_shop.Models;
using Online_book_shop.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Online_book_shop.Handlers.Invoice
{
    public static class PDFHandler
    {
        public static Paragraph GetSectionHeader(string content)
        {
            BaseFont bfTimes = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
            iTextSharp.text.Font times = new iTextSharp.text.Font(bfTimes, 8, 0, new BaseColor(215,25,35));
            Paragraph paragraph = new Paragraph(content, times);
            //paragraph.IndentationLeft = 20f;

            paragraph.SpacingBefore = 10f;
            paragraph.SpacingAfter = 5f;

            return paragraph;
        }
        public static Paragraph GetTableHeader(string content)
        {
            BaseFont bfTimes = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, false);
            iTextSharp.text.Font times = new iTextSharp.text.Font(bfTimes, 6);
            return new Paragraph(content, times);
        }
        public static Paragraph GetNormalText(string content)
        {
            BaseFont bfTimes = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
            iTextSharp.text.Font times = new iTextSharp.text.Font(bfTimes, 6);
            return new Paragraph(content, times);
        }
        public static Paragraph GetNormalLable(string content)
        {
            BaseFont bfTimes = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, false);
            iTextSharp.text.Font times = new iTextSharp.text.Font(bfTimes, 6);
            return new Paragraph(content, times);
        }
        public static Paragraph GetTableContent(string content)
        {
            BaseFont bfTimes = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
            iTextSharp.text.Font times = new iTextSharp.text.Font(bfTimes, 6);
            return new Paragraph(content, times);
        }
        public static Paragraph GetRedTableContent(string content)
        {
            BaseFont bfTimes = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
            iTextSharp.text.Font times = new iTextSharp.text.Font(bfTimes, 6, 0, new BaseColor(250, 7, 7));
            return new Paragraph(content, times);
        }
        public static Paragraph GetConsoleContent(string content)
        {
            BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ITALIC, BaseFont.CP1252, false);
            iTextSharp.text.Font times = new iTextSharp.text.Font(bfTimes, 8);
            return new Paragraph(content, times);
        }
        public static PdfPTable GenaratePdfTable(int numberOfcoulumns, float[] coulumnsWidthArray, string[] coulumnsHeadersArray)
        {

            PdfPTable table = new PdfPTable(numberOfcoulumns);
            table.TotalWidth = 550f;
            table.LockedWidth = true;
            table.SetWidths(coulumnsWidthArray);
            table.HorizontalAlignment = 0;
            table.DefaultCell.BorderWidth = 0.0001f;
            //table.DefaultCell.BorderColor = new BaseColor(0, 150, 17);
            foreach (string header in coulumnsHeadersArray)
            {
                table.AddCell(GetTableHeader(header));
            }
            return table;
        }
        public static PdfPTable PDfCoulumns(int numberOfcoulumns, float[] coulumnsWidthArray)
        {
            PdfPTable table = new PdfPTable(numberOfcoulumns);
            table.TotalWidth = 550f;
            table.LockedWidth = true;
            table.SetWidths(coulumnsWidthArray);
            table.DefaultCell.PaddingLeft = -2;
            table.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            return table;
        }
        public static PdfPTable SetHeaderTheme(PdfWriter writer, Document doc)

        {
            PdfPTable uperTbl = new PdfPTable(1);
            uperTbl.TotalWidth = 800;
            uperTbl.HorizontalAlignment = Element.ALIGN_CENTER;

            PdfPCell cellBtm1 = new PdfPCell(new Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)));
            BaseColor grey = WebColors.GetRGBColor("#8f8f94");
            cellBtm1.BackgroundColor = grey;
            cellBtm1.Border = 0;
            cellBtm1.PaddingLeft = 0;
            uperTbl.AddCell(cellBtm1);

            PdfPCell cellBtm2 = new PdfPCell(new Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)));
            BaseColor green = WebColors.GetRGBColor("#32a852");
            cellBtm2.BackgroundColor = green;
            cellBtm2.Border = 0;
            cellBtm2.PaddingLeft = 0;
            uperTbl.AddCell(cellBtm2);

            uperTbl.WriteSelectedRows(0, 0, 0, doc.TopMargin, writer.DirectContent);
            return uperTbl;
        }
      

        public static double GetRemainingPercentage(double total, double remaining)
        {
            double input = (remaining / total) * 100;
            return Math.Round(input, 2);
        }
        public static bool GenaratePDF(Order order, Cart cart, string pdfLocation = null, string filename = "Invoice.pdf", bool openAfterGenerate = false, string clintLogo = "http://devranga-001-site2.atempurl.com/Content/images/logo.png",bool isPaid=false)
        {
            bool result = false;
            if (!string.IsNullOrEmpty(pdfLocation))
            {
                filename = HttpContext.Current.Server.MapPath(pdfLocation) + "\\" + filename;
            }
            Document document = new Document(PageSize.A4, 20f, 20f, 20f, 20f);
            try
            {
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(filename, FileMode.Create));
                
                if (!document.IsOpen())
                {
                    document.Open();
                }

                //LineSeparator lineTop1 = new LineSeparator(50f, 200f, new BaseColor(215, 25, 35), Element.ALIGN_TOP, 0f);
                //document.Add(lineTop1);

                //document.Add(PDFHandler.GetSectionHeader(""));
                //LineSeparator lineTop2 = new LineSeparator(20f, 200f, new BaseColor(128, 128, 128), Element.ALIGN_BASELINE, 0.1f);
                //document.Add(lineTop2);


                //iTextSharp.text.Image mobileImageClient = null;
                //if (File.Exists("logoTest.png"))
                //{
                //    mobileImageClient = iTextSharp.text.Image.GetInstance("logoTest.png");
                //    mobileImageClient.Alignment = iTextSharp.text.Image.ALIGN_LEFT;
                //    mobileImageClient.ScaleToFit(100, 100);

                //    Chunk cmobImgClient = new Chunk(mobileImageClient, 0, -2);
                //}
                //else
                //{
                //    mobileImageClient = iTextSharp.text.Image.GetInstance(clintLogo);
                //    mobileImageClient.Alignment = iTextSharp.text.Image.ALIGN_LEFT;
                //    mobileImageClient.ScaleToFit(100, 100);

                //    Chunk cmobImgClient = new Chunk(mobileImageClient, 30, -2);
                //}

                iTextSharp.text.Image mobileImage = null;
                string base64 = @"iVBORw0KGgoAAAANSUhEUgAAAGwAAABTCAIAAAD1KHXiAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAyZpVFh0WE1MOmNvbS5hZG9iZS54bXAAAAAAADw/eHBhY2tldCBiZWdpbj0i77u/IiBpZD0iVzVNME1wQ2VoaUh6cmVTek5UY3prYzlkIj8+IDx4OnhtcG1ldGEgeG1sbnM6eD0iYWRvYmU6bnM6bWV0YS8iIHg6eG1wdGs9IkFkb2JlIFhNUCBDb3JlIDUuNi1jMDY3IDc5LjE1Nzc0NywgMjAxNS8wMy8zMC0yMzo0MDo0MiAgICAgICAgIj4gPHJkZjpSREYgeG1sbnM6cmRmPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5LzAyLzIyLXJkZi1zeW50YXgtbnMjIj4gPHJkZjpEZXNjcmlwdGlvbiByZGY6YWJvdXQ9IiIgeG1sbnM6eG1wPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvIiB4bWxuczp4bXBNTT0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIgeG1sbnM6c3RSZWY9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9zVHlwZS9SZXNvdXJjZVJlZiMiIHhtcDpDcmVhdG9yVG9vbD0iQWRvYmUgUGhvdG9zaG9wIENDIDIwMTUgKFdpbmRvd3MpIiB4bXBNTTpJbnN0YW5jZUlEPSJ4bXAuaWlkOkEyRkUyNzg1QTMwRDExRUE5RjUwQ0RBNUY3ODc1N0MzIiB4bXBNTTpEb2N1bWVudElEPSJ4bXAuZGlkOkEyRkUyNzg2QTMwRDExRUE5RjUwQ0RBNUY3ODc1N0MzIj4gPHhtcE1NOkRlcml2ZWRGcm9tIHN0UmVmOmluc3RhbmNlSUQ9InhtcC5paWQ6QTJGRTI3ODNBMzBEMTFFQTlGNTBDREE1Rjc4NzU3QzMiIHN0UmVmOmRvY3VtZW50SUQ9InhtcC5kaWQ6QTJGRTI3ODRBMzBEMTFFQTlGNTBDREE1Rjc4NzU3QzMiLz4gPC9yZGY6RGVzY3JpcHRpb24+IDwvcmRmOlJERj4gPC94OnhtcG1ldGE+IDw/eHBhY2tldCBlbmQ9InIiPz4KyQGHAAAR50lEQVR42uxd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";
                byte[] imageBytes = Convert.FromBase64String(base64);
                mobileImage = iTextSharp.text.Image.GetInstance(imageBytes);
                mobileImage.Alignment = iTextSharp.text.Image.ALIGN_RIGHT;
                mobileImage.ScaleToFit(60, 60);

                Chunk cmobImg = new Chunk(mobileImage, 0, -2);


                document.Add(PDFHandler.GetSectionHeader(""));


                Chunk glue = new Chunk(new VerticalPositionMark());
                Phrase ph1 = new Phrase();
                ph1.Add(new Chunk(Environment.NewLine));
                Paragraph main = new Paragraph();
                Paragraph para = new Paragraph();
                //if (mobileImageClient != null)
                //{
                //    ph1.Add(new Chunk(mobileImageClient, 0, -2));
                //}

                ph1.Add(glue);
                if (mobileImage != null)
                {
                    ph1.Add(new Chunk(mobileImage, 0, -2));
                }

                main.Add(ph1);
                para.Add(main);
                document.Add(para);

                LineSeparator line = new LineSeparator(0.5f, 100f, new BaseColor(0, 150, 17), Element.ALIGN_BASELINE, 2);
                //---------------------------BillingAddress-------------------------------////
                Paragraph ourname = new Paragraph("Muses Publishing House (Pvt) Ltd");
                ourname.Alignment = Element.ALIGN_RIGHT;
                document.Add(ourname);

                Paragraph date = new Paragraph(DateTime.Today.ToString("MM/dd/yyyy"));
                date.Alignment = Element.ALIGN_RIGHT;
                document.Add(date);

                //Paragraph header = new Paragraph("Invoice : "+order.UId);
                //header.Alignment = Element.ALIGN_CENTER;
                //document.Add(header);

                Chunk chunk1 = new Chunk("Invoice : " + order.UId);
                chunk1.SetUnderline(2, -3);
                document.Add(new Phrase(chunk1));



                Paragraph name = new Paragraph(order.FirstName+" "+ order.LastName);
                name.Alignment = Element.ALIGN_LEFT;
                document.Add(name);
                Paragraph address = new Paragraph(order.DeliveryAddress);
                address.Alignment = Element.ALIGN_LEFT;
                document.Add(address);

                document.Add(PDFHandler.GetSectionHeader(""));
                LineSeparator lineTicketInfo1 = new LineSeparator(0.001F, 100f, new BaseColor(215, 25, 35), Element.ALIGN_LEFT, 2);
                document.Add(lineTicketInfo1);

                //---------------------------Invoice--------------------------------------////
                Paragraph purchaseDetail = new Paragraph("Purchased Items");
                purchaseDetail.Alignment = Element.ALIGN_LEFT;
                document.Add(purchaseDetail);
                //document.Add(PDFHandler.GetSectionHeader("Invoice"));
                PdfPTable table = PDFHandler.GenaratePdfTable(6, new float[] { 6f, 6f, 4f, 4f,4f,4f }, new string[] { "Book Name","", "Quantity", "Total Price", "Discount", "Net Total" });
                foreach (Cart_Book cb in cart.Items)
                {
                    BookVMTile b = BusinessHandlerBook.GetSearchedBookForView(cb.BookId);
                    BookProperties p = b.Property.Where(x => x.Id == cb.BookPropertyId).FirstOrDefault();

                    table.AddCell(PDFHandler.GetTableContent(b.BookName));
                    if (p != null)
                    {
                        table.AddCell(PDFHandler.GetTableContent(p.Title));
                    }
                    else
                    {
                        table.AddCell(PDFHandler.GetTableContent(""));
                    }
                    
                    table.AddCell(PDFHandler.GetTableContent(cb.NumberOfItems.ToString()));
                    table.AddCell(PDFHandler.GetTableContent(cb.AmountBeforeDiscount.ToString()));
                    table.AddCell(PDFHandler.GetTableContent(cb.Discount.ToString()));
                    table.AddCell(PDFHandler.GetTableContent(cb.AmountAfterDiscount.ToString()));
                }

                table.AddCell(PDFHandler.GetTableContent(""));
                table.AddCell(PDFHandler.GetTableContent(""));
                table.AddCell(PDFHandler.GetTableContent(cart.Items.Count+ "Item(s)"));
                table.AddCell(PDFHandler.GetTableContent("Rs: "+cart.AmountBeforeDiscount));
                table.AddCell(PDFHandler.GetTableContent("Rs: "+cart.Discount));
                table.AddCell(PDFHandler.GetTableContent("Rs: "+cart.AmountAfterDiscount));

                table.AddCell(PDFHandler.GetTableContent(""));
                table.AddCell(PDFHandler.GetTableContent(""));
                table.AddCell(PDFHandler.GetTableContent(""));
                table.AddCell(PDFHandler.GetTableContent(""));
                table.AddCell(PDFHandler.GetTableContent("Delivery Charges"));
                table.AddCell(PDFHandler.GetTableContent("Rs: "+order.DeliveryCharges));

                table.AddCell(PDFHandler.GetTableContent(""));
                table.AddCell(PDFHandler.GetTableContent(""));
                table.AddCell(PDFHandler.GetTableContent(""));
                table.AddCell(PDFHandler.GetTableContent(""));
                table.AddCell(PDFHandler.GetTableContent("Total Amount"));
                table.AddCell(PDFHandler.GetTableContent("Rs: "+(cart.AmountAfterDiscount + order.DeliveryCharges)));

                decimal AmountTobeCollected = cart.AmountAfterDiscount + order.DeliveryCharges;
                if (!string.IsNullOrEmpty(cart.VoucherCode))
                {
                    Voucher voucher = BusinessHandlerVoucher.GetActiveVoucherByCodeForInvoice(cart.VoucherCode);

                    table.AddCell(PDFHandler.GetTableContent(""));
                    table.AddCell(PDFHandler.GetTableContent(""));
                    table.AddCell(PDFHandler.GetTableContent(""));
                    table.AddCell(PDFHandler.GetTableContent(""));
                    table.AddCell(PDFHandler.GetTableContent("Voucher Added("+voucher.Code+")"));
                    table.AddCell(PDFHandler.GetTableContent("Rs: "+voucher.VoucherAmount));

                if ((cart.AmountAfterDiscount + order.DeliveryCharges) - voucher.VoucherAmount > -1)
                    {
                        AmountTobeCollected = ((cart.AmountAfterDiscount + order.DeliveryCharges) - voucher.VoucherAmount);

                        table.AddCell(PDFHandler.GetTableContent(""));
                        table.AddCell(PDFHandler.GetTableContent(""));
                        table.AddCell(PDFHandler.GetTableContent(""));
                        table.AddCell(PDFHandler.GetTableContent(""));
                        table.AddCell(PDFHandler.GetTableContent("Remaining Payment"));
                        table.AddCell(PDFHandler.GetTableContent("Rs: "+((cart.AmountAfterDiscount + order.DeliveryCharges) - voucher.VoucherAmount)));
                    }
                else
                {
                    AmountTobeCollected = 0;
                        table.AddCell(PDFHandler.GetTableContent(""));
                        table.AddCell(PDFHandler.GetTableContent(""));
                        table.AddCell(PDFHandler.GetTableContent(""));
                        table.AddCell(PDFHandler.GetTableContent(""));
                        table.AddCell(PDFHandler.GetTableContent("Remaining Payment"));
                        table.AddCell(PDFHandler.GetTableContent("Rs: 0.00"));

                    }

                }

                document.Add(table);
                document.Add(PDFHandler.GetSectionHeader(""));
                LineSeparator lineTicketInfo = new LineSeparator(0.001F, 100f, new BaseColor(215, 25, 35), Element.ALIGN_LEFT, 2);
                document.Add(lineTicketInfo);
                if (isPaid)
                {
                    iTextSharp.text.Image mobileImage2 = null;
                    string base642 = @"data:image/jpeg;base64,/9j/4AAQSkZJRgABAQEAYABgAAD/4REARXhpZgAATU0AKgAAAAgABAE7AAIAAAASAAAISodpAAQAAAABAAAIXJydAAEAAAAkAAAQ1OocAAcAAAgMAAAAPgAAAAAc6gAAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAEVyYW5nYSBXaWpldGh1bmdhAAAFkAMAAgAAABQAABCqkAQAAgAAABQAABC+kpEAAgAAAAM4MwAAkpIAAgAAAAM4MwAA6hwABwAACAwAAAieAAAAABzqAAAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAMjAyMTowMjoxMCAyMDowMzo0MwAyMDIxOjAyOjEwIDIwOjAzOjQzAAAARQByAGEAbgBnAGEAIABXAGkAagBlAHQAaAB1AG4AZwBhAAAA/+ELJGh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC8APD94cGFja2V0IGJlZ2luPSfvu78nIGlkPSdXNU0wTXBDZWhpSHpyZVN6TlRjemtjOWQnPz4NCjx4OnhtcG1ldGEgeG1sbnM6eD0iYWRvYmU6bnM6bWV0YS8iPjxyZGY6UkRGIHhtbG5zOnJkZj0iaHR0cDovL3d3dy53My5vcmcvMTk5OS8wMi8yMi1yZGYtc3ludGF4LW5zIyI+PHJkZjpEZXNjcmlwdGlvbiByZGY6YWJvdXQ9InV1aWQ6ZmFmNWJkZDUtYmEzZC0xMWRhLWFkMzEtZDMzZDc1MTgyZjFiIiB4bWxuczpkYz0iaHR0cDovL3B1cmwub3JnL2RjL2VsZW1lbnRzLzEuMS8iLz48cmRmOkRlc2NyaXB0aW9uIHJkZjphYm91dD0idXVpZDpmYWY1YmRkNS1iYTNkLTExZGEtYWQzMS1kMzNkNzUxODJmMWIiIHhtbG5zOnhtcD0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wLyI+PHhtcDpDcmVhdGVEYXRlPjIwMjEtMDItMTBUMjA6MDM6NDMuODI2PC94bXA6Q3JlYXRlRGF0ZT48L3JkZjpEZXNjcmlwdGlvbj48cmRmOkRlc2NyaXB0aW9uIHJkZjphYm91dD0idXVpZDpmYWY1YmRkNS1iYTNkLTExZGEtYWQzMS1kMzNkNzUxODJmMWIiIHhtbG5zOmRjPSJodHRwOi8vcHVybC5vcmcvZGMvZWxlbWVudHMvMS4xLyI+PGRjOmNyZWF0b3I+PHJkZjpTZXEgeG1sbnM6cmRmPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5LzAyLzIyLXJkZi1zeW50YXgtbnMjIj48cmRmOmxpPkVyYW5nYSBXaWpldGh1bmdhPC9yZGY6bGk+PC9yZGY6U2VxPg0KCQkJPC9kYzpjcmVhdG9yPjwvcmRmOkRlc2NyaXB0aW9uPjwvcmRmOlJERj48L3g6eG1wbWV0YT4NCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAg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";
                    byte[] imageBytes2 = Convert.FromBase64String(base642);
                    mobileImage2 = iTextSharp.text.Image.GetInstance(imageBytes);
                    mobileImage2.Alignment = iTextSharp.text.Image.ALIGN_RIGHT;
                    mobileImage2.ScaleToFit(60, 60);
                    Chunk cmobImg2 = new Chunk(mobileImage2, 0, -2);
                }
                

                

                writer.PageEvent = new Footer();
                if (openAfterGenerate)
                {
                    //Process.Start(filename);
                }
                result = true;
            }
            catch (Exception ex)
            {
                if (!document.IsOpen())
                {
                    document.Open();
                }
                document.Add(PDFHandler.GetNormalText("Error Genarating Invoice"));
                result = false;
            }
            finally
            {
                if (document.IsOpen())
                {
                    document.Close();
                }
                
            }
            return result;
        }
    }

    public partial class Footer : PdfPageEventHelper

    {

        public override void OnEndPage(PdfWriter writer, Document doc)

        {
            PdfPTable footerTbl = new PdfPTable(1);
            footerTbl.TotalWidth = 800;
            footerTbl.HorizontalAlignment = Element.ALIGN_CENTER;

            PdfPCell cell = new PdfPCell(new Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)));
            BaseColor green = WebColors.GetRGBColor("#d71923");
            cell.BackgroundColor = green;
            cell.Border = 0;
            cell.PaddingLeft = 0;
            footerTbl.AddCell(cell);
            PdfPCell cell2 = new PdfPCell(new Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)));
            BaseColor grey = WebColors.GetRGBColor("#8f8f94");
            cell2.BackgroundColor = grey;
            cell2.Border = 0;
            cell2.PaddingLeft = 0;
            footerTbl.AddCell(cell2);
            footerTbl.WriteSelectedRows(0, -1, 0, 25, writer.DirectContent);
        }

    }
    public partial class Header : PdfPageEventHelper
    {
        public override void OnStartPage(PdfWriter writer, Document document)
        {
            PdfPTable HeaderTbl = new PdfPTable(1);
            HeaderTbl.TotalWidth = 800;
            HeaderTbl.HorizontalAlignment = Element.ALIGN_CENTER;

            PdfPCell cell2 = new PdfPCell(new Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)));
            BaseColor grey = WebColors.GetRGBColor("#8f8f94");
            cell2.BackgroundColor = grey;
            cell2.Border = 0;
            cell2.PaddingLeft = 0;
            HeaderTbl.AddCell(cell2);
            HeaderTbl.WriteSelectedRows(0, -1, 0, 25, writer.DirectContent);
        }
    }


}