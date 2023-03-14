using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day01
{
    /*     Dictionary<TKey, TValue>       
             
    
            Vocabulary:
        
                  Dictionary<TKey, TValue>: a generic collection that stores key-value pairs in no particular order.

            Links:
                  https://www.tutorialsteacher.com/csharp/csharp-dictionary
                  
             Lecture videos:
                  Dictionary LECTURE:
                  https://fullsailedu-my.sharepoint.com/:v:/g/personal/ggirod_fullsail_com/EYgLFeMMYXhBp9WKgfYoV5MBdnCbUoqH6wigGr1pohYdwg?e=bCwXUH


    */
    internal class DictionarySamples
    {
        enum Weapon
        {
            Sword, Axe, Spear, Mace
        }
        public void CodeSamples()
        {
            /* [  Creating a Dictionary  ]
                
                Dictionary<TKey, TValue>  - the TKey is a placeholder for the type of the keys. TValue is a placeholder for the type of the values.
                
                When you want to create a Dictionary variable, replace TKey with whatever type of data you want to use for the keys and
                replace TValue with the type you want to use for the values.
            */
            Dictionary<Weapon, int> backpack = new Dictionary<Weapon, int>();



            /* [  Adding items to a Dictionary  ]

                There are 3 ways to add items to a Dictionaruy:
                1) on the initializer. 
                2) using the Add method. 
                3) using [key] = value
            */
            backpack = new Dictionary<Weapon, int>()
            {
                {Weapon.Sword, 5 } //adds the Sword key with a value of 5 to the dictionary
            };
            backpack.Add(Weapon.Axe, 2); //adds the Axe key with a value of 2 to the dictionary
            backpack[Weapon.Spear] = 1;  //adds/updates the Spear key with a value of 1 




            /* [  Looping over a Dictionary  ]

                You should use a foreach loop when needing to loop over the entire dictionary.
            */
            foreach (KeyValuePair<Weapon, int> weaponCount in backpack)
                Console.WriteLine($"You have {weaponCount.Value} {weaponCount.Key}");




            /* [  Checking for a key in a Dictionary  ]

                There are 2 ways to see if a key is in the dictionary:
                1) ContainsKey(key)
                2) TryGetValue(key, out value)               
            */
            if (backpack.ContainsKey(Weapon.Axe))
                Console.WriteLine($"{Weapon.Axe} count: {backpack[Weapon.Axe]}");

            if (backpack.TryGetValue(Weapon.Spear, out int spearCount))
                Console.WriteLine($"{Weapon.Spear} count: {spearCount}");





            /* [  Updating a value for a key in a Dictionary  ]

                To update an exisiting value in the dictionary, use the [ ] 
            */
            backpack[Weapon.Mace] = 0; //sell all maces




            /* [  Removing a key and its value from a Dictionary  ]

                Remove(key) -- returns true/false if the key was found

                You do NOT need to check if the key is in dictionary. Check the returned bool.
               
            */
            bool wasFound = backpack.Remove(Weapon.Mace);
            if (!wasFound) Console.WriteLine($"{Weapon.Mace} was NOT found.");
        }
    }
}
