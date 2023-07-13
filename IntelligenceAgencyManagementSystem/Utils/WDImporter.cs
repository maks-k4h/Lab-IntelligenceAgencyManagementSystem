using ClosedXML.Excel;
using IntelligenceAgencyManagementSystem.Utils.Algorithms;

namespace IntelligenceAgencyManagementSystem.Utils;

public class WDImporter
{
    public const string DepartmentHeader    = "департамент";
    public const string WorkerHeader        = "працівник";
    public const string RoleHeader          = "посада";
    public const string DescriptionHeader   = "опис";
    public const string FromDateHeader      = "від";
    public const string ToDateHeader        = "до";
    
    private const int MaxColNum             = 20;
    private readonly IaDbContext _context;

    private const int DepartmentLevenshteinD    = 4;
    private const int WorkerLevenshteinD        = 3;
    private const int RoleLevenshteinD          = 2;

    private class HeaderMapper
    {
        private readonly Dictionary<string, int?> _mapping = new Dictionary<string, int?>();
        private const int LevenshteinDistance = 1;

        public HeaderMapper()
        {
            _mapping[DepartmentHeader]  = null;
            _mapping[WorkerHeader]      = null;
            _mapping[RoleHeader]        = null;
            _mapping[DescriptionHeader] = null;
            _mapping[FromDateHeader]    = null;
            _mapping[ToDateHeader]      = null;
        }

        public int TryToAdd(string header, int value)
        {
            header = header.ToLower().Trim();
            if (header.Length == 0)
                return -1;

            if (_mapping.ContainsKey(header)){
                if (_mapping[header] != null)   // repeated addition not allowed
                    throw new Exception("Атрибут '" + header + "' не можна вказувати більше одного разу");
                
                _mapping[header] = value;   // header is correct, inserting the value
                return value;
            }
            
            // finding the best match
            for (int i = 1; i <= LevenshteinDistance; ++i)
            {
                foreach (var h in _mapping.Keys)
                {
                    if (Algorithms.LevenshteinDistance.Calculate(header, h) != i)
                        continue;

                    if (_mapping[h] != null)
                        throw new Exception("Атрибут '" + h + "' не можна вказувати більше одного разу");

                    _mapping[h] = value;
                    return value;
                }
            }

            return -1;
        }

        /// <summary>
        ///     Check if all the headers are mapped
        /// </summary>
        /// <returns>bool</returns>
        public bool IsCompleted()
        {
            foreach (var value in _mapping.Values)
                if (value == null)
                    return false;
            return true;
        }

        public int GetByHeader(string header)
        {
            var res = _mapping[header];
            if (res == null)
                throw new Exception();
            return (int)res;
        }
    }
    
    public WDImporter(IaDbContext context)
    {
        _context = context;
    }

    public void ImportExcel(XLWorkbook workbook, bool matchFirstColumns=false)
    {
        foreach (IXLWorksheet worksheet in workbook.Worksheets)
        {
            HeaderMapper mapper = new HeaderMapper();
            var headers = worksheet.RowsUsed().First();

            // mapping headers
            for (int col = 1; col <= MaxColNum; ++col)
            {
                mapper.TryToAdd(headers.Cell(col).Value.ToString(), col);
                if (matchFirstColumns && mapper.IsCompleted())
                    break;
            }

            if (!mapper.IsCompleted())
                throw new Exception("Не вдалося визначити заголовки!");

            foreach (var row in worksheet.RowsUsed().Skip(1))
            {
                var args = new Dictionary<string, string>();
                args[DepartmentHeader]  = row.Cell(mapper.GetByHeader(DepartmentHeader)).Value.ToString();
                args[WorkerHeader]      = row.Cell(mapper.GetByHeader(WorkerHeader)).Value.ToString();
                args[RoleHeader]        = row.Cell(mapper.GetByHeader(RoleHeader)).Value.ToString();
                args[DescriptionHeader] = row.Cell(mapper.GetByHeader(DescriptionHeader)).Value.ToString();
                args[FromDateHeader]    = row.Cell(mapper.GetByHeader(FromDateHeader)).Value.ToString();
                args[ToDateHeader]      = row.Cell(mapper.GetByHeader(ToDateHeader)).Value.ToString();
                try
                {
                    AddWD(args);
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message + " (сторінка '" + worksheet.Name + "', рядок " + row.RowNumber() + ")");
                }
            }
        }
        try
        {
            _context.SaveChanges();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw new Exception("Не вдалося імпортувати файл");
        }
    }


    private void AddWD(Dictionary<string, string> args)
    {
        Department department = GetDepartmentByHeader(args[DepartmentHeader]);
        Worker worker = GetWorkerByFullName(args[WorkerHeader]);
        Role? role = TryGetRoleByTitle(args[RoleHeader]);
        if (role == null)   // if the role does not exist — create one
        {
            role = new Role();
            string roleTitle = args[RoleHeader].ToLower();
            role.Title = char.ToUpper(roleTitle[0]) + roleTitle.Substring(1);

            _context.Roles.Add(role);
        }
        
        WorkingInDepartment wd = new WorkingInDepartment();
        wd.Department = department;
        wd.Worker = worker;
        wd.Role = role;
        wd.Description = args[DescriptionHeader];

        if (DateOnly.TryParse(args[FromDateHeader], out var dateStarted))
            wd.DateStarted = dateStarted;
        else
            throw new Exception("Значення поля 'від' є невалідним");
        if (DateOnly.TryParse(args[ToDateHeader], out var dateEnded))
            wd.DateEnded = dateEnded;

        wd.Description = args[DescriptionHeader];

        _context.WorkingInDepartments.Add(wd);
    }

    private Department GetDepartmentByHeader(string header)
    {
        header = header.Trim().ToLower();

        var departments = _context.Departments.ToArray();
        
        // exact match
        var count = departments
            .Count(department => department.Name.Trim().ToLower() == header);

        if (count == 1)
        {
             return departments
                .First(department => department.Name.Trim().ToLower() == header);
        }
        if (count > 1)
        {
            throw new Exception("Існує кілька департаменів з назвою '" + header + "'");
        }
        // approximate search
        for (int d = 1; d <= DepartmentLevenshteinD; ++d)
        {
            count = departments
                .Count(department => LevenshteinDistance.Calculate(department.Name.Trim().ToLower(), header) == d);
            
            if (count == 0) // no department found at this distance
                continue;
            if (count > 2) // two or more departments found at this distance
                break;

            return departments
                .First(department => LevenshteinDistance.Calculate(department.Name.Trim().ToLower(), header) == d);
        }
        
        throw new Exception("Департамент '" + header + "' визначити неможливо.");
    }
    
    private Worker GetWorkerByFullName(string fullName)
    {
        fullName = fullName.Trim().ToLower();

        var workers = _context.Workers.ToArray();
        
        // exact match
        var count = workers
            .Count(worker => worker.FullName.Trim().ToLower() == fullName);

        if (count == 1)
        {
            return workers
                .First(worker => worker.FullName.Trim().ToLower() == fullName);
        }
        if (count > 1)
        {
            throw new Exception("Існує кілька співробітників '" + fullName + "'");
        }
        // approximate search
        for (int d = 1; d <= WorkerLevenshteinD; ++d)
        {
            count = workers
                .Count(worker => LevenshteinDistance.Calculate(worker.FullName.Trim().ToLower(), fullName) == d);
            
            if (count == 0) // no worker found at this distance
                continue;
            if (count > 2) // two or more workers found at this distance
                break;

            return workers
                .First(worker => LevenshteinDistance.Calculate(worker.FullName.Trim().ToLower(), fullName) == d);
        }
        
        throw new Exception("Співробітника '" + fullName + "' визначити неможливо.");
    }
    
    private Role? TryGetRoleByTitle(string title)
    {
        title = title.Trim().ToLower();

        if (title.Length == 0)
            throw new Exception("Необхідно вказати посаду");

        var roles = _context.Roles.ToArray();
        
        // exact match
        var count = roles
            .Count(role => role.Title.Trim().ToLower() == title);

        if (count == 1)
        {
            return roles
                .First(role => role.Title.Trim().ToLower() == title);
        }
        if (count > 1)
        {
            throw new Exception("Існує кілька посад '" + title + "'");
        }
        // approximate search
        for (int d = 1; d <= RoleLevenshteinD; ++d)
        {
            count = roles
                .Count(role => LevenshteinDistance.Calculate(role.Title.Trim().ToLower(), title) == d);
            
            if (count == 0) // no roles found at this distance
                continue;
            if (count > 2) // two or more roles found at this distance
                break;

            return roles
                .First(role => LevenshteinDistance.Calculate(role.Title.Trim().ToLower(), title) == d);
        }

        return null;
    }
}