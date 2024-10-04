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
                    InitializeOrderPriorityFromExcel(excelPackage.Workbook.Worksheets["4"]);
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
            HashSet<string> excelOrderStatuses = new HashSet<string>();
            Dictionary<string, string?> statusDescriptions = new Dictionary<string, string?>();

            for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
            {
                if (worksheet.Cells[row, 1].Value != null) // Считываем первый столбец (статус)
                {
                    var statusName = worksheet.Cells[row, 1].Value.ToString();
                    excelOrderStatuses.Add(statusName);

                    if (worksheet.Cells[row, 2].Value != null) // Считываем второй столбец (описание)
                    {
                        var statusDescription = worksheet.Cells[row, 2].Value.ToString();
                        statusDescriptions[statusName] = statusDescription;
                    }
                    else
                    {
                        statusDescriptions[statusName] = null; // Если описание отсутствует
                    }
                }
            }

            var existingOrderStatuses = OrderStatuses.ToDictionary(c => c.Name, c => c);

            foreach (var status in excelOrderStatuses)
            {
                if (!existingOrderStatuses.ContainsKey(status))
                {
                    var orderStatus = new OrderStatus // Добавляем новый статус
                    {
                        Name = status,
                        Description = statusDescriptions.ContainsKey(status) ? statusDescriptions[status] : null
                    };
                    OrderStatuses.Add(orderStatus);
                }
                else
                {
                    var existingStatus = existingOrderStatuses[status]; // Обновляем описание существующего статуса, если оно было передано
                    if (statusDescriptions.ContainsKey(status))
                    {
                        existingStatus.Description = statusDescriptions[status];
                    }
                }
            }

            SaveChanges();
        }

        private void InitializeOrderPriorityFromExcel(ExcelWorksheet worksheet)
        {
            {
                HashSet<string> excelPriorityes = new HashSet<string>();
                Dictionary<string, string?> priorityDescriptions = new Dictionary<string, string?>();

                for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
                {
                    if (worksheet.Cells[row, 1].Value != null) // Считываем первый столбец (статус)
                    {
                        var priorityName = worksheet.Cells[row, 1].Value.ToString();
                        excelPriorityes.Add(priorityName);

                        if (worksheet.Cells[row, 2].Value != null) // Считываем второй столбец (описание)
                        {
                            var priorityDescription = worksheet.Cells[row, 2].Value.ToString();
                            priorityDescriptions[priorityName] = priorityDescription;
                        }
                        else
                        {
                            priorityDescriptions[priorityName] = null; // Если описание отсутствует
                        }
                    }
                }

                var existingOrderPriorityes = OrderPriority.ToDictionary(c => c.Name, c => c);

                foreach (var priority in excelPriorityes)
                {
                    if (!existingOrderPriorityes.ContainsKey(priority))
                    {
                        var orderPriority = new OrderPriority // Добавляем новый статус
                        {
                            Name = priority,
                            Description = priorityDescriptions.ContainsKey(priority) ? priorityDescriptions[priority] : null
                        };
                        OrderPriority.Add(orderPriority);
                    }
                    else
                    {
                        var existingStatus = existingOrderPriorityes[priority]; // Обновляем описание существующего статуса, если оно было передано
                        if (priorityDescriptions.ContainsKey(priority))
                        {
                            existingStatus.Description = priorityDescriptions[priority];
                        }
                    }
                }

                SaveChanges();
            }
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
        public DbSet<OrderPerformerMapping> OrderPerformerMappings { get; set; }
        public DbSet<QueueOrderCancellations> QueueOrderCancellations { get; set; }
        public DbSet<OrderScore> OrderScores { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<ChatRoom> ChatRooms { get; set; }
        public DbSet<ChatRoomMembers> ChatRoomMembers { get; set; }
    }
}
