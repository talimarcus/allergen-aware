using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;


namespace AllergenAware
{
    public class Person
    {
        public string personName; 
        public string photoSource; 
        public ArrayList allergiesList { get; private set; }

        public Person(string name, string source, ArrayList allergies){
            this.personName = name;
            this.photoSource = source;
            this.allergiesList = allergies;
        }

        public Person(string name, string pic)
        {
            this.personName = name;
            this.photoSource = pic;
            this.allergiesList = new ArrayList();  
        }

        public void setAllergiesList(ArrayList newAllergies)
        {
            this.allergiesList = newAllergies;
        }

        public string getName()
        {
            return personName;
        }

        public string toString()
        {
            string str = "";
            str += "Person name: " + personName + ". Photo source: " + photoSource + ". Allergies: ";
            string allergies = "";

            for (int i = 0; i < allergiesList.Count; i++)
            {
                if (((Allergen)allergiesList[i]).isSelected)
                {
                    allergies += ((Allergen)allergiesList[i]).getAllergenName();

                    if (((Allergen)allergiesList[i]).isSevere)
                    {
                        allergies += "***";
                    }
                    allergies += ",";                  
                }
            }
            str += allergies;
            return str;
        }
    }

}
