using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace AllergenAware
{
   public  class Allergen
    {
       public string allergenName;
       public bool isSevere;
       public bool isSelected; 

       public Allergen(string name, bool severe, bool select)
       {
           this.allergenName = name;
           this.isSevere = severe;
           this.isSelected = select;
          
       }
       public Allergen(string name)
       {
           this.allergenName = name;
           this.isSevere = false;
           this.isSelected = false;
       }

       public void setAllergenName(string newName) {
           this.allergenName = newName;
       }
       public void setIsSevere(bool newBool) {
           this.isSevere = newBool;
       }
       public void setIsSelected(bool newBool2) {
           this.isSelected = newBool2;
       }

       public string getAllergenName() {
           return allergenName;
       }
       public bool getIsSevere() {
           return isSevere;
       }
       public bool getIsSelected() {
           return isSelected;
       }
       public String toString()
       {
           string s = allergenName + ": Selected? " + isSelected + ". Severe? " + isSevere;
           return s;
       }
    }
}