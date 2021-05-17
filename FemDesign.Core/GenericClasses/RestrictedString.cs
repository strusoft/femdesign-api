// https://strusoft.com/
using System.Collections.Generic;

namespace FemDesign
{
    /// <summary>
    /// Contains string restrictions used for property definitions.
    /// 
    /// Restrictions are defined by strusoft.xsd
    /// </summary>
    internal class RestrictedString
    {
        /// <summary>
        /// Create error message from items.
        /// </summary>
        internal static string RestrictionItemsMsg(List<string> items)
        {
            string rtn = ": ";
            for (int idx = 0; idx < items.Count; idx++)
            {
                if (idx == 0)
                {
                    // pass
                }
                else
                {
                    rtn += ", ";
                }
                rtn += items;
            }
            return rtn;
        }

        /// <summary>
        /// Check if val in items.
        /// </summary>
        internal static string RestrictionItems(string val, List<string> items)
        {
            if (items.Contains(val))
            {
                return val;
            }
            else
            {
                string msg = RestrictedString.RestrictionItemsMsg(items);
                throw new System.ArgumentException($"Value must be{msg}");
            }
        }

        /// <summary>
        /// Check if length of val is smaller than len.
        /// </summary>
        /// <param name="val"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        internal static string Length(string val, int len)
        {
            if (val.Length > len)
            {
                throw new System.ArgumentException($"Length of value {val.Length} must be equal to {len} or shorter.");
            }
            else
            {
                return val;
            }
        }

        /// <summary>
        /// bar_buckling_type
        /// </summary>
        internal static string BarBucklingType(string val)
        {
            List<string> items = new List<string>(){"flexural_weak", "flexural_stiff", "pressured_flange", "lateral_torsional", "pressured_flange"};
            return RestrictedString.RestrictionItems(val, items);
        }

        /// <summary>
        /// cmdUserModule
        /// </summary>
        internal static string CmdUserModule(string val)
        {
            List<string> items = new List<string>(){"RESMODE", "RCDESIGN", "STEELDESIGN", "TIMBERDESIGN"};
            return RestrictedString.RestrictionItems(val, items);
        }

        /// <summary>
        /// detach_type
        /// </summary>
        internal static string DetachType(string val)
        {
            List<string> items = new List<string>(){"", "x_tens", "x_comp", "y_tens", "y_comp", "z_tens", "z_comp"};
            return RestrictedString.RestrictionItems(val, items);
        }

        /// <summary>
        /// direction_type
        /// </summary>
        internal static string DirectionType(string val)
        {
            List<string> items = new List<string>(){"x", "y"};
            return RestrictedString.RestrictionItems(val, items);
        }

        /// <summary>
        /// edgetype
        /// </summary>
        internal static string EdgeType(string val)
        {
            List<string> items = new List<string>(){"line", "arc", "spline", "polyline", "circle"};
            return RestrictedString.RestrictionItems(val, items);
        }

        /// <summary>
        /// eurocodetype
        /// </summary>
        internal static string EurocodeType(string val)
        {
            List<string> items = new List<string>(){"common", "H", "RO", "DK", "S", "N", "FIN", "GB", "D", "PL", "TR", "EST", "LT", "n/a"};
            return RestrictedString.RestrictionItems(val, items);
        }

        /// <summary>
        /// force_load_type
        /// </summary>
        internal static string ForceLoadType(string val)
        {
            List<string> items = new List<string>(){"force", "moment"};
            return RestrictedString.RestrictionItems(val, items);
        }

        /// <summary>
        /// loadcasetype_type
        /// </summary>
        internal static string LoadCaseType(string val)
        {
            List<string> items = new List<string>(){"static", "dead_load", "shrinkage", "seis_max", "seis_sxp", "seis_sxm", "seis_syp", "seis_sym", "soil_dead_load", "prestressing", "fire", "deviation", "notional"};
            return RestrictedString.RestrictionItems(val, items);
        }

        /// <summary>
        /// loadcasedurationtype
        /// </summary>
        internal static string LoadCaseDurationType(string val)
        {
            List<string> items = new List<string>(){"permanent", "long-term", "medium-term", "short-term", "instantaneous"};
            return RestrictedString.RestrictionItems(val, items);
        }

        /// <summary>
        /// loadcombtype
        /// </summary>
        internal static string LoadCombType(string val)
        {
            List<string> items = new List<string>(){"ultimate_ordinary", "ultimate_accidental", "ultimate_seismic","serviceability_characteristic", "serviceability_quasi_permanent", "serviceability_frequent","serviceability"};
            return RestrictedString.RestrictionItems(val, items);
        }

        /// <summary>
        /// load_dir_type
        /// </summary>
        internal static string LoadDirType(string val)
        {
            List<string> items = new List<string>(){"constant", "changing"};
            return RestrictedString.RestrictionItems(val, items);
        }

        /// <summary>
        /// Set LoadDirType to string from bool
        /// </summary>
        internal static string LoadDirTypeFromBool(bool val)
        {
            if (val)
            {
                return RestrictedString.LoadDirType("constant");
            }
            else
            {
                return RestrictedString.LoadDirType("changing");
            }
        }

        /// <summary>
        /// Get LoadDirType as bool from string
        /// </summary>
        internal static bool LoadDirTypeToBool(string val)
        {
            if (val == "constant")
            {
                return true;
            }
            else if (val == "changing")
            {
                return false;
            }
            else
            {
                throw new System.ArgumentException($"Unallowed value, {val}, can't GetLoadDirType as bool.");
            }
        }

        /// <summary>
        /// paneltype
        /// </summary>
        internal static string PanelType(string val)
        {
            List<string> items = new List<string>(){"concrete", "timber"};
            return RestrictedString.RestrictionItems(val, items);
        }

        /// <summary>
        /// sf_rc_face
        /// </summary>
        internal static string SfRcFace(string val)
        {
            List<string> items = new List<string>(){"top", "bottom"};
            return RestrictedString.RestrictionItems(val, items);
        }

        /// <summary>
        /// slabtype
        /// </summary>
        internal static string SlabType(string val)
        {
            List<string> items = new List<string>(){"plate", "wall"};
            return RestrictedString.RestrictionItems(val, items);
        }

        /// <summary>
        /// standardtype
        /// </summary>
        internal static string StandardType(string val)
        {
            List<string> items = new List<string>(){"EC", "general"};
            return RestrictedString.RestrictionItems(val, items);
        }

        /// <summary>
        /// steelmadetype 
        /// </summary>
        internal static string SteelMadeType(string val)
        {
            List<string> items = new List<string>(){"rolled", "cold_worked", "welded"};
            return RestrictedString.RestrictionItems(val, items);
        }

        /// <summary>
        /// ver_align 
        /// </summary>
        internal static string VerticalAlign(string val)
        {
            List<string> items = new List<string>(){"top", "center", "bottom"};
            return RestrictedString.RestrictionItems(val, items);
        }

        /// <summary>
        /// wire_profile_type
        /// </summary>
        internal static string WireProfileType(string val)
        {
            List<string> items = new List<string>(){"smooth", "ribbed"};
            return RestrictedString.RestrictionItems(val, items);
        }

    }
}