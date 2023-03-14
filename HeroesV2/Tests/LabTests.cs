using HeroDB;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HeroesV2.Tests
{
    internal class LabTests
    {
        public static void RunTests()
        {
            Console.Clear();
            RunPartATests();
            RunPartBTests();
            RunPartCTests();

            Console.WriteLine("\nNOTE: Further review of the code is needed to determine the final grade.");
        }

        #region Tests Helpers
        enum Criteria
        {
            NoAttempt = 0,      //0%      F
            Insufficient = 59,  //1-59%   F
            Beginning = 69,     //60-69%  D
            Developing = 79,    //70-79%  C
            Effective = 89,     //80-89%  B
            Exemplary           //90-100% A
        }


        private static void ShowResult(string part, Criteria testCriteria, string message)
        {
            ConsoleColor back = ConsoleColor.Green;
            string title = string.Empty;
            switch (testCriteria)
            {
                case Criteria.NoAttempt:
                    title = "No Attempt 0%";
                    back = ConsoleColor.Red;
                    break;
                case Criteria.Insufficient:
                    title = "Insufficient 1-59%";
                    back = ConsoleColor.Red;
                    break;
                case Criteria.Beginning:
                    title = "Beginning 60-69%";
                    back = ConsoleColor.Yellow;
                    break;
                case Criteria.Developing:
                    title = "Developing 70-79%";
                    back = ConsoleColor.Gray;
                    break;
                case Criteria.Effective:
                    title = "Effective 80-89%";
                    back = ConsoleColor.Blue;
                    break;
                case Criteria.Exemplary:
                    title = "Exemplary 90-100%";
                    back = ConsoleColor.Green;
                    break;
                default:
                    break;
            }
            Console.BackgroundColor = back;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write($"{part,30} | {title,20} | ");
            int width = Console.WindowWidth - Console.CursorLeft - 1;
            Console.WriteLine($"{message.PadRight(width)}");

            Console.ResetColor();
        }

        enum MethodProblems
        {
            None,
            NotFound,
            ReturnType,
            Parameters
        }

        enum MethodParts
        {
            Name,
            ReturnType,
            Parameters,
            Execution
        }

        static Dictionary<MethodParts, string> GetInitialMethodResults(string methodName)
        {
            return new Dictionary<MethodParts, string>()
            {
                {MethodParts.Name, $"The {methodName} method does not appear to exist or is not public." },
                {MethodParts.ReturnType, "<not available>" },
                {MethodParts.Parameters, "<not available>" },
                {MethodParts.Execution, "<not available>" }
            };
        }

        private static void ShowMethodResult(string part, Criteria testCriteria, Dictionary<MethodParts, string> results)
        {
            Console.WriteLine();
            ConsoleColor back = ConsoleColor.Green;
            string title = string.Empty;
            switch (testCriteria)
            {
                case Criteria.NoAttempt:
                    title = "No Attempt 0%";
                    back = ConsoleColor.Red;
                    break;
                case Criteria.Insufficient:
                    title = "Insufficient 1-59%";
                    back = ConsoleColor.Red;
                    break;
                case Criteria.Beginning:
                    title = "Beginning 60-69%";
                    back = ConsoleColor.Yellow;
                    break;
                case Criteria.Developing:
                    title = "Developing 70-79%";
                    back = ConsoleColor.Gray;
                    break;
                case Criteria.Effective:
                    title = "Effective 80-89%";
                    back = ConsoleColor.Blue;
                    break;
                case Criteria.Exemplary:
                    title = "Exemplary 90-100%";
                    back = ConsoleColor.Green;
                    break;
                default:
                    break;
            }
            Console.BackgroundColor = back;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine($"{part,-30} | {title,-20}");
            foreach (var result in results)
            {
                Console.WriteLine($"{string.Empty,10}{result.Key,-20}   {result.Value,-20}");
                //Console.Write(result.Key);
                //Console.CursorLeft = 20;
                //Console.WriteLine(result.Value);
            }
            Console.ResetColor();
        }

        private static MethodProblems EvaluateMethod(string methodName, MethodInfo method, Type expectedReturnType, List<Type> parameterList, ref string message, out Dictionary<MethodParts, string> methodResults)
        {
            MethodProblems problem = MethodProblems.None;
            methodResults = GetInitialMethodResults(methodName);
            //Type heroDBType = typeof(HeroesDB);
            //var method = heroDBType.GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            //var methods = heroDBType.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            //foreach (var mtd in methods)
            //{
            //    Console.WriteLine($"Method: {mtd.Name}");
            //}
            if (method == null)
            {
                problem = MethodProblems.NotFound;
                message = $"The {methodName} method does not appear to exist or is not public.";
            }
            else
            {
                methodResults[MethodParts.Name] = "GOOD";
                if (expectedReturnType != method.ReturnType)
                {
                    problem = MethodProblems.ReturnType;
                    message = $"Return type for {methodName} should be {expectedReturnType}.";
                    methodResults[MethodParts.ReturnType] = message;
                }
                else
                {
                    methodResults[MethodParts.ReturnType] = "GOOD";
                    var parameters = method.GetParameters();
                    message = "GOOD";
                    if (parameters.Length != parameterList.Count)
                    {
                        problem = MethodProblems.Parameters;
                        message = $"Not the correct number of parameters for {methodName}. Expected: {parameterList.Count}. Actual: {parameters.Length}.";
                    }
                    else
                    {
                        for (int i = 0; i < parameters.Length; i++)
                        {
                            if (parameters[i].ParameterType != parameterList[i])
                            {
                                problem = MethodProblems.Parameters;
                                message = $"{methodName} parameter #{i + 1} is the wrong type. Expected: {parameterList[i]}. Actual: {parameters[i].ParameterType}";
                                break;
                            }
                        }
                    }
                    methodResults[MethodParts.Parameters] = message;
                }
            }
            if (problem == MethodProblems.None)
                message = $"{methodName} method signature is correct.";

            return problem;
        }

        private static string GetTestDataPath(string subfolder, string fileName)
        {
            string path = (string.IsNullOrEmpty(subfolder)) ? Path.Combine(System.IO.Directory.GetCurrentDirectory(), fileName) :
                Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Tests", fileName);
            return path;
        }

        private static List<Hero> LoadTestData(string subfolder, string fileName)
        {
            List<Hero> heroes;
            string jsonText = File.ReadAllText(GetTestDataPath(subfolder, fileName));
            try
            {
                heroes = JsonConvert.DeserializeObject<List<Hero>>(jsonText) ?? new List<Hero>();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                heroes = new List<Hero>();
            }
            return heroes;
        }
        #endregion

        #region Part A Tests
        private static void RunPartATests()
        {
            //PartA_1_MergeTest();
            PartA_1_MergeSortTest();
            PartA_2_SortByAttributeTest();
            PartA_3_BinarySearchTest();
            PartA_4_FindHeroTest();
        }
        private static void PartA_1_MergeTest()
        {
            HeroesDB.LoadHeroes();

            Criteria testCriteria = Criteria.NoAttempt;
            string message = string.Empty;
            string methodName = "Merge";

            //check Merge method
            Type heroDBType = typeof(HeroesDB);
            var method = heroDBType.GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            List<Type> expectedParams = new List<Type>() { typeof(List<Hero>), typeof(List<Hero>), typeof(SortBy) };
            MethodProblems methodProblems = EvaluateMethod(methodName, method, typeof(List<Hero>), expectedParams, ref message, out Dictionary<MethodParts, string> methodResults);
            if (methodProblems == MethodProblems.None)
            {
                //
                // Does the method work correctly
                //
                //PARAMS: List1, List2, SortBy
                //RETURNS: sorted List by the attribute
                //load the lists from a json file
                List<Hero> list1 = LoadTestData("Tests", "mergeList1.json");
                List<Hero> list2 = LoadTestData("Tests", "mergeList2.json");
                List<Hero> correctList = LoadTestData("Tests", "mergedList.json");
                SortBy sortBy = SortBy.Strength;
                List<Hero> mergedHeroes = method.Invoke(null, new object[] { list1, list2, sortBy }) as List<Hero>;
                if (mergedHeroes == null)
                {
                    testCriteria = Criteria.Developing;
                    message = $"The {methodName} method did not return a Hero object; it returned null.";
                }
                else
                {
                    //
                    // check that the returned list has the heroes in the correct order (CorrectResult)
                    if (!correctList.SequenceEqual(mergedHeroes))
                    {
                        testCriteria = Criteria.Effective;
                        message = $"The returned list from {methodName} does not contain the correct data.";
                    }
                    else
                    {
                        testCriteria = Criteria.Exemplary;
                        message = "GOOD";// $"{methodName} had the correct result. ";
                    }
                }
                methodResults[MethodParts.Execution] = message;
            }
            else
            {
                switch (methodProblems)
                {
                    case MethodProblems.NotFound:
                        testCriteria = Criteria.NoAttempt;
                        break;
                    case MethodProblems.ReturnType:
                    case MethodProblems.Parameters:
                        testCriteria = Criteria.Insufficient;
                        break;
                    default:
                        break;
                }
            }
            ShowMethodResult($"Part A-1: {methodName}", testCriteria, methodResults);
        }
        private static void PartA_1_MergeSortTest()
        {
            Criteria testCriteria = Criteria.NoAttempt;
            string message = string.Empty;
            string methodName = "MergeSort";

            //check MergeSort method
            Type heroDBType = typeof(HeroesDB);
            Dictionary<MethodParts, string> methodResults;
            try
            {
                var method = heroDBType.GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
                List<Type> expectedParams = new List<Type>() { typeof(List<Hero>), typeof(SortBy) };
                MethodProblems methodProblems = EvaluateMethod(methodName, method, typeof(List<Hero>), expectedParams, ref message, out methodResults);
                if (methodProblems == MethodProblems.None)
                {
                    //
                    // Does the method work correctly
                    //
                    //PARAMS: List1, SortBy
                    //RETURNS: sorted List by the attribute
                    //load the lists from a json file
                    List<Hero> list1 = LoadTestData("Tests", "shortList.json");
                    List<Hero> correctList = LoadTestData("Tests", "mergedList.json");
                    SortBy sortBy = SortBy.Strength;

                    List<Hero> mergedHeroes = method.Invoke(null, new object[] { list1, sortBy }) as List<Hero>;
                    //List<Hero> mergedHeroes = HeroesDB.MergeSort(list1, sortBy);
                    if (mergedHeroes == null)
                    {
                        testCriteria = Criteria.Developing;
                        message = $"The {methodName} method did not return a Hero object; it returned null.";
                    }
                    else
                    {
                        //
                        // check that the returned list has the heroes in the correct order (CorrectResult)
                        //for (int i = 0; i < correctList.Count; i++)
                        //{
                        //    Console.WriteLine($"{correctList[i].Name,20} {mergedHeroes[i].Name}");
                        //}
                        if (!correctList.SequenceEqual(mergedHeroes))
                        {
                            testCriteria = Criteria.Effective;
                            message = $"The returned list from {methodName} does not contain the correct data.";
                            message += "\nYOUR DATA              CORRECT DATA";
                            string empty = new string(' ', 23);
                            int maxCount = Math.Max(correctList.Count, mergedHeroes.Count);
                            for (int i = 0; i < maxCount; i++)
                            {
                                message += "\n";
                                if (i < mergedHeroes.Count)
                                    message += $"{mergedHeroes[i].Name,-23}";
                                else
                                    message += empty;

                                if (i < correctList.Count)
                                    message += correctList[i].Name;
                            }
                        }
                        else
                        {
                            testCriteria = Criteria.Exemplary;
                            message = "GOOD";// $"{methodName} had the correct result. ";
                        }
                    }
                    methodResults[MethodParts.Execution] = message;
                }
                else
                {
                    switch (methodProblems)
                    {
                        case MethodProblems.NotFound:
                            testCriteria = Criteria.NoAttempt;
                            break;
                        case MethodProblems.ReturnType:
                        case MethodProblems.Parameters:
                            testCriteria = Criteria.Insufficient;
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (AmbiguousMatchException am)
            {
                methodResults = GetInitialMethodResults(methodName);
                testCriteria = Criteria.Beginning;
                message = $"You should only have 1 {methodName} method.";
                methodResults[MethodParts.Name] = message;
            }
            catch (Exception ex)
            {
                methodResults = GetInitialMethodResults(methodName);
                testCriteria = Criteria.Insufficient;
                message = $"EXCEPTION: {ex.Message}";
                methodResults[MethodParts.Name] = message;
            }
            ShowMethodResult($"Part A-1: {methodName}", testCriteria, methodResults);
        }
        private static void PartA_2_SortByAttributeTest()
        {
            HeroesDB.LoadHeroes();

            Criteria testCriteria = Criteria.NoAttempt;
            string message = string.Empty;
            string methodName = "SortByAttribute";

            Type heroDBType = typeof(HeroesDB);
            var method = heroDBType.GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            List<Type> expectedParams = new List<Type>() { typeof(SortBy) };
            MethodProblems methodProblems = EvaluateMethod(methodName, method, typeof(void), expectedParams, ref message, out Dictionary<MethodParts, string> methodResults);

            switch (methodProblems)
            {
                case MethodProblems.None:
                    testCriteria = Criteria.Exemplary;
                    break;
                case MethodProblems.NotFound:
                    testCriteria = Criteria.NoAttempt;
                    break;
                case MethodProblems.ReturnType:
                case MethodProblems.Parameters:
                    testCriteria = Criteria.Insufficient;
                    break;
                default:
                    break;
            }
            ShowMethodResult($"Part A-2: {methodName}", testCriteria, methodResults);
        }
        private static void PartA_3_BinarySearchTest()
        {
            HeroesDB.LoadHeroes();

            Criteria testCriteria = Criteria.NoAttempt;
            string message = string.Empty;
            string methodName = "BinarySearch";

            Dictionary<MethodParts, string> methodResults;
            try
            {
                Type heroDBType = typeof(HeroesDB);
                var method = heroDBType.GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
                List<Type> expectedParams = new List<Type>() { typeof(List<Hero>), typeof(string), typeof(int), typeof(int) };
                MethodProblems methodProblems = EvaluateMethod(methodName, method, typeof(int), expectedParams, ref message, out methodResults);
                if (methodProblems == MethodProblems.None)
                {
                    List<Hero> list1 = LoadTestData(string.Empty, "heroes.json");
                    int index = (int)method.Invoke(null, new object[] { list1, "Batman", 0, list1.Count - 1 });
                    if (index == 51)
                    {
                        testCriteria = Criteria.Exemplary;
                        message = "GOOD";// $"{methodName} had the correct result. ";
                    }
                    else
                    {
                        testCriteria = Criteria.Effective;
                        message = $"The returned index from {methodName} was not correct. EXPECTED: 51, ACTUAL: {index}.";
                    }
                    methodResults[MethodParts.Execution] = message;
                }
                else
                {
                    switch (methodProblems)
                    {
                        case MethodProblems.NotFound:
                            testCriteria = Criteria.NoAttempt;
                            break;
                        case MethodProblems.ReturnType:
                        case MethodProblems.Parameters:
                            testCriteria = Criteria.Insufficient;
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (AmbiguousMatchException am)
            {
                methodResults = GetInitialMethodResults(methodName);
                testCriteria = Criteria.Beginning;
                message = $"You should only have 1 {methodName} method.";
                methodResults[MethodParts.Name] = message;
            }
            catch (Exception ex)
            {
                methodResults = GetInitialMethodResults(methodName);
                testCriteria = Criteria.Insufficient;
                message = $"EXCEPTION: {ex.Message}";
                methodResults[MethodParts.Name] = message;
            }
            ShowMethodResult($"Part A-3: {methodName}", testCriteria, methodResults);
        }
        private static void PartA_4_FindHeroTest()
        {
            HeroesDB.LoadHeroes();

            Criteria testCriteria = Criteria.NoAttempt;
            string message = string.Empty;
            string methodName = "FindHero";

            Type heroDBType = typeof(HeroesDB);
            var method = heroDBType.GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            List<Type> expectedParams = new List<Type>() { typeof(string) };
            MethodProblems methodProblems = EvaluateMethod(methodName, method, typeof(void), expectedParams, ref message, out Dictionary<MethodParts, string> methodResults);

            switch (methodProblems)
            {
                case MethodProblems.None:
                    testCriteria = Criteria.Exemplary;
                    break;
                case MethodProblems.NotFound:
                    testCriteria = Criteria.NoAttempt;
                    break;
                case MethodProblems.ReturnType:
                case MethodProblems.Parameters:
                    testCriteria = Criteria.Insufficient;
                    break;
                default:
                    break;
            }
            ShowMethodResult($"Part A-4: {methodName}", testCriteria, methodResults);
        }
        #endregion

        #region Part B Tests

        private static void RunPartBTests()
        {
            PartB_1_GroupHeroesTest();
            PartB_2_PrintGroupCountsTest();
            PartB_3_FindHeroesByLetterTest();
        }

        private static void PartB_1_GroupHeroesTest()
        {
            HeroesDB.LoadHeroes();

            Criteria testCriteria = Criteria.NoAttempt;
            string message = string.Empty;
            string methodName = "GroupHeroes";

            Type heroDBType = typeof(HeroesDB);
            var method = heroDBType.GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            List<Type> expectedParams = new List<Type>();
            MethodProblems methodProblems = EvaluateMethod(methodName, method, typeof(void), expectedParams, ref message, out Dictionary<MethodParts, string> methodResults);
            if (methodProblems == MethodProblems.None)
            {
                //
                // run the GroupHeroes method
                //
                method.Invoke(null, null);

                // get the groupedHeroes field
                //
                FieldInfo groupedField = heroDBType.GetField("_groupedHeroes", BindingFlags.NonPublic | BindingFlags.Static);
                var groupObj = groupedField.GetValue(null);
                Dictionary<string, List<Hero>> grouped = (Dictionary<string, List<Hero>>)groupObj;

                Dictionary<string, int> counts;

                string path = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Tests", "groupCounts.json");
                string jsonText = File.ReadAllText(path);// @".\Tests\groupCounts.json");
                counts = JsonConvert.DeserializeObject<Dictionary<string, int>>(jsonText) ?? new Dictionary<string, int>();
                if (grouped.Keys.Count != counts.Keys.Count)
                {
                    testCriteria = Criteria.Effective;
                    message = $"The dictionary generated by {methodName} was not correct. EXPECTED key count: {counts.Keys.Count}, ACTUAL: {grouped.Keys.Count}.";
                }
                else
                {
                    bool errors = false;
                    foreach (var item in grouped)
                    {
                        if (counts.TryGetValue(item.Key, out int count))
                        {
                            if (count != item.Value.Count)
                            {
                                testCriteria = Criteria.Effective;
                                message = $"The count for letter {item.Key} was not correct. EXPECTED key count: {count}, ACTUAL: {item.Value.Count}.";
                                errors = true;
                                break;
                            }
                        }
                    }
                    if (!errors)
                    {
                        testCriteria = Criteria.Exemplary;
                        message = "GOOD";// $"{methodName} had the correct result. ";
                    }
                }
                methodResults[MethodParts.Execution] = message;
            }
            else
            {
                switch (methodProblems)
                {
                    case MethodProblems.NotFound:
                        testCriteria = Criteria.NoAttempt;
                        break;
                    case MethodProblems.ReturnType:
                    case MethodProblems.Parameters:
                        testCriteria = Criteria.Insufficient;
                        break;
                    default:
                        break;
                }
            }
            ShowMethodResult($"Part B-1: {methodName}", testCriteria, methodResults);
        }

        private static void PartB_2_PrintGroupCountsTest()
        {
            HeroesDB.LoadHeroes();

            Criteria testCriteria = Criteria.NoAttempt;
            string message = string.Empty;
            string methodName = "PrintGroupCounts";

            Type heroDBType = typeof(HeroesDB);
            var method = heroDBType.GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            List<Type> expectedParams = new List<Type>() {  };
            MethodProblems methodProblems = EvaluateMethod(methodName, method, typeof(void), expectedParams, ref message, out Dictionary<MethodParts, string> methodResults);

            switch (methodProblems)
            {
                case MethodProblems.None:
                    testCriteria = Criteria.Exemplary;
                    break;
                case MethodProblems.NotFound:
                    testCriteria = Criteria.NoAttempt;
                    break;
                case MethodProblems.ReturnType:
                case MethodProblems.Parameters:
                    testCriteria = Criteria.Insufficient;
                    break;
                default:
                    break;
            }
            ShowMethodResult($"Part B-2: {methodName}", testCriteria, methodResults);

        }
        private static void PartB_3_FindHeroesByLetterTest()
        {
            HeroesDB.LoadHeroes();

            Criteria testCriteria = Criteria.NoAttempt;
            string message = string.Empty;
            string methodName = "FindHeroesByLetter";

            Type heroDBType = typeof(HeroesDB);
            var method = heroDBType.GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            List<Type> expectedParams = new List<Type>() { typeof(string) };
            MethodProblems methodProblems = EvaluateMethod(methodName, method, typeof(void), expectedParams, ref message, out Dictionary<MethodParts, string> methodResults);

            switch (methodProblems)
            {
                case MethodProblems.None:
                    testCriteria = Criteria.Exemplary;
                    break;
                case MethodProblems.NotFound:
                    testCriteria = Criteria.NoAttempt;
                    break;
                case MethodProblems.ReturnType:
                case MethodProblems.Parameters:
                    testCriteria = Criteria.Insufficient;
                    break;
                default:
                    break;
            }
            ShowMethodResult($"Part B-3: {methodName}", testCriteria, methodResults);

        }


        #endregion

        #region Part C Tests

        private static void RunPartCTests()
        {
            PartC_1_RemoveHeroTest();
        }

        private static void PartC_1_RemoveHeroTest()
        {
            HeroesDB.LoadHeroes();

            Criteria testCriteria = Criteria.NoAttempt;
            string message = string.Empty;
            string methodName = "RemoveHero";


            Type heroDBType = typeof(HeroesDB);
            var method = heroDBType.GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            List<Type> expectedParams = new List<Type>() { typeof(string) };
            MethodProblems methodProblems = EvaluateMethod(methodName, method, typeof(void), expectedParams, ref message, out Dictionary<MethodParts, string> methodResults);
            if (methodProblems == MethodProblems.None)
            {
                //
                // run the GroupHeroes method
                //
                method.Invoke(null, new object[] { "Aquaman" });

                // get the groupedHeroes field
                //
                FieldInfo groupedField = heroDBType.GetField("_groupedHeroes", BindingFlags.NonPublic | BindingFlags.Static);
                var groupObj = groupedField.GetValue(null);
                Dictionary<string, List<Hero>> grouped = groupObj as Dictionary<string, List<Hero>>;

                Dictionary<string, int> counts;

                string path = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Tests", "groupCounts.json");
                string jsonText = File.ReadAllText(path);// @".\Tests\groupCounts.json");
                counts = JsonConvert.DeserializeObject<Dictionary<string, int>>(jsonText) ?? new Dictionary<string, int>();
                if (grouped == null)
                {
                    testCriteria = Criteria.Beginning;
                    message = "Your dictionary is null.";
                }
                else if (!grouped.ContainsKey("A"))
                {
                    testCriteria = Criteria.Beginning;
                    message = "Your dictionary does not have a key for 'A'.";
                }
                else if (grouped["A"].Count != counts["A"] - 1)
                {
                    testCriteria = Criteria.Effective;
                    message = $"{methodName} did not remove 'Aquaman' from the list stored for the 'A' key. The Count of heroes for 'A' should be {counts["A"] - 1} but your count is {grouped["A"].Count}.";
                }
                else
                {
                    bool errors = false;
                    List<Hero> aHeroes = grouped["A"];
                    foreach (Hero aHero in aHeroes)
                    {
                        if (aHero.Name.Equals("Aquaman", StringComparison.OrdinalIgnoreCase))
                        {
                            errors = true;
                            break;
                        }
                    }
                    if (errors)
                    {
                        testCriteria = Criteria.Effective;
                        message = $"{methodName} did not remove 'Aquaman' from the list stored for the 'A' key.";
                    }
                    else
                    {
                        FieldInfo heroesField = heroDBType.GetField("_heroes", BindingFlags.NonPublic | BindingFlags.Static);
                        var heroesObj = heroesField.GetValue(null);
                        List<Hero> heroes = heroesObj as List<Hero>;
                        if(heroes.Find(h => h.Name.Equals("Aquaman")) != null)
                        {
                            testCriteria = Criteria.Effective;
                            message = $"{methodName} did not remove 'Aquaman' from the _heroes list.";
                        }
                        else
                        {
                            testCriteria = Criteria.Exemplary;
                            message = "GOOD";// $"{methodName} had the correct result. ";
                        }
                    }
                }
                methodResults[MethodParts.Execution] = message;
            }
            else
            {
                switch (methodProblems)
                {
                    case MethodProblems.NotFound:
                        testCriteria = Criteria.NoAttempt;
                        break;
                    case MethodProblems.ReturnType:
                    case MethodProblems.Parameters:
                        testCriteria = Criteria.Insufficient;
                        break;
                    default:
                        break;
                }
            }
            ShowMethodResult($"Part C-1: {methodName}", testCriteria, methodResults);
        }
        #endregion
    }
}
