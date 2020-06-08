// https://strusoft.com/
using System.Collections.Generic;
using System.Xml.Serialization;

namespace FemDesign.Bars.Buckling
{
    [System.Serializable]
    public class BucklingData
    {
        [XmlElement("buckling_length")]
        public List<BucklingLength> bucklingLength = new List<BucklingLength>();
        private BucklingData()
        {

        }
        /// <summary>
        /// Set BucklingData on a Concrete bar-element.
        /// </summary>
        /// <remarks>Action</remarks>
        /// <param name="bar">Bar. Concrete bar-element.</param>
        /// <param name="flexuralStiff">BucklingLength definition in Flexural Stiff direction.</param>
        /// <param name="flexuralWeak">BucklingLength definition in Flexural Weak direction.</param>
        /// <returns></returns>
        public static Bar SetOnConcreteBar(Bar bar, BucklingLength flexuralStiff, BucklingLength flexuralWeak)
        {
            // assert input
            if (bar.material.concrete == null)
            {
                throw new System.ArgumentException("Material of bar element must be concrete!");
            }
            if (flexuralStiff.type != "flexural_stiff")
            {
                throw new System.ArgumentException("flexuralStiff is not of type FlexuralStiff!");
            }
            if (flexuralWeak.type != "flexural_weak")
            {
                throw new System.ArgumentException("flexuralWeak is not of type FlexuralWeak!");
            }
            
            // add input
            BucklingData bucklingData = new BucklingData();
            bucklingData.bucklingLength.Add(flexuralStiff);
            bucklingData.bucklingLength.Add(flexuralWeak);
            bar.barPart.bucklingData = bucklingData;

            return bar;
        }
        /// <summary>
        /// Set BucklingData on a Steel bar-element.
        /// </summary>
        /// <remarks>Action</remarks>
        /// <param name="bar">Bar. Steel bar-element.</param>
        /// <param name="flexuralStiff">BucklingLength definition in Flexural Stiff direction.</param>
        /// <param name="flexuralWeak">BucklingLength definition in Flexural Weak direction.</param>
        /// <param name="pressuredFlange">BucklingLength definition for Pressured Flange.</param>
        /// <param name="pressuredBottomFlange">BucklingLength definition for Pressured Bottom Flange.</param>
        /// <returns></returns>
        public static Bar SetOnSteelBar(Bar bar, BucklingLength flexuralStiff, BucklingLength flexuralWeak, BucklingLength pressuredFlange, BucklingLength pressuredBottomFlange)
        {
            // assert input
            if (bar.material.steel == null)
            {
                throw new System.ArgumentException("Material of bar element must be steel!");
            }
            if (flexuralStiff.type != "flexural_stiff")
            {
                throw new System.ArgumentException("flexuralStiff is not of type FlexuralStiff!");
            }
            if (flexuralWeak.type != "flexural_weak")
            {
                throw new System.ArgumentException("flexuralWeak is not of type FlexuralWeak!");
            }
            if (pressuredFlange.type != "pressured_flange")
            {
                throw new System.ArgumentException("pressuredFlange is not of type PressuredFlange!");
            }
            if (pressuredBottomFlange.type != "pressured_bottom_flange")
            {
                throw new System.ArgumentException("pressuredBottomFlange is not of type PressuredBottomFlange!");
            }

            // add input
            BucklingData bucklingData = new BucklingData();
            bucklingData.bucklingLength.Add(flexuralStiff);
            bucklingData.bucklingLength.Add(flexuralWeak);
            bucklingData.bucklingLength.Add(pressuredFlange);
            bucklingData.bucklingLength.Add(pressuredBottomFlange);
            bar.barPart.bucklingData = bucklingData;

            return bar;
        }
        /// <summary>
        /// Set BucklingData on a Timber bar-element.
        /// </summary>
        /// <remarks>Action</remarks>
        /// <param name="bar">Bar. Timber bar-element.</param>
        /// <param name="flexuralStiff">BucklingLength definition in Flexural Stiff direction.</param>
        /// <param name="flexuralWeak">BucklingLength definition in Flexural Weak direction.</param>
        /// <param name="lateralTorsional">BucklingLength definition for Lateral Torsional Buckling.</param>
        /// <returns></returns>
        public static Bar SetOnTimberBar(Bar bar, BucklingLength flexuralStiff, BucklingLength flexuralWeak, BucklingLength lateralTorsional)
        {
            // assert input
            if (bar.material.timber == null)
            {
                throw new System.ArgumentException("Material of bar element must be timber!");
            }
            if (flexuralStiff.type != "flexural_stiff")
            {
                throw new System.ArgumentException("flexuralStiff is not of type FlexuralStiff!");
            }
            if (flexuralWeak.type != "flexural_weak")
            {
                throw new System.ArgumentException("flexuralWeak is not of type FlexuralWeak!");
            }
            if (lateralTorsional.type != "lateral_torsional")
            {
                throw new System.ArgumentException("lateralTorsional is not of type LateralTorsional!");
            }

            // add input.
            BucklingData bucklingData = new BucklingData();
            bucklingData.bucklingLength.Add(flexuralStiff);
            bucklingData.bucklingLength.Add(flexuralWeak);
            bucklingData.bucklingLength.Add(lateralTorsional);
            bar.barPart.bucklingData = bucklingData;

            return bar;
        }
    }
}