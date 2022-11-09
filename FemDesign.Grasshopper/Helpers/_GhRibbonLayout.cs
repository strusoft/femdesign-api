namespace FemDesign.Grasshopper
{
    /// <summary>
    /// Class containing the ribbon tab display name
    /// 
    /// Call this class from all components in plugin for naming consistency
    /// </summary>
    public class CategoryName
    {
        public static string Name()
        {
            return "FEM-Design";
        }
    }

    /// <summary>
    /// Class containing ribbon category names
    /// 
    /// Call this class from all components to pick which category component should sit in
    /// 
    /// Sorting of categories in the ribbon is controlled with a number of spaces in front of the name
    /// to avoid naming each category with a number in front. Spaces will automatically be removed when displayed
    /// </summary>
    internal class SubCategoryName
    {
        public static string Cat0()
        {
            return new string(' ', 7) + "Foundations";
        }
        public static string Cat1()
        {
            return new string(' ', 7) + "Supports";
        }

        public static string Cat2a()
        {
            return new string(' ', 6) + "Bars";
        }

        public static string Cat2b()
        {
            return new string(' ', 6) + "Shells";
        }

        public static string Cat3()
        {
            return new string(' ', 5) + "Loads";
        }

        public static string Cat4b()
        {
            return new string(' ', 4) + "Sections";
        }

        public static string Cat4a()
        {
            return new string(' ', 4) + "Materials";
        }

        public static string Cat5()
        {
            return new string(' ', 3) + "Release";
        }

        public static string Cat6()
        {
            return new string(' ', 2) + "Model";
        }

        public static string Cat7a()
        {
            return new string(' ', 1) + "Calculate";
        }

        public static string Cat7b()
        {
            return new string(' ', 1) + "Results";
        }
        public static string CatLast()
        {
            return "StruSoft";
        }

    }
}