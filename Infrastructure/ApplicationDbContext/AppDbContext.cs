using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ModelsEntity;
using OfficeOpenXml;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace ApplicationDbContext
{
    public class AppDbContext : DbContext
    {
        private readonly string _excelDataFilePath;

        public AppDbContext(DbContextOptions<AppDbContext> options, string excelDataFilePath) : base(options)
        {
            _excelDataFilePath = excelDataFilePath;
            Database.EnsureCreated();
            InitializeDataFromExcel();
        }

        private void InitializeDataFromExcel()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            try
            {
                using (var excelPackage = new ExcelPackage(new FileInfo(_excelDataFilePath)))
                {
                    InitializeCategoriesFromExcel(excelPackage.Workbook.Worksheets["1"]);
                    InitializeProfessionsFromExcel(excelPackage.Workbook.Worksheets["2"]);
                    InitializeOrderStatusFromExcel(excelPackage.Workbook.Worksheets["3"]);
                }
            }
            catch (FileNotFoundException ex)
            {
                throw new Exception($"Файл Excel не найден по пути - {_excelDataFilePath}", ex);
            }
            catch (Exception ex) 
            {
                throw new Exception("Произошла ошибка при обработке данных из файле Excel", ex);
            }
        }

        private void InitializeCategoriesFromExcel(ExcelWorksheet worksheet)
        {
            List<string> excelCategories = new List<string>();
            for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
            {
                if (worksheet.Cells[row, 1].Value != null)
                {
                    excelCategories.Add(worksheet.Cells[row, 1].Value.ToString());
                }
            }

            var existingCategories = Categories.Where(c => excelCategories.Contains(c.NameOfCategory)).ToList();

            foreach (var existingCategory in existingCategories)
            {
                excelCategories.Remove(existingCategory.NameOfCategory);
            }

            foreach (var categoryName in excelCategories)
            {
                var category = new Category { NameOfCategory = categoryName };
                Categories.Add(category);
            }
            SaveChanges();
        }

        private void InitializeProfessionsFromExcel(ExcelWorksheet worksheet)
        {
            List<string> excelProfessions = new List<string>();
            for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
            {
                if (worksheet.Cells[row, 1].Value != null)
                {
                    excelProfessions.Add(worksheet.Cells[row, 1].Value.ToString());
                }
            }

            var existingProfessions = Professions.Where(c => excelProfessions.Contains(c.NameOfProfession)).ToList();

            foreach (var existingProfession in existingProfessions)
            {
                excelProfessions.Remove(existingProfession.NameOfProfession);
            }

            foreach (var professionName in excelProfessions)
            {
                var profession = new Profession { NameOfProfession = professionName };
                Professions.Add(profession);
            }
            SaveChanges();
        }

        private void InitializeOrderStatusFromExcel(ExcelWorksheet worksheet)
        {
            List<string> excelOrderStatuses = new List<string>();
            for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
            {
                if (worksheet.Cells[row, 1].Value != null)
                {
                    excelOrderStatuses.Add(worksheet.Cells[row, 1].Value.ToString());
                }
            }

            var existingOrderStatuses = OrderStatuses.Where(c => excelOrderStatuses.Contains(c.Name)).ToList();

            foreach (var existingOrderStatus in existingOrderStatuses)
            {
                excelOrderStatuses.Remove(existingOrderStatus.Name);
            }

            foreach (var orderStatusName in excelOrderStatuses)
            {
                var orderStatus = new OrderStatus { Name = orderStatusName };
                OrderStatuses.Add(orderStatus);
            }
            SaveChanges();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var config = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json")
                            .Build();

            optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultDatabaseConnection"));
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRoleMapping> UserRoleMappings { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Profession> Professions { get; set; }
        public DbSet<ProfessionServiceMapping> ProfessionServiceMappings { get; set; }
        public DbSet<CategoryProfessionMapping> CategoryProfessionMappings { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Performer> Performers { get; set; }
        public DbSet<PerformerServiceMapping> PerformerServicesMapping { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }
        public DbSet<OrderPriority> OrderPriority { get; set; }
    }
}
