using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using IFC.Examples;
using GeometryGym.Ifc;

namespace IFCExamples
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
            DirectoryInfo di = Directory.GetParent(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location));
            di = Directory.GetParent(di.FullName);

            string path = Path.Combine(di.FullName, "examples");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            //new BeamExtruded().GenerateExample(path, ModelView.Ifc2x3Coordination);
            //new BeamTessellated().GenerateExample(path, ModelView.Ifc2x3Coordination);

            //new IndexedColourMap().GenerateExample(path, ModelView.Ifc2x3Coordination);
            //new BeamUnitTestsVaryingProfile().GenerateExample(path, ModelView.Ifc2x3Coordination);
            //new BeamUnitTestsVaryingPath().GenerateExample(path, ModelView.Ifc2x3Coordination);
            //new BeamUnitTestsVaryingCardinal().GenerateExample(path, ModelView.Ifc2x3Coordination);
            ////todo tapered
            //new Slab().GenerateExample(path, ModelView.Ifc2x3Coordination);
            //new SlabOpenings().GenerateExample(path, ModelView.Ifc2x3Coordination);
            new Walls2().GenerateExample(path, ModelView.If2x3NotAssigned);
            //new Wall().GenerateExample(path, ModelView.Ifc2x3Coordination);
            //todo wall with Openings
            new Bath().GenerateExample(path, ModelView.If2x3NotAssigned);
            //new BasinAdvancedBrep().GenerateExample(path, ModelView.Ifc2x3Coordination);
            //new BasinBrep().GenerateExample(path, ModelView.Ifc2x3Coordination);
            //new BasinTessellation().GenerateExample(path, ModelView.Ifc2x3Coordination);
            //new ReinforcingBar().GenerateExample(path, ModelView.Ifc2x3Coordination);
            //new ReinforcingAssembly().GenerateExample(path, ModelView.Ifc2x3Coordination);
            //new Column().GenerateExample(path, ModelView.Ifc2x3Coordination);
            //new CurveParametersRadians().GenerateExample(path, ModelView.Ifc2x3Coordination, true);
            //new CurveParametersDegrees().GenerateExample(path, ModelView.Ifc2x3Coordination, false);
            //new CurveParametersRadians().GenerateExample(path, ModelView.Ifc2x3Coordination,true);
            //new CurveParametersDegrees().GenerateExample(path, ModelView.Ifc2x3Coordination,false);
            //Possible Examples to add
            // IfcBuildingStorey with datums and local placements relative to building
            // IfcProjectLibrary
            // GeoSpatial SetOut of Building


            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
		}
	}
}
