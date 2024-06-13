using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Hospital.ModelsDTO;
using Hospital.Models;
using System.Drawing.Printing;
using Avalonia.Dialogs;
using PdfiumViewer;
using System.Diagnostics;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using System.Threading.Tasks;

namespace Hospital.Services
{
    public class Report(DateOnly[] range)
    {
        public Document PdfDoc { get; set; } = null!;

        public DateOnly[] DateRange { get; set; } = range;

        public List<DispensingDrug> DispensingDrugList { get; set; } = null!;

        public List<ReceivingDrug> ReceivingDrugList { get; set; } = null!;

        public void GetReportData()
        {
            try
            {
                DispensingDrugList = DBCall.GetDispensingDrugData(DateRange);
                ReceivingDrugList = DBCall.GetReceivingDrugData(DateRange);
            }
            catch (Exception)
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

        public async Task<string> CreateReport(ReportWindow window)
        {
            var storageProvider = window.StorageProvider;
            var result = await storageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
            {
                Title = "Сохранить отчет как",
                FileTypeChoices =
                [
                    new FilePickerFileType("PDF")
                    {
                        Patterns = ["*.pdf"]
                    }
                ],
                DefaultExtension = "pdf"
            });
            if (result != null)
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                PdfDoc = new Document(PageSize.A4, 40f, 40f, 60f, 60f);

                try
                {
                    using var fs = await result.OpenWriteAsync();
                    PdfWriter.GetInstance(PdfDoc, fs);
                    PdfDoc.Open();

                    string fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "times.TTF");
                    BaseFont fgBaseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                    Font fgFont = new(fgBaseFont, 14, Font.NORMAL, new BaseColor(0, 0, 0));

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
                    return "Отчет создан";
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return "";
        }

        private void AddHeaderTable(Font fgFont)
        {
            var headerTable = new PdfPTable([.75f])
            {
                HorizontalAlignment = 0,
                WidthPercentage = 75,
                DefaultCell = { MinimumHeight = 22f },
            };

            PdfPCell cell = new(new Phrase($"Дата: {DateTime.Now:dd.MM.yyyy}", fgFont))
            {
                Border = Rectangle.NO_BORDER
            };
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
            if (DispensingDrugList.Count > 0)
            {
                var dispensingDrugsTable = new PdfPTable([.75f, .75f])
                {
                    HorizontalAlignment = 1,
                    WidthPercentage = 75,
                    DefaultCell = { MinimumHeight = 22f },
                };

                foreach (var item in DispensingDrugList)
                {
                    PdfPCell cell = new(new Phrase(item.Drug.Name, fgFont));
                    dispensingDrugsTable.AddCell(cell);

                    PdfPCell cell2 = new(new Phrase(item.Count.ToString(), fgFont));
                    dispensingDrugsTable.AddCell(cell2);
                }
                PdfDoc.Add(dispensingDrugsTable);
            }
            else
            {
                var title = new Paragraph($"Отсутсвует", fgFont)
                {
                    SpacingAfter = 25f,
                    Alignment = Element.ALIGN_CENTER
                };
                PdfDoc.Add(title);

            }

        }

        private void AddReceivingDrugTable(Font fgFont)
        {
            CreateTitle("Статистика полученных лекарств за период");

            if (ReceivingDrugList.Count > 0)
            {
                var receivingDrugsTable = new PdfPTable([.75f, .75f])
                {
                    HorizontalAlignment = 1,
                    WidthPercentage = 75,
                    DefaultCell = { MinimumHeight = 22f },
                };

                foreach (var item in ReceivingDrugList)
                {
                    PdfPCell cell = new(new Phrase(item.Drug.Name, fgFont));
                    receivingDrugsTable.AddCell(cell);

                    PdfPCell cell2 = new(new Phrase(item.Count.ToString(), fgFont));
                    receivingDrugsTable.AddCell(cell2);
                }

                PdfDoc.Add(receivingDrugsTable);
            }
            else
            {
                var title = new Paragraph($"Отсутсвует", fgFont)
                {
                    SpacingAfter = 25f,
                    Alignment = Element.ALIGN_CENTER
                };
                PdfDoc.Add(title);
            }
        }
    }
}
