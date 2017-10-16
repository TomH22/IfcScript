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
    /// </summary>
    class Walls2 : IFCExampleInstance
    {
        protected override void GenerateInstance(IfcBuilding building)
        {
            genWall1(building);
            genWall2(building);
        }

        private void genWall1(IfcBuilding building)
        {
            DatabaseIfc db = building.Database;

            // ==== Material ====
            IfcMaterial material = new IfcMaterial(db, "Material");
            IfcMaterialLayer materialLayer = new IfcMaterialLayer(material, 100, "");
            materialLayer.IsVentilated = IfcLogicalEnum.UNKNOWN;
            IfcMaterialLayerSet materialLayerSet = new IfcMaterialLayerSet(new List<IfcMaterialLayer>() { materialLayer }, "");
            IfcMaterialLayerSetUsage layerSetUsage = new IfcMaterialLayerSetUsage(materialLayerSet,
                IfcLayerSetDirectionEnum.AXIS2, IfcDirectionSenseEnum.NEGATIVE, 0);

            // ==== local placement ====
            IfcCartesianPoint cartesianPoint = new IfcCartesianPoint(db, 0, 1000, 0);
            IfcDirection directionZ = new IfcDirection(db, 0, 0, 1);
            IfcDirection directionX = new IfcDirection(db, 1, 0, 0);

            IfcAxis2Placement3D axis2Placement3D = new IfcAxis2Placement3D(cartesianPoint);

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
            IfcWallStandardCase wallStandardCase = new IfcWallStandardCase(building, layerSetUsage, axis2Placement3D, productDefinitionShape);

            genOpening(building, wallStandardCase);
            db.NextObjectRecord = 100;
        }

        private void genWall2(IfcBuilding building)
        {
            DatabaseIfc db = building.Database;

            // ==== Material ====
            IfcMaterial material = new IfcMaterial(db, "Material");
            IfcMaterialLayer materialLayer = new IfcMaterialLayer(material, 100, "");
            materialLayer.IsVentilated = IfcLogicalEnum.UNKNOWN;
            IfcMaterialLayerSet materialLayerSet = new IfcMaterialLayerSet(new List<IfcMaterialLayer>() { materialLayer }, "");
            IfcMaterialLayerSetUsage layerSetUsage = new IfcMaterialLayerSetUsage(materialLayerSet,
                IfcLayerSetDirectionEnum.AXIS2, IfcDirectionSenseEnum.NEGATIVE, 0);

            // ==== local placement ====
            IfcCartesianPoint cartesianPoint = new IfcCartesianPoint(db, 0, 1000, 0);
            IfcDirection directionZ = new IfcDirection(db, 0, 0, 1);
            IfcDirection directionX = new IfcDirection(db, 1, 0, 0);

            IfcAxis2Placement3D axis2Placement3D = new IfcAxis2Placement3D(cartesianPoint);

            // ==================
            // ==== Geometry ====
            // ==================
            List<IfcShapeModel> shapeModelList = new List<IfcShapeModel>();

            // ==== Axis ====
            IfcCartesianPoint point1 = new IfcCartesianPoint(db, 0, 0, 0);
            IfcCartesianPoint point2 = new IfcCartesianPoint(db, 0, 8000, 0);

            IfcPolyline polyline = new IfcPolyline(new List<IfcCartesianPoint>() { point1, point2 });
            IfcShapeRepresentation axisShape = IfcShapeRepresentation.GetAxisRep(polyline);
            shapeModelList.Add(axisShape);

            // ==== Body ====
            IfcCartesianPoint p1 = new IfcCartesianPoint(db, 0, 0, 0);
            IfcCartesianPoint p2 = new IfcCartesianPoint(db, -100, -100, 0);
            IfcCartesianPoint p3 = new IfcCartesianPoint(db, -100, 8100, 0);
            IfcCartesianPoint p4 = new IfcCartesianPoint(db, 0, 8000, 0);
            IfcCartesianPoint p5 = new IfcCartesianPoint(db, 0, 0, 0);


            IfcPolyline ifcPolylineBody = new IfcPolyline(new List<IfcCartesianPoint>() { p1, p2, p3, p4, p5 });

            IfcArbitraryClosedProfileDef arbitraryClosedProfileDef = new IfcArbitraryClosedProfileDef("", ifcPolylineBody);

            IfcExtrudedAreaSolid extrudedAreaSolid = new IfcExtrudedAreaSolid(arbitraryClosedProfileDef, new IfcAxis2Placement3D(new IfcCartesianPoint(db, 0, 0, 0)), 2800);

            IfcShapeRepresentation bodyShape = new IfcShapeRepresentation(extrudedAreaSolid);

            shapeModelList.Add(bodyShape);

            // ==== Wall ====
            IfcProductDefinitionShape productDefinitionShape = new IfcProductDefinitionShape(shapeModelList);
            IfcWallStandardCase wallStandardCase = new IfcWallStandardCase(building, layerSetUsage, axis2Placement3D, productDefinitionShape);

            genOpening(building, wallStandardCase);
            db.NextObjectRecord = 100;
        }

        private void genOpening(IfcBuilding building, IfcWallStandardCase wall)
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

        private void genDoor(IfcBuilding building, IfcOpeningElement openingElement)
        {
            DatabaseIfc db = openingElement.Database;

            // ==== local placement ====
            IfcLocalPlacement localPlacement = new IfcLocalPlacement(openingElement.Placement, new IfcAxis2Placement3D(new IfcCartesianPoint(db, 0, 0, 0)));

            // ==== MappedRepresentation ====

            // Body
            IfcFace f1 = genFace(db, new IfcCartesianPoint(db, 0, 0, 0), new IfcCartesianPoint(db, 1300, 0, 0), new IfcCartesianPoint(db, 1300, 0, 2100), new IfcCartesianPoint(db, 0, 0, 2100));
            IfcFace f2 = genFace(db, new IfcCartesianPoint(db, 0, 0, 0), new IfcCartesianPoint(db, 0, -100, 0), new IfcCartesianPoint(db, 0, -100, 2100), new IfcCartesianPoint(db, 0, 0, 2100));
            IfcFace f3 = genFace(db, new IfcCartesianPoint(db, 1300, 0, 0), new IfcCartesianPoint(db, 1300, -100, 0), new IfcCartesianPoint(db, 1300, -100, 2100), new IfcCartesianPoint(db, 1300, 0, 2100));
            IfcFace f4 = genFace(db, new IfcCartesianPoint(db, 0, -100, 0), new IfcCartesianPoint(db, 1300, -100, 0), new IfcCartesianPoint(db, 1300, -100, 2100), new IfcCartesianPoint(db, 0, -100, 2100));

            // doorway
            //double length = 1300;
            double lX = 1300;
            double lY = 0;
            double lXRotation = -0.64278761 * lY + 0.76604444 * lX;
            double lYRotation = 0.76604444 * lY + 0.64278761 * lX;

            IfcFace f5 = genFace(db, new IfcCartesianPoint(db, 0, 0, 0),
                new IfcCartesianPoint(db, lXRotation, lYRotation, 0), new IfcCartesianPoint(db, lXRotation, lYRotation, 2100), new IfcCartesianPoint(db, 0, 0, 2100));

            IfcConnectedFaceSet connectedFaceSet = new IfcConnectedFaceSet(new List<IfcFace>() { f1, f2, f3, f4, f5 });

            IfcFaceBasedSurfaceModel faceBasedSurfaceModel = new IfcFaceBasedSurfaceModel(new List<IfcConnectedFaceSet>() { connectedFaceSet });


            IfcShapeRepresentation shapeRepresentation = new IfcShapeRepresentation(faceBasedSurfaceModel);
            IfcProductRepresentation productRepresesentation = new IfcProductRepresentation(shapeRepresentation);

            // ==== Door ====
            IfcDoor door = new IfcDoor(building, openingElement, localPlacement, productRepresesentation);
        }

        private static IfcFace genFace(DatabaseIfc db, IfcCartesianPoint v1, IfcCartesianPoint v2, IfcCartesianPoint v3, IfcCartesianPoint v4)
        {
            IfcPolyloop polyloop1 = new IfcPolyloop(new List<IfcCartesianPoint> { v1, v2, v3, v4 });
            return new IfcFace(new IfcFaceOuterBound(polyloop1, true));
        }
    }
}
