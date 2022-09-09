﻿namespace FemDesign.Grasshopper
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
            return "!Help";
        }
        public static string Cat1()
        {
            return new string(' ', 6) + "Supports";
        }

        public static string Cat2()
        {
            return new string(' ', 5) + "Bars";
        }

        public static string Cat3()
        {
            return new string(' ', 5) + "Shells";
        }

        public static string Cat4()
        {
            return new string(' ', 4) + "Sections";
        }

        public static string Cat5()
        {
            return new string(' ', 4) + "Materials";
        }

        public static string Cat6()
        {
            return new string(' ', 3) + "Loads";
        }
        public static string Cat6a()
        {
            return new string(' ', 3) + "Release";
        }

        public static string Cat7()
        {
            return new string(' ', 2) + "Model";
        }

        public static string CatCalculate()
        {
            return new string(' ', 1) + "Calculate";
        }

        public static string Cat9()
        {
            return new string(' ', 1) + "Results";
        }
    }
}