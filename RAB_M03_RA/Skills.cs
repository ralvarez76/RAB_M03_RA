#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

#endregion

namespace RAB_M03_RA
{
    [Transaction(TransactionMode.Manual)]
    public class Skills : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // this is a variable for the Revit application
            UIApplication uiapp = commandData.Application;

            // this is a variable for the current Revit model
            Document doc = uiapp.ActiveUIDocument.Document;

            // 2. Create instances of class - v1
            Building theater = new Building("Grand Opera House", "5 Main Street", 4, 35000);
            Building hotel = new Building("Fancy Hotel", "10 Main Street", 10, 100000);
            Building office = new Building("Big Office Building", "15 Main Street", 15, 150000);
            //theater.Name = "Grand Opera House";
            //theater.Address = "5 Main Street";
            //theater.NumFloors = 4;
            //theater.Area = 35000;

            // 3. Create list of buildings
            List<Building> buildingList = new List<Building>();
            buildingList.Add(theater);
            buildingList.Add(hotel);
            buildingList.Add(office);
            buildingList.Add(new Building("Hospital", "20 Main Street", 20, 350000));

            // 6. Create instance of class and use method
            Neighborhood downtown = new Neighborhood("Downtown", "Middletown", "CT", buildingList);

            TaskDialog.Show("Test", $"There are {downtown.GetBuildingCount()} " + $"buildings in the {downtown.Name} neighborhood.");

            // 7. Working with Rooms
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfCategory(BuiltInCategory.OST_Rooms);

            // 8. Instert family
            FamilySymbol curFS = Utils.GetFamilySymbolByName(doc, "Desk", "60\" x 30\"");

            using (Transaction t = new Transaction(doc))
            {
                t.Start("Insert family into room");

                // 9. Activate family symbol
                curFS.Activate();

                foreach (SpatialElement room in collector)
                {
                    LocationPoint loc = room.Location as LocationPoint;
                    XYZ roomPoint = loc.Point as XYZ;

                    FamilyInstance curFI = doc.Create.NewFamilyInstance(roomPoint, curFS, StructuralType.NonStructural);

                    // 10. Get parameter value
                    string name = Utils.GetParameterValueAsString(room, "Department");

                    // 1.. Set parameter values
                    Utils.SetParameterValue(room, "Floor Finish", "CT");
                }
                t.Commit();

                // 11. string splitting
                string myLine = "one, two, three, four, five";
                string[] splitLine = myLine.Split(',');
                TaskDialog.Show("Test", splitLine[0].Trim());
                TaskDialog.Show("Test", splitLine[3].Trim());
            }

            return Result.Succeeded;
        }

       
        internal static PushButtonData GetButtonData()
        {
            // use this method to define the properties for this command in the Revit ribbon
            string buttonInternalName = "btnCommand1";
            string buttonTitle = "Button 1";

            ButtonDataClass myButtonData1 = new ButtonDataClass(
                buttonInternalName,
                buttonTitle,
                MethodBase.GetCurrentMethod().DeclaringType?.FullName,
                Properties.Resources.Blue_32,
                Properties.Resources.Blue_16,
                "This is a tooltip for Button 1");

            return myButtonData1.Data;
        }
    }

    public class Building
    {
        public string Name { get; set; }

        public string Address { get; set; }

        public int NumFloors { get; set; }

        public double Area { get; set; }

        public Building (string _name, string _address, int _numFloors, double _area)
        {
            Name = _name;
            Address = _address;
            NumFloors = _numFloors;
            Area = _area;
        }
    }

    public class Neighborhood
    { 
        public string Name { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public List<Building> BuildingList { get; set; }

        public Neighborhood (string _name, string _city, string _State, List<Building> _buildings)
        {
            Name = _name;
            City = _city;
            State = _State;
            BuildingList = _buildings;
        }

        // 5. Add method to class
        public int GetBuildingCount()
        {
            return BuildingList.Count;
        }
    }
}
