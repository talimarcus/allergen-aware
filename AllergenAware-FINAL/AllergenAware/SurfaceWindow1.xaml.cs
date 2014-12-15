using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Surface;
using Microsoft.Surface.Presentation;
using Microsoft.Surface.Presentation.Controls;
using Microsoft.Surface.Presentation.Input;
using System.Collections;
using System.Collections.ObjectModel;

namespace AllergenAware
{
    /// <summary>
    /// Interaction logic for SurfaceWindow1.xaml
    /// </summary>
    public partial class SurfaceWindow1 : SurfaceWindow
    {
        //initialize global variables
        String avatar = "";
        String allergenName;
        //ArrayList allergens = new ArrayList();
        Allergen dairy = new Allergen("dairy", false, false);
        Allergen gluten = new Allergen("gluten", false, false);
        Allergen peanut = new Allergen("peanut", false, false);
        Allergen soy = new Allergen("soy", false, false);
        Allergen shellfish = new Allergen("shellfish", false, false);
        Allergen eggs = new Allergen("eggs", false, false);

        Group masterGroup = new Group("Master", "groupPhoto"); //a master group holds all users that have been created
        ArrayList createdGroups = new ArrayList(); //a collection of all groups that have been created
        ArrayList groupMembers = new ArrayList(); //a collection of all members to be added to a new group 

        ArrayList memberNames = new ArrayList(); //a collection of textblocks through which to modify the appearance of selected user's names
        ArrayList allGroupNames = new ArrayList();
        Group selectedGroup; //the group the user is currently cooking for
        TextBlock groupName = new TextBlock(); //a textblock through which to modify the appearance of a selected group name

        ArrayList allAllergens = new ArrayList(); //a list of all the allergens of the currently selected group


        /// <summary>
        /// Default constructor.
        /// </summary>
        public SurfaceWindow1()
        {
            InitializeComponent();

            // Add handlers for window availability events
            AddWindowAvailabilityHandlers();
        }

        /// <summary>
        /// Occurs when the window is about to close. 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            // Remove handlers for window availability events
            RemoveWindowAvailabilityHandlers();
        }

        /// <summary>
        /// Adds handlers for window availability events.
        /// </summary>
        private void AddWindowAvailabilityHandlers()
        {
            // Subscribe to surface window availability events
            ApplicationServices.WindowInteractive += OnWindowInteractive;
            ApplicationServices.WindowNoninteractive += OnWindowNoninteractive;
            ApplicationServices.WindowUnavailable += OnWindowUnavailable;
        }

        /// <summary>
        /// Removes handlers for window availability events.
        /// </summary>
        private void RemoveWindowAvailabilityHandlers()
        {
            // Unsubscribe from surface window availability events
            ApplicationServices.WindowInteractive -= OnWindowInteractive;
            ApplicationServices.WindowNoninteractive -= OnWindowNoninteractive;
            ApplicationServices.WindowUnavailable -= OnWindowUnavailable;
        }

        /// <summary>
        /// This is called when the user can interact with the application's window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWindowInteractive(object sender, EventArgs e)
        {
            //TODO: enable audio, animations here
        }

        /// <summary>
        /// This is called when the user can see but not interact with the application's window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWindowNoninteractive(object sender, EventArgs e)
        {
            //TODO: Disable audio here if it is enabled

            //TODO: optionally enable animations here
        }

        /// <summary>
        /// This is called when the application's window is not visible or interactive.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWindowUnavailable(object sender, EventArgs e)
        {
            //TODO: disable audio, animations here
        }

        
        //Create a new user profile
        private void newUser_TouchDown(object sender, TouchEventArgs e)
        {
            //clear last user input
            NameBox.Text = "";

            //clear previously selected avatar
            avatar = "";
            momBorder.BorderBrush = Brushes.LightGray;
            dadBorder.BorderBrush = Brushes.LightGray;
            sisterBorder.BorderBrush = Brushes.LightGray;
            brotherBorder.BorderBrush = Brushes.LightGray;
            maidBorder.BorderBrush = Brushes.LightGray;

            //clear previously selected allergens
            dairy.isSelected = false;
            dairy.isSevere = false;
            eggs.isSelected = false; 
            eggs.isSevere = false;
            gluten.isSelected = false;
            gluten.isSevere = false;
            soy.isSelected = false;
            soy.isSevere = false;
            peanut.isSelected = false;
            peanut.isSevere = false;
            shellfish.isSelected = false;
            shellfish.isSevere = false;

            //visually de-select all allergens
            dairyBorder.BorderBrush = Brushes.LightGray;
            glutenBorder.BorderBrush = Brushes.LightGray;
            eggsBorder.BorderBrush = Brushes.LightGray;
            shellfishBorder.BorderBrush = Brushes.LightGray;
            soyBorder.BorderBrush = Brushes.LightGray;
            peanutBorder.BorderBrush = Brushes.LightGray;

            //switch screen view
            customizationGrid.Visibility = Visibility.Hidden;
            customizationAlertLabel.Visibility = Visibility.Hidden;
            newPersonGrid.Visibility = Visibility.Visible;
        }

        //Select and set an avatar for a new profile
        private void Image_TouchDown(object sender, TouchEventArgs e)
        {
            //get the source of the chosen image and store as a String
            Image image = new Image();
            sender.GetType();
            Image photo = (Image)sender;
            String uri = photo.Source.ToString();
            String junk = "pack://application:,,,/Drag and Drop;component/";
            int index = junk.Length;
            String actualUri = uri.Substring(index);
            avatar = actualUri; //global var "avatar" will be used to create a new user

            String chosenAvatar = actualUri.Substring(10);
            chosenAvatar = chosenAvatar.Substring(0, chosenAvatar.Length - 4);

            //de-select any currently selected images
            momBorder.BorderBrush = Brushes.LightGray;
            dadBorder.BorderBrush = Brushes.LightGray;
            brotherBorder.BorderBrush = Brushes.LightGray;
            sisterBorder.BorderBrush = Brushes.LightGray;
            maidBorder.BorderBrush = Brushes.LightGray;

            //select chosen image
            if (chosenAvatar.Equals("mom"))
            {
                momBorder.BorderBrush = Brushes.DarkCyan;
            }
            else if (chosenAvatar.Equals("dad"))
            {
                dadBorder.BorderBrush = Brushes.DarkCyan;
            }
            else if (chosenAvatar.Equals("brother"))
            {
                brotherBorder.BorderBrush = Brushes.DarkCyan;
            }
            else if (chosenAvatar.Equals("sister"))
            {
                sisterBorder.BorderBrush = Brushes.DarkCyan;
            }
            else if (chosenAvatar.Equals("maid"))
            {
                maidBorder.BorderBrush = Brushes.DarkCyan;
            }

        }

        //Select and set allergens for a new profile
        private void Allergen_TouchDown(object sender, TouchEventArgs e)
        {
            //figure out which allergen icon was touched   
            Image sendImg = sender as Image;
            String src = sendImg.Source.ToString();
            String junk = "pack://application:,,,/AllergenAware;component/Resources/";
            int index = junk.Length;
            allergenName = src.Substring(index);
            allergenName = allergenName.Substring(0, allergenName.Length - 4);

            //change border color around selected image
            //set alert level of each allergen
            if (allergenName.Equals("eggs"))
            {
                if (eggsBorder.BorderBrush.Equals(Brushes.LightGray)) //if allergen is unselected
                {
                    eggs.isSelected = true; //set allergen to "selected"
                    eggs.isSevere = false;
                    eggsBorder.BorderBrush = Brushes.Orange;
                }
                else if (eggsBorder.BorderBrush.Equals(Brushes.Orange)) //if allergen is selected
                {
                    eggs.isSelected = true;
                    eggs.isSevere = true; //set allergen to severe
                    eggsBorder.BorderBrush = Brushes.Red;
                }
                else if (eggsBorder.BorderBrush.Equals(Brushes.Red)) //if allergen is severe
                {
                    eggs.isSelected = false; //deselect allergen
                    eggs.isSevere = false; //remove severity of allergen
                    eggsBorder.BorderBrush = Brushes.LightGray;
                }
            }
            else if (allergenName.Equals("gluten"))
            {
                if (glutenBorder.BorderBrush.Equals(Brushes.LightGray))
                {
                    gluten.isSelected = true;
                    gluten.isSevere = false;
                    glutenBorder.BorderBrush = Brushes.Orange;
                }
                else if (glutenBorder.BorderBrush.Equals(Brushes.Orange))
                {
                    gluten.isSelected = true;
                    gluten.isSevere = true;
                    glutenBorder.BorderBrush = Brushes.Red;
                }
                else if (glutenBorder.BorderBrush.Equals(Brushes.Red))
                {
                    gluten.isSelected = false;
                    gluten.isSevere = false;
                    glutenBorder.BorderBrush = Brushes.LightGray;
                }
            }
            else if (allergenName.Equals("soy"))
            {
                if (soyBorder.BorderBrush.Equals(Brushes.LightGray))
                {
                    soy.isSelected = true;
                    soy.isSevere = false;
                    soyBorder.BorderBrush = Brushes.Orange;
                }
                else if (soyBorder.BorderBrush.Equals(Brushes.Orange))
                {
                    soy.isSelected = true;
                    soy.isSevere = true;
                    soyBorder.BorderBrush = Brushes.Red;
                }
                else if (soyBorder.BorderBrush.Equals(Brushes.Red))
                {
                    soy.isSelected = false;
                    soy.isSevere = false;
                    soyBorder.BorderBrush = Brushes.LightGray;
                }
            }
            else if (allergenName.Equals("shellfish"))
            {
                if (shellfishBorder.BorderBrush.Equals(Brushes.LightGray))
                {
                    shellfish.isSelected = true;
                    shellfish.isSevere = false;
                    shellfishBorder.BorderBrush = Brushes.Orange;
                }
                else if (shellfishBorder.BorderBrush.Equals(Brushes.Orange))
                {
                    shellfish.isSelected = true;
                    shellfish.isSevere = true;
                    shellfishBorder.BorderBrush = Brushes.Red;
                }
                else if (shellfishBorder.BorderBrush.Equals(Brushes.Red))
                {
                    shellfish.isSelected = false;
                    shellfish.isSevere = false;
                    shellfishBorder.BorderBrush = Brushes.LightGray;
                }
            }
            else if (allergenName.Equals("peanut"))
            {
                if (peanutBorder.BorderBrush.Equals(Brushes.LightGray))
                {
                    peanut.isSelected = true;
                    peanut.isSevere = false;
                    peanutBorder.BorderBrush = Brushes.Orange;
                }
                else if (peanutBorder.BorderBrush.Equals(Brushes.Orange))
                {
                    peanut.isSelected = true;
                    peanut.isSevere = true;
                    peanutBorder.BorderBrush = Brushes.Red;
                }
                else if (peanutBorder.BorderBrush.Equals(Brushes.Red))
                {
                    peanut.isSelected = false;
                    peanut.isSevere = false;
                    peanutBorder.BorderBrush = Brushes.LightGray;
                }
            }
            else if (allergenName.Equals("dairy"))
            {
                if (dairyBorder.BorderBrush.Equals(Brushes.LightGray))
                {
                    dairy.isSelected = true;
                    dairy.isSevere = false;
                    dairyBorder.BorderBrush = Brushes.Orange;
                }
                else if (dairyBorder.BorderBrush.Equals(Brushes.Orange))
                {
                    dairy.isSelected = true;
                    dairy.isSevere = true;
                    dairyBorder.BorderBrush = Brushes.Red;
                }
                else if (dairyBorder.BorderBrush.Equals(Brushes.Red))
                {
                    dairy.isSelected = false;
                    dairy.isSevere = false;
                    dairyBorder.BorderBrush = Brushes.LightGray;
                }
            }
        }

        //Save a new profile
        private void SaveProfileButton_TouchDown(object sender, TouchEventArgs e)
        {
            
            String name;
            Image testImage; //store user's avatar to be displayed elsewhere
            Image testImage2; //store user's avatar to be displayed elsewhere
            alertLabel.Content = ""; 

            //check that a name has been entered and an avatar chosen for the user
            if (NameBox.Text.Equals("") || (momBorder.BorderBrush.Equals(Brushes.LightGray) && dadBorder.BorderBrush.Equals(Brushes.LightGray)
                    && brotherBorder.BorderBrush.Equals(Brushes.LightGray) && sisterBorder.BorderBrush.Equals(Brushes.LightGray)
                    && maidBorder.BorderBrush.Equals(Brushes.LightGray)))
            {
                //check that a name has been entered
                //if not, alert the user
                if (NameBox.Text.Equals("")) 
                {
                    alertLabel.Content += "Please assign a name for the user.\n";
                    alertLabel.Visibility = Visibility.Visible;
                }
                //check that an avatar has been chosen
                //if not, alert the user
                if (momBorder.BorderBrush.Equals(Brushes.LightGray) && dadBorder.BorderBrush.Equals(Brushes.LightGray)
                    && brotherBorder.BorderBrush.Equals(Brushes.LightGray) && sisterBorder.BorderBrush.Equals(Brushes.LightGray)
                    && maidBorder.BorderBrush.Equals(Brushes.LightGray))
                {
                    alertLabel.Content += "Please select an avatar for the user.\n";
                    alertLabel.Visibility = Visibility.Visible;
                }

            }
            else
            {
                name = NameBox.Text;

                //format the avatar to be displayed elsewhere
                testImage = new Image();
                testImage.Source = new BitmapImage(new Uri(avatar, UriKind.Relative));
                testImage.Height = 110;
                testImage.Width = 110;
                testImage2 = new Image();
                testImage2.Source = new BitmapImage(new Uri(avatar, UriKind.Relative));
                testImage2.Height = 100;
                testImage2.Width = 100;

                ArrayList allergens = new ArrayList();
                Allergen newDairy = new Allergen("dairy", dairy.isSevere, dairy.isSelected);
                Allergen newEggs = new Allergen("eggs", eggs.isSevere, eggs.isSelected);                
                Allergen newGluten = new Allergen("gluten", gluten.isSevere, gluten.isSelected);               
                Allergen newSoy= new Allergen("soy",soy.isSevere, soy.isSelected);                
                Allergen newPeanut = new Allergen("peanut",peanut.isSevere, peanut.isSelected);
                Allergen newShellfish = new Allergen("shellfish", shellfish.isSevere, shellfish.isSelected);

                //add all allergens with chosen selection/severity to a collection of allergens
                allergens.Add(newDairy);
                allergens.Add(newEggs);
                allergens.Add(newGluten);
                allergens.Add(newSoy);
                allergens.Add(newPeanut);
                allergens.Add(newShellfish);

                Person newUser = new Person(name, avatar, allergens); //create a new Person with user inputs
                masterGroup.peopleList.Add(newUser); //add new Person to master group of all user profiles

                //create a StackPanel with the new user's name and profile
                //add StackPanel to existing StackPanel displaying all users created thus far
                StackPanel panel = new StackPanel();
                panel.Orientation = Orientation.Vertical;
                panel.Height = 150;
                panel.Width = 150;
                panel.Children.Add(testImage);
                TextBlock text = new TextBlock();
                text.Text = NameBox.Text;
                text.FontSize = 18;
                text.Foreground = Brushes.Black;
                panel.Children.Add(text);
                text.Margin = new Thickness(30,0,0,0);
                allUsers.Children.Add(panel);

                //create a StackPanel with the new user's name and profile
                //add StackPanel to existing StackPanel displaying all users available to select as members of a new group
                StackPanel panel2 = new StackPanel();
                panel2.Orientation = Orientation.Vertical;
                panel2.Height = 120;
                panel2.Width = 120;
                panel2.Children.Add(testImage2);
                TextBlock text2 = new TextBlock();
                text2.Text = NameBox.Text;
                text2.FontSize = 18;
                text2.Foreground = Brushes.Black;
                panel2.Children.Add(text2);
                panel2.TouchDown += new EventHandler<System.Windows.Input.TouchEventArgs>(chooseMembers_touchDown); //dynamically create a new TouchDown event for the new panel 
                text2.Margin = new Thickness(30, 0, 0, 0);
                chooseMembers.Children.Add(panel2);

                //return to customization screen
                alertLabel.Visibility = Visibility.Hidden;
                newPersonGrid.Visibility = Visibility.Hidden;
                customizationGrid.Visibility = Visibility.Visible;
            }

        }
        
        //Select group members
        private void chooseMembers_touchDown(object sender, TouchEventArgs e)
        {

            //save name and pointer to selected member to be accessed later
            TextBlock textBlock = (sender as StackPanel).Children.OfType<TextBlock>().FirstOrDefault();
            String chosenMember = textBlock.Text;
            memberNames.Add(textBlock);

            //check whether user has already been added to the group
            if (textBlock.FontWeight.Equals(FontWeights.Bold))
            {
                for (int i = 0; i < groupMembers.Count; i++)
                {
                    if ((((Person)groupMembers[i]).getName()).Equals(chosenMember))
                    {
                        groupMembers.Remove((Person)groupMembers[i]); //remove the selected user from the new group
                        textBlock.FontWeight = FontWeights.Normal; //unbold the name of the selected user to indicate de-selection
                    }
                }
            }
            else
            {
                for (int i = 0; i < masterGroup.peopleList.Count; i++)
                {
                    //go through list of all users to find the selected user
                    if ((((Person)masterGroup.peopleList[i]).getName()).Equals(chosenMember)) //Cannot add multiple members with the same name!!
                    {
                        groupMembers.Add((Person)masterGroup.peopleList[i]); //add the selected user to the new group
                        textBlock.FontWeight = FontWeights.Bold; //change the name of the selected user to bold to indicate selection
                    }
                }
            }


        }
        
        //Create a new group of profiles
        private void newGroup_TouchDown(object sender, TouchEventArgs e)
        {
            //reset all inputs
            groupMembers.Clear();
            GroupNameBox.Text = "";
            homeBorder.BorderBrush = Brushes.LightGray;
            bikeBorder.BorderBrush = Brushes.LightGray;
            bookclubBorder.BorderBrush = Brushes.LightGray;
            businessBorder.BorderBrush = Brushes.LightGray;
            heartsBorder.BorderBrush = Brushes.LightGray;

            //bring user to New Group screen
            customizationGrid.Visibility = Visibility.Hidden;
            customizationAlertLabel.Visibility = Visibility.Hidden;
            newPersonGrid.Visibility = Visibility.Hidden;
            newGroupGrid.Visibility = Visibility.Visible;
        }

        //Select and set an avatar for a new group
        private void GroupImage_TouchDown(object sender, TouchEventArgs e)
        {
            //get Source of group avatar (as a String)
            Image image = new Image();
            sender.GetType();
            Image photo = (Image)sender;
            String uri = photo.Source.ToString();
            String junk = "pack://application:,,,/Drag and Drop;component/";
            int index = junk.Length;
            String actualUri = uri.Substring(index);
            avatar = actualUri; //save selected image in a global variable

            String chosenAvatar = actualUri.Substring(10);
            chosenAvatar = chosenAvatar.Substring(0, chosenAvatar.Length - 4);

            //de-select any currently selected images
            homeBorder.BorderBrush = Brushes.LightGray;
            bikeBorder.BorderBrush = Brushes.LightGray;
            bookclubBorder.BorderBrush = Brushes.LightGray;
            businessBorder.BorderBrush = Brushes.LightGray;
            heartsBorder.BorderBrush = Brushes.LightGray;

            //select chosen image
            if (chosenAvatar.Equals("home"))
            {
                homeBorder.BorderBrush = Brushes.DarkCyan;
            }
            else if (chosenAvatar.Equals("bike"))
            {
                bikeBorder.BorderBrush = Brushes.DarkCyan;
            }
            else if (chosenAvatar.Equals("bookclub"))
            {
                bookclubBorder.BorderBrush = Brushes.DarkCyan;
            }
            else if (chosenAvatar.Equals("business"))
            {
                businessBorder.BorderBrush = Brushes.DarkCyan;
            }
            else if (chosenAvatar.Equals("hearts"))
            {
                heartsBorder.BorderBrush = Brushes.DarkCyan;
            }

        }

        //Save a new group
        private void SaveGroupButton_TouchDown(object sender, TouchEventArgs e)
        {
            String name;
            Image testImage;
            groupAlertLabel.Content = "";

            //Make sure name has been assigned and avatar selected
            if (GroupNameBox.Text.Equals("") || (homeBorder.BorderBrush.Equals(Brushes.LightGray) && bikeBorder.BorderBrush.Equals(Brushes.LightGray)
                    && bookclubBorder.BorderBrush.Equals(Brushes.LightGray) && businessBorder.BorderBrush.Equals(Brushes.LightGray)
                    && heartsBorder.BorderBrush.Equals(Brushes.LightGray)))
            {
                if (GroupNameBox.Text.Equals(""))
                {
                    groupAlertLabel.Content += "Please assign a name for the group.\n";
                    groupAlertLabel.Visibility = Visibility.Visible;
                }
                if (homeBorder.BorderBrush.Equals(Brushes.LightGray) && bikeBorder.BorderBrush.Equals(Brushes.LightGray)
                    && bookclubBorder.BorderBrush.Equals(Brushes.LightGray) && businessBorder.BorderBrush.Equals(Brushes.LightGray)
                    && heartsBorder.BorderBrush.Equals(Brushes.LightGray))
                {
                    groupAlertLabel.Content += "Please select an avatar for the group.\n";
                    groupAlertLabel.Visibility = Visibility.Visible;
                }

            }
            else
            {
                //set group name
                name = GroupNameBox.Text;

                //set group avatar
                testImage = new Image();
                testImage.Source = new BitmapImage(new Uri(avatar, UriKind.Relative));
                testImage.Height = 110;
                testImage.Width = 110;

                //create new group with user inputs
                //**Bug here: global var groupMembers gets modified each time a new group is created, thus changing the members of all the existing groups too
                Group newGroup = new Group(name, avatar, groupMembers); 
                createdGroups.Add(newGroup); //add new group to collection of all groups

                for (int i=0; i<memberNames.Count; i++) {
                    ((TextBlock)memberNames[i]).FontWeight = FontWeights.Normal; //visually deselect all selected users
                }

                //create new StackPanel of group name and avatar to add to StackPanel of existing groups
                StackPanel panel = new StackPanel();
                panel.Orientation = Orientation.Vertical;
                panel.Height = 150;
                panel.Width = 150;
                panel.Children.Add(testImage);
                TextBlock text = new TextBlock();
                text.Text = GroupNameBox.Text;
                text.TextAlignment = TextAlignment.Center;
                text.FontSize = 20;
                text.Foreground = Brushes.Black;
                allGroupNames.Add(text);
                panel.Children.Add(text);
                text.Margin = new Thickness(30, 0, 0, 0);
                panel.Margin = new Thickness(0, 0, 15, 0);
                panel.TouchDown += new EventHandler<System.Windows.Input.TouchEventArgs>(selectGroup_TouchDown); //dynamically add TouchDown event for the new panel
                allGroups.Children.Add(panel);

                //return to the Customization screen
                alertLabel.Visibility = Visibility.Hidden;
                newPersonGrid.Visibility = Visibility.Hidden;
                newGroupGrid.Visibility = Visibility.Hidden;
                customizationGrid.Visibility = Visibility.Visible;
            }
        }

        //Choose a group to cook for
        private void selectGroup_TouchDown(object sender, TouchEventArgs e)
        {
            allAllergens.Clear();

            for (int i = 0; i < allGroupNames.Count; i++)
            {
                ((TextBlock)allGroupNames[i]).FontWeight = FontWeights.Normal; //visually deselect all selected users
            }

            //set selectedGroup variable to chosen group
            //set allergen list to that of selected group
            TextBlock textBlock = (sender as StackPanel).Children.OfType<TextBlock>().FirstOrDefault();
            String chosenGroup = textBlock.Text;

            for (int i = 0; i < createdGroups.Count; i++)
            {
                if (((Group)createdGroups[i]).groupName.Equals(chosenGroup))
                {
                    selectedGroup = (Group)createdGroups[i];
                    textBlock.FontWeight = FontWeights.ExtraBold; //indicate to user that group has been selected
                    
                }
            }

            customizationAlertLabel.Visibility = Visibility.Hidden;
            allAllergens = selectedGroup.getAllAllergens(); //set allergens to those of selected group
        }

        //Switch to cooking screen
        private void startCooking_TouchDown(object sender, TouchEventArgs e)
        {
            if (selectedGroup == null) //check that the user has actually chosen a group to cook for 
            {
                customizationAlertLabel.Content = "Please select a group to cook for";
                customizationAlertLabel.Visibility = Visibility.Visible;
            }
            else {
                groupName.FontWeight = FontWeights.Normal;

                newPersonGrid.Visibility = Visibility.Hidden;
                customizationGrid.Visibility = Visibility.Hidden;
                cookingGrid.Visibility = Visibility.Visible;

                Image groupImage = new Image();
                groupImage.Source = new BitmapImage(new Uri(selectedGroup.photoSource, UriKind.Relative));

                //update "Cooking For" icon in sidebar
                cookingFor.Source = groupImage.Source;
                cookingFor2.Source = groupImage.Source;
                cookingFor3.Source = groupImage.Source;

                customizationAlertLabel.Visibility = Visibility.Hidden;
            }

        }

        //Allow user to choose new group to cook for
        private void cookingFor_TouchDown(object sender, TouchEventArgs e)
        {
            cookingGrid.Visibility = Visibility.Hidden;
            newPersonGrid.Visibility = Visibility.Hidden;
            historyGrid.Visibility = Visibility.Hidden;
            awareGrid.Visibility = Visibility.Hidden;
            customizationGrid.Visibility = Visibility.Visible;
        }

        //Clear surface of all allergens
        //Indicate in history that surface has been cleaned
        private void reset_TouchDown(object sender, TouchEventArgs e)
        {
            //Return to neutral background and clear list of allergens
            cookingGrid.Background = Brushes.DarkCyan;
            clean.Visibility = Visibility.Visible;
            warning.Content = "";
            warning.Visibility = Visibility.Hidden;

            //add "cleaned" info to History
            DateTime currentTime = DateTime.Now;
            TextBlock newDate = new TextBlock();
            newDate.Text = currentTime.ToShortDateString();
            newDate.FontSize = 18;
            newDate.Foreground = Brushes.Black;
            date.Children.Add(newDate);

            TextBlock newTime = new TextBlock();
            newTime.Text = currentTime.ToLongTimeString();
            newTime.FontSize = 18;
            newTime.Foreground = Brushes.Black;
            time.Children.Add(newTime);

            TextBlock reset = new TextBlock();
            reset.Text = "*Surface cleaned*";
            reset.FontSize = 18;
            reset.Foreground = Brushes.Black;
            reset.FontStyle = FontStyles.Italic;
            allergen.Children.Add(reset);
        }

        //View what allergens have recently been used on the surface
        private void history_TouchDown(object sender, TouchEventArgs e)
        {
            cookingGrid.Visibility = Visibility.Hidden;
            customizationGrid.Visibility = Visibility.Hidden;
            newPersonGrid.Visibility = Visibility.Hidden;
            historyGrid.Visibility = Visibility.Visible;
            awareGrid.Visibility = Visibility.Hidden;

        }

        //Return to cooking screen from current screen
        private void keepCooking_Click(object sender, RoutedEventArgs e)
        {
            cookingGrid.Visibility = Visibility.Visible;
            newPersonGrid.Visibility = Visibility.Hidden;
            historyGrid.Visibility = Visibility.Hidden;
            awareGrid.Visibility = Visibility.Hidden;
            customizationGrid.Visibility = Visibility.Hidden;
        }

        //View currently selected allergens
        private void selectedAllergens_TouchDown(object sender, TouchEventArgs e)
        {
            //reset images of aware allergens
            awareImages.Children.Clear();

            if (((Allergen)allAllergens[0]).isSelected) //dairy
            {
                Image testImage = new Image();
                testImage.Source = new BitmapImage(new Uri("Resources/dairy.png", UriKind.Relative));
                testImage.Height = 120;
                testImage.Width = 120;
                testImage.Margin = new Thickness(0, 15, 0, 15);

                //display selected allergen icons in the stack panel of aware allergens
                //display **SEVERE** text if allergy is severe
                StackPanel panel = new StackPanel();
                panel.Orientation = Orientation.Vertical;
                panel.Height = 200;
                panel.Width = 200;
                panel.Children.Add(testImage);
                if (((Allergen)allAllergens[0]).isSevere)
                {
                    TextBlock text = new TextBlock();
                    text.Text = "**SEVERE**";
                    text.FontSize = 22;
                    text.FontWeight = FontWeights.Bold;
                    text.Foreground = Brushes.Black;
                    text.Margin = new Thickness(30, 0, 0, 0);
                    panel.Children.Add(text);
                }
                panel.Margin = new Thickness(0, 0, 15, 0);
                awareImages.Children.Add(panel);
            }

                if (((Allergen)allAllergens[1]).isSelected) //eggs
                {
                    Image testImage = new Image();
                    testImage.Source = new BitmapImage(new Uri("Resources/eggs.png", UriKind.Relative));
                    testImage.Height = 120;
                    testImage.Width = 120;
                    testImage.Margin = new Thickness(0, 15, 0, 15);

                    StackPanel panel = new StackPanel();
                    panel.Orientation = Orientation.Vertical;
                    panel.Height = 200;
                    panel.Width = 200;
                    panel.Children.Add(testImage);
                    if (((Allergen)allAllergens[1]).isSevere)
                    {
                        TextBlock text = new TextBlock();
                        text.Text = "**SEVERE**";
                        text.FontSize = 22;
                        text.FontWeight = FontWeights.Bold;
                        text.Foreground = Brushes.Black;
                        text.Margin = new Thickness(30, 0, 0, 0);
                        panel.Children.Add(text);
                    }
                    panel.Margin = new Thickness(0, 0, 15, 0);
                    awareImages.Children.Add(panel);
                }

                if (((Allergen)allAllergens[2]).isSelected) //gluten
                {
                    Image testImage = new Image();
                    testImage.Source = new BitmapImage(new Uri("Resources/gluten.png", UriKind.Relative));
                    testImage.Height = 120;
                    testImage.Width = 120;
                    testImage.Margin = new Thickness(0, 15, 0, 15);

                    StackPanel panel = new StackPanel();
                    panel.Orientation = Orientation.Vertical;
                    panel.Height = 200;
                    panel.Width = 200;
                    panel.Children.Add(testImage);
                    if (((Allergen)allAllergens[2]).isSevere)
                    {
                        TextBlock text = new TextBlock();
                        text.Text = "**SEVERE**";
                        text.FontSize = 22;
                        text.FontWeight = FontWeights.Bold;
                        text.Foreground = Brushes.Black;
                        text.Margin = new Thickness(30, 0, 0, 0);
                        panel.Children.Add(text);
                    }
                    panel.Margin = new Thickness(0, 0, 15, 0);
                    awareImages.Children.Add(panel);
                }
                if (((Allergen)allAllergens[3]).isSelected) //soy
                {
                    Image testImage = new Image();
                    testImage.Source = new BitmapImage(new Uri("Resources/soy.png", UriKind.Relative));
                    testImage.Height = 120;
                    testImage.Width = 120;
                    testImage.Margin = new Thickness(0, 15, 0, 15);

                    StackPanel panel = new StackPanel();
                    panel.Orientation = Orientation.Vertical;
                    panel.Height = 200;
                    panel.Width = 200;
                    panel.Children.Add(testImage);
                    if (((Allergen)allAllergens[3]).isSevere)
                    {
                        TextBlock text = new TextBlock();
                        text.Text = "**SEVERE**";
                        text.FontSize = 22;
                        text.FontWeight = FontWeights.Bold;
                        text.Foreground = Brushes.Black;
                        text.Margin = new Thickness(30, 0, 0, 0);
                        panel.Children.Add(text);
                    }
                    panel.Margin = new Thickness(0, 0, 15, 0);
                    awareImages.Children.Add(panel);
                }
                if (((Allergen)allAllergens[4]).isSelected) //nuts
                {
                    Image testImage = new Image();
                    testImage.Source = new BitmapImage(new Uri("Resources/peanut.png", UriKind.Relative));
                    testImage.Height = 120;
                    testImage.Width = 120;
                    testImage.Margin = new Thickness(0, 15, 0, 15);

                    StackPanel panel = new StackPanel();
                    panel.Orientation = Orientation.Vertical;
                    panel.Height = 200;
                    panel.Width = 200;
                    panel.Children.Add(testImage);
                    if (((Allergen)allAllergens[4]).isSevere)
                    {
                        TextBlock text = new TextBlock();
                        text.Text = "**SEVERE**";
                        text.FontSize = 22;
                        text.FontWeight = FontWeights.Bold;
                        text.Foreground = Brushes.Black;
                        text.Margin = new Thickness(30, 0, 0, 0);
                        panel.Children.Add(text);
                    }
                    panel.Margin = new Thickness(0, 0, 15, 0);
                    awareImages.Children.Add(panel);
                }
                if (((Allergen)allAllergens[5]).isSelected) //shellfish
                {
                    Image testImage = new Image();
                    testImage.Source = new BitmapImage(new Uri("Resources/shellfish.png", UriKind.Relative));
                    testImage.Height = 120;
                    testImage.Width = 120;
                    testImage.Margin = new Thickness(0, 15, 0, 15);

                    StackPanel panel = new StackPanel();
                    panel.Orientation = Orientation.Vertical;
                    panel.Height = 200;
                    panel.Width = 200;
                    panel.Children.Add(testImage);
                    if (((Allergen)allAllergens[5]).isSevere)
                    {
                        TextBlock text = new TextBlock();
                        text.Text = "**SEVERE**";
                        text.FontSize = 22;
                        text.FontWeight = FontWeights.Bold;
                        text.Foreground = Brushes.Black;
                        text.Margin = new Thickness(30, 0, 0, 0);
                        panel.Children.Add(text);
                    }
                    panel.Margin = new Thickness(0, 0, 15, 0);
                    awareImages.Children.Add(panel);
                }


            cookingGrid.Visibility = Visibility.Hidden;
            customizationGrid.Visibility = Visibility.Hidden;
            newPersonGrid.Visibility = Visibility.Hidden;
            historyGrid.Visibility = Visibility.Hidden;
            awareGrid.Visibility = Visibility.Visible;
        }

        //Implement surface response to allergens
        private void TagVisualizer_VisualizationAdded(object sender, TagVisualizerEventArgs e)
        {
            DateTime currentTime = DateTime.Now;
            string tag = e.TagVisualization.VisualizedTag.Value.ToString();

            if (((tag.Equals("2")) || (tag.Equals("3"))) & (((Allergen)allAllergens[2]).isSelected)) //gluten
            {
                //Add allergen to history
                TextBlock newDate = new TextBlock();
                newDate.Text = currentTime.ToShortDateString();
                newDate.FontSize = 18;
                newDate.Foreground = Brushes.Black;
                date.Children.Add(newDate);

                TextBlock newTime = new TextBlock();
                newTime.Text = currentTime.ToLongTimeString();
                newTime.FontSize = 18;
                newTime.Foreground = Brushes.Black;
                time.Children.Add(newTime);

                TextBlock newAllergen = new TextBlock();
                newAllergen.Text = "Gluten";
                newAllergen.FontSize = 18;
                newAllergen.Foreground = Brushes.Black;
                allergen.Children.Add(newAllergen);

                //Make cooking grid respond to allergen
                //Severe alert
                if (((Allergen)allAllergens[2]).isSevere)
                {
                    cookingGrid.Background = Brushes.Red;
                    warning.Content += "\nCaution: Gluten present **SEVERE**";
                }
                //Regular alert
                else
                {
                    //do not change background color if severe alert has been triggered
                    if (!cookingGrid.Background.Equals(Brushes.Red))
                    {
                        cookingGrid.Background = Brushes.Orange;
                        
                    }
                    
                }
                clean.Visibility = Visibility.Hidden;
                warning.Visibility = Visibility.Visible;
                warning.Content += "\nCaution: Gluten present";
                
            }
            
            else if (tag.Equals("4") & (((Allergen)allAllergens[3]).isSelected)) //soy
            {
                //Add allergen to history
                TextBlock newDate = new TextBlock();
                newDate.Text = currentTime.ToShortDateString();
                newDate.FontSize = 18;
                newDate.Foreground = Brushes.Black;
                date.Children.Add(newDate);

                TextBlock newTime = new TextBlock();
                newTime.Text = currentTime.ToLongTimeString();
                newTime.FontSize = 18;
                newTime.Foreground = Brushes.Black;
                time.Children.Add(newTime);

                TextBlock newAllergen = new TextBlock();
                newAllergen.Text = "Soy";
                newAllergen.FontSize = 18;
                newAllergen.Foreground = Brushes.Black;
                allergen.Children.Add(newAllergen);

                //Make cooking grid respond to allergen
                //Severe alert
                if (((Allergen)allAllergens[3]).isSevere)
                {
                    cookingGrid.Background = Brushes.Red;
                    warning.Content += "\nCaution: Soy present **SEVERE**";
                }
                //Regular alert
                else
                {
                    //do not change background color if severe alert has been triggered
                    if (!cookingGrid.Background.Equals(Brushes.Red))
                    {
                        cookingGrid.Background = Brushes.Orange;
                        
                    }
                    clean.Visibility = Visibility.Hidden;
                    warning.Visibility = Visibility.Visible;
                    warning.Content += "\nCaution: Soy present";
                }
            }
            else if (tag.Equals("5") & (((Allergen)allAllergens[0]).isSelected)) //dairy
            {
                //Add allergen to history
                TextBlock newDate = new TextBlock();
                newDate.Text = currentTime.ToShortDateString();
                newDate.FontSize = 18;
                newDate.Foreground = Brushes.Black;
                date.Children.Add(newDate);

                TextBlock newTime = new TextBlock();
                newTime.Text = currentTime.ToLongTimeString();
                newTime.FontSize = 18;
                newTime.Foreground = Brushes.Black;
                time.Children.Add(newTime);

                TextBlock newAllergen = new TextBlock();
                newAllergen.Text = "Dairy";
                newAllergen.FontSize = 18;
                newAllergen.Foreground = Brushes.Black;
                allergen.Children.Add(newAllergen);

                //Make cooking grid respond to allergen
                //Severe alert
                if (((Allergen)allAllergens[0]).isSevere)
                {
                    cookingGrid.Background = Brushes.Red;
                    warning.Content += "\nCaution: Dairy present **SEVERE**";
                }
                //Regular alert
                else
                {
                    //do not change background color if severe alert has been triggered
                    if (!cookingGrid.Background.Equals(Brushes.Red))
                    {
                        cookingGrid.Background = Brushes.Orange;
                        
                    }
                    clean.Visibility = Visibility.Hidden;
                    warning.Visibility = Visibility.Visible;
                    warning.Content += "\nCaution: Dairy present";
                }
            }
            else if (tag.Equals("6") & (((Allergen)allAllergens[4]).isSelected)) //nuts
            {
                //Add allergen to history
                TextBlock newDate = new TextBlock();
                newDate.Text = currentTime.ToShortDateString();
                newDate.FontSize = 18;
                newDate.Foreground = Brushes.Black;
                date.Children.Add(newDate);

                TextBlock newTime = new TextBlock();
                newTime.Text = currentTime.ToLongTimeString();
                newTime.FontSize = 18;
                newTime.Foreground = Brushes.Black;
                time.Children.Add(newTime);

                TextBlock newAllergen = new TextBlock();
                newAllergen.Text = "Nuts";
                newAllergen.FontSize = 18;
                newAllergen.Foreground = Brushes.Black;
                allergen.Children.Add(newAllergen);

                //Make cooking grid respond to allergen
                //Severe alert
                if (((Allergen)allAllergens[4]).isSevere)
                {
                    cookingGrid.Background = Brushes.Red;
                    warning.Content += "\nCaution: Nuts present **SEVERE**";
                }
                //Regular alert
                else
                {
                    //do not change background color if severe alert has been triggered
                    if (!cookingGrid.Background.Equals(Brushes.Red))
                    {
                        cookingGrid.Background = Brushes.Orange;
                        
                    }
                    clean.Visibility = Visibility.Hidden;
                    warning.Visibility = Visibility.Visible;
                    warning.Content += "\nCaution: Nuts present";
                }
            }
            else if (tag.Equals("7") & (((Allergen)allAllergens[1]).isSelected)) //eggs
            {
                //Add allergen to history
                TextBlock newDate = new TextBlock();
                newDate.Text = currentTime.ToShortDateString();
                newDate.FontSize = 18;
                newDate.Foreground = Brushes.Black;
                date.Children.Add(newDate);

                TextBlock newTime = new TextBlock();
                newTime.Text = currentTime.ToLongTimeString();
                newTime.FontSize = 18;
                newTime.Foreground = Brushes.Black;
                time.Children.Add(newTime);

                TextBlock newAllergen = new TextBlock();
                newAllergen.Text = "Eggs";
                newAllergen.FontSize = 18;
                newAllergen.Foreground = Brushes.Black;
                allergen.Children.Add(newAllergen);

                //Make cooking grid respond to allergen
                //Severe alert
                if (((Allergen)allAllergens[1]).isSevere)
                {
                    cookingGrid.Background = Brushes.Red;
                    warning.Content += "\nCaution: Eggs present **SEVERE**";
                }
                //Regular alert
                else
                {
                    //do not change background color if severe alert has been triggered
                    if (!cookingGrid.Background.Equals(Brushes.Red))
                    {
                        cookingGrid.Background = Brushes.Orange;
                        
                    }
                    clean.Visibility = Visibility.Hidden;
                    warning.Visibility = Visibility.Visible;
                    warning.Content += "\nCaution: Eggs present";
                }
            }
            else if (tag.Equals("8") & (((Allergen)allAllergens[5]).isSelected)) //shellfish
            {
                //Add allergen to history
                TextBlock newDate = new TextBlock();
                newDate.Text = currentTime.ToShortDateString();
                newDate.FontSize = 18;
                newDate.Foreground = Brushes.Black;
                date.Children.Add(newDate);

                TextBlock newTime = new TextBlock();
                newTime.Text = currentTime.ToLongTimeString();
                newTime.FontSize = 18;
                newTime.Foreground = Brushes.Black;
                time.Children.Add(newTime);

                TextBlock newAllergen = new TextBlock();
                newAllergen.Text = "Shellfish";
                newAllergen.FontSize = 18;
                newAllergen.Foreground = Brushes.Black;
                allergen.Children.Add(newAllergen);

                //Make cooking grid respond to allergen
                //Severe alert
                if (((Allergen)allAllergens[5]).isSevere)
                {
                    cookingGrid.Background = Brushes.Red;
                    warning.Content += "\nCaution: Shellfish present **SEVERE**";
                }
                //Regular alert
                else
                {
                    //do not change background color if severe alert has been triggered
                    if (!cookingGrid.Background.Equals(Brushes.Red))
                    {
                        cookingGrid.Background = Brushes.Orange;
                    }
                    clean.Visibility = Visibility.Hidden;
                    warning.Visibility = Visibility.Visible;
                    warning.Content += "\nCaution: Shellfish present";
                }
            }





            /*
             *CHANGE TAG IDs FOR THE DEMO
            if ((tag.Equals("1")) & (((Allergen)allAllergens[0]).isSelected)) //dairy
            {
                //Add allergen to history
                TextBlock newDate = new TextBlock();
                newDate.Text = currentTime.ToShortDateString();
                newDate.FontSize = 18;
                newDate.Foreground = Brushes.Black;
                date.Children.Add(newDate);

                TextBlock newTime = new TextBlock();
                newTime.Text = currentTime.ToLongTimeString();
                newTime.FontSize = 18;
                newTime.Foreground = Brushes.Black;
                time.Children.Add(newTime);

                TextBlock newAllergen = new TextBlock();
                newAllergen.Text = "Dairy";
                newAllergen.FontSize = 18;
                newAllergen.Foreground = Brushes.Black;
                allergen.Children.Add(newAllergen);

                //Make cooking grid respond to allergen
                //Severe alert
                if (((Allergen)allAllergens[0]).isSevere)
                {
                    cookingGrid.Background = Brushes.Red;
                    //warning.Content += "\n**Caution: Dairy present** SEVERE";
                }
                //Regular alert
                else
                {
                    //do not change background color if severe alert has been triggered
                    if (!cookingGrid.Background.Equals(Brushes.Red))
                    {
                        cookingGrid.Background = Brushes.Orange;
                    }
                    //warning.Content += "\nCaution: Dairy present";
                }
                //clean.Visibility = Visibility.Hidden;
                //warning.Visibility = Visibility.Visible;


            }
            else if (tag.Equals("2") & (((Allergen)allAllergens[1]).isSelected)) //eggs
            {
                //Add allergen to history
                TextBlock newDate = new TextBlock();
                newDate.Text = currentTime.ToShortDateString();
                newDate.FontSize = 18;
                newDate.Foreground = Brushes.Black;
                date.Children.Add(newDate);

                TextBlock newTime = new TextBlock();
                newTime.Text = currentTime.ToLongTimeString();
                newTime.FontSize = 18;
                newTime.Foreground = Brushes.Black;
                time.Children.Add(newTime);

                TextBlock newAllergen = new TextBlock();
                newAllergen.Text = "Eggs";
                newAllergen.FontSize = 18;
                newAllergen.Foreground = Brushes.Black;
                allergen.Children.Add(newAllergen);

                //Make cooking grid respond to allergen
                //Severe alert
                if (((Allergen)allAllergens[1]).isSevere)
                {
                    cookingGrid.Background = Brushes.Red;
                    //warning.Content += "\n**Caution: Eggs present** SEVERE";
                }
                //Regular alert
                else
                {
                    //do not change background color if severe alert has been triggered
                    if (!cookingGrid.Background.Equals(Brushes.Red))
                    {
                        cookingGrid.Background = Brushes.Orange;
                    }
                    //warning.Content += "\nCaution: Eggs present";

                }
                //clean.Visibility = Visibility.Hidden;
                //warning.Visibility = Visibility.Visible;

            }
            else if (tag.Equals("3") & (((Allergen)allAllergens[2]).isSelected)) //gluten
            {
                //Add allergen to history
                TextBlock newDate = new TextBlock();
                newDate.Text = currentTime.ToShortDateString();
                newDate.FontSize = 18;
                newDate.Foreground = Brushes.Black;
                date.Children.Add(newDate);

                TextBlock newTime = new TextBlock();
                newTime.Text = currentTime.ToLongTimeString();
                newTime.FontSize = 18;
                newTime.Foreground = Brushes.Black;
                time.Children.Add(newTime);

                TextBlock newAllergen = new TextBlock();
                newAllergen.Text = "Gluten";
                newAllergen.FontSize = 18;
                newAllergen.Foreground = Brushes.Black;
                allergen.Children.Add(newAllergen);

                //Make cooking grid respond to allergen
                //Severe alert
                if (((Allergen)allAllergens[2]).isSevere)
                {
                    cookingGrid.Background = Brushes.Red;
                    //warning.Content += "\n**Caution: Gluten present** SEVERE";
                }
                //Regular alert
                else
                {
                    //do not change background color if severe alert has been triggered
                    if (!cookingGrid.Background.Equals(Brushes.Red))
                    {
                        cookingGrid.Background = Brushes.Orange;
                    }
                    //clean.Visibility = Visibility.Hidden;
                    //warning.Visibility = Visibility.Visible;
                    //warning.Content += "\nCaution: Gluten present";
                }
            }
            else if (tag.Equals("4") & (((Allergen)allAllergens[3]).isSelected)) //soy
            {
                //Add allergen to history
                TextBlock newDate = new TextBlock();
                newDate.Text = currentTime.ToShortDateString();
                newDate.FontSize = 18;
                newDate.Foreground = Brushes.Black;
                date.Children.Add(newDate);

                TextBlock newTime = new TextBlock();
                newTime.Text = currentTime.ToLongTimeString();
                newTime.FontSize = 18;
                newTime.Foreground = Brushes.Black;
                time.Children.Add(newTime);

                TextBlock newAllergen = new TextBlock();
                newAllergen.Text = "Soy";
                newAllergen.FontSize = 18;
                newAllergen.Foreground = Brushes.Black;
                allergen.Children.Add(newAllergen);

                //Make cooking grid respond to allergen
                //Severe alert
                if (((Allergen)allAllergens[3]).isSevere)
                {
                    cookingGrid.Background = Brushes.Red;
                    //warning.Content += "\n**Caution: Soy present** SEVERE";
                }
                //Regular alert
                else
                {
                    //do not change background color if severe alert has been triggered
                    if (!cookingGrid.Background.Equals(Brushes.Red))
                    {
                        cookingGrid.Background = Brushes.Orange;
                    }
                    //clean.Visibility = Visibility.Hidden;
                    //warning.Visibility = Visibility.Visible;
                    //warning.Content += "\nCaution: Soy present";
                }
            }
            else if (tag.Equals("5") & (((Allergen)allAllergens[4]).isSelected)) //nuts
            {
                //Add allergen to history
                TextBlock newDate = new TextBlock();
                newDate.Text = currentTime.ToShortDateString();
                newDate.FontSize = 18;
                newDate.Foreground = Brushes.Black;
                date.Children.Add(newDate);

                TextBlock newTime = new TextBlock();
                newTime.Text = currentTime.ToLongTimeString();
                newTime.FontSize = 18;
                newTime.Foreground = Brushes.Black;
                time.Children.Add(newTime);

                TextBlock newAllergen = new TextBlock();
                newAllergen.Text = "Nuts";
                newAllergen.FontSize = 18;
                newAllergen.Foreground = Brushes.Black;
                allergen.Children.Add(newAllergen);

                //Make cooking grid respond to allergen
                //Severe alert
                if (((Allergen)allAllergens[4]).isSevere)
                {
                    cookingGrid.Background = Brushes.Red;
                    //warning.Content += "\n**Caution: Nuts present** SEVERE";
                }
                //Regular alert
                else
                {
                    //do not change background color if severe alert has been triggered
                    if (!cookingGrid.Background.Equals(Brushes.Red))
                    {
                        cookingGrid.Background = Brushes.Orange;
                    }
                    //clean.Visibility = Visibility.Hidden;
                    //warning.Visibility = Visibility.Visible;
                    //warning.Content += "\nCaution: Nuts present";
                }
            }
            else if (tag.Equals("6") & (((Allergen)allAllergens[5]).isSelected)) //shellfish
            {
                //Add allergen to history
                TextBlock newDate = new TextBlock();
                newDate.Text = currentTime.ToShortDateString();
                newDate.FontSize = 18;
                newDate.Foreground = Brushes.Black;
                date.Children.Add(newDate);

                TextBlock newTime = new TextBlock();
                newTime.Text = currentTime.ToLongTimeString();
                newTime.FontSize = 18;
                newTime.Foreground = Brushes.Black;
                time.Children.Add(newTime);

                TextBlock newAllergen = new TextBlock();
                newAllergen.Text = "Shellfish";
                newAllergen.FontSize = 18;
                newAllergen.Foreground = Brushes.Black;
                allergen.Children.Add(newAllergen);

                //Make cooking grid respond to allergen
                //Severe alert
                if (((Allergen)allAllergens[5]).isSevere)
                {
                    cookingGrid.Background = Brushes.Red;
                    //warning.Content += "\n**Caution: Shellfish present** SEVERE";
                }
                //Regular alert
                else
                {
                    //do not change background color if severe alert has been triggered
                    if (!cookingGrid.Background.Equals(Brushes.Red))
                    {
                        cookingGrid.Background = Brushes.Orange;
                    }
                    //clean.Visibility = Visibility.Hidden;
                    //warning.Visibility = Visibility.Visible;
                    //warning.Content += "\nCaution: Shellfish present";
                }
            }
             */

        }



    }
}
