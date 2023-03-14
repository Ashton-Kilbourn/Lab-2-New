using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day01
{
    /*                    List<T>       
             
    
            Vocabulary:
        
                  List<T>: https://www.tutorialsteacher.com/csharp/csharp-list#:~:text=C%23%20-%20List%3CT%3E%201%20List%3CT%3E%20Characteristics%20List%3CT%3E%20equivalent,8%20Check%20Elements%20in%20List%20...%20More%20items
                    a collection of strongly typed objects that can be accessed by index. Indexes start at 0.

            Links:
                  C# List Tutorial: https://www.c-sharpcorner.com/article/c-sharp-list/

                  Removing from a List:
                      Remove():     https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1.remove?view=net-7.0
                      RemoveAt():   https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1.removeat?view=net-7.0
                  
             Lecture videos:
                  LIST LECTURE:
                  https://fullsailedu-my.sharepoint.com/:v:/g/personal/ggirod_fullsail_com/ERG1iZHKJgFIoj6W8dhxPPgBIIY-Ot1b6Ueh-50Ggfcikg?e=goT9d6


    */
    internal class ListSamples
    {
        public void CodeSamples()
        {
            /* [  Declaring a List<T>  ]            
                replace T with the type you want to store in the list
            */
            List<string> names; //names is null. no list has been created yet.


            /* [  Initializing a List<T>  ]            
                use the initializer to add values when you declare the list variable
            */
            names = new List<string>() { "Joker", "Bane", "Riddler" };


            /* [  Initializing a List<T>  ]            
                create an empty list
            */
            names = new List<string>();



            /* [  Adding to a List<T>  ]            
                Use the Add(item) method to add the item to the end of the list.
            */
            names.Add("Batman");
            names.Add("The Bats");
            names.Add("Bruce");
            names.Add("Alfred");


            /* [  looping over a List<T>  ]            
                Use a for loop to loop over list.
                use .Count in the for loop condition
            */
            for (int i = 0; i < names.Count; i++)
            {
                Console.WriteLine(names[i]); // use [ ] to access the item at a specific index. use 'i' as the index.
            }


            /* [  looping over a List<T>  ]            
                Use a foreach to loop over a List.
            */
            foreach (string name in names)
            {
                Console.WriteLine(name);
            }


            /* [  Removing from a List<T>  ]            
                RemoveAt(index) - removes whatever item is at the index 

                Indexes are zero-based. In this example below, 1 refers to the 2nd item in the list.
            */
            names.RemoveAt(1);


            /* [  Removing from a List<T>  ]            
                Remove(item) - removes the first occurrence of the item in the list, if found. 
            */
            names.Remove("Alfred"); //will remove the first item from the list that matches the value passed to Remove.


            /* [  Removing from a List<T>  ]            
                If removing multiple items from a list in a loop, you need to make sure items in the list are not skipped.
                use a reverse for loop
            */
            List<string> villains = new List<string>() { "Joker", "Aquaman", "Aquaman", "Bane", "Riddler" };
            for (int i = villains.Count - 1; i >= 0; i--)
            {
                if (villains[i].Equals("Aquaman", StringComparison.OrdinalIgnoreCase))
                    villains.RemoveAt(i);
            }
        }
    }
}
