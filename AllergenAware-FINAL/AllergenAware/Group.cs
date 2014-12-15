using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AllergenAware
{
    class Group
    {
        public string groupName { get; private set; }
        public string photoSource { get; private set; }
        public ArrayList peopleList { get; private set; }

        public Group(string name, string source, ArrayList people){
            this.groupName = name;
            this.photoSource = source;
            this.peopleList = people;
        }

        public Group(string name, string source)
        {
            this.groupName = name;
            this.photoSource = source;
            this.peopleList = new ArrayList();
        }

        public Group(string name)
        {
            this.groupName = name;
            this.peopleList = new ArrayList();
        }

        public string toString()
        {
            string str = "";
            str += "Group: " + this.groupName + ". Members: ";
            for (int i = 0; i < this.peopleList.Count; i++)
            {
                str += ((Person)peopleList[i]).getName() + ", ";
            }
            return str;
        }
        public ArrayList getAllAllergens()
        {
            ArrayList allAllergies = new ArrayList();
            allAllergies.Add(new Allergen("dairy", false, false));
            allAllergies.Add(new Allergen("eggs", false, false));
            allAllergies.Add(new Allergen("gluten", false, false));
            allAllergies.Add(new Allergen("soy", false, false));
            allAllergies.Add(new Allergen("peanut", false, false));
            allAllergies.Add(new Allergen("shellfish", false, false));
           

            for (int i=0; i<this.peopleList.Count; i++) {
                Person currentPerson = (Person)this.peopleList[i];
                for (int j = 0; j < allAllergies.Count; j++) {
                    if (((Allergen)currentPerson.allergiesList[j]).isSelected)
                    {
                        ((Allergen)allAllergies[j]).isSelected = true;
                        if (((Allergen)currentPerson.allergiesList[j]).isSevere)
                        {
                            ((Allergen)allAllergies[j]).isSevere = true;
                        }
                    }
                }
            }
            return allAllergies;
        }

    }
}

