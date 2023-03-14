using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HeroDB
{
    public enum SortBy
    {
        Intelligence = 1,
        Strength,
        Speed,
        Durability,
        Power,
        Combat
    }

    


    public class HeroesDB
    {
        private static List<Hero> _heroes;
        private static Dictionary<string, List<Hero>> _groupedHeroes;
        static HeroesDB()
        {
            LoadHeroes();
        }
        public static int Count
        {
            get
            {
                return _heroes.Count;
            }
        }
        public static void LoadHeroes()
        {
            string jsonText = File.ReadAllText("heroes.json");
            try
            {
                _heroes = JsonConvert.DeserializeObject<List<Hero>>(jsonText) ?? new List<Hero>();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _heroes = new List<Hero>();
            }
        }



        //
        //sort heroes by Name descendingly using the Bubble Sort algorithm
        //
        public static void SortByNameDescending()
        {
            List<Hero> sorted = _heroes.ToList();//clone

            int n = sorted.Count;
            bool swapped;
            do
            {
                swapped = false;
                for (int i = 1; i < n; i++)
                {
                    int compResult = string.Compare(sorted[i - 1].Name, sorted[i].Name, true);//pass true to ignore case
                    if (compResult < 0)
                    {
                        swapped = true;
                        (sorted[i - 1], sorted[i]) = (sorted[i], sorted[i - 1]);
                    }
                }
                --n;
            } while (swapped);


            foreach (var hero in sorted)
            {
                Console.WriteLine($"{hero.Id,3}: {hero.Name}");
            }
        }



        //---------------------------------------------------------------------------------------------------------------------------------------------
        //
        //PART A
        //

        //
        // Part A-1: MergeSort
        //      Implement the MergeSort and Merge methods in the HeroesDB class.
        //      Your code must follow the pseudo-code. You will find the pseudocode for the methods in the lab document (see the Solution Items in the Solution Explorer)
        //
        //      NOTE: you must add a parameter of type SortBy to both methods.
        //      You will get the user’s sort by selection in part A-2 and pass it to MergeSort.
        //
        //      NOTE: to compare heroes, use the Hero.Compare method.
        //      EX: int compResult = Hero.Compare(hero1, hero2, sortBy); //returns -1 is hero1 < hero2, 0 if hero1 = hero2, or 1 is hero1 > hero2
        //

        public static List<Hero> MergeSort(List<Hero> m, SortBy sort)
        {
            //Base Case
            //A list of zero or one elements is sorted, by definition
            if (m.Count <= 1)
            {
                return m;
            }

            //Recursive Case
            //First, divide the list into equal-sized sublists consisting of the first half and second half of the list
            //This assumes lists start at index 0
            var left = new List<Hero>();
            var right = new List<Hero>();

            for (int i = 0; i < m.Count; i++)
            {
                if (i < (m.Count / 2))
                {
                    left.Add(m[i]);
                }
                else
                {
                    right.Add(m[i]);
                }
            }

            //Recursively sort both sublists
            left = MergeSort(left, sort);
            right = MergeSort(right, sort);

            //Then merge the now-sorted sublists
            //Uncomment Code Below
            return Merge(left, right, sort);
        }

        public static List<Hero> Merge(List<Hero> left, List<Hero> right, SortBy sort)
        {
            var result = new List<Hero>();

            while (left.Count > 0 && right.Count > 0)
            {
                int compResult = Hero.Compare(left[0], right[0], sort);

                if (compResult <= 0)
                {
                    result.Add(left[0]);
                    left.RemoveAt(0);
                }
                else
                {
                    result.Add(right[0]);
                    right.RemoveAt(0);
                }
            }

            // Either left or right may have elements left; consume them
            // (Only one of the following loops will actually be entered)
            while (left.Count > 0)
            {
                result.Add(left[0]);
                left.RemoveAt(0);
            }

            while (right.Count > 0)
            {
                result.Add(right[0]);
                right.RemoveAt(0);
            }

            return result;
        }

        //
        // Part A-2: SortByAttribute
        //
        //      The method should have a SortBy parameter passed to it.
        //      In the method, call the MergeSort method from part A-1.
        //      Pass to it the _heroes list of the class and the SortBy parameter.
        //      After calling MergeSort, print the items in the sorted list that is returned from MergeSort.
        //          NOTE: print the hero ID, selected attribute, and name(see screenshot).
        //          To get the selected attribute, call the GetSortByAttribute on each hero.
        //

        public static void SortByAttribute(SortBy sort)
        {
            List<Hero> sortedHeroes = MergeSort(_heroes, sort);

            foreach (Hero hero in sortedHeroes)
            {
                Console.WriteLine($"{hero.Id}. {hero.Name} ---> {sort} {hero.GetSortByAttribute(sort)}");
            }
        }

        //
        // Part A-3: BinarySearch
        //      Implement the BinarySearch method in the HeroesDB class.
        //      Your code must follow the pseudo-code. You will find the pseudocode for the methods in the lab document (see the Solution Items in the Solution Explorer)
        //


        public static int BinarySearch(List<Hero> heroes, string searchTerm, int low, int high)
        {
            if (high < low)
            {
                return -1;
            }

            int mid = (low + high) / 2;

            if (String.Compare(searchTerm, heroes[mid].Name) < 0)
            {
                return BinarySearch(heroes, searchTerm, low, mid - 1);
            }
            else if (String.Compare(searchTerm, heroes[mid].Name) > 0)
            {
                return BinarySearch(heroes, searchTerm, mid + 1, high);
            }
            else
            {
                return mid;
            }
        }

        //
        // Part A-4: FindHero
        //      The method should have a string parameter for the name of the hero to find.
        //      Call the BinarySearch method from part A-3.
        //      Print the result.
        //      If the found index is -1,
        //          print "<insert heroName> is not found"
        //      otherwise
        //          print "<insert heroName> was found at index <insert found index>"
        //

        public static void FindHero(string heroName)
        {
            List<Hero> allHeroes = new List<Hero>();
            foreach (Hero hero in _heroes)
            {
                allHeroes.Add(hero);
            }

            allHeroes.Sort((h1, h2) => h1.Name.CompareTo(h2.Name));

            int result = BinarySearch(allHeroes, heroName, 0, allHeroes.Count - 1);

            if (result == -1)
            {
                Console.WriteLine($"{heroName} was not found!");
            }
            else
            {
                Console.WriteLine($"{heroName} was found at index {result}!");
            }
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------
        //
        // PART B
        //
        //
        // Part B-1: GroupHeroes
        //      Add a method called GroupHeroes to the HeroesDB class.
        //      The method should initialize the _groupedHeroes dictionary.
        //      Make sure to make the keys case insensitive (ignore the case). HINT: look at the constructors of the Dictionary class for an overload that you can use.
        //
        //      You want to create a dictionary where the keys are the first letters of the heroes and the value for each key is a list of the heroes whose names start with that letter.
        //      EX: for the key “B”, the value would contain a list of all the heroes whose names start with B.
        //
        //      Loop over the heroes list.
        //      Check if the first letter of each hero name is in the _groupedHeroes dictionary.
        //      If not,
        //          then create a new list,
        //          add the hero to the list,
        //          then add the list to the dictionary as the value for that initial letter.
        //      Else If it is in the dictionary already,
        //          then add the hero to the list that is stored for that key.
        //

        public static void GroupHeroes()
        {
            _groupedHeroes = new Dictionary<string, List<Hero>>(StringComparer.OrdinalIgnoreCase);

            foreach(Hero hero in _heroes)
            {
                string firstLetter = hero.Name[0].ToString().ToLower();

                if (!_groupedHeroes.ContainsKey(firstLetter))
                {
                    _groupedHeroes[firstLetter] = new List<Hero> { hero };
                }
                else
                {
                    _groupedHeroes[firstLetter].Add(hero);
                }
            }
        }


        //
        // Part B-2: PrintGroupCounts
        //
        //      Add a method called PrintGroupCounts to the HeroesDB class.
        //      In the method, if _groupedHeroes is null, call the GroupHeroes method from part B-1.
        //      Loop over the dictionary and print each key and the count of the list for each key.

        public static void PrintGroupCounts()
        {
            if (_groupedHeroes == null)
            {
                GroupHeroes();
            }

            foreach (KeyValuePair<string, List<Hero>> group in _groupedHeroes)
            {
                Console.WriteLine("{0}: {1}", group.Key, group.Value.Count);
            }
        }

        //
        // Part B-3: FindHeroesByLetter
        //      The method should take a string parameter for the first letter.
        //      In the method, if _groupedHeroes is null, call the GroupHeroes method from part B-1.
        //      Then, check if the letter parameter is in the dictionary.
        //      If it is not,
        //          then print a message that no heroes were found that start with the letter.
        //      Else,
        //          loop over the list of heroes for the key and print the ID and name.
        //

        public static void FindHeroesByLetter(string firstLetter)
        {
            if (_groupedHeroes == null)
            {
                GroupHeroes();
            }

            if (!_groupedHeroes.ContainsKey(firstLetter.ToUpper()))
            {
                Console.WriteLine($"No heroes were found that start with the letter {firstLetter}.");
                return;
            }

            List<Hero> heroList = _groupedHeroes[firstLetter.ToUpper()];

            foreach (Hero hero in heroList)
            {
                Console.WriteLine($"{hero.Id}: {hero.Name}");
            }
        }



        //---------------------------------------------------------------------------------------------------------------------------------------------
        //
        // PART C
        //
        //
        // Part C-1: RemoveHero
        //
        //      The method should take a string parameter that is the name of the hero to remove.
        //      In the method, if _groupedHeroes is null, call the GroupHeroes method from part B-1.
        //      Then, check if the _groupedHeroes dictionary contains a key with the first letter of the name.
        //      If not,
        //          print a message saying the hero was not found.
        //     If the key is found, then get the list for the key. The list is the value stored in the dictionary for the key.
        //          call the BinarySearch method to get the index of the hero to remove for the list.
        //          If BinarySearch returns the index, then remove the hero from the list AND from the _heroes list. Print that the hero was removed.
        //              NOTE: if removing the hero makes the list empty for the letter, then remove the letter (which is the key) from the dictionary.
        //          If BinarySearch returns -1 (meaning the hero is not in the list), print a message that the hero was notfound.
        //

        public static void RemoveHero (string name)
        {
                if (_groupedHeroes == null)
                {
                    GroupHeroes();
                }

                string firstLetter = name.Substring(0, 1).ToUpper();

                if (!_groupedHeroes.ContainsKey(firstLetter))
                {
                    Console.WriteLine("{0} was not found.", name);
                }
                else
                {
                    var list = _groupedHeroes[firstLetter];
                    var index = BinarySearch(list, name, 0, list.Count - 1);

                    if (index == -1)
                    {
                        Console.WriteLine("{0} was not found.", name);
                    }
                    else
                    {
                        list.RemoveAt(index);
                        _heroes.RemoveAll(h => h.Name == name);

                        if (list.Count == 0)
                        {
                            _groupedHeroes.Remove(firstLetter);
                        }

                        Console.WriteLine("{0} was removed.", name);
                    }
                }
        }

    }
}
