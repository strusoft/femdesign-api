using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using GH_IO.Serialization;
using GH = Grasshopper;
using System.Drawing;
using Grasshopper.GUI.Canvas;
using System.Reflection;
using FGH = FemDesign.Grasshopper;

namespace FemDesign.Info
{
    public static class Info
    {
        public static string GetCurrentFemDesignApiVersion()
        {
            IEnumerable<AssemblyName> assembly = Assembly.GetExecutingAssembly().GetReferencedAssemblies().Where(x => x.Name.Contains("FemDesign.Core"));
            string assemblyVersion = assembly.First().Version?.ToString();
            var ver = Version.Parse(assemblyVersion);
            return $"{ver.Major}.{ver.Minor}.{ver.Build}";
        }
    }

    public class InfoComponent : GH_Component
    {
        public InfoComponent() : base("Info", "Info", "Information about FEM Design API", FGH.CategoryName.Name(), FGH.SubCategoryName.CatLast())
        {

        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {

        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {

        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var current = Info.GetCurrentFemDesignApiVersion();

            if (current != VersionWhenFirstCreated)
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, $"The version of FEM-Design API installed ({current}) is different than the version when this script was created ({VersionWhenFirstCreated}). Most things are expected to work anyways. If you update the components marked with the \"OLD\"-tag it is recommended that you also create a new \"Info\"-component to indicate that this script has been updated to the current version ({current}).");
        }
        public override void CreateAttributes()
        {
            m_attributes = new InfoAttributes(this);
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("2fba9422-b154-4e5b-ac13-80d6f7839f5f"); }
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.Fd_TabIcon_24_24;
            }
        }

        public string VersionWhenFirstCreated = null;
        public override void AddedToDocument(GH_Document document)
        {
            if (VersionWhenFirstCreated is null)
                VersionWhenFirstCreated = Info.GetCurrentFemDesignApiVersion();
        }

        public override bool Write(GH_IWriter writer)
        {
            // Save the version when this component was created
            writer.SetString("versionWhenCreated", VersionWhenFirstCreated);
            return base.Write(writer);
        }

        public override bool Read(GH_IReader reader)
        {
            // Read the version when this component was created
            try
            {
                VersionWhenFirstCreated = reader.GetString("versionWhenCreated");
            }
            catch (NullReferenceException) { } // In case the info component was created before the VersionWhenFirstCreated was implemented.
            return base.Read(reader);
        }
    }

    public class InfoAttributes : GH_Attributes<InfoComponent>
    {
        RectangleF link1 = new RectangleF();
        RectangleF link2 = new RectangleF();
        RectangleF link3 = new RectangleF();
        public InfoAttributes(InfoComponent owner) : base(owner) { }


        protected override void Layout()
        {
            // Compute the width of the NickName of the owner (plus some extra padding), 
            // then make sure we have at least 80 pixels.
            //int width = GH_FontServer.StringWidth(Owner.NickName, GH_FontServer.Standard);
            int width = 220; //Math.Max(width + 10, 80);

            // The height of our object is always 60 pixels
            int height = 200;

            // Assign the width and height to the Bounds property.
            // Also, make sure the Bounds are anchored to the Pivot
            Bounds = new RectangleF(Pivot, new SizeF(width, height));
        }

        protected override void Render(GH_Canvas canvas, Graphics graphics, GH_CanvasChannel channel)
        {

            // Render the parameter capsule and any additional text on top of it.
            if (channel == GH_CanvasChannel.Objects)
            {
                // Define the default palette.
                GH_Palette palette = GH_Palette.White;

                // Adjust palette based on the Owner's worst case messaging level.
                switch (Owner.RuntimeMessageLevel)
                {
                    case GH_RuntimeMessageLevel.Warning:
                        palette = GH_Palette.Warning;
                        break;

                    case GH_RuntimeMessageLevel.Error:
                        palette = GH_Palette.Error;
                        break;
                }

                // Create a new Capsule without text or icon.
                GH_Capsule capsule = GH_Capsule.CreateCapsule(Bounds, palette);

                // Render the capsule using the current Selection, Locked and Hidden states.
                // Integer parameters are always hidden since they cannot be drawn in the viewport.
                capsule.Render(graphics, Selected, Owner.Locked, true);

                // Always dispose of a GH_Capsule when you're done with it.
                capsule.Dispose();
                capsule = null;

                // Now it's time to draw the text on top of the capsule.
                // First we'll draw the Owner NickName using a standard font and a black brush.
                // We'll also align the NickName in the center of the Bounds.
                StringFormat format = new StringFormat();
                format.Alignment = StringAlignment.Center;
                format.LineAlignment = StringAlignment.Center;
                format.Trimming = StringTrimming.EllipsisCharacter;

                // Our entire capsule is 60 pixels high, and we'll draw 
                // three lines of text, each 20 pixels high.
                RectangleF textRectangle = Bounds;
                textRectangle.Height = 20;

                textRectangle.Y += 5;
                // Draw the NickName in a Standard Grasshopper font.
                graphics.DrawString("About FemDesign API", GH_FontServer.Large, Brushes.Black, textRectangle, format);


                // Now we need to draw the median and mean information.
                // Adjust the formatting and the layout rectangle.
                format.Alignment = StringAlignment.Near;
                textRectangle.Inflate(-5, 0);

                //API version number
                string currentVersion = Info.GetCurrentFemDesignApiVersion();
                string creationVersion = this.Owner.VersionWhenFirstCreated;

                Pen pen = new Pen(Brushes.Black, Convert.ToSingle(0.5));
                PointF pt1 = new PointF(textRectangle.X, textRectangle.Y + 20);
                PointF pt2 = new PointF(textRectangle.X + textRectangle.Width, textRectangle.Y + 20);
                graphics.DrawLine(pen, pt1, pt2);

                textRectangle.Y += 25;
                graphics.DrawString($"Current version: {currentVersion}", GH_FontServer.StandardItalic, Brushes.Black, textRectangle, format);

                textRectangle.Y += 20;
                graphics.DrawString($"Created with: {creationVersion}", GH_FontServer.StandardItalic, Brushes.Black, textRectangle, format);

                textRectangle.Y += 20;
                graphics.DrawString(String.Format("Useful links:"), GH_FontServer.StandardItalic, Brushes.Black, textRectangle, format);

                textRectangle.Y += 15;
                link1 = textRectangle;
                Font linkFont = new Font(GH_FontServer.StandardItalic, FontStyle.Underline);
                graphics.DrawString(String.Format("https://strusoft.freshdesk.com/", 5), linkFont, Brushes.Blue, textRectangle, format);

                textRectangle.Y += 15;
                link2 = textRectangle;
                graphics.DrawString(String.Format("https://wiki.fem-design.strusoft.com/"), linkFont, Brushes.Blue, textRectangle, format);

                textRectangle.Y += 15;
                link3 = textRectangle;
                graphics.DrawString(String.Format("https://github.com/strusoft/femdesign-api"), linkFont, Brushes.Blue, textRectangle, format);


                textRectangle.Y += 25;
                textRectangle.Height = Convert.ToSingle(textRectangle.Width * 0.227);
                Image image = FemDesign.Properties.Resources.fdlogo;

                graphics.DrawImage(image, textRectangle);

                // Always dispose of any GDI+ object that implement IDisposable.
                format.Dispose();
            }
        }

        public override GH.GUI.Canvas.GH_ObjectResponse RespondToMouseUp(GH.GUI.Canvas.GH_Canvas sender, GH.GUI.GH_CanvasMouseEvent e)
        {
            if (e.Button != System.Windows.Forms.MouseButtons.Left) 
                return base.RespondToMouseUp(sender, e);

            // Left mouse button up
            if (link1.Contains(e.CanvasLocation))
            {
                System.Diagnostics.Process.Start("https://strusoft.freshdesk.com/");
                return GH.GUI.Canvas.GH_ObjectResponse.Handled;
            }
            else if (link2.Contains(e.CanvasLocation))
            {
                System.Diagnostics.Process.Start("https://wiki.fem-design.strusoft.com/");
                return GH.GUI.Canvas.GH_ObjectResponse.Handled;
            }
            else if (link3.Contains(e.CanvasLocation))
            {
                System.Diagnostics.Process.Start("https://github.com/strusoft/femdesign-api");
                return GH.GUI.Canvas.GH_ObjectResponse.Handled;
            }
            return GH.GUI.Canvas.GH_ObjectResponse.Ignore;
        }
    }

}




