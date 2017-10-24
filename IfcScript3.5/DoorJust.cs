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
        public enum DoorSwing
        {
            SingleSwingLeft,
            SingleSwingLeftOtherSide,
            SingleSwingRight,
            SingleSwingRightOtherSide,
            DoubleSwing,
            DoubleSwingOtherSide
        }

        public class Vector3D{
            public Vector3D(double x, double y, double z)
            {
                this.X = x;
                this.Y = y;
                this.Z = z;
            }
            public double X;
            public double Y;
            public double Z;
        }

        public class Polygon {
            public Polygon(List<Vector3D> geometry){
                this.Geometry = geometry;
            }
            public List<Vector3D> Geometry;
        }

        public class Door
        {
            public Door()
            {
                Widht = 1300.0;
                Height = 2100;
                Thickness = 100;

                Bottom = new Polygon(new List<Vector3D>() {
                new Vector3D(0,0,0), 
                new Vector3D(0, -Thickness, 0), 
                new Vector3D(Widht, -Thickness, 0),
                new Vector3D(Widht,0,0), 
                new Vector3D(0,0,0)});

                Dir = new Vector3D(1, 0, 0);

                DoorSwing = DoorSwing.SingleSwingLeft;
            }

            public double Widht;
            public double Height;
            public double Thickness;
            public Polygon Bottom;
            public Vector3D Dir;

            public DoorSwing DoorSwing;
        }

        public string GetDoorIFC()
        {
            int doorIndex = -1;
            int line = 200;
            int ownerHistoryIndex = 10;
            int wallIndex = 188;

            Door door = new Door();
            
            // ==== Create Database ====
            DatabaseIfc db = new DatabaseIfc(false, ModelView.If2x3NotAssigned);
            db.Factory.Options.GenerateOwnerHistory = false;
            db.Factory.Options.AngleUnitsInRadians = true;
            db.NextObjectRecord = line;
            db.Release = ReleaseVersion.IFC2x3;

            return genDoor(db, ownerHistoryIndex, wallIndex, door, ref doorIndex);
        }
        private static string genDoor(DatabaseIfc db, int ownerHistoryIndex, int wallIndex, Door door, ref int doorIndex)
        {
            IfcOpeningElement openingElement = genOpening(db, wallIndex, door);

            // ==== local placement ====
            IfcCartesianPoint cartesianPoint = null;
            IfcDirection directionZ = new IfcDirection(db, 0, 0, 1);
            IfcDirection directionX = null;

            switch (door.DoorSwing)
            {
                case DoorSwing.SingleSwingLeft:
                case DoorSwing.SingleSwingRight:
                case DoorSwing.DoubleSwing:
                    cartesianPoint = new IfcCartesianPoint(db, 0, -door.Thickness, 0);
                    directionX = new IfcDirection(db, 1, 0, 0);
                    break;
                case DoorSwing.SingleSwingLeftOtherSide:
                case DoorSwing.SingleSwingRightOtherSide:
                case DoorSwing.DoubleSwingOtherSide:
                    cartesianPoint = new IfcCartesianPoint(db, door.Widht, 0, 0);
                    directionX = new IfcDirection(db, -1, 0, 0);
                    break;
            }

            IfcLocalPlacement localPlacement = new IfcLocalPlacement(openingElement.Placement, new IfcAxis2Placement3D(cartesianPoint, directionZ, directionX));

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

            IfcFace f5 = null;
            IfcFace f6 = null;

            switch (door.DoorSwing)
            {
                case DoorSwing.SingleSwingLeft:
                case DoorSwing.SingleSwingLeftOtherSide:
                    f5 = IfcFace.GenFace(db, new IfcCartesianPoint(db, 0, 100, 0),
                         new IfcCartesianPoint(db, lXRotation, lYRotation, 0), new IfcCartesianPoint(db, lXRotation, lYRotation, 2110), new IfcCartesianPoint(db, 0, 100, 2110));
                    break;
                case DoorSwing.SingleSwingRight:
                case DoorSwing.SingleSwingRightOtherSide:
                    lXRotation = -0.64278761 * lY - 0.76604444 * lX + 860;

                    f5 = IfcFace.GenFace(db, new IfcCartesianPoint(db, 860, 100, 0),
                         new IfcCartesianPoint(db, lXRotation, lYRotation, 0), new IfcCartesianPoint(db, lXRotation, lYRotation, 2110), new IfcCartesianPoint(db, 860, 100, 2110));
                    break;
                case DoorSwing.DoubleSwing:
                case DoorSwing.DoubleSwingOtherSide:
                    lX = 430;

                    lXRotation = -0.64278761 * lY + 0.76604444 * lX;
                    lYRotation = 0.76604444 * lY + 0.64278761 * lX;

                    f5 = IfcFace.GenFace(db, new IfcCartesianPoint(db, 0, 100, 0),
                         new IfcCartesianPoint(db, lXRotation, lYRotation, 0), new IfcCartesianPoint(db, lXRotation, lYRotation, 2110), new IfcCartesianPoint(db, 0, 100, 2110));

                    lXRotation = -0.64278761 * lY - 0.76604444 * lX + 860;

                    f6 = IfcFace.GenFace(db, new IfcCartesianPoint(db, 860, 100, 0),
                         new IfcCartesianPoint(db, lXRotation, lYRotation, 0), new IfcCartesianPoint(db, lXRotation, lYRotation, 2110), new IfcCartesianPoint(db, 860, 100, 2110));
                    break;
            }

            List<IfcFace> facesList = new List<IfcFace>() { f1, f2, f3, f4, f5 };
            if (f6 != null)
                facesList.Add(f6);

            IfcConnectedFaceSet connectedFaceSet = new IfcConnectedFaceSet(facesList);

            IfcFaceBasedSurfaceModel faceBasedSurfaceModel = new IfcFaceBasedSurfaceModel(new List<IfcConnectedFaceSet>() { connectedFaceSet });

            IfcRepresentationMap representationMap = new IfcRepresentationMap(faceBasedSurfaceModel);

            IfcCartesianTransformationOperator3DnonUniform trans = new IfcCartesianTransformationOperator3DnonUniform(db);
            trans.Scale = door.Widht / 860.0;
            trans.Scale2 = door.Thickness / 100.0;
            trans.Scale3 = door.Height / 2110.0;

            IfcMappedItem mappedItem = new IfcMappedItem(representationMap, trans);
            IfcShapeRepresentation shapeRepresentation = new IfcShapeRepresentation(mappedItem);
            IfcProductRepresentation productRepresesentation = new IfcProductRepresentation(shapeRepresentation);

            // ==== Door ====
            IfcDoor ifcDoor = new IfcDoor(db, openingElement.Index, localPlacement, productRepresesentation);
            ifcDoor.SetOwnerHistoryIndex(ownerHistoryIndex);
            ifcDoor.OverallWidth = door.Widht;
            ifcDoor.OverallHeight = door.Height;
            ifcDoor.Name = "door_" + door.Widht.ToString("0") + "x" + door.Height.ToString("0");
            doorIndex = ifcDoor.Index;// for IFCRELCONTAINEDINSPATIALSTRUCTURE

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

            IfcDoorStyle doorStyle = new IfcDoorStyle(ifcDoor, doorTypeOperation, IfcDoorStyleConstructionEnum.NOTDEFINED, false, false);

            string ifcString = extractDoorEntities(db);

            return ifcString;
        }

        private static IfcOpeningElement genOpening(DatabaseIfc db, int wallIndex, Door door)
        {
            // ==== local placement ====
            IfcCartesianPoint cartesianPoint = new IfcCartesianPoint(db, door.Bottom.Geometry[0].X, door.Bottom.Geometry[0].Y, door.Bottom.Geometry[0].Z);
            IfcDirection directionZ = new IfcDirection(db, 0, 0, 1);
            IfcDirection directionX = new IfcDirection(db, door.Dir.X, door.Dir.Y, door.Dir.Z);

            IfcLocalPlacement localPlacement = new IfcLocalPlacement(wallIndex, new IfcAxis2Placement3D(cartesianPoint, directionZ, directionX));

            // ==== Body ====
            List<Vector3D> bottomPolyList = door.Bottom.Geometry;
            IfcCartesianPoint p1 = new IfcCartesianPoint(db, bottomPolyList[0].X, bottomPolyList[0].Y, bottomPolyList[0].Z);
            IfcCartesianPoint p2 = new IfcCartesianPoint(db, bottomPolyList[1].X, bottomPolyList[1].Y, bottomPolyList[1].Z);
            IfcCartesianPoint p3 = new IfcCartesianPoint(db, bottomPolyList[2].X, bottomPolyList[2].Y, bottomPolyList[2].Z);
            IfcCartesianPoint p4 = new IfcCartesianPoint(db, bottomPolyList[3].X, bottomPolyList[3].Y, bottomPolyList[3].Z);
            IfcCartesianPoint p5 = new IfcCartesianPoint(db, bottomPolyList[4].X, bottomPolyList[4].Y, bottomPolyList[4].Z);

            IfcPolyline ifcPolylineBody = new IfcPolyline(new List<IfcCartesianPoint>() { p1, p2, p3, p4, p5 });

            IfcArbitraryClosedProfileDef arbitraryClosedProfileDef = new IfcArbitraryClosedProfileDef("", ifcPolylineBody);

            IfcExtrudedAreaSolid extrudedAreaSolid = new IfcExtrudedAreaSolid(arbitraryClosedProfileDef, new IfcAxis2Placement3D(new IfcCartesianPoint(db, 0, 0, 0)), door.Height);

            IfcShapeRepresentation bodyShape = new IfcShapeRepresentation(extrudedAreaSolid);

            IfcProductRepresentation productRepresesentation = new IfcProductRepresentation(bodyShape);

            // ==== Opening ====
            return new IfcOpeningElement(db, wallIndex, localPlacement, productRepresesentation);
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
