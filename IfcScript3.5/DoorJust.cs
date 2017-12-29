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
            // parameter
            int line = 200;
            Door door = new Door();
            door.DoorSwing = DoorSwing.SingleSwingLeft;
            int ownerHistoryIndex = 10;
            int doorIndex = -1;
            
            // variables in class
            int wallIndex = 188;
            
            
            // ==== Create Database ====
            DatabaseIfc db = new DatabaseIfc(false, ModelView.If2x3NotAssigned);
            db.Factory.Options.GenerateOwnerHistory = false;
            db.Factory.Options.AngleUnitsInRadians = true;
            db.NextObjectRecord = line;
            db.Release = ReleaseVersion.IFC2x3;

            // Position and direction
            IfcDirection directionX = new IfcDirection(db, 1, 0, 0);
            IfcCartesianPoint ifcCartesianPoint =  new IfcCartesianPoint(db, 500, 0, 0);

            genDoor(door, directionX, ifcCartesianPoint,
                db, ownerHistoryIndex, ref doorIndex,
                wallIndex);

            string ifcString = extractDoorEntities(db);

            return ifcString;
        }

        private void genDoor(Door door, IfcDirection directionXOpening, IfcCartesianPoint cartesianPointOpening,
            DatabaseIfc db, int ownerHistoryIndex, ref int doorIndex, 
            int wallIndex)
        {
            IfcOpeningElement openingElement = 
                genOpeningForDoor(door, directionXOpening, cartesianPointOpening,
                db, wallIndex);


            // ==== local placement ====
            IfcCartesianPoint cartesianPointDoor = null;
            IfcDirection directionZ = new IfcDirection(db, 0, 0, 1);
            IfcDirection directionXDoor = new IfcDirection(db, 1, 0, 0);
            switch (door.DoorSwing)
            {
                case DoorSwing.SingleSwingLeft:
                case DoorSwing.SingleSwingRight:
                case DoorSwing.DoubleSwing:
                    cartesianPointDoor = new IfcCartesianPoint(db, 0, -door.Thickness, 0);
                    directionXDoor = new IfcDirection(db, 1, 0, 0);
                    break;
                case DoorSwing.SingleSwingLeftOtherSide:
                case DoorSwing.SingleSwingRightOtherSide:
                case DoorSwing.DoubleSwingOtherSide:
                    cartesianPointDoor = new IfcCartesianPoint(db, door.Width, 0, 0);
                    directionXDoor = new IfcDirection(db, -1, 0, 0);
                    break;
            }

            IfcLocalPlacement localPlacement = new IfcLocalPlacement(openingElement.Placement, new IfcAxis2Placement3D(cartesianPointDoor, directionZ, directionXDoor));


            // ==== MappedRepresentation ====
            // Body
            IfcFace f1 = IfcFace.GenFace(db, new IfcCartesianPoint(db, 0, 0, 0), new IfcCartesianPoint(db, 860, 0, 0), new IfcCartesianPoint(db, 860, 0, 2110), new IfcCartesianPoint(db, 0, 0, 2110));
            IfcFace f2 = IfcFace.GenFace(db, new IfcCartesianPoint(db, 0, 0, 0), new IfcCartesianPoint(db, 0, 100, 0), new IfcCartesianPoint(db, 0, 100, 2110), new IfcCartesianPoint(db, 0, 0, 2110));
            IfcFace f3 = IfcFace.GenFace(db, new IfcCartesianPoint(db, 860, 0, 0), new IfcCartesianPoint(db, 860, 100, 0), new IfcCartesianPoint(db, 860, 100, 2110), new IfcCartesianPoint(db, 860, 0, 2110));
            IfcFace f4 = IfcFace.GenFace(db, new IfcCartesianPoint(db, 0, 100, 0), new IfcCartesianPoint(db, 860, 100, 0), new IfcCartesianPoint(db, 860, 100, 2110), new IfcCartesianPoint(db, 0, 100, 2110));
            // bottom
            IfcFace f5 = IfcFace.GenFace(db, new IfcCartesianPoint(db, 0, 0, 0), new IfcCartesianPoint(db, 860, 0, 0), new IfcCartesianPoint(db, 860, 100, 0), new IfcCartesianPoint(db, 0, 100, 0));
            // face up
            IfcFace f6 = IfcFace.GenFace(db, new IfcCartesianPoint(db, 0, 0, 2110), new IfcCartesianPoint(db, 860, 0, 2110), new IfcCartesianPoint(db, 860, 100, 2110), new IfcCartesianPoint(db, 0, 100, 2110));

            // door swing
            double lX = 860;
            double lY = 0;
            double lXRotation = -0.64278761 * lY + 0.76604444 * lX;
            double lYRotation = 0.76604444 * lY + 0.64278761 * lX;

            // door plank
            IfcFace f7 = null;
            // If double door, the 2. face
            IfcFace f8 = null;

            switch (door.DoorSwing)
            {
                case DoorSwing.SingleSwingLeft:
                case DoorSwing.SingleSwingLeftOtherSide:
                    f7 = IfcFace.GenFace(db, new IfcCartesianPoint(db, 0, 100, 0),
                         new IfcCartesianPoint(db, lXRotation, lYRotation, 0), new IfcCartesianPoint(db, lXRotation, lYRotation, 2110), new IfcCartesianPoint(db, 0, 100, 2110));
                    break;
                case DoorSwing.SingleSwingRight:
                case DoorSwing.SingleSwingRightOtherSide:
                    lXRotation = -0.64278761 * lY - 0.76604444 * lX + 860;

                    f7 = IfcFace.GenFace(db, new IfcCartesianPoint(db, 860, 100, 0),
                         new IfcCartesianPoint(db, lXRotation, lYRotation, 0), new IfcCartesianPoint(db, lXRotation, lYRotation, 2110), new IfcCartesianPoint(db, 860, 100, 2110));
                    break;
                case DoorSwing.DoubleSwing:
                case DoorSwing.DoubleSwingOtherSide:
                    lX = 430;

                    lXRotation = -0.64278761 * lY + 0.76604444 * lX;
                    lYRotation = 0.76604444 * lY + 0.64278761 * lX;

                    f7 = IfcFace.GenFace(db, new IfcCartesianPoint(db, 0, 100, 0),
                         new IfcCartesianPoint(db, lXRotation, lYRotation, 0), new IfcCartesianPoint(db, lXRotation, lYRotation, 2110), new IfcCartesianPoint(db, 0, 100, 2110));

                    lXRotation = -0.64278761 * lY - 0.76604444 * lX + 860;

                    f8 = IfcFace.GenFace(db, new IfcCartesianPoint(db, 860, 100, 0),
                         new IfcCartesianPoint(db, lXRotation, lYRotation, 0), new IfcCartesianPoint(db, lXRotation, lYRotation, 2110), new IfcCartesianPoint(db, 860, 100, 2110));
                    break;
            }

            List<IfcFace> facesList = new List<IfcFace>() { f1, f2, f3, f4, f7, f5, f6 };
            if (f8 != null)
                facesList.Add(f8);

            IfcConnectedFaceSet connectedFaceSet = new IfcConnectedFaceSet(facesList);

            IfcFaceBasedSurfaceModel faceBasedSurfaceModel = new IfcFaceBasedSurfaceModel(new List<IfcConnectedFaceSet>() { connectedFaceSet });

            IfcRepresentationMap representationMap = new IfcRepresentationMap(faceBasedSurfaceModel);

            IfcCartesianTransformationOperator3DnonUniform trans = new IfcCartesianTransformationOperator3DnonUniform(db);
            trans.Scale = door.Width / 860.0;
            trans.Scale2 = door.Thickness / 100.0;
            trans.Scale3 = door.Height / 2110.0;

            IfcMappedItem mappedItem = new IfcMappedItem(representationMap, trans);
            IfcShapeRepresentation shapeRepresentation = new IfcShapeRepresentation(mappedItem);
            IfcProductRepresentation productRepresesentation = new IfcProductRepresentation(shapeRepresentation);

            // ==== Door ====
            IfcDoor ifcDoor = new IfcDoor(db, openingElement.Index, localPlacement, productRepresesentation);
            ifcDoor.OverallWidth = door.Width;
            ifcDoor.OverallHeight = door.Height;
            ifcDoor.SetOwnerHistoryIndex(ownerHistoryIndex);
            doorIndex = ifcDoor.Index;

            // ==== IfcDoorStyle ====
            IfcDoorTypeOperationEnum doorTypeOperation = IfcDoorTypeOperationEnum.NOTDEFINED;
            switch (door.DoorSwing)
            {
                case DoorSwing.SingleSwingLeft:
                case DoorSwing.SingleSwingLeftOtherSide:
                    doorTypeOperation = IfcDoorTypeOperationEnum.SINGLE_SWING_LEFT;
                    break;
                case DoorSwing.SingleSwingRight:
                case DoorSwing.SingleSwingRightOtherSide:
                    doorTypeOperation = IfcDoorTypeOperationEnum.SINGLE_SWING_RIGHT;
                    break;
                case DoorSwing.DoubleSwing:
                case DoorSwing.DoubleSwingOtherSide:
                    doorTypeOperation = IfcDoorTypeOperationEnum.DOUBLE_DOOR_DOUBLE_SWING;
                    break;
            }

            ifcDoor.Name = "Door " + door.Width.ToString("0") + "x" + door.Height.ToString("0");

            IfcDoorStyle doorStyle = new IfcDoorStyle(ifcDoor, doorTypeOperation, IfcDoorStyleConstructionEnum.NOTDEFINED, false, false);
        }

        private static IfcOpeningElement genOpeningForDoor(
            Door door, IfcDirection directionX, IfcCartesianPoint cartesianPoint,
            DatabaseIfc db, int wallIndex)
        {
            // ==== local placement ====
            IfcDirection directionZ = new IfcDirection(db, 0, 0, 1);
            IfcLocalPlacement localPlacement = new IfcLocalPlacement(wallIndex, new IfcAxis2Placement3D(cartesianPoint, directionZ, directionX));

            // ==== Body ====
            IfcCartesianPoint p1 = new IfcCartesianPoint(db, 0, 0, 0);
            IfcCartesianPoint p2 = new IfcCartesianPoint(db, 0, -door.Thickness, 0);
            IfcCartesianPoint p3 = new IfcCartesianPoint(db, door.Width, -door.Thickness, 0);
            IfcCartesianPoint p4 = new IfcCartesianPoint(db, door.Width, 0, 0);
            IfcCartesianPoint p5 = new IfcCartesianPoint(db, 0, 0, 0);

            IfcPolyline ifcPolylineBody = new IfcPolyline(new List<IfcCartesianPoint>() { p1, p2, p3, p4, p5 });

            IfcArbitraryClosedProfileDef arbitraryClosedProfileDef = new IfcArbitraryClosedProfileDef("", ifcPolylineBody);

            IfcExtrudedAreaSolid extrudedAreaSolid = new IfcExtrudedAreaSolid(arbitraryClosedProfileDef, new IfcAxis2Placement3D(new IfcCartesianPoint(db, 0, 0, 0)), door.Height);

            IfcShapeRepresentation bodyShape = new IfcShapeRepresentation(extrudedAreaSolid);

            IfcProductRepresentation productRepresesentation = new IfcProductRepresentation(bodyShape);

            // ==== Opening ====
            IfcOpeningElement openingElement = new IfcOpeningElement(db, wallIndex, localPlacement, productRepresesentation);
            openingElement.Name = "Opening";
            return openingElement;
        }

        private static string extractDoorEntities(DatabaseIfc db)
        {
            string ifcElements = db.GetEntitiesString();
            List<String> ifcList = new List<string>(ifcElements.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries));

            List<string> patterns = new List<string>() {"^#[0-9]*= IFCPERSON", "^#[0-9]*= IFCORGANIZATION"
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
