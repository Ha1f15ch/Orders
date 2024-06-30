using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ModelsEntity;
using OfficeOpenXml;
using System.IO;

namespace ApplicationDbContext
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options, string excelDataFilePath) : base(options)
        {
            Database.EnsureCreated();
            initializeCategoriesFromExcel(excelDataFilePath);
            initializeProfessionsFromExcel(excelDataFilePath);
        }

        private void initializeCategoriesFromExcel(string excelDataFilePath)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            FileInfo fi = new FileInfo(excelDataFilePath);

            var excelPackage = new ExcelPackage(fi);
            var worksheet1 = excelPackage.Workbook.Worksheets["1"];

            List<string> excelCategories = new List<string>();
            for (int row = 2; row <= worksheet1.Dimension.End.Row; row++)
            {
                excelCategories.Add(worksheet1.Cells[row, 1].Value.ToString());
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

        private void initializeProfessionsFromExcel(string excelDataFilePath)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            FileInfo fi = new FileInfo(excelDataFilePath);

            var excelPackage = new ExcelPackage(fi);
            var worksheet2 = excelPackage.Workbook.Worksheets["2"];

            List<string> excelProfessions = new List<string>();
            for (int row = 2; row <= worksheet2.Dimension.End.Row; row++)
            {
                excelProfessions.Add(worksheet2.Cells[row, 1].Value.ToString());
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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var config = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json")
                            .Build();

            string excelDataFilePath = config.GetConnectionString("ExcelDataPath");
            FileInfo fi = new FileInfo(excelDataFilePath);
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
    }
}
