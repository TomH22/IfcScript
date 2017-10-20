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
    /// - 1 door with mapped representation
    /// - 1 window with mapped representation
    /// </summary>
    class Walls2 : IFCExampleInstance
    {
        protected override void GenerateInstance(IfcBuilding building)
        {
            DatabaseIfc db = building.Database;

            // ==== Material ====
            IfcMaterial material = new IfcMaterial(db, "Material");
            IfcMaterialLayer materialLayer = new IfcMaterialLayer(material, 100, "");
            materialLayer.IsVentilated = IfcLogicalEnum.UNKNOWN;
            IfcMaterialLayerSet materialLayerSet = new IfcMaterialLayerSet(new List<IfcMaterialLayer>() { materialLayer }, "");
            IfcMaterialLayerSetUsage layerSetUsage = new IfcMaterialLayerSetUsage(materialLayerSet,
                IfcLayerSetDirectionEnum.AXIS2, IfcDirectionSenseEnum.NEGATIVE, 0);

            genWallWithDoor(building, layerSetUsage);
            genWallWithWindow(building, layerSetUsage);
        }

        private void genWallWithDoor(IfcBuilding building, IfcMaterialLayerSetUsage layerSetUsage)
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

            genOpeningWithDoor(building, wallStandardCase);
            db.NextObjectRecord = 100;
        }

        private void genWallWithWindow(IfcBuilding building, IfcMaterialLayerSetUsage layerSetUsage)
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

            genOpeningWithWindow(building, wallStandardCase);
            db.NextObjectRecord = 100;
        }

        private void genOpeningWithDoor(IfcBuilding building, IfcWallStandardCase wall)
        {
            DatabaseIfc db = wall.Database;

            // ==== local placement ====
            IfcLocalPlacement localPlacement = new IfcLocalPlacement(wall.Placement, new IfcAxis2Placement3D(new IfcCartesianPoint(db, 1500, 0, 0)));

            // ==== Body ====
            IfcCartesianPoint p1 = new IfcCartesianPoint(db, 0, 0, 0);
            IfcCartesianPoint p2 = new IfcCartesianPoint(db, 0, -100, 0);
            IfcCartesianPoint p3 = new IfcCartesianPoint(db, 1300, -100, 0);
            IfcCartesianPoint p4 = new IfcCartesianPoint(db, 1300, 0, 0);
            IfcCartesianPoint p5 = new IfcCartesianPoint(db, 0, 0, 0);

            IfcPolyline ifcPolylineBody = new IfcPolyline(new List<IfcCartesianPoint>() { p1, p2, p3, p4, p5 });

            IfcArbitraryClosedProfileDef arbitraryClosedProfileDef = new IfcArbitraryClosedProfileDef("", ifcPolylineBody);

            IfcExtrudedAreaSolid extrudedAreaSolid = new IfcExtrudedAreaSolid(arbitraryClosedProfileDef, new IfcAxis2Placement3D(new IfcCartesianPoint(db, 0, 0, 0)), 2100);

            IfcShapeRepresentation bodyShape = new IfcShapeRepresentation(extrudedAreaSolid);

            IfcProductRepresentation productRepresesentation = new IfcProductRepresentation(bodyShape);

            // ==== Opening ====
            IfcOpeningElement openingElement = new IfcOpeningElement(wall, localPlacement, productRepresesentation);
            genDoor(building, openingElement);
        }

        private void genOpeningWithWindow(IfcBuilding building, IfcWallStandardCase wall)
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
            genWindow(building, openingElement);
        }

        private void genDoor(IfcBuilding building, IfcOpeningElement openingElement)
        {
            DatabaseIfc db = openingElement.Database;

            // ==== local placement ====
            IfcLocalPlacement localPlacement = new IfcLocalPlacement(openingElement.Placement, new IfcAxis2Placement3D(new IfcCartesianPoint(db, 0, 0, 0)));

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
            trans.Scale = 1300 / 860.0;
            trans.Scale2 = 100 / 100.0;
            trans.Scale3 = 2100 / 2110.0;

            IfcMappedItem mappedItem = new IfcMappedItem(representationMap, trans);
            IfcShapeRepresentation shapeRepresentation = new IfcShapeRepresentation(mappedItem);
            IfcProductRepresentation productRepresesentation = new IfcProductRepresentation(shapeRepresentation);

            // ==== Door ====
            IfcDoor door = new IfcDoor(building, openingElement, localPlacement, productRepresesentation);
        }

        private void genWindow(IfcBuilding building, IfcOpeningElement openingElement)
        {
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
        }
    }
}
