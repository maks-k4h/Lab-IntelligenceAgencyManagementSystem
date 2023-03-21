using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.EntityFrameworkCore;

namespace IntelligenceAgencyManagementSystem.Utils;

public class WDExporter
{
    private IaDbContext _context;
    
    public const string DepartmentHeader    = "Департамент";
    public const string WorkerHeader        = "Працівник";
    public const string RoleHeader          = "Посада";
    public const string DescriptionHeader   = "Опис";
    public const string FromDateHeader      = "Від";
    public const string ToDateHeader        = "До";

    private Dictionary<string, string> _headerMapping = new Dictionary<string, string>();

    public WDExporter(IaDbContext context)
    {
        _context = context;

        _headerMapping[DepartmentHeader]    = "A";
        _headerMapping[WorkerHeader]        = "B";
        _headerMapping[RoleHeader]          = "C";
        _headerMapping[DescriptionHeader]   = "D";
        _headerMapping[FromDateHeader]      = "E";
        _headerMapping[ToDateHeader]        = "F";
    }

    public XLWorkbook ExportExcel()
    {
        var workbook = new XLWorkbook();
        
        // for each department create new sheet
        foreach (var department in _context.Departments.ToArray())
        {
            var worksheet = workbook.Worksheets.Add(GetAcronym(department.Name));
            
            var wds = _context
                .WorkingInDepartments
                .Where(wd => wd.DepartmentId == department.Id)
                .Include(wd => wd.Department)
                .Include(wd => wd.Worker)
                .Include(wd => wd.Role)
                .ToArray();

            WriteToExcelSheet(wds, worksheet);
        }

        return workbook;
    }

    private void WriteToExcelSheet(WorkingInDepartment[] wds, IXLWorksheet worksheet)
    {
        // header
        worksheet.Row(1).Style.Fill.BackgroundColor = XLColor.Orange;
        worksheet.Row(1).Style.Font.Bold = true;
        worksheet.Cell(_headerMapping[DepartmentHeader] + "1").Value    = DepartmentHeader;
        worksheet.Cell(_headerMapping[WorkerHeader] + "1").Value        = WorkerHeader;
        worksheet.Cell(_headerMapping[RoleHeader] + "1").Value          = RoleHeader;
        worksheet.Cell(_headerMapping[DescriptionHeader] + "1").Value   = DescriptionHeader;
        worksheet.Cell(_headerMapping[FromDateHeader] + "1").Value      = FromDateHeader;
        worksheet.Cell(_headerMapping[ToDateHeader] + "1").Value        = ToDateHeader;
        
        worksheet.Column(_headerMapping[DepartmentHeader]).Width = 40;
        worksheet.Column(_headerMapping[WorkerHeader]).Width = 20;
        worksheet.Column(_headerMapping[RoleHeader]).Width =25;
        worksheet.Column(_headerMapping[DescriptionHeader]).Width = 40;
        worksheet.Column(_headerMapping[FromDateHeader]).Width = 15;
        worksheet.Column(_headerMapping[ToDateHeader]).Width = 15;


        for (int r = 0; r < wds.Length; ++r)
        {
            worksheet.Cell(_headerMapping[DepartmentHeader] + (r+2)).Value    = wds[r].Department!.Name;
            worksheet.Cell(_headerMapping[WorkerHeader] + (r+2)).Value        = wds[r].Worker!.FullName;
            worksheet.Cell(_headerMapping[RoleHeader] + (r+2)).Value          = wds[r].Role!.Title;
            worksheet.Cell(_headerMapping[DescriptionHeader] + (r+2)).Value   = wds[r].Description;
            worksheet.Cell(_headerMapping[FromDateHeader] + (r+2)).Value      = wds[r].DateStarted.ToString();
            worksheet.Cell(_headerMapping[ToDateHeader] + (r+2)).Value        = wds[r].DateEnded != null ? wds[r-1].DateEnded.ToString() : "";
        }
    }

    private string GetAcronym(string str)
    {
        var arr = str.Split();
        string res = "";
        foreach (var word in arr)
        {
            res += char.ToUpper(word[0]);
        }

        return res;
    }
}