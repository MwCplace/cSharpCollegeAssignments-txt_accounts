using static System.Console;

class Files
{
    private static string path = null;// путь к файлу будет храниться тут

    private static string tryAgainText = ". Попробуйте еще раз\n";
    private static string[] rules = {
        "Во время любого ввода вы можете ввести число \"0\" и программа завершится",
        "Во время любого ввода вы можете ввести слово \"п\" и будут показаны правила программы",
        "При завершении ввода данных нажимайте клавишу Enter",
        "При сообщении \"Введите: да/нет\" если вы согласны, введите слово \"да\", иначе введите любое другое слово или символ",
        "Программа не может хранить более 1-ого текста, имейте это ввиду"
    };

    // other
    // проверяет на ключевые слова и позволяет избежать ввода
    public static bool CheckForKeywords(string text)
    {
        if (text == "0")
        {
            Environment.Exit(0);
        }
        else if (text == "м")
        {
            Menu();
        }
        else if (text == "п")
        {
            TitleMsg("\nПРАВИЛА\n");
            foreach (string i in rules) // вывести все правила в консоль
            {
                WriteLine("\t" + i);
            }
            Menu();
        }
        else if (text == "")
        {
            ErrorMsg("Вы ничего не ввели!" + tryAgainText + "\n\n");
            Menu();
        }
        else if (text == " ")
        {
            ErrorMsg("Такой опции нет!" + tryAgainText + "\n\n");
            Menu();
        }
        else
        {
            return false;
        }
        return true;
    }

    public static void ErrorMsg(string err)
    {
        ForegroundColor = ConsoleColor.Red;
        Write(err);
        ForegroundColor = ConsoleColor.Gray;
    }

    public static void WarningMsg(string err)
    {
        ForegroundColor = ConsoleColor.Yellow;
        Write(err);
        ForegroundColor = ConsoleColor.Gray;
    }

    public static void ResultMsg(string text)
    {
        ForegroundColor = ConsoleColor.Green;
        Write(text);
        ForegroundColor = ConsoleColor.Gray;
    }

    public static void TitleMsg(string text)
    {
        ForegroundColor = ConsoleColor.Blue;
        Write(text);
        ForegroundColor = ConsoleColor.Gray;
    }

    private static int GetOption()
    {
        string str = ReadLine();

        CheckForKeywords(str);

        int num = 0;
        try
        {
            num = Convert.ToInt32(str);
        }
        catch (Exception ex) // проверка ввода (если не число, то вывести ошибку и попросить ввести данные снова
        {
            if (ex is FormatException)
            {
                ErrorMsg("Введено не число" + tryAgainText + "\n");
                Menu();
            }
        }

        if (num < 0) // проверка ввода (если число не положительное, то вывести ошибку и попросить ввести данные снова
        {
            ErrorMsg("Число должно быть положительным" + tryAgainText + "\n");
            Menu();
        }

        return num;
    }

    private static string CheckPath(string path)
    {
        try
        {
            FileInfo fileInfo = new FileInfo(path);
            if (!fileInfo.Exists)
            {
                return null;
            }
        }
        catch
        {
            return null;
        }

        return path;
    }

    //1
    private static void UserDialogWrap1()
    {
        // получить первый путь
        WriteLine("\n\nВведите путь к файлу (откуда скопировать): ");
        string pathFrom = ReadLine();
        CheckForKeywords(pathFrom);
        pathFrom = CheckPath(pathFrom);

        if (pathFrom != null)
        {
            //FileExistanceCheck(pathFrom);
            // получить второй путь
            WriteLine("Введите путь к файлу (куда скопировать): ");
            string pathTo = ReadLine();
            CheckForKeywords(pathTo);
            pathTo = CheckPath(pathTo);

            if (pathTo != null)
            {
                //FileExistanceCheck(pathTo);
                // скопировать текст из файлаОт в файлКуда
                string copyingResult = CopyFile(GetString(pathFrom), pathTo);
                if (copyingResult != null)
                {
                    ResultMsg($"Копирование прошло успешно!");
                }
                else
                {
                    ErrorMsg($"Пути {pathTo} не существует");
                }
            }
            else
            {
                ErrorMsg("Пути не существует" + tryAgainText);
            }
        }
        else
        {
            ErrorMsg("Пути не существует" + tryAgainText);
        }
    }

    private static string CopyFile(string text, string pathTo)
    {
        try
        {
            File.WriteAllText(pathTo, text);
        }
        catch
        {
            return null;
        }

        return "";
    }

    private static string GetString(string path) // получить содержимое файла
    {
        try
        {
            string[] lines = File.ReadAllLines(path);
            string str = null;

            int count = 0;

            foreach (string line in lines)
            {
                if (count == lines.Length - 1) // убираю лишний (последний) перенос строки
                {
                    str += line;
                }
                else
                {
                    str += line + "\n";
                }

                count++;
            }

            return str;
        }
        catch (FileNotFoundException)
        {
            return null;
        }
    }

    //2 (получить инфо о кол-ве паролей)
    private static void UserDialogWrap2()
    {
        Write("Укажите путь к файлу: ");

        string path = ReadLine();
        CheckForKeywords(path);
        path = CheckPath(path);

        ResultMsg("Кол-во паролей: " + GetNumberOfPasswords(path) + "\n");
    }

    private static int GetNumberOfPasswords(string path)
    {
        string[] lines = GetString(path).Split(); // разделить слова в тексте через пробел
        return lines.Length / 2;
    }

    //3
    private static void UserDialogWrap3()
    {
        /* Общий план
        
            ask the user for path
            1 check for login duplicates, 2 show them to the user and 3 dont let the user to do anything till they fix these duplicates - Login change Menu - 3
            1 check for password duplicates and 2 ask the user if they want to fix those duplicates
                                                    |- if dont - New account creation Menu
                                                    |- if do - Password change Menu
            1 check for password duplicates again 2 ask the user if they want to continue
                                                    |- if do - Password change Menu
                                                    |- if dont - ask the user if they want to continue
                                                        |- if do - New account creation Menu
                                                        |- if dont - Main Menu

        -- Login change Menu --
            1 ask for login
            2 check the login for duplicates
                |- while has - show warning and return to 1
            write new login
            save this change
        -- Password change Menu --
            1 ask for password
            2 check the login for duplicates
                |- while has - show warning and return to 1
            write new password
            save this change
        */

        /* Детальный план
        
            returns null - minimal: Display text error to the user and return to Main Menu

        --- SECTION 1 ---
            func CheckForKeywords()
                |- checks for keywords like 0 (exit) and menu (fo to Main Menu), etc.
            Output - Dear User, give as a file path
            func GetPath()
                |- gets string from console
                |- CheckForKeywords()
                |- checks for exeptions
                    |- if none: returns a string with path
                    |- else: returns null
            IDEALY: we'he got a path

        --- SECTION 2 ---
            func GetTextAsLines(path)
            |- checks for exeptions
                |- if none: returns lines (array)
                |- else: returns null
            func GetLogins(file)
            |- checks for exeptions
                |- if none: returns logins (array)
                |- else: returns null
            func GetDuplicates(logins)
            |- checks for duplicates
                |- if none: returns login duplicates indexes (array)
                |- else: returns empty array
            while (login duplicates exist)
            |- Output - Dear User, we can't continue. Please get rid of login duplicates and restart the program or let's continue and get rid of duplicates in this program. Continue?
                |- if dont: return to Main Menu
                |- if do: 
                    |- ChangeDuplicates(fileLines, loginDuplicatesIndexes) returns string
                    |- ChangeFileText(filePath, text)
                        |- checks for exeptions
                                |- if none: returns empty string
                                |- else: returns null
            
            func GetPasswords(file)
            |- checks for exeptions
                |- if none: returns passwords (array)
                |- else: returns null
            func GetDuplicates(passwords)
            |- checks for duplicates
                |- if none: returns password duplicates indexes (array)
                |- else: returns empty array
            while (passwords duplicates exist)
            |- Output - Dear User, do u want to change password duplicates?
                |- if do: 
                    |- ChangeDuplicates(fileLines, loginDuplicatesIndexes) returns string
                    |- ChangeFileText(filePath, text)
                        |- checks for exeptions
                                |- if none: returns empty string
                                |- else: returns null
                |- if dont: 
                    |- func CreateNewAccount()
                    |- ChangeFileText(filePath, text)
                    |- Output - Dear User, do u want to continue?
                        |- if do: func CreateNewAccount()
                        |- else: return to Main Menu
        */

        while (true) // пока не возникнет исключение - продолжать работу
        {
            path = "E:\\c sharp for uni\\чистовики (код) ФИНАЛ\\СИ ШАРП 2-ой семестр\\Assignment 2\\sampleFolder\\subFolder2\\accounts2.txt";
            WriteLine("\nВведите путь к файлу: ");
            string pathFrom = ReadLine();
            CheckForKeywords(pathFrom);
            pathFrom = CheckPath(pathFrom);

            if (pathFrom == null)
            {
                ErrorMsg("Пути не существует" + tryAgainText);
                break;
            }

            string[] accounts = GetArrayOfAccounts(path);
            if (!FileHasValidFormatting(accounts))
            {
                ErrorMsg("Формат акканутов в файле не соответствует виду\nлогин + отступ + пароль + отступ");
                break;
            }

            string[] logins = GetArrayOfLoginsOrPasswords(accounts, true);
            string[] passwords = GetArrayOfLoginsOrPasswords(accounts, false);
            HashSet<int> duplicatesIndexes = GetDuplicates(logins);

            if (duplicatesIndexes.Count > 0)
            {
                ErrorMsg("Аккаунты в файле имеют общие логины в кол-ве: " + duplicatesIndexes.Count/2 + "\nПожалуйста замените их");
                break;
            }

            ResultMsg("Файл принят!\n");

            duplicatesIndexes = GetDuplicates(passwords);
            if (duplicatesIndexes.Count > 0)
            {
                ErrorMsg("Аккаунты в файле имеют общие пароли. Заменить? да/нет\n");
                string option = ReadLine();
                if (option == "да")
                {
                    DuplicateChangeMenu(path, accounts, GetDuplicates(passwords), logins, passwords);
                }
            }

            CreateNewAccountMenu(accounts, logins);
            break;
        }
    }

    // получить список логинов И паролей
    private static string[] GetArrayOfAccounts(string path)
    {
        string text = GetString(path); // получить текст из файла в виде строки
        string[] lines = text.Split();// разделить слова в тексте через пробел
        return lines;
    }

    // проверка текста файла на соответствие формату логин + пароль
    private static bool FileHasValidFormatting(string[] file)
    {
        if ((file.Length) % 2 == 0) // если в файле всё ок (четное количество слов)
        {
            foreach (string line in file)
            {
                if (line.Length == 0) { return false; } // если строка окажется пустой
            }
            return true;
        }

        return false;
    }

    // получить список логинов ИЛИ паролей
    private static string[] GetArrayOfLoginsOrPasswords(string[] accounts, bool logins)
    {
        string[] result = new string[accounts.Length / 2];
        int count = 0;

        if (logins)
        {
            foreach (string line in accounts) // для каждой строки в массиве
            {
                if (count % 2 == 0)
                {
                    result[count / 2] = line; // аккаунты стоят на четных позициях
                }
                count++; // счетчик +1
            }
        }
        else
        {
            foreach (string line in accounts) // для каждой строки в массиве
            {
                if (count % 2 == 1)
                {
                    result[count / 2] = line; // пароли стоят на нечетных позициях
                }
                count++; // счетчик +1
            }
        }
        return result;
    }

    // возвращает индексы дубликатов
    private static HashSet<int> GetDuplicates(string[] lines)
    {
        HashSet<int> duplicatesIndexes = new HashSet<int>(); // только уникальные индексы

        for (int i = 0; i < lines.Length; i++)
        {
            for (int j = i + 1; j < lines.Length; j++)
            {
                if (lines[i] == lines[j]) // если слова равны
                {
                    if (!duplicatesIndexes.Contains(i)) // если таких нет еще нет в списке индексов дубликатов, то
                    {
                        duplicatesIndexes.Add(i); // добавить
                    }
                    if (!duplicatesIndexes.Contains(j))
                    {
                        duplicatesIndexes.Add(j);
                    }
                }
            }
        }

        return duplicatesIndexes;
    }

    // показывает дубликаты
    public static void OutputDuplicates(HashSet<int> duplicatesIndexes, string[] nonDuplicates, string[] duplicates)
    {
        foreach (int line in duplicatesIndexes)
        {
            WriteLine($"___ На строке {line+1} ___");
            WarningMsg("\t" + nonDuplicates[line] + " ");
            ErrorMsg(duplicates[line] + "\n");
            //Write("\t" + nonDuplicates[line[1]] + " ");
            //ErrorMsg(duplicates[line[1]] + "\n\n");
        }
    }
    // меню для изменения дубликатов
    public static void DuplicateChangeMenu(string path, string[] accounts, HashSet<int> duplicatesIndexes, string[] nonDuplicates, string[] duplicates)
    {
        TitleMsg("\nМЕНЮ ИЗМЕНЕНИЯ ДУБЛИКАТОВ\n");
        OutputDuplicates(duplicatesIndexes, nonDuplicates, duplicates);
        // пока не является действенным индексом - просить ввести другой индекс
        int index = GetOption() - 1; // выбор пары
        while (!duplicatesIndexes.Contains(index))
        {
            ErrorMsg("Данный индекс недоступен. Попробуйте еще раз\n");
            index = GetOption() - 1; // замена
        }

        index = index * 2 + 1;
        string change = ReadLine(); // замена
        // пока является дубликатом - просить ввести другую замену
        while(isDuplicate(change, duplicates))
        {
            ErrorMsg("Такой пароль уже существует. Попробуйте еще раз\n");
            change = ReadLine(); // замена
        }

        ChangeDuplcate(path, accounts, index, change);
    }

    public static bool isDuplicate(string change, string[] lines)
    {
        for (int i = 0; i < lines.Length; i++)
        {
            if (change == lines[i])
            {
                return true;
            }
        }
        return false;
    }

    // заменить текст
    public static void ChangeDuplcate(string path, string[] accounts, int index, string change)
    {
        accounts[index] = change;
        CopyFile(Join(accounts), path);
    }

    public static string Join(string[] lines)
    {
        string s = "";
        for (int i = 0; i < lines.Length; i++)
        {
            s += lines[i];

            if (i % 2 == 1 || i < lines.Length-1) // если индекс нечетный, это значит что слово является паролем и после него должена последовать новая строка. А также не ставить перенос строки после последнего элемента
            {
                s += "\n";
            }
            else // если индекс четный, это значит что слово является логином и после него должен стоять пробел
            {
                s += " ";
            }
        }
        return s;
    }

    public static bool LoginHasDuplicate(string login, string[] logins)
    {
        bool HasDuplicates = false;

        for (int i = 0; i < logins.Length; i++)
        {
            if (login == logins[i]) // если строки равны
            {
                HasDuplicates = true;
                break;
            }
        }

        return HasDuplicates;
    }

    public static void CreateNewAccountMenu(string[] accounts, string[] logins)
    {
        TitleMsg("\nМЕНЮ СОЗДАНИЯ НОВОГО АККАУНТА\n");

        string newAccount = null;
        string optionStr = "да";
        int count = 0;

        while (optionStr == "да")
        {
            Write("Введите логин: ");
            string str1 = ReadLine();
            if (CheckForKeywords(str1))
            {
                break;
            }

            // проверка на то, имеет ли новый логин дубликаты
            while (LoginHasDuplicate(str1, logins)) // если да, то вывести ошибку и отправить пользователя в меню
            {
                ErrorMsg("Такой логин уже существует" + tryAgainText + "\n");
                Write("Введите логин: ");
                str1 = ReadLine();
                if (CheckForKeywords(str1))
                {
                    break;
                }
            }

            Write("Введите пароль: ");
            string str2 = ReadLine();
            if (CheckForKeywords(str2))
            {
                break;
            }

            newAccount += "\n" + str1 + " " + str2;
            File.AppendAllText(path, newAccount);
            ResultMsg("Новый аккаунт добавлен!\n");

            // подтвердить, что пользовать хочет продолжать добавлять
            WriteLine("Продолжить? да/нет");
            optionStr = ReadLine();
            CheckForKeywords(optionStr);

            count++;
        }
    }
//E:\\c sharp for uni\\чистовики (код) ФИНАЛ\\СИ ШАРП 2-ой семестр\\Assignment 2\\sampleFolder\\subFolder2\\accounts2.txt
    public static void Menu()
    {
        string[] exercises =
        {
            "Скопировать содержимое файла в другой файл", // 1
            "Вывести количество паролей", // 2
            "Записать в файл новый логин и пароль" // 3
        };

        int option = 1; // выбор пользователя

        while (option != 0)
        {
            TitleMsg("\n\nГЛАВНОЕ МЕНЮ");
            WriteLine("\nВыберите опцию:");
            WriteLine("\t0 - Завершить программу");
            for (int i = 0; i < exercises.Length; i++)
            {
                WriteLine($"\t{i + 1} - {exercises[i]}");
            }
            Write("Ваш выбор: ");
            option = GetOption(); // выбор пользователя меняется и теперь от него зависят дальнейшие действия программы

            if (option != 0)
            { // если выбрана опция 0, то код просто заканчивается
                switch (option)
                {
                    case 1:
                        UserDialogWrap1();
                        break;

                    case 2:
                        UserDialogWrap2();
                        break;

                    case 3:
                        UserDialogWrap3();
                        break;

                    default:
                        ErrorMsg("Такой опции нет" + tryAgainText + "\n");
                        break;
                }
            }
        }
        Menu(); // recursion
    }

    static void Main()
    {
        WriteLine("Практикум по программированию. Практическая 2, задание 1. Работа с файлами");
        CheckForKeywords("п");
    }
}