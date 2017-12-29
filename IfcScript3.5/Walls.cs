using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using GeometryGym.Ifc;

namespace IFC.Examples
{
    /// <summary>
    /// Author: Tom Hennersdorf
    /// Build elements, which function in PaletteCAD.
    /// 
    /// Here are:
    /// - 2 walls
    /// - 4 door 
    ///     - with mapped representation
    ///     - different opening directions
    /// - 1 window with mapped representation
    /// </summary>
    class Walls : IFCExampleInstance
    {
        protected override void GenerateInstance(IfcBuilding building)
        {
            DatabaseIfc db = building.Database;
            building.Name = "Building";


            // ==== Material ====
            IfcMaterial material = new IfcMaterial(db, "Material");
            IfcMaterialLayer materialLayer = new IfcMaterialLayer(material, 100, "");
            materialLayer.IsVentilated = IfcLogicalEnum.UNKNOWN;
            IfcMaterialLayerSet materialLayerSet = new IfcMaterialLayerSet(new List<IfcMaterialLayer>() { materialLayer }, "");
            IfcMaterialLayerSetUsage layerSetUsage = new IfcMaterialLayerSetUsage(materialLayerSet,
                IfcLayerSetDirectionEnum.AXIS2, IfcDirectionSenseEnum.NEGATIVE, 0);

            genWallForDoor(building, layerSetUsage);
            genWallForWindow(building, layerSetUsage);
        }

        private void genWallForDoor(IfcBuilding building, IfcMaterialLayerSetUsage layerSetUsage)
        {
            DatabaseIfc db = building.Database;

            // ==== local placement ====
            IfcCartesianPoint cartesianPoint = new IfcCartesianPoint(db, 0, 1000, 0);
            IfcDirection directionZ = new IfcDirection(db, 0, 0, 1);
            IfcDirection directionX = new IfcDirection(db, 1, 0, 0);

            IfcAxis2Placement3D localPlacement = new IfcAxis2Placement3D(cartesianPoint, directionZ, directionX);

            // ==================
            // ==== Geometry ====
            // ==================
            List<IfcShapeModel> shapeModelList = new List<IfcShapeModel>();

            // ==== Axis ====
            IfcCartesianPoint point1 = new IfcCartesianPoint(db, 0, 0, 0);
            IfcCartesianPoint point2 = new IfcCartesianPoint(db, 8000, 0, 0);

            IfcPolyline polyline = new IfcPolyline(new List<IfcCartesianPoint>() { point1, point2 });
            IfcShapeRepresentation axisShape = IfcShapeRepresentation.GetAxisRep(polyline);
            shapeModelList.Add(axisShape);

            // ==== Body ====
            IfcCartesianPoint p1 = new IfcCartesianPoint(db, 0, 0, 0);
            IfcCartesianPoint p2 = new IfcCartesianPoint(db, -100, -100, 0);
            IfcCartesianPoint p3 = new IfcCartesianPoint(db, 8100, -100, 0);
            IfcCartesianPoint p4 = new IfcCartesianPoint(db, 8000, 0, 0);
            IfcCartesianPoint p5 = new IfcCartesianPoint(db, 0, 0, 0);


            IfcPolyline ifcPolylineBody = new IfcPolyline(new List<IfcCartesianPoint>() { p1, p2, p3, p4, p5 });

            IfcArbitraryClosedProfileDef arbitraryClosedProfileDef = new IfcArbitraryClosedProfileDef("", ifcPolylineBody);

            IfcExtrudedAreaSolid extrudedAreaSolid = new IfcExtrudedAreaSolid(arbitraryClosedProfileDef, new IfcAxis2Placement3D(new IfcCartesianPoint(db, 0, 0, 0)), 2800);

            IfcShapeRepresentation bodyShape = new IfcShapeRepresentation(extrudedAreaSolid);

            shapeModelList.Add(bodyShape);

            // ==== Wall ====
            IfcProductDefinitionShape productDefinitionShape = new IfcProductDefinitionShape(shapeModelList);
            IfcWallStandardCase wallStandardCase = new IfcWallStandardCase(building, layerSetUsage, localPlacement, productDefinitionShape);
            wallStandardCase.Name = "Wall with doors";


            IfcDirection directionXDoor = new IfcDirection(db, 1, 0, 0);

            Door door1 = new Door();
            door1.DoorSwing = DoorSwing.SingleSwingLeft;
            genDoor(building, wallStandardCase, door1, directionXDoor, new IfcCartesianPoint(db, 300 + 1500, 0, 0));

            Door door2 = new Door();
            door2.DoorSwing = DoorSwing.SingleSwingLeftOtherSide;
            genDoor(building, wallStandardCase, door2, directionXDoor, new IfcCartesianPoint(db, 1800 + 1500, 0, 0));

            Door door3 = new Door();
            door3.DoorSwing = DoorSwing.SingleSwingRight;
            genDoor(building, wallStandardCase, door3, directionXDoor, new IfcCartesianPoint(db, 3300 + 1500, 0, 0));

            Door door4 = new Door();
            door4.DoorSwing = DoorSwing.DoubleSwingOtherSide;
            genDoor(building, wallStandardCase, door4, directionXDoor, new IfcCartesianPoint(db, 4800 + 1500, 0, 0));
            db.NextObjectRecord = 100;
        }

        private void genWallForWindow(IfcBuilding building, IfcMaterialLayerSetUsage layerSetUsage)
        {
            DatabaseIfc db = building.Database;

            // ==== local placement ====
            IfcCartesianPoint cartesianPoint = new IfcCartesianPoint(db, 0, 1000, 0);
            IfcDirection directionZ = new IfcDirection(db, 0, 0, 1);
            IfcDirection directionX = new IfcDirection(db, 0, 1, 0);

            IfcAxis2Placement3D axis2Placement3D = new IfcAxis2Placement3D(cartesianPoint, directionZ, directionX);

            // ==================
            // ==== Geometry ====
            // ==================
            List<IfcShapeModel> shapeModelList = new List<IfcShapeModel>();

            // ==== Axis ====
            IfcCartesianPoint point1 = new IfcCartesianPoint(db, 0, 0, 0);
            IfcCartesianPoint point2 = new IfcCartesianPoint(db, 8000, 0, 0);

            IfcPolyline polyline = new IfcPolyline(new List<IfcCartesianPoint>() { point1, point2 });
            IfcShapeRepresentation axisShape = IfcShapeRepresentation.GetAxisRep(polyline);
            shapeModelList.Add(axisShape);

            // ==== Body ====
            IfcCartesianPoint p1 = new IfcCartesianPoint(db, 0, 0, 0);
            IfcCartesianPoint p2 = new IfcCartesianPoint(db, -100, 100, 0);
            IfcCartesianPoint p3 = new IfcCartesianPoint(db, 8100, 100, 0);
            IfcCartesianPoint p4 = new IfcCartesianPoint(db, 8000, 0, 0);
            IfcCartesianPoint p5 = new IfcCartesianPoint(db, 0, 0, 0);


            IfcPolyline ifcPolylineBody = new IfcPolyline(new List<IfcCartesianPoint>() { p1, p2, p3, p4, p5 });

            IfcArbitraryClosedProfileDef arbitraryClosedProfileDef = new IfcArbitraryClosedProfileDef("", ifcPolylineBody);

            IfcExtrudedAreaSolid extrudedAreaSolid = new IfcExtrudedAreaSolid(arbitraryClosedProfileDef, new IfcAxis2Placement3D(new IfcCartesianPoint(db, 0, 0, 0)), 2800);

            IfcShapeRepresentation bodyShape = new IfcShapeRepresentation(extrudedAreaSolid);

            shapeModelList.Add(bodyShape);

            // ==== Wall ====
            IfcProductDefinitionShape productDefinitionShape = new IfcProductDefinitionShape(shapeModelList);
            IfcWallStandardCase wallStandardCase = new IfcWallStandardCase(building, layerSetUsage, axis2Placement3D, productDefinitionShape);
            wallStandardCase.Name = "Wall with window";

            genWindow(building, wallStandardCase);
            db.NextObjectRecord = 100;
        }

        private void genDoor(IfcBuilding building, IfcWallStandardCase ifcWall, Door door, IfcDirection directionXOpening, IfcCartesianPoint cartesianPointOpening)
        {
            IfcOpeningElement openingElement = genOpeningForDoor(ifcWall, door, directionXOpening, cartesianPointOpening);

            DatabaseIfc db = openingElement.Database;

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

            //IfcFaceBasedSurfaceModel faceBasedSurfaceModel = new IfcFaceBasedSurfaceModel(new List<IfcConnectedFaceSet>() { connectedFaceSet });
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
            IfcDoor ifcDoor = new IfcDoor(building, openingElement, localPlacement, productRepresesentation);
            ifcDoor.OverallWidth = door.Width;
            ifcDoor.OverallHeight = door.Height;

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

            //switch (door.DoorSwingV)
            //{
            //    case DoorSwing.SingleSwingLeft:
            //        ifcDoor.Name = "Door single swing left";
            //        break;
            //    case DoorSwing.SingleSwingLeftOtherSide:
            //        ifcDoor.Name = "Door single swing left other side";
            //        break;
            //    case DoorSwing.SingleSwingRight:
            //        ifcDoor.Name = "Door single swing right";
            //        break;
            //    case DoorSwing.SingleSwingRightOtherSide:
            //        ifcDoor.Name = "Door single swing right other side";
            //        break;
            //    case DoorSwing.DoubleSwing:
            //        ifcDoor.Name = "Door double swing";
            //        break;
            //    case DoorSwing.DoubleSwingOtherSide:
            //        ifcDoor.Name = "Door double swing other side";
            //        break;
            //}

            IfcDoorStyle doorStyle = new IfcDoorStyle(ifcDoor, doorTypeOperation, IfcDoorStyleConstructionEnum.NOTDEFINED, false, false);

            // ==== transparency ====
            IfcColourRgb ifcColourRgb = new IfcColourRgb(db, 0.2, 0.2, 0.9);

            IfcSurfaceStyleRendering ifcSurfaceStyleRendering = new IfcSurfaceStyleRendering(ifcColourRgb);//, 0.8, new IfcNormalisedRatioMeasure( 0.1 ), null, null, 
            ifcSurfaceStyleRendering.Transparency = 0.8;
            ifcSurfaceStyleRendering.DiffuseColour = new IfcNormalisedRatioMeasure(0.1);
            ifcSurfaceStyleRendering.SpecularColour = new IfcNormalisedRatioMeasure(0.1);

            IfcSurfaceStyle ifcSurfaceStyle = new IfcSurfaceStyle(ifcSurfaceStyleRendering);
            ifcSurfaceStyle.Name = "blue transparency";

            IfcStyledItem ifcStyledItem = new IfcStyledItem(ifcSurfaceStyle);
            ifcStyledItem.Item = faceBasedSurfaceModel;
        }

        private static IfcOpeningElement genOpeningForDoor(IfcWallStandardCase ifcWall, Door door,
            IfcDirection directionX, IfcCartesianPoint cartesianPoint)
        {
            DatabaseIfc db = ifcWall.Database;

            // ==== local placement ====
            IfcDirection directionZ = new IfcDirection(db, 0, 0, 1);
            IfcLocalPlacement localPlacement = new IfcLocalPlacement(ifcWall.Placement, new IfcAxis2Placement3D(cartesianPoint, directionZ, directionX));

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
            IfcOpeningElement openingElement = new IfcOpeningElement(ifcWall, localPlacement, productRepresesentation);
            openingElement.Name = "Opening";
            return openingElement;
        }

        private void genWindow(IfcBuilding building, IfcWallStandardCase wall)
        {
            IfcOpeningElement openingElement = genOpeningForWindow(wall);

            DatabaseIfc db = openingElement.Database;

            // ==== local placement ====
            IfcLocalPlacement localPlacement = new IfcLocalPlacement(openingElement.Placement, new IfcAxis2Placement3D(new IfcCartesianPoint(db, 0, 0, 0)));

            // ==== MappedRepresentation ====

            // Body
            List<IfcFace> faces = new List<IfcFace>() {
            IfcFace.GenFace(db, new IfcCartesianPoint(db, 0, 0, 0), new IfcCartesianPoint(db, 1000, 0, 0), new IfcCartesianPoint(db, 890, 0, 110), new IfcCartesianPoint(db, 110, 0, 110)),
            IfcFace.GenFace(db, new IfcCartesianPoint(db, 110, 0, 110), new IfcCartesianPoint(db, 890, 0, 110), new IfcCartesianPoint(db, 880, 30, 120), new IfcCartesianPoint(db, 120, 30, 120)),
            IfcFace.GenFace(db, new IfcCartesianPoint(db, 120, 30, 120), new IfcCartesianPoint(db, 880, 30, 120), new IfcCartesianPoint(db, 900, 30, 100), new IfcCartesianPoint(db, 100, 30, 100)),
            IfcFace.GenFace(db, new IfcCartesianPoint(db, 100, 30, 100), new IfcCartesianPoint(db, 900, 30, 100), new IfcCartesianPoint(db, 900, 40, 100), new IfcCartesianPoint(db, 100, 40, 100)),
            IfcFace.GenFace(db, new IfcCartesianPoint(db, 100, 40, 100), new IfcCartesianPoint(db, 900, 40, 100), new IfcCartesianPoint(db, 180, 40, 120), new IfcCartesianPoint(db, 120, 40, 120)),
            IfcFace.GenFace(db, new IfcCartesianPoint(db, 120, 40, 120), new IfcCartesianPoint(db, 880, 40, 120), new IfcCartesianPoint(db, 890, 70, 110), new IfcCartesianPoint(db, 110, 70, 110)),
            IfcFace.GenFace(db, new IfcCartesianPoint(db, 110, 70, 110), new IfcCartesianPoint(db, 890, 70, 110), new IfcCartesianPoint(db, 1000, 70, 0), new IfcCartesianPoint(db, 0, 70, 0)),
            IfcFace.GenFace(db, new IfcCartesianPoint(db, 0, 70, 0), new IfcCartesianPoint(db, 1000, 70, 0), new IfcCartesianPoint(db, 1000, 0, 0), new IfcCartesianPoint(db, 0, 0, 0)),
            IfcFace.GenFace(db, new IfcCartesianPoint(db, 1000, 0, 0), new IfcCartesianPoint(db, 1000, 0, 1000), new IfcCartesianPoint(db, 890, 0, 890), new IfcCartesianPoint(db, 890, 0, 110)),
            IfcFace.GenFace(db, new IfcCartesianPoint(db, 890, 0, 110), new IfcCartesianPoint(db, 890, 0, 890), new IfcCartesianPoint(db, 880, 30, 880), new IfcCartesianPoint(db, 880, 30, 120)),
            IfcFace.GenFace(db, new IfcCartesianPoint(db, 880, 30, 120), new IfcCartesianPoint(db, 880, 30, 880), new IfcCartesianPoint(db, 900, 30, 900), new IfcCartesianPoint(db, 900, 30, 100)),
            IfcFace.GenFace(db, new IfcCartesianPoint(db, 900, 30, 100), new IfcCartesianPoint(db, 900, 30, 900), new IfcCartesianPoint(db, 900, 40, 900), new IfcCartesianPoint(db, 900, 40, 100)),
            IfcFace.GenFace(db, new IfcCartesianPoint(db, 900, 40, 100), new IfcCartesianPoint(db, 900, 40, 900), new IfcCartesianPoint(db, 880, 40, 880), new IfcCartesianPoint(db, 880, 40, 120)),
            IfcFace.GenFace(db, new IfcCartesianPoint(db, 880, 40, 120), new IfcCartesianPoint(db, 880, 40, 880), new IfcCartesianPoint(db, 890, 70, 890), new IfcCartesianPoint(db, 890, 70, 110)),
            IfcFace.GenFace(db, new IfcCartesianPoint(db, 890, 70, 110), new IfcCartesianPoint(db, 890, 70, 890), new IfcCartesianPoint(db, 1000, 70, 1000), new IfcCartesianPoint(db, 1000, 70, 0)),
            IfcFace.GenFace(db, new IfcCartesianPoint(db, 1000, 70, 0), new IfcCartesianPoint(db, 1000, 70, 1000), new IfcCartesianPoint(db, 1000, 0, 1000), new IfcCartesianPoint(db, 1000, 0, 0)),
            IfcFace.GenFace(db, new IfcCartesianPoint(db, 1000, 0, 1000), new IfcCartesianPoint(db, 0, 0, 1000), new IfcCartesianPoint(db, 110, 0, 890), new IfcCartesianPoint(db, 890, 0, 890)),
            IfcFace.GenFace(db, new IfcCartesianPoint(db, 890, 0, 890), new IfcCartesianPoint(db, 110, 0, 890), new IfcCartesianPoint(db, 120, 30, 880), new IfcCartesianPoint(db, 880, 30, 880)),
            IfcFace.GenFace(db, new IfcCartesianPoint(db, 880, 30, 880), new IfcCartesianPoint(db, 120, 30, 880), new IfcCartesianPoint(db, 100, 30, 900), new IfcCartesianPoint(db, 900, 30, 900)),
            IfcFace.GenFace(db, new IfcCartesianPoint(db, 900, 30, 900), new IfcCartesianPoint(db, 100, 30, 900), new IfcCartesianPoint(db, 100, 40, 900), new IfcCartesianPoint(db, 900, 40, 900)),
            IfcFace.GenFace(db, new IfcCartesianPoint(db, 900, 40, 900), new IfcCartesianPoint(db, 100, 40, 900), new IfcCartesianPoint(db, 120, 40, 880), new IfcCartesianPoint(db, 880, 40, 880)),
            IfcFace.GenFace(db, new IfcCartesianPoint(db, 880, 40, 880), new IfcCartesianPoint(db, 120, 40, 880), new IfcCartesianPoint(db, 110, 70, 890), new IfcCartesianPoint(db, 890, 70, 890)),
            IfcFace.GenFace(db, new IfcCartesianPoint(db, 890, 70, 890), new IfcCartesianPoint(db, 110, 70, 890), new IfcCartesianPoint(db, 0, 70, 1000), new IfcCartesianPoint(db, 1000, 70, 1000)),
            IfcFace.GenFace(db, new IfcCartesianPoint(db, 1000, 70, 1000), new IfcCartesianPoint(db, 0, 70, 1000), new IfcCartesianPoint(db, 0, 0, 1000), new IfcCartesianPoint(db, 1000, 0, 1000)),
            IfcFace.GenFace(db, new IfcCartesianPoint(db, 0, 0, 1000), new IfcCartesianPoint(db, 0, 0, 0), new IfcCartesianPoint(db, 110, 0, 110), new IfcCartesianPoint(db, 110, 0, 890)),
            IfcFace.GenFace(db, new IfcCartesianPoint(db, 110, 0, 890), new IfcCartesianPoint(db, 110, 0, 110), new IfcCartesianPoint(db, 120, 30, 120), new IfcCartesianPoint(db, 120, 30, 880)),
            IfcFace.GenFace(db, new IfcCartesianPoint(db, 120, 30, 880), new IfcCartesianPoint(db, 120, 30, 120), new IfcCartesianPoint(db, 100, 30, 1000), new IfcCartesianPoint(db, 100, 30, 900)),
            IfcFace.GenFace(db, new IfcCartesianPoint(db, 100, 30, 900), new IfcCartesianPoint(db, 100, 30, 100), new IfcCartesianPoint(db, 100, 40, 100), new IfcCartesianPoint(db, 100, 40, 900)),
            IfcFace.GenFace(db, new IfcCartesianPoint(db, 100, 40, 900), new IfcCartesianPoint(db, 100, 40, 100), new IfcCartesianPoint(db, 120, 40, 120), new IfcCartesianPoint(db, 120, 40, 880)),
            IfcFace.GenFace(db, new IfcCartesianPoint(db, 120, 40, 880), new IfcCartesianPoint(db, 120, 40, 120), new IfcCartesianPoint(db, 110, 70, 110), new IfcCartesianPoint(db, 110, 70, 890)),
            IfcFace.GenFace(db, new IfcCartesianPoint(db, 110, 70, 890), new IfcCartesianPoint(db, 110, 70, 110), new IfcCartesianPoint(db, 0, 70, 0), new IfcCartesianPoint(db, 0, 70, 1000)),
            IfcFace.GenFace(db, new IfcCartesianPoint(db, 0, 70, 1000), new IfcCartesianPoint(db, 0, 70, 0), new IfcCartesianPoint(db, 0, 0, 0), new IfcCartesianPoint(db, 0, 0, 1000)),
            IfcFace.GenFace(db, new IfcCartesianPoint(db, 0, 0, 0), new IfcCartesianPoint(db, 0, 0, 0), new IfcCartesianPoint(db, 0, 0, 0), new IfcCartesianPoint(db, 0, 0, 0)),
            IfcFace.GenFace(db, new IfcCartesianPoint(db, 0, 0, 0), new IfcCartesianPoint(db, 0, 0, 0), new IfcCartesianPoint(db, 0, 0, 0), new IfcCartesianPoint(db, 0, 0, 0))
            };

            IfcConnectedFaceSet connectedFaceSet = new IfcConnectedFaceSet(faces);
            IfcFaceBasedSurfaceModel faceBasedSurfaceModel = new IfcFaceBasedSurfaceModel(new List<IfcConnectedFaceSet>() { connectedFaceSet });

            IfcRepresentationMap representationMap = new IfcRepresentationMap(faceBasedSurfaceModel);

            IfcCartesianTransformationOperator3DnonUniform trans = new IfcCartesianTransformationOperator3DnonUniform(db);
            trans.Scale = 2000 / 1000.0;
            trans.Scale2 = 100 / 70.0;
            trans.Scale3 = 1200 / 1000.0;

            IfcMappedItem mappedItem = new IfcMappedItem(representationMap, trans);
            IfcShapeRepresentation shapeRepresentation = new IfcShapeRepresentation(mappedItem);
            IfcProductRepresentation productRepresesentation = new IfcProductRepresentation(shapeRepresentation);

            // ==== Window ====
            IfcWindow window = new IfcWindow(building, openingElement, localPlacement, productRepresesentation);
            window.Name = "Window";
        }

        private static IfcOpeningElement genOpeningForWindow(IfcWallStandardCase wall)
        {
            DatabaseIfc db = wall.Database;

            // ==== local placement ====
            IfcDirection directionZ = new IfcDirection(db, 0, 0, 1);
            IfcDirection directionX = new IfcDirection(db, 1, 0, 0);
            IfcLocalPlacement localPlacement = new IfcLocalPlacement(wall.Placement, new IfcAxis2Placement3D(new IfcCartesianPoint(db, 1500, 0, 1200), directionZ, directionX));

            // ==== Body ====
            IfcCartesianPoint p1 = new IfcCartesianPoint(db, 0, 0, 0);
            IfcCartesianPoint p2 = new IfcCartesianPoint(db, 0, 100, 0);
            IfcCartesianPoint p3 = new IfcCartesianPoint(db, 2000, 100, 0);
            IfcCartesianPoint p4 = new IfcCartesianPoint(db, 2000, 0, 0);
            IfcCartesianPoint p5 = new IfcCartesianPoint(db, 0, 0, 0);

            IfcPolyline ifcPolylineBody = new IfcPolyline(new List<IfcCartesianPoint>() { p1, p2, p3, p4, p5 });

            IfcArbitraryClosedProfileDef arbitraryClosedProfileDef = new IfcArbitraryClosedProfileDef("", ifcPolylineBody);

            IfcExtrudedAreaSolid extrudedAreaSolid = new IfcExtrudedAreaSolid(arbitraryClosedProfileDef, new IfcAxis2Placement3D(new IfcCartesianPoint(db, 0, 0, 0)), 1200);

            IfcShapeRepresentation bodyShape = new IfcShapeRepresentation(extrudedAreaSolid);

            IfcProductRepresentation productRepresesentation = new IfcProductRepresentation(bodyShape);

            // ==== Opening ====
            IfcOpeningElement openingElement = new IfcOpeningElement(wall, localPlacement, productRepresesentation);
            openingElement.Name = "Opening";
            return openingElement;
        }
    }


    public enum DoorSwing
    {
        SingleSwingLeft,
        SingleSwingLeftOtherSide,
        SingleSwingRight,
        SingleSwingRightOtherSide,
        DoubleSwing,
        DoubleSwingOtherSide
    }

    public class Vector3D
    {
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

    public class Polygon
    {
        public Polygon(List<Vector3D> geometry)
        {
            this.Geometry = geometry;
        }
        public List<Vector3D> Geometry;
    }

    public class Door
    {
        public Door()
        {
            Width = 900;
            Height = 2000;
            Thickness = 100;

            Bottom = new Polygon(new List<Vector3D>() {
                new Vector3D(0,0,0), 
                new Vector3D(0, -Thickness, 0), 
                new Vector3D(Width, -Thickness, 0),
                new Vector3D(Width,0,0), 
                new Vector3D(0,0,0)});

            Dir = new Vector3D(1, 0, 0);

            DoorSwing = DoorSwing.SingleSwingLeft;
        }

        public double Width;
        public double Height;
        public double Thickness;
        public Polygon Bottom;
        public Vector3D Dir;

        public DoorSwing DoorSwing;
    }
}
