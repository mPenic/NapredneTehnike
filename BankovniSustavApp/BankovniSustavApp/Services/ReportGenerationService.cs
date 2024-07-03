using System.Collections.ObjectModel;
using BankovniSustavApp.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Windows.Documents;
using System.Xml.Linq;
using System.Windows;

namespace BankovniSustavApp.Services
{
    public class ReportGenerationService
    {
        public void GeneratePdfReport(ObservableCollection<Transakcije> transactions, string filePath)
        {
            Document document = new Document();
            PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));
            document.Open();
            foreach (var transaction in transactions)
            {
                document.Add(new iTextSharp.text.Paragraph($"Transaction ID: {transaction.TransakcijaID}, Amount: {transaction.Iznos}"));
            }
            document.Close();

            MessageBox.Show("Document successfully added to the root repository", "Success");
        }

        public void GenerateRtfReport(ObservableCollection<Transakcije> transactions, string filePath)
        {
            var document = new FlowDocument();
            var paragraph = new System.Windows.Documents.Paragraph();
            foreach (var transaction in transactions)
            {
                string line = $"ID: {transaction.TransakcijaID}, Account: {transaction.RacunID}, Date/Time: {transaction.DatumVrijeme}, Amount: {transaction.Iznos}, Type: {transaction.Vrsta}, Description: {transaction.Opis}\n";
                paragraph.Inlines.Add(new Run(line));
            }
            document.Blocks.Add(paragraph);

            TextRange range = new TextRange(document.ContentStart, document.ContentEnd);
            using (FileStream fileStream = File.Create(filePath))
            {
                range.Save(fileStream, DataFormats.Rtf);
            }
            MessageBox.Show("Document successfully added to the root repository", "Success");
        }
    }
}
