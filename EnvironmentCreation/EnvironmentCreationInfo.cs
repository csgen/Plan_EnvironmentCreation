using Grasshopper;
using Grasshopper.Kernel;
using System;
using System.Drawing;

namespace EnvironmentCreation
{
    public class EnvironmentCreationInfo : GH_AssemblyInfo
    {
        public override string Name => "PathCreation";

        //Return a 24x24 pixel bitmap to represent this GHA library.
        public override Bitmap Icon => null;

        //Return a short string describing the purpose of this GHA library.
        public override string Description => "";

        public override Guid Id => new Guid("d89d35df-b6a3-4254-92d9-0ae4ba8034b3");

        //Return a string identifying you or your company.
        public override string AuthorName => "";

        //Return a string representing your preferred contact details.
        public override string AuthorContact => "";
    }
}