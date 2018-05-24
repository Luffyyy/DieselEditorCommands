using DieselEngineFormats;
using DieselEngineFormats.Bundle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DieselEditorCommands
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                string method = args[0];
                switch (method)
                {
                    case "WriteMassUnit":
                        WriteMassUnit(args);
                        break;
                    case "RewriteMassUnit":
                        RewriteMassUnit(args);
                        break;
                    default:
                        Console.WriteLine("No method with the name " + method + " was found..");
                        break;
                }        
            }
            else
                Console.WriteLine("No arguments given.");
            
            Environment.Exit(0);
        }

        public static void RewriteMassUnit(string[] args)
        {
            Console.WriteLine("Reading massunit..");

            MassUnit massUnit = new MassUnit(args[1]);

            Console.WriteLine("Writing massunit..");

            massUnit.WriteFile(args[2]);

            Console.WriteLine("Successfully wrote the massunit!");

            Environment.Exit(1);
        }

        public static void WriteMassUnit(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Usage(qx: quaternion x): WriteMassUnit \"path/to/massunit.massunit\" \"path/to/brush/unit|x,y,z,qx,qy,qz,qw\"");
                return;
            }


            List<MassUnitHeader> Headers = new List<MassUnitHeader>();

            for (int i = 2; i < args.Length; i++)
            {
                string[] unitDef = args[i].Split('|');
                if (unitDef.Length < 2) {
                    Console.WriteLine("ERROR! Invalid arguments given {0}", args[i]);
                    return;
                }

                List<Vector3> Positions = new List<Vector3>();
                List<Quaternion> Rotations = new List<Quaternion>();
                for (int x = 1; x < unitDef.Length; x++)
                {
                    string[] posRot = unitDef[1].Split(',');
                    if(posRot.Length < 7)
                    {
                        Console.WriteLine("ERROR! Invalid position and rotation given %s", unitDef[1]);
                        return;
                    }

                    Positions.Add(new Vector3(float.Parse(posRot[0]), float.Parse(posRot[1]), float.Parse(posRot[2])));
                    Rotations.Add(new Quaternion(float.Parse(posRot[3]), float.Parse(posRot[4]), float.Parse(posRot[5]), float.Parse(posRot[6])));
                }
                Console.WriteLine("Unit {0} count {1}", unitDef[0], Positions.Count);
                Headers.Add(new MassUnitHeader(new Idstring(unitDef[0]), Positions, Rotations));
            }
            MassUnit massUnit = new MassUnit(Headers);
            massUnit.WriteFile(args[1]);

            Console.WriteLine("Successfully wrote the massunit!");

            Environment.Exit(1);
        }
    }
}
