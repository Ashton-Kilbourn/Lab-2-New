namespace HeroDB
{
    public struct Stats
    {
        public int Intelligence;
        public int Strength;
        public int Speed;
        public int Durability;
        public int Power;
        public int Combat;
    }

    public struct Appearance
    {
        public string Gender;
        public string Race;
        public string[] Height;
        public string[] Weight;
        public string EyeColor;
        public string HairColor;
    }

    public struct Bio
    {
        public string FullName;
        public string AlterEgos;
        public string[] Aliases;
        public string PlaceOfBirth;
        public string FirstAppearance;
        public string Publisher;
        public string Alignment;
    }
    public struct Work
    {
        public string Occupation;
        public string Base;
    }
    public struct Connections
    {
        public string GroupAffiliation;
        public string Relatives;
    }
    public struct Images
    {
        public string XS;
        public string SM;
        public string MD;
        public string LG;
    }
    public class Hero : IEquatable<Hero>
    {
        public int Id { get; set; }
        public string Name { get; set; }

        //string slug

        public Stats Powerstats { get; set; }
        public Appearance Appearance { get; set; }
        public Bio Biography { get; set; }
        public Work Work { get; set; }
        public Connections Connections { get; set; }
        public Images Images { get; set; }



        public bool Equals(Hero other)
        {
            if (other is null)
                return false;

            return this.Name == other.Name && this.Id == other.Id;
        }

        public override bool Equals(object obj) => Equals(obj as Hero);
        public override int GetHashCode() => (Name, Id).GetHashCode();


        public string GetSortByAttribute(SortBy sortBy)
        {
            string attribute = string.Empty;
            switch (sortBy)
            {
                case SortBy.Intelligence:
                    attribute = Powerstats.Intelligence.ToString();
                    break;
                case SortBy.Strength:
                    attribute = Powerstats.Strength.ToString();
                    break;
                case SortBy.Speed:
                    attribute = Powerstats.Speed.ToString();
                    break;
                case SortBy.Durability:
                    attribute = Powerstats.Durability.ToString();
                    break;
                case SortBy.Power:
                    attribute = Powerstats.Power.ToString();
                    break;
                case SortBy.Combat:
                    attribute = Powerstats.Combat.ToString();
                    break;
                default:
                    break;
            }
            return attribute;
        }

        public static int Compare(Hero hero1, Hero hero2, SortBy sortBy)
        {
            int attr1 = 0, attr2 = 0;
            switch (sortBy)
            {
                case SortBy.Intelligence:
                    attr1 = hero1.Powerstats.Intelligence;
                    attr2 = hero2.Powerstats.Intelligence;
                    break;
                case SortBy.Strength:
                    attr1 = hero1.Powerstats.Strength;
                    attr2 = hero2.Powerstats.Strength;
                    break;
                case SortBy.Speed:
                    attr1 = hero1.Powerstats.Speed;
                    attr2 = hero2.Powerstats.Speed;
                    break;
                case SortBy.Durability:
                    attr1 = hero1.Powerstats.Durability;
                    attr2 = hero2.Powerstats.Durability;
                    break;
                case SortBy.Power:
                    attr1 = hero1.Powerstats.Power;
                    attr2 = hero2.Powerstats.Power;
                    break;
                case SortBy.Combat:
                    attr1 = hero1.Powerstats.Combat;
                    attr2 = hero2.Powerstats.Combat;
                    break;
                default:
                    break;
            }
            return attr1.CompareTo(attr2);
        }
    }
}