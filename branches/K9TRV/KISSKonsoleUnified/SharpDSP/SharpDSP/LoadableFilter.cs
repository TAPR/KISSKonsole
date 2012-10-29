/*
This file is part of a program that implements a Software-Defined Radio.

Copyright (C) 2007, 2008 Philip A Covington

This program is free software; you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation; either version 2 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software
Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

The author can be reached by email at

p.covington@gmail.com

*/

using System;
using System.Runtime.InteropServices;

namespace SharpDSP2._1
{
	/// <summary>
	/// Filter (Frequency Domain)
	/// </summary>
		
	public class LoadableFilter 
	{
		#region Private members
                                
        private CPX[] filter_cpx;
        private CPX[] delay_cpx;
        private int state = 0;

        #endregion
        
		#region Constructor

        public LoadableFilter(int nsize, int ntaps)
        {            
            this.filter_cpx = new CPX[ntaps];
            this.delay_cpx = new CPX[nsize * 2];
        }

        #endregion
        
        #region Public Methods		
                        
        public bool LoadFilter(CPX[] cpx_filter_coeff)
        {
            this.filter_cpx = (CPX[])cpx_filter_coeff.Clone();            
            return true;
        }
                
        internal void ClearFilter()
        {
        	Array.Clear(this.filter_cpx, 0, this.filter_cpx.Length);
        }

        internal void ClearDelayLine()
        {
            Array.Clear(this.delay_cpx, 0, this.delay_cpx.Length);
        }

        unsafe public void Process(float * real, float * imag, int nsize)
        {
            CPX input;
            CPX output;

            ClearDelayLine();

            for (int i = 0; i < nsize; i++)
            {
                input.real = real[i];
                input.imaginary = imag[i];

                output = DoFir(input, filter_cpx.Length, filter_cpx, delay_cpx);

                real[i] = output.real;
                imag[i] = output.imaginary;
            }
        }

        #endregion

        private CPX DoFir(CPX input, int ntaps, CPX[] h, CPX[] z)
        {
            CPX accum;            

            accum.real = 0f;
            accum.imaginary = 0f;

            z[state] = z[state + ntaps] = input;            

            for (int i = 0; i < ntaps; i++)
            {
                accum.real += h[i].real * z[state + i].real;
                accum.imaginary += h[i].imaginary * z[state + i].imaginary;
            }

            if (--state < 0) state += ntaps;
            
            return accum;
        }
        
    }

    public class FilterCoeffSet
    {
        private CPX[] coeff;
        private int size = 256;
        //private int size = 64;
        //private int size = 32;

        // this is the tap set
        // generated in octave
        //double[] B = new double[256] 
        //{
        //0.0001212467029651,0.0008854147044834, 0.003151945184232, 0.007035935216091,
        //0.01046498473454, 0.009836101685977, 0.003817603890758,-0.003517822311532,
        //-0.005655668588125,-0.001140184763808, 0.003869949389703, 0.003027961692355,
        //-0.001802493745036,-0.003377783158697,0.0002515305504413, 0.003080407344625,
        //0.0007737236984118,-0.002566987492918,-0.001408796216841, 0.002030058225865,
        //0.001789578513444,-0.001540407171609,-0.002016905296429, 0.001111155299667,
        //0.002151079360814,-0.0007422579010042, -0.00223716259982,0.0004186403557414,
        //0.002299879523996,-0.000123207345728,-0.002348897659174,-0.000153380066355,
        //0.002394761782962,0.0004266915650005,-0.002436779591886,-0.0007076251018686,
        //0.002467641748649,0.0009962474121068,-0.002487781285025, -0.00129655382111,
        //0.00249628097415, 0.001614722910043,-0.002486719372779,-0.001951804356173,
        //0.002454495582975, 0.002309221891398,-0.002392700263604,-0.002684699993092,
        //0.00229553621584, 0.003072222345357,-0.002164045285525,-0.003472090365127,
        //0.001996497520958, 0.003886881130574,-0.001784935548752,-0.004313000639073,
        //0.001523550324315, 0.004745068359119,-0.001208553552681,-0.005177398030176,
        //0.0008374821147734, 0.005603840108681,-0.0004110583314793,-0.006022675004727,
        //-7.293344319318e-005, 0.006433509314972,0.0006229575688115,-0.006828299084073,
        //-0.001244384053858, 0.007194473612837, 0.001933322101499,-0.007527116096461,
        //-0.002686659994465, 0.007828854343299, 0.003513554832045, -0.00809329634888,
        //-0.004424788683352, 0.008301660961528, 0.005416018631771,-0.008443453059677,
        //-0.006479813367691, 0.008520819210819, 0.007624130729755,-0.008527119134364,
        //-0.008860804690609, 0.008442263458594,  0.01018843493923,-0.008251657032524,
        //-0.01160195231658, 0.007951812789079,  0.01310998096921,-0.007532922632659,
        //-0.01472984000237, 0.006967194243049,  0.01646559038553,-0.006229499336255,
        //-0.01831633578666, 0.005308685609681,  0.02030575127302,-0.004173896436676,
        //-0.02246379264654, 0.002767684739696,  0.02479875585672,-0.001046807533514,
        //-0.0273406135319,-0.001042234340189,  0.03015754433096,  0.00361276125941,
        //-0.03330858133254,-0.006807488376724,  0.03687222503345,  0.01080915418667,
        //-0.04100640557251,  -0.0159464425125,  0.04591985218217,  0.02275125485918,
        //-0.05192050169054, -0.03211866036857,  0.05956076720188,   0.0458299269958,
        //-0.06973096897279, -0.06771888386291,  0.08395956896593,   0.1076587812334,
        //-0.1045794500644,  -0.2001127933892,   0.1254126398425,   0.5660710647242,
        //0.5660710647242,   0.1254126398425,  -0.2001127933892,  -0.1045794500644,
        //0.1076587812334,  0.08395956896593, -0.06771888386291, -0.06973096897279,
        //0.0458299269958,  0.05956076720188, -0.03211866036857, -0.05192050169054,
        //0.02275125485918,  0.04591985218217,  -0.0159464425125, -0.04100640557251,
        //0.01080915418667,  0.03687222503345,-0.006807488376724, -0.03330858133254,
        //0.00361276125941,  0.03015754433096,-0.001042234340189,  -0.0273406135319,
        //-0.001046807533514,  0.02479875585672, 0.002767684739696, -0.02246379264654,
        //-0.004173896436676,  0.02030575127302, 0.005308685609681, -0.01831633578666,
        //-0.006229499336255,  0.01646559038553, 0.006967194243049, -0.01472984000237,
        //-0.007532922632659,  0.01310998096921, 0.007951812789079, -0.01160195231658,
        //-0.008251657032524,  0.01018843493923, 0.008442263458594,-0.008860804690609,
        //-0.008527119134364, 0.007624130729755, 0.008520819210819,-0.006479813367691,
        //-0.008443453059677, 0.005416018631771, 0.008301660961528,-0.004424788683352,
        //-0.00809329634888, 0.003513554832045, 0.007828854343299,-0.002686659994465,
        //-0.007527116096461, 0.001933322101499, 0.007194473612837,-0.001244384053858,
        //-0.006828299084073,0.0006229575688115, 0.006433509314972,-7.293344319318e-005,
        //-0.006022675004727,-0.0004110583314793, 0.005603840108681,0.0008374821147734,
        //-0.005177398030176,-0.001208553552681, 0.004745068359119, 0.001523550324315,
        //-0.004313000639073,-0.001784935548752, 0.003886881130574, 0.001996497520958,
        //-0.003472090365127,-0.002164045285525, 0.003072222345357,  0.00229553621584,
        //-0.002684699993092,-0.002392700263604, 0.002309221891398, 0.002454495582975,
        //-0.001951804356173,-0.002486719372779, 0.001614722910043,  0.00249628097415,
        //-0.00129655382111,-0.002487781285025,0.0009962474121068, 0.002467641748649,
        //-0.0007076251018686,-0.002436779591886,0.0004266915650005, 0.002394761782962,
        //-0.000153380066355,-0.002348897659174,-0.000123207345728, 0.002299879523996,
        //0.0004186403557414, -0.00223716259982,-0.0007422579010042, 0.002151079360814,
        //0.001111155299667,-0.002016905296429,-0.001540407171609, 0.001789578513444,
        //0.002030058225865,-0.001408796216841,-0.002566987492918,0.0007737236984118,
        //0.003080407344625,0.0002515305504413,-0.003377783158697,-0.001802493745036,
        //0.003027961692355, 0.003869949389703,-0.001140184763808,-0.005655668588125,
        //-0.003517822311532, 0.003817603890758, 0.009836101685977,  0.01046498473454,
        //0.007035935216091, 0.003151945184232,0.0008854147044834,0.0001212467029651
        //};

        double[] B = new double[64] 
        {
        -0.0004048874117378,-0.002301261499961,-0.006431550506588, -0.01097022978185,
    -0.0112904927941, -0.00457722113049, 0.004711267473067, 0.007287648064967,
  0.0001426386635794, -0.00737692328734, -0.00422728104842, 0.006108587623475,
   0.008195576646968,-0.002993086314273, -0.01151904364988,-0.002346223882122,
    0.01301199150945, 0.009661884595477, -0.01127506740082, -0.01796324697543,
   0.005039803403436,  0.02554063178006, 0.006644033979087, -0.03003331111349,
   -0.02450737697416,  0.02825440086555,  0.04994861462765, -0.01471587804963,
   -0.08922049756348, -0.02867311736306,   0.1902349013999,   0.3972941522219,
     0.3972941522219,   0.1902349013999, -0.02867311736306, -0.08922049756348,
   -0.01471587804963,  0.04994861462765,  0.02825440086555, -0.02450737697416,
   -0.03003331111349, 0.006644033979087,  0.02554063178006, 0.005039803403436,
   -0.01796324697543, -0.01127506740082, 0.009661884595477,  0.01301199150945,
  -0.002346223882122, -0.01151904364988,-0.002993086314273, 0.008195576646968,
   0.006108587623475, -0.00422728104842, -0.00737692328734,0.0001426386635794,
   0.007287648064967, 0.004711267473067, -0.00457722113049,  -0.0112904927941,
   -0.01097022978185,-0.006431550506588,-0.002301261499961,-0.0004048874117378
        };

        //double[] B = new double[32] 
        //{
        //-0.0003422199920911,-0.0001034202524363, 0.005614958983286,  0.01430400381711,
        //0.004649666975561, -0.02091454700776,-0.007067509916099,  0.04043669245672,
        //0.008510908563843, -0.07449990912961,-0.003352610552893,   0.1329201610539,
        //-0.01451227197644,  -0.2474905749085,  0.04162663950817,   0.6346072165431,
        //0.6346072165431,  0.04162663950817,  -0.2474905749085, -0.01451227197644,
        //0.1329201610539,-0.003352610552893, -0.07449990912961, 0.008510908563843,
        //0.04043669245672,-0.007067509916099, -0.02091454700776, 0.004649666975561,
        //0.01430400381711, 0.005614958983286,-0.0001034202524363,-0.0003422199920911
        //};

        public FilterCoeffSet(int nsize)
        {
            this.size = nsize;
            coeff = new CPX[nsize];

            for (int i = 0; i < nsize; i++)
            {
                coeff[i].real = (float)B[i];
                coeff[i].imaginary = (float)B[i];
            }
        }

        public CPX[] GetFilterCoeffSet()
        {
            return coeff;
        }
    }
}
