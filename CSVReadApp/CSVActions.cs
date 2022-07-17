using CSVReadApp.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CSVReadApp
{
    public static class CSVActions
    {
        public static List<string[]> ReadCSVToList(string fileName)
        {
            var lines = File.ReadAllLines(fileName, Encoding.Default);

            return lines.Select(l => l.Split(';')).ToList();
        }
        
        public static void UpdateDepartmentsList(List<string[]> list)
        {
            var currentLine = 0;
            var departments = new List<Department>();
            var context = DatabaseContext.GetInstance();

            foreach (var line in list)
            {
                if(line.Length > 2 && currentLine > 5) // пропускаем заголовок
                {
                    var lineName = line[2];

                    if (lineName.Length > 0) //Проверка на пустое значение
                    {
                        if (departments.Count(i => i.Name.Equals(lineName)) == 0) // проверка на уникальность
                        {
                            if (context.Department.Select(e => e.Name).Contains(lineName) == false)
                                departments.Add(new Department(lineName));
                        }
                    }
                }
                currentLine++;
            }

            context.AddRange(departments);
            context.SaveChanges();
        }
        public static void UpdateCatalogAndRepoList(List<string[]> list)
        {
            var letterId = 0;
            var currentLine = 0;
            char[] alphabet = { 'А', 'B', 'C', 'D' };
            var catalogsStack = new Stack<Catalog>();
            var allCatalogs = new List<Catalog>();
            var allRepositories = new List<Repository>();
            var repositoryDepartments = new List<RepositoryDepartment>();
            var context = DatabaseContext.GetInstance();

            var currentRepository = new Repository();
            
            foreach (var line in list)
            {
                if (currentLine > 5) // пропускаем заголовок
                {
                    if(line.Length < 2) continue;
                    
                    var lineKey = line[0];
                    var lineName = line[1];
                    var departmentName = line[2];
                    
                    if (line[2].Length == 0) // Проверка длинны
                    {
                        if (lineKey.Length == 0) // Если каталог не содержит кода
                        {
                            var catalog = new Catalog(alphabet[letterId].ToString(), lineName);
                            catalogsStack.Clear();
                            catalogsStack.Push(catalog);

                            if (!context.Catalog.Contains(catalog))
                                allCatalogs.Add(catalog);
                            letterId++;
                        }
                        else
                        {
                            if (lineKey.ToUpperInvariant().IndexOf(catalogsStack.Peek().IdKey.ToUpperInvariant()) != -1) //если мы в подкаталоге
                            {
                                var catalog = new Catalog(lineKey, lineName, catalogsStack.Peek().IdKey);
                                catalogsStack.Push(catalog);

                                if (!context.Catalog.Contains(catalog))
                                    allCatalogs.Add(catalog);
                            }
                            else
                            {
                                do //Поднимаемся в пред. каталог
                                {
                                    catalogsStack.Pop();
                                } while (lineKey.ToUpperInvariant().IndexOf(catalogsStack.Peek().IdKey.ToUpperInvariant()) == -1);

                                var catalog = new Catalog(lineKey, lineName, catalogsStack.Peek().IdKey);
                                catalogsStack.Push(catalog);

                                if (!context.Catalog.Contains(catalog))
                                    allCatalogs.Add(catalog);
                            }
                        }
                    }

                    if(line[2].Length != 0) //Добавляем репозиторий
                    {
                        if (!lineKey.Equals("")) // если строка ключа не пустая
                        {
                            var repository = new Repository(lineKey, lineName, catalogsStack.Peek().IdKey);
                            if (!context.Repository.Contains(repository))
                            {
                                allRepositories.Add(repository);
                            }

                            currentRepository = repository;
                        }

                        var departmentId = context.Department.Where(i => i.Name.Equals(departmentName)).Select(i => i.Id).FirstOrDefault();
                        var repositoryDepartment = new RepositoryDepartment(departmentId, currentRepository.IdKey);
                        
                        var existedRepository = context.RepositoryDepartment
                            .FirstOrDefault(i => i.DepartmentId == departmentId && i.RepositoryId == currentRepository.IdKey);

                        if(existedRepository == null)
                            repositoryDepartments.Add(repositoryDepartment);
                    }
                }
                
                currentLine++;
            }
            
            context.AddRange(allCatalogs);
            context.AddRange(allRepositories);
            context.AddRange(repositoryDepartments);
            context.SaveChanges();
        }
    }
}
