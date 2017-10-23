using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using GeometryGym.Ifc;
using System.Text.RegularExpressions;

namespace IFC.Examples
{
    /// <summary>
    /// Author: Tom Hennersdorf
    /// 
    /// Creates just the door. 
    /// Without enclosing elements!
    /// </summary>
    class DoorJust
    {
        public string GetDoorIFC()
        {
            int line = 200;
            int openingIndex = 188;
            int localIndex = 20;
            int buildingIndex = 12;
            int ownerHistoryIndex = 10;

            double widht = 1300.0;
            double height = 2100;
            double thickness = 100;

            // ==== Create Database ====
            DatabaseIfc db = new DatabaseIfc(false, ModelView.If2x3NotAssigned);
            db.Factory.Options.GenerateOwnerHistory = false;
            db.Factory.Options.AngleUnitsInRadians = true;
            db.NextObjectRecord = line;
            db.Release = ReleaseVersion.IFC2x3;

            // ==== Create OpeningElement just for the ID. ====
            IfcOpeningElement openingElement = new IfcOpeningElement(db);
            openingElement.Index = openingIndex;


            // ==== local placement ====
            IfcLocalPlacement localPlacement = new IfcLocalPlacement(localIndex, new IfcAxis2Placement3D(new IfcCartesianPoint(db, 0, 0, 0)));

            // ==== MappedRepresentation ====
            // Body
            IfcFace f1 = IfcFace.GenFace(db, new IfcCartesianPoint(db, 0, 0, 0), new IfcCartesianPoint(db, 860, 0, 0), new IfcCartesianPoint(db, 860, 0, 2110), new IfcCartesianPoint(db, 0, 0, 2110));
            IfcFace f2 = IfcFace.GenFace(db, new IfcCartesianPoint(db, 0, 0, 0), new IfcCartesianPoint(db, 0, -100, 0), new IfcCartesianPoint(db, 0, -100, 2110), new IfcCartesianPoint(db, 0, 0, 2110));
            IfcFace f3 = IfcFace.GenFace(db, new IfcCartesianPoint(db, 860, 0, 0), new IfcCartesianPoint(db, 860, -100, 0), new IfcCartesianPoint(db, 860, -100, 2110), new IfcCartesianPoint(db, 860, 0, 2110));
            IfcFace f4 = IfcFace.GenFace(db, new IfcCartesianPoint(db, 0, -100, 0), new IfcCartesianPoint(db, 860, -100, 0), new IfcCartesianPoint(db, 860, -100, 2110), new IfcCartesianPoint(db, 0, -100, 2110));

            // doorway
            //double length = 1300;
            double lX = 860;
            double lY = 0;
            double lXRotation = -0.64278761 * lY + 0.76604444 * lX;
            double lYRotation = 0.76604444 * lY + 0.64278761 * lX;

            IfcFace f5 = IfcFace.GenFace(db, new IfcCartesianPoint(db, 0, 0, 0),
                new IfcCartesianPoint(db, lXRotation, lYRotation, 0), new IfcCartesianPoint(db, lXRotation, lYRotation, 2110), new IfcCartesianPoint(db, 0, 0, 2110));

            IfcConnectedFaceSet connectedFaceSet = new IfcConnectedFaceSet(new List<IfcFace>() { f1, f2, f3, f4, f5 });

            IfcFaceBasedSurfaceModel faceBasedSurfaceModel = new IfcFaceBasedSurfaceModel(new List<IfcConnectedFaceSet>() { connectedFaceSet });

            IfcRepresentationMap representationMap = new IfcRepresentationMap(faceBasedSurfaceModel);

            IfcCartesianTransformationOperator3DnonUniform trans = new IfcCartesianTransformationOperator3DnonUniform(db);
            trans.Scale = widht / 860.0;
            trans.Scale2 = thickness / 100.0;
            trans.Scale3 = height / 2110.0;

            IfcMappedItem mappedItem = new IfcMappedItem(representationMap, trans);
            IfcShapeRepresentation shapeRepresentation = new IfcShapeRepresentation(mappedItem);
            IfcProductRepresentation productRepresesentation = new IfcProductRepresentation(shapeRepresentation);

            // ==== Door ====
            IfcDoor door = new IfcDoor(db, openingIndex, localPlacement, productRepresesentation);
            door.SetOwnerHistoryIndex(ownerHistoryIndex);
            door.OverallWidth = widht;
            door.OverallHeight = height;
            door.Name = "door"+ widht.ToString("0") + "x" + height.ToString("0");

            string ifcString = extractDoorEntities(db);

            return ifcString;
        }

        private static string extractDoorEntities(DatabaseIfc db)
        {
            string ifcElements = db.GetEntitiesString();
            List<String> ifcList = new List<string>(ifcElements.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries));

            List<string> patterns = new List<string>() { "^#[0-9]*= IFCOPENINGELEMENT", "^#[0-9]*= IFCPERSON", "^#[0-9]*= IFCORGANIZATION"
            , "^#[0-9]*= IFCPERSONANDORGANIZATION", "^#[0-9]*= IFCAPPLICATION", "^#[0-9]*= IFCOWNERHISTORY"};

            for (int i = 0; i < ifcList.Count; )
            {
                bool contin = false;

                foreach (string pattern in patterns)
                {
                    if (Regex.IsMatch(ifcList[i], pattern, RegexOptions.IgnoreCase))
                    {
                        ifcList.RemoveAt(i);
                        contin = true;
                        break;
                    }
                }

                if (!contin)
                    i++;
            }

            string result = "";
            foreach (string ifcElem in ifcList)
            {
                result += ifcElem + "\r\n";
            }
            return result;
        }
    }
}
