using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAB_M03_RA
{
    public class FurnitureType
    {
        public string Name { get; set; }
        public string FamilyName { get; set; }
        public string TypeName { get; set; }

        public FurnitureType(string _name, string _familyname, string _typename)
        {
            Name = _name;
            FamilyName = _familyname;
            TypeName = _typename;
        }
    }

    public class FurnitureSet
    {
        public string Set { get; set; }
        public string RoomType { get; set; }
        public string[] Furniture { get; set; }

        public FurnitureSet(string _set, string _roomType, string _furnlist)
        {
            Set = _set;
            RoomType = _roomType;
            Furniture = _furnlist.Split(',');
        }

        public int GetFurnitureCount()
        {
            return Furniture.Length;
        }
    }
}
