
#region dynamo
using Autodesk.DesignScript.Runtime;
using Autodesk.Revit.DB;
using DocumentManager = RevitServices.Persistence.DocumentManager;
using TransactionManager = RevitServices.Transactions.TransactionManager;

namespace FemDesign.RevitTools
{
    /// <summary>
    /// Class to contain methods to change Revit project settings.
    /// </summary>
    [IsVisibleInDynamoLibrary(false)]
    public partial class ProjectSettings
    {
        /// <summary>
        /// Set project length unit to [m].
        /// </summary>
        /// <remarks>Action</remarks>
        /// <param name="runIt">Bool. Set to true to run.</param>
        [IsVisibleInDynamoLibrary(true)]
        public static void SetRevitUnitsToMeters(bool runIt)
        {
            if (runIt)
            {
                Document document = DocumentManager.Instance.CurrentDBDocument;
                TransactionManager.Instance.EnsureInTransaction(document);
                Units units = document.GetUnits();
                FormatOptions format = new FormatOptions(DisplayUnitType.DUT_METERS);
                units.SetFormatOptions(UnitType.UT_Length, format);
                document.SetUnits(units);
                TransactionManager.Instance.TransactionTaskDone();
            }    
        }
    }
}
#endregion