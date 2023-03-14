using HeroDB;
using HeroesV2.Tests;
using PG2Input;
using System;
using System.Diagnostics;
using System.Runtime.Intrinsics.X86;

namespace HeroesV2
{
    /*  Lab Video   
      
        Here's a video showing what the lab could look like when completed:
        https://web.microsoftstream.com/video/01959e6a-381e-4992-846e-fa7aa91a8459

    */
    internal class Program
    {
        static void Main(string[] args)
        {
            string userName = string.Empty;
            Input.GetString("What is your name? ", ref userName);
            Console.Clear();

            List<string> menu = new List<string>()
            { "1. Sort by Name (descending)", "2. Sort By", "3. Find Hero (Binary Search)", "4. Print Group Counts", "5. Find All Heroes by first letter", "6. Remove Hero", "7. Run Lab tests", "8. Exit" };

            List<string> sortBy = new List<string>()
            { "1. Intelligence", "2. Strength", "3. Speed", "4. Durability", "5. Power", "6. Combat" };

            int selection;
            do
            {
                Console.Clear();
                Console.WriteLine($"Hello {userName}. Welcome to Heroes DB (v2)");
                Input.GetMenuChoice("Choice? ", menu, out selection);

                switch (selection)
                {
                    case 1:
                        HeroesDB.SortByNameDescending();
                        break;
                    case 2:
                        //
                        // Part A-2: SortByAttribute
                        //
                        // In the HeroesDB.cs file in the HeroDB project...
                        //      Add a method called SortByAttribute to the HeroesDB class.
                        //      The method should have a SortBy parameter passed to it.
                        //      In the method, call the MergeSort method from part A-1.
                        //      Pass to it the _heroes list of the class and the SortBy parameter.
                        //      After calling MergeSort, print the items in the sorted list that is returned from MergeSort.
                        //          NOTE: print the hero ID, selected attribute, and name(see screenshot).
                        //          To get the selected attribute, call the GetSortByAttribute on each hero.

                        //
                        // In Main (here), add code to case 2 of the switch to call the method.
                        //      Call the SortByAttribute method and pass the sortByChoice variable to it.
                        //
                        Input.GetMenuChoice("Sort by? ", sortBy, out int sortBySelection);
                        SortBy sortByChoice = (SortBy)sortBySelection;

                        HeroesDB.SortByAttribute(sortByChoice);

                        break;
                    case 3:
                        //
                        // Part A-4: FindHero
                        //
                        // In the HeroesDB.cs file in the HeroDB project...
                        //      Add a method called FindHero to the HeroesDB class.
                        //      The method should have a string parameter for the name of the hero to find.
                        //      Call the BinarySearch method from part A-3.
                        //      Print the result.
                        //      If the found index is -1,
                        //          print "<insert heroName> is not found"
                        //      otherwise
                        //          print "<insert heroName> was found at index <insert found index>"

                        //
                        // In Main (here), add code to case 3 of the switch to call the method.
                        //      Using Input.GetString (see example above), ask the user to enter the name of the hero to find.
                        //      Call the FindHero method and pass the string the user entered.
                        //

                        string heroToFind = string.Empty;
                        Input.GetString("Plase enter the name of the hero you wish to find: ", ref heroToFind);

                        HeroesDB.FindHero(heroToFind);

                        break;
                    case 4:
                        //
                        // Part B-2: PrintGroupCounts
                        //
                        // In the HeroesDB.cs file in the HeroDB project...
                        //      Add a method called PrintGroupCounts to the HeroesDB class.
                        //      In the method, if _groupedHeroes is null, call the GroupHeroes method from part B-1.
                        //      Loop over the dictionary and print each key and the count of the list for each key.
                        //
                        //      In Main (here), add code to case 4 to call the PrintGroupCounts method.
                        
                        HeroesDB.PrintGroupCounts();

                        break;
                    case 5:
                        //
                        // Part B-3: FindHeroesByLetter
                        //
                        // In the HeroesDB.cs file in the HeroDB project...
                        //      Add a method called FindHeroesByLetter to the HeroesDB class.
                        //      The method should take a string parameter for the first letter.
                        //      In the method, if _groupedHeroes is null, call the GroupHeroes method from part B-1.
                        //      Then, check if the letter parameter is in the dictionary.
                        //      If it is not,
                        //          then print a message that no heroes were found that start with the letter.
                        //      Else,
                        //          loop over the list of heroes for the key and print the ID and name.
                        //

                        //
                        // In Main (here), add code to case 4 of the switch to call the method.
                        //      Using Input.GetString (see example above), ask the user to enter the Letter of the hero to find.
                        //      Call the FindHeroesByLetter method and pass the string the user entered.
                        //

                        string letterOfHero = string.Empty;
                        Input.GetString("Please enter the first letter of the hero you would like to find: ", ref letterOfHero);

                        HeroesDB.FindHeroesByLetter(letterOfHero);

                        break;
                    case 6:
                        //
                        // Part C-1: RemoveHero
                        //
                        // In the HeroesDB.cs file in the HeroDB project...
                        //      Add a method called RemoveHero to the HeroesDB class.
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
                        // In Main (here), add code to case 5 of the switch to call the method.
                        //      Using Input.GetString (see example above), ask the user to enter the Letter of the hero to remove.
                        //      Call the RemoveHero method and pass the string the user entered.
                        //

                        string hero = string.Empty;
                        Input.GetString($"What is the name of the hero you would like to remove? ", ref hero);

                        HeroesDB.RemoveHero(hero);

                        break;
                    case 7:
                        LabTests.RunTests();
                        break;
                    case 8:
                        return;
                    default:
                        break;
                }
                Console.ReadKey();

            } while (selection != menu.Count);
        }


    }
}