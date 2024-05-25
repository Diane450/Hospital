using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using Avalonia.Platform;
using Hospital.ModelsDTO;
using Hospital.Models;

namespace Hospital.Services
{
    public class Report
    {
        public Document PdfDoc { get; set; } = null!;

        public DateOnly[] DateRange { get; set; }

        public List<DispensingDrug> DispensingDrugList { get; set; } = null!;
        
        public List<ReceivingDrug> ReceivingDrugList { get; set; } = null!;


        public Report(DateOnly[] range)
        {
            DateRange = range;
        }

        public void GetReportData()
        {
            try
            {
                DispensingDrugList = DBCall.GetDispensingDrugData(DateRange);
                ReceivingDrugList = DBCall.GetReceivingDrugData(DateRange);
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }

        private void CreateTitle(string text)
        {
            string fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "times.TTF");
            BaseFont fgBaseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);

            var title = new Paragraph(text, new Font(fgBaseFont, 14, Font.BOLD, new BaseColor(0, 0, 0)))
            {
                SpacingAfter = 25f,
                SpacingBefore = 25f,
                Alignment = Element.ALIGN_CENTER
            };
            PdfDoc.Add(title);
        }

        public void CreateReport()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            PdfDoc = new Document(PageSize.A4, 40f, 40f, 60f, 60f);
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            PdfWriter.GetInstance(PdfDoc, new FileStream(desktopPath + $"\\Отчет {DateTime.Now:yyyy-MM-dd_HH-mm-ss}.pdf", FileMode.Create));
            PdfDoc.Open();

            string fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "times.TTF");
            BaseFont fgBaseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            Font fgFont = new Font(fgBaseFont, 14, Font.NORMAL, new BaseColor(0, 0, 0));

            //AddReportLogo();

            var spacer = new Paragraph("")
            {
                SpacingAfter = 10f,
                SpacingBefore = 10f
            };
            PdfDoc.Add(spacer);

            var title = new Paragraph($"ОТЧЕТ ВЫДАЧИ И ПРИЕМА МЕДИКАМЕНТОВ \r ОТ {DateRange[0]} ДО {DateRange[1]}", new Font(fgBaseFont, 14, Font.BOLD, new BaseColor(0, 0, 0)))
            {
                SpacingAfter = 25f,
                Alignment = Element.ALIGN_CENTER
            };
            PdfDoc.Add(title);

            AddHeaderTable(fgFont);

            AddDispensingDrugsTable(fgFont);

            AddReceivingDrugTable(fgFont);

            PdfDoc.Close();
        }

        private void AddHeaderTable(Font fgFont)
        {
            var headerTable = new PdfPTable(new[] { .75f })
            {
                HorizontalAlignment = 0,
                WidthPercentage = 75,
                DefaultCell = { MinimumHeight = 22f },
            };

            PdfPCell cell = new PdfPCell(new Phrase($"Дата: {DateTime.Now.ToString("dd.MM.yyyy")}", fgFont));
            cell.Border = Rectangle.NO_BORDER;
            headerTable.AddCell(cell);

            cell.Phrase = new Phrase($"Отдел: {CurrentUser.Worker.JobTitle.Department.Name}", fgFont);
            headerTable.AddCell(cell);

            cell.Phrase = new Phrase($"Автор: {CurrentUser.Worker.FullName}", fgFont);
            headerTable.AddCell(cell);

            cell.Phrase = new Phrase($"Должность: {CurrentUser.Worker.JobTitle.Title}", fgFont);
            headerTable.AddCell(cell);

            PdfDoc.Add(headerTable);
        }

        private void AddDispensingDrugsTable(Font fgFont)
        {
            CreateTitle("Статистика выданных лекарств за период");
            var dispensingDrugsTable = new PdfPTable(new[] { .75f, .75f})
            {
                HorizontalAlignment = 1,
                WidthPercentage = 75,
                DefaultCell = { MinimumHeight = 22f },
            };

            foreach (var item in DispensingDrugList)
            {
                PdfPCell cell = new PdfPCell(new Phrase(item.Drug.Name, fgFont));
                dispensingDrugsTable.AddCell(cell);

                PdfPCell cell2 = new PdfPCell(new Phrase(item.Count.ToString(), fgFont));
                dispensingDrugsTable.AddCell(cell2);
            }
            PdfDoc.Add(dispensingDrugsTable);
        }

        private void AddReceivingDrugTable(Font fgFont)
        {
            CreateTitle("Статистика полученных лекарств за период");
            var receivingDrugsTable = new PdfPTable(new[] { .75f, .75f })
            {
                HorizontalAlignment = 1,
                WidthPercentage = 75,
                DefaultCell = { MinimumHeight = 22f },
            };

            foreach (var item in ReceivingDrugList)
            {
                PdfPCell cell = new PdfPCell(new Phrase(item.Drug.Name, fgFont));
                receivingDrugsTable.AddCell(cell);

                PdfPCell cell2 = new PdfPCell(new Phrase(item.Count.ToString(), fgFont));
                receivingDrugsTable.AddCell(cell2);
            }

            PdfDoc.Add(receivingDrugsTable);
        }
    }
}
