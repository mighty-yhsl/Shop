using Shop;
using System;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;

class Program
{
    static void AttachObserverToDao<T>(IDAO<T> dao, IObserver observer)
    {
        if (dao != null && dao is IDAOObservable observableDao)
        {
            observableDao.AddObserver(observer);
        }
    }

    static void Main(string[] args)
    {
        int choiceTable = 0;
        int choiceRole = 0;
        int choiceVariantFirst = 0;
        int choiceVariantSecond = 0;
        int choiceVariantThird = 0;
        int choiceVariantFourth = 0;

        Console.InputEncoding = Encoding.Unicode;
        Console.OutputEncoding = Encoding.Unicode;

        IObserver observer = new Observer();
        Caretaker caretaker = new Caretaker();
        DAOFactory factory = DAOFactory.GetInstance();

        IDAO<Vehicle> vehicleDao = factory.CreateDAO<Vehicle>();
        List<Vehicle> allVehicles = vehicleDao.GetAll();
        AttachObserverToDao(vehicleDao, observer);

        IDAO<Supplier> supplierDao = factory.CreateDAO<Supplier>();
        List<Supplier> allSuppliers = supplierDao.GetAll();
        AttachObserverToDao(supplierDao, observer);

        IDAO<Manufacturer> manufacturerDao = factory.CreateDAO<Manufacturer>();
        List<Manufacturer> allManufacturers = manufacturerDao.GetAll();
        AttachObserverToDao(manufacturerDao, observer);

        IDAO<Roles> rolesDao = factory.CreateDAO<Roles>();

        IDAO<Users> usersDao = factory.CreateDAO<Users>();
        List<Users> allUsers = usersDao.GetAll();
        AttachObserverToDao(usersDao, observer);
        Users? loggedInUser = null;

        Console.WriteLine("\nВітаю!!! Для того щоб потрапити до магазину вам потрібно зареєструватися " +
                "або авторизуватися.\n Щоб зареєструватися натисніть - 1, для авторизації натисніть - 2");
            Console.WriteLine("--------------------------------------------------------------------------------");
            if (int.TryParse(Console.ReadLine(), out choiceRole))
            {
                if (choiceRole == 1)
                {
                    Register(usersDao);
                }
                else if (choiceRole == 2)
                {
                     Login(usersDao);
                }
                else
                {
                    Console.WriteLine("Неправильний вибір, такого варіанту не існує");
                }
            }
            else
            {
                Console.WriteLine("Неправильний вибір, натисніть від 1 до 2");
            }
        do
        {
            Console.WriteLine("-------------------------------------------------------------------------");
            Console.WriteLine("\nДля того, щоб перейти в пункт керування оберіть:" + "\n" +
            "\n Транспорт - 1 \n Постачальник - 2 \n Виробник - 3 \n Користувач - 4\n Вихід з програми - 0");
            Console.WriteLine("-------------------------------------------------------------------------");

            if (int.TryParse(Console.ReadLine(), out choiceTable))
            {
                switch (choiceTable)
                {
                    case 0:
                        Console.WriteLine(" \n Приходьте ще !!!");
                        return;
                    case 1:
                        Console.WriteLine("\n Переглянути всі товари - 1 \n Додати товар - 2 \n Видалити товар за Id - 3 " +
                            "\n Редагувати товар - 4 \n Пошук за назвою - 5 \n Скасування останнього оновлення - 6");
                        Console.WriteLine("-------------------------------------------------------------------------");
                        if (int.TryParse(Console.ReadLine(), out choiceVariantFirst))
                        {
                            switch (choiceVariantFirst)
                            {
                                case 1:
                                    foreach (var vehicle in allVehicles)
                                        Console.WriteLine(JsonSerializer.Serialize(vehicle));
                                    break;
                                case 2:
                                    Console.WriteLine("\n Всі назви писати англійською :\n");
                                    Console.WriteLine("\n Введіть назву товару:\n");
                                    string name = Console.ReadLine();

                                    Console.WriteLine("\n Введіть ціну товару(₴):\n");
                                    float price = float.Parse(Console.ReadLine());

                                    Console.WriteLine("\n Введіть потужність товару(W), опціональне поле :\n");
                                    string powerInput = Console.ReadLine();
                                    int? power = string.IsNullOrEmpty(powerInput) ? (int?)null : (int?)Convert.ToInt32(powerInput);

                                    Console.WriteLine("\n Введіть швидкість товару(km/h), опціональне поле:\n");
                                    string speedInput = Console.ReadLine();
                                    int? speed = string.IsNullOrEmpty(speedInput) ? (int?)null : (int?)Convert.ToInt32(speedInput);

                                    Console.WriteLine("\n Введіть вагу товару(kg), опціональне поле:\n");
                                    string weightInput = Console.ReadLine();
                                    int? weight = string.IsNullOrEmpty(weightInput) ? (int?)null : (int?)Convert.ToInt32(weightInput);

                                    Console.WriteLine("\n Введіть Id виробника товару:\n");
                                    int manufacturer = Convert.ToInt32(Console.ReadLine());

                                    Console.WriteLine("\n Введіть Id постачальника товару:\n");
                                    int supplier = Convert.ToInt32(Console.ReadLine());

                                    Shop.Vehicle newVehicle = new Shop.Vehicle.VehicleBuilder()
                                        .SetName(name)
                                        .SetPrice(price)
                                        .SetPower(power)
                                        .SetSpeed(speed)
                                        .SetWeight(weight)
                                        .SetManufacturerId(manufacturer)
                                        .SetSupplierId(supplier)
                                        .Build();
                                    
                                    vehicleDao.Add(newVehicle);                            
                                    break;
                                case 3:
                                    Console.WriteLine("\n Введіть Id товару для видалення:\n ");
                                    if (int.TryParse(Console.ReadLine(), out int deleteId))
                                    {
                                        vehicleDao.Delete(deleteId);
                                    }
                                    else
                                    {
                                        Console.WriteLine("\n Некоректні дані. Введіть коректний Id товару для видалення.\n");
                                    }
                                    break;
                                case 4:
                                    Console.WriteLine("\n Введіть Id транспорту, який потрібно оновити:\n");
                                    if (int.TryParse(Console.ReadLine(), out int updateId))
                                    {
                                        Shop.Vehicle existingVehicle = allVehicles.FirstOrDefault(vehicle => vehicle.Id == updateId);
                                        if (existingVehicle != null)
                                        {
                                            Vehicle beforeUpdateVehicle = vehicleDao.GetById(updateId);
                                            caretaker.AddChange(beforeUpdateVehicle.Save());

                                            Console.WriteLine("\n Поточні дані для транспорту:\n");
                                            Console.WriteLine(JsonSerializer.Serialize(existingVehicle));

                                            Console.WriteLine("\n Введіть нові дані для транспорту:\n");
                                            Console.WriteLine("\n Всі назви писати англійською :\n");
                                            Console.WriteLine("\n Нова назва товару:\n");
                                            string newName = Console.ReadLine();
                                            if (!string.IsNullOrEmpty(newName))
                                            {
                                                existingVehicle.Name = newName;
                                            }

                                            Console.WriteLine("\n Нова ціна товару(₴):\n");
                                            if (float.TryParse(Console.ReadLine(), out float newPrice) && newPrice >= 0)
                                            {
                                                existingVehicle.Price = newPrice;
                                            }

                                            Console.WriteLine("\n Нова потужність товару(W):\n");
                                            if (int.TryParse(Console.ReadLine(), out int newPower) && newPower >= 0)
                                            {
                                                existingVehicle.Power = newPower;
                                            }

                                            Console.WriteLine("\n Нова швидкість товару(km/h):\n");
                                            if (int.TryParse(Console.ReadLine(), out int newSpeed) && newSpeed >= 0)
                                            {
                                                existingVehicle.Speed = newSpeed;
                                            }

                                            Console.WriteLine("\n Нова вага товару(kg):\n");
                                            if (int.TryParse(Console.ReadLine(), out int newWeight) && newWeight >= 0)
                                            {
                                                existingVehicle.Weight = newWeight;
                                            }

                                            Console.WriteLine("\n Новий виробник товару(Id):\n");
                                            if (int.TryParse(Console.ReadLine(), out int newManufacturer_id) && newManufacturer_id >= 0)
                                            {
                                                existingVehicle.Manufacturer_id = newManufacturer_id;
                                            }

                                            Console.WriteLine("\n Новий постачальник товару(Id):\n");
                                            if (int.TryParse(Console.ReadLine(), out int newSupplier_id) && newSupplier_id >= 0)
                                            {
                                                existingVehicle.Supplier_id = newSupplier_id;
                                            }

                                            vehicleDao.Update(existingVehicle);
                                        }
                                        else
                                        {
                                            Console.WriteLine($"\n Транспорт з Id {updateId} не знайдений.\n");
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("\n Невірний Id. Введіть коректний Id.\n");
                                    }
                                    break;
                                case 5:
                                    Console.WriteLine("\n Всі назви писати англійською :\n");
                                    Console.WriteLine("\n Введіть назву товару для пошуку: \n");
                                    string searchName = Console.ReadLine();
                                    Vehicle foundVehicle = vehicleDao.GetByName(searchName);

                                    if (foundVehicle != null)
                                    {
                                        Console.WriteLine(JsonSerializer.Serialize(foundVehicle));
                                    }
                                    else
                                    {
                                        Console.WriteLine($"\nТовар з назвою '{searchName}' не знайдено.\n");
                                    }
                                    break;
                                case 6:
                                    if (caretaker._changes.Count >= 1)
                                    {
                                        Memento memento = caretaker.GetChange();
                                        Vehicle vehicleRestored = new Vehicle();
                                        vehicleRestored.Restore(memento);
                                        vehicleDao.Update(vehicleRestored);
                                        Console.WriteLine("\nДія транспортного засобу скасована.\n");
                                        Console.WriteLine($"\nВідновлений стан: Id={vehicleRestored.Id}, Name={vehicleRestored.Name}, ...");
                                    }
                                    else
                                    {
                                        Console.WriteLine("\nПомилка: Немає змінених станів для відміни.\n");
                                    }
                                    break;
                                default:
                                    Console.WriteLine("\n Невірний вибір.\n");
                                    break;
                            }
                        }
                        else
                        {
                            Console.WriteLine("\n Невірний число. Введіть число від 1 до 3.\n");
                        }
                        break;
                    case 2:
                        Console.WriteLine("\n Переглянути всіх постачальників - 1 \n Додати постачальника - 2 \n Видалити постачальника за Id - 3 " +
                           "\n Редагувати інформацію про постачальника - 4 \n Пошук за Ім'ям постачальника - 5");
                        Console.WriteLine("-------------------------------------------------------------------------");
                        if (int.TryParse(Console.ReadLine(), out choiceVariantSecond))
                        {
                            switch (choiceVariantSecond)
                            {
                                case 1:
                                    foreach (var supplier in allSuppliers)
                                        Console.WriteLine(JsonSerializer.Serialize(supplier));
                                    break;
                                case 2:
                                    Console.WriteLine("\n Всі назви писати англійською :\n");
                                    Console.WriteLine("\n Введіть ім'я постачальника:\n");
                                    string firstName = Console.ReadLine();

                                    Console.WriteLine("\n Введіть прізвище постачальника:\n");
                                    string lastName = Console.ReadLine();

                                    Console.WriteLine("\n Введіть адресу електронної пошти постачальника(gmail.com), опціональне поле :\n");
                                    string emailInput = Console.ReadLine();
                                    string email = string.IsNullOrEmpty(emailInput) ? "(не вказано)" : emailInput;

                                    Console.WriteLine("\n Введіть номер телефону постачальника(+380), опціональне поле :\n");
                                    string phoneInput = Console.ReadLine();
                                    string phone = string.IsNullOrEmpty(phoneInput) ? "(не вказано)" : phoneInput;

                                    Shop.Supplier newSupplier = new Shop.Supplier.SupplierBuilder()
                                        .SetFirstName(firstName)
                                        .SetLastName(lastName)
                                        .SetEmail(email)
                                        .SetPhone(phone)
                                        .Build();

                                    supplierDao.Add(newSupplier);
                                    break;
                                case 3:
                                    Console.WriteLine("\n Введіть Id постачальника для видалення:\n ");
                                    if (int.TryParse(Console.ReadLine(), out int deleteId))
                                    {
                                        supplierDao.Delete(deleteId);
                                    }
                                    else
                                    {
                                        Console.WriteLine("\n Некоректні дані. Введіть коректний Id постачальника для видалення.\n");
                                    }
                                    break;
                                case 4:
                                    Console.WriteLine("\n Введіть Id постачальника, якого потрібно оновити:\n");
                                    if (int.TryParse(Console.ReadLine(), out int updateId))
                                    {
                                        Shop.Supplier existingSupplier = allSuppliers.FirstOrDefault(supplier => supplier.Id == updateId);
                                        if (existingSupplier != null)
                                        {
                                            Console.WriteLine("\n Поточні дані для постачальника:\n");
                                            Console.WriteLine(JsonSerializer.Serialize(existingSupplier));

                                            Console.WriteLine("\n Введіть нові дані для постачальника:\n");
                                            Console.WriteLine("\n Всі назви писати англійською :\n");
                                            Console.WriteLine("\n Нове ім'я постачальника:\n");
                                            string newFirstName = Console.ReadLine();
                                            if (!string.IsNullOrEmpty(newFirstName))
                                            {
                                                existingSupplier.FirstName = newFirstName;
                                            }

                                            Console.WriteLine("\n Нове прізвище постачальника:\n");
                                            string newLastName = Console.ReadLine();
                                            if (!string.IsNullOrEmpty(newLastName))
                                            {
                                                existingSupplier.LastName = newLastName;
                                            }

                                            Console.WriteLine("\n Нова адреса електронної пошти постачальника (@gmail.com):\n");
                                            string newEmail = Console.ReadLine();
                                            if (!string.IsNullOrEmpty(newEmail))
                                            {
                                                existingSupplier.Email = newEmail;
                                            }

                                            Console.WriteLine("\n Новий номер телефону постачальника (+380):\n");
                                            string newPhone = Console.ReadLine();
                                            if (!string.IsNullOrEmpty(newPhone))
                                            {
                                                existingSupplier.Phone = newPhone;
                                            }

                                            supplierDao.Update(existingSupplier);

                                        }
                                        else
                                        {
                                            Console.WriteLine($"\n Постачальник з Id {updateId} не знайдений.\n");
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("\n Невірний Id. Введіть коректний Id.\n");
                                    }
                                    break;
                                case 5:
                                    Console.WriteLine("\n Всі назви писати англійською :\n");
                                    Console.WriteLine("\n Введіть Ім'я постачальника для пошуку: \n");
                                    string searchName = Console.ReadLine();
                                    Supplier foundSupplier = supplierDao.GetByName(searchName);

                                    if (foundSupplier != null)
                                    {
                                        Console.WriteLine(JsonSerializer.Serialize(foundSupplier));
                                    }
                                    else
                                    {
                                        Console.WriteLine($"\n Постачальника з Ім'ям '{searchName}' не знайдено.");
                                    }
                                    break;
                                default:
                                    Console.WriteLine("\n Невірний вибір.");
                                    break;
                            }
                        }
                        break;
                    case 3:
                        Console.WriteLine("\n Переглянути всіх виробників - 1 \n Додати виробника - 2 \n Видалити виробника за Id - 3 " +
                           "\n Редагувати інформацію про виробника - 4 \n Пошук за Ім'ям виробника - 5");
                        Console.WriteLine("-------------------------------------------------------------------------");
                        if (int.TryParse(Console.ReadLine(), out choiceVariantThird))
                        {
                            switch (choiceVariantThird)
                            {
                                case 1:
                                    foreach (var manufacturer in allManufacturers)
                                        Console.WriteLine(JsonSerializer.Serialize(manufacturer));
                                    break;
                                case 2:
                                    Console.WriteLine("\n Всі назви писати англійською :\n");
                                    Console.WriteLine("\n Введіть ім'я виробника:\n");
                                    string Name = Console.ReadLine();

                                    Shop.Manufacturer newManufacturer = new Shop.Manufacturer.ManufacturerBuilder()
                                        .SetName(Name)
                                        .Build();

                                    manufacturerDao.Add(newManufacturer);
                                    break;
                                case 3:
                                    Console.WriteLine("\n Введіть Id виробника для видалення:\n ");
                                    if (int.TryParse(Console.ReadLine(), out int deleteId))
                                    {
                                        supplierDao.Delete(deleteId);
                                    }
                                    else
                                    {
                                        Console.WriteLine("\n Некоректні дані. Введіть коректний Id виробника для видалення.\n");
                                    }
                                    break;
                                case 4:
                                    Console.WriteLine("\n Введіть Id виробника, якого потрібно оновити:\n");
                                    if (int.TryParse(Console.ReadLine(), out int updateId))
                                    {
                                        Shop.Manufacturer existingManufacturer = allManufacturers.FirstOrDefault(manufacturer => manufacturer.Id == updateId);
                                        if (existingManufacturer != null)
                                        {
                                            Console.WriteLine("\n Поточні дані для виробника:\n");
                                            Console.WriteLine(JsonSerializer.Serialize(existingManufacturer));

                                            Console.WriteLine("\n Введіть нові дані для виробника:\n");
                                            Console.WriteLine("\n Всі назви писати англійською :\n");
                                            Console.WriteLine("\n Нова назва виробника:\n");
                                            string newManufacturerName = Console.ReadLine();
                                            if (!string.IsNullOrEmpty(newManufacturerName))
                                            {
                                                existingManufacturer.Name = newManufacturerName;
                                            }

                                            manufacturerDao.Update(existingManufacturer);

                                        }
                                        else
                                        {
                                            Console.WriteLine($"\n Виробник з Id {updateId} не знайдений.\n");
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("\n Невірний Id. Введіть коректний Id.\n");
                                    }
                                    break;
                                case 5:
                                    Console.WriteLine("\n Всі назви писати англійською :\n");
                                    Console.WriteLine("\n Введіть Ім'я виробника для пошуку: \n");
                                    string searchName = Console.ReadLine();
                                    Manufacturer foundManufacturer = manufacturerDao.GetByName(searchName);

                                    if (foundManufacturer != null)
                                    {
                                        Console.WriteLine(JsonSerializer.Serialize(foundManufacturer));
                                    }
                                    else
                                    {
                                        Console.WriteLine($"\n Виробник з Ім'ям '{searchName}' не знайдено.");
                                    }
                                    break;
                                default:
                                    Console.WriteLine("\n Невірний вибір.");
                                    break;
                            }
                        }
                        break;
                    case 4:
                        Console.WriteLine("\n Переглянути всіх користувачів - 1 \n Видалити користувача за Id - 2 " +
                           "\n Редагувати інформацію про користувача - 3 \n Зареєструвати нового користувача - 4 \n");
                        Console.WriteLine("-------------------------------------------------------------------------");
                        if (int.TryParse(Console.ReadLine(), out choiceVariantFourth))
                        {
                            switch (choiceVariantFourth)
                            {
                                case 1:
                                    foreach (var users in allUsers)
                                        Console.WriteLine(JsonSerializer.Serialize(users));
                                    break;
                                case 2:
                                    Console.WriteLine("\n Введіть Id користувача для видалення:\n ");
                                    if (int.TryParse(Console.ReadLine(), out int deleteId))
                                    {
                                        usersDao.Delete(deleteId);
                                    }
                                    else
                                    {
                                        Console.WriteLine("\n Некоректні дані. Введіть коректний Id користувача для видалення.\n");
                                    }
                                    break;
                                case 3:
                                    Console.WriteLine("\n Введіть Id користувача, якого потрібно оновити:\n");
                                    if (int.TryParse(Console.ReadLine(), out int updateId))
                                    {
                                        Shop.Users existingUser = allUsers.FirstOrDefault(user => user.Id == updateId);

                                        if (existingUser != null)
                                        {
                                            Console.WriteLine("\n Поточні дані для користувача:\n");
                                            Console.WriteLine(JsonSerializer.Serialize(existingUser));

                                            Console.WriteLine("\n Введіть нові дані для користувача:\n");
                                            Console.WriteLine("\n Всі назви писати англійською :\n");

                                            Console.Write("\n Новий пароль користувача (натисніть Enter, щоб залишити без змін): ");
                                            string newUserPassword = Console.ReadLine();
                                            if (!string.IsNullOrEmpty(newUserPassword))
                                            {
                                                existingUser.Password = newUserPassword;
                                            }

                                            Console.Write("\n Новий логін користувача (натисніть Enter, щоб залишити без змін): ");
                                            string newUserLogin = Console.ReadLine();
                                            if (!string.IsNullOrEmpty(newUserLogin))
                                            {
                                                existingUser.Login = newUserLogin;
                                            }

                                            Console.Write("\n Нова роль користувача (натисніть Enter, щоб залишити без змін): ");
                                            string newUserRoleInput = Console.ReadLine();

                                            if (!string.IsNullOrEmpty(newUserRoleInput) && int.TryParse(newUserRoleInput, out int newUserRole))
                                            {
                                                Roles role = rolesDao.GetById(newUserRole);

                                                if (role != null)
                                                {
                                                    existingUser.RolesId = newUserRole;
                                                }
                                                else
                                                {
                                                    Console.WriteLine("\n Роль із зазначеним ідентифікатором не існує. Роль залишена без змін.");
                                                }
                                            }
                                            else
                                            {
                                                Console.WriteLine("\n Невірний ввід для ролі. Роль залишена без змін.");
                                            }
                                            usersDao.Update(existingUser);
                                        }
                                        else
                                        {
                                            Console.WriteLine($"\n Користувач з Id {updateId} не знайдений.");
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("\n Невірний Id. Введіть коректний Id.\n");
                                    }
                                    break;
                                case 4:
                                    Register(usersDao);
                                    break;
                                default:
                                    Console.WriteLine("\n Невірний вибір.");
                                    break;
                            }
                        }
                        break;
                    default:
                        Console.WriteLine("\n Невірний вибір.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("\n Невірний число. Введіть число від 1 до 4.");
            }
        }
        while (true);
    }
    static Users Login(IDAO<Users> usersDao)
    {
        Users loggedInUser = null;

        do
        {
            Console.WriteLine("\n Введіть дані для входу (всі назви писати на англ):\n ");

            Console.Write("\n Логін: ");
            string login = Console.ReadLine();

            Console.Write("\n Пароль: ");
            string password = Console.ReadLine();

            loggedInUser = usersDao.GetByLogin(login);

            if (loggedInUser != null && loggedInUser.Password == password)
            {
                Console.WriteLine($"\n Ви успішно увійшли як {loggedInUser.Login}.\n");
                return loggedInUser;
            }
            else
            {
                Console.WriteLine("\n Невірний логін або пароль. Спробуйте ще раз.\n");
            }
        } while (true);
    }

    static void Register(IDAO<Users> usersDao)
    {
        Console.WriteLine("\n Введіть дані для реєстрації (всі назви писати на англ):\n ");

        Console.Write("\n Логін: ");
        string login = Console.ReadLine();

        Console.Write("\n Пароль: ");
        string password = Console.ReadLine();

        Console.Write("\n Роль (Admin - 1/User - 2): ");
        int roleId = Convert.ToInt32(Console.ReadLine());

        Users newUser = new Users
        {
            Login = login,
            Password = password,
            RolesId = roleId
        };

        usersDao.Add(newUser);

        Console.WriteLine($"\n Користувач {newUser.Login} успішно зареєстрований з роллю ідентифікатором {newUser.RolesId}.\n");
    }
    static int GetCurrentLoggedInUserRolesId(IDAO<Users> usersDao)
    {
        Users loggedInUser = Login(usersDao);

        if (loggedInUser != null)
        {
            return loggedInUser.RolesId;
        }
        else 
        {
            Console.WriteLine("Eror");
            return -1;
        }
    }
}
